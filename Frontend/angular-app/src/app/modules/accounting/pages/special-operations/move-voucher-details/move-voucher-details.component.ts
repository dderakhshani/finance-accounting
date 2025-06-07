import {Component, Inject} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {FormControl, FormGroup} from "@angular/forms";
import {VoucherDetail} from "../../../entities/voucher-detail";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {AccountManagerService} from "../../../services/account-manager.service";
import {VoucherHead} from "../../../entities/voucher-head";
import {GetVoucherHeadsQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {MoveVoucherDetailsCommand} from "../../../repositories/voucher-head/commands/move-voucher-details-command";
import {number} from "echarts";

@Component({
  selector: 'app-move-voucher-details',
  templateUrl: './move-voucher-details.component.html',
  styleUrls: ['./move-voucher-details.component.scss']
})
export class MoveVoucherDetailsComponent extends BaseComponent {

  public entities: VoucherDetail[] = []
  voucherHeads: VoucherHead[] = []
  tableConfigurations!: TableConfigurations;
  destinationVoucherHeadIdFormControl = new FormControl()

  voucherHeadDisplayFn(voucherHeadId: number) {
    let voucherHead = this.voucherHeads.find(x => x.id === voucherHeadId)
    return voucherHead?.voucherNo
  };

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<MoveVoucherDetailsComponent>,
    private accountManagerService: AccountManagerService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super()
    this.entities = data
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    await this.getVoucherHeads().then(() => {
      this.configTable()
      this.destinationVoucherHeadIdFormControl.valueChanges.subscribe(async (next) => {
        if (!this.voucherHeads.map(x => x.id).includes(next)) await this.getVoucherHeads(next)
      })
    })

  }

  async getVoucherHeads(searchParam?: any) {
    let query = new GetVoucherHeadsQuery()
    query.orderByProperty = 'voucherNo DESC'
    query.conditions = []


    if (searchParam && typeof (+searchParam) === "number")
      query.conditions.push(
        new SearchQuery({
          propertyName: 'voucherNo',
          comparison: 'equals',
          values: [+searchParam],
          nextOperand: 'and'
        }));

    query.conditions?.push(new SearchQuery({
      propertyName: 'id',
      comparison: 'notEqual',
      values: [this.entities[0]?.id]
    }));

    await this._mediator.send(query).then(res => {
      this.voucherHeads = res.data;
    })
  }

  configTable() {
    let columns = [
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%',
      },

      new TableColumn(
        'referenceId1',
        'تفصیل شناور',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('referenceId1', TableColumnFilterTypes.Text, true),
        (voucherDetail: VoucherDetail) => {
          let accountReference = this.accountManagerService.accountReferences.value?.find(x => x.id === voucherDetail.referenceId1);
          return accountReference ? accountReference.code : ''
        }
      ),
      new TableColumn(
        'accountReferencesGroupId',
        'گروه تفصیل',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('accountReferencesGroupId', TableColumnFilterTypes.Text, true),
        (voucherDetail: VoucherDetail) => {
          let accountReferencesGroup = this.accountManagerService.accountReferenceGroups.value?.find(x => x.id === voucherDetail.accountReferencesGroupId);
          return accountReferencesGroup ? accountReferencesGroup.code : ''
        },
      ),
      new TableColumn(
        'accountHeadId',
        'کد حساب',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('accountHeadId', TableColumnFilterTypes.Text, true),
        (voucherDetail: VoucherDetail) => {
          let selectedAccountHead = this.accountManagerService.accountHeads.value?.find(x => x.id === voucherDetail.accountHeadId);
          return selectedAccountHead ? selectedAccountHead?.fullCode : '';
        }
      ),


      new TableColumn(
        'voucherRowDescription',
        'شرح',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('voucherRowDescription', TableColumnFilterTypes.Text),
        (voucherDetail: any) => {
          return voucherDetail?.voucherRowDescription ?? (voucherDetail ?? "");
        }
      ),

      new TableColumn(
        'debit',
        'بدهکار',
        TableColumnDataType.Money,
        '7.5%',
        true,
        new TableColumnFilter('debit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'credit',
        'بستانکار',
        TableColumnDataType.Money,
        '7.5%',
        true,
        new TableColumnFilter('credit', TableColumnFilterTypes.Money)
      )
    ]

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true));
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'rowIndex';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';
    this.tableConfigurations.options.showSumRow = true;
  }

  async moveVoucherDetails() {
    let request = new MoveVoucherDetailsCommand()
    request.voucherHeadId = this.destinationVoucherHeadIdFormControl.value;
    request.voucherDetailIds = this.entities.map(x => x.id)
    await this._mediator.send(request).then(res => {
      this.dialogRef.close(true)
    })
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }


  update(param?: any): any {
  }


}
