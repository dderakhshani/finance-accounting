import {Component, HostListener, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetAccountReviewReportQuery} from "../../../repositories/reporting/queries/get-account-review-report-query";
import {Router} from "@angular/router";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {SearchQuery} from 'src/app/shared/services/search/models/search-query';
import {AccountHead} from '../../../entities/account-head';
import {GetAccountHeadsQuery} from '../../../repositories/account-head/queries/get-account-heads-query';
import {Action, ActionTypes} from 'src/app/core/components/custom/action-bar/action-bar.component';
import {PagesCommonService} from 'src/app/shared/services/pages/pages-common.service';
import {IdentityService} from 'src/app/modules/identity/repositories/identity.service';
import {ToPersianDatePipe} from 'src/app/core/pipes/to-persian-date.pipe';
import {BaseValue} from "../../../../admin/entities/base-value";
import {forkJoin, from} from "rxjs";
import {distinct} from "rxjs/operators";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {AccountReviewReportResultModel} from '../../../repositories/account-review/account-review-report-result-model';
import {UserYear} from 'src/app/modules/identity/repositories/models/user-year';
import {FormControl} from '@angular/forms';
import {AccountReference} from '../../../entities/account-reference';
import {AccountReferencesGroup} from '../../../entities/account-references-group';
import {CodeVoucherGroup} from '../../../entities/code-voucher-group';
import {TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import {TableOptions} from 'src/app/core/components/custom/table/models/table-options';
import {ReportFormatTypes} from '../../../repositories/reporting/ReportFormatTypes';
import {GetAccountReferencesQuery} from '../../../repositories/account-reference/queries/get-account-references-query';
import {
  GetAccountReferencesGroupsQuery
} from '../../../repositories/account-reference-group/queries/get-account-references-groups-query';
import {
  GetCodeVoucherGroupsQuery
} from '../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query';
import {AccountingMoneyPipe} from 'src/app/core/pipes/accounting-money.pipe';
import {environment} from 'src/environments/environment';
import {addDays} from 'date-fns';
import {GetVoucherHeadsQuery} from "../../../repositories/voucher-head/queries/get-voucher-heads-query";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  ConfirmDialogComponent,
  ConfirmDialogConfig
} from "../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component";
import {VoucherHead} from "../../../entities/voucher-head";
import {MoneyPipe} from "../../../../../core/pipes/money.pipe";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {AccountManagerService} from "../../../services/account-manager.service";

@Component({
  selector: 'app-account-review-report',
  templateUrl: './account-review-report.component.html',
  styleUrls: ['./account-review-report.component.scss']
})
export class AccountReviewReportComponent extends BaseComponent {

  fetchOnInit = false;
  reportName: string = '';
  selectedTabIndex: number = 0;

  reportResult: AccountReviewReportResultModel[] = []
  originalReportResult: AccountReviewReportResultModel[] = []
  accountLevels: any[] = []
  columnsCount: any[] = []
  columnTypeControl = new FormControl();
  accountLevelSelected: number = 0;
  voucherStates: any[] = []
  accountTypes: any[] = []
  reportTypes: any[] = []

  //userAllowedYears: UserYear[] = [];

  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();

  selectedAccountReferences: AccountReference[] = [];
  accountReferenceControl = new FormControl();

  selectedAccountReferencesGroups: AccountReferencesGroup[] = [];
  accountReferencesGroupControl = new FormControl();

  selectedCodeVoucherGroups: CodeVoucherGroup[] = [];
  codeVoucherGroupControl = new FormControl();

  tableConfigurations!: TableConfigurations;
  columns: TableColumn[] = []
  totalDebit = new FormControl();
  totalCredit = new FormControl();
  currencyTypes: BaseValue[] = [];

  accountReviewPanelState: boolean = true;
  applicationUserFullName!: string;
  showCurrencyFieldsStatus: boolean = false;
  selectedColumn = 6;

  requestsList: string[] = [];
  requestsIndex: number = -1;

  deleteBeforSelected: boolean = false;

