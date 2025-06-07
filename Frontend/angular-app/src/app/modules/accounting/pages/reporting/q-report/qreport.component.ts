import {BaseComponent} from 'src/app/core/abstraction/base.component';
import {Component, ViewChild} from '@angular/core';
import {Mediator} from "src/app/core/services/mediator/mediator.service";
import {Router} from "@angular/router";
import {TableColumnFilter} from "src/app/core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "src/app/core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "src/app/core/components/custom/table/models/table-column-filter-types";
import {TableColumn} from "src/app/core/components/custom/table/models/table-column";
import {SearchQuery} from 'src/app/shared/services/search/models/search-query';
import {Action, ActionTypes} from 'src/app/core/components/custom/action-bar/action-bar.component';
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {IdentityService} from 'src/app/modules/identity/repositories/identity.service';
import {
  GetAccountReviewReportQuery
} from 'src/app/modules/accounting/repositories/reporting/queries/get-account-review-report-query';
import {FormControl} from '@angular/forms';
import {BaseValue} from 'src/app/modules/admin/entities/base-value';
import {TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import {CodeVoucherGroup} from 'src/app/modules/accounting/entities/code-voucher-group';
import {AccountReferencesGroup} from 'src/app/modules/accounting/entities/account-references-group';
import {AccountReference} from 'src/app/modules/accounting/entities/account-reference';
import {AccountHead} from 'src/app/modules/accounting/entities/account-head';
import {UserYear} from 'src/app/modules/identity/repositories/models/user-year';
import {
  AccountReviewReportResultModel
} from 'src/app/modules/accounting/repositories/account-review/account-review-report-result-model';
import {TableOptions} from 'src/app/core/components/custom/table/models/table-options';
import {forkJoin} from 'rxjs';
import {
  GetBaseValuesByUniqueNameQuery
} from 'src/app/modules/admin/repositories/base-value/queries/get-base-values-by-unique-name-query';

import {
  GetCodeVoucherGroupsQuery
} from "src/app/modules/accounting/repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import {
  GetAccountReferencesQuery
} from "src/app/modules/accounting/repositories/account-reference/queries/get-account-references-query";
import {
  GetAccountHeadsQuery
} from "src/app/modules/accounting/repositories/account-head/queries/get-account-heads-query";

import {ReportFormatTypes} from "src/app/modules/accounting/repositories/reporting/ReportFormatTypes";
import {
  GetAccountReferencesGroupsQuery
} from "src/app/modules/accounting/repositories/account-reference-group/queries/get-account-references-groups-query";
import {AccountingMoneyPipe} from 'src/app/core/pipes/accounting-money.pipe';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';
import {environment} from "../../../../../../environments/environment";
import {AccountManagerService} from "../../../services/account-manager.service";
import {GetLedgerReportQuery} from "../../../repositories/reporting/queries/get-ledger-report-query";
import {RequestsCacheService} from "../../../../../shared/services/RequestsCache/requests-cache.service";
import moment from "jalali-moment";


@Component({
  selector: 'app-qreport',
  templateUrl: './qreport.component.html',
  styleUrls: ['./qreport.component.scss']
})
export class QReportComponent extends BaseComponent {


  columns: TableColumn[] = [];

  applicationUserFullName!: string;
  //showCurrencyFieldsStatus: boolean = false;
  selectedTabIndex: number = 0;

  reportResult: AccountReviewReportResultModel[] = []
  originalReportResult: AccountReviewReportResultModel[] = []
  accountLevels: any[] = []
  accountLevelSelected: number = 0;
  voucherStates: any[] = []
  accountTypes: any[] = []
  reportTypes: any[] = []
  //
  saveRequests: any;
  requestsIndex: number = -1;
  //userAllowedYears: UserYear[] = [];

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

  tableConfigurations!: TableConfigurations;
  showCurrencyRelatedFieldsStatus: boolean = false;
  totalDebit = new FormControl();
  totalCredit = new FormControl();
  currencyTypes: BaseValue[] = [];

