import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {AccountManagerService} from "../../../services/account-manager.service";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {VoucherDetailsReportQuery} from "../../../repositories/reporting/queries/voucher-details-report-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {VoucherDetailReportResultModel} from "../../../entities/voucher-detail-report-result-model";
import {MoneyPipe} from "../../../../../core/pipes/money.pipe";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {
  GetCodeRowDescriptionsQuery
} from "../../../repositories/code-row-description/queries/code-row-descriptions-query";
import {ToPersianDatePipe} from "../../../../../core/pipes/to-persian-date.pipe";

@Component({
  selector: 'app-voucher-details-report',
  templateUrl: './voucher-details-report.component.html',
  styleUrls: ['./voucher-details-report.component.scss']
})
export class VoucherDetailsReportComponent extends BaseComponent {

  tableConfigurations!: TableConfigurations;
  reportData!: []


  showLastLevelDetails = true;

  groupingKeys: any[] = [];

  groupingKeyOptions = [
    {
      title: 'تفصیل شناور',
      value: 'AccountReferenceId'
    },
    {
      title: 'گروه تفصیل',
      value: 'AccountReferenceGroupId'
    },
    {
      title: 'گروه',
      value: 'Level1'
    },
    {
      title: 'کل',
      value: 'Level2'
    },
    {
      title: 'معین',
      value: 'Level3'
    },
    {
      title: 'تاریخ',
      value: 'Date'
    },
  ]

  constructor(private identityService: IdentityService,
              private accountManagerService: AccountManagerService,
              private mediator: Mediator
  ) {
    super()
    this.request = new VoucherDetailsReportQuery();
  }

