import { Component, TemplateRef, ViewChild, Inject, Input } from '@angular/core';

import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";

import { TableConfigurations } from '../../../../../core/components/custom/table/models/table-configurations';

import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';

import { ReceiptItem } from '../../../entities/receipt-item';
import { GetDocumentItemsBomByIdQuery } from '../../../repositories/receipt/queries/receipt/get-documentItemsbom-Id-query';

import { PageModes } from '../../../../../core/enums/page-modes';
import { GetALLDocumentItemsBomQuery } from '../../../repositories/receipt/queries/receipt/get-all-document-items-bom-query';
import { UpdateDocumentItemsBomQuantityCommand } from '../../../repositories/receipt/commands/receipt-items/update-document-Items-bom-quantity-command';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { AddDocumentItemsBomQuantityCommand } from '../../../repositories/receipt/commands/receipt-items/add-document-Items-bom-quantity-command';
import { DocumentItemsBom } from '../../../entities/commodity-boms';
import da from 'date-fns/esm/locale/da/index';

@Component({
  selector: 'app-document-Items-bom-dialog',
  templateUrl: './document-Items-bom-dialog.component.html',
  styleUrls: ['./document-Items-bom-dialog.component.scss']
})

export class DocumentItemsBomDialog extends BaseComponent {

  @ViewChild('txtQuantity', { read: TemplateRef }) txtQuantity!: TemplateRef<any>;


  pageModes = PageModes;
  documentItemsId: number = 0;
  ReceiptItems: DocumentItemsBom[] = [];
  Reports_filter: DocumentItemsBom[] = [];
  allowViewPrice: boolean = false;
  isReadOnly: boolean = false;
  IslargeSize: boolean = false
  totalItemUnitPrice: number = 0;
  totalQuantity: number = 0;
  sumAll: number = 0;

  tableConfigurations!: TableConfigurations;




  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<UpdateDocumentItemsBomQuantityCommand>,
    public Service: PagesCommonService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();


    this.documentItemsId = data.documentItemsId;
    this.allowViewPrice = data.allowViewPrice;
    this.isReadOnly = data.isReadOnly != undefined ? data.isReadOnly : false;
    this.pageMode = data.pageMode;
    this.request = new UpdateDocumentItemsBomQuantityCommand();

  }

  async ngOnInit() {

  }

  ngAfterViewInit() {

    this.resolve()
  }
  async resolve() {


    let colQuantity = new TableColumn(
      'quantity',
      'تعداد/مقدار مصرف شده',
      TableColumnDataType.Template,
      '25%',
      true,
      new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
    )
    colQuantity.template = this.txtQuantity;


    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        'measureTitle',
        'واحد کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Number)
      ),

      colQuantity,
      //colEdit

    ]
    if (this.allowViewPrice) {
      columns.push(new TableColumn(
        'unitPrice',
        'قیمت واحد',
        TableColumnDataType.Money,
        '25%',
        true,
        new TableColumnFilter('unitPrice', TableColumnFilterTypes.Money)
      ))
      columns.push(new TableColumn(
        'productionCost',
        'قیمت کل',
        TableColumnDataType.Money,
        '25%',
        true,
        new TableColumnFilter('productionCost', TableColumnFilterTypes.Money)
      ))
    }
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, false))

    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.options.showSumRow = true;
    //--------------------------------------------------
    await this.get();


  }

  async get() {
    await this._mediator.send(new GetALLDocumentItemsBomQuery(this.documentItemsId)).then(res => {
      this.ReceiptItems = res.data
      this.Reports_filter = res.data
      this.CalculateSum();


    });

  }
  oncalculate(item: DocumentItemsBom) {
    item.productionCost = Number(item.quantity) * Number(item.unitPrice);
    this.CalculateSum();
  }

  
  async update() {

    await this.ReceiptItems.forEach(async resc => {

      //Edit
      if (resc.id != undefined && resc.id > 0) {
        let request = new UpdateDocumentItemsBomQuantityCommand();

        request.id = resc.id;
        request.quantity = resc.quantity;

        await this._mediator.send(<UpdateDocumentItemsBomQuantityCommand>request);
      }
      //Add

      if (resc.id == 0 && resc.commodityId != undefined && resc.quantity>0) {
        let request = new AddDocumentItemsBomQuantityCommand();

        request.commodityId = resc.commodityId;
        request.quantity = resc.quantity;
        request.documentItemsId = resc.documentItemId

        await this._mediator.send(<AddDocumentItemsBomQuantityCommand>request);
      }

    }


    )
    this.dialogRef.close({

      pageMode: this.pageMode
    })
  }

  initialize() {

  }
  CalculateSum() {
   
    this.totalItemUnitPrice = 0;
    this.totalQuantity = 0;
    this.sumAll = 0;
   
    this.ReceiptItems.forEach(a => this.totalItemUnitPrice += Number(a.unitPrice));
    this.ReceiptItems.forEach(a => this.totalQuantity += Number(a.quantity));
    this.ReceiptItems.forEach(a => this.sumAll += Number(a.quantity) * Number(a.unitPrice));
  }
  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.ReceiptItems = data
    this.CalculateSum();
  }
  async add() {

    this.ReceiptItems.push({
      id: 0,
      commodityId: undefined,
      commodityTitle: "",
      commodityCode: "",
      mainMeasureId: undefined,
      measureTitle: "",
      unitPrice: undefined,
      productionCost: undefined,
      quantity: 0,
      documentItemId: this.documentItemsId,

    });
  }
  getCommodityById(item: any, rowFg: any) {

    rowFg.commodityId=item?.id;
    rowFg.measureTitle =item?.measureTitle;
    rowFg.commodityCode =item?.code;

  }
  async print() {

    let printContents = document.getElementById('report-table-bom-item')?.innerHTML;
    if (this.ReceiptItems.length > 0) {
      this.Service.onPrint(printContents, 'مواد اولیه مصرفی در فرمول ساخت')
    }
  }
  close(): any {
  }

  delete(i: number): any {

    this.ReceiptItems = this.ReceiptItems.filter((_, index) => index !== i);

  }


}
