import { Component, HostListener, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { FormControl, FormGroup } from '@angular/forms';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Commodity } from '../../../../accounting/entities/commodity';
import { GetReceiptsCommoditesQuery } from '../../../repositories/commodity/get-receipt-commodites-query';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { BaseSetting } from '../../../entities/base';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { Receipt } from '../../../entities/receipt';
import {
  ConfirmDialogComponent
} from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { GetAllReceiptItemsQuery } from '../../../repositories/receipt/queries/receipt/get-all-receipt-items-query';
import { DocumentState } from '../../../entities/documentState';
import { GetAssetsByDocumentIdQuery } from '../../../repositories/assets/queries/get-assets-by-documentId';
import {
  CommoditySerialViewDialog
} from '../../component/commodity-serial-view-dialog/commodity-serial-view-dialog.component';
import { GetAllReceiptGroupbyInvoiceQuery } from '../../../repositories/reports/get-receipts-groupby-invoice-query';
import {
  ConvertToMechanizedDocumentCommand
} from '../../../repositories/mechanized-document/commands/convert-mechanized-document-command';
import { ReceiptItem } from "../../../entities/receipt-item";
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';
import { GetReceiptGroupInvoiceQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-group-invoice-query';

@Component({
  template: ''
})
export abstract class InvoiceBuy extends BaseSetting {
  public rialsStatus: number = 1;
  public isProvider: boolean = false;
  public isViewJustSelected: boolean = false;
  public Receipt: Receipt[] = [];
  public Receipts: any[] = [];
  public selectedAll: boolean = false;
  public selectedAllRead: boolean = false;
  public total: number = 0;
  public totalItemUnitPrice: number = 0;
  public totalQuantity: number = 0;
  public sumAll: number = 0;
  public isImportPurchase = this.getQueryParam('isImportPurchase')
  public AmountReturnReceipt: number = 2381;
  public privateListId: string[] = [];
  // @ts-ignore
  public isReadReceipt: number = 1;
  public showReceiptListTab: boolean = false;
  public text_danger: string = ''

  constructor(
    public router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    public route: ActivatedRoute,
    public sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
  }

  public SearchForm = new FormGroup({
    quantity: new FormControl(),
    requestNo: new FormControl(),
    invoceNo: new FormControl(),
    documentNo: new FormControl(),
    scaleBill: new FormControl(),
    documentId: new FormControl(),
    commodityId: new FormControl(),
    warehouseId: new FormControl(),
    accountReferencesId: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    debitAccountReferencesId: new FormControl(),
    debitAccountHeadId: new FormControl(),
    creditAccountHeadId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });
  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]

  async ngOnInit() {
    this.Service.ListId = [];
    this.privateListId = [];
    //--------------------------بررسی کدهایی که سند  مکانیزه آن خورده باشد اما بروزرسانی نشده باشد.
    var request = new ConvertToMechanizedDocumentCommand();
    await this._mediator.send(<ConvertToMechanizedDocumentCommand>request)
  }

  async ngAfterViewInit() {
  }

  async resolve() {
  }

  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);
  }

  debitAccountReferencesIdSelect(item: any) {
    this.SearchForm.controls.debitAccountReferencesId.setValue(item?.id);
  }

  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }

  onChangeState(bottomName: string) {
    this.currentPage = 1;
    switch (bottomName) {
      case 'convertRials': {
        this.rialsStatus = 1;
        break;
      }
      case 'Rials': {
        this.rialsStatus = 2;
        break;
      }
      case 'document': {
        this.rialsStatus = 3;
        break;
      }
    }
    this.get([]);
  }

  async get(searchQueries: SearchQuery[] = []) {
    await this.getData(searchQueries).then(a => {

      let that = this;
      setTimeout(() => {
        that.ViewSelectedRowsOnly();
      }, 1);
    })
  }
  returnReceipt(codeVoucherGroupId: number) {
    var values = [2381, 2382, 2383]
    return values.includes(codeVoucherGroupId)

  }

  FilterConvertToReial(searchQueries: SearchQuery[] = []) {
    //مرجوعی
    if (this.SearchForm.controls.codeVoucherGroupId.value == 3) {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2381],
        comparison: 'in',
        nextOperand: "and"
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeInvoiceAmountLeave],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    //فقط خرید
    if (this.SearchForm.controls.codeVoucherGroupId.value == 2) {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2320, 2322, 2323, 2324],
        comparison: 'in',
        nextOperand: "and"
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeDirectReceipt],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    //همه
    else {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2320, 2322, 2323, 2324, 2381],//مصرفی - قطعات - اموال- مواد اولیه -کالا برگشتی
        comparison: "in",
        nextOperand: "and "
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeDirectReceipt, this.Service.CodeInvoiceAmountLeave],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    return searchQueries;
  }

  FilterReials(searchQueries: SearchQuery[] = []) {
    //مرجوعی
    if (this.SearchForm.controls.codeVoucherGroupId.value == 3) {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2382],
        comparison: 'in',
        nextOperand: "and"
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeInvoiceAmountLeave],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    //فقط خرید
    if (this.SearchForm.controls.codeVoucherGroupId.value == 2) {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2329, 2330, 2331, 2332],
        comparison: 'in',
        nextOperand: "and"
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeInvoiceAmountReceipt],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    //همه
    else {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2329, 2330, 2331, 2332, 2382],//مصرفی - قطعات - اموال- مواد اولیه -رسید ریالی کالا برگشتی
        comparison: "in",
        nextOperand: "and "
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [this.Service.CodeInvoiceAmountReceipt, this.Service.CodeInvoiceAmountLeave],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    return searchQueries;
  }

  FilterDocument(searchQueries: SearchQuery[] = []) {
    //مرجوعی
    if (this.SearchForm.controls.codeVoucherGroupId.value == 3) {
      searchQueries.push(new SearchQuery({
        propertyName: "codeVoucherGroupId",
        values: [2381, 2382, 2383],
        comparison: 'in',
        nextOperand: "and"
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [DocumentState.registrationAccountingLeave],
        comparison: "in",
        nextOperand: "and "
      }))
    }
    //فقط خرید
    if (this.SearchForm.controls.codeVoucherGroupId.value == 2) {
      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [DocumentState.registrationAccounting],
        comparison: "in",
        nextOperand: "and "
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "viewId",
        values: [122],
        comparison: "notEqual",
        nextOperand: "and "
      }))
    }
    //همه
    else {



      searchQueries.push(new SearchQuery({
        propertyName: "documentStauseBaseValue",
        values: [DocumentState.registrationAccounting, DocumentState.registrationAccountingLeave],
        comparison: "in",
        nextOperand: "and "
      }))
      searchQueries.push(new SearchQuery({
        propertyName: "viewId",
        values: [122],
        comparison: "notEqual",
        nextOperand: "and "
      }))
    }
    return searchQueries;
  }

  async getData(searchQueries: SearchQuery[] = []) {
    searchQueries = this.GetChildFilter();
    searchQueries.push(new SearchQuery({
      propertyName: "isImportPurchase",
      values: [this.isImportPurchase],
      comparison: "equal",
      nextOperand: "and"
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "isPlacementComplete",
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
    searchQueries.push(new SearchQuery({
      propertyName: "viewId",
      values: [122],
      comparison: "notEqual",
      nextOperand: "and"
    }))

    switch (this.isReadReceipt) {
      case 2: {
        searchQueries.push(new SearchQuery({
          propertyName: "isRead",
          values: [true],
          comparison: "equal",
          nextOperand: "and"
        }));
        break;
      }
      case 3: {
        searchQueries.push(new SearchQuery({
          propertyName: "isRead",
          values: [false],
          comparison: "equal",
          nextOperand: "and"
        }));
        break;
      }
    }

    switch (this.rialsStatus) {
      case 1: {
        searchQueries = this.FilterConvertToReial(searchQueries)
        break;
      }
      case 2: {
        searchQueries = this.FilterReials(searchQueries)
        break;
      }
      case 3: {

        searchQueries = this.FilterDocument(searchQueries)
        break;
      }
    }
    //============================================

    searchQueries = this.FilterForm(searchQueries);

    //+++++++++++++++++++++++++++++++++++++++++++++++
    let orderby = this.isImportPurchase == 'true' ? 'creditReferenceTitle' : 'DocumentDate'
    let request = new GetAllReceiptItemsQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.currentPage,
      this.gePageSize(),
      searchQueries, orderby)
    await this._mediator.send(request).then(res => {
      if (this.currentPage != 0) {
        this.data = res.data;
        
        this.Reports_filter = res.data;


        if (this.isProvider) {
          // Step 1: Group by title
          const groupedByTitle = this.data.reduce((acc: { [key: string]: { [key: string]: Receipt[] } }, item: any) => {
            const { creditReferenceTitle, partNumber } = item;

            // Initialize title group if it doesn't exist
            if (!acc[creditReferenceTitle]) {
              acc[creditReferenceTitle] = {};
            }
            // Initialize partNumber group within title if it doesn't exist
            if (!acc[creditReferenceTitle][partNumber]) {
              acc[creditReferenceTitle][partNumber] = [];
            }
            // Push the current item into the appropriate group
            acc[creditReferenceTitle][partNumber].push(item);
            return acc;
          }, {});

          this.Receipts = Object.keys(groupedByTitle).map(title => {
            const parts = Object.keys(groupedByTitle[title]).map(partNumber => ({
              PartNumber: partNumber,
              Invoices: groupedByTitle[title][partNumber].map(({ partNumber, creditReferenceTitle, ...rest }) => rest),
             
            }));
            return {
              CreditReferenceTitle: title,
              Parts: parts,
              

            };
          });
        }
        if (this.currentPage == 1) {
          this.RowsCount = res.totalCount;
          
        }
        this.sumAll = res.totalSum;
        this.CalculateSum();
      } else {

        this.exportexcel(res.data);
      }
      if (this.data.length > 0)
        this.showReceiptListTab = true;
      else
        this.showReceiptListTab = false;
      return this.data;
    });
    return this.data;
  }
 
  FilterForm(searchQueries: SearchQuery[] = []) {
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.commodityId.value != undefined && this.SearchForm.controls.commodityId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "Commodityld",
        values: [this.SearchForm.controls.commodityId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.accountReferencesId.value != undefined) {
       if (this.SearchForm.controls.codeVoucherGroupId.value == 3) {
        searchQueries.push(new SearchQuery({
          propertyName: "debitAccountReferenceId",
          values: [this.SearchForm.controls.accountReferencesId.value],
          comparison: 'equal',
          nextOperand: "and"
        }))
      } else {
        searchQueries.push(new SearchQuery({
          propertyName: "creditAccountReferenceId",
          values: [this.SearchForm.controls.accountReferencesId.value],
          comparison: 'equal',
          nextOperand: "and"
        }))
      }

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
    if (this.SearchForm.controls.scaleBill.value != undefined && this.SearchForm.controls.scaleBill.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "scaleBill",
        values: [this.SearchForm.controls.scaleBill.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.quantity.value != undefined && this.SearchForm.controls.quantity.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "quantity",
        values: [this.SearchForm.controls.quantity.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.documentId.value != undefined && this.SearchForm.controls.documentId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentId",
        values: [this.SearchForm.controls.documentId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.debitAccountHeadId.value != undefined && this.SearchForm.controls.debitAccountHeadId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountHeadId",
        values: [this.SearchForm.controls.debitAccountHeadId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.creditAccountHeadId.value != undefined && this.SearchForm.controls.creditAccountHeadId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountHeadId",
        values: [this.SearchForm.controls.creditAccountHeadId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    return searchQueries
  }

  async ViewSelectedRowsOnly() {
    this.privateListId.forEach(a => {

      var selected = this.data.find(b => Number(b.documentHeadId).toString() == a)
      if (selected) {
        selected.selected = true;
      }
    })
    if (this.data.filter(a => a.selected).length > 0) {
      this.onViewJustSelected();
    }
  }

  selectedRows(event: any) {
    this.CalculateSumSelectRows();
  }

  CalculateSum(isSelectedRows?: any) {
    this.total = 0;
    this.totalItemUnitPrice = 0;
    this.totalQuantity = 0;
   
    this.dataLength = this.data.length
    this.data.forEach(a => this.total += Number(a.quantity));
    this.data.forEach(a => this.totalItemUnitPrice += Number(a.itemUnitPrice) * a.quantity);
    this.data.forEach(a => this.totalQuantity += Number(a.totalQuantity));


    //this.sumAll = 0;
    //this.data.forEach(a => this.sumAll += ((a.documentStauseBaseValue == this.Service.CodeInvoiceAmountLeave || a.documentStauseBaseValue == this.Service.CodeLeaveReceipt) ? -1 : 1) * Number(a.itemUnitPrice * a.quantity));

  }

  CalculateSumSelectRows() {
    this.sumAll = 0;
    this.totalQuantity = 0;
    this.total = 0;
    this.dataLength = this.data.filter(a => a.selected).length
    this.data.filter(a => a.selected).forEach(a => this.total += Number(a.quantity));
    this.data.filter(a => a.selected).forEach(a => this.sumAll += ((a.documentStauseBaseValue == this.Service.CodeInvoiceAmountLeave || a.documentStauseBaseValue == this.Service.CodeLeaveReceipt) ? -1 : 1) * Number(a.itemUnitPrice) * Number(a.quantity));
    this.data.filter(a => a.selected).forEach(a => this.totalQuantity += Number(a.totalQuantity));
    if (this.sumAll == 0 && this.total == 0) {
      this.CalculateSum();
    }
  }

  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {
    this.data = data
    this.ViewSelectedRowsOnly();
    this.CalculateSum();
  }

  async print(isRowSelect: any = undefined) {
    let printContents = '';
    let printtable = `<table><thead>
                     <tr>
                                  <th >ردیف</th>
                                  <th > تاریخ مالی</th>
                                  <th > شماره رسید  </th>
                                  <th > نام کالا </th>
                                  <th > کد کالا </th>
                                  <th > مقدار ورودی</th>
                                  <th > مبلغ واحد </th>
                                  <th >مبلغ کل</th>
                                  <th >مبلغ کل صورتحساب</th>
                                  <th >حساب بستانکار</th>
                                  <th >حساب بدهکار</th>
                                  <th > شماره مالی </th>
                                  <th > شماره صورتحساب</th>
                                  <th > شماره درخواست</th>
                                  <th >شرح رسید</th>
                                  <th >شرح کالا</th>
                     </tr>
                   </thead><tbody>`;
    let i = 1;
    var sumAll: any = 0
    var totalQuantity: any = 0
    var totalProductionCost: any = 0;
    this.data.filter(a => a.selected == true || isRowSelect == undefined).map(data => {
      printtable += `<tr>
                           <td>${i}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.itemUnitPrice?.toLocaleString()}</td>
                           <td>${(((data.documentStauseBaseValue == this.Service.CodeInvoiceAmountLeave || data.documentStauseBaseValue == this.Service.CodeLeaveReceipt) ? -1 : 1) * Number(data.itemUnitPrice * data.quantity)).toLocaleString()}</td>
                           <td>${data.totalProductionCost?.toLocaleString()}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.debitReferenceTitle}</td>
                           <td>${data.documentId}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${data.requestNo}</td>
                           <td>${data.documentDescription}</td>
                           <td>${data.descriptionItems}</td>

                        </tr>`
      i++;
      sumAll += ((data.documentStauseBaseValue == this.Service.CodeInvoiceAmountLeave || data.documentStauseBaseValue == this.Service.CodeLeaveReceipt) ? -1 : 1) * Number(data.itemUnitPrice * data.quantity)
      totalQuantity += Number(data.quantity)

      totalProductionCost = data.totalProductionCost;
    })
    printtable += `<tr>
                        <td colspan="5"></td>
                        <td colspan="1">${totalQuantity.toLocaleString()}</td>
                        <td ></td>
                        <td colspan="1">${sumAll.toLocaleString()}</td>
                        <td colspan="8"></td>
                  </tr>

                  </tbody></table>`

    printContents += `<hr />
                      <div style="width:100%;display:flex;text-align:center;font-size:14px;">
                          <div style="width:20%;">
                              <p>تاریخ مالی :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا  ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}</p>

                          </div>
                         <div style="width:40%;">
                          </div>
                          <div style="width:40%;">
                              <p> طرف حساب :${this.SearchForm.controls.accountReferencesId.value != undefined ? this.data[0]?.creditReferenceTitle : 'همه'}</p>

                          </div>
                      </div>
                    ${printtable}
       <div style="margin-top:15px; width:100%;height:75px;display:flex;text-align:center;font-size:14px; border: solid;border-width: 1px;">
            <b style="margin-top:5px;margin-right:5px;">توضیحات</b>

        </div>
        <div style="margin-top:15px; width:100%;display:flex;text-align:center;font-size:14px;">
            <div style="width: 24%;height:75px;text-align:start; border:solid;border-width:1px; padding:15px;"><b>نام و امضا حسابدار انبار</b></div>
            <div style="width: 24%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا سرپرست انبار</b></div>
            <div style="width: 24%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا بازرگانی</b></div>
            <div style="width: 23%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا اتوماسیون بایگانی</b></div>
        </div>
         `
    this.Service.onPrint(printContents, this.SearchForm.controls.codeVoucherGroupId.value == 3 ? 'عملیات برگشت از خرید' : 'عملیات خرید')
  }

  async navigateToHistory(Receipt: any) {
    let searchQueries: SearchQuery[] = []
    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [Receipt.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(true, Receipt.warehouseId, "", 0, 50, searchQueries)).then(res => {
      var url = `inventory/commodityReceiptReports?commodityId=${res.data[0].id}&warehouseId=${Receipt.warehouseId}`
      this.router.navigateByUrl(url)
    });
  }

  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }

  async navigateToReceipt(item: any) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?id=${item.documentHeadId}`)
  }

  async navigateToreceiptDetails(item: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${item.documentHeadId}&displayPage=none&isImportPurchase=${this.isImportPurchase}`)
  }

  async navigateToReceiptRials(item: any) {
    this.clearListId();
    this.checkValue(item);
    let url = `inventory/rialsReceiptDetails?documentId=${item.documentId}&isImportPurchase=${this.isImportPurchase}`
    await this.router.navigateByUrl(url)
  }

  async navigateToRialReceipt(Receipt: Receipt, editType: number) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?documentId=${Receipt.documentId}&isImportPurchase=${Receipt.isImportPurchase}&editType=${editType}`)
  }

  async navigateToReceiptRialsAll() {
    this.Service.ListId = [];
    this.privateListId = [];
    this.data.filter(a => a.selected).forEach(a => {
      if (a.documentHeadId != undefined) {
        this.Service.ListId.push(Number(a.documentHeadId).toString());
        this.privateListId.push(Number(a.documentHeadId).toString());
      }
    });
    if (this.Service.ListId.length == 0) {
      this.Service.showHttpFailMessage('هیچ سطری انتخاب نشده است');
      return;
    }
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?isImportPurchase=${this.isImportPurchase}`)
  }

  async navigateToRequestReceipt(requestNo: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${requestNo}&displayPage=archive&documentStauseBaseValue=${this.Service.CodeRequestBuy}`)

  }
  debitAccountHeadIdSelect(item: any) {

    this.SearchForm.controls.debitAccountHeadId.setValue(item?.id);

  }

  creditAccountHeadIdSelect(item: any) {

    this.SearchForm.controls.creditAccountHeadId.setValue(item?.id);

  }

  //انتخاب لیست جهت ریالی کردن رسید
  checkValue(SelectedReceipt: any) {
    let valid: boolean = true;
    this.data.filter(a => a.selected).forEach(request => {
      if (!request) return; // Skip if the request is undefined or null
      // Check for "ریالی نشده" receipts
      if (this.rialsStatus === 1) {
        const creditAccountMismatch = SelectedReceipt.creditAccountReferenceId !== undefined &&
          request.creditAccountReferenceId !== undefined &&
          SelectedReceipt.creditAccountReferenceId !== request.creditAccountReferenceId;
        if (creditAccountMismatch) {
          valid = false;
          this.Service.showHttpFailMessage('تنها رسید هایی قابل انتخاب است که دارای یک تامین کننده باشند');
          SelectedReceipt.selected = false;
          return;
        }
      }
    });

    // If valid
    if (valid && SelectedReceipt.documentHeadId !== undefined) {
      const documentHeadIdStr = Number(SelectedReceipt.documentHeadId).toString();
      this.Service.ListId.push(documentHeadIdStr);
      this.privateListId.push(documentHeadIdStr);
      let selectedRow = this.data.find(x => x.documentHeadId == SelectedReceipt.documentHeadId);
      selectedRow.selected = true;
      SelectedReceipt.selected = true;
      this.CalculateSumSelectRows();
    }
  }

  RemoveId(SelectedReceipt: any) {
    SelectedReceipt.id = SelectedReceipt.documentHeadId
    this.Service.RemoveId(SelectedReceipt);
    this.privateListId = this.privateListId.filter(a => (a != SelectedReceipt.id.toString()))
    this.CalculateSumSelectRows();
  }

  clearListId() {
    this.Service.clearListId(this.data);
    this.privateListId = [];
    this.CalculateSum();
  }

  onViewJustSelected() {
    if (this.isViewJustSelected) {
      this.data = this.data.filter(a => a.selected);
    } else {
      this.data = this.Reports_filter;
    }
    this.CalculateSum();
  }

  async exportexcel(data: any[]) {
    this.Service.onExportToExcel(data);
  }

  clearDocuments() {
    var list = this.data.filter((a: any) => a.allowInput == true)
    list.forEach(m => {
      m.allowInput = false;
      this.RemoveId(m);
    })
  }

  async ConvertToInvoice() {
    if (this.rialsStatus == 1) {
      this.Service.showHttpFailMessage('رسیدهای ریالی شده با قابلیت صدور سند مکانیزه انتخاب کنید')
      return;
    }
    if (this.privateListId.length == 0) {
      this.Service.showHttpFailMessage('هیچ رسیدی انتخاب نشده است')
      return;
    }
    let searchQueries: SearchQuery[] = []
    let documentIds = ''
    this.privateListId.forEach(res => {
      var dd = this.data.find((b: any) => b.documentHeadId == res);
      var list = this.data.filter((a: any) => a.documentId == dd?.documentId)
      list.forEach(m => {
        m.allowInput = true
        m.selected = false
      })
      if (dd?.documentId != undefined) {
        documentIds += ',' + dd?.documentId?.toString()

      }

    });
    documentIds = documentIds.substring(1, documentIds.length)
    let request = new GetReceiptGroupInvoiceQuery(

      undefined,
      undefined,
      documentIds,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      0,
      0,
      searchQueries, '')
    await this._mediator.send(request).then(res => {
      this.Receipt = [];
      this.Receipt = res.data;
      this.Receipt.forEach(a => {
        a.selected = true;
      })

      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'تایید صدور سند',
          message: this.Message(),
          /* icon: ConfirmDialogIcons.warning,*/
          actions: {
            confirm: { title: 'با همین شرایط سند صادر شود', show: true }, cancel: { title: 'سند صادر نشود', show: true }
          }
        }
      });
      dialogRef.afterClosed().subscribe(async res => {
        if (res == true) {
          this.ApiCallService.updateCreateAddAutoVoucher2(this.Receipt, 'insert').then(q => {
            this.Receipt.forEach(b => {
              if (b?.voucherHeadId > 0) {
                var list = this.data.filter((a: any) => a.documentId == b.documentId)
                list.forEach(m => {
                  m.allowInput = false;
                  m.allowOutput = true;
                  this.RemoveId(m)
                })
              }
            })
          });
        }
      });
    });
  }

  Message() {
    
    let printtable = `<h4>لیست اقلام تاثیر گذار در سند</h4>
                      <table class='mas-table' ><thead>
                     <tr>
                        <th>ردیف</th>
                        <th>تاریخ</th>
                        <th>شماره رسید</th>
                        <th>شماره درخواست</th>
                        <th>شماره فاکتور</th>
                        <th>شماره مالی</th>
                        <th>کد کالا</th>
                        <th>نام کالا</th>
                        <th>مقدار</th>
                        <th>قیمت واحد</th>
                        <th>مبلغ کل</th>
                        <th>طرف حساب</th>
                     </tr>
                   </thead><tbody>`;
    let i = 1;
    let totalQuantity = 0;

    const { sumAll } = this.data
      .filter(item => item.allowInput)
      .reduce((acc, item) => {
        if (acc.lastDocumentNo !== item.documentNo) {
          acc.sumAll += Number(item.totalProductionCost) || 0;
          acc.lastDocumentNo = item.documentNo;
        }
        return acc;
      }, { sumAll: 0, lastDocumentNo: null as number | null });
    this.data.filter(a => a.allowInput == true).map(data => {
      
      printtable += `<tr>
                           <th>${i}</th>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.requestNo}</td>
                           <td>${data.invoiceNo == undefined ? '' : data.invoiceNo}</td>
                           <td>${data.documentId}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.itemUnitPrice}</td>
                           <td>${Number(data.itemUnitPrice) * Number(data.quantity)}</td>
                           <td>${data.creditReferenceTitle}</td>
                        </tr>`
      i++;
      

      totalQuantity += Number(data.quantity)
    })
    printtable += `<tr>
                        <td colspan="8">جمع کل</td>
                        <td colspan="1">${totalQuantity}</td>
                        <td></td>
                        <td colspan="1">${sumAll}</td>
                        <td></td>
                  </tr>
                  </tbody></table>`

    return printtable;
  }

  async CommoditySerials(Receipt: any) {
    let dialogConfig = new MatDialogConfig();
    let assetsList: any = undefined;
    let searchQueries: SearchQuery[] = []
    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [Receipt.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(true, Receipt.warehouseId, "", 0, 50, searchQueries)).then(res => {
      let commodityId = res.data[0].id;
      this._mediator.send(new GetAssetsByDocumentIdQuery(Receipt.documentItemId, res.data[0].id)).then(res => {
        assetsList = res;
        dialogConfig.data = {
          commodityCode: Receipt.commodityCode,
          commodityId: commodityId,
          commodityTitle: Receipt.commodityTitle,
          quantity: Receipt.quantity,
          documentItemId: Receipt.documentItemId,
          assets: assetsList
        };
        let dialogReference = this.dialog.open(CommoditySerialViewDialog, dialogConfig);
        dialogReference.afterClosed();
      });
    });
  }
}
