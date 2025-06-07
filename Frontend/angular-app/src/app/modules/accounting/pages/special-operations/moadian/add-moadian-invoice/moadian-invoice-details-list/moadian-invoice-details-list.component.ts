import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {FormActionTypes} from "../../../../../../../core/constants/form-action-types";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableConfigurations} from "../../../../../../../core/components/custom/table/models/table-configurations";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {
  CodeVoucherExtendTypeDialogComponent
} from "../../../../base-values/code-voucher-extend-type/code-voucher-extend-type-dialog/code-voucher-extend-type-dialog.component";
import {
  MoadianInvoiceDetailDialogComponent
} from "../moadian-invoice-detail-dialog/moadian-invoice-detail-dialog.component";
import {FormGroup} from "@angular/forms";
import {PreDefinedActions} from "../../../../../../../core/components/custom/action-bar/action-bar.component";

@Component({
  selector: 'app-moadian-invoice-details-list',
  templateUrl: './moadian-invoice-details-list.component.html',
  styleUrls: ['./moadian-invoice-details-list.component.scss']
})
export class MoadianInvoiceDetailsListComponent extends BaseComponent {
  tableConfigurations!: TableConfigurations;

  constructor(
    public dialog: MatDialog,

  ) {
    super()
  }

  ngAfterViewInit() {
    super.ngAfterViewInit();
    this.actionBar.actions = [
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {
    await  this.resolve()
  }
  async resolve() {
    this.configTable()
  }

  configTable() {
    let columns = [
      <TableColumn>{
        name: 'index',
        title: 'ردیف',
        type: TableColumnDataType.Index,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'selected',
        title: '',
        type: TableColumnDataType.Select,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'sstt',
        title: 'شرح خدمت/کالا',
        type: TableColumnDataType.Text,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'sstid',
        title: 'شناسه خدمت/کالا',
        type: TableColumnDataType.Text,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'mu',
        title: 'واحد اندازه گیری',
        type: TableColumnDataType.Text,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'am',
        title: 'تعداد/مقدار',
        type: TableColumnDataType.Text,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'fee',
        title: 'مبلغ واحد',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'prdis',
        title: 'مبلغ قبل از تخفیف',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'dis',
        title: 'مبلغ تخفیف',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'adis',
        title: 'مبلغ بعد از تخفیف',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'vra',
        title: 'نرخ مالیات بر ارزش افزوده',
        type: TableColumnDataType.Number,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'vam',
        title: 'مبلغ مالیات بر ارزش افزوده',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
      <TableColumn>{
        name: 'tsstam',
        title: 'مبلغ کل کالا/خدمت',
        type: TableColumnDataType.Money,
        width: '2.5%',
      },
    ]


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true));
    this.tableConfigurations.options.useBuiltInPagination = true;
    this.tableConfigurations.options.useBuiltInFilters = true;
    this.tableConfigurations.options.useBuiltInSorting = true;
  }

  add(param?: any): any {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(MoadianInvoiceDetailDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe((response) => {
      if (response) {
        this.form.controls.push(this.createForm(response))
      }
    })
  }

  update(param?: any): any {
    let selectedInvoiceDetails = this.form.controls.filter((x:FormGroup) => x.value?.selected === true);
    if (selectedInvoiceDetails.length > 0) {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        pageMode: PageModes.Update,
        invoiceDetail: selectedInvoiceDetails[0]
      };

      let dialogReference = this.dialog.open(MoadianInvoiceDetailDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe((response) => {
        if (response) {
          this.form.controls.push(response)
        }
      })
    }
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  initialize(params?: any): any {
  }



}
