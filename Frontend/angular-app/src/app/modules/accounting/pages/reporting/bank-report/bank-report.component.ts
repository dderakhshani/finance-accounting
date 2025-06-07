import { Component, OnInit } from '@angular/core';
import { BankReportModel } from '../../../repositories/bank-report/bank-report-model';
import { TableColumn } from 'src/app/core/components/custom/table/models/table-column';
import { TableConfigurations } from 'src/app/core/components/custom/table/models/table-configurations';
import { FormControl } from '@angular/forms';
import { PagesCommonService } from 'src/app/shared/services/pages/pages-common.service';
import { IdentityService } from 'src/app/modules/identity/repositories/identity.service';
import { Mediator } from "src/app/core/services/mediator/mediator.service";
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableColumnDataType } from 'src/app/core/components/custom/table/models/table-column-data-type';
import { TableColumnFilter } from 'src/app/core/components/custom/table/models/table-column-filter';
import { TableColumnFilterTypes } from 'src/app/core/components/custom/table/models/table-column-filter-types';
import { BankReportQuery } from '../../../repositories/reporting/queries/bank-report-query';
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';

@Component({
  selector: 'app-bank-report',
  templateUrl: './bank-report.component.html',
  styleUrls: ['./bank-report.component.scss']
})
export class BankReportComponent extends BaseComponent {


  constructor(public Mediator: Mediator, public identityService: IdentityService,
    public Service: PagesCommonService) { super(); }
  result: BankReportModel[] = [];
  columns: TableColumn[] = [];
  selectedTabIndex: number = 0;

  tableConfigurations!: TableConfigurations;
  totalDebit = new FormControl();
  totalCredit = new FormControl();
  async ngOnInit() {
    await this.initialize();
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
        name: 'title',
        title: 'نام بانک',
        type: TableColumnDataType.Text,
        sortable: true,
        filter: new TableColumnFilter('title', TableColumnFilterTypes.Text)
      },
      <TableColumn>{
        name: 'bankremain',
        title: 'موجودی',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('bankremain', TableColumnFilterTypes.Money),
      },
      <TableColumn>{
        name: 'balancebankremain',
        title: 'چک',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('balancebankremain', TableColumnFilterTypes.Money),
      },
      // <TableColumn>{
      //   name: 'debit',
      //   title: 'بدهکار',
      //   type: TableColumnDataType.Money,
      //   sortable: true,
      //   filter: new TableColumnFilter('debit', TableColumnFilterTypes.Money),
      // },
      <TableColumn>{
        name: 'remain',
        title: 'مانده',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('remain', TableColumnFilterTypes.Money),
      },
      <TableColumn>{
        name: 'facilitiesremain',
        title: 'تسهیلات',
        type: TableColumnDataType.Money,
        sortable: true,
        filter: new TableColumnFilter('facilitiesremain', TableColumnFilterTypes.Money),
      }
    ]

    let voucherDateFrom = this.identityService.getActiveYearStartDate();
    let voucherDateTo = this.identityService.getActiveYearEndDate();
    let query = new BankReportQuery(voucherDateFrom, voucherDateTo);

    this.request = query;

    this.tableConfigurations = new TableConfigurations(this.columns, new TableOptions())
    this.tableConfigurations.options.usePagination = true;

    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.form.patchValue({
      voucherDateFrom: this.identityService.getActiveYearStartDate(),
      voucherDateTo: this.identityService.getActiveYearEndDate(),
      companyId: +this.identityService.applicationUser.companyId,
    })

    await this.get();
  }
  printstimul() {

  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  async get(param?: any) {
    this.isLoading = true;
    let query = <BankReportQuery>this.request;
    //@ts-ignore
    query.voucherDateTo = this.addHours(query.voucherDateTo, 24)
    this.result = await this.Mediator.send(query);
    this.isLoading = false;
  }

  addHours = (date: Date, hours: number): Date => {
    const result = new Date(date);
    result.setHours(result.getHours() + hours);
    return result;
  };
  async exportFile(format: string) {
    let request = <BankReportQuery>this.request;
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
