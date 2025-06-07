import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { AnnualLedgerReportModel } from '../../../repositories/annual-ledger/annual-ledger-report-model';
import { ToPersianDatePipe } from 'src/app/core/pipes/to-persian-date.pipe';
import { PagesCommonService } from 'src/app/shared/services/pages/pages-common.service';
import { Mediator } from "src/app/core/services/mediator/mediator.service";
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';
import { AnnualLedgerReportQuery } from '../../../repositories/reporting/queries/annual-ledger-report-query';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { AccountHead } from '../../../entities/account-head';
import { FormControl } from '@angular/forms';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { GetAccountHeadsQuery } from '../../../repositories/account-head/queries/get-account-heads-query';
import { AccountingMoneyPipe } from 'src/app/core/pipes/accounting-money.pipe';
import { formatDate } from '@angular/common';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-annual-ledger-report',
  templateUrl: './annual-ledger-report.component.html',
  styleUrls: ['./annual-ledger-report.component.scss']
})
export class AnnualLedgerReportComponent extends BaseComponent {

  result = new AnnualLedgerReportModel();
  dateNow = new ToPersianDatePipe().transform(new Date());
  columns: TableColumn[] = [];
  selectedTabIndex: number = 0;
  accountHeads: AccountHead[] = [];
  selectedAccountHeads: AccountHead[] = [];
  accountHeadControl = new FormControl();


  tableConfigurations!: TableConfigurations;
  totalDebit = new FormControl();
  totalCredit = new FormControl();

  constructor(public Mediator: Mediator, public identityService: IdentityService,
    public Service: PagesCommonService) { super(); }