  async ngOnInit() {
    await this.resolve()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.refresh()
    ]
  }


  async resolve() {
    let columns: TableColumn[] = [
      // <TableColumn>{
      //   name: 'selected',
      //   title: '',
      //   type: TableColumnDataType.Select,
      //   width: '2.5%',
      // },
      // <TableColumn>{
      //   name: 'index',
      //   title: 'ردیف',
      //   type: TableColumnDataType.Index,
      //   width: '2.5%'
      // },
      <TableColumn>{
        name: 'code',
        title: 'کد',
        type: TableColumnDataType.Text,
        width: '15%',
        displayFn: (reportItem: VoucherDetailReportResultModel) => {
          if(reportItem.title?.toLowerCase() === 'AccountReferenceId'.toLowerCase()) {
            return this.accountManagerService.accountReferenceDisplayFn(+reportItem.code)
          }
          if(reportItem.title?.toLowerCase() === 'AccountReferenceGroupId'.toLowerCase()) {
            return this.accountManagerService.accountReferenceGroupDisplayFn(+reportItem.code)
          }
          if(reportItem.title?.toLowerCase() === 'Date'.toLowerCase()) {
            return new ToPersianDatePipe().transform(reportItem.code)
          }
          if(reportItem.title?.toLowerCase() === 'Level1'.toLowerCase()) {
            return this.accountManagerService.accountHeadDisplayFn(+reportItem.code)
          }
          if(reportItem.title?.toLowerCase() === 'Level2'.toLowerCase()) {
            return this.accountManagerService.accountHeadDisplayFn(+reportItem.code)
          }
          if(reportItem.title?.toLowerCase() === 'Level3'.toLowerCase()) {
            return this.accountManagerService.accountHeadDisplayFn(+reportItem.code)
          }
          return '';
        }
      },
      <TableColumn>{
        name: 'voucherNumber',
        title: 'سند',
        type: TableColumnDataType.Number,
        width: '2.5%',
        filter: new TableColumnFilter('voucherNumber', TableColumnFilterTypes.Number),
        sortable: true
      },
      <TableColumn>{
        name: 'date',
        title: 'تاریخ',
        type: TableColumnDataType.Date,
        width: '5%',
        filter: new TableColumnFilter('date', TableColumnFilterTypes.Date),
        sortable: true,
        lineStyle: 'onlyShowFirstLine'
      },
      <TableColumn>{
        name: 'accountHead',
        title: 'حساب',
        type: TableColumnDataType.Text,
        width: '10%',
        filter: new TableColumnFilter('accountHead', TableColumnFilterTypes.Text),
        sortable: true,
        lineStyle: 'onlyShowFirstLine'
      },

      <TableColumn>{
        name: 'accountReferenceGroup',
        title: 'گروه تفصیل',
        type: TableColumnDataType.Text,
        width: '10%',
        filter: new TableColumnFilter('accountReferenceGroup', TableColumnFilterTypes.Text),
        sortable: true,
        lineStyle: 'onlyShowFirstLine'
      },
      <TableColumn>{
        name: 'accountReference',
        title: 'تفصیل شناور',
        type: TableColumnDataType.Text,
        width: '10%',
        filter: new TableColumnFilter('accountReference', TableColumnFilterTypes.Text),
        sortable: true,
        lineStyle: 'onlyShowFirstLine'
      },

      <TableColumn>{
        name: 'description',
        title: 'شرح',
        type: TableColumnDataType.Text,
        width: '20%',
        filter: new TableColumnFilter('description', TableColumnFilterTypes.Text),
        sortable: true,
        lineStyle: 'onlyShowFirstLine'
      },
      <TableColumn>{
        name: 'debit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
        sortable: true
      },
      <TableColumn>{
        name: 'credit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
        sortable: true
      },
      <TableColumn>{
        name: 'remain',
        title: 'مانده',
        type: TableColumnDataType.Text,
        width: '10%',
        filter: new TableColumnFilter('remain', TableColumnFilterTypes.Text),
        sortable: true,
        showSumRow: true,
        // sumRowDisplayFn: () => {
        //
        //   return new MoneyPipe().transform(this.remaining.value)
        // }
        displayFn: (reportItem: VoucherDetailReportResultModel) => {
          if (reportItem.id) {
            reportItem.remain > 0 ? new MoneyPipe().transform(reportItem.remain) : (reportItem.remain < 0 ? "(" + new MoneyPipe().transform(reportItem.remain * -1) + ")" : '')
          }
          let remain: any = reportItem.credit - reportItem.debit;
          if (remain >= 0) return new MoneyPipe().transform(remain)
          return "(" + new MoneyPipe().transform(remain * -1) + ")"
        }
      },
      <TableColumn>{
        name: 'currencyDebit',
        title: 'بدهکار ارز',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('currencyDebit', TableColumnFilterTypes.Money),
        sortable: true,
        show: false
      },
      <TableColumn>{
        name: 'currencyCredit',
        title: 'بستانکار ارز',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('currencyCredit', TableColumnFilterTypes.Money),
        sortable: true,
        show: false
      },
      <TableColumn>{
        name: 'currencyRemain',
        title: 'مانده ارز',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('currencyRemain', TableColumnFilterTypes.Money),
        sortable: true,
        show: false,
        showSumRow: true,
        // sumRowDisplayFn: () => {
        //
        //   return new MoneyPipe().transform(this.currencyRemain.value)
        // }
      },
      <TableColumn>{
        name: 'currencyFee',
        title: 'نرخ تبدیل ارز',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('currencyFee', TableColumnFilterTypes.Money),
        sortable: true,
        show: false,
        showSumRow: false
      },
      <TableColumn>{
        name: 'currencyTypeBaseTitle',
        title: 'نوع ارز',
        type: TableColumnDataType.Text,
        width: '2.5%',
        filter: new TableColumnFilter('currencyTypeBaseTitle', TableColumnFilterTypes.Text),
        sortable: true,
        show: false
      },
    ]
    let tableOptions = new TableOptions();
    tableOptions.usePagination = true;
    this.tableConfigurations = new TableConfigurations(columns, tableOptions)

    return await this.get()
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  async get(param?: any) {
    this.isLoading = true;
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

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    if (!orderByProperty) orderByProperty = 'date ASC, voucherNumber ASC, rowIndex ASC'

    let request = new VoucherDetailsReportQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty);
    // @ts-ignore
    request.groupingKeys = this.groupingKeys.map(x => x.value);

    let userActiveYear = this.identityService.applicationUser.years.find(x => x.id == this.identityService.applicationUser.yearId);
    request.fromDate = userActiveYear?.firstDate
    request.toDate = userActiveYear?.lastDate

    request.showLastLevelDetails = this.showLastLevelDetails;

    return await this.mediator.send(request).then(res => {
      this.reportData = res.data
      this.tableConfigurations.pagination.totalItems = res.totalCount

    }).finally(() => {
      this.isLoading = false
    })
  }

  update(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }


}
