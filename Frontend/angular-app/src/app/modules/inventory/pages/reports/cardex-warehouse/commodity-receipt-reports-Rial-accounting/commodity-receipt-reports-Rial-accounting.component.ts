import { ActivatedRoute, Router } from "@angular/router";
import { Warehouse } from '../../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { Component, ViewChild } from '@angular/core';
import { Commodity } from '../../../../../commodity/entities/commodity';
import { FormAction } from "../../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { GetCommodityReciptReportsQuery } from "../../../../repositories/reports/get-commodity-recipt-reports";
import { DomSanitizer } from "@angular/platform-browser";
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import { GetReceiptsCommoditesQuery } from "../../../../repositories/commodity/get-receipt-commodites-query";
import { BaseSetting } from "../../../../entities/base";
import { GetCommodityReportsQuery } from "../../../../repositories/reports/get-commodity-reports";
import { GetCommodityReceiptReportsRialQuery } from "../../../../repositories/reports/get-commodity-recipt-reports-accounting";
import { GetCommodityReportsRialQuery } from "../../../../repositories/reports/get-commodity-reports-rial";
@Component({
  selector: 'app-commodity-receipt-reports-Rial-accounting.component',
  templateUrl: './commodity-receipt-reports-Rial-accounting.component.html',
  styleUrls: ['./commodity-receipt-reports-Rial-accounting.component.scss']
})

export class CommodityReceiptReportsRialAccountingComponent extends BaseSetting {

  prefix_TotalItemPrice: number = 0;
  current_Enter_TotalItemPrice: number = 0;
  current_Exit_TotalItemPrice: number = 0;
  postfix_TotalItemPrice: number = 0;

  prefix_Quantity: number = 0;
  current_Enter_Quantity: number = 0;
  current_Exit_Quantity: number = 0;
  postfix_Quantity: number = 0;
  postfix_ItemUnitPrice: number = 0;

  response :  any;


