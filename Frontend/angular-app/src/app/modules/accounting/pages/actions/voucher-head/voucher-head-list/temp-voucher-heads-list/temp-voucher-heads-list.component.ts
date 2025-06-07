import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableConfigurations} from "../../../../../../../core/components/custom/table/models/table-configurations";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {VoucherHead} from "../../../../../entities/voucher-head";
import {LocalStorageRepository} from "../../../../../../../core/services/storage/local-storage-repository.service";
import {
  CreateVouchersHeadCommand
} from "../../../../../repositories/voucher-head/commands/create-vouchers-head-command";
import {Router} from "@angular/router";
import {PreDefinedActions} from "../../../../../../../core/components/custom/action-bar/action-bar.component";

@Component({
  selector: 'app-temp-voucher-heads-list',
  templateUrl: './temp-voucher-heads-list.component.html',
  styleUrls: ['./temp-voucher-heads-list.component.scss']
})
export class TempVoucherHeadsListComponent extends BaseComponent {
  tempVoucherTableConfigurations!:TableConfigurations;
  tempVoucherHeads: CreateVouchersHeadCommand[] = [];

  constructor(
    private localStorageRepository:LocalStorageRepository,
    private router:Router,


  ) {
    super()
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add(),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),

    ]
  }
  ngOnInit(): void {
    this.resolve()
  }

  resolve(params?: any): any {

    let tempVouchersTableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index','ردیف',TableColumnDataType.Index,''),
      new TableColumn(
        'voucherNo',
        'شماره سند',
        TableColumnDataType.Number,
        '',
        true,
        new TableColumnFilter('voucherNo',TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('voucherDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'voucherDescription',
        'شرح سند',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('voucherDescription',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'creatorFirstName',
        'صادر کننده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('creator',TableColumnFilterTypes.Text),

      ),
    ]
    this.tempVoucherTableConfigurations = new TableConfigurations(tempVouchersTableColumns,new TableOptions(false,true, undefined, true))
    this.get();
  }

  get() {

    let tempVouchers = JSON.parse(this.localStorageRepository.get('tempVoucherHeads') ?? '[]');
    if (tempVouchers) this.tempVoucherHeads = tempVouchers

  }

  async navigateToTempVoucherHead(voucherHead:CreateVouchersHeadCommand){
    await this.router.navigateByUrl(`/accounting/voucherHead/add?tempId=${voucherHead.requestId}`)
  }

  async add() {
    await this.router.navigateByUrl('/accounting/voucherHead/add')
  }

  close(): any {
  }

  delete(param?: any): any {
    // @ts-ignore
    let selectedTempVouchersToRemove = this.tempVoucherHeads.filter(x => x.selected === true)

    let tempVouchers = JSON.parse(this.localStorageRepository.get('tempVoucherHeads') ?? '[]')
    selectedTempVouchersToRemove?.forEach((x) => {
      let tempVoucher = tempVouchers.find((x:CreateVouchersHeadCommand) => x.requestId === x.requestId)
      tempVouchers.splice(tempVouchers.indexOf(tempVoucher), 1)
    })

    this.localStorageRepository.update('tempVoucherHeads', JSON.stringify(tempVouchers,(key : any, value:any) => typeof value === 'undefined' ? null : value))

    this.get()
  }

  initialize(params?: any): any {
  }


  update(param?: any): any {
  }
}
