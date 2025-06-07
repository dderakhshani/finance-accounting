import {Component, TemplateRef, ViewChild} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetLedgerReportQuery} from "../../../repositories/reporting/queries/get-ledger-report-query";
import {ActivatedRoute, Router} from "@angular/router";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";

import {VoucherDetail} from "../../../entities/voucher-detail";
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {distinct} from 'rxjs/operators';
import {forkJoin, from} from 'rxjs';
import {IdentityService} from 'src/app/modules/identity/repositories/identity.service';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';

import {AccountHead} from '../../../entities/account-head';
import {FormControl} from '@angular/forms';
import {AccountReference} from '../../../entities/account-reference';
import {AccountReferencesGroup} from '../../../entities/account-references-group';
import {CodeVoucherGroup} from '../../../entities/code-voucher-group';

import {TableOptions} from 'src/app/core/components/custom/table/models/table-options';
import {SearchQuery} from 'src/app/shared/services/search/models/search-query';
import {GetAccountHeadsQuery} from '../../../repositories/account-head/queries/get-account-heads-query';
import {GetAccountReferencesQuery} from '../../../repositories/account-reference/queries/get-account-references-query';
import {
  GetAccountReferencesGroupsQuery
} from '../../../repositories/account-reference-group/queries/get-account-references-groups-query';
import {
  GetCodeVoucherGroupsQuery
} from '../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query';
import {ReportFormatTypes} from '../../../repositories/reporting/ReportFormatTypes';
import {BaseValue} from 'src/app/modules/admin/entities/base-value';
import {
  GetBaseValuesByUniqueNameQuery
} from 'src/app/modules/admin/repositories/base-value/queries/get-base-values-by-unique-name-query';


import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";

import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";

import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  LedgerReportAdvancedFilterComponent
} from "./ledger-report-advanced-filter/ledger-report-advanced-filter.component";

import {Toastr_Service} from "../../../../../shared/services/toastrService/toastr_.service";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {SSRSReportDialogComponent} from "../../../components/ssrsreport-dialog/ssrsreport-dialog.component";
import {UserYear} from "../../../../identity/repositories/models/user-year";
import {SSRSReportService} from "../../../../../shared/services/SSRSReport/ssrsreport.service";
import {UpdateTaxpayerFlagCommand} from "../../../repositories/voucher-head/commands/update-taxpayer-flag-command";
import {TaxpayerFlagUpdateModel} from "../../../entities/TaxpayerFlag";
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {BaseTable} from "../../../../../core/abstraction/base-table";
import {CustomDecimalPipe} from "../../../../../core/components/custom/table/table-details/pipe/custom-decimal.pipe";
import {AccountManagerService} from "../../../services/account-manager.service";
import {ArrayFilterPipe} from "../../../../../core/pipes/arrayFilter.pipe";
import {de} from "date-fns/locale";

export interface TaxType {
  id: number;
  label: string;
  value: boolean | null;
}

@Component({
  selector: 'app-ledger-report2',
  templateUrl: './ledger-report2.component.html',
  styleUrls: ['./ledger-report2.component.scss']
})
export class LedgerReport2Component extends BaseTable {

  @ViewChild('expandRowWithTemplate', {read: TemplateRef}) expandRowWithTemplate!: TemplateRef<any>;
  @ViewChild('updateTaxpayerFlag', {read: TemplateRef}) updateTaxpayerFlag!: TemplateRef<any>;
  columns: Column[] = [];
  applicationUserFullName!: string;
  allowedYears: UserYear[] = [];
  currentYear!: UserYear;
  selectedTabIndex: number = 0;

  reportResult: any[] = []
  accountLevels: any[] = []
  voucherStates: any[] = []
  accountTypes: any[] = []
  reportTypes: any[] = []

  //userAllowedYears: UserYear[] = [];

  accountHeads: AccountHead[] = [];
  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();

  accountReferences: AccountReference[] = [];
  selectedAccountReferences: AccountReference[] = [];
  typeListTax: TaxType[] = [
    {
      id: 1,
      label: 'اعلام شده',
      value: true
    },
    {
      id: 2,
      label: 'اعلام نشده',
      value: false
    },
    {
      id: 3,
      label: 'همه',
      value: null

    }
  ]
  accountReferenceControl = new FormControl();

  accountReferencesGroups: AccountReferencesGroup[] = [];
  selectedAccountReferencesGroups: AccountReferencesGroup[] = [];
  accountReferencesGroupControl = new FormControl();

  codeVoucherGroups: CodeVoucherGroup[] = [];
  selectedCodeVoucherGroups: CodeVoucherGroup[] = [];
  codeVoucherGroupControl = new FormControl();


  totalDebit = new FormControl();
  totalCredit = new FormControl();
  totalCurrencyCredit = new FormControl();
  totalCurrencyDebit = new FormControl();
  remaining = new FormControl();
  currencyRemain = new FormControl();

  isPrinting: boolean = false;
  showCurrencyFields: boolean = false;
  currencyTypes: BaseValue[] = [];
  includedRows: any = [];
  selectedItemsFilterForPrint: any = [new SearchQuery({
    propertyName: 'id',
    values: [],
    comparison: 'in',
    nextOperand: 'and'
  })];
  CountSelectedItemsFilterForPrint: number = 0;
  excludedRows: any = [];
  //
  changeTaxpayerFlagList: TaxpayerFlagUpdateModel[] = [];

//  ClickAdvancedFilter
  countAdvancedFilter = 0;

