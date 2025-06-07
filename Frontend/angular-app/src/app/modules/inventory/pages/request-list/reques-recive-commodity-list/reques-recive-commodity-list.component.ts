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
} from "../../../../../core/components/custom//table/models/table-configurations";
import { FormControl, FormGroup } from '@angular/forms';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { Warehouse } from '../../../entities/warehouse';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { GetReceiptsdocumentStauseBaseValueQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-documentStauseBaseValue-query';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-reques-recive-commodity-list',
  templateUrl: './reques-recive-commodity-list.component.html',
  styleUrls: ['./reques-recive-commodity-list.component.scss']
})
export class RequesReciveCommodityListComponent extends BaseComponent {

  @ViewChild('buttonMore', { read: TemplateRef }) buttonMore!: TemplateRef<any>;
  @ViewChild('txtcommodityCode', { read: TemplateRef }) txtcommodityCode!: TemplateRef<any>;
  @ViewChild('buttonDocumentNo', { read: TemplateRef }) buttonDocumentNo!: TemplateRef<any>;

  @ViewChild('txtcommodityTitle', { read: TemplateRef }) txtcommodityTitle!: TemplateRef<any>;
  @ViewChild('txtCodeVoucherGroupTitle', { read: TemplateRef }) txtCodeVoucherGroupTitle!: TemplateRef<any>;
  @ViewChild('txtRequesterReferenceTitle', { read: TemplateRef }) txtRequesterReferenceTitle!: TemplateRef<any>;

  Receipts: Receipt[] = [];
  tableConfigurations!: TableConfigurations;

  SearchForm = new FormGroup({
    documentNo: new FormControl(),
    warehouseId: new FormControl(),
    codeVoucherGroupId: new FormControl(this.getQueryParam('codeVoucherGroupId')),
    requesterReferenceId: new FormControl(),

    fromDate: new FormControl(),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
    FormActionTypes.add
  ]

  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {

    let colDocumentNo = new TableColumn(
      'documentNo',
      'شماره',
      TableColumnDataType.Template,
      '8%',
      true

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
    let colRequesterReferenceTitle = new TableColumn(
      'requesterReferenceTitle',
      'درخواست دهنده',
      TableColumnDataType.Template,
      '11%',
      true,
      new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text)
    );
    let colcodeVoucherGroupTitle = new TableColumn(
      'codeVoucherGroupTitle',
      'نوع سند',
      TableColumnDataType.Template,
      '20%',
      true,
      new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
    );

    colMore.template = this.buttonMore;
    colDocumentNo.template = this.buttonDocumentNo;
    colcommodityCode.template = this.txtcommodityCode;
    colCommodityTitle.template = this.txtcommodityTitle;
    colcodeVoucherGroupTitle.template = this.txtCodeVoucherGroupTitle;
    colRequesterReferenceTitle.template = this.txtRequesterReferenceTitle;

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
        'requestNo',
        'شماره درخواست خرید',
        TableColumnDataType.Text,
        '8%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'documentDate',
        'تاریخ سند',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)
      ),
      colRequesterReferenceTitle,
      colCommodityTitle,
      colcommodityCode,


      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Money)
      )
      ,
      new TableColumn(
        'remainQuantity',
        'تعداد مانده',
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('remainQuantity', TableColumnFilterTypes.Money)
      )
      ,
      colcodeVoucherGroupTitle,

      colMore

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))

    //--------------------------------------------------
    await this.get();

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

    if (this.SearchForm.controls.requesterReferenceId.value != undefined && this.SearchForm.controls.requesterReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requesterReferenceId",
        values: [this.SearchForm.controls.requesterReferenceId.value],
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

    let request = new GetReceiptsdocumentStauseBaseValueQuery(
      Number(this.Service.CodeRequestRecive),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }

  requesterReferenceSelect(item: any) {


    this.SearchForm.controls.requesterReferenceId.setValue(item?.id);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async add() {
    if (this.getQueryParam('codeVoucherGroupId') != undefined) {
      await this.router.navigateByUrl(`inventory/request-operations/requestCommodity?codeVoucherGroupId=${this.SearchForm.controls.codeVoucherGroupId.value}`)
    }
    else {
      await this.router.navigateByUrl(`inventory/request-operations/requestCommodity`)
    }

  }


  async Edit(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/requestCommodity?id=${Receipt.id}`)
  }
  async printRequest(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/RequestPrint?id=${Receipt.id}&displayPage=receive`)
  }
  async navigateToRecive(Receipt: Receipt) {
    var codeVoucherGroupCode= this.ApiCallService.AllReceiptStatus.find(a => a.id == Receipt?.codeVoucherGroupId)?.code;
    var NewCode = codeVoucherGroupCode?.substring(0, 2) + (Number(this.Service.CodeLeaveReceipt)).toString();
    var codeVoucherGroupId = this.ApiCallService.AllReceiptStatus.find(a => a.code == NewCode)?.id;

    await this.router.navigateByUrl(`inventory/leavingConsumableWarehouse?documentNo=${Receipt.documentNo}&codeVoucherGroupId=${codeVoucherGroupId}`)

  }
  async navigateToArchive(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
  }
  
  async navigateToMaterail(Receipt: Receipt,code :number) {
    await this.router.navigateByUrl(`inventory/receipt-operations/leavingMaterialWarehouse?RequestId=${Receipt.id}&GroupCode=${code}`)
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
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره سند</th>
                       <th>تاریخ سند</th>
                       <th>انبار</th>
                       <th>درخواست دهنده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>نوع سند</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>

                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.warehouseTitle}</td>
                           <td>${data.requesterReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای دریافت کالا')
    }
  }
  ngOnInit(): void {

  }

  initialize() {
  }

  async update() {

  }
  close(): any {
  }

  delete(): any {
  }


}
