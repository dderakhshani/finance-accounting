import {Component, ViewChild} from '@angular/core';
import {VoucherHead} from "../../../../entities/voucher-head";
import {ExtensionsService} from "../../../../../../shared/services/extensions/extensions.service";
import {Router} from "@angular/router";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {GetVoucherHeadsQuery} from "../../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {TableConfigurations} from "../../../../../../core/components/custom/table/models/table-configurations";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {SearchQuery} from 'src/app/shared/services/search/models/search-query';
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import {MatDialog} from "@angular/material/dialog";
import {LocalStorageRepository} from "../../../../../../core/services/storage/local-storage-repository.service";
import {
  GetTadbirReportQuery
} from 'src/app/modules/accounting/repositories/voucher-head/queries/get-tadbir-report-query';
import {TableComponent} from "../../../../../../core/components/custom/table/table.component";
import {IdentityService} from "../../../../../identity/repositories/identity.service";
import {DeleteVouchersHeadCommand} from "../../../../repositories/voucher-head/commands/delete-voucher-head-command";

@Component({
  selector: 'app-voucher-head-list',
  templateUrl: './voucher-head-list.component.html',
  styleUrls: ['./voucher-head-list.component.scss']
})
export class VoucherHeadListComponent extends BaseComponent {
  selectedRowIndex: number = -1;

  voucherHeads: VoucherHead[] = [];

  tableConfigurations!: TableConfigurations;


  totalDebit: number = 0;
  totalCredit: number = 0;

  onlyShowMyVouchers: boolean = false

  constructor(
    private _mediator: Mediator,
    private router: Router,
    public identityService: IdentityService
  ) {
    super();
  }

  @ViewChild(TableComponent) tableComponent!: TableComponent;

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.edit().setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Edit')),
      PreDefinedActions.add().setDisable(!this.identityService.doesHavePermission('VoucherHeadsList-Add')),
      PreDefinedActions.refresh(),

      new Action('دریافت گزارش برای تدبیر', 'print', ActionTypes.custom, 'getTadbirReport'),
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    this.toggleIsLoading();


    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),
      new TableColumn(
        'voucherNo',
        'شماره سند',
        TableColumnDataType.Number,
        '',
        true,
        new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'voucherDate',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'totalDebit',
        'بدهکار',
        TableColumnDataType.Money,
        '',
        true,
        new TableColumnFilter('totalDebit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'totalCredit',
        'بستانکار',
        TableColumnDataType.Money,
        '',
        true,
        new TableColumnFilter('totalCredit', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'difference',
        'مانده',
        TableColumnDataType.Money,
        '',
        true,
        new TableColumnFilter('difference', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherStateName',
        'وضعیت سند',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('voucherStateName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'voucherDescription',
        'شرح سند',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('voucherDescription', TableColumnFilterTypes.Text)
      ),
      // new TableColumn(
      //   'voucherDailyId',
      //   'شماره روزانه',
      //   TableColumnDataType.Number,
      //   '',
      //   true,
      //   new TableColumnFilter('voucherDailyId',TableColumnFilterTypes.Number)
      // ),
      new TableColumn(
        'traceNumber',
        'شماره پیگیری',
        TableColumnDataType.Number,
        '',
        true,
        new TableColumnFilter('traceNumber', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'creator',
        'صادر کننده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('creator', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'modifier',
        'آخرین تغیید دهنده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('modifier', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'attachments',
        'پیوست / یادداشت',
        TableColumnDataType.Text,
        '',
        true,
        undefined,
        (voucherHead: VoucherHead) => {
          return voucherHead.attachments ? 'دارد' : 'ندارد';
        }
      ),

    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.sumLabel = 'جمع کل';


    await this.get();
    this.toggleIsLoading();

  }

  initialize(): any {
  }


  async get(id?: number) {
    this.voucherHeads = [];
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })

    }
    if (this.onlyShowMyVouchers) {
      searchQueries.push(
        {
          comparison: 'equals',
          propertyName: 'createdById',
          values: [this.identityService.applicationUser.id],
          nextOperand: 'or'
        }
      )
      searchQueries.push({
        comparison: 'equals',
        propertyName: 'modifiedById',
        values: [this.identityService.applicationUser.id],
        nextOperand: 'or'
      })
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    if (!orderByProperty) orderByProperty = 'voucherDate DESC'
    let request = new GetVoucherHeadsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.voucherHeads = response.data;
    this.totalCredit = response.totalCredit;
    this.totalDebit = response.totalDebit;
    if (response.totalCount) this.tableConfigurations.pagination.totalItems = response.totalCount;
  }

  async handleCustomActions(action: Action) {
    if (action.uniqueName === 'getTadbirReport') {
      this.getTadbirReport();
    }
  }

  async getTadbirReport() {
    this.isLoading = true;

    let query = new GetTadbirReportQuery();
    // @ts-ignore
    query.VoucherHeadIds = this.voucherHeads.filter(x => x.selected).map(x => x.id);

    let response = await this._mediator.send(query).catch(() => {
      this.isLoading = false;
    });
    // @ts-ignore
    this.convertToExcel(response);
    this.isLoading = false;
  }

  async convertToExcel(result: any) {
    var binaryData = atob(result);

    const blob = new Blob([new Uint8Array(binaryData.length).map((_, index) => binaryData.charCodeAt(index))], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'گزارش برای تدبیر.xlsx';
    a.click();

    window.URL.revokeObjectURL(url);

  }

  async navigateToVoucherHead(voucherHead: VoucherHead) {
    await this.router.navigateByUrl(`/accounting/voucherHead/add?id=${voucherHead.id}`)
  }


  async add() {
    await this.router.navigateByUrl('/accounting/voucherHead/add')
  }

  close(): any {
  }

  async delete() {
    try {
      // @ts-ignore
      this.voucherHeads.filter(x => x.selected).forEach(async (voucher) => {
        await this._mediator.send(new DeleteVouchersHeadCommand(voucher.id))
        await this.get()
      })
    } finally {
    }
  }


  async update() {
    // @ts-ignore
    let entity = this.voucherHeads.filter(x => x.selected)[0];
    await this.router.navigateByUrl(`/accounting/voucherHead/add?id=${entity.id}`);
  }

  protected readonly onload = onload;
}
