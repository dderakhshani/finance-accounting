import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";

import { ActivatedRoute, Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { FormArray, FormGroup } from "@angular/forms";
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { Receipt } from '../../../entities/receipt';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import { TableComponent } from '../../../../../core/components/custom/table/table.component';
import { AddItemsCommand } from '../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { TableColumnFilterTypes } from '../../../../../core/components/custom/table/models/table-column-filter-types';
import { TableColumnFilter } from '../../../../../core/components/custom/table/models/table-column-filter';
import { GetQuantityCommodityQuery } from '../../../repositories/commodity/get-quantity-commodity';

@Component({
  selector: 'app-add-manual-items',
  templateUrl: './add-manual-items.component.html',
  styleUrls: ['./add-manual-items.component.scss']
})
export class ManualItemsComponent extends BaseComponent {
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('txtquantity', { read: TemplateRef }) txtquantity!: TemplateRef<any>;
  @ViewChild('checkboxWrong', { read: TemplateRef }) checkboxWrong!: TemplateRef<any>;
  @ViewChild('txtDescription', { read: TemplateRef }) txtDescription!: TemplateRef<any>;
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;
  

  tableConfigurations!: TableConfigurations;


  @Output() Additems = new EventEmitter<any>();
  @Input() receipt: Receipt | undefined = undefined;
  @Input() warehouseId: null | undefined = undefined;
  @Input() codeVoucherGroup: string = "";
  @Input() IsConsumable: boolean = false;
  @Input() IsAsset: boolean = false;

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
   
  constructor(private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
    private Service: PagesCommonService) {
    super(route, router);

  }


  get form1(): FormArray {
    return this.form as FormArray
  }

  async ngOnInit() {
  }


  async ngOnChanges() {
    this.resolve()
  }

  async resolve(params?: any) {

    let colCommodities = new TableColumn(
      'commodityId',
      'عنوان کالا',
      TableColumnDataType.Template,
      '25%',


    );
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '4%',
      false
    );
    let colQuantity = new TableColumn(
      'quantity',
      'مقدار (واحد اصلی)',
      TableColumnDataType.Template,
      '10%',
    );
    let colWrongMeasure = new TableColumn(
      'isWrongMeasure',
      'اشتباه در واحد کالا',
      TableColumnDataType.Template,
      '7%',
    );
    let Description = new TableColumn(
      'description',
      'توضیحات',
      TableColumnDataType.Template,
      '20%',
    );

    colDelete.template = this.btnDelete;
    colQuantity.template = this.txtquantity;
    colWrongMeasure.template = this.checkboxWrong;
    colCommodities.template = this.dropDowncommodities;
    Description.template = this.txtDescription;

    var columns: any = undefined;

    if (this.codeVoucherGroup == this.Service.CodeRequestBuy.toString() || this.codeVoucherGroup == this.Service.CodeLeaveReceipt.toString()) {
      columns = [
        new TableColumn('index', 'ردیف', TableColumnDataType.Index, '4%'),

        colCommodities,
        new TableColumn(
          'commodityCode',
          'کد کالا',
          TableColumnDataType.Text,
          '15%',
          true,
          new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
        ),
       
        colQuantity,
        new TableColumn(
          'remainQuantity',
          'تعداد مانده',
          TableColumnDataType.Number,
          '10%',
        ),
       
        new TableColumn(
          'quantityChose',
          'موجودی انبار',
          TableColumnDataType.Number,
          '10%',
        ),
        new TableColumn(
          'mainMeasureTitle',
          'واحد  کالا',
          TableColumnDataType.Text,
          '5%',

        ),
        Description,
       
        
        colDelete,

      ];
    }
    else {
      columns = [
        new TableColumn('index', 'ردیف', TableColumnDataType.Index, '4%'),

        colCommodities,
        new TableColumn(
          'commodityCode',
          'کد کالا',
          TableColumnDataType.Text,
          '15%',
          true,
          new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
        ),
        
       
        colQuantity,

        new TableColumn(
          'quantityChose',
          'موجودی انبار',
          TableColumnDataType.Number,
          '10%',
        ),
        new TableColumn(
          'mainMeasureTitle',
          'واحد  کالا',
          TableColumnDataType.Text,
          '10%',
        ),
        Description,
      
        colDelete,

      ];
    }

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true,true,true))
    this.tableConfigurations.options.exportOptions.showExportButton = false;

  }
  CommoditySelect(item: any, rowFg: any) {
    
    rowFg.controls.commodityId.setValue(item?.id);
    rowFg.controls.mainMeasureId.setValue(item?.measureId);
    rowFg.controls.mainMeasureTitle.setValue(item?.measureTitle);
    rowFg.controls.commodityCode.setValue(item?.code);
    rowFg.controls.documentMeasureId.setValue(item?.measureId);
    this.GetQuantityCommodity(rowFg);
  }
  onWrongMeasure(rowFg: any) {
    var _value = rowFg.controls.isWrongMeasure.value;
    rowFg.controls.isWrongMeasure.setValue(!_value);
  }
  onDelete(form: any) {

    this.Service.DeleteRow(form, this.form)
    this.child.tableRows = this.form.controls;

  }
  GetQuantityCommodity(rowFg: any) {
    if (this.warehouseId) {
      this._mediator.send(new GetQuantityCommodityQuery(this.warehouseId, rowFg.controls.commodityId.value)).then(rec => {
        rowFg.controls.quantityChose.setValue(rec);
      })
    }
   
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
