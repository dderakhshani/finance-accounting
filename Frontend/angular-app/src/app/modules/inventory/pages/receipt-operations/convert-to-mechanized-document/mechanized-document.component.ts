import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { ActivatedRoute, Data, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { TableConfigurations } from "../../../../../core/components/custom/table/models/table-configurations";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { AccountReference } from '../../../../accounting/entities/account-reference';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetReferenceReciptWarhouseQuery } from '../../../repositories/personal/get-reference-receipt-query';
import { ConvertToMechanizedDocumentCommand } from '../../../repositories/mechanized-document/commands/convert-mechanized-document-command';
import { DocumentState } from '../../../entities/documentState';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { GetAllReceiptGroupbyInvoiceQuery } from '../../../repositories/reports/get-receipts-groupby-invoice-query';


@Component({
  selector: 'app-mechanized-document',
  templateUrl: './mechanized-document.component.html',
  styleUrls: ['./mechanized-document.component.scss']
})

export class MechanizedDocumentComponent extends BaseComponent {
  @ViewChild('buttonDetails', { read: TemplateRef }) buttonDetails!: TemplateRef<any>;
  @ViewChild('txtdebitReferenceTitle', { read: TemplateRef }) txtdebitReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtcreditReferenceTitle', { read: TemplateRef }) txtcreditReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtCodeVoucherGroupTitle', { read: TemplateRef }) txtCodeVoucherGroupTitle!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  totalItemPrice: number = 0;
  accountReferences: AccountReference[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];

  panelOpenState = true;
  tableConfigurations!: TableConfigurations;
  viewType: string = this.getQueryParam('viewType');

  SearchForm = new FormGroup({
    invoceNo: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
    debitAccountHeadId: new FormControl(),
    creditAccountHeadId: new FormControl(),
    financialOperationNumber: new FormControl(),
    documentStauseBaseValue: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh
  ]

  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private _snackBar: MatSnackBar,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async ngOnInit() {

    //--------------------------بررسی کدهایی که سند  مکانیزه آن خورده باشد اما بروزرسانی نشده باشد.
    var request = new ConvertToMechanizedDocumentCommand();
    await this._mediator.send(<ConvertToMechanizedDocumentCommand>request)

  }

  async resolve() {


    let colCodeVoucherGroupTitle = new TableColumn(
      'codeVoucherGroupTitle',
      'نوع رسید',
      TableColumnDataType.Template,
      '11%',
      true,
      new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
    );
    let colbuttonDetails = new TableColumn(
      'details',
      'جزئیات',
      TableColumnDataType.Template,
      '5%',
      true,

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
    let voucherHeadId = new TableColumn(
      'voucherHeadId',
      'سند مکانیزه',
      TableColumnDataType.Text,
      '5%',
      true,
      new TableColumnFilter('voucherHeadId', TableColumnFilterTypes.Number)

    );
    let financialOperationNumber = new TableColumn(
      'financialOperationNumber',
      'شماره عملیات',
      TableColumnDataType.Text,
      '5%',
      true,
      new TableColumnFilter('financialOperationNumber', TableColumnFilterTypes.Number)

    );

    let totalProductionCost = new TableColumn(
      'totalProductionCost',
      'مبلغ اقلام رسید',
      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('totalProductionCost', TableColumnFilterTypes.Number)
    );


    let vatDutiesTax = new TableColumn(
      'vatDutiesTax',
      'مبلغ مالیات',
      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('vatDutiesTax', TableColumnFilterTypes.Number)
    );

    let extraCost = new TableColumn(
      'extraCost',
      'هزینه اضافی',
      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('extraCost', TableColumnFilterTypes.Money)
    );
    let totalItemPrice = new TableColumn(
      'totalItemPrice',
      'مبلغ کل',

      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('totalItemPrice', TableColumnFilterTypes.Number)
    );



    colCodeVoucherGroupTitle.template = this.txtCodeVoucherGroupTitle;
    colredebitferenceTitle.template = this.txtdebitReferenceTitle;
    colcreditreferenceTitle.template = this.txtcreditReferenceTitle;
    colbuttonDetails.template = this.buttonDetails;

    let columns: TableColumn[] = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'selected',
        'انتخاب',
        TableColumnDataType.Select,
        '5%',
        false,
        new TableColumnFilter('select', TableColumnFilterTypes.Select)

      ),
      colCodeVoucherGroupTitle,
      new TableColumn(
        'invoiceNo',
        'شماره رسید',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('invoiceNo', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'invoiceDate',
        'تاریخ مالی',
        TableColumnDataType.Date,
        '5%',
        true,
        new TableColumnFilter('invoiceDate', TableColumnFilterTypes.Date)
      ),
      voucherHeadId,
      colredebitferenceTitle,
      colcreditreferenceTitle,


    ]

    if (this.viewType == 'accounting') {
      columns.push(financialOperationNumber, totalProductionCost, vatDutiesTax, extraCost, totalItemPrice, colbuttonDetails)
    }
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();
    this.ReferenceFilter('');
    await this.initialize()
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
    //-------------لیست تامین کنندگان در لیست------------------------------
    await this._mediator.send(new GetReferenceReciptWarhouseQuery(0, 25, filter)).then(res => {

      this.accountReferences = res.data
    })

  }
  async initialize() {


  }

  async get(searchQueries: SearchQuery[] = []) {

    this.Service.ListId = [];


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



    if (this.SearchForm.controls.invoceNo.value != undefined && this.SearchForm.controls.invoceNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.invoceNo.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.documentStauseBaseValue.value != undefined && this.SearchForm.controls.documentStauseBaseValue.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.SearchForm.controls.documentStauseBaseValue.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }

    //-------------------سندهایی که صادر نشده اند.
    searchQueries.push(new SearchQuery({
      propertyName: "VoucherHeadId",
      values: [0],
      comparison: 'equal',
      nextOperand: "and"
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "documentStauseBaseValue",
      values: [DocumentState.invoiceAmount, DocumentState.invoiceAmountLeave, DocumentState.invoiceAmountStart],
      comparison: "in",
      nextOperand: "and "
    }))

    if (this.viewType != 'accounting') {
      searchQueries = this.GetJustExitsReceipts(searchQueries)
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }


    let request = new GetAllReceiptGroupbyInvoiceQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      undefined,
      undefined,
      this.SearchForm.controls.creditAccountReferenceId.value,
      this.SearchForm.controls.debitAccountReferenceId.value,
      this.SearchForm.controls.creditAccountHeadId.value,
      this.SearchForm.controls.debitAccountHeadId.value,
      this.SearchForm.controls.creditAccountReferenceId.value,
      this.SearchForm.controls.debitAccountReferenceGroupId.value,
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);


    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    this.tableConfigurations.options.showSumRow = false;
    this.Receipts = response.data;
    if (this.viewType == 'accounting') {
      this.totalItemPrice = response.totalSum;
    }


  }


  codeVoucherGroupSelect(id: any) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(id);

  }
  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }


  clearListId() {
    this.Service.ListId = [];
    this.Receipts.forEach(a => {
      a.selected = false;
    })
  }
  getThreeDayBefor() {
    let searchQueries: SearchQuery[] = []

    searchQueries = this.GetJustExitsReceipts(searchQueries)
    this.SearchForm.controls.toDate.setValue(new Date(new Date().setHours(-72, 0, 0, 0)))

    this.get(searchQueries).then(a => {
      this.Receipts.forEach(a => a.selected = true);
      this.update()
    });


  }
  GetJustExitsReceipts(searchQueries: SearchQuery[] = []) {
    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2329],
      comparison: "notEqual",
      nextOperand: "and "
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2330],
      comparison: "notEqual",
      nextOperand: "and "
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2331],
      comparison: "notEqual",
      nextOperand: "and "
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2332],
      comparison: "notEqual",
      nextOperand: "and "
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2382],
      comparison: "notEqual",
      nextOperand: "and "
    }))
    return searchQueries;
  }
  async navigateToReceiptAll(Receipt: Receipt) {

    await this.router.navigateByUrl(`inventory/documentAccountingDatails?documentId=${Receipt.documentId}`);

  }
  async update() {
    this.ApiCallService.updateCreateAddAutoVoucher2(this.Receipts, 'insert');

  }

  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره vs</th>
                       <th>تاریخ مالی</th>
                       <th>شماره عملیات مالی</th>
                       <th>حساب بدهکار</th>
                       <th>حساب بستانکار</th>
                       <th>مبلغ اقلام رسید</th>
                       <th>مبلغ مالیات</th>
                       <th>هزینه اضافی</th>
                       <th>مبلغ کل</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.invoiceDate)}</td>
                           <td>${data.financialOperationNumber}</td>
                           <td>${data.debitReferenceTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.totalProductionCost.toLocaleString()}</td>
                           <td>${data.vatDutiesTax.toLocaleString()}</td>
                           <td>${data.extraCost.toLocaleString()}</td>
                           <td>${data.totalItemPrice.toLocaleString()}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای ریالی')
    }
  }
  async add() {

  }

  close(): any {
  }

  delete(): any {
  }


}
