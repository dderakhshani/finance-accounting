import {Component, OnInit, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetLedgerReportQuery} from "../../../repositories/reporting/queries/get-ledger-report-query";
import {GetJournalReportQuery} from "../../../repositories/reporting/queries/get-journal-report-query";
import {
  AccountingReportTemplateComponent
} from "../../../components/accounting-report-template/accounting-report-template.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-journal-report',
  templateUrl: './journal-report.component.html',
  styleUrls: ['./journal-report.component.scss']
})
export class JournalReportComponent  extends BaseComponent {

  @ViewChild(AccountingReportTemplateComponent) accountingReportTemplateComponent!:AccountingReportTemplateComponent;
  columns:TableColumn[] = [];

  constructor(
    private mediator: Mediator
  ) {
    super();
  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve(params?: any) {
    this.columns = [
      <TableColumn>{
        name:'selected',
        title:'',
        type: TableColumnDataType.Select,
        width: '2.5%',
      },
      <TableColumn>{
        name:'index',
        title:'ردیف',
        type: TableColumnDataType.Index,
        width: '1%',
      },
      <TableColumn>{
        name:'voucherNo',
        title:'شماره سند',
        type: TableColumnDataType.Text,
        width: '1%',
        filter: new TableColumnFilter('voucherNo',TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name:'voucherDate',
        title:'تاریخ سند',
        type: TableColumnDataType.Date,
        width: '5%',
        filter: new TableColumnFilter('voucherDa',TableColumnFilterTypes.Date),
        sortable: true
      },
      <TableColumn>{
        name:'accountHeadCode',
        title:'کد حساب',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('account',TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name:'title',
        title:'عنوان حساب',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('title',TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name:'referenceCode_1',
        title:'کد',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('debit',TableColumnFilterTypes.Money),
        sortable: true
      },
      <TableColumn>{
        name:'referenceName_1',
        title:'عنوان تفصیل',
        type: TableColumnDataType.Text,
        width: '5%',
        filter: new TableColumnFilter('referenceNa',TableColumnFilterTypes.Text),
        sortable: true
      },

      <TableColumn>{
        name:'voucherRowDescription',
        title:'شرح آرتیکل',
        type: TableColumnDataType.Text,
        width: '20%',
        filter: new TableColumnFilter('voucherRow',TableColumnFilterTypes.Text),
        sortable: true
      },
      <TableColumn>{
        name:'debit',
        title:'بدهکار',
        type: TableColumnDataType.Money,
        width: '5%',
        filter: new TableColumnFilter('debit',TableColumnFilterTypes.Money),
        sortable: true
      },
      <TableColumn>{
        name:'credit',
        title:'بستانکار',
        type: TableColumnDataType.Money,
        width: '5%',
        filter: new TableColumnFilter('credit',TableColumnFilterTypes.Money),
        sortable: true
      },
    ]


    await this.initialize()
  }

  async initialize(params?: any) {
    let query = new GetJournalReportQuery();
    query.level = 1;

    this.request = query;

  }

  async get(param?: any) {
  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  update(param?: any): any {
  }

}
