import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Receipt } from "../../../entities/receipt";
import { FormControl, FormGroup } from '@angular/forms';
import { Warehouse } from '../../../entities/warehouse';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DocumentState } from '../../../entities/documentState';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Data, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableConfigurations } from "../../../../../core/components/custom/table/models/table-configurations";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { GetReceiptsCommoditesQuery } from '../../../repositories/commodity/get-receipt-commodites-query';
import { DocumentHeadExtraCostDialogComponent } from '../../component/documents-extra-cost-dialog/documents-extra-cost-dialog.component';
@Component({
  selector: 'app-Rials-receipt',
  templateUrl: './Rials-receipt.component.html',
  styleUrls: ['./Rials-receipt.component.scss']
})
export class RialsReceiptComponent extends BaseComponent {
  @ViewChild('buttonMore', { read: TemplateRef }) buttonMore!: TemplateRef<any>;
  @ViewChild('buttonStack', { read: TemplateRef }) buttonStack!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('txtcommodityCode', { read: TemplateRef }) txtcommodityCode!: TemplateRef<any>;
  @ViewChild('checkboxSelected', { read: TemplateRef }) checkboxSelected!: TemplateRef<any>;
  @ViewChild('txtcommodityTitle', { read: TemplateRef }) txtcommodityTitle!: TemplateRef<any>;
  @ViewChild('txtdebitReferenceTitle', { read: TemplateRef }) txtdebitReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtcreditReferenceTitle', { read: TemplateRef }) txtcreditReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtCodeVoucherGroupTitle', { read: TemplateRef }) txtCodeVoucherGroupTitle!: TemplateRef<any>;


  Receipts: Receipt[] = [];

  //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
  //در حالتی که از سیستم آرانی درخواست ها خوانده شود و در سیستم ایفا ثبت نشده باشد.
  public commodityId: number | undefined = undefined;

  tableConfigurations!: TableConfigurations;
  panelOpenState = true;

  isImportPurchase = this.getQueryParam('isImportPurchase')

  SearchForm = new FormGroup({
    invoceNo: new FormControl(),
    requestNo: new FormControl(),
    warehouseId: new FormControl(),
    documentNo: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
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
    private _snackBar: MatSnackBar,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnInit() {
    //await this.resolve()
    this.Service.ListId = [];

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve() {
    await this.ApiCallService.getReceiptAllStatus('');


    let colSelected = new TableColumn(
      'selected',
      'انتخاب',
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
    let colStack = new TableColumn(
      'quantity',
      'تعداد',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colMore = new TableColumn(
      'colMore',
      'بیشتر',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colCommodityTitle = new TableColumn(
      'commodityTitle',
      'نام کالا',
      TableColumnDataType.Template,
      '12%',
      true,
      new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
    );
    let colcommodityCode = new TableColumn(
      'commodityCode',
      'کد کالا',
      TableColumnDataType.Template,
      '12%',
      true,
      new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
    );
    let colCodeVoucherGroupTitle = new TableColumn(
      'codeVoucherGroupTitle',
      'نوع رسید',
      TableColumnDataType.Template,
      '11%',
      true,
      new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
    );
    let colcreditreferenceTitle = new TableColumn(
      'creditReferenceTitle',
      'حساب بستانکار',
      TableColumnDataType.Template,
      '20%',
      true,
      new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
    );
    let colredebitferenceTitle = new TableColumn(
      'debitReferenceTitle',
      'حساب بدهکار',
      TableColumnDataType.Template,
      '20%',
      true,
      new TableColumnFilter('debitReferenceTitle', TableColumnFilterTypes.Text)
    );

    colMore.template = this.buttonMore;

    colTagArray.template = this.buttonTagArray;
    colSelected.template = this.checkboxSelected;
    colcommodityCode.template = this.txtcommodityCode;
    colCommodityTitle.template = this.txtcommodityTitle;
    colredebitferenceTitle.template = this.txtdebitReferenceTitle;
    colcreditreferenceTitle.template = this.txtcreditReferenceTitle;
    colCodeVoucherGroupTitle.template = this.txtCodeVoucherGroupTitle;
    colStack.template = this.buttonStack;
    let columns: TableColumn[] = [

      colSelected,

      new TableColumn(
        'documentNo',
        'شماره رسید',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'documentDate',
        'تاریخ رسید',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),


      colCommodityTitle,

      colStack,

      colredebitferenceTitle,
      colcreditreferenceTitle,
      colCodeVoucherGroupTitle,

    ]
    if (this.isImportPurchase == 'false') {
      columns.push(new TableColumn(
        'invoiceNo',
        'صورتحساب',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('invoiceNo', TableColumnFilterTypes.Text)
      ))
    }
    if (this.isImportPurchase == 'true') {
      columns.push(new TableColumn(
        'invoiceNo',
        'شماره پرونده',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('invoiceNo', TableColumnFilterTypes.Text)
      ))
      columns.push(new TableColumn(
        'modifiedAt',
        'تارخ ثبت',
        TableColumnDataType.Date,
        '5%',
        true,
        new TableColumnFilter('modifiedAt', TableColumnFilterTypes.Date)
      ))
    }
    columns.push(colMore)
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //---------------------------------------------------------------------
    await this.get();


    await this.initialize()
  }

  async initialize() {
  }

  async get() {
    this.Service.ListId = [];

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

    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.invoceNo.value != undefined && this.SearchForm.controls.invoceNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceNo",
        values: [this.SearchForm.controls.invoceNo.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
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
    if (this.SearchForm.controls.creditAccountReferenceId.value != undefined && this.SearchForm.controls.creditAccountReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountReferenceId",
        values: [this.SearchForm.controls.creditAccountReferenceId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.creditAccountReferenceGroupId.value != undefined && this.SearchForm.controls.creditAccountReferenceGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountReferenceGroupId",
        values: [this.SearchForm.controls.creditAccountReferenceGroupId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.debitAccountReferenceId.value != undefined && this.SearchForm.controls.debitAccountReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountReferenceId",
        values: [this.SearchForm.controls.debitAccountReferenceId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.debitAccountReferenceGroupId.value != undefined && this.SearchForm.controls.debitAccountReferenceGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountReferenceGroupId",
        values: [this.SearchForm.controls.debitAccountReferenceGroupId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    searchQueries.push(new SearchQuery({
      propertyName: "documentStauseBaseValue",
      values: [DocumentState.Direct, DocumentState.Leave],
      comparison: "in",
      nextOperand: "and "
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "IsPlacementComplete",
      values: [true],
      comparison: "equal",
      nextOperand: "and"
    }))

    searchQueries.push(new SearchQuery({
      propertyName: "isDocumentIssuance",
      values: [true],
      comparison: "equal",
      nextOperand: "and"
    }))
    let orderByProperty = 'invoiceDate';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }


    let request = new GetReceiptsQuery(0,
     this.getQueryParam('isImportPurchase'),//-------------------تفکیک بین صورتحساب های داخلی و وارداتی-----------------
     new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;

    var Receipts = this.Receipts.reduce(function (r, a) {
      r[a.creditAccountReferenceId.toString()] = r[a.creditAccountReferenceId.toString()] || [];
      r[a.creditAccountReferenceId.toString()].push(a);
      return r;
    }, Object.create(null));

    var reciepts: any = Object.keys(Receipts).map((key: any) => Receipts[key]);

    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  async navigateToReceiptRials(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?id=${Receipt.id}&isImportPurchase=${this.isImportPurchase}`)
  }

  async navigateToReceiptAll() {

    if (this.Service.ListId.length > 0) {
      await this.router.navigateByUrl(`inventory/rialsReceiptDetails?isImportPurchase=${this.isImportPurchase}`)
    }

  }
  async navigateToReceipt(Receipt: Receipt) {

    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?id=${Receipt.id}`)
  }


  async navigateToHistory(Receipt: Receipt) {


    let searchQueries: SearchQuery[] = []

    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [Receipt.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(false, Receipt.warehouseId, "", 0, 50, searchQueries)).then(res => {

      this.router.navigateByUrl(`inventory/commodityReceiptReportsRial?commodityId=${res.data[0].id}&warehouseId=${Receipt.warehouseId}`)
    });


  }

  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  //------------------------------------------------------------------
  //انتخاب لیست جهت ریالی کردن رسید
  checkValue(SelectedReceipt: Receipt) {

    var valid: boolean = true;
    this.Receipts.filter(a => a.selected).forEach(request => {
      if (request) {
        if ((SelectedReceipt.creditAccountReferenceId != undefined && request.creditAccountReferenceId != undefined) && (SelectedReceipt.creditAccountReferenceId != request.creditAccountReferenceId)) {
          valid = false;
          this.Service.showHttpFailMessage('تنها رسید هایی قابل انتخاب است که برای یک تامین کننده باشند');
          SelectedReceipt.selected = false;
          return;
        }
      }

    })
    if (valid) {
      this.Service.ListId.push(SelectedReceipt.id.toString())
      SelectedReceipt.selected = true;
    }

  }
  RemoveId(SelectedReceipt: Receipt) {
    this.Service.RemoveId(SelectedReceipt);

  }
  clearListId() {
    this.Service.clearListId(this.Receipts);

  }

  //------------------------------------------------------------------------------
  async archive(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف/ بایگانی',
        message: `آیا مطمئن به بایگانی شماره رسید ` + Receipt.documentNo + ` می باشید؟`,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(Receipt.id));
        this.Receipts = this.Receipts.filter(a => a.id != Receipt.id);
      }
    });

  }

  async print(isRowSelect: any = undefined) {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره رسید</th>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ رسید</th>
                       <th>نام کالا</th>
                       <th>کد کالا</th>
                       <th>نوع رسید</th>
                       <th>حساب بدهکار</th>
                       <th>حساب بستانکار</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.filter(a => a.selected == true || isRowSelect == undefined).map(data => {
        printContents += `<tr>

                           <td>${data.documentNo}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.debitReferenceTitle}</td>


                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'فرم ریالی')
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
