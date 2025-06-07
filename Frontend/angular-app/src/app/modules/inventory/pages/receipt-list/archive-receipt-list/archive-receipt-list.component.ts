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


import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';

import { BaseValueModel } from '../../../entities/base-value';

import { MatDialog } from '@angular/material/dialog';

import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';

import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";




@Component({
  selector: 'app-archive-receipt-list',
  templateUrl: './archive-receipt-list.component.html',
  styleUrls: ['./archive-receipt-list.component.scss'],
  providers: [DatePipe]
})
export class ArchiveReceiptListComponent extends BaseComponent {

  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;

  Receipts: Receipt[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  ReceiptBaseValue: BaseValueModel[] = [];


  SearchForm = new FormGroup({

    requestNo: new FormControl(),
    invoiceNo: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    codeVoucherGroupId: new FormControl(),
  });



  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService
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
    colTagArray.template = this.buttonTagArray;
    colbuttonVisibility.template = this.buttonVisibility;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
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
        'شماره سند',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'invoiceNo',
        'شماره صورتحساب',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Text)

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
        'requesterReferenceTitle',
        'درخواست دهنده',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text)
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
      ),
      new TableColumn(
        'documentDescription',
        'توضیحات',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('documentDescription', TableColumnFilterTypes.Text)
      )
      ,
      colTagArray,
      colbuttonVisibility
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

    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value!="") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.invoiceNo.value != undefined && this.SearchForm.controls.invoiceNo.value!="") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceNo",
        values: [this.SearchForm.controls.invoiceNo.value],
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

    var param: number[] = [];
    param.push(Number(this.Service.CodeArchiveReceipt));

    let request = new GetReceiptsQuery(Number(this.Service.CodeArchiveReceipt),
                                      undefined,
                                      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
                                      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
                                      this.tableConfigurations.pagination.pageIndex + 1,
                                      this.tableConfigurations.pagination.pageSize,
                                      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }



  async navigateToReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
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
                       <th>درخواست دهنده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>توضیحات</th>
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
                           <td>${data.documentDescription}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'بایگانی رسید موقت')
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
