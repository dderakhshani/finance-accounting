import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../../core/services/mediator/mediator.service";
import { ActivatedRoute, Router } from "@angular/router";
import {
  TableConfigurations
} from "../../../../../../../core/components/custom/table/models/table-configurations";
import { FormArray, FormControl, FormGroup } from "@angular/forms";

import { Receipt } from '../../../../../entities/receipt';
import { BaseValueModel } from '../../../../../entities/base-value';
import { PagesCommonService } from '../../../../../../../shared/services/pages/pages-common.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from '../../../../../../../core/enums/page-modes';

import {
  DocumentItemsBomDialog
} from '../../../../component/document-Items-bom-dialog/document-Items-bom-dialog.component';
import { TableColumnDataType } from "../../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from "../../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../../core/components/custom/table/models/table-column";

import {
  UpdateQuantityDialogComponent
} from '../../../../component/update-quantity-dialog/update-quantity-dialog.component';
import { GetCurrencyBaseValueQuery } from '../../../../../repositories/base-value/get-base-value-currency-query';
import { elementAt } from 'rxjs/operators';
import { TableComponent } from '../../../../../../../core/components/custom/table/table.component';
import {
  SplitCommodityQuantityDialogComponent
} from '../../../../component/split-quantity-dialog/split-quantity-dialog.component';
import { GetAssetsByDocumentIdQuery } from '../../../../../repositories/assets/queries/get-assets-by-documentId';
import {
  CommoditySerialViewDialog
} from '../../../../component/commodity-serial-view-dialog/commodity-serial-view-dialog.component';
import {
  TableColumnFilterTypes
} from '../../../../../../../core/components/custom/table/models/table-column-filter-types';
import { TableColumnFilter } from '../../../../../../../core/components/custom/table/models/table-column-filter';


@Component({
  selector: 'app-Rials-receipt-items',
  templateUrl: './Rials-receipt-items.component.html',
  styleUrls: ['./Rials-receipt-items.component.scss']
})
export class RialsReceiptItemsComponent extends BaseComponent {

  @ViewChild('btnDelete', { read: TemplateRef }) btnDelete!: TemplateRef<any>;
  @ViewChild('txtUnitPrice', { read: TemplateRef }) txtUnitPrice!: TemplateRef<any>;
  @ViewChild('txtRatePrice', { read: TemplateRef }) txtRatePrice!: TemplateRef<any>;
  @ViewChild('txtTotalPrice', { read: TemplateRef }) txtTotalPrice!: TemplateRef<any>;
  @ViewChild('txtsumCurrencyPrice', { read: TemplateRef }) txtsumCurrencyPrice!: TemplateRef<any>;
  @ViewChild('buttonBomValue', { read: TemplateRef }) buttonBomValue!: TemplateRef<any>;
  @ViewChild('txtDescription', { read: TemplateRef }) txtDescription!: TemplateRef<any>;
  @ViewChild('dropdownCurrency', { read: TemplateRef }) dropdownCurrency!: TemplateRef<any>;
  @ViewChild('txtCurrencyPrice', { read: TemplateRef }) txtCurrencyPrice!: TemplateRef<any>;
  @ViewChild('txtCommodity', { read: TemplateRef }) txtCommodity!: TemplateRef<any>;
  @ViewChild('buttonEditQuantity', { read: TemplateRef }) buttonEditQuantity!: TemplateRef<any>;
  @ViewChild('txtAddCurrencyPrice', { read: TemplateRef }) txtAddCurrencyPrice!: TemplateRef<any>;


  tableConfigurations!: TableConfigurations;
  currencyBaseValue: BaseValueModel[] = [];

  public extraCost: number = 0;
  public _sumCurrencyPrice: number = 0;
  public sumCurrencyPrice: number = 0;
  public isVate: boolean = false;
  public isNegative: boolean = false;
  public isFreightChargePaid: boolean = false;
  
