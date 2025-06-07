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
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Commodity } from '../../../../accounting/entities/commodity';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { BaseSetting } from '../../../entities/base';
import { GetCommodityCostQuery } from '../../../repositories/reports/get-commodity-cost';
import { DeleteAccessToWarehouseCommand } from '../../../repositories/accessToWarehouse/commands/delete-access-to-warehouse-command';
import { GetWarehousesLastLevelQuery } from '../../../repositories/warehouse/queries/get-warehouses-recursives-query';

@Component({
  selector: 'app-commodity-cost',
  templateUrl: './commodity-cost.component.html',
  styleUrls: ['./commodity-cost.component.scss']
})
export class CommodityCostComponent extends BaseSetting {


  WarehouseNodes: any[] = [];
  Warehouses: any[] = [];
  filterWarehouseNodes: any[] = [];

  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });
  ToggleForPrint: boolean = false;
  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnInit() {

    this.resolve();
  }


  async ngAfterViewInit() {

  }
  async resolve() {

    await this._mediator.send(new GetWarehousesLastLevelQuery(0, 0, undefined)).then(async (res) => {

      this.WarehouseNodes = res.data

    })
  }


  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);

    let warehouseIds = ''
    this.filterWarehouseNodes.forEach(a => warehouseIds+=','+a.id.toString())

    await this._mediator.send(new GetCommodityCostQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      warehouseIds,
      this.currentPage,
      this.gePageSize(),
      searchQueries)).then(res => {
        if (this.currentPage != 0) {
          this.data = res.data;
          this.Reports_filter = res.data;
          this.CalculateSum();
          this.RowsCount = res.totalCount;
        }
        else {

          this.exportexcel(res.data);
        }

      })
  }
  CalculateSum() {

  }
  getWarehousesList(items: any[]) {

    this.filterWarehouseNodes = items;
  }
  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }
  async onDelete(param: any, tableName: string) {
    var id =
      await this._mediator.send(new DeleteAccessToWarehouseCommand(param.id, this.form.controls.userId.value, tableName))
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);
    var title = `نرخ کالا  :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, title)
    }
  }

  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }

  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }


}
