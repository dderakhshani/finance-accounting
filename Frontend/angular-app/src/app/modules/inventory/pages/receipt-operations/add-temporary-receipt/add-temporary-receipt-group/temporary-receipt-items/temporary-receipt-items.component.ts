import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";

import { ActivatedRoute, Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../../../core/components/custom/table/models/table-configurations";
import { FormArray, FormGroup } from "@angular/forms";
import { MeasureUnit } from "../../../../../../commodity/entities/measure-unit";

import { Mediator } from '../../../../../../../core/services/mediator/mediator.service';
import { Receipt } from '../../../../../entities/receipt';
import { AccountReference } from '../../../../../../accounting/entities/account-reference';
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { AddCommoditySerialDialog } from '../../../../component/commodity-serial-dialog/add-commodity-serial-dialog.component';
import { measureUnit } from '../../../../../entities/receipt-item';
import { Assets, AssetsSerial } from '../../../../../entities/Assets';
import {TableColumnFilter} from "../../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import {
  TableColumnFilterTypes
} from "../../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../../core/components/custom/table/models/table-column";
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';


@Component({
  selector: 'app-temporary-receipt-items',
  templateUrl: './temporary-receipt-items.component.html',
  styleUrls: ['./temporary-receipt-items.component.scss']
})
export class TemporaryReceiptItemsComponent extends BaseComponent {
  tableConfigurations!: TableConfigurations;

  measureUnits: MeasureUnit[] = [];
  commodityMeasureUnits: measureUnit[] = [];
  @ViewChild('txtquantity', { read: TemplateRef }) txtquantity!: TemplateRef<any>;
  @ViewChild('txtInvoceNo', { read: TemplateRef }) txtInvoceNo!: TemplateRef<any>;
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('checkboxSelected', { read: TemplateRef }) checkboxSelected!: TemplateRef<any>;
  @ViewChild('checkboxWrong', { read: TemplateRef }) checkboxWrong!: TemplateRef<any>;
  @ViewChild('txtcommodityTitle', { read: TemplateRef }) txtcommodityTitle!: TemplateRef<any>;
  @ViewChild('txtDescription', { read: TemplateRef }) txtDescription!: TemplateRef<any>;
  @ViewChild('txtRequesterReferenceTitle', { read: TemplateRef }) txtRequesterReferenceTitle!: TemplateRef<any>;
  @ViewChild('dropDownAccountReferences', { read: TemplateRef }) dropDownAccountReferences!: TemplateRef<any>;


