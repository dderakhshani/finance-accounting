import {Component, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetLedgerReportQuery} from "../../../repositories/reporting/queries/get-ledger-report-query";
import {ActivatedRoute, Router} from "@angular/router";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {VoucherDetail} from "../../../entities/voucher-detail";
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {distinct} from 'rxjs/operators';
import {forkJoin, from, of} from 'rxjs';
import {IdentityService} from 'src/app/modules/identity/repositories/identity.service';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';
import {UserYear} from 'src/app/modules/identity/repositories/models/user-year';
import {AccountHead} from '../../../entities/account-head';
import {FormControl} from '@angular/forms';
import {AccountReference} from '../../../entities/account-reference';
import {AccountReferencesGroup} from '../../../entities/account-references-group';
import {CodeVoucherGroup} from '../../../entities/code-voucher-group';
import {TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
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
import {AccountingMoneyPipe} from 'src/app/core/pipes/accounting-money.pipe';
import {MoneyPipe} from "../../../../../core/pipes/money.pipe";
import {AccountManagerService} from "../../../services/account-manager.service";
import {CustomDecimalPipe} from "../../../../../core/components/custom/table/table-details/pipe/custom-decimal.pipe";

@Component({
  selector: 'app-ledger-report',
  templateUrl: './ledger-report.component.html',
  styleUrls: ['./ledger-report.component.scss']
})
export class LedgerReportComponent extends BaseComponent {

  columns: TableColumn[] = [];
  hasPassedInFilters = false;
  applicationUserFullName!: string;
  selectedTabIndex: number = 0;

  reportResult: any[] = []
  accountLevels: any[] = []
  accountLevelSelected: number = 0;
  voucherStates: any[] = []
  accountTypes: any[] = []
  reportTypes: any[] = []

  //userAllowedYears: UserYear[] = [];

  // accountHeads: AccountHead[] = [];
  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();

  // accountReferences: AccountReference[] = [];
  selectedAccountReferences: AccountReference[] = [];
  accountReferenceControl = new FormControl();

  // accountReferencesGroups: AccountReferencesGroup[] = [];
  selectedAccountReferencesGroups: AccountReferencesGroup[] = [];
  accountReferencesGroupControl = new FormControl();

  // codeVoucherGroups: CodeVoucherGroup[] = [];
  selectedCodeVoucherGroups: CodeVoucherGroup[] = [];
  codeVoucherGroupControl = new FormControl();

  tableConfigurations!: TableConfigurations;
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
  excludedRows: any = [];

  constructor(
    private mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute, public Service: PagesCommonService, public identityService: IdentityService,
    public accountingManagerService:AccountManagerService,
    private customDecimal: CustomDecimalPipe,
  ) {

    super(route, router);

    this.request = new GetLedgerReportQuery();
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
      }
    });
  }


  async ngOnInit() {
    await this.resolve();
  }

  // ngAfterViewInit(): void {
  //   let currentUserYear = this.userAllowedYears.find(x => x.id == this.identityService.applicationUser.yearId);
  //   if (currentUserYear) currentUserYear._selected = true;

  //   this.form.patchValue({
  //     voucherDateFrom: currentUserYear?.firstDate,
  //     voucherDateTo: new Date(new Date().setHours(0, 0, 0, 0)),
  //     companyId: +this.identityService.applicationUser.companyId,
  //     reportFormat: ReportFormatTypes.None
  //   })
  //   this.form.controls['yearIds'].controls.push(new FormControl(+this.identityService.applicationUser.yearId))
  //   this.form.patchValue(this.form.getRawValue())


  //   this.accountHeadControl.valueChanges.subscribe(async (newValue) => {
  //     if (typeof newValue !== "number") this.accountHeads = await this.getAccountHeads(newValue);
  //   })
  //   this.accountReferenceControl.valueChanges.subscribe(async (newValue) => {
  //     if (typeof newValue !== "number") this.accountReferences = await this.getAccountReferences(newValue);
  //   })
  //   this.accountReferencesGroupControl.valueChanges.subscribe(async (newValue) => {
  //     if (typeof newValue !== "number") this.accountReferencesGroups = await this.getAccountReferencesGroups(newValue);
  //   })
  //   this.codeVoucherGroupControl.valueChanges.subscribe(async (newValue) => {
  //     if (typeof newValue !== "number") this.codeVoucherGroups = await this.getCodeVoucherGroups(newValue);
  //     if (!newValue) this.form.controls['codeVoucherGroupId'].setValue(null)
  //   })
  // }
  async resolve(params?: any) {
    this.isLoading = true;
    this.columns = [
      <TableColumn>{
        name: 'selected',
        title: '',
        type: TableColumnDataType.Select,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%'
      },
      <TableColumn>{
        name: 'documentNo',
        title: 'شماره سند مرتبط',
        type: TableColumnDataType.Number,
        width: '2.5%',
        filter: new TableColumnFilter('documentNo', TableColumnFilterTypes.Number),
        sortable: true
      },
      <TableColumn>{
        name: 'voucherNo',
        title: 'شماره سند',
        type: TableColumnDataType.Number,
        width: '2.5%',
        filter: new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number),
        sortable: true
      },
      <TableColumn>{
        name: 'voucherDate',
        title: 'تاریخ سند',
        type: TableColumnDataType.Date,
        width: '5%',
        filter: new TableColumnFilter('voucherDate', TableColumnFilterTypes.Date),
        sortable: true
      },
      <TableColumn>{
        name: 'accountHeadCode',
        title: 'کد حساب',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('accountHeadCode', TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name: 'title',
        title: 'عنوان حساب',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name: 'referenceCode_1',
        title: 'کد',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('referenceCode_1', TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name: 'referenceName_1',
        title: 'عنوان تفصیل',
        type: TableColumnDataType.Text,
        width: '15%',
        filter: new TableColumnFilter('referenceName_1', TableColumnFilterTypes.Text),
        sortable: true
      },

      <TableColumn>{
        name: 'voucherRowDescription',
        title: 'شرح آرتیکل',
        type: TableColumnDataType.Text,
        width: '20%',
        filter: new TableColumnFilter('voucherRowDescription', TableColumnFilterTypes.Text),
        sortable: true
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
        name: 'remaining',
        title: 'مانده',
        type: TableColumnDataType.Money,
        width: '10%',
        filter: new TableColumnFilter('remaining', TableColumnFilterTypes.Money),
        sortable: true,
        showSumRow: true,
        sumRowDisplayFn: () => {

          return new MoneyPipe().transform(this.remaining.value)
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
        sumRowDisplayFn: () => {

          return new MoneyPipe().transform(this.currencyRemain.value)
        }
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

    this.tableConfigurations = new TableConfigurations(this.columns, new TableOptions())
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    this.tableConfigurations.pagination.pageSize = 100;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.lazyLoading = true;
    await forkJoin([

      this.mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    ]).subscribe(async ([
                          currencyTypes]) => {

      this.currencyTypes = currencyTypes;

        this.initialize().then(() => {
          this.get();
        });
      });


    // await this.initialize()

  }

  async initialize(params?: any) {
    let allcurrency = new BaseValue();
    allcurrency.baseValueTypeId = 0;
    allcurrency.code = '0';
    allcurrency.id = 0;
    allcurrency.isReadOnly = true;
    allcurrency.levelCode = '0';
    allcurrency.orderIndex = 1;
    allcurrency.title = 'همه';
    allcurrency.uniqueName = 'all';
    allcurrency.value = '0';
    this.currencyTypes.push(allcurrency);
    let query = new GetLedgerReportQuery();
    //query.level = +this.getQueryParam('level') ?? 1;

    let accountHeadId = <string>this.getQueryParam('accountHeadId');
    if (accountHeadId) {
      let accountHeads = accountHeadId.split(',').map(x => Number(x));
      for (let i = 0; i < accountHeads.length; i++) {
        if (accountHeads[i] != 0) {
          query.accountHeadIds.push(+accountHeads[i]);
          this.selectedAccountHeads.push(<AccountHead>this.accountingManagerService.accountHeads.value.find(x => x.id === +accountHeads[i]))
        }
      }
    }

    let accountReferenceGroupId = <string>this.getQueryParam('accountReferenceGroupId');
    if (accountReferenceGroupId) {
      let accountReferenceGroupIds = accountReferenceGroupId.split(',').map(x => Number(x));
      for (let i = 0; i < accountReferenceGroupIds.length; i++) {
        if (accountReferenceGroupIds[i] != 0) {
          //this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountReferencesGroups.find(x => x.id === +accountReferenceGroupIds[i]))
          query.referenceGroupIds.push(+accountReferenceGroupIds[i]);
        }
      }
    }

    let accountReferenceId = <string>this.getQueryParam('accountReferenceId');
    if (accountReferenceId) {
      let accountReferenceIds = accountReferenceId.split(',').map(x => Number(x));
      for (let i = 0; i < accountReferenceIds.length; i++) {
        if (accountReferenceIds[i] != 0) {

          //this.selectedAccountReferences.push(<AccountReference>this.accountReferences.find(x => x.id === +accountReferenceIds[i]))
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
    if (typeId) {

      //query.accountHeadIds.push(+accountHeadId);
    }


    // let accountId = this.getQueryParam('accountId')
    // if (accountId) {
    //   if (query.level !== 4) query.accountHeadIds.push(+accountId)
    //   if (query.level === 4) query.referenceIds.push(+accountId)
    //   this.hasPassedInFilters = true;
    // }
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


    this.isLoading = false;
  }

  async navigateToVoucherHead(voucherdetail: VoucherDetail) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${voucherdetail.voucherId}&vdId=${voucherdetail.id}`)
  }

  async getDataFromServer(printType: number, forcePrint: boolean = false) {
    let pageSize = 0;
    let pageIndex = 0;
    let orginal = JSON.stringify(<GetLedgerReportQuery>this.request)
    let copy = <GetLedgerReportQuery>Object.assign({}, this.request);
    let query = <GetLedgerReportQuery>this.request;
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
    let responce = await this.mediator.send(query);
    let result = responce.data;
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

  async convertToExcel(result: any) {
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
    a.download = 'دفتر تفصیلی مطابق ردیفهای سند.xlsx'; // Set the file name with .xlsx extension
    a.click();

    // Clean up and release the object URL
    window.URL.revokeObjectURL(url);
  }

  async downloadRialExcel() {

    this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(0, true);
    if (typeof result === "string") {
      this.convertToExcel(result);
    }
    this.isLoading = false;
  }

  async downloadDollarExcel() {

    this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(1, true);
    if (typeof result === "string") {
      this.convertToExcel(result)
    }
    this.isLoading = false;
  }

  async downloadRialDollarExcel() {

    this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(2, true);
    if (typeof result === "string") {
      this.convertToExcel(result)
    }
    this.isLoading = false;
  }

  async printRial() {
    this.isPrinting = true;
    this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(0, false);
    if (typeof result === "string") {
      this.convertToExcel(result)

      this.isPrinting = false;
      this.isLoading = false;
      return;
    }

    let printContents = '';
    let name = '';
    if (result.length > 0) {
      let accountHeadName = '';
      if (this.selectedAccountHeads.length == 1) {
        accountHeadName = "کد حساب: " + this.selectedAccountHeads[0].code + "  -  عنوان حساب: " + this.selectedAccountHeads[0].title;
      }
      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result).pipe(distinct(e => e.referenceCode_1))
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

      if (this.selectedAccountHeads.length != 1) {
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

        if (this.selectedAccountHeads.length != 1) {
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

      if (this.selectedAccountHeads.length != 1) {
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
    this.isLoading = true;
    this.isPrinting = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(1, false);
    if (typeof result === "string") {
      this.convertToExcel(result);
      this.isLoading = false;
      return;
    }
    let printContents = '';
    let name = '';
    if (result.length > 0) {

      let accountHeadName = '';
      if (this.selectedAccountHeads.length == 1) {
        accountHeadName = "کد حساب: " + this.selectedAccountHeads[0].code + "  ,  عنوان حساب: " + this.selectedAccountHeads[0].title;
      }

      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result).pipe(distinct(e => e.referenceCode_1))
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

      // if (this.selectedAccountHeads.length != 1) {

      //   printContents += `
      // <th style="width: 6%;">کد حساب</th>
      // <th style="width: 8%;">عنوان حساب</th>`;
      // }
      // if (query.referenceIds.length == 0) {
      //   printContents += `
      //               <th style="width: 6%;">کد تفصیل</th>
      //               <th style="width: 8%;">عنوان تفصیل</th>`;
      // }
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

        // if (this.selectedAccountHeads.length != 1) {
        //   printContents += `
        // <td style="font-size: 9px;">${data.accountHeadCode}</td>
        // <td style="font-size: 9px;">${data.title}</td>‍`;
        // }
        // if (query.referenceIds.length == 0) {
        //   printContents += `
        //   <td style="font-size: 9px;">${data.referenceCode_1}</td>
        //               <td style="font-size: 9px;">${data.referenceName_1}</td>`;
        // }

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
        if (showSum == true) {
          printContents += `
            <tr style = "direction: ltr;text-align: center;background-color: #e7e7eb;" >
              <td style="font-size: 9px;"></td>
              <td style="font-size: 9px;"> </td>
              <td style="font-size: 9px;"> </td>`;

          // if (this.selectedAccountHeads.length != 1) {
          //   printContents += `
          //     <td style="font-size: 9px;"> </td>
          //     <td style="font-size: 9px;"> </td>`;
          // }
          // if (query.referenceIds.length == 0) {
          //   printContents += `
          //           <td></th>
          //           <td></th>`;
          // }

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

    this.isLoading = true;
    let query = <GetLedgerReportQuery>this.request;
    let result = await this.getDataFromServer(2, false);
    if (typeof result === "string") {
      this.convertToExcel(result)
      this.isLoading = false;
      return;
    }
    let printContents = '';
    let name = '';
    if (result.length > 0) {

      let accountHeadName = '';
      if (this.selectedAccountHeads.length == 1) {
        accountHeadName = "کد حساب: " + this.selectedAccountHeads[0].code + "  ,  عنوان حساب: " + this.selectedAccountHeads[0].title;
      }
      if (query.referenceIds.length == 0) {
        name += 'نام: همه';
      } else {
        // @ts-ignore
        from(result).pipe(distinct(e => e.referenceCode_1))
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

      // if (this.selectedAccountHeads.length != 1) {

      //   printContents += `
      // <th style="width: 6%;">کد حساب</th>
      // <th style="width: 8%;">عنوان حساب</th>`;
      // }
      // if (query.referenceIds.length == 0) {
      //   printContents += `
      //               <th style="width: 6%;">کد تفصیل</th>
      //               <th style="width: 8%;">عنوان تفصیل</th>`;
      // }
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
        if (showSum == true) {
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


  async get(param?: any) {
    this.isLoading = true;
    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];
    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.includedRows));
    searchQueries.push(...this.excludedRows);
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
    if (this.tableConfigurations.sortKeys.length == 0) {
      orderByProperty = 'voucherDate ASC';
    } else {

      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
      let lastorder = orderByProperty.split(',')[orderByProperty.split(',').length - 1];
      orderByProperty = lastorder;
      this.tableConfigurations.sortKeys = [lastorder];

    }
    //@ts-ignore
    this.request.usePagination = true;
    let request = <GetLedgerReportQuery>this.request;
    request.reportFormat = 1;
    request.conditions = searchQueries;
    request.pageSize = this.tableConfigurations.pagination.pageSize
    request.pageIndex = this.tableConfigurations.pagination.pageIndex + 1
    request.orderByProperty = orderByProperty;
    request.yearIds = [+this.identityService.applicationUser.yearId]


    return await this.mediator.send(request).then(res => {
      this.reportResult = res.data;
      this.totalDebit.setValue(res.totalDebit)
      this.totalCredit.setValue(res.totalCredit)
      this.totalCurrencyDebit.setValue(res.totalCurrencyDebit)
      this.totalCurrencyCredit.setValue(res.totalCurrencyCredit)
      this.remaining.setValue(res.remaining);
      this.currencyRemain.setValue(res.currencyRemain);
      res.totalCount ? this.tableConfigurations.pagination.totalItems = res.totalCount : '';
      this.tableConfigurations.options.isLoadingTable = false;
      this.isLoading = false;
    }).catch(() => {
      this.isLoading = false;
    })

  }

  updateTotalCreditAndTotalDebit() {
    let selectedItems = this.reportResult.filter(x => x.selected);
    let items = selectedItems.length > 0 ? selectedItems : this.reportResult;

    // this.totalCredit.setValue(items.map((x: any) => x.credit).reduce((partialSum: any, a: any) => partialSum + a, 0));
    // this.totalDebit.setValue(items.map((x: any) => x.debit).reduce((partialSum: any, a: any) => partialSum + a, 0));
  }

  // async getExcel() {
  //   let a = this.request;
  //   // @ts-ignore
  //   a.reportFormat = ReportFormatTypes.Excel;

  //   return await this.mediator.send(a).then(res => {
  //     let binaryData = [];
  //     binaryData.push(res);
  //     let blob = new Blob(binaryData, { type: res.type });

  //     let a = document.createElement("a");
  //     document.body.appendChild(a);

  //     let url = window.URL.createObjectURL(blob);
  //     a.href = url;
  //     a.download = 'filename.xlsx';
  //     a.click();

  //     setTimeout(function () {
  //       window.URL.revokeObjectURL(url);
  //     }, 100);
  //   })
  // }

  // async exportFile(format: string) {
  //   let request = this.request;
  //   // @ts-ignore
  //   if (format === 'pdf') request.reportFormat = ReportFormatTypes.Pdf;
  //   // @ts-ignore
  //   if (format === 'excel') request.reportFormat = ReportFormatTypes.Excel;
  //   // @ts-ignore
  //   if (format === 'word') request.reportFormat = ReportFormatTypes.Word;

  //   return await this.mediator.send(request).then((res: any) => {
  //     let binaryData = [];
  //     binaryData.push(res);
  //     let blob = new Blob(binaryData, { type: res.type });

  //     let a = document.createElement("a");
  //     document.body.appendChild(a);

  //     let url = window.URL.createObjectURL(blob);
  //     a.href = url;

  //     if (format === 'pdf') a.download = 'filename.pdf';
  //     if (format === 'excel') a.download = 'filename.xlsx';
  //     if (format === 'word') a.download = 'filename.docx';
  //     a.click();

  //     setTimeout(function () {
  //       window.URL.revokeObjectURL(url);
  //     }, 100);
  //   })
  // }

  // writeReportToIframe() {

  //   let iframe = <any>document.getElementById('iframe');
  //   let content = this.reportResult;
  //   let doc = iframe.contentDocument || iframe.contentWindow;
  //   doc.open();
  //   doc.write(content);
  //   doc.close();
  // }
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
    return await this.mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, "fullCode")).then(res => {
      return res.filter(x => x.lastLevel)
    })
  }

  resetAccountHeadIdsFilter() {
    // @ts-ignore
    this.request.accountHeadIds = []
  }

  handleAccountHeadSelection(accountHeadId: number) {

    this.selectedAccountHeads.push(<AccountHead>this.accountingManagerService.accountHeads.value.find(x => x.id === accountHeadId))
    this.form.controls['accountHeadIds'].controls.push(new FormControl(accountHeadId))
    this.form.patchValue(this.form.getRawValue())
    this.accountHeadControl.setValue(null)

  }

  handleincludeOnlySelectedItemsEvent(ids: number[]) {
    this.includedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'ofList',
      nextOperand: 'and'
    })];
    this.get();

  }

  handleClearSelectedItemsEvent() {
    this.includedRows = [];
    this.excludedRows = [];
    this.get();
  }

  handleExcludeSelectedItemsEvent(ids: number[]) {
    this.excludedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'notIn',
      nextOperand: 'and'
    })];
    this.get();
  }

  removeAccountHead(accountHeadId: number) {
    this.selectedAccountHeads = this.selectedAccountHeads.filter(a => a.id != accountHeadId);
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
    return await this.mediator.send(new GetAccountReferencesQuery(1, 10, searchQueries, "code")).then(res => res.data)
  }



  handleAccountReferenceSelection(accountReferenceId: number) {
    this.selectedAccountReferences.push(<AccountReference>this.accountingManagerService.accountReferences.value.find(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].controls.push(new FormControl(accountReferenceId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferenceControl.setValue(null)
  }

  removeAccountReference(accountReferenceId: number) {
    this.selectedAccountReferences = this.selectedAccountReferences.filter(a => a.id != accountReferenceId);
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
    return await this.mediator.send(new GetAccountReferencesGroupsQuery(1, 10, searchQueries, "code")).then(res => res.data)
  }



  handleAccountReferencesGroupSelection(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountingManagerService.accountReferenceGroups.value.find(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].controls.push(new FormControl(accountReferencesGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferencesGroupControl.setValue(null)
  }

  removeAccountReferencesGroup(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups = this.selectedAccountReferencesGroups.filter(a => a.id != accountReferencesGroupId);
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
    return await this.mediator.send(new GetCodeVoucherGroupsQuery(1, 10, searchQueries)).then(res => res.data)
  }

  handleCodeVoucherGroupSelection(codeVoucherGroupIds: number) {
    this.selectedCodeVoucherGroups.push(<CodeVoucherGroup>this.accountingManagerService.codeVoucherGroups.value.find(x => x.id === codeVoucherGroupIds));
    this.form.controls['codeVoucherGroupIds'].controls.push(new FormControl(codeVoucherGroupIds))
    this.form.patchValue(this.form.getRawValue())
    this.codeVoucherGroupControl.setValue(null)
  }



  showCurrencyRelatedFields(show: boolean) {
    this.showCurrencyFields = show;
    const targetColumns = new Set(['currencyTypeBaseTitle', 'currencyDebit', 'currencyCredit', 'currencyRemain', 'currencyFee']);
    const currencyColumns = this.tableConfigurations.columns.filter(x => targetColumns.has(x.name));
    // let currencyColumns = this.tableConfigurations.columns.filter(x => x.name === 'currencyTypeBaseTitle' || x.name === 'currencyDebit' || x.name === 'currencyCredit' || x.name === 'currencyRemain' || x.name === 'currencyFee')

    currencyColumns.forEach((x) => {
      x.show = show
    })
  }

  removeCodeVoucherGroup(codeVoucherGroupIds: number) {
    this.selectedCodeVoucherGroups = this.selectedCodeVoucherGroups.filter(a => a.id != codeVoucherGroupIds);
    this.form.controls["codeVoucherGroupIds"].removeAt(this.form.controls['codeVoucherGroupIds'].value.findIndex((x: any) => x === codeVoucherGroupIds))
  }


  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }


  removeAllFilters() {
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

    this.form.controls['creditFrom'].setValue("")
    this.form.controls['creditTo'].setValue("")
    this.form.controls['debitFrom'].setValue("")
    this.form.controls['debitTo'].setValue("")
    this.form.controls['voucherRowDescription'].setValue("");
  }
}
