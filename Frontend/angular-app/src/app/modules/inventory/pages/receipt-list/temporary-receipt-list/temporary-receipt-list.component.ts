import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { ActivatedRoute, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";

import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';
import { UpdateStatusDirectReceiptCommand } from '../../../repositories/receipt/commands/reciept/update-status-direct-receipt-command';
import { BaseValueModel } from '../../../entities/base-value';
import { GetReceiptALLBaseValueQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-all-base-value-query';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { AccountReference } from '../../../../accounting/entities/account-reference';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { PlacementWarehouseDirectReceiptCommand } from '../../../repositories/receipt/commands/reciept/update-placement-in-warehouse-command';

import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { Warehouse } from '../../../entities/warehouse';
import { GetRequesterReferenceWarhouse } from '../../../repositories/personal/get-requester-reference-query';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { GetAllTempraryReceiptsQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-all-receipts-query';


@Component({
  selector: 'app-temporary-receipt-list',
  templateUrl: './temporary-receipt-list.component.html',
  styleUrls: ['./temporary-receipt-list.component.scss'],
  providers: [DatePipe]
})
export class TemporaryReceiptListComponent extends BaseComponent {
  @ViewChild('buttonMore', { read: TemplateRef }) buttonMore!: TemplateRef<any>;
  @ViewChild('buttonEstate', { read: TemplateRef }) buttonEstate!: TemplateRef<any>;
  @ViewChild('buttonConvert', { read: TemplateRef }) buttonConvert!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('txtcommodityCode', { read: TemplateRef }) txtcommodityCode!: TemplateRef<any>;
  @ViewChild('txtcommodityTitle', { read: TemplateRef }) txtcommodityTitle!: TemplateRef<any>;
  @ViewChild('txtReferenceTitle', { read: TemplateRef }) txtReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtRequesterReferenceTitle', { read: TemplateRef }) txtRequesterReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtDesctiption', { read: TemplateRef }) txtDesctiption!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  ReceiptBaseValue: BaseValueModel[] = [];
  accountReferences: AccountReference[] = [];

  SearchForm = new FormGroup({
    requestNo: new FormControl(),
    invoiceNo: new FormControl(),
    warehouseId: new FormControl(),
    codeVoucherGroupId: new FormControl(this.getQueryParam('codeVocherGroup')),
    accountReferencesId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  tableConfigurations!: TableConfigurations;

  listActions: FormAction[] = [

    FormActionTypes.refresh

  ]

  constructor(

    private router: Router,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
  }

  async ngOnInit() {
    await this.ReferenceFilter('');
    await this.getReceiptBaseValue();
  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {

    let colMore = new TableColumn(
      'colMore',
      'بیشتر',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colConvert = new TableColumn(
      'colConvert',
      'وضعیت',
      TableColumnDataType.Template,
      '6%',
      false

    );
    let colPrint = new TableColumn(
      'buttonPrint',
      'چاپ',
      TableColumnDataType.Template,
      '6%',
      false

    );
    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '6%',
      false

    );
    let colButtonEstate = new TableColumn(
      'buttonEstate',
      'کد اموال',
      TableColumnDataType.Template,
      '6%',
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

    let colRequesterReferenceTitle = new TableColumn(
      'requesterReferenceTitle',
      'درخواست دهنده',
      TableColumnDataType.Template,
      '11%',
      true,
      new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text)
    );
    let colreferenceTitle = new TableColumn(
      'creditReferenceTitle',
      'تامین کننده',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
    );
    let coldocumentDescription = new TableColumn(
      'documentDescription',
      'شرح',
      TableColumnDataType.Template,
      '30%',
      true,
      new TableColumnFilter('documentDescription', TableColumnFilterTypes.Text)
    );

    colMore.template = this.buttonMore;
    colConvert.template = this.buttonConvert;
    colTagArray.template = this.buttonTagArray;
    colButtonEstate.template = this.buttonEstate
    colcommodityCode.template = this.txtcommodityCode;
    colreferenceTitle.template = this.txtReferenceTitle
    colCommodityTitle.template = this.txtcommodityTitle;
    colRequesterReferenceTitle.template = this.txtRequesterReferenceTitle;
    coldocumentDescription.template = this.txtDesctiption;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'selected',
        'انتخاب',
        TableColumnDataType.Select,
        '5%',
        true,
        new TableColumnFilter('selected', TableColumnFilterTypes.Select)

      ),
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
        '8%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'documentNo',
        'شماره رسید',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),
      //new TableColumn(
      //  'invoiceNo',
      //  'شماره صورتحساب',
      //  TableColumnDataType.Text,
      //  '8%',
      //  true,
      //  new TableColumnFilter('invoiceNo', TableColumnFilterTypes.Text)

      //),

      new TableColumn(
        'documentDate',
        'تاریخ رسید',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),
      colCommodityTitle,
      colcommodityCode,
      colRequesterReferenceTitle,
      colreferenceTitle,
      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),
      coldocumentDescription,
      /*colTagArray,*/
      colButtonEstate,
      colConvert,
      colMore,

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();


  }


  async ReferenceFilter(searchTerm: string) {
    var filter: any = undefined;
    if (searchTerm != '') {
      filter = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'or'
        }),

      ]
    }

    await this._mediator.send(new GetRequesterReferenceWarhouse(
      this.Service.CodeTemporaryReceipt,
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      0, 25, filter)).then(res => {

        this.accountReferences = res.data
      })

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
    if (this.SearchForm.controls.accountReferencesId.value != undefined && this.SearchForm.controls.accountReferencesId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requesterReferenceId",
        values: [this.SearchForm.controls.accountReferencesId.value],
        comparison: 'equal',
        nextOperand: 'and'
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

    if (this.SearchForm.controls.invoiceNo.value != undefined && this.SearchForm.controls.invoiceNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceNo",
        values: [this.SearchForm.controls.invoiceNo.value],
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


    let request = new GetAllTempraryReceiptsQuery(Number(this.Service.CodeTemporaryReceipt),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;

    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
    await this.ReferenceFilter('');

  }

  //------------بدست آوردن حالت های مختلف تایید یک رسید--------------------
  async getReceiptBaseValue() {
    let request = new GetReceiptALLBaseValueQuery()
    let response = await this._mediator.send(request);
    this.ReceiptBaseValue = response.data;
  }
  async update() {

  }
  async update_Status_all(statusId: number) {
    var responce = this.Receipts.filter(a => a.selected)
    if (responce.length > 0) {
      responce.forEach(async res => {
        if (res.selected && res.isAllowedInputOrOutputCommodity!=false) {
          await this.update_Status(res, statusId)
        }

      })
    }
    else {
      this._notificationService.showWarningMessage('رسیدهای موقت جهت تغییر وضعیت به رسید مستقیم را انتخاب نمایید');
    }

  }

  //----------------------تبدیل به رسید مستقیم----------------------------
  async update_Status(item: any, statusId: number) {
    var request_ = new UpdateStatusDirectReceiptCommand();
    request_.id = item.id;
    request_.statusId = statusId;

    //-----------جایگذاری کالا در اولین محلی که در انبار پیدا کرد
    var newRequest = new PlacementWarehouseDirectReceiptCommand();
    newRequest.id = item.id;

    await this._mediator.send(<PlacementWarehouseDirectReceiptCommand>newRequest).then(res => {


       this._mediator.send(<UpdateStatusDirectReceiptCommand>request_).then(a => {
        this.Receipts = this.Receipts.filter(a => a.id != item.id);
      });


    });


  }

  ReferenceSelect(item: any) {

    this.SearchForm.controls.accountReferencesId.setValue(item);
  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async add() {



  }
  //------------------------------------
  async Edit(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?id=${Receipt.id}`)
  }

  //-----------------Print--------------------------------------------------------
  async ConfirmPageTemporaryReceiptALL() {
    this.printList();

    if (this.Receipts.filter(a => a.selected).length == 0) {


      this.Service.showHttpFailMessage('لیست رسیدهای چاپی را انتخاب نمایید')
    }
    else if (this.SearchForm.controls.accountReferencesId.value == undefined) {


      this.Service.showHttpFailMessage('درخواست دهنده مورد نظر را انتخاب کنید')
    }
    else {

      await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceiptPrint?accountReferencesId=${this.SearchForm.controls.accountReferencesId.value}`);
    }
  }
  printList() {
    var responce = this.Receipts.filter(a => a.selected)
    if (responce.length > 0) {

      this.Service.ListId = [];
      responce.forEach(async res => {
        if (res.selected) {
          this.Service.ListId.push(res.id.toString());
        }

      })

    }
  }
  async ConfirmPageTemporaryReceipt
    (Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceiptPrint?id=${Receipt.id}`)
  }
  //------------------------------------------------------------------------------
  async archive(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: `آیا مطمئن به بایگانی این شماره رسید ` + Receipt.documentNo + ` می باشید؟`,
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
  async NotConfirm(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید مرجوعی و خروج',
        message: `این  شماره رسید  ` + Receipt.documentNo + ` بدون صدور فاکتور فروش مرجوع می گردد و مورد تایید جهت مرجوعی می باشد`,
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
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره رسید</th>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ رسید</th>
                       <th>درخواست دهنده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>تامین کننده</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.requesterReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.creditReferenceTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای موقت')
    }
  }

  close(): any {
  }

  delete(): any {

  }



}
