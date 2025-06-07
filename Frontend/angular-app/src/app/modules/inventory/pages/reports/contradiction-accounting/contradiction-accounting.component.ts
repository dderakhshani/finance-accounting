import { Component } from '@angular/core';
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
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { BaseSetting } from '../../../entities/base';
import { GetContradictionAccountingQuery } from '../../../repositories/reports/get-contradiction-accounting';
import { Commodity } from '../../../../commodity/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { WarehouseCheckLoseDataCommand } from '../../../repositories/receipt/commands/reciept/warehouse-checkLose-data';
@Component({
  selector: 'app-contradiction-accounting.component',
  templateUrl: './contradiction-accounting.component.html',
  styleUrls: ['./contradiction-accounting.component.scss']
})
export class ContradictionAccountingComponent extends BaseSetting {




  sumInventory: number = 0;
  sumAccounting: number = 0;
  contradiction: number = 0;

  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({
    accountHeadId: new FormControl(),
    warehouseId: new FormControl(),
    commodityId: new FormControl(),
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


  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);

    await this._mediator.send(new GetContradictionAccountingQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.accountHeadId.value,
      this.currentPage,
      this.gePageSize(),
      searchQueries)).then(res => {

          this.data = res;
          this.Reports_filter = res;
          this.CalculateSum();


      })
  }
  selectedRows(event: any) {
    this.CalculateSum(event);
  }
  CalculateSum( isSelectedRows:boolean =false ) {
    this.sumInventory = 0;
    this.contradiction = 0;
    this.sumInventory = 0;

    if (isSelectedRows) {
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
      //totalItemUnitPrice
      this.Service.calculateSumTableOrRows('sumInventory').then(
        res => {
          this.sumInventory = res;

        }
      )
      //sumAll
      this.Service.calculateSumTableOrRows('sumAccounting').then(
        res => {
          this.sumAccounting = res;
        }
      )
      this.Service.calculateSumTableOrRows('contradiction').then(
        res => {
          this.contradiction = res;
        }
      )

    }else {
      this.data.forEach(a => this.sumInventory += Number(a.sumInventory));
      this.data.forEach(a => this.contradiction += Number(a.contradiction));
      this.data.forEach(a => this.sumAccounting += Number(a.sumAccounting));
      this.dataLength = this.data.length;
    }
  }
  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }

  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);
    var title = `گزارش مغایرت های حسابداری:${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, title)
    }
  }

  async updateAvgPrice() {

    let requset = new WarehouseCheckLoseDataCommand();

    this._mediator.send(<WarehouseCheckLoseDataCommand>requset).then(() => {
      this.get()
    });


  }
  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?documentId=${item.documentId}&displayPage=none`)

  }

  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);
    this.SearchForm.controls.accountHeadId.setValue(item?.accountHeadId);

  }
  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }
  async ngOnInit() {

    this.resolve();
  }


  async ngAfterViewInit() {

  }
  async resolve() {


  }


}
