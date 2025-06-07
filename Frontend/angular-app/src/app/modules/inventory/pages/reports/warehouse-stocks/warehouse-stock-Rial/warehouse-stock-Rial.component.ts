import { ActivatedRoute, Router } from "@angular/router";
import { Warehouse } from '../../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { Component } from '@angular/core';
import { Commodity } from '../../../../../commodity/entities/commodity';
import { FormAction } from "../../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { DomSanitizer } from "@angular/platform-browser";
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import { GetWarehouseStockQuery } from "../../../../repositories/warehouse-stocks/get-warehouse-layouts-query";
import { BaseSetting } from "../../../../entities/base";

@Component({
  selector: 'app-warehouse-stock-Rial',
  templateUrl: './warehouse-stock-Rial.component.html',
  styleUrls: ['./warehouse-stock-Rial.component.scss']
})
export  class warehouseStockReportsRialComponent extends BaseSetting {


  quantity: number = 0;
  totalPrice: number = 0;
  warehouseTitle: string = '';

  SearchForm = new FormGroup({
    commodityId: new FormControl(),
    warehouseId: new FormControl(),

  });

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

    if (this.getQueryParam('commodityId') != undefined) {
      this.SearchForm.controls.commodityId.setValue(this.getQueryParam('commodityId'));
    }
    if (this.getQueryParam('warehouseId') != undefined) {
      this.SearchForm.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));
      this.get();
    }
  }
  //--------------------Filter Table need those methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }


  //--------------------------------------------------
  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);

    if (this.SearchForm.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار انتخاب نمایید');
      return;
    }
    if (this.SearchForm.controls.commodityId.value != undefined && this.SearchForm.controls.commodityId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityId",
        values: [this.SearchForm.controls.commodityId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    let request = new GetWarehouseStockQuery(this.currentPage, this.gePageSize(), searchQueries, '')
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
  CalculateSum() {


    this.quantity = this.data.reduce((sum, current) => sum + Number(current.quantity), 0);
    this.totalPrice = this.data.reduce((sum, current) => sum + Number(current.price), 0);

  }

  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
    this.warehouseTitle = item?.title;
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);



    var title = ` گزارش ریالی استاک ` + this.warehouseTitle
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data);

  }
}

