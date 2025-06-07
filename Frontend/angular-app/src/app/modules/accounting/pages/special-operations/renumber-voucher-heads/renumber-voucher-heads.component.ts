import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {RenumberVoucherHeadsCommand} from "../../../repositories/voucher-head/commands/renumber-voucher-heads-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../core/enums/page-modes";


import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {VoucherHead} from "../../../entities/voucher-head";
import {GetVoucherHeadsBySpecificQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-by-specific";
import {UserYear} from "../../../../identity/repositories/models/user-year";
import {Router} from "@angular/router";
import {forkJoin} from "rxjs";
import {
  GetCodeVoucherGroupsQuery
} from "../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-renumber-voucher-heads',
  templateUrl: './renumber-voucher-heads.component.html',
  styleUrls: ['./renumber-voucher-heads.component.scss']
})
export class RenumberVoucherHeadsComponent extends BaseComponent {
  codeVoucherGroups: CodeVoucherGroup[] = [];
  vouchers: VoucherHead[] = [];
  selectedVouchers: VoucherHead[] = [];
  tableConfiguration!: TableConfigurations;
  selectedVouchersTableConfiguration!: TableConfigurations;
  selectedYear?: UserYear;


  constructor(
    private _mediator: Mediator,
    private router: Router,
  ) {
    super();
    this.pageMode = PageModes.Update;
    this.request = new RenumberVoucherHeadsCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {


    let selectedVouchersTableColumns: TableColumn[] = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'voucherNo',
        'شماره سند',
        TableColumnDataType.Number,
        '5%',
        true,
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '5%',
        true,
      ),
      new TableColumn(
        'totalDebit',
        'بدهکار',
        TableColumnDataType.Number,
        '5%',
        true,
      ),
      new TableColumn(
        'totalCredit',
        'بستانکار',
        TableColumnDataType.Number,
        '5%',
        true,
      ),
      new TableColumn(
        'difference',
        'مانده',
        TableColumnDataType.Number,
        '5%',
        true,
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '5%',
        true,
      ),
      new TableColumn(
        'voucherStateName',
        'وضعیت سند',
        TableColumnDataType.Text,
        '5%',
        true,
      ),
      new TableColumn(
        'voucherDescription',
        'شرح سند',
        TableColumnDataType.Text,
        '5%',
        true,
      ),
      new TableColumn(
        'voucherDailyId',
        'شماره روزانه',
        TableColumnDataType.Number,
        '5%',
        true,
      ),
      new TableColumn(
        'traceNo',
        'شماره پیگیری',
        TableColumnDataType.Text,
        '5%',
        true,
      ),
      new TableColumn(
        'creator',
        'صادر کننده',
        TableColumnDataType.Text,
        '5%',
        true,
        undefined,

      ),
      new TableColumn(
        'modifier',
        'آخرین تغیید دهنده',
        TableColumnDataType.Text,
        '5%',
        true,
        undefined,
      ),
      new TableColumn(
        'attachments',
        'پیوست / یادداشت',
        TableColumnDataType.Text,
        '5%',
        true,
        undefined,
        (voucherHead: VoucherHead) => {
          return voucherHead.attachments ? 'دارد' : 'ندارد';
        }
      ),
    ];
    this.selectedVouchersTableConfiguration = new TableConfigurations(selectedVouchersTableColumns, new TableOptions(false, true));

    forkJoin([
      this._mediator.send(new GetCodeVoucherGroupsQuery()),
      this.get()
    ]).subscribe(([
      codeVoucherGroups,
      report
                      ]) => {
      this.codeVoucherGroups = codeVoucherGroups.data
    })

  }

  initialize(): any {
  }

  add(): any {
  }

  async get(id?: number) {
    this.selectedVouchersTableConfiguration.pagination.pageIndex = 0;


    let orderByProperty = '';
    if (this.selectedVouchersTableConfiguration.sortKeys) {
      this.selectedVouchersTableConfiguration.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetVoucherHeadsBySpecificQuery(
      this.selectedVouchersTableConfiguration.pagination.pageIndex,
      this.selectedVouchersTableConfiguration.pagination.pageSize,
      undefined,
      orderByProperty
    );
    request.fromDateTime = (<RenumberVoucherHeadsCommand>this.request).fromDateTime;
    request.toDateTime = (<RenumberVoucherHeadsCommand>this.request).toDateTime;
    request.fromNo = (<RenumberVoucherHeadsCommand>this.request).fromNo;
    request.toNo = (<RenumberVoucherHeadsCommand>this.request).toNo;
    request.voucherStateId = (<RenumberVoucherHeadsCommand>this.request).voucherStateId;
    request.codeVoucherGroupId = (<RenumberVoucherHeadsCommand>this.request).codeVoucherGroupId;

    let response = await this._mediator.send(request);
    this.selectedVouchers = response.data;
    return this.selectedVouchers;
  }

  async update() {
    let response = await this._mediator.send(this.request);
  }

  async navigateToVoucherHead(voucherHead: VoucherHead) {
    await this.router.navigateByUrl(`/accounting/voucherHead/add?id=${voucherHead.id}`)
  }

  delete(): any {
  }

  close(): any {
  }

}