  showAccountReferenceGroup = false;
  constructor(
    private mediator: Mediator,
    private router: Router,
    public Service: PagesCommonService,
    public identityService: IdentityService,
    private matDialog: MatDialog,
    public accountManagerService:AccountManagerService
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

  async getDataFromServer() {
    let pageSize = 0;
    let pageIndex = 0;
    let query = <GetAccountReviewReportQuery>this.request;
    query.pageSize = pageSize;
    query.pageIndex = pageIndex;
    let responce = await this.mediator.send(query);
    let result = responce;
    return result;
  }

  addHours = (date: Date, hours: number): Date => {
    const result = new Date(date);
    result.setHours(result.getHours() + hours, 0, -1);
    return result;
  };

  async printRial() {

    let query = <GetAccountReviewReportQuery>this.request;

    if (this.columnTypeControl.value == 1) {

      let token = this.identityService.getToken();
      //@ts-ignore
      window.open(`${environment.apiURL}/accountingreports/AccountReviewReport/index?access_token=${token}&Column=${4}&AccountHeadIds=${query.accountHeadIds}&CodeVoucherGroupIds=${query.codeVoucherGroupIds}&CurrencyTypeBaseId=${query.currencyTypeBaseId}&Level=${query.level}&ReferenceGroupIds=${query.referenceGroupIds}&ReferenceIds=${query.referenceIds}&fromDate=${query.voucherDateFrom.toISOString()}&toDate=${query.voucherDateTo.toISOString()}&VoucherNoFrom=${query.voucherNoFrom}&VoucherNoTo=${query.VoucherNoTo}`, "_blank");
    } else if (this.columnTypeControl.value == 2) {

      let token = this.identityService.getToken();

      //@ts-ignore
      window.open(`${environment.apiURL}/accountingreports/AccountReviewReport/index?access_token=${token}&Column=${6}&AccountHeadIds=${query.accountHeadIds}&CodeVoucherGroupIds=${query.codeVoucherGroupIds}&CurrencyTypeBaseId=${query.currencyTypeBaseId}&Level=${query.level}&ReferenceGroupIds=${query.referenceGroupIds}&ReferenceIds=${query.referenceIds}&fromDate=${query.voucherDateFrom.toISOString()}&toDate=${query.voucherDateTo.toISOString()}&VoucherNoFrom=${query.voucherNoFrom}&VoucherNoTo=${query.VoucherNoTo}`, "_blank");
    } else {
      let result = await this.getDataFromServer();
      let printContents = '';
      if (result.length > 0) {
        //@ts-ignore
        result = result.sort((a, b) => (a.code < b.code ? -1 : 1));
        printContents = `<div style="text-align: center">
                          <div style="width: 20%;float: right;font-size: 14px;">
                              از تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateFrom)}
                              <br>
                              تا تاریخ:  ${new ToPersianDatePipe().transform(query.voucherDateTo)}
                          </div>
                          <div style="width: 60%;float: right;">
                                  ${this.identityService.applicationUser.companies.find(x => x.id == this.identityService.applicationUser.companyId)?.title}
                                 <br>
                                 مرور حساب ها
                                 <br><br><br>
                          </div>
                          <div style="width: 20%;float: right;font-size: 14px;">
                                تاریخ گزارش: ${new ToPersianDatePipe().transform(new Date())}
                          </div>
                        </div>`;

        printContents += `<table>
      <thead>
                 <tr>
                   <td colspan="3"> </td>`;
        let columnTypeId = this.columnTypeControl.value;
        if (columnTypeId == 1) {
          printContents += `
        <th colspan="2" scope="colgroup">گردش طی دوره</th>
        <th colspan="2" scope="colgroup">مانده</th>
        </tr>
        <tr>
        <th scope="col">ردیف</th>
        <th scope="col">کد</th>
        <th scope="col">عنوان</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        </tr>
</thead><tbody>`;
        } else if (columnTypeId == 2) {

          printContents += `
        <th colspan="2" scope="colgroup">مانده ابتدای دوره</th>
        <th colspan="2" scope="colgroup">گردش طی دوره</th>
        <th colspan="2" scope="colgroup">مانده</th>
        </tr>
        <tr>
        <th scope="col">ردیف</th>
        <th scope="col">کد</th>
        <th scope="col">عنوان</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        </tr>
</thead><tbody>`;
        } else {
          printContents += `
        <th colspan="2" scope="colgroup">مانده ابتدای دوره</th>
        <th colspan="2" scope="colgroup">گردش طی دوره</th>
        <th colspan="2" scope="colgroup">مانده</th>
        <th colspan="2" scope="colgroup">بعد از دوره</th>
        </tr>
        <tr>
        <th scope="col">ردیف</th>
        <th scope="col">کد</th>
        <th scope="col">عنوان</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        <th scope="col">بدهکار</th>
        <th scope="col">بستانکار</th>
        </tr>
</thead><tbody>`;
        }


        let alldebitBeforeDate = 0;
        let allcreditBeforeDate = 0;
        let alldebit = 0;
        let allcredit = 0;
        let allremainDebit = 0;
        let allremainCredit = 0;
        let alldebitAfterDate = 0;
        let allcreditAfterDate = 0;

        if (!this.showCurrencyFieldsStatus) {
          for (let i = 0; i < result.length; i++) {
            let data = result[i]

            printContents += `
          <tr  style="direction: ltr;text-align: center;">
          <td >${i + 1}</td>
          <td>${data.codeTitle}</td>
          <td>${data.title}</td>`;

            if (columnTypeId == 1) {
              printContents += `<td>${new AccountingMoneyPipe().transform(data.debit ? data.debit : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.credit ? data.credit : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.remainDebit ? data.remainDebit : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.remainCredit ? data.remainCredit : 0)}</td>`;
            } else if (columnTypeId == 2) {

              printContents += `<td>${new AccountingMoneyPipe().transform(data.debitBeforeDate ? data.debitBeforeDate : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditBeforeDate ? data.creditBeforeDate : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debit ? data.debit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.credit ? data.credit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.remainDebit ? data.remainDebit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.remainCredit ? data.remainCredit : 0)}</td>`;
            } else {
              printContents += `<td>${new AccountingMoneyPipe().transform(data.debitBeforeDate ? data.debitBeforeDate : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditBeforeDate ? data.creditBeforeDate : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debit ? data.debit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.credit ? data.credit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.remainDebit ? data.remainDebit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.remainCredit ? data.remainCredit : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitAfterDate ? data.debitAfterDate : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditAfterDate ? data.creditAfterDate : 0)}</td>`;
            }

            alldebitBeforeDate += data.debitBeforeDate ? data.debitBeforeDate : 0;
            allcreditBeforeDate += data.creditBeforeDate ? data.creditBeforeDate : 0;
            alldebit += data.debit ? data.debit : 0;
            allcredit += data.credit ? data.credit : 0;
            allremainDebit += data.remainDebit ? data.remainDebit : 0;
            allremainCredit += data.remainCredit ? data.remainCredit : 0;
            alldebitAfterDate += data.debitAfterDate ? data.debitAfterDate : 0;
            allcreditAfterDate += data.creditAfterDate ? data.creditAfterDate : 0;
          }
        } else {
          for (let i = 0; i < result.length; i++) {
            let data = result[i]

            printContents += `
          <tr  style="direction: ltr;text-align: center;">
          <td >${i + 1}</td>
          <td>${data.codeTitle}</td>
          <td>${data.title}</td>`;

            if (columnTypeId == 1) {
              printContents += `<td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmount ? data.debitCurrencyAmount : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmount ? data.creditCurrencyAmount : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.debitCurrencyRemain ? data.debitCurrencyRemain : 0)}</td>
          <td>${new AccountingMoneyPipe().transform(data.creditCurrencyRemain ? data.creditCurrencyRemain : 0)}</td>`;
            } else if (columnTypeId == 2) {

              printContents += `<td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmountBefore ? data.debitCurrencyAmountBefore : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmountBefore ? data.creditCurrencyAmountBefore : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmount ? data.debitCurrencyAmount : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmount ? data.creditCurrencyAmount : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitCurrencyRemain ? data.debitCurrencyRemain : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyRemain ? data.creditCurrencyRemain : 0)}</td>`;
            } else {
              printContents += `<td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmountBefore ? data.debitCurrencyAmountBefore : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmountBefore ? data.creditCurrencyAmountBefore : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmount ? data.debitCurrencyAmount : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmount ? data.creditCurrencyAmount : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitCurrencyRemain ? data.debitCurrencyRemain : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyRemain ? data.creditCurrencyRemain : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.debitCurrencyAmountAfter ? data.debitCurrencyAmountAfter : 0)}</td>
            <td>${new AccountingMoneyPipe().transform(data.creditCurrencyAmountAfter ? data.creditCurrencyAmountAfter : 0)}</td>`;
            }

            alldebitBeforeDate += data.debitCurrencyAmountBefore ? data.debitCurrencyAmountBefore : 0;
            allcreditBeforeDate += data.creditCurrencyAmountBefore ? data.creditCurrencyAmountBefore : 0;
            alldebit += data.debitCurrencyAmount ? data.debitCurrencyAmount : 0;
            allcredit += data.creditCurrencyAmount ? data.creditCurrencyAmount : 0;
            allremainDebit += data.debitCurrencyRemain ? data.debitCurrencyRemain : 0;
            allremainCredit += data.creditCurrencyRemain ? data.creditCurrencyRemain : 0;
            alldebitAfterDate += data.debitCurrencyAmountAfter ? data.debitCurrencyAmountAfter : 0;
            allcreditAfterDate += data.creditCurrencyAmountAfter ? data.creditCurrencyAmountAfter : 0;
          }
        }


        printContents += `
      <tr style="direction: ltr;text-align: center;background-color: #e7e7eb;">
      <td></td>
      <td></td>
      <td style="font-size: 18px;"> جمع کل</td>`;

        if (columnTypeId == 1) {
          printContents += `<td style="font-size: 18px;"> ${new AccountingMoneyPipe().transform(alldebit)}</td>
      <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcredit)}</td>
      <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainDebit)}</td>
      <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainCredit)}</td>
      </tr>`;
        } else if (columnTypeId == 2) {

          printContents += `<td style="font-size: 18px;"> ${new AccountingMoneyPipe().transform(alldebitBeforeDate)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcreditBeforeDate)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(alldebit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcredit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainDebit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainCredit)}</td>
        </tr>`;
        } else {
          printContents += `<td style="font-size: 18px;"> ${new AccountingMoneyPipe().transform(alldebitBeforeDate)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcreditBeforeDate)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(alldebit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcredit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainDebit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allremainCredit)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(alldebitAfterDate)}</td>
        <td style="font-size: 15px;"> ${new AccountingMoneyPipe().transform(allcreditAfterDate)}</td>
        </tr>`;
        }

        printContents += `</tbody></table>
      <p style="text-align: left;padding: 30px;">
        کاربر چاپ کننده: ${this.applicationUserFullName}
      </p>`;

        this.Service.onPrint(printContents, "")
      }
    }
  }

  downloadRialExcel() {

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
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text),
      },
      <TableColumn>{
        name: 'accountReferencesGroupsTitle',
        title: 'گروه تفصیل',
        type: TableColumnDataType.Text,
        sortable: true,
        filter: new TableColumnFilter('accountReferencesGroupsTitle', TableColumnFilterTypes.Text),
        show: false,
      },
      <TableColumn>{
        name: 'debitBeforeDate',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debitBeforeDate', TableColumnFilterTypes.Money),
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        show: true,
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmountBefore : x.debitBeforeDate;
        }
      },
      <TableColumn>{
        name: 'creditBeforeDate',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('creditBeforeDate', TableColumnFilterTypes.Money),
        groupName: 'مانده ابتدای دوره',
        groupId: 'beforeDate',
        show: true,
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmountBefore : x.creditBeforeDate;
        }
      },
      <TableColumn>{
        name: 'debit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmount : x.debit;
        }
      },
      <TableColumn>{
        name: 'credit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
        groupName: 'گردش طی دوره',
        groupId: 'dueDate',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmount : x.credit;
        }
      },
      <TableColumn>{
        name: 'remainDebit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('remainDebit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyRemain : x.remainDebit;
        }
      },
      <TableColumn>{
        name: 'remainCredit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('remainCredit', TableColumnFilterTypes.Money),
        groupName: 'مانده',
        groupId: 'remain',
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyRemain : x.remainCredit;
        }
      },
      <TableColumn>{
        name: 'debitAfterDate',
        title: ' بدهکار ',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debitAfterDate', TableColumnFilterTypes.Money),
        groupName: ' بعد از دوره',
        groupId: 'afterDate',
        show: false,
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.debitCurrencyAmountAfter : x.debitAfterDate;
        }
      },
      <TableColumn>{
        name: 'creditAfterDate',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('creditAfterDate', TableColumnFilterTypes.Money),
        groupName: 'گردش بعد از دوره',
        groupId: 'afterDate',
        show: false,
        displayFn: (x: any) => {
          return this.showCurrencyFieldsStatus ? x.creditCurrencyAmountAfter : x.creditAfterDate;
        }
      },
    ]

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

    await this.initialize()

  }

  async initialize(accountHeadIds?: []) {
    let query = new GetAccountReviewReportQuery();
    if (accountHeadIds) {
      query.accountHeadIds = accountHeadIds
      query.referenceIds = (<GetAccountReviewReportQuery>this.request)?.referenceIds
      //@ts-ignore
      query.level = this.request.level + 1

      // query.currencyTypeBaseId = this.accountingReportTemplateComponent?.form?.getRawValue()?.currencyTypeBaseId;
      // query.voucherDateFrom = this.accountingReportTemplateComponent?.form?.getRawValue()?.voucherDateFrom ?? undefined;
      // query.voucherDateTo = this.accountingReportTemplateComponent?.form?.getRawValue()?.voucherDateTo ?? undefined;
      query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
      query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
      ;
      query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);
      //@ts-ignore
      this.request = query;
      this.tempresolve()
    } else {
      query.accountHeadIds = [];
      query.level = 2;
      query.currencyTypeBaseId = this.form?.getRawValue()?.currencyTypeBaseId ?? undefined;
      this.request = query;
    }
  }


  async tempresolve(params?: any) {
    this.isLoading = true;
    this.columnsCount = [
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
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'code';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';
    this.columnTypeControl.setValue(this.columnsCount[1].id)


    this.tableConfigurations.options.exportOptions.showExportButton = false;
    this.tableConfigurations.options.exportOptions.customExportButtonTitle = 'دانلود '
    this.tableConfigurations.options.exportOptions.customExportCallbackFn = (voucherDetails: AccountReviewReportResultModel[]) => {
      return voucherDetails.map((x) => {
        return {
          'کد': x.code,
          'عنوان': x.title,
          'ابتدای دوره-بدهکار': x.debitBeforeDate,
          'ابتدای دوره بستانکار': x.creditBeforeDate,
          'طی دوره-بدهکار': x.debit,
          'طی دوره-بستانکار': x.credit,
          'مانده-بدهکار': x.remainDebit,
          'مانده-بستانکار': x.remainCredit
        }
      })
    }


    await forkJoin([
      this.mediator.send(new GetBaseValuesByUniqueNameQuery('currencyType'))
    ]).subscribe(async ([
                          currencyTypes]) => {
      this.currencyTypes = currencyTypes;
      this.form.patchValue({
        currencyTypeBaseId: this.currencyTypes.find(x => x.uniqueName === 'IRR')?.id,
      })
      await this.tempinitialize()
      this.isLoading = false;
      await this.get()
    })
  }

  async tempinitialize(params?: any) {
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

  }



  async handleRowDoubleClick(row: any) {

    let res = this.accountManagerService.accountHeads.value.find(x => x.fullCode == row.code)
      //@ts-ignore
      if (+(this.request.level) < 4) {
        let query = <GetAccountReviewReportQuery>this.request;

        if (query.level != 3) {

          let accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.parentId == res?.id)
          //@ts-ignore
          query.accountHeadIds = accountHeads.map((x: AccountHead) => x.id)
          //@ts-ignore
          query.level = this.request.level + 1


          if(row.accountReferencesGroupsCode) {
            // @ts-ignore
            query.referenceGroupIds = [this.accountManagerService.accountReferenceGroups.value.find(x => x.code == row.accountReferencesGroupsCode)?.id]
          }
          query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
          query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
          query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);

          this.request = query;
          //this.initialize(accountHeads.map((x: AccountHead) => x.id))

          this.get(this.request);
        } else {
          query.accountHeadIds = [<number>res?.id]
          //@ts-ignore
          query.level = this.request.level + 1

          query.currencyTypeBaseId = this.form.controls['currencyTypeBaseId'].value;
          query.voucherDateFrom = new Date(this.form.controls['voucherDateFrom'].value);
          ;
          query.voucherDateTo = new Date(this.form.controls['voucherDateTo'].value);
          //@ts-ignore
          this.request = query;

          await this.get(this.request);

          if (this.originalReportResult.length == 0) {

            await this.get(null, "back");
            //@ts-ignore
            this.navigateToAccountLedgerReport(query.accountHeadIds, query.referenceGroupIds,
              //@ts-ignore
              null, query.voucherDateFrom, query.voucherDateTo)

          }
        }

      } else {
        //@ts-ignore
        this.navigateToAccountLedgerReport(this.request.accountHeadIds, this.request.referenceGroupIds,
          //@ts-ignore
          this.accountManagerService.accountReferences.value.find(x => x.code == row.code)?.id, this.request.voucherDateFrom, this.request.voucherDateTo)
      }


  }

  async navigateToAccountLedgerReport(accountHeadId: number[], accountReferenceGroupId: number[], accountReferenceId: number[], fromDate: Date, toDate: Date, typeId: number) {

    await this.router.navigateByUrl(`/accounting/reporting/ledgerReport?accountHeadId=${accountHeadId}&accountReferenceGroupId=${accountReferenceGroupId}&accountReferenceId=${accountReferenceId}&fromDate=${this.Service.formatDate(fromDate)}&toDate=${this.Service.formatDate(toDate)}&typeId=${typeId}`)
    this.isLoading = false;
  }

  ledgerReport() {
    //@ts-ignore
    this.navigateToAccountLedgerReport(this.request.accountHeadIds, this.request.referenceGroupIds,
      //@ts-ignore
      this.request.referenceIds, this.request.voucherDateFrom, this.request.voucherDateTo)
  }



  async get(param?: any, action?: string) {
    this.isLoading = true;
    this.selectedTabIndex = 0;
    this.totalDebit.setValue(0)
    this.totalCredit.setValue(0)
    this.reportResult = [];
    let request = <GetAccountReviewReportQuery>this.request;
    if (param) {
      request = <GetAccountReviewReportQuery>param;
    }
    if (action == 'back') {
      this.requestsList.pop();
      this.requestsIndex--;
      if (this.requestsList[this.requestsIndex]) {
        let temp = <GetAccountReviewReportQuery>JSON.parse(this.requestsList[this.requestsIndex]);
        request = this.createRequest(temp);
        this.request = request;
      }
    } else {
      this.requestsList.push(JSON.stringify(request));
      this.requestsIndex++;
    }

    this.deleteBeforSelected = true;
    this.form.controls['accountReferencesGroupflag'].setValue(this.showAccountReferenceGroup ? 1 : 0)
    return await this.mediator.send(request).then(res => {
      this.reportResult = res.filter((x: any) => ((
          x.debitBeforeDate ||
          x.creditBeforeDate ||
          x.debit ||
          x.credit ||
          x.remainDebit ||
          x.remainCredit ||
          x.debitAfterDate ||
          x.creditAfterDate
        ) && !this.showCurrencyFieldsStatus) ||
        ((
          x.debitCurrencyAmountBefore ||
          x.creditCurrencyAmountBefore ||
          x.debitCurrencyAmount ||
          x.creditCurrencyAmount ||
          x.debitCurrencyRemain ||
          x.creditCurrencyRemain ||
          x.debitCurrencyAmountAfter ||
          x.creditCurrencyAmountAfter
        ) && this.showCurrencyFieldsStatus));


      this.originalReportResult = res;

      this.isLoading = false;
      this.updateTotalCreditAndTotalDebit(null)
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

  async updateTotalCreditAndTotalDebit(row: any) {
    //let selectedItems = this.reportResult.filter(x => x.selected);
    //let items = selectedItems.length > 0 ? selectedItems : this.reportResult;
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
    if (row) {

      //@ts-ignore
      if (this.request.level == 1 || this.request.level == 2 || this.request.level == 3) {
        let accountHeads = this.accountManagerService.accountHeads.value.filter(x => x.parentId === this.accountManagerService.accountHeads.value.find(x => x.code === row.code)?.id);
        let accountHeadIds = accountHeads.map((x: AccountHead) => x.id);
        if (row.selected == true) {
          if (this.deleteBeforSelected == false) {
            //@ts-ignore
            this.request.accountHeadIds.push(...accountHeadIds);
            this.selectedAccountHeads.push(...accountHeads);
            this.form.controls['accountHeadIds'].value.push(...accountHeadIds);
            this.form.controls['accountHeadIds'].controls.push(...accountHeadIds);
          } else {
            this.deleteBeforSelected = false;
            //@ts-ignore
            this.request.accountHeadIds = accountHeadIds;
            this.selectedAccountHeads = accountHeads;
            this.form.controls['accountHeadIds'].value = accountHeadIds;
            this.form.controls['accountHeadIds'].controls = accountHeadIds;
          }
        } else {
          for (let i = 0; i < accountHeadIds.length; i++) {
            let element = accountHeadIds[i];
            this.removeAccountHead(element);
          }
        }
      }
      //@ts-ignore
      else if (this.request.level == 4) {
        if (row.selected == true) {

          if (this.deleteBeforSelected == false) {
            this.selectedAccountReferences.push(...row.id);
            this.form.controls['referenceIds'].value.push(...row.id);
            this.form.controls['referenceIds'].controls.push(...row.id);
            // @ts-ignore
            this.request.referenceIds.push(...row.id);
          } else {
            this.deleteBeforSelected = false;
            this.selectedAccountReferences = [row.id];
            this.form.controls['referenceIds'].value = [row.id];
            this.form.controls['referenceIds'].controls = [row.id];
            // @ts-ignore
            this.request.referenceIds = [row.id];
          }
        } else {
          this.removeAccountReference(row.id);
        }
      }
    }
  }



  resetAccountHeadIdsFilter() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];

  }


  async handleAccountHeadSelection(accountHeadId: number) {
    let accountHeads : AccountHead[] = (this.accountManagerService.accountHeads.value.filter(x => x.parentId === accountHeadId));
    if(accountHeads?.length === 0) accountHeads = [<AccountHead>this.accountManagerService.accountHeads.value.find(x => x.id === accountHeadId)]
    let query = <GetAccountReviewReportQuery>this.request;
    //@ts-ignore
    query.accountHeadIds.push(...accountHeads.map((x: AccountHead) => x.id))
    //@ts-ignore
    this.request = query;

    this.accountHeadControl.setValue(null);

  }

  removeAccountHead(accountHeadId: number) {
    this.selectedAccountHeads = this.selectedAccountHeads.filter(a => a.id != accountHeadId);
    this.form.controls['accountHeadIds'].value = this.form.controls['accountHeadIds'].value.filter((a: number) => a != accountHeadId);
    //@ts-ignore
    this.request.accountHeadIds = this.request.accountHeadIds.filter((a: number) => a != accountHeadId);
  }

  removeAllAccountHead() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.selectedAccountHeads = [];
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];
    this.accountHeadControl.setValue(null);
  }




  handleAccountReferenceSelection(accountReferenceId: number) {
    this.selectedAccountReferences.push(<AccountReference>this.accountManagerService.accountReferences.value.find(x => x.id === accountReferenceId))
    this.form.controls['referenceIds'].controls.push(new FormControl(accountReferenceId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferenceControl.setValue(null)
  }

  removeAccountReference(accountReferenceId: number) {
    this.selectedAccountReferences = this.selectedAccountReferences.filter(a => a.id != accountReferenceId);
    this.form.controls['referenceIds'].value = this.form.controls['referenceIds'].value.filter((a: number) => a != accountReferenceId);
    //@ts-ignore
    this.request.referenceIds = this.request.referenceIds.filter((a: number) => a != accountReferenceId);
  }

  removeAllAccountRefrence() {
    this.selectedAccountReferences = [];
    this.form.controls['referenceIds'].value = [];
    this.form.controls['referenceIds'].controls = [];
    // @ts-ignore
    this.request.referenceIds = [];
    this.accountReferenceControl.setValue(null);

  }



  handleAccountReferencesGroupSelection(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups.push(<AccountReferencesGroup>this.accountManagerService.accountReferenceGroups.value.find(x => x.id === accountReferencesGroupId))
    this.form.controls['referenceGroupIds'].controls.push(new FormControl(accountReferencesGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.accountReferencesGroupControl.setValue(null)
  }

  removeAccountReferencesGroup(accountReferencesGroupId: number) {
    this.selectedAccountReferencesGroups = this.selectedAccountReferencesGroups.filter(a => a.id != accountReferencesGroupId);
    this.form.controls['referenceGroupIds'].value = this.form.controls['referenceGroupIds'].value.filter((a: number) => a != accountReferencesGroupId);
    //@ts-ignore
    this.request.referenceGroupIds = this.request.referenceGroupIds.filter((a: number) => a != accountHeadId);
  }

  removeAllRefrenceGroup() {
    this.selectedAccountReferencesGroups = [];
    this.form.controls['referenceGroupIds'].value = [];
    this.form.controls['referenceGroupIds'].controls = [];
    // @ts-ignore
    this.request.referenceGroupIds = [];
    this.accountReferencesGroupControl.setValue(null);

  }



  handleCodeVoucherGroupSelection(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups.push(<CodeVoucherGroup>this.accountManagerService.codeVoucherGroups.value.find(x => x.id === codeVoucherGroupId));
    this.form.controls['codeVoucherGroupIds'].controls.push(new FormControl(codeVoucherGroupId))
    this.form.patchValue(this.form.getRawValue())
    this.codeVoucherGroupControl.setValue(null)
  }


  showCurrencyRelatedFields() {

    this.showCurrencyFieldsStatus = this.currencyTypes.find(x => x.id == this.form.getRawValue().currencyTypeBaseId)?.uniqueName !== 'IRR';
    this.reportResult = this.originalReportResult.filter((x: any) => ((
        x.debitBeforeDate ||
        x.creditBeforeDate ||
        x.debit ||
        x.credit ||
        x.remainDebit ||
        x.remainCredit ||
        x.debitAfterDate ||
        x.creditAfterDate
      ) && !this.showCurrencyFieldsStatus) ||
      ((
        x.debitCurrencyAmountBefore ||
        x.creditCurrencyAmountBefore ||
        x.debitCurrencyAmount ||
        x.creditCurrencyAmount ||
        x.debitCurrencyRemain ||
        x.creditCurrencyRemain ||
        x.debitCurrencyAmountAfter ||
        x.creditCurrencyAmountAfter
      ) && this.showCurrencyFieldsStatus));

    this.get()
  }

  removeCodeVoucherGroup(codeVoucherGroupId: number) {
    this.selectedCodeVoucherGroups = this.selectedCodeVoucherGroups.filter(a => a.id != codeVoucherGroupId);
    this.form.controls['codeVoucherGroupIds'].value = this.form.controls['codeVoucherGroupIds'].value.filter((a: number) => a != codeVoucherGroupId);
    //@ts-ignore
    this.request.codeVoucherGroupIds = this.request.codeVoucherGroupIds.filter((a: number) => a != accountHeadId);
  }

  removeAllCodeVoucherGroup() {
    this.selectedCodeVoucherGroups = [];
    this.form.controls['codeVoucherGroupIds'].value = [];
    this.form.controls['codeVoucherGroupIds'].controls = [];
    // @ts-ignore
    this.request.codeVoucherGroupIds = [];
    this.codeVoucherGroupControl.setValue(null);

  }

  changeColumns() {
    let id = this.columnTypeControl.value;
    if (id == 1) {

      this.tableConfigurations.columns.filter(a => a.name == "debitBeforeDate")[0].show = false;
      this.tableConfigurations.columns.filter(a => a.name == "creditBeforeDate")[0].show = false;

      this.tableConfigurations.columns.filter(a => a.name == "creditAfterDate")[0].show = false;
      this.tableConfigurations.columns.filter(a => a.name == "debitAfterDate")[0].show = false;
      this.selectedColumn = 4;
      this.tableConfigurations.options.exportOptions.customExportCallbackFn = (voucherDetails: AccountReviewReportResultModel[]) => {
        return voucherDetails.map((x) => {
          return {
            'کد': x.code,
            'عنوان': x.title,
            'طی دوره-بدهکار': x.debit,
            'طی دوره-بستانکار': x.credit,
            'مانده-بدهکار': x.remainDebit,
            'مانده-بستانکار': x.remainCredit
          }
        })
      }
    } else if (id == 2) {

      this.tableConfigurations.columns.filter(a => a.name == "debitBeforeDate")[0].show = true;
      this.tableConfigurations.columns.filter(a => a.name == "creditBeforeDate")[0].show = true;

      this.tableConfigurations.columns.filter(a => a.name == "creditAfterDate")[0].show = false;
      this.tableConfigurations.columns.filter(a => a.name == "debitAfterDate")[0].show = false;
      this.selectedColumn = 6;
      this.tableConfigurations.options.exportOptions.customExportCallbackFn = (voucherDetails: AccountReviewReportResultModel[]) => {
        return voucherDetails.map((x) => {
          return {
            'کد': x.code,
            'عنوان': x.title,
            'ابتدای دوره-بدهکار': x.debitBeforeDate,
            'ابتدای دوره بستانکار': x.creditBeforeDate,
            'طی دوره-بدهکار': x.debit,
            'طی دوره-بستانکار': x.credit,
            'مانده-بدهکار': x.remainDebit,
            'مانده-بستانکار': x.remainCredit
          }
        })
      }
    } else {
      this.tableConfigurations.columns.filter(a => a.name == "debitBeforeDate")[0].show = true;
      this.tableConfigurations.columns.filter(a => a.name == "creditBeforeDate")[0].show = true;

      this.tableConfigurations.columns.filter(a => a.name == "creditAfterDate")[0].show = true;
      this.tableConfigurations.columns.filter(a => a.name == "debitAfterDate")[0].show = true;
      this.selectedColumn = 8;
      this.tableConfigurations.options.exportOptions.customExportCallbackFn = (voucherDetails: AccountReviewReportResultModel[]) => {
        return voucherDetails.map((x) => {
          return {
            'کد': x.code,
            'عنوان': x.title,
            'ابتدای دوره-بدهکار': x.debitBeforeDate,
            'ابتدای دوره بستانکار': x.creditBeforeDate,
            'طی دوره-بدهکار': x.debit,
            'طی دوره-بستانکار': x.credit,
            'مانده-بدهکار': x.remainDebit,
            'مانده-بستانکار': x.remainCredit,
            'بعد از دوره-بدهکار': x.debitAfterDate,
            'بعد از دوره-بستانکار': x.creditAfterDate,
          }
        })
      }
    }
  }

  changeLevel(level: number) {
    //@ts-ignore
    this.request.level = level;
    this.form.controls['level'].value = level;
    this.form.controls['level'].controls = level;
    this.get();
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

    // @ts-ignore
    this.request.voucherNoFrom = undefined;
    // @ts-ignore
    this.request.voucherNoTo = undefined;
    this.form.controls['voucherNoFrom'].value = '';
    this.form.controls['voucherNoTo'].value = '';

    this.form.controls['creditFrom'].value = '';
    this.form.controls['creditTo'].value = '';
    this.form.controls['debitFrom'].value = '';
    this.form.controls['debitTo'].value = '';
    this.form.controls['voucherRowDescription'].value = '';
    this.get();
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }

  handleAccountReferenceGroupView($event: MatCheckboxChange) {
    this.showAccountReferenceGroup = $event.checked;
    let col = this.tableConfigurations.columns.find(x => x.name === 'accountReferencesGroupsTitle');
    if(col) col.show = this.showAccountReferenceGroup
    return this.get()
  }
}
