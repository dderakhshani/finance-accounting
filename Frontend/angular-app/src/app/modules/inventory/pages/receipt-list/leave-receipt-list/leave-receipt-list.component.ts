import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { BaseValueModel } from '../../../entities/base-value';
import { MatDialog } from '@angular/material/dialog';
import { FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetAllReceiptItemsCommodityQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-items-commodity-query';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { Commodity } from '../../../../accounting/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';



@Component({
  selector: 'app-leave-receipt-list',
  templateUrl: './leave-receipt-list.component.html',
  styleUrls: ['./leave-receipt-list.component.scss'],
  providers: [DatePipe]
})
export class LeaveReceiptListComponent extends BaseComponent {

  @ViewChild('buttonRedo', { read: TemplateRef }) buttonRedo!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;


  totalQuantity: number = 0;
  Receipts: Receipt[] = [];
  ReceiptBaseValue: BaseValueModel[] = [];



  SearchForm = new FormGroup({
    requestNo: new FormControl(),
    documentNo: new FormControl(),
    warehouseId: new FormControl(),
    commodityCode: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    codeVoucherGroupId: new FormControl(),
  });



  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
  ) {
    super();
  }

  async ngOnInit() {

  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {
    await this.ApiCallService.getReceiptAllStatus('');

    let colbuttonVisibility = new TableColumn(
      'buttonVisibility',
      'مشاهده',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colRedo = new TableColumn(
      'colRedo',
      'بایگانی',
      TableColumnDataType.Template,
      '10%',
      false

    );
    colRedo.template = this.buttonRedo;
    colTagArray.template = this.buttonTagArray;
    colbuttonVisibility.template = this.buttonVisibility;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'serialNumber',
        'سریال',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('serialNumber', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'requestNo',
        'شماره درخواست ',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'documentNo',
        'شماره حواله',

        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),

      new TableColumn(
        'documentDate',
        'تاریخ حواله',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)

      ),
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
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'descriptionItem',
        'توضیحات',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('descriptionItem', TableColumnFilterTypes.Text)
      )
      ,
      colTagArray,
      colbuttonVisibility,
      colRedo
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();

  }

  initialize() {
  }

  onSearch() {
    if (this.SearchForm.valid) {
      this.get();
    }
  }
  async get() {

    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }

    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.codeVoucherGroupId.value != undefined && this.SearchForm.controls.codeVoucherGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [this.SearchForm.controls.codeVoucherGroupId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.commodityCode.value != undefined && this.SearchForm.controls.commodityCode.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityCode",
        values: [this.SearchForm.controls.commodityCode.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetAllReceiptItemsCommodityQuery(
      Number(this.Service.CodeInvoiceAmountLeave),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    this.totalQuantity = 0;

    this.Receipts.forEach(a => this.totalQuantity += Number(a.quantity));

    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityCode.setValue(item?.code);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره حواله</th>
                       <th>تاریخ حواله</th>
                       <th>انبار</th>
                       <th>عنوان کالا</th>
                       <th>کد کالا</th>
                       <th>تعداد</th>
                       <th>توضیحات</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.warehouseTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.quantity}</td>
                           <td>${data.descriptionItem}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'حوالههای خروج از انبار')
    }
  }
  async archive(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: this.Message(Receipt.id),

        actions: {
          confirm: { title: 'تایید و بایگانی', show: true }, cancel: { title: 'عدم بایگانی', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(Receipt.id));
        this.Receipts = this.Receipts.filter(a => a.id != Receipt.id);
      }
      //-------------------------------------------------------------
      let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
      UpdateAvgPrice.documentId = Receipt?.documentId;
      this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
    });

  }
  Message(id:number) {
    let printtable = `<h4>کالا هایی که در شماره حواله مربوطه به بایگانی می روند</h4>
                      <table class='mas-table' ><thead>
                     <tr>
                        <th>ردیف</th>
                        <th>تاریخ</th>
                        <th>شماره حواله</th>
                        <th>شماره درخواست</th>
                        <th>انبار</th>
                        <th>کد کالا</th>
                        <th>نام کالا</th>
                        <th>مقدار</th>
                     </tr>
                   </thead><tbody>`;
    let i = 1;


    this.Receipts.filter(a => a.id == id).map(data => {
      printtable += `<tr>
                           <th>${i}</th>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.requestNo}</td>
                           <td>${data.warehouseTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                        </tr>`
      i++;

    })
    printtable += `</tbody></table>`


    return printtable;
  }


  async navigateToReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
  }
  async update() {


  }

  async add() {

  }

  close(): any {
  }

  delete(): any {

  }



}
