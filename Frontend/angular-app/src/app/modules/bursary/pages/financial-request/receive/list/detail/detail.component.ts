import { Component, Inject, OnInit } from '@angular/core';
import { FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { TableConfigurations} from 'src/app/core/components/custom/table/models/table-configurations';
import { FormActionTypes } from 'src/app/core/constants/form-action-types';
import { FormAction } from 'src/app/core/models/form-action';
import { DocumentHead } from 'src/app/modules/accounting/entities/document-head';
import { FinancialRequestDetail } from 'src/app/modules/bursary/entities/financial-detail';
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.scss']
})
export class DetailComponent extends BaseComponent {


  documentDetails : FinancialRequestDetail[] = [];


  tableConfigurations!: TableConfigurations;

  listActions: FormAction[] = [
    FormActionTypes.add,
    FormActionTypes.edit,
    FormActionTypes.refresh,
    FormActionTypes.delete
  ]

  constructor(    private route: ActivatedRoute,
    public dialogRef: MatDialogRef<DetailComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public data: any)
     {super() }

  ngOnInit(): void {
      this.resolve();

  }



  async resolve() {
    this.documentDetails = JSON.parse(this.data.data);
    this.getFinancialDetails(this.documentDetails)
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

  getFinancialDetails(data: FinancialRequestDetail[]) {

    let columns: TableColumn[] = [

      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        "amount",
        "مبلغ",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter(
          "amount",
          TableColumnFilterTypes.Text
        )
      ),
      new TableColumn(
        "creditAccountReferenceGroupTitle",
        "گروه بستانکار",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceGroupTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "creditAccountReferenceTitle",
        "تفصیل بستانکار",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("creditAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "debitAccountReferenceGroupTitle",
        "گروه بدهکار",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountReferenceGroupTitle", TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        "debitAccountReferenceTitle",
        "تفصیل بدهکار",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("debitAccountReferenceTitle", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "description",
        "توضیحات",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("description", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "createName",
        "ثبت کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("createName", TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        "modifyName",
        "ویرایش کننده",
        TableColumnDataType.Text,
        "15%",
        true,
        new TableColumnFilter("modifyName", TableColumnFilterTypes.Text)
      ),

    ];
    this.tableConfigurations = new TableConfigurations(
      columns,
      new TableOptions(true, true)
    );
    this.form = new FormArray(data.map((x) => this.createForm(x)));
  }

}
