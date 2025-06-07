import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../entities/receipt";
import { Router } from "@angular/router";
import { FormAction } from "../../../../core/models/form-action";
import { FormActionTypes } from "../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import { TableConfigurations } from "../../../../core/components/custom/table/models/table-configurations";
import { ReceiptAllStatusModel } from '../../entities/receipt-all-status';
import { FormControl, FormGroup } from '@angular/forms';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../core/components/custom/table/models/table-column";
import { GetComprehensiveListQuery } from '../../repositories/receipt/queries/receipt/get-receipts-comprehensive-list-query';
import { ConfirmDialogComponent } from '../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { ArchiveReceiptCommand } from '../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { MatDialog } from '@angular/material/dialog';
import { Warehouse } from '../../entities/warehouse';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';



@Component({
  selector: 'app-receipt-list',
  templateUrl: './receipt-list.component.html',
  styleUrls: ['./receipt-list.component.scss']
})
export class ReceiptListComponent extends BaseComponent {

  @ViewChild('buttonRedo', { read: TemplateRef }) buttonRedo!: TemplateRef<any>;

  @ViewChild('buttonVoucherNo', { read: TemplateRef }) buttonVoucherNo!: TemplateRef<any>;

  Receipts: Receipt[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];

  tableConfigurations!: TableConfigurations;

  SearchForm = new FormGroup({

    requestNo: new FormControl(),
    documentNo: new FormControl(),
    voucherNo: new FormControl(),
    warehouseId: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,

  ]

  constructor(

    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super();
  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve() {

    await this.getReceiptAllStatus();


    let colVoucherNo = new TableColumn(
      'voucherNo',
      'سند مکانیزه',
      TableColumnDataType.Template,
      '5%',
      true,
      new TableColumnFilter('voucherNo', TableColumnFilterTypes.Number)

    );
    let colRedo = new TableColumn(
      'colRedo',
      'عملیات',
      TableColumnDataType.Template,
      '10%',
      false

    );
    colRedo.template = this.buttonRedo;

    colVoucherNo.template = this.buttonVoucherNo;
    let columns: TableColumn[] = [
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
        'شماره رسید',
        TableColumnDataType.Text,
        '5%',
        true,

        new TableColumnFilter('documentNo', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '5%',
        true,


      ),
      new TableColumn(
        'creditReferenceTitle',
        'تامین کننده',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'documentDate',
        'تاریخ سند',
        TableColumnDataType.Date,
        '5%',
        true,

      ),
      colVoucherNo,

      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
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
        'measureTitle',
        'واحد کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'username',
        'ثبت کننده',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('username', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'documentStauseBaseValueTitle',
        'وضعیت',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('documentStauseBaseValueTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'isImportPurchase',
        'وارداتی',
        TableColumnDataType.CheckBox,
        '10%',
        true,
        new TableColumnFilter('isImportPurchase', TableColumnFilterTypes.CheckBox)
      ),

      colRedo
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();

  }

  initialize() {
  }

  async get() {
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery(
          {
            propertyName: filter.columnName,
            values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
            comparison: filter.searchCondition,
            nextOperand: "and"
          }
        ))
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
    if (this.SearchForm.controls.voucherNo.value != undefined && this.SearchForm.controls.voucherNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "voucherNo",
        values: [this.SearchForm.controls.voucherNo.value],
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
    if (this.SearchForm.controls.codeVoucherGroupId.value != undefined && this.SearchForm.controls.codeVoucherGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [this.SearchForm.controls.codeVoucherGroupId.value],
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


    let request = new GetComprehensiveListQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  async getReceiptAllStatus() {

    await this.ApiCallService.getReceiptAllInventoryStatus().then(res => {

      this.ReceiptAllStatus = res;
    });

  }
  codeVoucherGroupSelect(item: any) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async navigateToReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
  }
  async navigateToVoucher(Receipt: Receipt) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${Receipt.voucherHeadId}`)
  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره سند</th>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ سند</th>
                       <th>نوع سند</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>تامین کننده</th>
                       <th>کاربر</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.username}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدها')
    }
  }
  async archive(receipt: Receipt) {


    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: this.Message(receipt.id),

        actions: {
          confirm: { title: 'تایید و بایگانی', show: true }, cancel: { title: 'عدم بایگانی', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(receipt.id));
        this.Receipts = this.Receipts.filter(a => a.id != receipt.id);
        //-------------------------------------------------------------
        let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
        UpdateAvgPrice.documentId = receipt?.documentId;
        this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
      }
    });

  }
  async Edit(Receipt: Receipt,type :number) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?id=${Receipt.id}&EditType=${type}`)
  }
  Message(id: number) {
    let printtable = `<h4>کالا هایی که در شماره سند مربوطه به بایگانی می روند</h4>
                      <table class='mas-table' ><thead>
                     <tr>
                        <th>ردیف</th>
                        <th>تاریخ</th>
                        <th>شماره سند</th>
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
  async update() {

  }

  async add() {

  }



  close(): any {
  }

  delete(): any {
  }


}
