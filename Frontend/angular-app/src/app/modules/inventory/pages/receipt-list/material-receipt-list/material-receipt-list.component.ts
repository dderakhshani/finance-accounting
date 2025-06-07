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
import { LoaderService } from '../../../../../core/services/loader.service';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { GetReceiptsViewIdQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-viewId-query';
import { Warehouse } from '../../../entities/warehouse';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';


@Component({
  selector: 'app-material-receipt-list',
  templateUrl: './material-receipt-list.component.html',
  styleUrls: ['./material-receipt-list.component.scss']
})
export class MaterilaReceiptListComponent extends BaseComponent {

  @ViewChild('buttonEdit', { read: TemplateRef }) buttonEdit!: TemplateRef<any>;
  @ViewChild('buttonRedo', { read: TemplateRef }) buttonRedo!: TemplateRef<any>;


  Receipts: Receipt[] = [];

  tableConfigurations!: TableConfigurations;

  listActions: FormAction[] = [

    FormActionTypes.refresh,
    FormActionTypes.add,

  ]

  SearchForm = new FormGroup({
    debitReferenceId: new FormControl(),
    creditReferenceId: new FormControl(),
    codeVoucherGroupId: new FormControl(),

    documentNo: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });


  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
  }
  ngOnInit(params?: any): void {

  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {

    let colEdit = new TableColumn(
      'colEdit',
      'ویرایش',
      TableColumnDataType.Template,
      '10%',
      false

    );
    let colRedo = new TableColumn(
      'colRedo',
      'بایگانی',
      TableColumnDataType.Template,
      '10%',
      false

    );

    colEdit.template = this.buttonEdit;
    colRedo.template = this.buttonRedo;
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
      new TableColumn(
        'creditAccountHeadTitle',
        'انبار تحویل دهنده',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('creditAccountHeadTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'debitAccountHeadTitle',
        'انبار تحویل گیرنده',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('debitAccountHeadTitle', TableColumnFilterTypes.Text)
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
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      )
      ,

      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع رسید',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),
      colEdit,
      colRedo

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

    if (this.SearchForm.controls.debitReferenceId.value != undefined && this.SearchForm.controls.debitReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountHeadId",
        values: [this.SearchForm.controls.debitReferenceId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.creditReferenceId.value != undefined && this.SearchForm.controls.creditReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountHeadId",
        values: [this.SearchForm.controls.creditReferenceId.value],
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
    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
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


    let request = new GetReceiptsViewIdQuery(
      Number(this.Service.ViewIdRemoveAddWarehouse),
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  FromWarehouseIdSelect(item: Warehouse) {


    this.SearchForm.controls.creditReferenceId.setValue(item?.accountHeadId);

  }
  ToWarehouseIdSelect(item: Warehouse) {


    this.SearchForm.controls.debitReferenceId.setValue(item?.accountHeadId);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async add() {
    await this.router.navigateByUrl(`inventory/receipt-operations/leavingMaterialWarehouse`)
  }


  async Edit(Receipt: Receipt) {
    if (Receipt.documentStauseBaseValue == this.Service.CodeRegistrationAccountingLeave || Receipt.documentStauseBaseValue == this.Service.CodeRegistrationAccountingReceipt) {

      await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)

    }
    else {
      await this.router.navigateByUrl(`inventory/receipt-operations/updateMaterilaReceip?id=${Receipt.id}`)

    }
  }
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
        //-------------------------------------------------------------
        let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
        UpdateAvgPrice.documentId = Receipt?.documentId;
        this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
      }
    });

  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>

                       <th>شماره رسید</th>
                       <th>تاریخ رسید</th>
                       <th>انبار</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>نوع رسید</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>

                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.warehouseTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای انبار مواد اولیه')
    }
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