  //محاسبات سیستمی انجام شود=100
  //101=محاسبات دستی انجام شود
  public vatDutiesTax: number = 0;//مالیات ارزش افزوده
  public vatPercentage: number = 0;//درصد مالیات ارزش افزوده
  public voucherHeadId: number = 0;//سند مکانیزه صادر شده است یا خیر؟
  public totalItemPrice: number = 0; //قیمت قابل پرداخت
  public totalProductionCost: number = 0;//قیمت اقلام سند
  public allowNotEditInvoice: boolean = this.Service.identityService.doesHavePermission('allowNotEditInvoice')
  public isEditQuantity: boolean = true;
  previousValues: { [key: string]: string } = {};

  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
  ) {
    super(route, router);

  }

  @Input() receipt: Receipt | undefined = undefined;
  @Input() extraCostRialTemp: number = 0;
  @Input() extraCostCurrency: number = 0;
  @Input() calculationType: number = 100;
  @Output() getData = new EventEmitter<any>();

  isImportPurchase = this.getQueryParam('isImportPurchase')

  child: any;

  @ViewChild(TableComponent)
  set appShark(child: TableComponent) {
    this.child = child
  };

  @Input() set formSetter(form: FormArray) {
    if (form) {


      this.form = form;
      form.controls.forEach((control: any) => {
        (control as FormGroup).controls['mainMeasureId'].disable();
        (control as FormGroup).controls['commodityTitle'].disable();
        (control as FormGroup).controls['commodityCode'].disable();
        (control as FormGroup).controls['mainMeasureTitle'].disable();
        (control as FormGroup).controls['unitBasePrice'].disable();

      })

    }

  }

  //----------------------در سندهای مربوط به اموال شناسه های اموال دریافت شود
  async CommoditySerials(rowFg: any) {

    let dialogConfig = new MatDialogConfig();
    let assetsList: any = undefined;


    await this._mediator.send(new GetAssetsByDocumentIdQuery(rowFg.controls.id.value, rowFg.controls.commodityId.value)).then(res => {

      assetsList = res
    });

    dialogConfig.data = {
      commodityCode: rowFg.controls.commodityCode.value,
      commodityId: rowFg.controls.commodityId.value,
      commodityTitle: rowFg.controls.commodityTitle.value,
      quantity: rowFg.controls.quantity.value,
      documentItemId: rowFg.controls.id.value,
      assets: assetsList

    };

    let dialogReference = this.dialog.open(CommoditySerialViewDialog, dialogConfig);
    dialogReference.afterClosed();
  }

  get form1(): FormArray {

    return this.form as FormArray

  }

  async ngOnInit() {
    this._mediator.send(new GetCurrencyBaseValueQuery()).then(result => {
      this.currencyBaseValue = result.data
    });

  }

  ngAfterViewInit() {

    this.resolve()
  }

  async resolve(params?: any) {

    let Quantity = new TableColumn(
      'quantity',
      'تعداد کالا',
      TableColumnDataType.Template,
      '5%',
      true,
      new TableColumnFilter('quantity', TableColumnFilterTypes.Text),
    );
    let UnitPrice = new TableColumn(
      'unitPrice',
      'قیمت واحد ریالی',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('unitPrice', TableColumnFilterTypes.Text),
    );

    let Description = new TableColumn(
      'description',
      'توضیحات',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('description', TableColumnFilterTypes.Text),
    );

    let TotalPrice = new TableColumn(
      'productionCost',
      'مبلغ کل ریالی',
      TableColumnDataType.Template,
      '18%',
      true,
      new TableColumnFilter('productionCost', TableColumnFilterTypes.Text),
    );
    let BomValue = new TableColumn(
      'BomValue',
      'جزئیات / بارکد',
      TableColumnDataType.Template,
      '8%',
      false
    );
    let dropdownCurrency = new TableColumn(
      'currencyBaseId',
      'واحد ارز',
      TableColumnDataType.Template,
      '5%',
    );
    let CurrencyPrice = new TableColumn(
      'currencyPrice',
      'فی ارز',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('currencyPrice', TableColumnFilterTypes.Text),
    );
    let sumCurrencyPrice = new TableColumn(
      'sumCurrencyPrice',
      'جمع ارزی',
      TableColumnDataType.Template,
      '12%',
      true,
      new TableColumnFilter('sumCurrencyPrice', TableColumnFilterTypes.Text),
    );

    let CurrencyRate = new TableColumn(
      'currencyRate',
      'ضریب تبدیل',
      TableColumnDataType.Template,
      '5%',
    );
    let unitBasePrice = new TableColumn(
      'unitBasePrice',
      'مبلغ تخمینی',
      TableColumnDataType.Money,
      '8%',
      false,
    )
    let commodityTitle = new TableColumn(
      'commodityTitle',
      'نام کالا',
      TableColumnDataType.Template,
      '14%',
      true,
      new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text),
    )
    let addCurrencyPrice = new TableColumn(
      'addCurrencyPrice',
      'هزینه ارزی',
      TableColumnDataType.Template,
      '8%',
      true,
      new TableColumnFilter('addCurrencyPrice', TableColumnFilterTypes.Text),
    )
    let addCostPrice = new TableColumn(
      'addCurrencyPrice',
      'مبلغ تسهیم',
      TableColumnDataType.Money,
      '8%',
      true,
      new TableColumnFilter('addCurrencyPrice', TableColumnFilterTypes.Text),
    )

    let colRequestNo = new TableColumn(
      'requestNo',
      'شماره درخواست',
      TableColumnDataType.Number,
      '5%',
      true,
      new TableColumnFilter('requestNo', TableColumnFilterTypes.Text),
    )
    let colUnitPriceWithExtraCost = new TableColumn(
      'unitPriceWithExtraCost',
      'قیمت تمام شده',
      TableColumnDataType.Money,
      '8%',
    )
    let colDelete = new TableColumn(
      'delete',
      'حذف',
      TableColumnDataType.Template,
      '4%',
      false
    );

    colDelete.template = this.btnDelete;
    UnitPrice.template = this.txtUnitPrice;
    BomValue.template = this.buttonBomValue;
    CurrencyRate.template = this.txtRatePrice;
    TotalPrice.template = this.txtTotalPrice;
    Description.template = this.txtDescription;
    commodityTitle.template = this.txtCommodity;
    Quantity.template = this.buttonEditQuantity;
    dropdownCurrency.template = this.dropdownCurrency;
    CurrencyPrice.template = this.txtCurrencyPrice;
    sumCurrencyPrice.template = this.txtsumCurrencyPrice;
    addCurrencyPrice.template = this.txtAddCurrencyPrice;
    addCostPrice.template = this.txtAddCurrencyPrice;

    let columns: any[] = [];
    columns = [

      new TableColumn(
        'documentNo',
        'شماره رسید',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Text),
      ),

      commodityTitle,
      Quantity,

    ];


    if (this.isImportPurchase == 'true' && !this.allowNotEditInvoice) {
      /*columns.push(dropdownCurrency)*/

      columns.push(CurrencyPrice)
      columns.push(sumCurrencyPrice)
      columns.push(addCurrencyPrice)
      columns.push(CurrencyRate)
      columns.push(UnitPrice);
      columns.push(TotalPrice);
      columns.push(Description);
      /*columns.push(unitBasePrice);*/
      columns.push(colRequestNo);
      columns.push(BomValue);
      /* columns.push(colDelete);*/


    }

    if (this.isImportPurchase == 'false' && !this.allowNotEditInvoice) {
      columns.push(addCostPrice)
      columns.push(UnitPrice);
      columns.push(TotalPrice);
      columns.push(Description)
      columns.push(unitBasePrice);
      columns.push(colRequestNo);
      columns.push(colUnitPriceWithExtraCost);
      columns.push(BomValue);
      //columns.push(colDelete);


    }

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true, true, true))
    this.tableConfigurations.options.showSumRow = true;
    this.tableConfigurations.pagination.pageSize = 1000

    this.tableConfigurations.options.exportOptions.showExportButton = false

  }

  ImportPurchaseComputeCostRate() {

    this._sumCurrencyPrice = 0;
    this.sumCurrencyPrice = 0;
    let w = this.extraCostCurrency;
    let negative = this.isNegative ? -1 : 1;

    this.form.controls.forEach((control: any) => {

      var q = (control as FormGroup).controls.quantity.value;
      var c_p = (control as FormGroup).controls.currencyPrice.value;
      var c_R = (control as FormGroup).controls.currencyRate.value;

      if (!c_R) {
        (control as FormGroup).controls.currencyRate.setValue(1);
      }

      this._sumCurrencyPrice += (c_p * q);


    })


    let x = this._sumCurrencyPrice + Number(w);
    let z = this.extraCostRialTemp;
    let y = Number(Number(z) / Number(x));


    this.form.controls.forEach((control: any) => {

      var q = (control as FormGroup).controls.quantity.value;
      var c_p = (control as FormGroup).controls.currencyPrice.value;

      var total_c_p = c_p * q;
      var f = total_c_p / this._sumCurrencyPrice;
      var s = f * w;

      var negative_s: number = Number((negative) * s);
      (control as FormGroup).controls.currencyRate.setValue(parseFloat(y.toFixed(6)));
      (control as FormGroup).controls.addCurrencyPrice.setValue(negative_s);

      (control as FormGroup).controls.sumCurrencyPrice.setValue(Number(q * c_p + negative_s));


      this.sumCurrencyPrice = this.sumCurrencyPrice + Number(q * c_p + negative_s);

    })

  }

  ImportPurchaseComputeCostRial() {

    this._sumCurrencyPrice = 0;
    let w = this.extraCostCurrency;
    let negative = this.isNegative ? -1 : 1;
    let e = this.extraCost;


    this.form.controls.forEach((control: any) => {

      var q = (control as FormGroup).controls.quantity.value;
      var u = (control as FormGroup).controls.unitPrice.value;


      this._sumCurrencyPrice += (u * q);

    })

    this.form.controls.forEach((control: any) => {

      var q = (control as FormGroup).controls.quantity.value;
      var u = (control as FormGroup).controls.unitPrice.value;
      var u_c = (control as FormGroup).controls.unitPriceWithExtraCost.value;


      var total_u = u * q;
      var f = total_u / this._sumCurrencyPrice;
      var s = f * w / q;

      var a = f * e / q;


      (control as FormGroup).controls.addCurrencyPrice.setValue((negative) * s);

      let unitPriceWithExtraCost: Number = this.isFreightChargePaid ? Number(u + a + ((negative) * s)) : Number(u + ((negative) * s));

      (control as FormGroup).controls.unitPriceWithExtraCost.setValue(unitPriceWithExtraCost);


    })

  }

  //-----------------محاسبه قیمت های کل--------------------------
  onComputing(inputId: string = '') {


    this.vatDutiesTax = 0;
    this.totalItemPrice = 0;
    this.totalProductionCost = 0;

    if (this.isImportPurchase)
      this.ImportPurchaseComputeCostRate();
    if (!this.isImportPurchase && inputId != "productionCost")
      this.ImportPurchaseComputeCostRial();

    this.form.controls.forEach((control: any) => {

      var q = (control as FormGroup).controls.quantity.value;
      var p = (control as FormGroup).controls.productionCost.value;
      var u = (control as FormGroup).controls.unitPrice.value;
      var c_p = (control as FormGroup).controls.currencyPrice.value;
      var c_R = (control as FormGroup).controls.currencyRate.value;
      var a_c = (control as FormGroup).controls.addCurrencyPrice.value;
      var total = (control as FormGroup).controls.sumCurrencyPrice.value;


      if (inputId == 'quantity' && u != undefined && u != "") {
        let unit = Number(u * q) + Number(a_c);
        (control as FormGroup).controls.productionCost.setValue(parseFloat(unit.toFixed(4)));

      } else if (inputId == 'unitPrice' && q != undefined && q != "") {
        let unit = Number(u * q) + Number(a_c);
        (control as FormGroup).controls.productionCost.setValue(parseFloat(unit.toFixed(4)));
        let c_p = Number(u / c_R);
        (control as FormGroup).controls.currencyPrice.setValue(parseFloat(c_p.toFixed(6)));
      }

      if (inputId == 'productionCost' && q != undefined && q != "") {
        let unit = Number(p / q);
        (control as FormGroup).controls.unitPrice.setValue(parseFloat(unit.toFixed(4)));
        let c_p = Number(u / c_R);
        (control as FormGroup).controls.currencyPrice.setValue(parseFloat(c_p.toFixed(6)));
        this.ImportPurchaseComputeCostRial();

      } else if (inputId == 'currencyPrice' && q != undefined && q != "" && c_R != undefined && c_R != "") {

        let q_d = total / q;
        let unit = Math.round(c_R * Number(q_d));
        (control as FormGroup).controls.unitPrice.setValue(unit);
        (control as FormGroup).controls.productionCost.setValue(total * c_R);

      }
      if (inputId == 'currencyRate' && q != undefined && q != "" && c_p != undefined && c_p != "") {

        let q_d = total / q;
        let unit = Math.round(c_R * Number(q_d));

        (control as FormGroup).controls.unitPrice.setValue(parseFloat(unit.toFixed(4)));
        (control as FormGroup).controls.productionCost.setValue(total * c_R);

      }
      //--------------------------------------------------------------------------
      q = (control as FormGroup).controls.quantity.value;
      u = (control as FormGroup).controls.unitPrice.value;
      c_R = (control as FormGroup).controls.currencyRate.value;
      a_c = (control as FormGroup).controls.addCurrencyPrice.value;
      total = (control as FormGroup).controls.sumCurrencyPrice.value;
      var u_e = (control as FormGroup).controls.unitPriceWithExtraCost.value;

      if (this.isImportPurchase) {

        if (inputId == 'productionCost') {
          this.totalProductionCost = Number(this.totalProductionCost) + Number(u * q) + Number(a_c);
        }
        else {
          this.totalProductionCost = Number(this.totalProductionCost) + Number(total * c_R);
        }


      } else {

        this.totalItemPrice = Number(this.totalItemPrice) + Number(u_e * q);
        this.totalProductionCost = Number(this.totalProductionCost) + Number(u * q) + Number(a_c);
      }
      //--------------------------------------------------------------------------------

    })

    //if (!this.isImportPurchase && inputId == "productionCost")

    if (this.isVate) {
      this.vatDutiesTax = (Number(this.vatPercentage) * Number(this.totalProductionCost)) / 100

    }
    if (!this.isImportPurchase) {

      this.totalItemPrice = this.totalItemPrice + this.vatDutiesTax;
    } else {
      this.totalItemPrice = this.totalProductionCost + this.vatDutiesTax;
    }

  }

  OnRowsChange(inputId: string = '', form: FormGroup) {
    
    if (this.calculationType == 101) {
     
     
      var q = form.controls.quantity.value;
      var p = form.controls.productionCost.value;
      var u = form.controls.unitPrice.value;
      var c_p = form.controls.currencyPrice.value;
      var c_R = form.controls.currencyRate.value;
      c_R = c_R == 0 ? c_R = 1 : c_R
      var a_c = form.controls.addCurrencyPrice.value;
      var total = form.controls.sumCurrencyPrice.value;

      

      if (inputId == 'quantity' && u != undefined && u != "") {
        let unit = Number(u * q) + Number(a_c);
        form.controls.productionCost.setValue(parseFloat(unit.toFixed(4)));

      }

      else if (inputId == 'unitPrice' && q != undefined && q != "") {
        let unit = Number(u * q) + Number(a_c);
        form.controls.productionCost.setValue(unit);
        let c_p = Number(u / c_R);
        /*form.controls.currencyPrice.setValue(c_p);*/
      }

      else if (inputId == 'productionCost' && q != undefined && q != "") {
        let c = Number(p / q);
        form.controls.unitPrice.setValue(c);
        let c_p = Number(c / c_R);
        //form.controls.currencyPrice.setValue(c_p);
        //form.controls.sumCurrencyPrice.setValue(c_p * q);

      }
      else if (inputId == 'currencyPrice' && q != undefined && q != "" && c_R != undefined && c_R != "") {

        let q_d = total / q;
        let unit =  Math.round(c_R * Number(q_d));
        //form.controls.unitPrice.setValue(parseFloat(unit.toFixed(4)));
       /* form.controls.productionCost.setValue(total * c_R);*/
        form.controls.sumCurrencyPrice.setValue(c_p * q);

      }
      else if (inputId == 'currencyRate' && q != undefined && q != "" && c_p != undefined && c_p != "") {

        let q_d = total / q;
        let unit = Math.round(c_R * Number(q_d));

        form.controls.unitPrice.setValue(parseFloat(unit.toFixed(4)));
        form.controls.productionCost.setValue(total * c_R);

      }
      
        this.totalItemPrice = (this.totalItemPrice - p) + form.controls.productionCost.value;
        this.totalProductionCost = (this.totalItemPrice - p) + form.controls.productionCost.value;
      
      
      
    }
    else {
      this.onComputing(inputId)
    }
    
    
  }

  onSumCurrencyPrice(item: any) {

    var q = item.controls.quantity.value;
    let q_d = Number(item.controls.sumCurrencyPrice.value / q);
    item.controls.currencyPrice.setValue(parseFloat(q_d.toFixed(6)));


  }

  async updateQuantity(item: any) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      DocumentItem: item,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(UpdateQuantityDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.onComputing('unitPrice');
      }
    })

  }

  getBomValue(item: any) {
    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      documentItemsId: item.controls.id.value,
      allowViewPrice: true,

      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(DocumentItemsBomDialog, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
        this.onComputing('unitPrice');
      }
    })
  }

  splitQuantity(item: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      DocumentItem: item,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(SplitCommodityQuantityDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {

      this.getData.emit(item);

    })
  }

  onDelete(form: any) {

    var i: number = 0;
    var j: number = 0;

    this.form.controls.forEach((control: any) => {

      if ((control as FormGroup).controls.documentHeadId.value == form.controls.documentHeadId.value) {
        j = i;

      }

      i = i + 1;
    })
    if (i > 1) {
      this.form.removeAt(j);

    }

    this.child.tableRows = this.form.controls;
    this.OnRowsChange('quantity', form)

  }

  async navigateToHistory(ReceiptItem: any) {


    var url = `inventory/commodityReceiptReports?commodityId=${ReceiptItem.controls.commodityId.value}&warehouseId=${ReceiptItem.controls.warehouseId.value}`
    this.router.navigateByUrl(url)

  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {

  }

  initialize(params?: any)
    :
    any {
  }


  update(param?: any)
    :
    any {
  }


  onInput(event: Event, inputId: string, rowFg: any) {
    this.processInputChange(event, inputId, rowFg);
  }

  onPaste(event: ClipboardEvent, inputId: string, rowFg: any) {
    event.preventDefault();

    const clipboardData = event.clipboardData;
    let pastedData = clipboardData?.getData('text') || '';


    const filteredValue = pastedData.replace(/[^0-9.]/g, '');

    const parts = filteredValue.split('.');
    let value;
    if (parts.length > 2) {
      value = parts[0] + '.' + parts[1]; // فقط اولین نقطه را نگه می‌داریم
    } else {
      value = filteredValue;
    }

    rowFg.get(inputId)?.setValue(value);
    this.checkForChanges(value, inputId, rowFg);
  }

  processInputChange(event: Event, inputId: string, rowFg: any) {
    const inputElement = event.target as HTMLInputElement;
    let value = inputElement.value;

    const filteredValue = value.replace(/[^0-9.]/g, '');

    const parts = filteredValue.split('.');
    if (parts.length > 2) {
      value = parts[0] + '.' + parts[1];
    } else {
      value = filteredValue;
    }

    rowFg.get(inputId)?.setValue(value);

    this.checkForChanges(value, inputId, rowFg);
  }

  checkForChanges(newValue: string, inputId: string, rowFg: any) {
    // ذخیره یا بازیابی مقدار قبلی برای هر ردیف در کنترل rowFg
    const previousValue = rowFg.get(`${inputId}_previousValue`)?.value || '';


    if (newValue !== previousValue && /^-?\d*\.?\d*$/.test(newValue)) {
      const changeIds = ['quantity', 'currencyPrice', 'productionCost', 'unitPrice'];
      if (changeIds.includes(inputId)) {
        this.OnRowsChange(inputId, rowFg);
      } else if (inputId === 'sumCurrencyPrice') {
        this.onSumCurrencyPrice(rowFg);
      }

      // به‌روزرسانی مقدار قبلی در rowFg
      rowFg.get(`${inputId}_previousValue`)?.setValue(newValue) || rowFg.addControl(`${inputId}_previousValue`, new FormControl(newValue));
    }
  }

}
