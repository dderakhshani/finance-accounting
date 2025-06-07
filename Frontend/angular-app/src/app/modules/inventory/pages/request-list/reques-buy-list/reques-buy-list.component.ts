import { Router } from "@angular/router";
import { MatDialog } from '@angular/material/dialog';
import { Receipt } from '../../../entities/receipt';
import { Warehouse } from '../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Commodity } from '../../../../commodity/entities/commodity';
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableConfigurations } from '../../../../../core/components/custom/table/models/table-configurations';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { GetAllReceiptItemsCommodityQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-items-commodity-query';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';



@Component({
  selector: 'app-reques-buy-list',
  templateUrl: './reques-buy-list.component.html',
  styleUrls: ['./reques-buy-list.component.scss']
})

export class requesBuyListComponent extends BaseComponent {

  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;
  @ViewChild('buttonRedo', { read: TemplateRef }) buttonRedo!: TemplateRef<any>;
  @ViewChild('buttonMore', { read: TemplateRef }) buttonMore!: TemplateRef<any>;
  @ViewChild('buttonPrint', { read: TemplateRef }) buttonPrint!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('buttonDocumentNo', { read: TemplateRef }) buttonDocumentNo!: TemplateRef<any>;
  @ViewChild('txtDesctiption', { read: TemplateRef }) txtDesctiption!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  totalQuantity: number = 0;
  totalRemainQuantity: number = 0;

  tableConfigurations!: TableConfigurations;


  SearchForm = new FormGroup({
    documentNo: new FormControl(),
    warehouseId: new FormControl(),
    commodityCode: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    accountReferencesId: new FormControl(),

    fromDate: new FormControl(),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super();
  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {
    await this.resolve();
  }
  async resolve() {
    await this.ApiCallService.getReceiptAllStatus('');

    let colMore = new TableColumn(
      'colMore',
      'بیشتر',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '8%',
      false

    )

    let colDocumentNo = new TableColumn(
      'documentNo',
      'شماره',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

    );
    let coldocumentDescription = new TableColumn(
      'descriptionItem',
      'شرح',
      TableColumnDataType.Text,
      '30%',
      true,
      new TableColumnFilter('descriptionItem', TableColumnFilterTypes.Text)
    );
    colMore.template = this.buttonMore;
    colTagArray.template = this.buttonTagArray;
    colDocumentNo.template = this.buttonDocumentNo;
    coldocumentDescription.template = this.txtDesctiption;
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
      colDocumentNo,
      new TableColumn(
        'documentDate',
        'تاریخ سند',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        'requesterReferenceTitle',
        'درخواست دهنده',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text)
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
        'measureTitle',
        'واحد کالا',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
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
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Money)
      ),


      new TableColumn(
        'totalQuantity',
        'تعداد ورودی',
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('totalQuantity', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'remainQuantity',
        'تعداد مانده',
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('remainQuantity', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'isImportPurchase',
        'وارداتی',
        TableColumnDataType.CheckBox,
        '5%',
        true,
        new TableColumnFilter('isImportPurchase', TableColumnFilterTypes.CheckBox)
      ),
      coldocumentDescription,
      /*colTagArray,*/
      colMore,



    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();


  }

  async initialize() {

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
    if (this.SearchForm.controls.commodityCode.value != undefined && this.SearchForm.controls.commodityCode.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityCode",
        values: [this.SearchForm.controls.commodityCode.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.accountReferencesId.value != undefined && this.SearchForm.controls.accountReferencesId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "referenceId",
        values: [this.SearchForm.controls.accountReferencesId.value],
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
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }


    let request = new GetAllReceiptItemsCommodityQuery(
      Number(this.Service.CodeRequestBuy),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)

    let response = await this._mediator.send(request);

    this.Receipts = response.data;
    this.totalQuantity = 0;
    this.totalRemainQuantity = 0;
    this.Receipts.forEach(a => {
      this.totalQuantity += Number(a.quantity),
        this.totalRemainQuantity += Number(a.remainQuantity),
        a.totalQuantity = Number(a.quantity) - Number(a.remainQuantity)
    });
    //this.Receipts.forEach(a => this.totalRemainQuantity += Number(a.remainQuantity));
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);


  }
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityCode.setValue(item?.code);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async navigateToEditReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/requestBuy?id=${Receipt.id}&displayPage=edit`)

  }
  async navigateToCopyReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/requestBuy?id=${Receipt.id}&displayPage=copy`)

  }
  async navigateToTemproryReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?documentNo=${Receipt.documentNo}&warehouseId=${Receipt.warehouseId}`)

  }
  async navigateToReturnReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/addRequestReturnCommodity?documentNo=${Receipt.documentNo}&warehouseId=${Receipt.warehouseId}`)
  }
  async navigateToArchive(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
  }
  async printRequest(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/RequestPrint?id=${Receipt.id}&displayPage=buy`)
  }
  async requesBuyMadeList(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/requesBuyMadeList?documentNo=${Receipt.documentNo}&warehouseId=${Receipt.warehouseId}&totalQuantity=${Receipt.quantity}&commodityCode=${Receipt.commodityCode}`)
  }

  //--------------------------------------------------------------
  async archive(Receipts: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: `آیا مطمئن به بایگانی  شماره سند ` + Receipts.documentNo + ` می باشید؟`,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(Receipts.id));
        this.Receipts = this.Receipts.filter(a => a.id != Receipts.id);
      }
    });

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره سند</th>
                       <th>تاریخ سند</th>
                       <th>درخواست دهنده</th>
                       <th>پیگیری کننده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>تعداد مانده</th>
                       <th>نوع سند</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.requesterReferenceTitle}</td>
                           <td>${data.followUpReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.remainQuantity}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'درخواست های خرید')
    }
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
