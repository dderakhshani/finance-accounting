import { Router } from "@angular/router";
import { MatDialog } from '@angular/material/dialog';
import { Warehouse } from '../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { Component } from '@angular/core';
import { Commodity } from '../../../../commodity/entities/commodity';
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableConfigurations } from '../../../../../core/components/custom/table/models/table-configurations';
import { GetCommodityReportsQuery } from "../../../repositories/reports/get-commodity-reports";
import { spCommodityReports } from "../../../entities/spCommodityReports";
import { BaseSetting } from "../../../entities/base";
import { GetCommodityReportsSumAllQuery } from "../../../repositories/reports/get-commodity-reports-sum-all";
import { GetWarehousesLastLevelQuery } from "../../../repositories/warehouse/queries/get-warehouses-recursives-query";
import { GetCategoriesCodeProductQuery } from "../../../repositories/commodity-categories/get-commodity-categories-code-product-query";
import { CommodityCategory } from "../../../../commodity/entities/commodity-category";


@Component({
  template: ''
})

export abstract class AvailableCommodityReportsComponent extends BaseSetting {


  sumALL: any[] = [];
  WarehouseNodes: any[] = [];
  Warehouses: any[] = [];
  filterWarehouseNodes: any[] = [];

  prefix_TotalItemPrice: number = 0;
  current_Enter_ItemUnitPrice: number = 0;
  current_Exit_TotalItemPrice: number = 0;
  postfix_TotalItemPrice: number = 0;

  prefix_Quantity: number = 0;
  current_Enter_Quantity: number = 0;
  current_Exit_Quantity: number = 0;
  postfix_Quantity: number = 0;
  postfix_ItemUnitPrice: number = 0;

  tableConfigurations!: TableConfigurations;

