import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';


import { ActivatedRoute, Router } from "@angular/router";
import { FormArray, FormGroup } from "@angular/forms";
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { TableConfigurations} from '../../../../../core/components/custom/table/models/table-configurations';
import { invoice } from '../../../entities/invoice';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { measureUnit } from '../../../entities/invoice-item';
import { MeasureUnit } from '../../../../commodity/entities/measure-unit';
import { Commodity } from '../../../../commodity/entities/commodity';
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { GetMeasureUnitsQuery } from '../../../../commodity/repositories/measure-units/queries/get-measure-units-query';
import { BaseValueModel } from '../../../../inventory/entities/base-value';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { GeVatDutiesTaxValueQuery } from '../../../repositories/base-value/ge-vat-duties-tax-query';
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import { AddInvoiceItemCommand } from '../../../repositories/invoice/commands/invoice-item/add-invoice-item-command';
import { TableComponent } from '../../../../../core/components/custom/table/table.component';


@Component({
  selector: 'app-add-invoice-items',
  templateUrl: './add-invoice-items.component.html',
  styleUrls: ['./add-invoice-items.component.scss']
})
export class AddIncoiceItemsComponent extends BaseComponent {

  @ViewChild('txtquantity', { read: TemplateRef }) txtquantity!: TemplateRef<any>;
  @ViewChild('dropDowncommodities', { read: TemplateRef }) dropDowncommodities!: TemplateRef<any>;
  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('dropdownCurrency', { read: TemplateRef }) dropdownCurrency!: TemplateRef<any>;
  @ViewChild('txtUnitPrice', { read: TemplateRef }) txtUnitPrice!: TemplateRef<any>;
  @ViewChild('txtTotalPrice', { read: TemplateRef }) txtTotalPrice!: TemplateRef<any>;
  @ViewChild('checkboxWrong', { read: TemplateRef }) checkboxWrong!: TemplateRef<any>;

  public tableConfigurations!: TableConfigurations;
  public commodities: Commodity[] = [];
  public measureUnits: MeasureUnit[] = [];
  public commodityMeasureUnits: measureUnit[] = [];
  public currencyBaseValue: BaseValueModel[] = [];
  public isVate: boolean = false;
  public vatDutiesTax: number = 0;//مالیات ارزش افزوده
  public totalProductionCost: number = 0;//قیمت اقلام سند
  public totalItemPrice: number = 0; //قیمت قابل پرداخت
  public vatPercentage: number = 0;//درصد مالیات ارزش افزوده

  @Input() warehouseId: any = undefined;
  @Output() Additems = new EventEmitter<any>();
  @Input() set formSetter(form: FormArray) {
    if (form) {
      this.form = form;
      form.controls.forEach((control: any) => {
        (control as FormGroup).controls['mainMeasureId'].disable();
        (control as FormGroup).controls['commodityCode'].disable();
        (control as FormGroup).controls['mainMeasureTitle'].disable();


      })
    }

  }
  @Input() Invoice: invoice | undefined = undefined;

