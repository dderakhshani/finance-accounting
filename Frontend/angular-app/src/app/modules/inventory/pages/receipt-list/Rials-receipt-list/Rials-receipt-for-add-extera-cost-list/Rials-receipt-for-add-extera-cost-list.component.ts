import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../../entities/receipt";
import { ActivatedRoute, Router } from "@angular/router";
import { FormAction } from "../../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../../shared/services/search/models/search-query";
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { GetReceiptsQuery } from '../../../../repositories/receipt/queries/receipt/get-receipts-query';
import { TableConfigurations } from '../../../../../../core/components/custom/table/models/table-configurations';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';


import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { DocumentState } from '../../../../entities/documentState';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-Rials-receipt-for-add-extera-cost-list',
  templateUrl: '../Rials-receipt-list.component.html',
  styleUrls: ['../Rials-receipt-list.component.scss']
})
export class RialsReceiptForAddExteraCostListComponent extends BaseComponent {

  @ViewChild('txtCommodity', { read: TemplateRef }) txtCommodity!: TemplateRef<any>;
  @ViewChild('txtDocumentId', { read: TemplateRef }) txtDocumentId!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('txtCurrencyPrice', { read: TemplateRef }) txtCurrencyPrice!: TemplateRef<any>;
  @ViewChild('txtdebitReferenceTitle', { read: TemplateRef }) txtdebitReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtcreditReferenceTitle', { read: TemplateRef }) txtcreditReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtCodeVoucherGroupTitle', { read: TemplateRef }) txtCodeVoucherGroupTitle!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  tableConfigurations!: TableConfigurations;



  SearchForm = new FormGroup({
    requestNo: new FormControl(),
    invoiceNo: new FormControl(),
    documentNo: new FormControl(),
    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
    financialOperationNumber: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    fromInvoiceDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toInvoiceDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
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

  async ngOnInit() {

  }
  async ngAfterViewInit() {

    await this.resolve()
  }
  async resolve() {

    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '10%',
      false

    )
    let colSelected = new TableColumn(
      'selected',
      'انتخاب',
      TableColumnDataType.Template,
      '5%',
      false
    );
    let colDocumentId = new TableColumn(
      'documentId',
      'شماره مالی',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('documentId', TableColumnFilterTypes.Number)
    );
    let colCommodityTitle = new TableColumn(
      'commodityTitle',
      '    کالا  ',
      TableColumnDataType.Template,
      '24%',
      true,
      new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
    );
    let colCodeVoucherGroupTitle = new TableColumn(
      'codeVoucherGroupTitle',
      'نوع رسید',
      TableColumnDataType.Template,
      '5%',
      true,
      new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
    );
    let colcreditreferenceTitle = new TableColumn(
      'creditReferenceTitle',
      'حساب بستانکار',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
    );
    let colredebitferenceTitle = new TableColumn(
      'debitReferenceTitle',
      'حساب بدهکار',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('debitReferenceTitle', TableColumnFilterTypes.Text)
    );

    let colExtraCost = new TableColumn(
      'extraCost',
      'کرایه حمل',
      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('extraCost', TableColumnFilterTypes.Money)
    )

    let colFinancialOperationNumber = new TableColumn(
      'financialOperationNumber',
      'عملیات مالی',
      TableColumnDataType.Text,
      '10%',
      true,
      new TableColumnFilter('financialOperationNumber', TableColumnFilterTypes.Text)
    )


    colTagArray.template = this.buttonTagArray;
    colCommodityTitle.template = this.txtCommodity;
    colredebitferenceTitle.template = this.txtdebitReferenceTitle;
    colcreditreferenceTitle.template = this.txtcreditReferenceTitle;
    colCodeVoucherGroupTitle.template = this.txtCodeVoucherGroupTitle;

    colDocumentId.template = this.txtDocumentId;
    //--------------------------------------------------------------------------
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      colDocumentId,
      new TableColumn(
        'documentNo',
        'شماره رسید',
        TableColumnDataType.Number,
        '10%',
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
      colredebitferenceTitle,
      colcreditreferenceTitle,

    ]

    columns.push(new TableColumn(
      'invoiceNo',
      'صورتحساب',
      TableColumnDataType.Text,
      '5%',
      true,
      new TableColumnFilter('invoiceNo', TableColumnFilterTypes.Text)
    ))
    columns.push(new TableColumn(
      'isFreightChargePaid',
      'کرایه حمل برعهده ماست؟',
      TableColumnDataType.CheckBox,
      '10%',
      true,
      new TableColumnFilter('isFreightChargePaid', TableColumnFilterTypes.Money)
    ))
    columns.push(colExtraCost)




    columns.push(colFinancialOperationNumber)
    columns.push(colCodeVoucherGroupTitle);

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
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }


    if (this.SearchForm.controls.fromInvoiceDate.value != undefined && this.SearchForm.controls.fromInvoiceDate.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceDate",
        values: [this.SearchForm.controls.fromInvoiceDate.value],
        comparison: "greaterThan",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.toInvoiceDate.value != undefined && this.SearchForm.controls.toInvoiceDate.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceDate",
        values: [new Date(new Date(<Date>(this.SearchForm.controls.toInvoiceDate.value)).setHours(24, 0, 0, -1))],
        comparison: "lessThan",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "lessThan",
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

    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.financialOperationNumber.value != undefined && this.SearchForm.controls.financialOperationNumber.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "financialOperationNumber",
        values: [this.SearchForm.controls.financialOperationNumber.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    searchQueries.push(new SearchQuery({
      propertyName: "documentStauseBaseValue",
      values: [DocumentState.invoiceAmount, DocumentState.registrationAccounting],
      comparison: "in",
      nextOperand: "and "
    }))

    searchQueries.push(new SearchQuery({
      propertyName: "isImportPurchase",
      values: [false],
      comparison: "equal",
      nextOperand: "and"
    }))


    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2330, 2334],
      comparison: "in",
      nextOperand: "and"
    }))


    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetReceiptsQuery(0,
      undefined,
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    this.tableConfigurations.options.showSumRow = true;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

    this.tableConfigurations.options.sumLabel = 'جمع کل';
    this.tableConfigurations.options.showSumRow = true;

  }

  async navigateToRialReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?documentId=${Receipt.documentId}&isImportPurchase=false`)
  }
  async navigateToReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?id=${Receipt.id}&isImportPurchase=false`)
  }
  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }

  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>

                       <th>شماره رسید</th>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ مالی</th>
                       <th>شماره عملیات مالی</th>
                       <th>نام کالا</th>
                       <th>کد کالا</th>
                       <th>نوع رسید</th>
                       <th>حساب بدهکار</th>
                       <th>حساب بستانکار</th>
                       <th>هزینه اضافی</th>
                       <th>مبلغ کل</th>

                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>

                           <td>${data.documentNo}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.invoiceDate)}</td>
                           <td>${data.financialOperationNumber}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.codeVoucherGroupTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.debitReferenceTitle}</td>
                           <td>${data.extraCost}</td>
                           <td>${data.totalItemPrice}</td>

                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای ریالی')
    }
  }


  //--------------------------------------------------------------
  async update() {

  }

  async add() {

  }



  close(): any {
  }

  delete(): any {
  }


}
