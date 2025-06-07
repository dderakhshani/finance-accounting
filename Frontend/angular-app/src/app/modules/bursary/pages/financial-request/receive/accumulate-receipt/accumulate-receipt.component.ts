import { Component, OnInit, Optional } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { CreateFinancialAttachmentCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command';
import { environment } from 'src/environments/environment';
import { CustomerReceiptAttachmentComponent } from '../add-receipt/customer-receipt-attachment/customer-receipt-attachment.component';
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-accumulate-receipt',
  templateUrl: './accumulate-receipt.component.html',
  styleUrls: ['./accumulate-receipt.component.scss']
})
export class AccumulateReceiptComponent extends BaseComponent{


  public isUploading !: boolean;
  public excelUrl!: string;
  tableConfigurations!: TableConfigurations;
  baseUrl : string = environment.fileServer+"/";



  set file(value: string) {
    this.excelUrl = "";
      this.excelUrl = value;
  }

  async ngOnInit() {
    await this.resolve();
  }

  ngAfterViewInit(): void {
    this.resolve();
  }
  async resolve() {

    this.formActions = [

    ];
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        "documentNo", "شماره پرداخت ",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter(
          "DocNumber",
          TableColumnFilterTypes.Text,
        )
      ),
      new TableColumn(
        "CustomerCode",
        "شماره مشتری ",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("CustomerCode", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "FullName",
        "طرف حساب",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("FullName", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "TargetAccountName",
        "واریزکننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("TargetAccountName", TableColumnFilterTypes.Text)
      ),
      //,

      new TableColumn(
        "ReceicePersianDate",
        "تاریخ",
        TableColumnDataType.Date,
        "15%",
        true,
        new TableColumnFilter("ReceicePersianDate", TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        "PaymentTitle",
        "پرداخت",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("PaymentTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "FactorTitle",
        "برای",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("FactorTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "Description",
        "توضیحات",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("Description", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "AmountPay",
        "مبلغ",
        TableColumnDataType.Money,
        "15%",
        true,
        new TableColumnFilter("AmountPay", TableColumnFilterTypes.Money)
      )


    ];
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, '', true));

    await this.get()
  }




  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }
  add(param?: any) {
    throw new Error('Method not implemented.');
  }
  get(param?: any) {
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

  constructor() {
    super();
  }

  showExcelFileInGrid(){

  }




}
