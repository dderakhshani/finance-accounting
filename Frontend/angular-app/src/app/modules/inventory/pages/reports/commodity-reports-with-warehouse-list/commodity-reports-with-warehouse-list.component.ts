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
import { GetCommodityReportsWithWarehouseQuery } from '../../../repositories/reports/get-commodity-reports-with-warehouse-query';
import { GetWarehousesLastLevelQuery } from '../../../repositories/warehouse/queries/get-warehouses-recursives-query';




@Component({
  selector: 'app-commodity-reports-with-warehouse-list',
  templateUrl: './commodity-reports-with-warehouse-list.component.html',
  styleUrls: ['./commodity-reports-with-warehouse-list.component.scss']
})
export class CommodityReportsWithWarehouseComponent extends BaseSetting {


  WarehouseNodes: any[] = [];
  Warehouses: any[] = [];
  filterWarehouseNodes: any[] = [];



  ReceiptAllStatus: ReceiptAllStatusModel[] = [];


  tableConfigurations!: TableConfigurations;
  panelOpenState = true;



  SearchForm = new FormGroup({
    warehouseId: new FormControl(),
    documentStauseBaseValue: new FormControl(1),
    codeVoucherGroupId: new FormControl(),
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

    await this._mediator.send(new GetWarehousesLastLevelQuery(0, 0, undefined)).then(async (res) => {

      this.WarehouseNodes = res.data

    })
  }
  setvalue(value:any) {
    this.SearchForm.controls.documentStauseBaseValue.setValue(value);
  }
  getWarehousesList(items: any[]) {

    this.filterWarehouseNodes = items;
  }
  async get(searchQueries: SearchQuery[] = []) {



    searchQueries = this.GetChildFilter(searchQueries);

    let warehouseIds = ''
    this.filterWarehouseNodes.forEach(a => warehouseIds += ',' + a.id.toString())


    let request = new GetCommodityReportsWithWarehouseQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      warehouseIds,
      this.SearchForm.controls.codeVoucherGroupId.value,
      this.SearchForm.controls.documentStauseBaseValue.value,
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

  public prefix_Quantity: number=0;

  public prefix_TotalItemPrice: number = 0;

  public current_Enter_Used_Quantity: number = 0;
  public current_Enter_Used_TotalItemPrice: number = 0;

  public current_Enter_Purchase_Quantity: number = 0;
  public current_Enter_Purchase_TotalItemPrice: number = 0;

  public current_Exit_Quantity: number = 0;
  public current_Exit_TotalItemPrice: number = 0;
  public postfix_Quantity: number= 0;

  public postfix_TotalItemPrice: number = 0;

  CalculateSum(isSelectedRows:boolean =false) {

    this.prefix_Quantity = 0;
    this.prefix_Quantity= 0;
    this.prefix_TotalItemPrice= 0;
    this.current_Enter_Purchase_Quantity= 0;
    this.current_Enter_Purchase_TotalItemPrice = 0;
    this.current_Enter_Used_Quantity= 0;
    this.current_Enter_Used_TotalItemPrice= 0;
    this.current_Exit_Quantity= 0;
    this.current_Exit_TotalItemPrice= 0;
    this.postfix_Quantity= 0;
    this.postfix_TotalItemPrice = 0;

    if (isSelectedRows) {
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
      //quantity
      this.Service.calculateSumTableOrRows('prefix_Quantity').then(
        res => {
          this.prefix_Quantity = res;
        })

      this.Service.calculateSumTableOrRows('current_Enter_Used_Quantity').then(
        res => {
          this.current_Enter_Used_Quantity = res;
        })
      this.Service.calculateSumTableOrRows('current_Enter_Purchase_Quantity').then(
        res => {
          this.current_Enter_Purchase_Quantity = res;
        })
      this.Service.calculateSumTableOrRows('current_Exit_Quantity').then(
        res => {
          this.current_Exit_Quantity = res;
        })
      this.Service.calculateSumTableOrRows('postfix_Quantity').then(
        res => {
          this.postfix_Quantity = res;
        })
      //totalProductionCost
      this.Service.calculateSumTableOrRows('prefix_TotalItemPrice').then(
        res => {
          this.prefix_TotalItemPrice = res;
        })

      this.Service.calculateSumTableOrRows('current_Enter_Used_TotalItemPrice').then(
        res => {
          this.current_Enter_Used_TotalItemPrice = res;
        })
      this.Service.calculateSumTableOrRows('current_Enter_Purchase_TotalItemPrice').then(
        res => {
          this.current_Enter_Purchase_TotalItemPrice = res;
        })

      this.Service.calculateSumTableOrRows('current_Exit_TotalItemPrice').then(
        res => {
          this.current_Exit_TotalItemPrice = res;
        })
      this.Service.calculateSumTableOrRows('postfix_TotalItemPrice').then(
        res => {
          this.postfix_TotalItemPrice = res;
        })


    } else {

      this.data.forEach(a => {


        this.prefix_Quantity += Number(a.prefix_Quantity);
        this.prefix_TotalItemPrice += Number(a.prefix_TotalItemPrice);
        this.current_Enter_Purchase_Quantity += Number(a.current_Enter_Purchase_Quantity);
        this.current_Enter_Purchase_TotalItemPrice += Number(a.current_Enter_Purchase_TotalItemPrice);

        this.current_Enter_Used_Quantity += Number(a.current_Enter_Used_Quantity);
        this.current_Enter_Used_TotalItemPrice += Number(a.current_Enter_Used_TotalItemPrice);

        this.current_Exit_Quantity += Number(a.current_Exit_Quantity);
        this.current_Exit_TotalItemPrice += Number(a.current_Exit_TotalItemPrice);
        this.postfix_Quantity += Number(a.postfix_Quantity);
        this.postfix_TotalItemPrice += Number(a.postfix_TotalItemPrice);
      });


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




  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }

  async print() {

    let printContents = document.getElementById('report-Used_Quantity-2')?.innerHTML;

    var title = `'گزارش موجودی ریالی و مصرفی انبار' :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }
}