  accountReferences: AccountReference[] = [];
  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };
  constructor(private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
    public Service: PagesCommonService,
    public dialog: MatDialog
  ) {
    super(route, router);

  }

  @Input() set formSetter(form: FormArray) {
    if (form) {
      this.form = form;
    }

  }
  @Input() receipt: Receipt | undefined = undefined;
  @Input() codeVoucherGroup: string | undefined = undefined;
  @Input() documentDate: string = new Date().toISOString();
  async ngOnInit() {


  }

  async ngAfterViewInit() {

    this.resolve()

  }
  async resolve(params?: any) {



    let InvoceNo = new TableColumn(
      'invoceNo',
      'شماره صورتحساب',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('invoceNo', TableColumnFilterTypes.Text,true)

    );
    let AccountReferences = new TableColumn(
      'referenceId',
      'تامین کننده',
      TableColumnDataType.Template,
      '25%',
      true,
      new TableColumnFilter('referenceId', TableColumnFilterTypes.Number, true)

    );
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '10%',
      false
    );
    let quantity = new TableColumn(
      'quantity',
      'مقدار (واحد اصلی)',
      TableColumnDataType.Template,
      '7%',
      true,
      new TableColumnFilter('quantity', TableColumnFilterTypes.Number, true)
    );

    let colOk = new TableColumn(
      'selected',
      '',
      TableColumnDataType.Template,
      '5%',
    );

    let colWrongMeasure = new TableColumn(
      'isWrongMeasure',
      'اشتباه در واحد کالا',
      TableColumnDataType.Template,
      '6%',
    );
    let colCommodityTitle = new TableColumn(
      'commodityTitle',
      'نام کالا',
      TableColumnDataType.Template,
      '14%',
      true,
      new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text, true)
    );
    let colDescription = new TableColumn(
      'description',
      'شرح',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('description', TableColumnFilterTypes.Text, true)
    );
    let colRequesterReferenceTitle =new TableColumn(
      'requesterReferenceTitle',
      'درخواست دهنده',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text, true)
    );
    quantity.template = this.txtquantity;
    InvoceNo.template = this.txtInvoceNo;
    AccountReferences.template = this.dropDownAccountReferences;
    colDelete.template = this.btnDelete;
    colOk.template = this.checkboxSelected;
    colWrongMeasure.template = this.checkboxWrong;
    colCommodityTitle.template = this.txtcommodityTitle;
    colDescription.template = this.txtDescription;
    colRequesterReferenceTitle.template = this.txtRequesterReferenceTitle;
    let columns = [
      new TableColumn('id', 'ردیف', TableColumnDataType.Index, '2%', true, new TableColumnFilter('id', TableColumnFilterTypes.Number)),

      new TableColumn(
        'requestNo',
        'شماره درخواست',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text, true)
      ),

      colCommodityTitle,
      
      //colRequesterReferenceTitle,

      //new TableColumn(
      //  'mainMeasureTitle',
      //  'واحد کالا',
      //  TableColumnDataType.Text,
      //  '5%',
      //  true,
      //  new TableColumnFilter('mainMeasureTitle', TableColumnFilterTypes.Text)
      //),
      colWrongMeasure,
      quantity,
      InvoceNo,
      AccountReferences,
      colDescription,
      colOk,

      colDelete,
    ];
    var tableOptions = new TableOptions();
    tableOptions.useBuiltInSorting = true;
    tableOptions.useBuiltInFilters = true;
    tableOptions.editRowOnDoubleClick = false;
    tableOptions.usePagination = true;
    tableOptions.useBuiltInPagination = true;
    this.tableConfigurations = new TableConfigurations(columns, tableOptions)

  }
  ReferenceSelect(item: any, rowFg: any) {

    rowFg.controls.creditAccountReferenceId.setValue(item?.id);
    rowFg.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  onWrongMeasure(rowFg: any) {
    var _value = rowFg.controls.isWrongMeasure.value;
    rowFg.controls.isWrongMeasure.setValue(!_value);
  }
  onDelete(form: any) {

    this.Service.DeleteRowById(form, this.form)
    this.child.tableRows = this.form;
  }
  async AddItems() {

    this.child.tableRows = this.form;
  }
  //===========================================================================
  //----------------------در سندهایی که درخواست خرید در سیستم ثبت شده باشد
  invoiceNoSelect(invoice: any, rowFg: any) {

    //-----------درحالتی که از لیست پیشنهادی قراردادها یا پیش فاکتورهای انتخاب شود
    if (invoice?.documentNo != undefined) {

      rowFg.controls.invoiceNo.setValue(invoice?.documentNo);
      rowFg.controls.creditAccountReferenceId.setValue(invoice?.creditAccountReferenceId);
      rowFg.controls.creditAccountReferenceGroupId.setValue(invoice.creditAccountReferenceGroupId);
      var invoiceItem = invoice?.items;

      var item = invoiceItem.find((a: { commodityId: any; }) => a.commodityId == rowFg.controls.commodityId.value);
      if (item != undefined) {

        rowFg.controls.unitPrice.setValue(item.unitPrice);
        rowFg.controls.unitBasePrice.setValue(item.unitPrice);
      }


    }
    else {//در حالتی که روی کد خود input چیزی تایپ شود.
      rowFg.controls.invoiceNo.setValue(invoice)
    }


  }
  //===========================================================================
  //----------------------در سندهای مربوط به اموال شناسه های اموال دریافت شود
  async addCommoditySerials(rowFg: any) {

    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      commodityCode: rowFg.controls.commodityCode.value,
      commodityId: rowFg.controls.commodityId.value,
      commodityTitle: rowFg.controls.commodityTitle.value,
      quantity: rowFg.controls.quantity.value,
      assets: rowFg.controls['assets'].value

    };

    let dialogReference = this.dialog.open(AddCommoditySerialDialog, dialogConfig);
    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      var Assets: Assets = response;
      if (Assets.assetsSerials !=undefined) {
        Assets?.assetsSerials.forEach(item => {

          rowFg.controls['assets'].setValue(Assets);

        })
        rowFg.controls.commoditySerial.setValue(response.length);
        rowFg.controls['quantity'].disable();
      }


    })
  }
  //----------------------------------------------------------------------------
  add(param?: any): any {

  }

  close()
    :
    any {
  }

  delete(param?: any)
    :
    any {
  }

  get(param?: any)
    :
    any {
  }

  initialize(params?: any)
    :
    any {
  }


  update(param?: any)
    :
    any {
  }
  
}