  constructor(
    private mediator: Mediator,
    public dialog: MatDialog,
    private router: Router,
    private toastr: Toastr_Service,
    private customDecimal: CustomDecimalPipe,
    private route: ActivatedRoute, public Service: PagesCommonService, public identityService: IdentityService,
    private SSRSReportService: SSRSReportService,
    private arrayFilterPipe: ArrayFilterPipe,
    protected accountManagerService: AccountManagerService,
  ) {

    super(route, router);

    this.request = new GetLedgerReportQuery();
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
        this.allowedYears = res.years;
        this.currentYear = <UserYear>res.years.find(x => x.id == res.yearId)
      }
    });
  }


  async ngOnInit() {
    await this.resolve();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.edit().setTitle('اعلام شده').setShow(this.identityService.doesHavePermission('TaxpayerFlag')),
    ];

    const columnExpand = this.tableConfigurations.columns.find((col: any) => col.field === 'expand');
    if (columnExpand) {
      columnExpand.expandRowWithTemplate = this.expandRowWithTemplate;
    }
    const columnTaxpayerFlag = this.tableConfigurations.columns.find((col: any) => col.field === 'taxpayerFlag');
    if (columnTaxpayerFlag) {
      columnTaxpayerFlag.template = this.updateTaxpayerFlag;
    }

  }

  async resolve(params?: any) {
    this.isLoading = true;

    this.accountLevels = [
      {
        title: 'گروه',
        id: 1
      },
      {
        title: 'کل',
        id: 2
      },
      {
        title: 'معین',
        id: 3
      },
      {
        title: 'تفصیل',
        id: 4
      },
    ]
    this.voucherStates = [
      {
        title: 'موقت',
        id: 1
      },
      {
        title: 'مرور',
        id: 2
      },
      {
        title: 'دائم',
        id: 3
      },
    ];
    this.accountTypes = [
      {
        title: 'موقت',
        id: 1
      },
      {
        title: 'دائم',
        id: 2
      },
    ]
    this.reportTypes = [
      {
        title: 'چهار ستونی',
        id: 4
      },
      {
        title: 'شش ستونی',
        id: 6
      },
      {
        title: 'هشت ستونی',
        id: 8
      }
    ]
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 2,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,


      },

      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 4,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',


      },

      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'documentNo',
        title: 'ش. سند مرتبط',
        width: 6,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
        filter: new TableColumnFilter('documentNo', TableColumnFilterTypes.Number),
        sumColumnValue: 0,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,


      },
      {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'voucherNo',
        title: 'شماره سند',
        width: 5,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
        filter: new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number),
        sumColumnValue: 0,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'voucherDate',
        title: 'تاریخ سند',
        width: 5,
        type: TableColumnDataType.Date,
        filter: new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.Date,

      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'accountHeadCode',
        title: 'کد حساب',
        width: 5,
        type: TableColumnDataType.Text,

        filter: new TableColumnFilter('accountHeadCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.accountManagerService.accountHeads.value,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'fullCode',
        optionsTitleKey: ['fullCode', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            const accountHeads = this.arrayFilterPipe.transform(this.accountManagerService.accountHeads.value, query, ['title', 'fullCode']);
            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'accountHeadCode');
            if (column) {
              column.filteredOptions = accountHeads;
            }
            return accountHeads;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },


      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'title',
        title: 'عنوان حساب',
        width: 9,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
        lineStyle: 'default',

        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.accountManagerService.accountHeads.value,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['fullCode', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            // const accountHeads = await this.getAccountHeads(query);
            const accountHeads = this.arrayFilterPipe.transform(this.accountManagerService.accountHeads.value, query, ['title', 'fullCode']);
            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'title');
            if (column) {
              column.filteredOptions = accountHeads;
            }
            return accountHeads;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'referenceCode_1',
        title: 'کد',
        width: 4,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('referenceCode_1', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.accountManagerService.accountReferences.value,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'code',
        optionsTitleKey: ['code', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            const accountReferences = this.arrayFilterPipe.transform(this.accountManagerService.accountReferences.value, query, ['code', 'title']);
            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'referenceCode_1');
            if (column) {
              column.filteredOptions = accountReferences;
            }
            return accountReferences;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'referenceName_1',
        title: 'عنوان تفضیل',
        width: 14,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('referenceName_1', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.accountManagerService.accountReferences.value,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['code', 'title'],
        filterOptionsFn: async (query: string) => {
          try {
            const accountReferences = this.arrayFilterPipe.transform(this.accountManagerService.accountReferences.value, query, ['code', 'title']);

            const column = this.tableConfigurations.columns.find((col: any) => col.field === 'referenceName_1');
            if (column) {
              column.filteredOptions = accountReferences;
            }
            return accountReferences;
          } catch (error) {
            console.error('Error fetching account references groups:', error);
            return [];
          }
        },

      },
      {
        ...this.defaultColumnSettings,
        index: 9,
        field: 'voucherRowDescription',
        title: 'شرح آرتیکل',
        width: 14,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('voucherRowDescription', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 10,
        field: 'taxpayerFlag',
        title: 'مودیان',
        width: 5,
        type: TableColumnDataType.Template,
        filter: new TableColumnFilter('typeTax', TableColumnFilterTypes.CheckBox, false, this.typeListTax),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.typeListTax,
        optionsValueKey: 'value',
        optionsSelectTitleKey: 'label',
        optionsTitleKey: ['label'],
        template: this.updateTaxpayerFlag,
        displayPrintFun: (value: any) => {
          return value ? 'اعلام شده' : 'اعلام نشده';
        }


      },
      {
        ...this.defaultColumnSettings,
        index: 11,
        field: 'debit',
        title: 'بدهکار',
        width: 7,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        showSum: true,
        sumColumnValue: 0,
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,


      },
      {
        ...this.defaultColumnSettings,
        index: 12,
        field: 'credit',
        title: 'بستانکار',
        width: 6,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        showSum: true,
        sumColumnValue: 0,
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,


      },
      {
        ...this.defaultColumnSettings,
        index: 13,
        field: 'remaining',
        title: 'مانده',
        width: 7,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        showSum: true,
        sumColumnValue: 0,
        sumRowDisplayFn: () => {
          const creditColumn = this.columns.find(col => col.field === 'credit');
          const debitColumn = this.columns.find(col => col.field === 'debit');
          const creditSum = creditColumn?.sumColumnValue || 0;
          const debitSum = debitColumn?.sumColumnValue || 0;
          return debitSum - creditSum;
        },
        filter: new TableColumnFilter('remaining', TableColumnFilterTypes.Money),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.None,


      },
      {
        ...this.defaultColumnSettings,
        index: 14,
        field: 'currencyDebit',
        title: 'بدهکار ارزی',
        width: 5,
        isCurrencyField: true,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        showSum: true,
        filter: new TableColumnFilter('currencyDebit', TableColumnFilterTypes.Money),
        display: false,
        print: false,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 15,
        field: 'currencyCredit',
        title: 'بستانکار ارزی',
        width: 5,
        isCurrencyField: true,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('currencyCredit', TableColumnFilterTypes.Money),
        showSum: true,
        display: false,
        print: false,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 16,
        field: 'currencyRemain',
        title: 'مانده ارزی',
        width: 5,
        isCurrencyField: true,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        showSum: true,
        sumRowDisplayFn: () => {
          const currencyCreditColumn = this.columns.find(col => col.field === 'currencyCredit');
          const currencyDebitColumn = this.columns.find(col => col.field === 'currencyDebit');
          const creditSum = currencyCreditColumn?.sumColumnValue || 0;
          const debitSum = currencyDebitColumn?.sumColumnValue || 0;
          return debitSum - creditSum;
        },
        filter: new TableColumnFilter('currencyRemain', TableColumnFilterTypes.Money),
        display: false,
        print: false,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.None,

      },
      {
        ...this.defaultColumnSettings,
        index: 17,
        field: 'currencyFee',
        title: 'نرخ تبدیل ارز',
        width: 4,
        isCurrencyField: true,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('currencyFee', TableColumnFilterTypes.Money),
        display: false,
        print: false,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 18,
        field: 'currencyTypeBaseTitle',
        title: 'نوع ارز',
        width: 5,
        isCurrencyField: true,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('currencyTypeBaseTitle', TableColumnFilterTypes.Text),
        display: false,
        print: false,
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NgSelect,
        filteredOptions: this.currencyTypes,
        optionsValueKey: 'id',
        optionsSelectTitleKey: 'title',
        optionsTitleKey: ['title'],

      },

    ];
    this.toolBar = {
      showTools: {
        tableSettings: true,
        includeOnlySelectedItemsLocal: true,
        excludeSelectedItemsLocal: false,
        includeOnlySelectedItemsFilter: false,
        excludeSelectedItemsFilter: true,
        undoLocal: false,
        deleteLocal: false,
        restorePreviousFilter: true,
        refresh: true,
        exportExcel: false,
        fullScreen: true,
        printFile: true,
        removeAllFiltersAndSorts: true,
        showAll: true
      },
      isLargeSize: false
    };
    if (!this.identityService.doesHavePermission('CurrencyFieldAccess')) {
      this.columns = this.columns.filter(
        column => !column.isCurrencyField
      )
    }

    if (!this.identityService.doesHavePermission('TaxpayerFlag')) {
      this.columns = this.columns.filter(
        column => column.field !== 'taxpayerFlag'
      )
    }


    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش دفتر تفضیلی مطابق ردیف های سند'));
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.printSumRow = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.pagination.pageSize = 500;
    this.tableConfigurations.options.exportOptions.showExportButton = false;


    forkJoin([

      this.mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    ]).subscribe(async ([ currencyTypes]) => {
      this.accountHeads = this.accountManagerService.accountHeads.value;
      this.accountReferencesGroups = this.accountManagerService.accountReferenceGroups.value;
      this.accountReferences = this.accountManagerService.accountReferences.value;
      this.codeVoucherGroups = this.accountManagerService.codeVoucherGroups.value;
      this.currencyTypes = currencyTypes;
      this.updateColumnFilteredOptions('title');
      this.updateColumnFilteredOptions('accountHeadCode');
      this.updateColumnFilteredOptions('referenceName_1');
      this.updateColumnFilteredOptions('referenceCode_1');
      this.updateColumnFilteredOptions('currencyTypeBaseTitle', currencyTypes);

      this.initialize().then(() => {
        this.get();
      });
    });


  }

  updateColumnFilteredOptions(columnField: string, data?: any[]) {
    const column = this.tableConfigurations.columns.find((col: any) => col.field === columnField);
    if (column?.field ==='referenceCode_1' || column?.field === 'referenceName_1') {
      column.filteredOptions = this.accountManagerService.accountReferences.value;

    }else if(column?.field ==='accountHeadCode' || column?.field === 'title'){
      column.filteredOptions = this.accountManagerService.accountHeads.value
    }
    else if(column && data)  {
      column.filteredOptions = [...data];

    }
  }

  async initialize(params?: any) {
    let allCurrency = new BaseValue();
    allCurrency.baseValueTypeId = 0;
    allCurrency.code = '0';
    allCurrency.id = 0;
    allCurrency.isReadOnly = true;
    allCurrency.levelCode = '0';
    allCurrency.orderIndex = 1;
    allCurrency.title = 'همه';
    allCurrency.uniqueName = 'all';
    allCurrency.value = '0';
    this.currencyTypes.push(allCurrency);
    let query = new GetLedgerReportQuery();
    let accountHeadId = <string>this.getQueryParam('accountHeadId');
    if (accountHeadId) {
      let accountHeads = accountHeadId.split(',').map(x => Number(x));
      for (let i = 0; i < accountHeads.length; i++) {
        if (accountHeads[i] != 0) {
          query.accountHeadIds.push(+accountHeads[i]);
          const foundAccountHead = <AccountHead>this.accountHeads.find(x => x.id === +accountHeads[i]);
          if (foundAccountHead) {
            this.selectedAccountHeads.push(foundAccountHead);
          }
        }
      }
    }
    let codeVoucherGroupIds = <string>this.getQueryParam('codeVoucherGroupIds');
    if (codeVoucherGroupIds) {
      let codeVoucherGroups = codeVoucherGroupIds.split(',').map(x => Number(x));
      for (let i = 0; i < codeVoucherGroups.length; i++) {
        if (codeVoucherGroups[i] != 0) {
          query.codeVoucherGroupIds.push(+codeVoucherGroups[i]);
          const foundCodeVoucherGroup = <CodeVoucherGroup>this.codeVoucherGroups.find(x => x.id === +codeVoucherGroups[i]);
          if (foundCodeVoucherGroup) {
            this.selectedCodeVoucherGroups.push(foundCodeVoucherGroup);
          }
        }
      }
    }

    let accountReferenceGroupId = <string>this.getQueryParam('accountReferenceGroupId');
    if (accountReferenceGroupId) {
      let accountReferenceGroupIds = accountReferenceGroupId.split(',').map(x => Number(x));
      for (let i = 0; i < accountReferenceGroupIds.length; i++) {
        if (accountReferenceGroupIds[i] && accountReferenceGroupIds[i] != 0 && accountReferenceGroupIds[i] != undefined) {
          query.referenceGroupIds.push(+accountReferenceGroupIds[i]);
        }
      }
    }

    let accountReferenceId = <string>this.getQueryParam('accountReferenceId');
    if (accountReferenceId) {
      let accountReferenceIds = accountReferenceId.split(',').map(x => Number(x));
      for (let i = 0; i < accountReferenceIds.length; i++) {
        if (accountReferenceIds[i] && accountReferenceIds[i] != 0 && accountReferenceIds[i] != undefined) {
          query.referenceIds.push(+accountReferenceIds[i]);
        }
      }
    }


    let fromDate = this.getQueryParam('fromDate');
    if (fromDate) {

      fromDate = decodeURIComponent(fromDate);
      fromDate = decodeURIComponent(fromDate);
      query.voucherDateFrom = new Date(fromDate);
    }
    let toDate = this.getQueryParam('toDate');
    if (toDate) {
      toDate = decodeURIComponent(toDate);
      toDate = decodeURIComponent(toDate);
      query.voucherDateTo = new Date(toDate);
    }
    let typeId = this.getQueryParam('typeId');


    this.request = query;


    // let currentUserYear = this.userAllowedYears.find(x => x.id == this.identityService.applicationUser.yearId);
    this.form.patchValue({
      voucherDateFrom: query.voucherDateFrom ? query.voucherDateFrom : this.identityService.getActiveYearStartDate(),
      voucherDateTo: query.voucherDateTo ? query.voucherDateTo : this.identityService.getActiveYearEndDate(),
      companyId: +this.identityService.applicationUser.companyId,
      reportFormat: ReportFormatTypes.None,
      currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'all')?.id,
    });

    this.form.controls['yearIds'].controls.push(new FormControl(+this.identityService.applicationUser.yearId))
    this.form.patchValue(this.form.getRawValue())


    // this.accountHeadControl.valueChanges.subscribe(async (newValue) => {
    //   if (typeof newValue !== "number") this.accountHeads = await this.getAccountHeads(newValue);
    // })
    // this.accountReferenceControl.valueChanges.subscribe(async (newValue) => {
    //   if (typeof newValue !== "number") this.accountReferences = await this.getAccountReferences(newValue);
    // })
    // this.accountReferencesGroupControl.valueChanges.subscribe(async (newValue) => {
    //   if (typeof newValue !== "number") this.accountReferencesGroups = await this.getAccountReferencesGroups(newValue);
    // })
    // this.codeVoucherGroupControl.valueChanges.subscribe(async (newValue) => {
    //   if (typeof newValue !== "number") this.codeVoucherGroups = await this.getCodeVoucherGroups(newValue);
    //   if (!newValue) this.form.controls['codeVoucherGroupIds'].setValue(null)
    // })
    this.isLoading = false;
  }

  async navigateToVoucherHead(voucherDetail: VoucherDetail) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${voucherDetail.voucherId}&vdId=${voucherDetail.id}`)
  }

  async getDataFromServer(printType: number, forcePrint: boolean = false): Promise<any> {
    let pageSize = 0;
    let pageIndex = 0;

    let copy = <GetLedgerReportQuery>Object.assign({}, this.request);
    let query = <GetLedgerReportQuery>this.request;
    let queryForSelectedItem = new GetLedgerReportQuery();

    let result;

    if (this.selectedItemsFilterForPrint && this.selectedItemsFilterForPrint[0].values.length > 0) {
      if (this.selectedItemsFilterForPrint.length > 1000) {
        this.toastr.showToast({message: '  برای دیتای بیشتر از 1000 رکورد خروجی اکسل داده میشود.', type: 'info'});
      } else {
        this.toastr.showToast({message: 'دیتا های انتخابی شما در حال ساخت است', type: 'info'});
      }
      queryForSelectedItem.conditions = this.selectedItemsFilterForPrint;
      queryForSelectedItem.pageSize = pageSize;
      queryForSelectedItem.pageIndex = pageIndex;
      queryForSelectedItem.useEF = true;
      queryForSelectedItem.isPrint = true;
      queryForSelectedItem.reportFormat = 1;
      queryForSelectedItem.printType = printType;
      queryForSelectedItem.forcePrint = forcePrint;
      queryForSelectedItem.companyId = query.companyId;
      queryForSelectedItem.currencyTypeBaseId = query.currencyTypeBaseId;
      queryForSelectedItem.yearIds = [+this.identityService.applicationUser.yearId]

      if (printType != 0) {
        queryForSelectedItem.orderByProperty = "currencyTypeBaseTitle DESC," + query.orderByProperty;
      }
      await this.mediator.send(queryForSelectedItem).then(
        (response) => {
          result = response.data
          this.isPrinting = false;

        }).catch(() => {
        this.isPrinting = false;
        this.toastr.showToast({message: 'خطا در گرفتن اطلاعات از سرور', type: 'warning'});
      });

    } else {
      if (this.tableConfigurations.pagination.totalItems > 1000) {
        this.toastr.showToast({message: '  برای دیتای بیشتر از 1000 رکورد خروجی اکسل داده میشود.', type: 'info'});
        this.isPrinting = false;
        this.isLoading = false;
      } else {

        this.toastr.showToast({message: 'دیتا های  شما در حال ساخت است', type: 'info'});
      }
      query.pageSize = pageSize;
      query.pageIndex = pageIndex;
      query.useEF = true;
      query.isPrint = true;
      query.reportFormat = 1;
      query.printType = printType;
      query.forcePrint = forcePrint;
      query.yearIds = [+this.identityService.applicationUser.yearId]
      if (!(query.conditions))
        query.conditions = [];
      query.conditions.push(...this.includedRows);
      query.conditions.push(...this.excludedRows);
      if (printType != 0) {
        query.orderByProperty = "currencyTypeBaseTitle DESC," + query.orderByProperty;
      }
      await this.mediator.send(query).then(
        (response) => {
          result = response.data
          this.isPrinting = false;

        }).catch(() => {
        this.toastr.showToast({message: 'خطا در گرفتن اطلاعات از سرور', type: 'warning'});
        this.isPrinting = false;
      })

    }

    this.request.pageSize = copy.pageSize;
    this.request.pageIndex = copy.pageIndex;
    //@ts-ignore
    this.request.useEF = copy.useEF;
    //@ts-ignore
    this.request.isPrint = copy.isPrint;
    //@ts-ignore
    this.request.reportFormat = copy.reportFormat;
    //@ts-ignore
    this.request.printType = copy.printType;
    //@ts-ignore
    this.request.forcePrint = copy.forcePrint;
    //@ts-ignore
    this.request.yearIds = copy.yearIds;
    //@ts-ignore
    this.request.orderByProperty = copy.orderByProperty;

    return result;
  }

  async convertToExcel(result: any , fileName:string = "دفتر تفصیلی مطابق ردیفهای سند"): Promise<void> {
    const binaryData = atob(result);

    // Convert binary string to an array of bytes
    const byteNumbers = new Uint8Array(binaryData.length);
    for (let i = 0; i < binaryData.length; i++) {
      byteNumbers[i] = binaryData.charCodeAt(i);
    }

    // Create a Blob from the byte array
    const blob = new Blob([byteNumbers], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});

    // Create a link element to trigger download
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = fileName+'.xlsx'; // Set the file name with .xlsx extension
    a.click();

    // Clean up and release the object URL
    window.URL.revokeObjectURL(url);
  }

  async downloadRialExcel() {
    this.isLoading = true;
    let result = await this.getDataFromServer(0, true);
    if (typeof result === "string") {
      await this.convertToExcel(result);
    }
    this.isLoading = false;
  }
  async downloadElectronicLedgersExcel() {
    this.isLoading = true;
    let result = await this.getDataFromServer(3, true);
    if (typeof result === "string") {
      await this.convertToExcel(result , 'دفاتر الکترونیکی');
    }
    this.isLoading = false;
  }
  async downloadDollarExcel() {

    this.isLoading = true;
    let result = await this.getDataFromServer(1, true);
    if (typeof result === "string") {
      await this.convertToExcel(result)
    }
    this.isLoading = false;
  }

  async downloadRialDollarExcel() {

    this.isLoading = true;
    let result = await this.getDataFromServer(2, true);
    if (typeof result === "string") {
      await this.convertToExcel(result)
    }
    this.isLoading = false;
  }

  async printSsrsRial() {

    let query = <GetLedgerReportQuery>this.request;
    let companyName = this.SSRSReportService.companyName;
    let userName = this.SSRSReportService.userName;
    let reportTime = this.SSRSReportService.reportTime;
    let CompanyId = this.SSRSReportService._companyId;
    let ReportType = this.SSRSReportService._reportType;
    let yearName = this.SSRSReportService.yearName;
    let voucherDateFrom = new Date(query.voucherDateFrom ?? new Date()).toISOString();
    let voucherDateTo = new Date(query.voucherDateTo ?? new Date()).toISOString();
    let persianDateFrom = new ToPersianDatePipe().transform(voucherDateFrom);
    let persianDateTo = new ToPersianDatePipe().transform(voucherDateTo);
    let reportTitle = "ریز گردش";

    let sourceURL = `Acc_Ledger_1&CompanyId=${CompanyId}&ReportType=${ReportType}&Level=3&ReportTime=${reportTime}&CompanyName=${companyName}&YearName=${yearName}&VoucherDateFrom=${voucherDateFrom}&VoucherDateTo=${voucherDateTo}&PersianDateFrom=${persianDateFrom}&PersianDateTo=${persianDateTo}&UserName=${userName}&ReportTitle=${reportTitle}`;

    sourceURL += this.SSRSReportService.addParamIfDefined('YearIds', query.yearIds, true);
    sourceURL += this.SSRSReportService.addParamIfDefined('ReferenceIds', query.referenceIds, true);
    sourceURL += this.SSRSReportService.addParamIfDefined('AccountHeadIds', query.accountHeadIds, true);
    sourceURL += this.SSRSReportService.addParamIfDefined('VoucherStateId', query.voucherStateId);
    sourceURL += this.SSRSReportService.addParamIfDefined('CodeVoucherGroupIds', query.codeVoucherGroupIds, true);
    sourceURL += this.SSRSReportService.addParamIfDefined('TransferId', query.transferId);
    sourceURL += this.SSRSReportService.addParamIfDefined('ReferenceGroupIds', query.referenceGroupIds, true);
    sourceURL += this.SSRSReportService.addParamIfDefined('ReferenceNo', query.referenceNo);
    sourceURL += this.SSRSReportService.addParamIfDefined('VoucherNoFrom', query.voucherNoFrom);
    sourceURL += this.SSRSReportService.addParamIfDefined('VoucherNoTo', query.voucherNoTo);
    sourceURL += this.SSRSReportService.addParamIfDefined('DebitFrom', query.debitFrom);
    sourceURL += this.SSRSReportService.addParamIfDefined('DebitTo', query.debitTo);
    sourceURL += this.SSRSReportService.addParamIfDefined('CreditFrom', query.creditFrom);
    sourceURL += this.SSRSReportService.addParamIfDefined('CreditTo', query.creditTo);
    sourceURL += this.SSRSReportService.addParamIfDefined('DocumentIdFrom', query.documentIdFrom);
    sourceURL += this.SSRSReportService.addParamIfDefined('DocumentIdTo', query.documentIdTo);
    sourceURL += this.SSRSReportService.addParamIfDefined('VoucherDescription', query.voucherDescription);
    sourceURL += this.SSRSReportService.addParamIfDefined('VoucherRowDescription', query.voucherRowDescription);
    sourceURL += this.SSRSReportService.addParamIfDefined('Remain', query.remain);


    if (query.currencyTypeBaseId) {
      sourceURL += this.SSRSReportService.addParamIfDefined('CurrencyTypeBaseId', query.currencyTypeBaseId);
    }


    let conditionsString = this.SSRSReportService.buildConditionsString(query.conditions);

    if (conditionsString) {
      sourceURL += `&${conditionsString}`;
    }

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      sourceURL: sourceURL,
    };
    this.dialog.open(SSRSReportDialogComponent, dialogConfig)


  }

  async printRial() {
    // this.isPrinting = true;
    // this.isLoading = true;

    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(0, false);
    if (typeof result === "string") {
      this.toastr.showToast({message: '  برای دیتای بیشتر از 1000 رکورد خروجی اکسل داده میشود.', type: 'info'});
      this.convertToExcel(result)
      // this.isPrinting = false;
      // this.isLoading = false;
      return;
    }

    let printContents = '';
    let name = '';
    if (result.length > 0) {
      let accountHeadName = '';

      if (query.accountHeadIds.length == 1) {
        const res1 = result.filter((x: any) => x.id !== 0);
        accountHeadName = "کد حساب: " + res1[0].accountHeadCode + "  -  عنوان حساب: " + res1[0].title;
      }
      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result.filter((x: any) => x.id !== 0)).pipe(distinct(e => e.referenceCode_1))
          // @ts-ignore
          .subscribe(x => name += `کد: ${x.referenceCode_1} - نام: ${x.referenceName_1} <br/>`)
      }
      printContents = `<div style="text-align: center">
                          <div style="width: 25%;float: right;font-size: 12px;">
                              از تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateFrom)}
                              تا تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateTo)}
                              <br>
                              ${accountHeadName}
                          </div>
                          <div style="width: 50%;float: right;font-size: 14px;">
                                  ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                                 <br>
                                 دفتر تفصیلی مطابق ردیفهای سند
                                 <br>
                                 <br>

                                 ${name}
                          </div>
                          <div style="width: 25%;float: right;font-size: 12px;">
                                تاریخ چاپ: ${new ToPersianDatePipe().transform(new Date())}
                          </div>
                        </div>`

      printContents += `<table><thead>
                       <tr>
                       <th style="width: 2%;">ردیف</th>
                       <th style="width: 3%;">سند</th>
                       <th style="width: 7%;">تاریخ</th>`;

      if (query.accountHeadIds.length != 1) {
        printContents += `
                                         <th style="width: 6%;">کد حساب</th>
                                         <th style="width: 8%;">عنوان حساب</th>`;
      }
      if (query.referenceIds.length == 0) {
        printContents += `
                    <th style="width: 6%;">کد تفصیل</th>
                    <th style="width: 8%;">عنوان تفصیل</th>`;
      }
      printContents += `<th style="width: 30%;">شرح</th>
                        <th>بدهکار</th>
                        <th>بستانکار</th>
                        <th>مانده</th>
                      </tr>
                    </thead><tbody>`
      // @ts-ignore
      let remaining = 0;
      let allDebit = 0;
      let allCredit = 0;
      for (let i = 0; i < result.length; i++) {
        let data = result[i]
        printContents += `
        <tr  style="direction: ltr;text-align: center;">
        <td style="font-size: 9px;">${i + 1}</td>
        <td style="font-size: 9px;">${data.voucherNo}</td>
        <td style="font-size: 9px;">${new ToPersianDatePipe().transform(data.voucherDate)}</td>`;

        if (query.accountHeadIds.length != 1) {
          printContents += `<td style="font-size: 9px;">${data.accountHeadCode}</td>
        <td style="font-size: 9px;">${data.title}</td>‍`;
        }
        if (query.referenceIds.length == 0) {
          printContents += `
          <td style="font-size: 9px;">${data.referenceCode_1}</td>
          <td style="font-size: 9px;">${data.referenceName_1}</td>`;
        }

        printContents += `
                <td style="font-size: 9px;">${data.voucherRowDescription}</td>
                <td style="font-size: 9px;">${this.customDecimal.transform(data.debit)}</td>
                <td style="font-size: 9px;">${this.customDecimal.transform(data.credit)}</td>`;

        remaining = data.debit - data.credit + remaining;
        allDebit += data.debit;
        allCredit += data.credit;
        printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(remaining)}</td></tr>`;
      }
      printContents += `
      <tr style="direction: ltr;text-align: center;background-color: #e7e7eb;">
      <td style="font-size: 9px;"></td>
      <td style="font-size: 9px;"></td>
      <td style="font-size: 9px;"></td>`;

      if (query.accountHeadIds.length != 1) {
        printContents += `
      <td style="font-size: 9px;"></td>
      <td style="font-size: 9px;"></td>`
      }
      if (query.referenceIds.length == 0) {
        printContents += `
                    <td></td>
                    <td></td>`;
      }

      printContents += ` <td style="font-size: 14px;"> جمع کل</td>
      <td style="font-size: 14px;"> ${this.customDecimal.transform(allDebit)}</td>
      <td style="font-size: 14px;"> ${this.customDecimal.transform(allCredit)}</td>
      <td style="font-size: 14px;">${this.customDecimal.transform(remaining)}</td>
      </tr>`;

      printContents += `</tbody></table>
      <p style="text-align: left;padding: 30px;">
        کاربر چاپ کننده: ${this.applicationUserFullName}
      </p>`;

      this.isPrinting = false;
      this.isLoading = false;
      this.Service.onPrint(printContents, "")
    }


    this.isPrinting = false;
    this.isLoading = false;
  }

  async printDollar() {

    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(1, false);
    if (typeof result === "string") {
      this.toastr.showToast({message: '  برای دیتای بیشتر از 1000 رکورد خروجی اکسل داده میشود.', type: 'info'});
      await this.convertToExcel(result);

      return;
    }
    let printContents = '';
    let name = '';
    if (result.length > 0) {

      let accountHeadName = '';
      if (query.accountHeadIds.length == 1) {
        const res1 = result.filter((x: any) => x.id !== 0);
        accountHeadName = "کد حساب: " + res1[0].accountHeadCode + "  -  عنوان حساب: " + res1[0].title;
      }

      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result.filter((x: any) => x.id !== 0)).pipe(distinct(e => e.referenceCode_1))
          // @ts-ignore
          .subscribe(x => name += `نام: ${x.referenceCode_1} , ${x.referenceName_1} <br/>`)
      }
      printContents = `<div style="text-align: center">
                          <div style="width: 20%;float: right;font-size: 14px;">
                              از تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateFrom)}
                              تا تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateTo)}
                          </div>
                          <div style="width: 60%;float: right;">
                                 ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                                 <br>
                                 دفتر تفصیلی مطابق ردیفهای سند
                                 <br><br><br>
                                 ${accountHeadName}
                                 <br>
                                 ${name}
                          </div>
                          <div style="width: 20%;float: right;font-size: 14px;">
                                تاریخ چاپ: ${new ToPersianDatePipe().transform(new Date())}
                          </div>
                        </div>`

      printContents += `<table><thead>
                       <tr>
                       <th style="width: 2%;">ردیف</th>
                       <th style="width: 3%;">سند</th>
                       <th style="width: 7%;">تاریخ</th>`;


      printContents += `<th style="width: 20%;">شرح</th>
                        <th>بدهکار</th>
                        <th>بستانکار</th>
                        <th>مانده</th>
                        <th>نوع ارز</th>
                      </tr>
                    </thead><tbody>`
      // @ts-ignore
      let remaining = 0;
      let allDebit = 0;
      let allCredit = 0;
      let showSum = false;
      result = result.filter((x: any) => x.currencyAmount);
      for (let i = 0; i < result.length; i++) {
        let data = result[i];
        printContents += `
        <tr  style="direction: ltr;text-align: center;">
        <td style="font-size: 9px;">${i + 1}</td>
        <td style="font-size: 9px;">${data.voucherNo}</td>
        <td style="font-size: 9px;">${new ToPersianDatePipe().transform(data.voucherDate)}</td>`;



        printContents += `<td style="font-size: 9px;">${data.voucherRowDescription}</td>`;

        if (data.debit != 0) {
          printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(data.currencyAmount)}</td>
          <td style="font-size: 9px;">${this.customDecimal.transform(data.credit)}</td>`
          remaining = data.currencyAmount - data.credit + remaining;
          allDebit += data.currencyAmount;
        }

        if (data.credit != 0) {
          printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(data.debit)}</td>
          <td style="font-size: 9px;">${this.customDecimal.transform(data.currencyAmount)}</td>`
          remaining = data.debit - data.currencyAmount + remaining;
          allCredit += data.currencyAmount;
        }

        printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(remaining)}</td>`;

        printContents += `<td style="font-size: 9px;">${data.currencyTypeBaseTitle}</td>‍</tr>`

        if (result[i + 1]) {
          if (result[i + 1].currencyTypeBaseTitle != data.currencyTypeBaseTitle) {
            showSum = true;
          }
        } else {
          showSum = true;
        }
        if (showSum) {
          printContents += `
            <tr style = "direction: ltr;text-align: center;background-color: #e7e7eb;" >
              <td style="font-size: 9px;"></td>
              <td style="font-size: 9px;"> </td>
              <td style="font-size: 9px;"> </td>`;


          printContents += ` <td style="font-size: 10px;"> جمع کل(${data.currencyTypeBaseTitle})</td>
      <td style="font-size: 10px;"> ${this.customDecimal.transform(allDebit)}</td>
      <td style="font-size: 10px;"> ${this.customDecimal.transform(allCredit)}</td>
      <td style="font-size: 10px;">${this.customDecimal.transform(remaining)}</td>
      <td style="font-size: 9px;"> </td>
      </tr>`;


          remaining = 0;
          allDebit = 0;
          allCredit = 0;
          showSum = false;
        }

      }


      printContents += `</tbody></table>
      <p style="text-align: left;padding: 30px;">
        کاربر چاپ کننده: ${this.applicationUserFullName}
      </p>`;

      this.isPrinting = false;
      this.isLoading = false;
      this.Service.onPrint(printContents, "")

    }

    this.isPrinting = false;
    this.isLoading = false;
  }

  async printDollarAndRial() {

    // this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(2, false);
    if (typeof result === "string") {
      this.toastr.showToast({message: '  برای دیتای بیشتر از 1000 رکورد خروجی اکسل داده میشود.', type: 'info'});
      await this.convertToExcel(result)
      this.isLoading = false;
      return;
    }
    let printContents = '';
    let name = '';
    if (result.length > 0) {

      let accountHeadName = '';
      if (query.accountHeadIds.length == 1) {
        const res1 = result.filter((x: any) => x.id !== 0);
        accountHeadName = "کد حساب: " + res1[0].accountHeadCode + "  -  عنوان حساب: " + res1[0].title;
      }
      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result.filter((x: any) => x.id !== 0)).pipe(distinct(e => e.referenceCode_1))
          // @ts-ignore
          .subscribe(x => name += `نام: ${x.referenceCode_1} , ${x.referenceName_1} <br/>`)
      }
      printContents = `<div style="text-align: center">
                          <div style="width: 20%;float: right;font-size: 14px;">
                              از تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateFrom)}
                              تا تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateTo)}
                          </div>
                          <div style="width: 60%;float: right;">
                                  ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                                 <br>
                                 دفتر تفصیلی مطابق ردیفهای سند
                                 <br><br>
                                 ${accountHeadName}
                                 <br>
                                 ${name}
                          </div>
                          <div style="width: 20%;float: right;font-size: 14px;">
                                تاریخ چاپ: ${new ToPersianDatePipe().transform(new Date())}
                          </div>
                        </div>`

      printContents += `<br><table><thead>
                       <tr>
                       <th style="width: 2%;">ردیف</th>
                       <th style="width: 3%;">سند</th>
                       <th style="width: 7%;">تاریخ</th>`;

      printContents += `<th style="width: 20%;">شرح</th>
                        <th>بدهکار ریالی</th>
                        <th>بستانکار ریالی</th>
                        <th>مانده ریالی</th>

                        <th>بدهکار ارزی</th>
                        <th>بستانکار ارزی</th>
                        <th>مانده ارزی</th>

                        <th>نوع ارز</th>
                      </tr>
                    </thead><tbody>`
      // @ts-ignore
      let remaining = 0;
      let dollarRemaining = 0;
      let allDebit = 0;
      let allCredit = 0;
      let allDollarDebit = 0;
      let allDollarCredit = 0;
      let showSum = false;
      result = result.filter((x: any) => x.currencyAmount);
      for (let i = 0; i < result.length; i++) {
        let data = result[i];

        printContents += `
        <tr  style="direction: ltr;text-align: center;">
        <td style="font-size: 9px;">${i + 1}</td>
        <td style="font-size: 9px;">${data.voucherNo}</td>
        <td style="font-size: 9px;">${new ToPersianDatePipe().transform(data.voucherDate)}</td>`;

        // if (this.selectedAccountHeads.length != 1) {

        //   printContents += `
        // <td style="font-size: 9px;">${data.accountHeadCode}</td>
        // <td style="font-size: 9px;">${data.title}</td>‍`;
        // }

        // if (query.referenceIds.length == 0) {
        //   printContents += `
        //   <td style="font-size: 9px;">${data.referenceCode_1}</td>
        //   <td style="font-size: 9px;">${data.referenceName_1}</td>`;
        // }

        printContents += `<td style="font-size: 9px;">${data.voucherRowDescription}</td>`;

        printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(data.debit)}</td>
          <td style="font-size: 9px;">${this.customDecimal.transform(data.credit)}</td>`

        remaining = data.debit - data.credit + remaining;
        allDebit += data.debit;
        allCredit += data.credit;

        printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(remaining)}</td>`;

        let debitDollar = 0;
        let creditDollar = 0;
        if (data.credit > 0) {
          creditDollar = data.currencyAmount;
          debitDollar = 0;
        } else {
          creditDollar = 0;
          debitDollar = data.currencyAmount;
        }
        allDollarCredit += creditDollar;
        allDollarDebit += debitDollar;
        dollarRemaining = debitDollar - creditDollar + dollarRemaining;

        printContents += `
        <td style="font-size: 9px;">${this.customDecimal.transform(debitDollar)}</td>
        <td style="font-size: 9px;">${this.customDecimal.transform(creditDollar)}</td>`

        printContents += `<td style="font-size: 9px;">${this.customDecimal.transform(dollarRemaining)}</td>`;

        printContents += ` <td style="font-size: 9px;">${data.currencyTypeBaseTitle}</td>‍</tr>`

        if (result[i + 1]) {
          if (result[i + 1].currencyTypeBaseTitle != data.currencyTypeBaseTitle) {
            showSum = true;
          }
        } else {
          showSum = true;
        }
        if (showSum) {
          printContents += `
  <tr style = "direction: ltr;text-align: center;background-color: #e7e7eb;" >
    <td style="font-size: 9px;"></td>
    <td style="font-size: 9px;"> </td>
    <td style="font-size: 9px;"> </td>`;
          //   if (this.selectedAccountHeads.length != 1) {

          //     printContents += `
          // <td style="font-size: 9px;"> </td>
          // <td style="font-size: 9px;"> </td>`;
          //   }
          //   if (query.referenceIds.length == 0) {
          //     printContents += `
          //   <td></td>
          //   <td></td>`;
          //   }

          printContents += ` <td style="font-size: 12px;"> جمع کل(${data.currencyTypeBaseTitle})</td>
<td style="font-size: 10px;"> ${this.customDecimal.transform(allDebit)}</td>
<td style="font-size: 10px;"> ${this.customDecimal.transform(allCredit)}</td>
<td style="font-size: 10px;">${this.customDecimal.transform(remaining)}</td>

<td style="font-size: 10px;">${this.customDecimal.transform(allDollarDebit)}</td>
<td style="font-size: 10px;">${this.customDecimal.transform(allDollarCredit)}</td>

<td style="font-size: 10px;">${this.customDecimal.transform(dollarRemaining)}</td>
<td style="font-size: 10px;"> </td>
</tr>`;


          remaining = 0;
          dollarRemaining = 0;
          allDebit = 0;
          allCredit = 0;
          allDollarDebit = 0;
          allDollarCredit = 0;
          showSum = false;
        }


      }


      printContents += `</tbody></table>
      <p style="text-align: left;padding: 30px;">
        کاربر چاپ کننده: ${this.applicationUserFullName}
      </p>`;

      this.isPrinting = false;
      this.isLoading = false;
      this.Service.onPrint(printContents, "")
    }
    this.isPrinting = false;
    this.isLoading = false;
  }


  async get(param?: any, action?: string) {
    this.tableConfigurations.options.isLoadingTable = true;
    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];

    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.excludedRows));
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
    if (this.filterConditionsInputSearch) {
      Object.keys(this.filterConditionsInputSearch).forEach(key => {
        const filter = this.filterConditionsInputSearch[key];
        if (filter && filter.propertyNames && filter.searchValues && filter.searchValues[0]) {
          filter.propertyNames.forEach((propertyName: string) => {
            searchQueries.push(new SearchQuery({
              propertyName: propertyName,
              values: filter.searchValues,
              comparison: filter.searchCondition,
              nextOperand: filter.nextOperand
            }));
          });
        }
      });
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys.length == 0) {
      orderByProperty = 'voucherDate ASC';
    } else {

      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })

    }
    //@ts-ignore
    this.request.usePagination = true;
    let request = <GetLedgerReportQuery>this.request;

    request.reportFormat = 1;
    request.conditions = searchQueries;
    request.pageSize = this.tableConfigurations.pagination.pageSize
    if (this.includedRows && this.includedRows?.length) {
      request.pageIndex = 1
      this.tableConfigurations.pagination.pageIndex = 1
    } else {

      request.pageIndex = this.tableConfigurations.pagination.pageIndex + 1
    }
    request.orderByProperty = orderByProperty;
    request.yearIds = [+this.identityService.applicationUser.yearId]

    if (action == 'back') {

      this.requestsList.pop();
      this.requestsIndex = this.requestsIndex - 1;
      if (this.requestsList.length > 0) {

        let temp = <GetLedgerReportQuery>JSON.parse(this.requestsList[this.requestsList.length - 1]);
        request = this.createRequest(temp);
        this.request = request;


      } else {
        this.resetRequest();
        return;
      }
    } else {
      this.requestsList.push(JSON.stringify(request));
      this.requestsIndex++;
    }

    return await this.mediator.send(request).then(res => {
      this.reportResult = res.data.map((item: any) => ({
        ...item,
        typeTax: Math.random() >= 0.5,
      }));
      this.totalDebit.setValue(res.totalDebit)
      this.totalCredit.setValue(res.totalCredit)
      this.totalCurrencyDebit.setValue(res.totalCurrencyDebit)
      this.totalCurrencyCredit.setValue(res.totalCurrencyCredit)
      this.remaining.setValue(res.remaining);
      this.currencyRemain.setValue(res.currencyRemain);
      res.totalCount ? this.tableConfigurations.pagination.totalItems = res.totalCount : '';
      this.tableConfigurations.options.isLoadingTable = false;
      this.tableConfigurations.printOptions.dateFrom = this.form.controls['voucherDateFrom'].value;
      this.tableConfigurations.printOptions.dateTo = this.form.controls['voucherDateTo'].value;
    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    })

  }

  resetRequest() {
    const temp = new GetLedgerReportQuery();
    this.request = this.createRequest(temp);
    this.includedRows = [];
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.filterConditionsInputSearch = {};
    this.requestsList = [];
    this.requestsIndex = -1;

    setTimeout(() => {
      this.get().then();
    }, 0);
  }

  createRequest(temp: GetLedgerReportQuery) {
    let request = new GetLedgerReportQuery();
    request.useEF = temp.useEF;
    request.reportType = temp.reportType;
    request.level = temp.level;
    request.companyId = temp.companyId;
    request.yearIds = temp.yearIds;
    request.voucherStateId = temp.voucherStateId;
    request.codeVoucherGroupIds = temp.codeVoucherGroupIds;
    request.transferId = temp.transferId;
    request.accountHeadIds = temp.accountHeadIds;
    request.referenceGroupIds = temp.referenceGroupIds;
    request.referenceIds = temp.referenceIds;
    request.referenceNo = temp.referenceNo;
    request.voucherNoFrom = temp.voucherNoFrom;
    request.voucherNoTo = temp.voucherNoTo;
    request.voucherDateFrom = temp.voucherDateFrom;
    request.voucherDateTo = temp.voucherDateTo;
    request.debitFrom = temp.debitFrom;
    request.debitTo = temp.debitTo;
    request.creditFrom = temp.creditFrom;
    request.creditTo = temp.creditTo;
    request.documentIdFrom = temp.documentIdFrom;
    request.documentIdTo = temp.documentIdTo;
    request.voucherDescription = temp.voucherDescription;
    request.voucherRowDescription = temp.voucherRowDescription;
    request.reportTitle = temp.reportTitle;
    request.remain = temp.remain;
    request.reportFormat = temp.reportFormat;
    request.isPrint = temp.isPrint;
    request.printType = temp.printType;
    request.forcePrint = temp.forcePrint;
    request.chequeSheetIds = temp.chequeSheetIds;
    request.currencyTypeBaseId = temp.currencyTypeBaseId;
    request.usePagination = temp.usePagination;

    request.pageIndex = temp.pageIndex;
    request.pageSize = temp.pageSize;
    request.orderByProperty = temp.orderByProperty;
    request.conditions = temp.conditions;


    return request;
  }

  handleOptionSelected(event: { typeFilterOptions: any, query: any }) {

    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {
      this.updateNgSelectFilters(event.query)
    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      this.updateInputSearchFilters(event.query)
    }

    if (event.typeFilterOptions == TypeFilterOptions.Date) {
      this.updateInputSearchFilters(event.query)
    }


  }

  async updateInputSearchFilters(filterConditions: { [key: string]: any }) {
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    await this.get()
  }

  async updateNgSelectFilters(filterConditions: { [key: string]: any }) {

    let query = <GetLedgerReportQuery>this.request;
    if (filterConditions['referenceName_1'] && filterConditions['referenceName_1'].searchValues) {
      if (filterConditions['referenceName_1'].searchValues !== 'clear') {
        query.referenceIds = [+filterConditions['referenceName_1'].searchValues];
      } else {
        query.referenceIds = [];
      }
    }
    if (filterConditions['referenceCode_1'] && filterConditions['referenceCode_1'].searchValues) {
      if (filterConditions['referenceCode_1'].searchValues !== 'clear') {

        query.referenceIds = [+filterConditions['referenceCode_1'].searchValues];
      } else {
        query.referenceIds = [];
      }
    }
    if (filterConditions['title'] && filterConditions['title'].searchValues) {
      if (filterConditions['title'].searchValues !== 'clear') {
        query.accountHeadIds = [+filterConditions['title'].searchValues];
      } else {
        query.accountHeadIds = [];
      }
    }
    if (filterConditions['accountHeadCode'] && filterConditions['accountHeadCode'].searchValues) {
      if (filterConditions['accountHeadCode'].searchValues !== 'clear') {
        query.accountHeadIds = [+filterConditions['accountHeadCode'].searchValues];
      } else {
        query.accountHeadIds = [];
      }
    }
    if (filterConditions['currencyTypeBaseTitle'] && filterConditions['currencyTypeBaseTitle'].searchValues) {
      if (filterConditions['currencyTypeBaseTitle'].searchValues !== 'clear') {
        query.currencyTypeBaseId = +filterConditions['currencyTypeBaseTitle'].searchValues;
      } else {
        query.currencyTypeBaseId = 0;
      }
    }
    this.request = query;
    await this.get()
  }

  handleTableConfigurationsChange(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
  }

  handleAdvancedFilter(e: any) {
    this.countAdvancedFilter += 1;
    if (this.countAdvancedFilter !== 1) {
      return
    }

    let dialogConfig = new MatDialogConfig();
    dialogConfig.width = '100%'
    dialogConfig.maxHeight = '100vh'
    dialogConfig.position = {left: '53px'}
    dialogConfig.data = {};
    let dialogReference = this.dialog.open(LedgerReportAdvancedFilterComponent, dialogConfig);
    dialogReference.afterClosed().subscribe((result) => {
      this.countAdvancedFilter = 0
      if (result) {
      }
    })

  }

  async getAccountHeads(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries.push(new SearchQuery({
        propertyName: 'fullCode',
        comparison: 'contains',
        values: [query],
        nextOperand: 'or'
      }))
      searchQueries.push(new SearchQuery({
        propertyName: 'title',
        comparison: 'contains',
        values: [query],
        nextOperand: 'or'
      }))
    }
    return await this.mediator.send(new GetAccountHeadsQuery(0, 50, searchQueries, "fullCode")).then(res => {
      return res.filter(x => x.lastLevel)
    })
  }

  async handleSelectedItemsFilterForPrint(ids: number[]) {
    this.CountSelectedItemsFilterForPrint = ids.length;
    this.selectedItemsFilterForPrint = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'in',
      nextOperand: 'and'
    })];
  }

  async handleRestorePreviousFilter(event: any) {
    if (event) {
      if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
        this.filterConditionsInputSearch = {
          ...this.filterConditionsInputSearch,
          ...event.filterConditions,
        };
      }
    }
    this.includedRows = [];
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    await this.get(undefined, 'back')
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.includedRows = [];
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.requestsList = this.requestsList.slice(0, 1);
    this.filterConditionsInputSearch = {};
    await this.get(undefined, 'back')

  }

  async handleExcludeSelectedItemsEvent(ids: number[]) {
    this.excludedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'notIn',
      nextOperand: 'and'
    })];
    await this.get();
  }

  async getAccountReferences(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this.mediator.send(new GetAccountReferencesQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }


  async getAccountReferencesGroups(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this.mediator.send(new GetAccountReferencesGroupsQuery(1, 10, searchQueries, "code")).then(res => res.data)
  }


  async getCodeVoucherGroups(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this.mediator.send(new GetCodeVoucherGroupsQuery(1, 10, searchQueries)).then(res => res.data)
  }


  showCurrencyRelatedFields(show: boolean) {
    this.showCurrencyFields = show;

  }

  add(param?: any): any {

  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }


  calculateMathABS(debitValue: number, creditValue: number): number {
    const balance = debitValue - creditValue;
    return Math.abs(balance);
  }


  protected readonly fieldTypes = TableColumnDataType;

  updateTaxpayerFlagFn() {
    let taxpayerFlagList = this.changeTaxpayerFlagList;
    if (taxpayerFlagList == null || taxpayerFlagList.length == 0)
      return this.toastr.showToast({message: ' لطفا یک آرتیکل را انتخاب کنید.', type: 'info'});
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تغییر وضعیت اعلام آرتیکل ها',
        message: 'آیا از تغییر وضعیت اعلام آرتیکل ها انتخاب شده مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {
        let request = new UpdateTaxpayerFlagCommand();
        request.taxpayerFlags = taxpayerFlagList;
        this.mediator.send(request)
          .then((response) => {
            this.changeTaxpayerFlagList = [];
            this.isLoading = false
          }).catch(() => {
          this.isLoading = false
        });
      }
    });
  }

  changeTaxpayerFlagFn(row: any, checked: boolean) {
    const existingIndex = this.changeTaxpayerFlagList.findIndex(
      (item) => item.voucherDetailId === row.id
    );
    if (existingIndex !== -1) {
      // Update the existing item
      this.changeTaxpayerFlagList[existingIndex].status = checked;
    } else {
      // Add a new item if it doesn't exist
      this.changeTaxpayerFlagList.push({
        status: checked,
        voucherDetailId: row.id,
      });
    }
  }
}

