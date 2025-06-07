import { Component, HostListener, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";

import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { FormControl, FormGroup } from '@angular/forms';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Commodity } from '../../../../accounting/entities/commodity';
import { BaseSetting } from '../../../entities/base';
import { GetReceiptsCommoditesQuery } from '../../../repositories/commodity/get-receipt-commodites-query';
import { GetWarhouseLayoutHistotyByDocumentQuery } from '../../../repositories/warehouse-layout/queries/warhouse-layouts-report/get-warehouse-layouts-document-history-query';
@Component({
  template: ''
})
export abstract class WarehouseReceiptsBooksComponent extends BaseSetting {

  total: number = 0;
  totalItemUnitPrice: number = 0;
  totalQuantity: number = 0;
  sumAll: number = 0;
  totalExteraCost: number = 0;


  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({
    invoceNo: new FormControl(),
    documentNo: new FormControl(),
    commodityId: new FormControl(),
    requestNo: new FormControl(),
    warehouseId: new FormControl(),
    accountReferencesId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(
    public router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
  }


  ReferenceSelect(item: any) {

    this.SearchForm.controls.accountReferencesId.setValue(item?.id);

  }

  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  ngOnInit(params?: any) {
    this.get();
  }
  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);


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
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountReferenceId",
        values: [this.SearchForm.controls.accountReferencesId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
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
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    let orderby = 'serialNumber'
    let request = new GetWarhouseLayoutHistotyByDocumentQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.currentPage,
      this.gePageSize(),
      searchQueries, orderby)
    await this._mediator.send(request).then(res => {


      if (this.currentPage != 0) {
        this.data = res.data;

        this.Reports_filter = res.data;
        this.CalculateSum();
        if (this.currentPage == 1) {
          this.RowsCount = res.totalCount;
        }
      } else {

        this.exportexcel(res.data);
      }

    })

  }

  selectedRows(event: any) {
    this.CalculateSum(event);
  }

  CalculateSum(isSelectedRows?: boolean) {

    this.total = 0;
    this.totalItemUnitPrice = 0;
    this.totalExteraCost = 0;
    this.totalQuantity = 0;
    this.sumAll = 0;
    if (!isSelectedRows) {
      this.dataLength = this.data.length;


      this.data.forEach(a => {
        this.total += Number(a.quantity ? a.quantity : 0);
        this.totalQuantity += Number(a.totalQuantity ? a.totalQuantity : 0);
        this.sumAll += Number(a.itemUnitPrice ? a.itemUnitPrice : 0) * Number(a.quantity ? a.quantity : 0);
        const extraCost = a && a.extraCost !== undefined && !isNaN(Number(a.extraCost)) ? Number(a.extraCost) : 0;
        this.totalExteraCost += extraCost;
        this.totalItemUnitPrice += Number(a.itemUnitPrice ? a.itemUnitPrice : 0);

      });
      // this.totalItemUnitPrice = this.sumAll / this.total;
    } else {
      //   extraCost itemUnitPrice quantity
      this.Service.calculateSumTableOrRows('quantity').then(
        res => {
          this.total = res;
        }
      )

      this.Service.calculateSumTableOrRows('itemUnitPrice').then(
        res => {
          this.totalItemUnitPrice = res;

        }
      )
      this.Service.calculateSumTableOrRows('extraCost').then(
        res => {
          this.totalExteraCost = res;
        }
      )
      //sumAll
      this.Service.calculateSumTableOrRows('itemUnitPrice', 'quantity').then(
        res => {
          this.sumAll = res;
          // this.totalItemUnitPrice = this.sumAll / this.total;
        }
      )
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
    }


  }

  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }

  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, 'گزارش ورودی و خروجی کالا انبارها')
    }
  }




  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }

  async navigateToRequestReceipt(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${item.requestNo}&displayPage=archive&codeVoucherGroupId=0`)

  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }

  async CopyInCommodity(item: any) {

    let searchQueries: SearchQuery[] = []

    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [item.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(true, this.SearchForm.controls.warehouseId.value, "", 0, 50, searchQueries)).then(res => {

      this.SearchForm.controls.commodityId.setValue(res.data[0].id);
      this.get();
    });

  }
}
