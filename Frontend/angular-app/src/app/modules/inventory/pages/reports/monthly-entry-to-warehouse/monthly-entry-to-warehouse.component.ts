import {Component, ElementRef, HostListener, ViewChild} from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import * as XLSX from 'xlsx';
import { GetjaratReportImportQuery } from '../../../repositories/reports/get-tejarat-reports-query';
import { FormControl, FormGroup } from '@angular/forms';
import { spTejaratReportImportCommodity } from '../../../entities/spTejaratReportImportCommodity';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { GetMonthlyEntryToWarehouseQuery } from '../../../repositories/reports/get-monthly-entry-to-warehouse';
import { MonthlyEntryToWarehouse, spGetMonthlyEntryToWarehouse } from '../../../entities/spGetMonthlyEntryToWarehouse';
import { UserYear } from '../../../../identity/repositories/models/user-year';
import { Commodity } from '../../../../accounting/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetFreightPaysQuery} from "../../../repositories/reports/get-freight-pays-query";
import {BaseSetting} from "../../../entities/base";

@Component({
  selector: 'app-monthly-entry-to-warehouse',
  templateUrl: './monthly-entry-to-warehouse.component.html',
  styleUrls: ['./monthly-entry-to-warehouse.component.scss']
})
export class GetMonthlyEntryToWarehouseComponent extends BaseSetting {

  IslargeSize: boolean = false;
  totalEnterResponce: number = 0;
  totalExitResponce: number = 0;
  EnterResponce: spGetMonthlyEntryToWarehouse[] = [];
  ExitResponce: spGetMonthlyEntryToWarehouse[] = [];
  MonthlyEntryToWarehouse: MonthlyEntryToWarehouse[] = [];
  isDisplayFooter: boolean = true;
  public Reports_filter: any[] = [];

  allowedYears: UserYear[] = [];

  SearchForm = new FormGroup({
    warehouseId: new FormControl(),
    commodityId: new FormControl(),
    yearId: new FormControl(),
    enterMode: new FormControl(),

  });
  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,
    public ApiCallService: ApiCallService,
  ) {
    super(route, router);
    identityService._applicationUser.subscribe(res => {
      this.allowedYears = res.years;

      this.SearchForm.patchValue({
        yearId: +res.yearId,

      });
     


    });

  }
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {

    await this.initialize()
  }


  async initialize(entity?: any) {



  }


  async get() {
    if (this.SearchForm.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار انتخاب نمایید');
      return;
    }

    this.EnterResponce = [];
    this.ExitResponce = [];

    await this._mediator.send(new GetMonthlyEntryToWarehouseQuery(
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.warehouseId.value,
      this.SearchForm.controls.yearId.value,
      1
    )).then(async (res) => {
      this.EnterResponce = res;
      this.totalEnterResponce = this.EnterResponce.reduce((sum, current) => sum + Number(current.total), 0);

    })
    await this._mediator.send(new GetMonthlyEntryToWarehouseQuery(
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.warehouseId.value,
      this.SearchForm.controls.yearId.value,
      -1
    )).then(async (res) => {
      this.ExitResponce = res;
      this.totalExitResponce = this.ExitResponce.reduce((sum, current) => sum + Number(current.total), 0);
    })

    if (this.EnterResponce.length > this.ExitResponce.length) {
      this.mergeData(this.EnterResponce)
    }
    else {
      this.mergeData(this.ExitResponce)
    }

  }

  async mergeData(Responce: spGetMonthlyEntryToWarehouse[]) {
    Responce.forEach(a => {
      let enter = this.EnterResponce.find(b => b.commodityCode == a.commodityCode);
      let exit = this.ExitResponce.find(b => b.commodityCode == a.commodityCode);
      this.MonthlyEntryToWarehouse.push(
        {
          commodityCode: a.commodityCode,
          title: a.title,
          enter: enter,
          exit: exit

        });
      this.Reports_filter = this.MonthlyEntryToWarehouse;
    })
  }

  
  async handleYearChange(yearId: number) {
    this.SearchForm.patchValue({
      yearId: yearId,

    });
    
  }
  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async print() {

    let printContents = document.getElementById('report-table-3')?.innerHTML;

    let title = `گزارش گردش کالا ماهیانه`
    if (this.EnterResponce.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }

 reset() {

  }


  async update() {

  }
  delete() {


  }
  async edit() {
  }
  close(): any {
  }
  add(param?: any) {

  }


  filterTable(data: any) {

    this.MonthlyEntryToWarehouse = data
    // this.CalculateSum();
  }
  selectedRows(event: any) {
    // this.CalculateSum(this.response,event);
    console.log('event', event)
    this.isDisplayFooter = ! event ;
  }

  async onGet(searchQueries: SearchQuery[] = []) {


  }
  CalculateSum() {

  }
  exportexcel() {

  }

}
