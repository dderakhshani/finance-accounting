import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";

import { ActivatedRoute, Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../../../core/components/custom/table/models/table-configurations";
import { FormArray, FormGroup } from "@angular/forms";
import { Mediator } from '../../../../../../../core/services/mediator/mediator.service';
import { Receipt } from '../../../../../entities/receipt';
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { DocumentItemsBomDialog } from '../../../../component/document-Items-bom-dialog/document-Items-bom-dialog.component';
import { PageModes } from '../../../../../../../core/enums/page-modes';
import { GetQuantityCommodityQuery } from '../../../../../repositories/commodity/get-quantity-commodity';
import { NotificationService } from '../../../../../../../shared/services/notification/notification.service';
import { TableColumnDataType } from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from "../../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../../core/components/custom/table/models/table-column";
import { AddItemsCommand } from '../../../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';
import { BomHeadersComponent } from '../../../../../../commodity/pages/bom-headers/bom-headers.component';
import { TableColumnFilterTypes } from '../../../../../../../core/components/custom/table/models/table-column-filter-types';
import { TableColumnFilter } from '../../../../../../../core/components/custom/table/models/table-column-filter';


@Component({
  selector: 'app-material-items',
  templateUrl: './material-items.component.html',
  styleUrls: ['./material-items.component.scss']
})
export class MaterialItemsComponent extends BaseComponent {


  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('txtquantity', { read: TemplateRef }) txtquantity!: TemplateRef<any>;
  @ViewChild('buttonStack', { read: TemplateRef }) buttonStack!: TemplateRef<any>;
  @ViewChild('dropDownBoms', { read: TemplateRef }) dropDownBoms!: TemplateRef<any>;
  @ViewChild('checkboxWrong', { read: TemplateRef }) checkboxWrong!: TemplateRef<any>;
  @ViewChild('txtdescription', { read: TemplateRef }) txtDescription!: TemplateRef<any>;
  @ViewChild('buttonBomValue', { read: TemplateRef }) buttonBomValue!: TemplateRef<any>;
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;

  tableConfigurations!: TableConfigurations;
  IsOpenModal: boolean = false;

  BomcommodityId: any = 0;

  @Output() Additems = new EventEmitter<any>();
  @Output() Get = new EventEmitter<any>();
  @Input() receipt: Receipt | undefined = undefined;
 
  @Input() codeVoucherGroup: string = "";
  @Input() fromWarehouseId: number = 0;
  @Input() toWarehouseId: number = 0;
  @Input() IsUpdateMode: boolean = false;
  public WarehouseList: number[] = [];

  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };
  @Input() set formSetter(form: FormArray) {
    if (form) {

      this.form = form;

    }

  }

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);

  }

  get form1(): FormArray {
    return this.form as FormArray
  }
  async ngOnInit() {
    this.resolve()
  }
  async ngAfterViewInit() {

  }
  async ngOnChanges() {


    await this.resolve()




  }
  async resolve(params?: any) {

    let colCommodities = new TableColumn(
      'commodityId',
      'عنوان کالا',
      TableColumnDataType.Template,
      '15%',
    );
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colQuantity = new TableColumn(
      'quantity',
      'مقدار (واحد اصلی)',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('quantity', TableColumnFilterTypes.Text),
    );
    let colBoms = new TableColumn(
      'bomsId',
      'فرمول ساخت',
      TableColumnDataType.Template,
      '22%',
    );
    let colWrongMeasure = new TableColumn(
      'isWrongMeasure',
      'اشتباه در واحد کالا',
      TableColumnDataType.Template,
      '10%',
    );
    let colBomValue = new TableColumn(
      'BomValue',
      'مواد اولیه',
      TableColumnDataType.Template,
      '7%',
    );
    let colCommodityCode = new TableColumn(
      'commodityCode',
      'کد کالا',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
    );
    let colDescription = new TableColumn(
      'description',
      'توضیحات',
      TableColumnDataType.Template,
      '20%',
      true,
      new TableColumnFilter('description', TableColumnFilterTypes.Text),
    );
    let fromQuantity = new TableColumn(
      'quantityChose',
      'موجودی انبار تحویل دهنده',
      TableColumnDataType.Money,
      '8%',
      true,
      new TableColumnFilter('quantityChose', TableColumnFilterTypes.Text),

    );
      let toQuantity = new TableColumn(
        'secondaryQuantity',
        'موجودی انبار تحویل گیرنده',
        TableColumnDataType.Money,
        '8%',
        true,
        new TableColumnFilter('secondaryQuantity', TableColumnFilterTypes.Text),

    )
    let colMeasureTitle = new TableColumn(
      'mainMeasureTitle',
      'واحد  کالا',
      TableColumnDataType.Text,
      '5%',
      true,
      new TableColumnFilter('mainMeasureTitle', TableColumnFilterTypes.Text),

    );
    colQuantity.template = this.txtquantity;
    colCommodities.template = this.dropDowncommodities;
    colDelete.template = this.btnDelete;
    colWrongMeasure.template = this.checkboxWrong;
    colBoms.template = this.dropDownBoms;
    colBomValue.template = this.buttonBomValue;
    colCommodityCode.template = this.buttonStack;
    colDescription.template = this.txtDescription;
    var columns: any = undefined;
    if (this.codeVoucherGroup == this.Service.ProductReceiptWarehouse) {
      columns = [
        new TableColumn('index', 'ردیف', TableColumnDataType.Index, '3%'),
        colCommodities,
        colCommodityCode,
        toQuantity,
        colQuantity,

      ];
      if (this.IsUpdateMode == false) {
        columns.push(colBoms, colDelete)
      }
      if (this.IsUpdateMode == true) {
        columns.push(colBoms, colBomValue, colDelete)
      }
    }
    if (this.codeVoucherGroup != this.Service.ProductReceiptWarehouse) {
      columns = [
        new TableColumn('index', 'ردیف', TableColumnDataType.Index, '3%'),
        colCommodities,
        colCommodityCode,
        fromQuantity,
        toQuantity,
        colQuantity,
        colDescription,
        colDelete,

      ];

    }

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true,true,true))

  }
  CommoditySelect(item: any, rowFg: any) {
   
    rowFg.controls.commodityId.setValue(item?.id);
    rowFg.controls.mainMeasureId.setValue(item?.measureId);
    rowFg.controls.commodityCode.setValue(item?.code);
    rowFg.controls.documentMeasureId.setValue(item?.measureId);
    rowFg.controls.mainMeasureTitle.setValue(item?.measureTitle);

    this.GetFromWarehouse(rowFg);
    this.GetToWarehouse(rowFg);

  }

  GetFromWarehouse(rowFg: any) {
    this._mediator.send(new GetQuantityCommodityQuery(this.fromWarehouseId, rowFg.controls.commodityId.value)).then(rec => {
      rowFg.controls.quantityChose.setValue(rec);
    })
  }

  GetToWarehouse(rowFg: any) {
    this._mediator.send(new GetQuantityCommodityQuery(this.toWarehouseId, rowFg.controls.commodityId.value)).then(rec => {
      rowFg.controls.secondaryQuantity.setValue(rec);
    })
  }

  CommodityBomsSelect(item: any, rowFg: any) {


    this.WarehouseList.push(item?.bomWarehouseId);

    var duplicate = this.WarehouseList.filter(a => a != item?.bomWarehouseId && a != undefined && item?.bomWarehouseId != undefined)
    if (duplicate.length > 0) {
      this.Service.showWarrningMessage('کالای انتخابی در انبار مبدا متفاوت می باشد ، لطفا به ثبت رسید دیگری نمایید');

      return

    } else {
      
      this.fromWarehouseId = item?.bomWarehouseId;
     
      rowFg.controls.bomValueHeaderId.setValue(item?.bomsHeaderId);
      this.GetFromWarehouse(rowFg);
      this.GetToWarehouse(rowFg);

    }
  }
  onWrongMeasure(rowFg: any) {
    var _value = rowFg.controls.isWrongMeasure.value;
    rowFg.controls.isWrongMeasure.setValue(!_value);
  }
  onDelete(form: any) {

    this.Service.DeleteRow(form, this.form)
    this.child.tableRows = this.form;

  }
  onAdditems(event: any, keyClick: string) {

    let valid = false;
    if (event.keyCode == 13 && keyClick == 'quantity') {
      valid = true;
    }
    if (keyClick == 'delete') {
      valid = true;
    }
    if (valid) {
      //آخرین سطر پر شده باشد و امکان ثبت جدید وجود داشته باشد
      var _value = (this.form.at(this.form.length - 1) as FormGroup);
      if (Number(_value.controls.quantity.value) > 0 && _value.controls.commodityId.value > 0) {

        //از کامپونت ولد استفاده شده است به این دلیل که تعداد سطرهای اضافه شده شمرده و شود و بتوان از بیرون هم سط به گرید اضافه کرد.  
        this.Additems.emit();
      }
    }

  }
  async AddItems() {


    var receiptDocumentItems = new AddItemsCommand();
    var list: FormArray = <FormArray>this.form;
    list.push(this.createForm(receiptDocumentItems));
    this.child.tableRows = list;


  }
  async getBomValue(item: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      documentItemsId: item.controls.id.value,
      allowViewPrice: false,
      pageMode: PageModes.Update

    };

    let dialogReference = this.dialog.open(DocumentItemsBomDialog, dialogConfig);

    dialogReference.afterClosed().subscribe(async result => {

      this.Get.emit();
    });

  }
  openAddBoms(item: number) {
    this.BomcommodityId = item;

    this.IsOpenModal = true;
  }
  async navigateToHistory(commodityId: any) {
   
    if (this.fromWarehouseId > 0) {
      this.router.navigateByUrl(`inventory/commodityReceiptReports?commodityId=${commodityId}&warehouseId=${this.fromWarehouseId}`)
    }
    else {
      this.router.navigateByUrl(`inventory/commodityReceiptReports?commodityId=${commodityId}`)
    }
    


  }
  onNotAllowExit(item: any) {
    return (item.controls.quantityChose.value <= 0 && this.codeVoucherGroup != this.Service.ProductReceiptWarehouse && this.codeVoucherGroup != this.Service.SemiFinishedMaterialsDirectReceip) && !this.IsUpdateMode
  }
  onAllowExit(item: any) {
    return item.controls.quantityChose.value > 0 || this.codeVoucherGroup == this.Service.ProductReceiptWarehouse || this.codeVoucherGroup == this.Service.SemiFinishedMaterialsDirectReceip || this.IsUpdateMode
  }
  add(param?: any): any {

  }

  close(): any {
    this.IsOpenModal = false;
    this.BomcommodityId.controls.bomValueHeaderId.setValue(0)

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