  async ngOnInit() {
    await this.initialize();
  }
  printstimul() {
    let query = <AnnualLedgerReportQuery>this.request;
    //@ts-ignore
    query.voucherDateTo = this.addHours(query.voucherDateTo, 24)
    let token = this.identityService.getToken();

    window.open(`${environment.apiURL}/accountingreports/StimulSoft/index?access_token=${token}&companyId=${query.companyId}&yearIds=
    ${query.yearIds}&fromDate=${query.voucherDateFrom.toISOString()}&toDate=${query.voucherDateTo.toISOString()}`, "_blank");
  }
  resolve(params?: any) {
    throw new Error('Method not implemented.');
  }
  async initialize(params?: any) {
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
        name: 'level2Code',
        title: 'کد حساب',
        type: TableColumnDataType.Text,
        sortable: true,
        filter: new TableColumnFilter('level2Code', TableColumnFilterTypes.Text)
      },
      <TableColumn>{
        name: 'level2Title',
        title: 'عنوان حساب',
        type: TableColumnDataType.Text,
        sortable: true,
        filter: new TableColumnFilter('level2Title', TableColumnFilterTypes.Text)
      },
      <TableColumn>{
        name: 'debit',
        title: 'بدهکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
      },
      <TableColumn>{
        name: 'credit',
        title: 'بستانکار',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('credit', TableColumnFilterTypes.Money),
      }
    ]

    let voucherDateFrom = this.identityService.getActiveYearStartDate();
    let voucherDateTo = this.identityService.getActiveYearEndDate();
    let companyId = +this.identityService.applicationUser.companyId;
    let yearid: number[] = [this.identityService.applicationUser.yearId]
    let query = new AnnualLedgerReportQuery(companyId, yearid, voucherDateFrom, voucherDateTo, []);
    this.request = query;

    this.tableConfigurations = new TableConfigurations(this.columns, new TableOptions())
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.accountHeads = await this.getAccountHeads();

    this.form.patchValue({
      voucherDateFrom: this.identityService.getActiveYearStartDate(),
      voucherDateTo: this.identityService.getActiveYearEndDate(),
      companyId: +this.identityService.applicationUser.companyId,
    })
    // this.form.controls['yearIds'].controls.push(new FormControl(+this.identityService.applicationUser.yearId))
    // this.form.patchValue(this.form.getRawValue())

    this.accountHeadControl.valueChanges.subscribe(async (newValue) => {
      if (typeof newValue !== "number") this.accountHeads = await this.getAccountHeads(newValue);
    })
    await this.get();
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  async get(param?: any) {
    this.isLoading = true;
    let query = <AnnualLedgerReportQuery>this.request;
    //@ts-ignore
    query.voucherDateTo = this.addHours(query.voucherDateTo, 24)
    this.result = <AnnualLedgerReportModel>await this.Mediator.send(query);
    this.isLoading = false;
  }
  addHours = (date: Date, hours: number): Date => {
    const result = new Date(date);
    result.setHours(result.getHours() + hours);
    return result;
  };
  async print() {

    this.isLoading = true;
    let query = <AnnualLedgerReportQuery>this.request;
    let printContents = `<div id="printElement">
    <div class="text-center">
        <h3>شرکت ایفاسرام(سهامی خاص)</h3>
        <div class="row">
            <div  style="text-align: right;float: right;width: 33.33%;">
            از تاریخ: ${new ToPersianDatePipe().transform(query.voucherDateFrom)}
            \t \t\t

            تا تاریخ: ${new ToPersianDatePipe().transform(query.voucherDateTo)}
            </div>
            <div class="col-3" style="float: right;text-align: center;width: 33.33%;">
              دفتر کل
            </div>
            <div class="col-4" style="text-align: left;float: right;width: 33.33%;">
                تاریخ گزارش: ${new ToPersianDatePipe().transform(this.dateNow)}
            </div>
        </div>
        <table>
            <thead>
                <tr>
                    <td class="text-center" style="width: 10%;">
                        کد حساب</td>
                    <td class="font16 text-center">عنوان حساب</td>
                    <td class="font16 text-center">بدهکار</td>
                    <td class="font16 text-center">بستانکار</td>
                </tr>
            </thead>
            <tbody>`;
    for (let i = 0; i < this.result.datas.length; i++) {
      const element = this.result.datas[i];
      printContents += `
                <tr >
                    <td class="font14 text-center">${element.level2Code}</td>
                    <td class="font14 text-center">${element.level2Title}</td>
                    <td class="font14 text-center">${new AccountingMoneyPipe().transform(element.debit)}</td>
                    <td class="font14 text-center">${new AccountingMoneyPipe().transform(element.credit)}</td>
                </tr> `
    }
    printContents += `
                <tr class="font16" style="background-color: #e7e7eb;">
                    <td class="text-center"></td>
                    <td class="font16 text-center">جمع</td>
                    <td class="font16 text-center">${new AccountingMoneyPipe().transform(this.result.sumDebit)}</td>
                    <td class="font16 text-center">${new AccountingMoneyPipe().transform(this.result.sumCredit)}</td>
                </tr>

            </tbody>
        </table>
    </div>
</div>`;
    this.isLoading = false;
    this.Service.onPrint(printContents?.toString(), '');
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
    return await this.Mediator.send(new GetAccountHeadsQuery(0, 0, searchQueries, "fullCode"));
  }

  removeAllFilters() {
    // @ts-ignore
    this.request.accountHeadIds = []
    this.selectedAccountHeads = [];
    this.form.controls['accountHeadIds'].value = [];
    this.form.controls['accountHeadIds'].controls = [];
    this.accountHeadControl.setValue(null);
  }

  updateTotalCreditAndTotalDebit() {
    let items = this.result.datas;
    items.forEach(x => {

      x.debit = x.debit;
      x.credit = x.credit;
    });
  }

  async exportFile(format: string) {
    let request = <AnnualLedgerReportQuery>this.request;
    // @ts-ignore
    if (format === 'pdf') request.reportFormat = ReportFormatTypes.Pdf;
    // @ts-ignore
    if (format === 'excel') request.reportFormat = ReportFormatTypes.Excel;
    // @ts-ignore
    if (format === 'word') request.reportFormat = ReportFormatTypes.Word;

    return await this.Mediator.send(request).then((res: any) => {
      let binaryData = [];
      binaryData.push(res);
      let blob = new Blob(binaryData, { type: res.type });

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

  async handleAccountHeadSelection(accountHeadId: number) {

    let accountHeads = await this.getAccountHeadChildren(<AccountHead>this.accountHeads.find(x => x.id === accountHeadId));
    let query = <AnnualLedgerReportQuery>this.request;


    //@ts-ignore
    query.accountHeadIds = accountHeads.map((x: AccountHead) => x.id)
    //@ts-ignore
    this.request = query;

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

  accountHeadDisplayFn(accountHeadId: number) {
    let accountHead = this.accountHeads.find(x => x.id === accountHeadId) ?? this.selectedAccountHeads.find(x => x.id === accountHeadId)
    return accountHead ? [accountHead.fullCode, accountHead.title].join(' ') : '';
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
