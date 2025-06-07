import {Component} from '@angular/core';
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {Router} from "@angular/router";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {AccountReviewReportResultModel} from "../../../repositories/account-review/account-review-report-result-model";
import {FormArray, FormControl} from "@angular/forms";
import {AccountHead} from "../../../entities/account-head";
import {AccountReference} from "../../../entities/account-reference";
import {AccountReferencesGroup} from "../../../entities/account-references-group";
import {CodeVoucherGroup} from "../../../entities/code-voucher-group";
import {BaseValue} from "../../../../admin/entities/base-value";
import {PagesCommonService} from "../../../../../shared/services/pages/pages-common.service";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {AccountManagerService} from "../../../services/account-manager.service";
import {GetAccountReviewReportQuery} from "../../../repositories/reporting/queries/get-account-review-report-query";
import {GetVoucherHeadsQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {VoucherHead} from "../../../entities/voucher-head";
import {
  ConfirmDialogComponent,
  ConfirmDialogConfig
} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {MoneyPipe} from "../../../../../core/pipes/money.pipe";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {forkJoin, Subject} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {ReportFormatTypes} from "../../../repositories/reporting/ReportFormatTypes";
import {GetAccountHeadsQuery} from "../../../repositories/account-head/queries/get-account-heads-query";
import {GetAccountReferencesQuery} from "../../../repositories/account-reference/queries/get-account-references-query";
import {
  GetAccountReferencesGroupsQuery
} from "../../../repositories/account-reference-group/queries/get-account-references-groups-query";
import {
  GetCodeVoucherGroupsQuery
} from "../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {
  TableSettingsService
} from "../../../../../core/components/custom/table/table-details/Service/table-settings.service";
import {RequestsCacheService} from "../../../../../shared/services/RequestsCache/requests-cache.service";
import {UserYear} from "../../../../identity/repositories/models/user-year";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {BaseTable} from "../../../../../core/abstraction/base-table";
import {GetCentralBankReportQuery} from "../../../../admin/repositories/person/queries/get-central-bank-report-query";
import {FontFamilies} from "../../../../../core/components/custom/table/models/font-families";
import {FontWeights} from "../../../../../core/components/custom/table/models/font-weights";
import {TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";

@Component({
  selector: 'app-balance-report',
  templateUrl: './balance-report.component.html',
  styleUrls: ['./balance-report.component.scss']
})
export class BalanceReportComponent extends BaseTable {


  selectedTabIndex: number = 0;
  reportResult: AccountReviewReportResultModel[] = []
  originalReportResult: AccountReviewReportResultModel[] = []
  columnTypeControl = new FormControl();
  private destroy$ = new Subject<void>();

  selectedItemsFilterForPrint: any = [];
  accountHeads: AccountHead[] = [];
  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();

  accountReferences: AccountReference[] = [];
  selectedAccountReferences: AccountReference[] = [];
  accountReferenceControl = new FormControl();

  accountReferencesGroups: AccountReferencesGroup[] = [];
  selectedAccountReferencesGroups: AccountReferencesGroup[] = [];
  accountReferencesGroupControl = new FormControl();

  codeVoucherGroups: CodeVoucherGroup[] = [];
  selectedCodeVoucherGroups: CodeVoucherGroup[] = [];
  codeVoucherGroupControl = new FormControl();
  columnsCount = [
    {
      title: '4 ستونی',
      id: 1
    },
    {
      title: '6 ستونی',
      id: 2
    },

    {
      title: '8 ستونی',
      id: 3
    }
  ]
  accountLevels = [
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

  accountTypes = [
    {
      title: 'موقت',
      id: 1
    },
    {
      title: 'دائم',
      id: 2
    },
  ]
  totalDebit = new FormControl();
  totalCredit = new FormControl();
  currencyTypes: BaseValue[] = [];
  accountReviewPanelState: boolean = true;
  applicationUserFullName!: string;
  allowedYears: UserYear[] = [];
  currentYear!: UserYear;
  showCurrencyFieldsStatus: boolean = false;
  selectedColumn = 6;
  deleteBeforeSelected: boolean = false;
  yearIds: number[] = [];

  constructor(
    private mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    private requestsCacheService: RequestsCacheService,
    public Service: PagesCommonService,
    public identityService: IdentityService,
    private matDialog: MatDialog,
    protected accountManagerService: AccountManagerService,
    private tableSettingsService: TableSettingsService,
  ) {
    super();
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
        this.allowedYears = res.years;
        this.currentYear = <UserYear>res.years.find(x => x.id == res.yearId)
      }
    });
  }


  async ngOnInit() {
    this.defaultColumnSettings.fontFamily =  FontFamilies.IranYekanBold;
    await this.resolve();
  }


  async resolve(params?: any) {
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
        groupRemainingNameOrFn: 'مانده',
        groupRemainingId: 'title',
      },

      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 2,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        groupRemainingNameOrFn: 'مانده',
        groupRemainingId: 'title',
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'code',
        title: 'کد',
        width: 3,
        type: TableColumnDataType.Text,
        // digitsInfo: DecimalFormat.None,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('code', TableColumnFilterTypes.Text),
      }, {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'title',
        title: 'عنوان',
        width: 5,
        type: TableColumnDataType.Text,
        lineStyle: 'default',
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'accountReferencesGroupsTitle',
        title: 'گروه تفصیل',
        width: 4,
        type: TableColumnDataType.Text,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('accountReferencesGroupsTitle', TableColumnFilterTypes.Text),
        display: false,
        print: false,

      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'debitBeforeDate',
        title: 'بدهکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('debitBeforeDate', TableColumnFilterTypes.Money),
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmountBefore : x.debitBeforeDate;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('creditBeforeDate', 'debitBeforeDate')
        },
        groupRemainingId: 'beforeDate',
        showSum: true,
        sumColumnValue: 0,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'creditBeforeDate',
        title: 'بستانکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('creditBeforeDate', TableColumnFilterTypes.Money),
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmountBefore : x.creditBeforeDate;
        },
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('creditBeforeDate', 'debitBeforeDate')
        },
        groupRemainingId: 'beforeDate',
        showSum: true,
        sumColumnValue: 0,
      },

      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'debit',
        title: 'بدهکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmount : x.debit;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('credit', 'debit')
        },
        groupRemainingId: 'dueDate',
        showSum: true,
        sumColumnValue: 0,
      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'credit',
        title: 'بستانکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmount : x.credit;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('credit', 'debit')
        },
        groupRemainingId: 'dueDate',
        showSum: true,
        sumColumnValue: 0,
      },

      {
        ...this.defaultColumnSettings,
        index: 9,
        field: 'remainDebit',
        title: 'بدهکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('remainDebit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyRemain : x.remainDebit;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('remainCredit', 'remainDebit')
        },
        groupRemainingId: 'remain',
        showSum: true,
        sumColumnValue: 0,
      },
      {
        ...this.defaultColumnSettings,
        index: 10,
        field: 'remainCredit',
        title: 'بستانکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('remainCredit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyRemain : x.remainCredit;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('remainCredit', 'remainDebit')
        },
        groupRemainingId: 'remain',
        showSum: true,
        sumColumnValue: 0,
      },

      {
        ...this.defaultColumnSettings,
        index: 11,
        field: 'debitAfterDate',
        title: 'بدهکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('debitAfterDate', TableColumnFilterTypes.Money),
        groupName: 'بعد از دوره',
        groupId: 'afterDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmountAfter : x.debitAfterDate;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('creditAfterDate', 'debitAfterDate')
        },
        groupRemainingId: 'afterDate',
        showSum: true,
        sumColumnValue: 0,
      },
      {
        ...this.defaultColumnSettings,
        index: 12,
        field: 'creditAfterDate',
        title: 'بستانکار',
        width: 4,
        type: TableColumnDataType.Money,
        digitsInfo: DecimalFormat.Default,
        lineStyle: 'onlyShowFirstLine',
        filter: new TableColumnFilter('creditAfterDate', TableColumnFilterTypes.Money),
        groupName: 'بعد از دوره',
        groupId: 'afterDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmountAfter : x.creditAfterDate;
        },
        groupRemainingNameOrFn: () => {
          return this.calculateRemainingColumn('creditAfterDate', 'debitAfterDate')
        },
        groupRemainingId: 'afterDate',
        showSum: true,
        sumColumnValue: 0,
      },
    ]
    this.toolBar = {
      showTools: {
        tableSettings: true,
        includeOnlySelectedItemsLocal: false,
        excludeSelectedItemsLocal: false,
        includeOnlySelectedItemsFilter: false,
        excludeSelectedItemsFilter: false,
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
    await this.checkUnbalancedVoucherHeads();
    this.saveRequests = await this.requestsCacheService.getRequest(window.location.pathname);
    if (this.saveRequests) {
      this.requestsIndex = 0;
    } else {
      this.requestsIndex = -1;
    }
    await this.initialize()
  }

  async checkUnbalancedVoucherHeads() {
    let unbalancedVoucherHeadsQuery = new GetVoucherHeadsQuery()
    unbalancedVoucherHeadsQuery.conditions = [new SearchQuery({
      propertyName: 'difference',
      comparison: 'greaterThan',
      values: [0],
      nextOperand: 'or'
    }),
      new SearchQuery({
        propertyName: 'difference',
        comparison: 'lessThan',
        values: [0],
        nextOperand: 'or'
      })]
    await this.mediator.send(unbalancedVoucherHeadsQuery).then(res => {
      if (res.data?.length > 0) {
        let totalDebit = res.data.map((x: VoucherHead) => x.totalDebit).reduce((a: number, b: number) => a + b, 0);
        let totalCredit = res.data.map((x: VoucherHead) => x.totalCredit).reduce((a: number, b: number) => a + b, 0);
        let dialogConfig = new MatDialogConfig();
        let confirmDialogData = new ConfirmDialogConfig()
        confirmDialogData.color = 'red'
        confirmDialogData.title = 'اخطار'
        confirmDialogData.message = `تعداد ${res.data.length} سند غیر موازنه با جمع بدهکار ${new MoneyPipe().transform(totalDebit)} و جمع بستانکار ${new MoneyPipe().transform(totalCredit)} در گزارش مرور حساب دیده نمیشوند.`;
        confirmDialogData.actions = {
          confirm: {title: 'تایید', show: true},
          cancel: {title: '', show: false},
        }
        dialogConfig.data = confirmDialogData
        this.matDialog.open(ConfirmDialogComponent, dialogConfig)
      }
    })
  }

  async initialize(accountHeadIds?: []) {

    let query = new GetAccountReviewReportQuery();
    if (accountHeadIds) {
      query.accountHeadIds = accountHeadIds
      query.referenceIds = (<GetAccountReviewReportQuery>this.request)?.referenceIds
      //@ts-ignore
      query.level = this.request.level + 1

      query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;

      //@ts-ignore
      query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
      query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);


    } else {
      query.accountHeadIds = [];
      query.level = 2;
      query.voucherDateFrom = this.identityService.getActiveYearStartDate();
      query.voucherDateTo = this.identityService.getActiveYearEndDate();

      query.currencyTypeBaseId = this.form?.controls['currencyTypeBaseId'].value ?? undefined;
      query.reportFormat = ReportFormatTypes.None
    }
    this.yearIds = this.calculateYearIds(query.voucherDateFrom, query.voucherDateTo);
    query.yearIds = this.yearIds;
    this.request = query;
    await this.tempresolve()
  }


  async tempresolve(params?: any) {
    this.isLoading = true;
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(), new TablePaginationOptions(),
      this.toolBar, new PrintOptions('گزارش مرور حساب ها'));
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = false;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.pagination.pageSize = 500;
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'code';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';
    this.tableConfigurations.options.showGroupRow = true;
    this.tableConfigurations.options.showGroupRemainingRow = true;
    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.options.printSumRow = true;
    this.columnTypeControl.setValue(this.columnsCount[1].id)
    forkJoin([

      this.mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    ]).subscribe(async ([

                          currencyTypes]) => {
      if (this.saveRequests && this.requestsIndex == 0) {
        const temp = JSON.parse(this.saveRequests);
        if (temp.referenceIds) {
          const tempAccountReferences = this.accountManagerService.getReferenceByIds(temp.referenceIds);
          this.accountReferences.push(...tempAccountReferences ?? []);
        }

        if (temp.accountHeadIds) {
          const tempAccountHeads = this.accountManagerService.getAccountHeadByIds(temp.accountHeadIds)
          this.accountHeads.push(...tempAccountHeads);
        }
      } else {
        this.accountHeads = this.accountManagerService.accountHeads.value;
        this.accountReferences = this.accountManagerService.accountReferences.value;
      }

      this.accountReferencesGroups = this.accountManagerService.accountReferenceGroups.value;
      this.codeVoucherGroups = this.accountManagerService.codeVoucherGroups.value;
      this.currencyTypes = currencyTypes;
      this.form.patchValue({
        currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
      })
      this.isLoading = false;
      await this.get()
      await this.tempinitialize();
    })
  }

  async tempinitialize(params?: any) {
    if (this.saveRequests && this.requestsIndex == 1) {
      const temp = JSON.parse(this.saveRequests);
      this.form.patchValue({
        voucherDateFrom: temp.voucherDateFrom,
        voucherDateTo: temp.voucherDateTo,
        companyId: temp.companyId,
        reportFormat: temp.reportFormat,
        yearIds: temp.yearIds,
      })

      this.saveRequests = null;

    } else {
      this.form.patchValue({
        voucherDateFrom: this.identityService.getActiveYearStartDate(),
        voucherDateTo: this.identityService.getActiveYearEndDate(),
        companyId: +this.identityService.applicationUser.companyId,
        reportFormat: ReportFormatTypes.None
      })
      const yearIdsArray = this.form.get('yearIds') as FormArray;
      yearIdsArray.clear();
      yearIdsArray.push(new FormControl(+this.identityService.applicationUser.yearId));
    }

  }

  async changeValueData() {
    const dateTo = this.form.controls['voucherDateTo'].value;
    const dateFrom = this.form.controls['voucherDateFrom'].value;
    // @ts-ignore
    this.request.yearIds = this.calculateYearIds(dateFrom, dateTo);
    await this.get()
  }

  calculateRemainingColumn(credit: string, debit: string): string {
    const creditColumn = this.columns.find(col => col.field === credit);
    const debitColumn = this.columns.find(col => col.field === debit);
    const creditSum = creditColumn?.sumColumnValue || 0;
    const debitSum = debitColumn?.sumColumnValue || 0;
    const remainingColumn = debitSum - creditSum;
    return remainingColumn.toString();
  }

  async handleAccountHeadSelection(accountHeadId: number) {
    let accountHeads: AccountHead[] = (this.accountManagerService.accountHeads.value.filter(x => x.parentId === accountHeadId));
    if (accountHeads?.length === 0) accountHeads = [<AccountHead>this.accountManagerService.accountHeads.value.find(x => x.id === accountHeadId)]
    let query = <GetAccountReviewReportQuery>this.request;
    //@ts-ignore
    query.accountHeadIds.push(...accountHeads.map((x: AccountHead) => x.id))
    this.form.controls['accountHeadIds'].controls.push(...accountHeads.map((x: AccountHead) => x.id))
    this.form.controls['accountHeadIds'].value.push(...accountHeads.map((x: AccountHead) => x.id))

    await this.get()

  }

  async removeAllAccountHead() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.selectedAccountHeads = [];
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];
    this.accountHeadControl.setValue(null);
    await this.get()
  }

  async removeAccountHead(accountHeadId: number) {
    this.selectedAccountHeads = this.selectedAccountHeads.filter(a => a.id != accountHeadId);

    this.form.controls['accountHeadIds'].value = this.form.controls['accountHeadIds'].value.filter((a: number) => a != accountHeadId);
    //@ts-ignore
    this.request.accountHeadIds = this.request.accountHeadIds.filter((a: number) => a != accountHeadId);

  }

  async handleRowDoubleClick(row: any) {

    let res = this.accountManagerService.accountHeads.value.find(x => x.fullCode == row.code)
    await this.updateTitles(row)
    //@ts-ignore
    if (+(this.request.level) < 4) {
      let query = <GetAccountReviewReportQuery>this.request;

      if (query.level != 3) {

        let accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.parentId == res?.id)
        //@ts-ignore
        query.accountHeadIds = accountHeads.map((x: AccountHead) => x.id)
        //@ts-ignore
        query.level = this.request.level + 1


        if (row.accountReferencesGroupsCode) {
          // @ts-ignore
          query.referenceGroupIds = [this.accountManagerService.accountReferenceGroups.value.find(x => x.code == row.accountReferencesGroupsCode)?.id]
        }
        query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
        query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
        query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);

        this.request = query;

        await this.get(this.request);
      } else {
        query.accountHeadIds = [<number>res?.id]
        //@ts-ignore
        query.level = this.request.level + 1

        query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
        query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
        query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);
        //@ts-ignore
        this.request = query;

        await this.get(this.request);

        if (this.originalReportResult.length == 0) {

          await this.get(null, "back");
          //@ts-ignore
          await this.navigateToAccountLedgerReport(query.accountHeadIds, query.referenceGroupIds,
            //@ts-ignore
            null, this.request.codeVoucherGroupIds, query.voucherDateFrom, query.voucherDateTo)

        } else if (this.originalReportResult.length == 1) {
          //@ts-ignore
          await this.navigateToAccountLedgerReport(query.accountHeadIds, query.referenceGroupIds,
            //@ts-ignore
            query.referenceIds, this.request.codeVoucherGroupIds, query.voucherDateFrom, query.voucherDateTo)
        }
      }

    } else {
      //@ts-ignore
      await this.navigateToAccountLedgerReport(this.request.accountHeadIds, this.request.referenceGroupIds,
        //@ts-ignore
        this.accountManagerService.accountReferences.value.find(x => x.code == row.code)?.id, this.request.codeVoucherGroupIds, this.request.voucherDateFrom, this.request.voucherDateTo)
    }


  }

  async updateTitles(row: any) {
    // @ts-ignore
    if (this.request.level == 1 || this.request.level == 2 || this.request.level == 3) {
      let accountHeads
      //@ts-ignore
      if (this.request.level == 3) {
        accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.code === row.code);

      } else {

        accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.parentId === this.accountManagerService.accountHeads.value.find(x => x.code === row.code)?.id);
      }
      let accountHeadIds = accountHeads.map((x: AccountHead) => x.id);
      // @ts-ignore
      this.request.accountHeadIds = accountHeadIds;
      this.selectedAccountHeads = accountHeads;
      this.form.controls['accountHeadIds'].value = accountHeadIds;
    }
    //@ts-ignore
    else if (this.request.level == 4) {
      let accountReferences = this.accountManagerService.accountReferences.value.filter(x => x.code === row.code);
      let accountReferenceIds = accountReferences.map((x: AccountReference) => x.id);
      this.deleteBeforeSelected = false;
      this.selectedAccountReferences = accountReferences;
      this.form.controls['referenceIds'].value = accountReferenceIds;
      // @ts-ignore
      this.request.referenceIds = accountReferenceIds;

    }

  }

  async navigateToAccountLedgerReport(accountHeadId: number[], accountReferenceGroupId: number[], accountReferenceId: number[], codeVoucherGroupIds: number[], fromDate: Date, toDate: Date, typeId: number) {

    await this.router.navigateByUrl(`/accounting/reporting/ledgerReport2?accountHeadId=${accountHeadId}&accountReferenceGroupId=${accountReferenceGroupId}&accountReferenceId=${accountReferenceId}&codeVoucherGroupIds=${codeVoucherGroupIds}&fromDate=${this.Service.formatDate(new Date(fromDate))}&toDate=${this.Service.formatDate(new Date(toDate))}&typeId=${typeId}`)
    this.isLoading = false;
  }

  async ledgerReport() {
    //@ts-ignore
    await this.navigateToAccountLedgerReport(this.request.accountHeadIds, this.request.referenceGroupIds,
      //@ts-ignore
      this.request.referenceIds, this.request.codeVoucherGroupIds, this.request.voucherDateFrom, this.request.voucherDateTo)
  }


  async get(param?: any, action?: string) {
    this.tableConfigurations.options.isLoadingTable = true;
    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];
    let request = new GetAccountReviewReportQuery().mapFromSelf(JSON.parse(JSON.stringify(
      <GetAccountReviewReportQuery>this.request)));
    if (param) {
      request = <GetAccountReviewReportQuery>param;
    }
    if (action == 'back') {
      this.requestsList.pop();
      this.requestsIndex--;
      if (this.requestsList[this.requestsIndex]) {
        let temp = new GetAccountReviewReportQuery().mapFromSelf(JSON.parse(this.requestsList[this.requestsList.length - 1]));
        request = this.createRequest(temp);
        this.request = request;
      } else {

        await this.resetRequest();
        return;
      }
    } else if (this.saveRequests && this.requestsIndex == 0) {
      let temp = new GetAccountReviewReportQuery().mapFromSelf(JSON.parse(this.saveRequests));
      request = this.createRequest(temp);

      this.request = request;
      this.requestsIndex++;

    } else {
      const latestRequest = JSON.stringify(request);
      this.requestsList.push(latestRequest);
      await this.requestsCacheService.saveRequest(window.location.pathname, latestRequest);
      this.requestsIndex++;
    }


    this.deleteBeforeSelected = true;
    this.tableConfigurations.filters = [];
    this.tableConfigurations.options.resetFiltersLocal =true;
    this.tableConfigurations.options.isExternalChange = true;

    return await this.mediator.send(request).then((res: any) => {
      this.reportResult = res
      this.originalReportResult = res;
      this.tableConfigurations.options.isLoadingTable = false;
      this.tableConfigurations.printOptions.dateFrom = this.form.controls['voucherDateFrom'].value;
      this.tableConfigurations.printOptions.dateTo = this.form.controls['voucherDateTo'].value;
      this.updateTotalCreditAndTotalDebit(null)

    }).catch((ex) => {
      console.warn(ex);

      this.tableConfigurations.options.isLoadingTable = false;
    })

  }

  createRequest(temp: GetAccountReviewReportQuery) {
    let request = new GetAccountReviewReportQuery();
    request.reportType = temp.reportType;
    request.level = temp.level;
    request.companyId = temp.companyId;
    request.yearIds = temp.yearIds;
    request.voucherStateId = temp.voucherStateId;
    request.codeVoucherGroupId = temp.codeVoucherGroupId;
    request.transferId = temp.transferId;
    request.accountHeadIds = temp.accountHeadIds;
    request.referenceGroupIds = temp.referenceGroupIds;
    request.referenceIds = temp.referenceIds;
    request.codeVoucherGroupIds = temp.codeVoucherGroupIds;
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
    request.currencyTypeBaseId = temp.currencyTypeBaseId;
    return request;
  }

  async resetRequest() {
    this.selectedItemsFilterForPrint = [];
    this.requestsList = [];
    this.requestsIndex = -1;
    // @ts-ignore
    this.request.level = 2;
    this.form.controls['level'].setValue(2);
    await this.initialize()

    // setTimeout(() => {
    //   this.get().then();
    // }, 0);
  }

  async updateTotalCreditAndTotalDebit(row: any) {

    let items = this.reportResult;

    if (this.showCurrencyFieldsStatus) {
      items.forEach(x => {
        x.debitBeforeDate = x.debitCurrencyAmountBefore;
        x.creditBeforeDate = x.creditCurrencyAmountBefore;
        x.debit = x.debitCurrencyAmount;
        x.credit = x.creditCurrencyAmount;
        x.remainDebit = x.debitCurrencyRemain;
        x.remainCredit = x.creditCurrencyRemain;
        x.debitAfterDate = x.debitCurrencyAmountAfter;
        x.creditAfterDate = x.creditCurrencyAmountAfter;
      });

    }
  }

  async rowSelected(row: any) {


    if (row) {

      //@ts-ignore
      if (this.request.level == 1 || this.request.level == 2 || this.request.level == 3) {
        let accountHeads
        //@ts-ignore
        if (this.request.level == 3) {
          accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.code === row.code);

        } else {

          accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.parentId === this.accountManagerService.accountHeads.value.find(x => x.code === row.code)?.id);
        }
        let accountHeadIds = accountHeads.map((x: AccountHead) => x.id);
        if (row.selected == true) {
          if (!this.deleteBeforeSelected) {
            //@ts-ignore
            this.request.accountHeadIds.push(...accountHeadIds);
            this.selectedAccountHeads.push(...accountHeads);
            this.form.controls['accountHeadIds'].value.push(...accountHeadIds);

          } else {
            this.deleteBeforeSelected = false;
            //@ts-ignore
            this.request.accountHeadIds = accountHeadIds;
            this.selectedAccountHeads = accountHeads;
            this.form.controls['accountHeadIds'].value = accountHeadIds;


          }
        } else {
          for (let i = 0; i < accountHeadIds.length; i++) {
            let element = accountHeadIds[i];
            await this.removeAccountHead(element);
          }
        }
      }
      //@ts-ignore
      else if (this.request.level == 4) {
        let accountReferences = this.accountManagerService.accountReferences.value.filter(x => x.code === row.code);
        let accountReferenceIds = accountReferences.map((x: AccountReference) => x.id);
        if (row.selected == true) {

          if (!this.deleteBeforeSelected) {
            const ids = Array.isArray(row.id) ? row.id : [row.id];
            this.selectedAccountReferences.push(...accountReferences);
            this.form.controls['referenceIds'].value.push(...accountReferenceIds);

            // @ts-ignore
            this.request.referenceIds.push(...accountReferenceIds);
          } else {
            this.deleteBeforeSelected = false;
            this.selectedAccountReferences = accountReferences;
            this.form.controls['referenceIds'].value = accountReferenceIds;

            // @ts-ignore
            this.request.referenceIds = accountReferenceIds;
          }
        } else {
          await this.removeAccountReferenceWithOutGet(row.id);
        }
      }
    }


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
    return await this.mediator.send(new GetAccountHeadsQuery(0, 50, searchQueries, "fullCode"))
  }

  async getAccountHeadsInList(query?: any) {
    let searchQueries: SearchQuery[] = [];

    if (query) {
      searchQueries.push(new SearchQuery({
        propertyName: 'id',
        comparison: 'in',
        values: query,
        nextOperand: 'or'
      }))

    }

    return await this.mediator.send(new GetAccountHeadsQuery(0, 50, searchQueries, "fullCode"))
  }


  accountHeadDisplayFn(accountHeadId: number) {

    let accountHead = this.accountHeads.find(x => x.id === accountHeadId) ?? this.selectedAccountHeads.find(x => x.id === accountHeadId)
    return accountHead ? [accountHead.fullCode, accountHead.title].join(' ') : '';
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

  async getAccountReferencesInlist(query?: any) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'id',
          comparison: 'in',
          values: query,
          nextOperand: 'or'
        })
      ]
    }
    return await this.mediator.send(new GetAccountReferencesQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }

  accountReferenceDisplayFn(accountReferenceId: number) {
    // let accountReference = this.accountReferences.find(x => x.id === accountReferenceId) ?? this.selectedAccountReferences.find(x => x.id === accountReferenceId)
    // return accountReference ? [accountReference.code, accountReference.title].join(' ') : '';
    return this.accountManagerService.accountReferences.value.find(x => x.id === accountReferenceId)?.title;
  }

  async handleAccountReferenceSelection(accountReferenceId: number) {
    this.selectedAccountReferences.push(<AccountReference>this.accountReferences.find(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].controls.push(accountReferenceId)
    this.form.controls['referenceIds'].value.push(accountReferenceId);

    this.accountReferenceControl.setValue(null)
    let query = <GetAccountReviewReportQuery>this.request;
    //@ts-ignore
    query.referenceIds.push(accountReferenceId)
    await this.get()
  }

  async removeAccountReference(accountReferenceId: number) {
    this.selectedAccountReferences = this.selectedAccountReferences.filter(a => a.id != accountReferenceId);
    this.form.controls['referenceIds'].value = this.form.controls['referenceIds'].value.filter((a: number) => a != accountReferenceId);
    //@ts-ignore
    this.request.referenceIds = this.request.referenceIds.filter((a: number) => a != accountReferenceId);
    await this.get()
  }

  async removeAccountReferenceWithOutGet(accountReferenceId: number) {
    this.selectedAccountReferences = this.selectedAccountReferences.filter(a => a.id != accountReferenceId);
    this.form.controls['referenceIds'].value = this.form.controls['referenceIds'].value.filter((a: number) => a != accountReferenceId);
    //@ts-ignore
    this.request.referenceIds = this.request.referenceIds.filter((a: number) => a != accountReferenceId);
    // await this.get()
  }

  async removeAllAccountRefrence() {
    this.selectedAccountReferences = [];
    this.form.controls['referenceIds'].value = [];
    this.form.controls['referenceIds'].controls = [];
    // @ts-ignore
    this.request.referenceIds = [];
    this.accountReferenceControl.setValue(null);

    await this.get()
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
    return await this.mediator.send(new GetAccountReferencesGroupsQuery(1, 60, searchQueries, "code")).then(res => res.data)
  }

  accountReferencesGroupDisplayFn(accountReferencesGroupId: number) {
    // let accountReferencesGroup = this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId) ?? this.selectedAccountReferencesGroups.find(x => x.id === accountReferencesGroupId)
    //
    // return accountReferencesGroup ? [accountReferencesGroup.code, accountReferencesGroup.title].join(' ') : '';
    return this.accountManagerService.accountReferenceGroups.value.find(x => x.id === accountReferencesGroupId)?.title;
  }

  async handleAccountReferencesGroupSelection(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId))

    this.form.controls['referenceGroupIds'].controls.push(accountReferencesGroupId)
    this.form.controls['referenceGroupIds'].value.push(accountReferencesGroupId)
    this.accountReferencesGroupControl.setValue(null)
    let query = <GetAccountReviewReportQuery>this.request;
    //@ts-ignore
    query.referenceGroupIds.push(accountReferencesGroupId)

    await this.get()

  }

  async removeAccountReferencesGroup(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups = this.selectedAccountReferencesGroups.filter(a => a.id != accountReferencesGroupId);
    this.form.controls['referenceGroupIds'].value = this.form.controls['referenceGroupIds'].value.filter((a: number) => a != accountReferencesGroupId);
    //@ts-ignore
    this.request.referenceGroupIds = this.request.referenceGroupIds.filter((a: number) => a != accountHeadId);

  }

  async removeAllReferenceGroup() {
    this.selectedAccountReferencesGroups = [];
    this.form.controls['referenceGroupIds'].value = [];
    this.form.controls['referenceGroupIds'].controls = [];
    // @ts-ignore
    this.request.referenceGroupIds = [];
    this.accountReferencesGroupControl.setValue(null);
    await this.get()
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
    return await this.mediator.send(new GetCodeVoucherGroupsQuery(1, 50, searchQueries)).then(res => res.data)
  }

  async handleCodeVoucherGroupSelection(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.push(<CodeVoucherGroup>this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId));
    this.form.controls['codeVoucherGroupIds'].controls.push(new FormControl(codeVoucherGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.codeVoucherGroupControl.setValue(null)
    // await this.get()
  }

  codeVoucherGroupDisplayFn(codeVoucherGroupId: number) {
    let codeVoucherGroup = this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId) ?? this.selectedCodeVoucherGroups.find(x => x.id === codeVoucherGroupId);
    return codeVoucherGroup ? [codeVoucherGroup.code, codeVoucherGroup.title].join(' ') : '';
  }

  async showCurrencyRelatedFields() {

    this.showCurrencyFieldsStatus = this.currencyTypes.find(x => x.id == this.form.controls['currencyTypeBaseId'].value)?.uniqueName !== 'IRR';
    // @ts-ignore
    this.request.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;

    await this.get()
  }

  async removeCodeVoucherGroup(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups = this.selectedCodeVoucherGroups.filter(a => a.id != codeVoucherGroupId);
    this.form.controls['codeVoucherGroupIds'].value = this.form.controls['codeVoucherGroupIds'].value.filter((a: number) => a != codeVoucherGroupId);
    //@ts-ignore
    this.request.codeVoucherGroupIds = this.request.codeVoucherGroupIds.filter((a: number) => a != codeVoucherGroupId);

  }


  async changeColumns() {
    let id = this.columnTypeControl.value;
    if (id == 1) {

      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].print = false;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].print = false;

      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].print = false;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].print = false;

      this.selectedColumn = 4;

    } else if (id == 2) {

      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].print = true;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].print = true;

      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].print = false;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].display = false;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].print = false;
      this.selectedColumn = 6;

    } else {
      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "debitBeforeDate")[0].print = true;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "creditBeforeDate")[0].print = true;

      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "creditAfterDate")[0].print = true;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].display = true;
      this.tableConfigurations.columns.filter(a => a.field == "debitAfterDate")[0].print = true;
      this.selectedColumn = 8;


    }
    this.tableConfigurations.columns.filter(a => a.field == "remainDebit")[0].display = true;
    this.tableConfigurations.columns.filter(a => a.field == "remainDebit")[0].print = true;
    this.tableConfigurations.columns.filter(a => a.field == "remainCredit")[0].display = true;
    this.tableConfigurations.columns.filter(a => a.field == "remainCredit")[0].print = true;
    this.tableConfigurations.columns.filter(a => a.field == "debit")[0].display = true;
    this.tableConfigurations.columns.filter(a => a.field == "debit")[0].print = true;
    this.tableConfigurations.columns.filter(a => a.field == "credit")[0].display = true;
    this.tableConfigurations.columns.filter(a => a.field == "credit")[0].print = true;

    this.tableConfigurations = new TableScrollingConfigurations(
      this.tableConfigurations.columns,
      this.tableConfigurations.options,
      this.tableConfigurations.pagination,
      this.tableConfigurations.toolBar,
      this.tableConfigurations.printOptions,
    );
    await this.save()
  }

  async save(): Promise<void> {

    this.tableConfigurations.options.isLoadingTable = false;
    const updatedColumns = this.columns.map(column => {
      const {
        sumRowDisplayFn,
        filterOptionsFunction,
        displayFunction,
        displayFn,
        filterOptionsFn,
        asyncOptions,
        groupRemainingNameOrFn,
        filter,
        ...rest
      } = column;

      return rest;
    });

    this.tableConfigurations.columns = this.tableConfigurations.columns.map(existingColumn => {
      const updatedColumn = updatedColumns.find(updated => updated.field === existingColumn.field);
      return updatedColumn ? {...existingColumn, ...updatedColumn} : existingColumn;
    });

    const cols = this.tableConfigurations.columns
    this.tableConfigurations.options.hasDefaultSortKey = true;
    const options = this.tableConfigurations.options
    await this.tableSettingsService.saveSettings(window.location.pathname, cols, options);
  }

  async changeLevel(level: number) {
    //@ts-ignore
    this.request.level = level;
    this.updateDate();
    this.form.patchValue({
      level: level
    })
    await this.get();
  }

  updateDate() {
    let voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
    let voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);
    this.yearIds = this.calculateYearIds(voucherDateFrom, voucherDateTo);
    //@ts-ignore
    this.request.voucherDateFrom = voucherDateFrom
    //@ts-ignore
    this.request.voucherDateTo = voucherDateTo;
    const yearIdsArray = this.form.get('yearIds') as FormArray;
    yearIdsArray.clear();
    this.yearIds.forEach(yearId => {

      yearIdsArray.push(new FormControl(yearId));
    })
    //@ts-ignore
    this.request.yearIds = this.yearIds;

  }

  async changeLevelControl() {

    await this.changeLevel(this.form.controls['level'].value)

  }

  async removeAllFilters() {
    this.selectedCodeVoucherGroups = [];
    this.form.controls['codeVoucherGroupIds'].value = [];
    this.form.controls['codeVoucherGroupIds'].controls = [];
    // @ts-ignore
    this.request.codeVoucherGroupIds = [];
    this.codeVoucherGroupControl.setValue(null);
    this.selectedAccountReferences = [];
    this.form.controls['referenceIds'].value = [];
    this.form.controls['referenceIds'].controls = [];
    // @ts-ignore
    this.request.referenceIds = [];
    this.accountReferenceControl.setValue(null);


    this.selectedAccountReferencesGroups = [];
    this.form.controls['referenceGroupIds'].value = [];
    this.form.controls['referenceGroupIds'].controls = [];
    // @ts-ignore
    this.request.referenceGroupIds = [];
    this.accountReferencesGroupControl.setValue(null);

    // @ts-ignore
    this.request.accountHeadIds = []
    this.selectedAccountHeads = [];
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];
    this.accountHeadControl.setValue(null);

    // @ts-ignore
    this.request.voucherNoFrom = undefined;
    // @ts-ignore
    this.request.voucherNoTo = undefined;
    // @ts-ignore
    // this.request.level = 2;
    this.form.controls['level'].setValue(2);
    this.form.controls['voucherNoFrom'].value = '';
    this.form.controls['voucherNoTo'].value = '';

    this.form.controls['creditFrom'].value = '';
    this.form.controls['creditTo'].value = '';
    this.form.controls['debitFrom'].value = '';
    this.form.controls['debitTo'].value = '';
    this.form.controls['voucherRowDescription'].value = '';

    this.form.patchValue({
      currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
      voucherDateFrom: this.identityService.getActiveYearStartDate(),
      voucherDateTo: this.identityService.getActiveYearEndDate(),
    })

    // @ts-ignore
    this.request.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
    this.showCurrencyFieldsStatus = false;
    await this.get();
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }

  handleOptionSelected($event: any) {

  }

  async handleRestorePreviousFilter() {
    this.selectedItemsFilterForPrint = [];
    await this.get(null, 'back')
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {
    this.selectedItemsFilterForPrint = [];
    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.requestsIndex = -1
    this.requestsList = [];
    this.saveRequests = null;
    await this.removeAllFilters()
  }

  handleTableConfigurationsChange(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.columns = config.columns;
    this.tableConfigurations.options = config.options;
    const debitBeforeDate = config.columns.find(col => col.field === 'debitBeforeDate');
    const creditBeforeDate = config.columns.find(col => col.field === 'creditBeforeDate');
    const creditAfterDate = config.columns.find(col => col.field === 'creditAfterDate');
    const debitAfterDate = config.columns.find(col => col.field === 'debitAfterDate');
    if (!debitBeforeDate?.display && !creditBeforeDate?.display && !creditAfterDate?.display && !debitAfterDate?.display) {
      this.selectedColumn = 4;
      this.columnTypeControl.setValue(1);
    } else if (debitBeforeDate?.display && creditBeforeDate?.display && !creditAfterDate?.display && !debitAfterDate?.display) {
      this.selectedColumn = 6;
      this.columnTypeControl.setValue(2);
    } else if (debitBeforeDate?.display && creditBeforeDate?.display && creditAfterDate?.display && debitAfterDate?.display) {
      this.selectedColumn = 8;
      this.columnTypeControl.setValue(3);
    }
  }

  handleExcludeSelectedItemsEvent($event: any) {
  }

  async handleSelectedItemsFilterForPrint(ids: number[]) {
    this.selectedItemsFilterForPrint = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'in',
      nextOperand: 'and'
    })];
  }


  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private calculateYearIds(DateFrom: Date, DateTo: Date) {
    return this.identityService.findYearIdsByDates(DateFrom, DateTo);
  }

  async getCentralBankReport() {
    this.isLoading = true;

    let request = new GetCentralBankReportQuery().mapFrom(this.request);

    await this.mediator.send(request).then(result => {
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
      a.download = 'گزارش بانک مرکزی.xlsx'; // Set the file name with .xlsx extension
      a.click();

      // Clean up and release the object URL
      window.URL.revokeObjectURL(url);
      this.isLoading = false;
    }).catch(() => {
      this.isLoading = false;
    })
  }
}