  SearchForm = new FormGroup({

    accountHeadId: new FormControl(),
    commodityId: new FormControl(),
    warehouseId: new FormControl(),
    documentNo: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  private warehouseTitle!: string  ;

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
    this.pageSize = 300;
    await this.initialize();

  }

  async initialize() {

    if (this.getQueryParam('commodityId') != undefined) {
      this.SearchForm.controls.commodityId.setValue(this.getQueryParam('commodityId'));
    }

    if (this.getQueryParam('fromDate')) {

      let getdate: string = this.getQueryParam('fromDate').replace('%2F', '/').replace('%2F', '/')

      let fromDate: Date = new Date(getdate)
      this.SearchForm.controls.fromDate.setValue(fromDate);

    }

    if (this.getQueryParam('toDate')) {

      let getdate: string = this.getQueryParam('toDate').replace('%2F', '/').replace('%2F', '/')
      let toDate: Date = new Date(getdate)


      this.SearchForm.controls.toDate.setValue(toDate);
    }
    if (this.getQueryParam('accountHeadId') != undefined) {
      this.SearchForm.controls.accountHeadId.setValue(this.getQueryParam('accountHeadId'));

    }
    if (this.getQueryParam('warehouseId') != undefined) {
      this.SearchForm.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));

    }
    this.get();
  }
  //--------------------Filter Table need those methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum(data);
  }

  //--------------------------------------------------
  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);

    if (this.SearchForm.controls['accountHeadId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار انتخاب نمایید');
      return;
    }

    await this._mediator.send(new GetCommodityReceiptReportsRialQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.accountHeadId.value,
      this.SearchForm.controls.warehouseId.value,
      this.SearchForm.controls.documentNo.value,
      
      this.currentPage,
      this.gePageSize(),
      this.searchQueries,
    )).then(async (res) => {

      if (this.currentPage != 0) {
        this.data = res;
        this.Reports_filter = res;

        this.RowsCount = Number(res[0]?.rowsCount)

      }
      else {

        this.exportexcel(res);
      }


    })

    if (searchQueries.length == 0) {
      await this._mediator.send(new GetCommodityReportsRialQuery(
        new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
        new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
        this.SearchForm.controls.commodityId.value,
        this.SearchForm.controls.accountHeadId.value,
        this.currentPage,
        this.gePageSize(),
        undefined
      )).then(async (response) => {
        this.response = response;
        this.CalculateSum(this.response);


      })
    }
    else {
      this.CalculateSum(this.response,true);
    }



  }
  selectedRows(event: any) {
    this.CalculateSum(this.response,event);
  }
  CalculateSum(response:any[],isSelectedRows:boolean = false) {


    if (isSelectedRows) {
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
      //prefix_TotalItemPrice
      this.Service.calculateSumTableOrRows('prefix_TotalItemPrice').then(
        res => {
          this.prefix_TotalItemPrice = res;

        }
      )
      //current_Enter_TotalItemPrice
      this.Service.calculateSumTableOrRows('current_Enter_TotalItemPrice').then(
        res => {
          this.current_Enter_TotalItemPrice = res;

        }
      )
      //current_Exit_TotalItemPrice
      this.Service.calculateSumTableOrRows('current_Exit_TotalItemPrice').then(
        res => {
          this.current_Exit_TotalItemPrice = res;

        }
      )
      //postfix_TotalItemPrice
      this.Service.calculateSumTableOrRows('postfix_TotalItemPrice').then(
        res => {
          this.postfix_TotalItemPrice = res;

        }
      )
      //prefix_Quantity
      this.Service.calculateSumTableOrRows('prefix_Quantity').then(
        res => {
          this.prefix_Quantity = res;

        }
      )
      //current_Enter_Quantity
      this.Service.calculateSumTableOrRows('current_Enter_Quantity').then(
        res => {
          this.current_Enter_Quantity = res;

        }
      )
      //current_Exit_Quantity
      this.Service.calculateSumTableOrRows('current_Exit_Quantity').then(
        res => {
          this.current_Exit_Quantity = res;

        }
      )
      //postfix_Quantity
      this.Service.calculateSumTableOrRows('postfix_Quantity').then(
        res => {
          this.postfix_Quantity = res;

        }
      )
      this.Service.calculateSumTableOrRows('postfix_Quantity','postfix_ItemUnitPrice').then(
        res => {
          this.postfix_TotalItemPrice = res;

        }
      )


    }
    else {
      this.prefix_TotalItemPrice = response.reduce((sum, current) => sum + Number(current.prefix_TotalItemPrice), 0);
      this.current_Enter_TotalItemPrice = response.reduce((sum, current) => sum + Number(current.current_Enter_TotalItemPrice), 0);
      this.current_Exit_TotalItemPrice = response.reduce((sum, current) => sum + Number(current.current_Exit_TotalItemPrice), 0);
      this.postfix_TotalItemPrice = response.reduce((sum, current) => sum + Number(current.postfix_Quantity) * Number(current.postfix_ItemUnitPrice), 0);

      this.prefix_Quantity = response.reduce((sum, current) => sum + Number(current.prefix_Quantity), 0);
      this.current_Enter_Quantity = response.reduce((sum, current) => sum + Number(current.current_Enter_Quantity), 0);
      this.current_Exit_Quantity = response.reduce((sum, current) => sum + Number(current.current_Exit_Quantity), 0);
      this.postfix_Quantity = response.reduce((sum, current) => sum + Number(current.postfix_Quantity), 0);
      this.dataLength = response.length;
    }
  }
  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);

  }
  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.accountHeadId.setValue(item?.accountHeadId);
    this.SearchForm.controls.warehouseId.setValue(item?.id);

    this.warehouseTitle = item?.title;
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter)
    let warehouseTitle = `( ${ this.warehouseTitle == undefined ? "" : this.warehouseTitle } )`;

    let title = ` گزارش ریالی گردش کالا انبار  ${warehouseTitle}
      <br> از تاریخ :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }

  async exportexcel(data :any[]) {

    this.Service.onExportToExcel(data)
  }


  async navigateToRecive(item: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${item.documentNo}&displayPage=none&documentStauseBaseValue=${item.documentStauseBaseValue}`)
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

