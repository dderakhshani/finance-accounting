import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {Year} from "../../../../admin/entities/year";
import {VoucherHead} from "../../../entities/voucher-head";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../core/enums/page-modes";
import {
  InsertBetweenVouchersHeadCommand
} from "../../../repositories/voucher-head/commands/insert-between-vouchers-head-command";
import {GetVoucherHeadsBySpecificQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-by-specific";
import {Router} from "@angular/router";
import {
  GetCodeVoucherGroupsQuery
} from "../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {forkJoin} from "rxjs";
import {GetYearsQuery} from "../../../../admin/repositories/year/queris/get-years-query";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-insert-between-voucher-heads',
  templateUrl: './insert-between-voucher-heads.component.html',
  styleUrls: ['./insert-between-voucher-heads.component.scss']
})
export class InsertBetweenVoucherHeadsComponent extends BaseComponent {

  codeVoucherGroups: CodeVoucherGroup[] = [];
  years: Year[] = [];
  vouchers: VoucherHead[] = [];
  tableConfiguration!: TableConfigurations;
  selectedVouchersTableConfiguration!: TableConfigurations;
  selectedVouchers: VoucherHead[] = [];

  // get availableMonths() {
  //   return
  // }

  constructor(
    private _mediator: Mediator,
    private router: Router,
  ) {
    super();
    this.pageMode = PageModes.Update;
    this.request = new InsertBetweenVouchersHeadCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    forkJoin([
      this._mediator.send(new GetCodeVoucherGroupsQuery()),
      this._mediator.send(new GetYearsQuery())
    ]).subscribe(([
      codeVoucherGroups,
      years
                      ]) => {
      this.codeVoucherGroups = codeVoucherGroups.data;
      this.years = years.data
    })


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
    await this.get();
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

    request.fromNo = (<InsertBetweenVouchersHeadCommand>this.request).fromNo;
    request.toNo = (<InsertBetweenVouchersHeadCommand>this.request).toNo;


    let response = await this._mediator.send(request);

    this.selectedVouchers = response.data;
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