  isContainsName: boolean = true;
  commodityCategoreis: CommodityCategory[] = [];
  SearchForm = new FormGroup({
    commodityTitle: new FormControl(),
    warehouseId: new FormControl(),
    commodityId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  constructor(
    public router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,
  ) {
    super();
    this.pageSize = 300;
  }

  ngOnInit(params?: any): void {
    this.resolve();
  }
  async resolve() {

    await this._mediator.send(new GetCategoriesCodeProductQuery()).then(res => {
      this.commodityCategoreis = res;
    });
    await this._mediator.send(new GetWarehousesLastLevelQuery(0, 0, undefined)).then(async (res) => {

      this.WarehouseNodes = res.data

    })
  }

  commodityGroupIdSelect(item: any) {

    this.SearchForm.controls.commodityTitle.setValue(item?.code)
    this.isContainsName = false;
  }
  async get(_searchQueries: SearchQuery[] = []) {

    this.sumALL = [];
    _searchQueries = this.GetChildFilter(_searchQueries);

    let warehouseIds = ''
    this.filterWarehouseNodes.forEach(a => warehouseIds += ',' + a.id.toString())

    if (warehouseIds == '') {

      this.Service.showHttpFailMessage('انبار انتخاب نمایید');
      return;

    }

    var commodityTitle = this.SearchForm.controls.commodityTitle.value

    if (this.isContainsName == true && commodityTitle) {
      commodityTitle = `%${commodityTitle}%`
    }

    if (this.isContainsName == false && commodityTitle) {
      commodityTitle = `${commodityTitle}%`
    }

    await this._mediator.send(new GetCommodityReportsQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.commodityId.value,
      warehouseIds,
      commodityTitle,
      this.currentPage,
      this.gePageSize(),
      _searchQueries
    )).then(async (res) => {

      if (this.currentPage != 0) {
        this.data = res;
        this.Reports_filter = res;
        this.RowsCount = Number(res[0]?.rowsCount)
        this.CalculateSum(res)
      }
      else {

        this.exportexcel(res);
      }
    })

    if (_searchQueries.length == 0) {
      await this._mediator.send(new GetCommodityReportsSumAllQuery(
        new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
        new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
        this.SearchForm.controls.commodityId.value,
        warehouseIds,
        commodityTitle,
        this.currentPage,
        this.gePageSize(),
        []
      )).then(async (res) => {

        this.sumALL = res


      })
    }


  }
  selectedRows(isSelectedRows: any) {
    this.CalculateSum(this.data, isSelectedRows);
  }
  CalculateSum(data:any[],isSelectedRows : boolean = false) {

    this.postfix_TotalItemPrice = 0;

    this.prefix_TotalItemPrice = 0;
    this.current_Enter_ItemUnitPrice = 0;
    this.current_Exit_TotalItemPrice = 0;
    if(isSelectedRows){
      // postfix_TotalItemPrice postfix_Quantity current_Exit_TotalItemPrice current_Exit_Quantity current_Enter_ItemUnitPrice prefix_TotalItemPrice prefix_Quantity

      this.Service.calculateSumTableOrRows('postfix_Quantity').then(
        res => this.postfix_Quantity = res
      )
      this.Service.calculateSumTableOrRows('current_Exit_TotalItemPrice').then(
        res => this.current_Exit_TotalItemPrice = res
      )
      this.Service.calculateSumTableOrRows('current_Exit_Quantity').then(
        res => this.current_Exit_Quantity = res
      )
      this.Service.calculateSumTableOrRows('current_Enter_TotalItemPrice').then(
        res => this.current_Enter_ItemUnitPrice = res
      )
      this.Service.calculateSumTableOrRows('prefix_TotalItemPrice').then(
        res => this.prefix_TotalItemPrice = res
      )
      this.Service.calculateSumTableOrRows('prefix_Quantity').then(
        res => this.prefix_Quantity = res
      )
      //current_Enter_Quantity
      this.Service.calculateSumTableOrRows('current_Enter_Quantity').then(
        res => this.current_Enter_Quantity = res
      )

      setTimeout(() => {
        this.postfix_TotalItemPrice = this.prefix_TotalItemPrice + this.current_Enter_ItemUnitPrice - this.current_Exit_TotalItemPrice;
      }, 5);


      this.Service.calculateLengthTableOrRows().then(
        res => this.dataLength = res
      )

    } else
    {
      this.prefix_TotalItemPrice = data.reduce((sum, current) => sum + Number(current.prefix_TotalItemPrice), 0);
      this.current_Enter_ItemUnitPrice = data.reduce((sum, current) => sum + Number(current.current_Enter_TotalItemPrice), 0);
      this.current_Exit_TotalItemPrice = data.reduce((sum, current) => sum + Number(current.current_Exit_TotalItemPrice), 0);


      this.prefix_Quantity = data.reduce((sum, current) => sum + Number(current.prefix_Quantity), 0);
      this.current_Enter_Quantity = data.reduce((sum, current) => sum + Number(current.current_Enter_Quantity), 0);
      this.current_Exit_Quantity = data.reduce((sum, current) => sum + Number(current.current_Exit_Quantity), 0);
      this.postfix_Quantity = data.reduce((sum, current) => sum + Number(current.postfix_Quantity), 0);


      this.dataLength = data.length;
     this.postfix_TotalItemPrice = this.prefix_TotalItemPrice + this.current_Enter_ItemUnitPrice - this.current_Exit_TotalItemPrice;
    }
    //  مبلغ نهایی = مبلغ اولیه + مبلغ وارد - مبلغ صادره
    // postfix_TotalItemPrice = prefix_TotalItemPrice + current_Enter_TotalItemPrice - current_Exit_TotalItemPrice

  }
  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);

  }
  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  getWarehousesList(items: any[]) {

    this.filterWarehouseNodes = items;

  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter)
    let warehouseTitles = this.filterWarehouseNodes.map(a => a.title);
    let warehouseList;

    if (warehouseTitles.length > 2) {
      warehouseList = `(${warehouseTitles.slice(0, 2).join(', ')} و...)`;
    } else {
      warehouseList = `(${warehouseTitles.join(', ')})`;
    }
    let title = `گزارش موجودی   ${warehouseList}
    <br />از تاریخ : ${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`;
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }
  filterTable(data: any) {

    this.data = data

  }
  CopyInCommodity(item: any) {
    this.SearchForm.controls.commodityId.setValue(item?.commodityId);
    this.get();
  }


  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data);
  }


}
