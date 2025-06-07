import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { ActivatedRoute, Data, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';

import { AccountReference } from '../../../../accounting/entities/account-reference';

import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { BaseSetting } from '../../../entities/base';
import { GetAllReceiptGroupbyInvoiceQuery } from '../../../repositories/reports/get-receipts-groupby-invoice-query';




@Component({
  selector: 'app-mechanized-receipt-list',
  templateUrl: './mechanized-receipt-list.component.html',
  styleUrls: ['./mechanized-receipt-list.component.scss']
})
export class MechanizedReciptListComponent extends BaseSetting {


  totalItemUnitPrice: number = 0;
  totalProductionCost: number = 0;

  ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  accountReferences: AccountReference[] = [];

  tableConfigurations!: TableConfigurations;
  panelOpenState = true;



  SearchForm = new FormGroup({
    invoceNo: new FormControl(),
    voucherNo: new FormControl(),
    documentId: new FormControl(),
    codeVoucherGroupId: new FormControl(),
    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
    debitAccountHeadId: new FormControl(),
    creditAccountHeadId: new FormControl(),
    financialOperationNumber: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [
    FormActionTypes.refresh

  ]

  constructor(
    private router: Router,
    public _mediator: Mediator,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);

  }

  async ngOnInit() {
    this.pageSize = 300;

  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async resolve() {


    await this.get();

    await this.initialize()
  }


  async initialize() {


  }

  async get(searchQueries: SearchQuery[] = []) {



    searchQueries = this.GetChildFilter(searchQueries);

    if (this.SearchForm.controls.invoceNo.value != undefined && this.SearchForm.controls.invoceNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceNo",
        values: [this.SearchForm.controls.invoceNo.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.financialOperationNumber.value != undefined && this.SearchForm.controls.financialOperationNumber.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "financialOperationNumber",
        values: [this.SearchForm.controls.financialOperationNumber.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.voucherNo.value != undefined && this.SearchForm.controls.voucherNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "voucherNo",
        values: [this.SearchForm.controls.voucherNo.value],
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

    searchQueries.push(new SearchQuery({
      propertyName: "voucherHeadId",
      values: [1],
      comparison: "greaterThan",
      nextOperand: "and "
    }))


    let request = new  GetAllReceiptGroupbyInvoiceQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      undefined,
      undefined,
      this.SearchForm.controls.creditAccountReferenceId.value,
      this.SearchForm.controls.debitAccountReferenceId.value,
      this.SearchForm.controls.creditAccountHeadId.value,
      this.SearchForm.controls.debitAccountHeadId.value,
      this.SearchForm.controls.creditAccountReferenceGroupId.value,
      this.SearchForm.controls.debitAccountReferenceGroupId.value,
      this.currentPage,
      this.gePageSize(),
      searchQueries)

    await this._mediator.send(request).then(res => {


      if (this.currentPage != 0) {
        this.data = res.data;
        this.Reports_filter = res.data;
        this.CalculateSum();
        if (this.currentPage == 1) {
          this.RowsCount = res.totalCount;

        }
      }
      else {

        this.exportexcel(res.data);
      }

    })

  }
  selectedRows(event: any) {
    this.CalculateSum(event);
  }
  CalculateSum(isSelectedRows:boolean =false) {

    this.totalItemUnitPrice = 0;
    this.totalProductionCost = 0;
    if (isSelectedRows) {
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
      //quantity
      this.Service.calculateSumTableOrRows('totalItemUnitPrice').then(
        res => {
          this.totalItemUnitPrice = res;
        })
      //totalProductionCost
      this.Service.calculateSumTableOrRows('totalProductionCost').then(
        res => {
          this.totalProductionCost = res;
        })


    }else {
      this.data.forEach(a => this.totalItemUnitPrice += Number(a.totalItemPrice));
      this.data.forEach(a => this.totalProductionCost += Number(a.totalProductionCost));
      this.dataLength = this.data.length;
    }
  }
  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }
  async navigateToRialReceipt(Receipt: Receipt,editType:number) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?documentId=${Receipt.documentId}&isImportPurchase=${Receipt.isImportPurchase}&editType=${editType}`)
  }

  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?id=${item.id}&displayPage=none&isImportPurchase=${item.isImportPurchase}`)

  }
  async navigateToVoucher(Receipt: Receipt) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${Receipt.voucherHeadId}`)
  }
  ToWarehouseIdSelect(item: any) {
    this.SearchForm.controls.debitAccountHeadId.setValue(item?.accountHeadId);
  }
  FromWarehouseIdSelect(item: any) {
    this.SearchForm.controls.creditAccountHeadId.setValue(item?.accountHeadId);
  }
  async getReceiptAllStatus() {


    this.ReceiptAllStatus = await (await this.ApiCallService.getReceiptAllInventoryStatus());

    this.ReceiptAllStatus = this.ReceiptAllStatus.filter(b =>
      b.code.substring(2, 4) == this.Service.CodeRegistrationAccountingLeave.toString() ||
      b.code.substring(2, 4) == this.Service.CodeRegistrationAccountingReceipt.toString() ||
      b.code.substring(2, 4) == this.Service.CodeRegistrationAccountingStart.toString()
    )

  }
  codeVoucherGroupSelect(id: any) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(id);

  }

  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);
  }


  async print() {

    let printContents = '';
    if (this.data.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ سند</th>
                       <th>شماره عملیات مالی</th>
                       <th>حساب بدهکار</th>
                       <th>حساب بستانکار</th>
                       <th>مبلغ اقلام سند</th>
                       <th>مبلغ مالیات</th>
                       <th>هزینه اضافی</th>
                       <th>مبلغ کل</th>

                     </tr>
                   </thead><tbody>`;
      this.data.map(data => {
        printContents += `<tr>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate) }</td>
                           <td>${data.financialOperationNumber}</td>
                           <td>${data.debitReferenceTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.totalProductionCost.toLocaleString() }</td>
                           <td>${data.vatDutiesTax.toLocaleString() }</td>
                           <td>${data.extraCost.toLocaleString() }</td>
                           <td>${data.totalItemPrice.toLocaleString() }</td>


                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای ریالی')
    }
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }

}