  constructor(private Mediator: Mediator,
              private router: Router, public Service: PagesCommonService, public identityService: IdentityService,
              private accountManagerService: AccountManagerService,
              private requestsCacheService: RequestsCacheService,
  ) {
    super();
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
      }
    });
  }

  async ngOnInit() {
    await this.resolve();
    await this.tempresolve();
  }


  async resolve(params?: any) {
    this.columns = [

      <TableColumn>{
        name: 'selected',
        title: '',
        type: TableColumnDataType.Select,
        sortable: true,
      },
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        sortable: true,
      },
      <TableColumn>{
        name: 'code',
        title: 'کد',
        type: TableColumnDataType.Text,
        sortable: true
      },
      <TableColumn>{
        name: 'title',
        title: 'عنوان',
        type: TableColumnDataType.Text,
        sortable: true,
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text)
      }, <TableColumn>{
        name: 'debitBeforeDate',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debitBeforeDate', TableColumnFilterTypes.Money),
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        show: true
      },
      <TableColumn>{
        name: 'creditBeforeDate',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('creditBeforeDate', TableColumnFilterTypes.Money),
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        show: true
      },
      <TableColumn>{
        name: 'debit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        // displayFn: (x: any) => {
        //   return this.showCurrencyFieldsStatus ? x.debitCurrencyAmount : x.debit;
        // }
      },
      <TableColumn>{
        name: 'credit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        // displayFn: (x: any) => {
        //   return this.showCurrencyFieldsStatus ? x.creditCurrencyAmount : x.credit;
        // }
      },
      <TableColumn>{
        name: 'remainDebit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('remainDebit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        // displayFn: (x: any) => {
        //   return this.showCurrencyFieldsStatus ? x.debitCurrencyRemain : x.remainDebit;
        // }
      },
      <TableColumn>{
        name: 'remainCredit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('remainCredit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        // displayFn: (x: any) => {
        //   return this.showCurrencyFieldsStatus ? x.creditCurrencyRemain : x.remainCredit;
        // }
      }
    ]

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


    await this.initialize()

  }

  async initialize(accountHeadIds?: []) {
    let query = new GetAccountReviewReportQuery();
    if (accountHeadIds) {
      query = <GetAccountReviewReportQuery>this.request
      query.accountHeadIds = accountHeadIds
      //query.referenceIds = (<GetAccountReviewReportQuery>this.request)?.referenceIds
      //@ts-ignore
      query.level = this.request.level + 1
      //query.level = 3;
      query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
      query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);

      query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);

      // query.currencyTypeBaseId = this.form.getRawValue()?.currencyTypeBaseId;
      // query.voucherDateFrom = this?.form?.getRawValue()?.voucherDateFrom ?? undefined;
      // query.voucherDateTo = this?.form?.getRawValue()?.voucherDateTo ?? undefined;
      //@ts-ignore
      this.request = query;
      await this.tempresolve()
    } else {
      query.accountHeadIds = [];
      query.level = 4;
      query.currencyTypeBaseId = this?.form?.getRawValue()?.currencyTypeBaseId ?? undefined;

      this.request = query;
    }
  }

  async tempresolve() {
    this.isLoading = true;


    this.tableConfigurations = new TableConfigurations(this.columns, new TableOptions())
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'code';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';


    //this.identityService.getUserAllowedYears().subscribe(a => this.userAllowedYears = a.objResult);
    let accountHeads = await this.getAccountHeads();
    let accountReferences = await this.getAccountReferences();
    let accountReferencesGroups = await this.getAccountReferencesGroups();
    let codeVoucherGroups = await this.getCodeVoucherGroups();
    let currencyTypes = await this.Mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'));

    this.accountHeads = accountHeads;
    this.accountReferences = accountReferences;
    this.accountReferencesGroups = accountReferencesGroups;
    this.codeVoucherGroups = codeVoucherGroups;
    this.currencyTypes = currencyTypes;
    this.form.patchValue({
      currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
    })


    await this.tempinitialize()
    this.isLoading = false;
    await this.get()

    // await forkJoin([
    //   await this.identityService.getUserAllowedYears(),
    //   await this.getAccountHeads(),
    //   await this.getAccountReferences(),
    //   await this.getAccountReferencesGroups(),
    //   await this.getCodeVoucherGroups(),
    //   await this.Mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    // ]).subscribe(async ([
    //   userAllowedYears,
    //   accountHeads,
    //   accountReferences,
    //   accountReferencesGroups,
    //   codeVoucherGroups,
    //   currencyTypes]) => {
    //   this.userAllowedYears = userAllowedYears.objResult;
    //   this.accountHeads = accountHeads;
    //   this.accountReferences = accountReferences;
    //   this.accountReferencesGroups = accountReferencesGroups;
    //   this.codeVoucherGroups = codeVoucherGroups;
    //   this.currencyTypes = currencyTypes;
    //   this.form.patchValue({
    //     currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
    //   })


    //   await this.tempinitialize()


    //   this.isLoading = false;
    //   await this.get()
    // })
  }


  async tempinitialize() {
    // let currentUserYear = this.userAllowedYears.find(x => x.id == this.identityService.applicationUser.yearId);
    // if (currentUserYear) currentUserYear._selected = true;

    this.form.patchValue({
      voucherDateFrom: this.identityService.getActiveYearStartDate(),
      voucherDateTo: this.identityService.getActiveYearEndDate(),
      companyId: +this.identityService.applicationUser.companyId,
      reportFormat: ReportFormatTypes.None
    })
    this.form.controls['yearIds'].controls.push(new FormControl(+this.identityService.applicationUser.yearId))
    this.form.patchValue(this.form.getRawValue())
    this.saveRequests = await this.requestsCacheService.getRequest(window.location.pathname);
    if (this.saveRequests && this.requestsIndex == -1) {
      const temp = JSON.parse(this.saveRequests);
      this.form.controls['referenceIds'].controls.push(new FormControl(temp.referenceIds[0]));


      this.accountReferenceControl.setValue(temp.referenceIds);
      this.accountReferencesGroupControl.setValue(temp.referenceGroupIds);
      this.accountHeadControl.setValue(temp.accountHeadIds);
      this.form.patchValue(this.form.getRawValue())

      if (temp.referenceIds) {
        this.accountReferences = await this.getAccountReferencesId(temp.referenceIds[0]);
      }
    }

    this.accountHeadControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountHeads = await this.getAccountHeads(newValue);
    })
    this.accountReferenceControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountReferences = await this.getAccountReferences(newValue);
    })
    this.accountReferencesGroupControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountReferencesGroups = await this.getAccountReferencesGroups(newValue);
    })
    this.codeVoucherGroupControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.codeVoucherGroups = await this.getCodeVoucherGroups(newValue);
      if (!newValue) this.form.controls['codeVoucherGroupId'].setValue(null)
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
    // searchQueries.push(new SearchQuery({
    //   propertyName: 'lastLevel',
    //   comparison: 'equal',
    //   values: [true],
    //   nextOperand: 'and'
    // }))

    return await this.Mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, "fullCode"));
  }

  updateTotalCreditAndTotalDebit() {
    //let selectedItems = this.reportResult.filter(x => x.selected);
    //let items = selectedItems.length > 0 ? selectedItems : this.reportResult;
    let items = this.reportResult;

    if (this.showCurrencyRelatedFieldsStatus) {
      items.forEach(x => {

        x.debit = x.debitCurrencyAmount;
        x.credit = x.creditCurrencyAmount;
        x.remainDebit = x.debitCurrencyRemain;
        x.remainCredit = x.creditCurrencyRemain;
      });

    }
  }

  writeReportToIframe() {

    let iframe = <any>document.getElementById('iframe');
    let content = this.reportResult;
    let doc = iframe.contentDocument || iframe.contentWindow;
    doc.open();
    doc.write(content);
    doc.close();
  }


  resetAccountHeadIdsFilter() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.form.controls['accountHeadIds'].value = [];

  }

  accountHeadDisplayFn(accountHeadId: number) {
    let accountHead = this.accountHeads.find(x => x.id === accountHeadId) ?? this.selectedAccountHeads.find(x => x.id === accountHeadId)
    return accountHead ? [accountHead.fullCode, accountHead.title].join(' ') : '';
  }

  async handleAccountHeadSelection(accountHeadId: number) {

    let accountHeads = await this.getAccountHeadChildren(<AccountHead>this.accountHeads.find(x => x.id === accountHeadId));
    let query = <GetAccountReviewReportQuery>this.request;


    //@ts-ignore
    query.accountHeadIds = accountHeads.map((x: AccountHead) => x.id)
    //@ts-ignore
    this.request = query;


    // @ts-ignore
    // this.request.accountHeadIds = []
    // this.selectedAccountHeads = [];
    // this.form.controls['accountHeadIds'].value = [];
    // this.form.controls['accountHeadIds'].controls = [];

    // this.selectedAccountHeads = accountHeads;
    // this.form.controls['accountHeadIds'].controls.push(new FormControl(accountHeadId))
    // this.form.patchValue(this.form.getRawValue())
  }

  removeAccountHead(accountHeadId: number) {
    this.selectedAccountHeads.slice(this.selectedAccountHeads.findIndex(x => x.id === accountHeadId))
    this.form.controls['accountHeadIds'].removeAt(this.form.controls['accountHeadIds'].value.findIndex((x: any) => x === accountHeadId))
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
    return await this.Mediator.send(new GetAccountReferencesQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }

  async getAccountReferencesId(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [

        new SearchQuery({
          propertyName: 'id',
          comparison: 'equal',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }
    return await this.Mediator.send(new GetAccountReferencesQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }

  accountReferenceDisplayFn(accountReferenceId: number) {

    let accountReference = this.accountReferences.find(x => x.id === accountReferenceId) ?? this.selectedAccountReferences.find(x => x.id === accountReferenceId)

    return accountReference ? [accountReference.code, accountReference.title].join(' ') : '';
  }

  handleAccountReferenceSelection(accountReferenceId: number) {

    this.selectedAccountReferences = [];
    this.form.controls['referenceIds'].value = [];
    this.form.controls['referenceIds'].controls = [];
    // @ts-ignore
    this.request.referenceIds = []


    this.selectedAccountReferences.push(<AccountReference>this.accountReferences.find(x => x.id === accountReferenceId))

    this.form.controls['referenceIds'].controls.push(new FormControl(accountReferenceId));

    this.form.patchValue(this.form.getRawValue())
    //this.accountReferenceControl.setValue(null);
  }

  removeAccountReference(accountReferenceId: number) {
    this.selectedAccountReferences.slice(this.selectedAccountReferences.findIndex(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].removeAt(this.form.controls['referenceIds'].value.findIndex((x: any) => x === accountReferenceId))
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
    return await this.Mediator.send(new GetAccountReferencesGroupsQuery(1, 50, searchQueries, "code")).then(res => res.data)
  }

  accountReferencesGroupDisplayFn(accountReferencesGroupId: number) {
    let accountReferencesGroup = this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId) ?? this.selectedAccountReferencesGroups.find(x => x.id === accountReferencesGroupId)

    return accountReferencesGroup ? [accountReferencesGroup.code, accountReferencesGroup.title].join(' ') : '';
  }

  handleAccountReferencesGroupSelection(accountReferencesGroupId: number) {

    this.selectedAccountReferencesGroups = [];
    this.form.controls['referenceGroupIds'].value = [];
    this.form.controls['referenceGroupIds'].controls = [];
    // @ts-ignore
    this.request.referenceGroupIds = [];

    this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountReferencesGroups.find(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].controls.push(new FormControl(accountReferencesGroupId))
    this.form.patchValue(this.form.getRawValue())
    //this.accountReferencesGroupControl.setValue(null);
  }

  removeAccountReferencesGroup(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.slice(this.selectedAccountReferencesGroups.findIndex(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].removeAt(this.form.controls['referenceGroupIds'].value.findIndex((x: any) => x === accountReferencesGroupId))
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
    return await this.Mediator.send(new GetCodeVoucherGroupsQuery(1, 50, searchQueries)).then(res => res.data)
  }

  handleCodeVoucherGroupSelection(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.push(<CodeVoucherGroup>this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId));
    this.form.controls['codeVoucherGroupIds'].controls.push(new FormControl(codeVoucherGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.codeVoucherGroupControl.setValue(null)
  }

  codeVoucherGroupDisplayFn(codeVoucherGroupId: number) {
    let codeVoucherGroup = this.codeVoucherGroups.find(x => x.id === codeVoucherGroupId) ?? this.selectedCodeVoucherGroups.find(x => x.id === codeVoucherGroupId);
    return codeVoucherGroup ? [codeVoucherGroup.code, codeVoucherGroup.title].join(' ') : '';
  }

  async showCurrencyRelatedFields() {

    this.showCurrencyRelatedFieldsStatus = this.currencyTypes.find(x => x.id == this.form.getRawValue().currencyTypeBaseId)?.uniqueName !== 'IRR';
    this.reportResult = this.originalReportResult.filter((x: any) => ((

        x.debit ||
        x.credit ||
        x.remainDebit ||
        x.remainCredit
      ) && !this.showCurrencyRelatedFieldsStatus) ||
      ((
        x.debitCurrencyAmountBefore ||
        x.creditCurrencyAmountBefore ||
        x.debitCurrencyAmount ||
        x.creditCurrencyAmount ||
        x.debitCurrencyRemain ||
        x.creditCurrencyRemain ||
        x.debitCurrencyAmountAfter ||
        x.creditCurrencyAmountAfter
      ) && this.showCurrencyRelatedFieldsStatus));
    await this.get()
  }

  removeCodeVoucherGroup(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.slice(this.selectedCodeVoucherGroups.findIndex(x => x.id === codeVoucherGroupId))
    this.form.controls["codeVoucherGroupIds"].removeAt(this.form.controls['codeVoucherGroupIds'].value.findIndex((x: any) => x === codeVoucherGroupId))
  }

  removeAllFilters() {
    this.selectedCodeVoucherGroups = [];
    this.form.controls['codeVoucherGroupIds'].value = [];
    this.form.controls['codeVoucherGroupIds'].controls = [];


    this.form.controls['voucherDateFrom'].setValue(this.identityService.getActiveYearStartDate());
    this.form.controls['voucherDateTo'].setValue(this.identityService.getActiveYearEndDate());

    // currencyTypeBaseId
    this.form.controls['currencyTypeBaseId'].setValue(this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id);


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

    // this.form.controls['creditFrom'].value = '';
    // this.form.controls['creditTo'].value = '';
    // this.form.controls['debitFrom'].value = '';
    // this.form.controls['debitTo'].value = '';
    // this.form.controls['voucherRowDescription'].value = '';
  }

  async handleRowDoubleClick(row: any) {

    let res = <AccountHead>this.accountManagerService.accountHeads.value.find(x => x.fullCode == row.code)

    let request = <GetAccountReviewReportQuery>this.request;
    // @ts-ignore
    if (request.level < 4) {
      if (!res.lastLevel) {
        //@ts-ignore
        request.level = request.level + 1;
        request.accountHeadIds = [];
        request.accountHeadIds.push((<AccountHead>this.accountManagerService.accountHeads.value.find(x => x.code === row.code)).id)
        this.request = request;
        await this.get();
      } else {
        //@ts-ignore
        await this.navigateToAccountLedgerReport((<AccountHead>this.accountManagerService.accountHeads.value.find(x => x.code === row.code)).id, this.request.referenceGroupIds[0],
          //@ts-ignore
          this.request.referenceIds[0], this.request.voucherDateFrom, this.request.voucherDateTo)
      }

    } else {

      //@ts-ignore
      this.navigateToAccountLedgerReport(this.request.accountHeadIds[0], this.request.referenceGroupIds[0],
        //@ts-ignore
        this.accountManagerService.accountReferences.value.find(x => x.code == row.code)?.id, this.request.voucherDateFrom, this.request.voucherDateTo)
    }

  }

  async navigateToAccountLedgerReport(accountHeadId: number, accountReferenceGroupId: number, accountReferenceId: number, fromDate: Date, toDate: Date, typeId: number) {
    await this.router.navigateByUrl(`/accounting/reporting/ledgerReport2?accountHeadId=${accountHeadId}&accountReferenceGroupId=${accountReferenceGroupId}&accountReferenceId=${accountReferenceId}&fromDate=${this.Service.formatDate(new Date(fromDate))}&toDate=${this.Service.formatDate(new Date(toDate))}&typeId=${typeId}`)
  }

  async getAccountHeadChildren(accountHead: AccountHead) {
    let searchQueries = [
      new SearchQuery({
        comparison: 'equals',
        propertyName: 'lastLevel',
        values: [true],
        nextOperand: 'and'
      }),
      new SearchQuery({
        comparison: 'startsWith',
        propertyName: 'fullCode',
        values: [accountHead.fullCode],
        nextOperand: 'and'
      }),

    ]
    return await this.Mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, "fullCode")).then(res => {
      return res
    })
  }

  async getAccountHeadById(id: number) {
    let searchQuery = new SearchQuery({
      comparison: 'equals',
      propertyName: 'id',
      values: [id],
      nextOperand: 'or'
    });

    return await this.Mediator.send(new GetAccountHeadsQuery(1, 1, [searchQuery], "fullCode")).then(res => {
      return res[0]
    })
  }

  async getDataFromServer() {
    let pageSize = 0;
    let pageIndex = 0;
    let query = <GetAccountReviewReportQuery>this.request;
    query.pageSize = pageSize;
    query.pageIndex = pageIndex;
    query.yearIds = [+this.identityService.applicationUser.yearId]

    let responce = await this.Mediator.send(query);
    let result = responce;
    return result;
  }

  async printRial() {
    let query = <GetAccountReviewReportQuery>this.request;
    let voucherDateTo = new Date()
    if (query.voucherDateTo) {
      const originalDate = new Date(query.voucherDateTo);
      const jalaliMoment = moment(originalDate).locale('fa');
      const jalaliYear = jalaliMoment.jYear();
      const month = jalaliMoment.jMonth() + 1; // 1-12
      const day = jalaliMoment.jDate();
      debugger
      // لیست سال‌های کبیسه بر اساس تقویم رسمی ایران (ولی فقط سال 1403 مشکل داره)
      const leapYears = [ 1403]; // می‌توانید لیست را کامل کنید
      const isLeapYear = leapYears.includes(jalaliYear);
      // اگر 30 اسفند در سال کبیسه باشد، 24 ساعت اضافه نکن
      const is30EsfandInLeapYear = (month === 12 && day === 30 && isLeapYear);
      // اگر سال کبیسه نیست یا تاریخ 30 اسفند در سال کبیسه نیست، 24 ساعت اضافه کن
      if (!is30EsfandInLeapYear) {
        voucherDateTo = this.addHours(new Date(query.voucherDateTo), 24);
      } else {
        voucherDateTo = new Date(query.voucherDateTo);
      }
    }

    let voucherDateFrom = new Date()
    if (query.voucherDateFrom) {
      voucherDateFrom = new Date(query.voucherDateFrom);
    }
    let token = this.identityService.getToken();
    const params = new URLSearchParams();
    if (token != null) {
      params.append('access_token', token);
    }
    params.append('Column', '6');
    if (query.accountHeadIds) { // @ts-ignore
      params.append('AccountHeadIds', query.accountHeadIds);
    }
    if (query.codeVoucherGroupIds) { // @ts-ignore
      params.append('CodeVoucherGroupIds', query.codeVoucherGroupIds);
    }
    if (query.currencyTypeBaseId) params.append('CurrencyTypeBaseId', query.currencyTypeBaseId.toString());
    if (query.level) params.append('Level', query.level.toString());
    if (query.referenceGroupIds) { // @ts-ignore
      params.append('ReferenceGroupIds', query.referenceGroupIds);
    }
    if (query.referenceIds) { // @ts-ignore
      params.append('ReferenceIds', query.referenceIds);
    }
    if (query.voucherDateFrom) params.append('fromDate', voucherDateFrom.toISOString());
    if (query.voucherDateTo) params.append('toDate', voucherDateTo.toISOString());

    if (query.voucherNoFrom) params.append('VoucherNoFrom', String(query.voucherNoFrom));
    if (query.voucherNoTo) params.append('VoucherNoTo', String(query.voucherNoTo));

    window.open(`${environment.apiURL}/accountingreports/AccountReviewReport/index?${params.toString()}`, "_blank");
  }

  addHours = (date: any, hours: number): Date => {
    const result = date;
    result.setHours(result.getHours() + hours, 0, -1);
    return result;
  };


  async getExcel() {
    let a = this.request;
    // @ts-ignore
    a.reportFormat = ReportFormatTypes.Excel;

    return await this.Mediator.send(a).then(res => {
      let binaryData = [];
      binaryData.push(res);
      let blob = new Blob(binaryData, {type: res.type});

      let a = document.createElement("a");
      document.body.appendChild(a);

      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = 'filename.xlsx';
      a.click();

      setTimeout(function () {
        window.URL.revokeObjectURL(url);
      }, 100);
    })
  }

  async exportFile(format: string) {
    let request = <GetAccountReviewReportQuery>this.request;
    // @ts-ignore
    if (format === 'pdf') request.reportFormat = ReportFormatTypes.Pdf;
    // @ts-ignore
    if (format === 'excel') request.reportFormat = ReportFormatTypes.Excel;
    // @ts-ignore
    if (format === 'word') request.reportFormat = ReportFormatTypes.Word;

    return await this.Mediator.send(request).then((res: any) => {
      let binaryData = [];
      binaryData.push(res);
      let blob = new Blob(binaryData, {type: res.type});

      let a = document.createElement("a");
      document.body.appendChild(a);

      let url = window.URL.createObjectURL(blob);
      a.href = url;

      if (format === 'pdf') a.download = 'filename.pdf';
      if (format === 'excel') a.download = 'filename.xlsx';
      if (format === 'word') a.download = 'filename.docx';
      a.click();

      setTimeout(function () {
        window.URL.revokeObjectURL(url);
      }, 100);
    })
  }

  async get(param?: any) {
    this.isLoading = true;

    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];
    if (param) {
      //@ts-ignore
      this.request.level = 3;
      //this.form.controls['level'].value=3;
    }
    let request = <GetAccountReviewReportQuery>this.request;

    if (request.accountHeadIds.length == 0 && request.referenceGroupIds.length == 0 && request.referenceIds.length == 0) {
      //@ts-ignore
      this.request.level = 4;
    }
    request.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
    request.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);

    request.yearIds = [+this.identityService.applicationUser.yearId]

    if (this.saveRequests && this.requestsIndex == -1) {


      let temp = <GetAccountReviewReportQuery>JSON.parse(this.saveRequests);
      request = this.createRequest(temp);
      this.request = request;
      this.requestsIndex++;

    } else {
      const latestRequest = JSON.stringify(request);
      await this.requestsCacheService.saveRequest(window.location.pathname, latestRequest);
      this.requestsIndex++;
    }
    return await this.Mediator.send(request).then(res => {
      this.reportResult = res.filter((x: any) => ((
          x.debitBeforeDate ||
          x.creditBeforeDate ||
          x.debit ||
          x.credit ||
          x.remainDebit ||
          x.remainCredit ||
          x.debitAfterDate ||
          x.creditAfterDate
        ) && !this.showCurrencyRelatedFieldsStatus) ||
        ((
          x.debitCurrencyAmountBefore ||
          x.creditCurrencyAmountBefore ||
          x.debitCurrencyAmount ||
          x.creditCurrencyAmount ||
          x.debitCurrencyRemain ||
          x.creditCurrencyRemain ||
          x.debitCurrencyAmountAfter ||
          x.creditCurrencyAmountAfter
        ) && this.showCurrencyRelatedFieldsStatus));


      this.originalReportResult = res;

      this.updateTotalCreditAndTotalDebit()
      this.isLoading = false;
    }).catch((ex) => {
      console.warn(ex);
      this.isLoading = false;
    })

  }

  createRequest(temp: GetAccountReviewReportQuery) {
    let request = new GetAccountReviewReportQuery();

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
    request.currencyTypeBaseId = temp.currencyTypeBaseId;
    request.pageIndex = temp.pageIndex;
    request.pageSize = temp.pageSize;
    request.orderByProperty = temp.orderByProperty;
    request.conditions = temp.conditions;


    return request;
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
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