  child: any;
  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };
  constructor(private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
    private Service: PagesCommonService
  ) {
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



    //--------------------درصد مالیات ارزش افزوده----------------------------------

    this.vatPercentage = await this._mediator.send(new GeVatDutiesTaxValueQuery());


    let colcommodities = new TableColumn(
      'commodityId',
      'عنوان کالا',
      TableColumnDataType.Template,
      '25%',


    );
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '10%',
      false
    );
    let colquantity = new TableColumn(
      'quantity',
      'تعداد/مقدار (واحد اصلی)',
      TableColumnDataType.Template,
      '7%',
    );


    let colUnitPrice = new TableColumn(
      'unitPrice',
      'مبلغ واحد',
      TableColumnDataType.Template,
      '15%',
    );

    let colTotalPrice = new TableColumn(
      'productionCost',
      'مبلغ کل ریال',
      TableColumnDataType.Template,
      '20%',
    );
    let colWrongMeasure = new TableColumn(
      'isWrongMeasure',
      'اشتباه در واحد کالا',
      TableColumnDataType.Template,
      '10%',
    );

    colUnitPrice.template = this.txtUnitPrice;
    colTotalPrice.template = this.txtTotalPrice;
    colquantity.template = this.txtquantity;
    colcommodities.template = this.dropDowncommodities;
    colDelete.template = this.btnDelete;
    colWrongMeasure.template = this.checkboxWrong;

    let columns = [
      new TableColumn('index', 'ردیف', TableColumnDataType.Index),

      colcommodities,
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '15%',
      ),


      new TableColumn(
        'mainMeasureTitle',
        'واحد  کالا',
        TableColumnDataType.Text,
        '10%',

      ),
       new TableColumn(
        'unitBasePrice',
        'قیمت تخمینی',
        TableColumnDataType.Money,
        '7%',
      ),
      colWrongMeasure,
      colquantity,
      colUnitPrice,
      colTotalPrice,
      colDelete,

    ];

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))
    //-----در هنگام ویرایش تیک مالیات خورده باشد.
    if (this.vatDutiesTax > 0) {
      this.isVate = true;
    }


  }

  CommoditySelect(item: any, rowFg: any) {

    rowFg.controls.commodityId.setValue(item?.id);
    rowFg.controls.commodityCode.setValue(item?.code);
    rowFg.controls.documentMeasureId.setValue(item?.measureId);
    rowFg.controls.mainMeasureId.setValue(item?.measureId);
    rowFg.controls.mainMeasureTitle.setValue(item?.measureTitle);
  }


  //-----------------محاسبه قیمت های کل--------------------------
  onComputing(inputId: string = '', event: any) {

    if (event.keyCode == 13 && inputId != 'quantity') {
      this.onAdditems();
    }
    this.Computing(inputId)
  }
  Computing(inputId: string = '') {
    this.vatDutiesTax = 0;
    this.totalItemPrice = 0;
    this.totalProductionCost = 0;

    this.form.controls.forEach((control: any) => {
      var q = (control as FormGroup).controls.quantity.value;
      var p = (control as FormGroup).controls.productionCost.value;
      var u = (control as FormGroup).controls.unitPrice.value;

      if (inputId == 'unitPrice' && q != undefined && q != "") {

        (control as FormGroup).controls.productionCost.setValue(u * q);
      }
      else if (inputId == 'productionCost' && q != undefined && q != "") {

        var c = Math.round(p / q);
        (control as FormGroup).controls.unitPrice.setValue(c);
      }
      else if (inputId == 'quantity' && u != undefined && u != "") {
        (control as FormGroup).controls.productionCost.setValue(u * q);
      }

      this.totalProductionCost = Number(this.totalProductionCost) + Number((control as FormGroup).controls.productionCost.value);


    })
    if (this.isVate) {
      this.vatDutiesTax = (Number(this.vatPercentage) * Number(this.totalProductionCost)) / 100

    }
    this.totalItemPrice = this.totalProductionCost + this.vatDutiesTax;
  }
  onDelete(form: any) {

    this.Service.DeleteRow(form, this.form)
    this.child.tableRows = this.form;
  }
  onAdditems() {

    //آخرین سطر پر شده باشد و امکان ثبت جدید وجود داشته باشد
    var _value = (this.form.at(this.form.length - 1) as FormGroup);

    if (Number(_value.controls.productionCost.value) > 0) {
      this.Additems.emit();
    }


  }
  async AddItems() {
    var receiptDocumentItems = new AddInvoiceItemCommand();
    var list: FormArray = <FormArray>this.form;
    list.push(this.createForm(receiptDocumentItems));
    this.child.tableRows = list;
  }
  onWrongMeasure(rowFg: any) {
    var _value = rowFg.controls.isWrongMeasure.value;
    rowFg.controls.isWrongMeasure.setValue(!_value);
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
