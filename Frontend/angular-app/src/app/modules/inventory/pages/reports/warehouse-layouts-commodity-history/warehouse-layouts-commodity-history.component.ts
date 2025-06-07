import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Commodity } from '../../../../commodity/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { GetCommodityReciptReportsQuery } from '../../../repositories/reports/get-commodity-recipt-reports';

import { BaseSetting } from '../../../entities/base';
import { GetReceiptsCommoditesQuery } from '../../../repositories/commodity/get-receipt-commodites-query';

@Component({
  selector: 'app-warehouse-layouts-commodity-history',
  templateUrl: './warehouse-layouts-commodity-history.component.html',
  styleUrls: ['./warehouse-layouts-commodity-history.component.scss']
})
export class WarehouseLayoutsCommodityHistoryComponent extends BaseSetting {
  @Input() SearchForm!: FormGroup;

  IslargeSize: boolean = false;
  ActivePage: number = 1;
  searchQueries: SearchQuery[] = []

  currentPage: number = 1;

  current_Enter_Quantity: number = 0;
  current_Exit_Quantity: number = 0;
  postfix_Quantity: number = 0;


  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]

  constructor(
    public _mediator: Mediator
    , private router: Router
    , private route: ActivatedRoute
    , private sanitizer: DomSanitizer
    , public Service: PagesCommonService
    , public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnInit() {
    this.resolve();
  }

  async resolve() {

    await this.initialize();
  }

  async initialize() {



    if (this.getQueryParam('commodityId') != undefined && this.getQueryParam('warehouseId') != undefined) {
      this.SearchForm.controls.commodityId.setValue(this.getQueryParam('commodityId'));
      this.SearchForm.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));
    }

    if (this.getQueryParam('fromDate') != undefined) {
      this.SearchForm.controls.fromDate.setValue(this.getQueryParam('fromDate'));
    }
    if (this.getQueryParam('toDate') != undefined) {
      this.SearchForm.controls.toDate.setValue(this.getQueryParam('toDate'));
    }
    this.get();
  }
  //--------------------Filter Table need those methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }

  //--------------------------------------------------
  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);


    await this._mediator.send(new GetCommodityReciptReportsQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.warehouseId.value,
      this.SearchForm.controls.documentNo.value,
      this.SearchForm.controls.requestNo.value,
      this.SearchForm.controls.commodityTitle.value,
      this.currentPage,
      this.gePageSize(),
      this.searchQueries,
    )).then(async (res) => {
      if (this.currentPage != 0) {
        this.data = res;
        this.Reports_filter = res;
        this.RowsCount = Number(res[0]?.rowsCount)
        this.CalculateSum();
      }
      else {

        this.exportexcel(res);
      }
    })

  }
  CalculateSum() {

    this.current_Enter_Quantity = this.data.reduce((sum, current) => sum + Number(current.current_Enter_Quantity), 0);
    this.current_Exit_Quantity = this.data.reduce((sum, current) => sum + Number(current.current_Exit_Quantity), 0);
    this.postfix_Quantity = Number(this.data[this.data.length - 1].postfix_Quantity)

  }

  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);

  }
  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);

    var title = `گزارش سابقه موجودی کالا از تاریخ :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }
  async navigateToRecive(item: any) {
    
    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${item.documentNo}&displayPage=none&documentStauseBaseValue=${item.documentStauseBaseValue}&documentItemId=${item.documentItemId}&CommodityCode=${item.commodityCode}`)
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

