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
import { GetCommodityReportsQuery } from "../../../../repositories/reports/get-commodity-reports";
import { DomSanitizer } from "@angular/platform-browser";
import { SearchQuery } from '../../../../../../shared/services/search/models/search-query';
import { GetReceiptsCommoditesQuery } from "../../../../repositories/commodity/get-receipt-commodites-query";
import { BaseSetting } from "../../../../entities/base";
import { GetAssetsByDocumentIdQuery } from "../../../../repositories/assets/queries/get-assets-by-documentId";
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { CommoditySerialViewDialog } from "../../../component/commodity-serial-view-dialog/commodity-serial-view-dialog.component";
import { GetCategoriesCodeProductQuery } from "../../../../repositories/commodity-categories/get-commodity-categories-code-product-query";
import { CommodityCategory } from "../../../../../commodity/entities/commodity-category";

@Component({
  selector: 'app-commodity-receipt-reports',
  templateUrl: './commodity-receipt-reports.component.html',
  styleUrls: ['../commodity-receipt-reports.component.scss']
})

export class CommodityReceiptReportsComponent extends BaseSetting {

  current_Enter_Quantity_total: number = 0;
  current_Exit_Quantity_total: number = 0;
  postfix_Quantity_total: number = 0;
  isContainsName: boolean = true;
  // response
  response2: any;

  SearchForm = new FormGroup({

    warehouseId: new FormControl(),
    commodityId: new FormControl(),
    documentNo: new FormControl(),
    requestNo: new FormControl(),
    commodityTitle: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  private warehouseTitle: string = "";
  commodityCategoreis: CommodityCategory[] = [];

  constructor(
    public _mediator: Mediator
    , private router: Router
    , public dialog: MatDialog
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
    await this._mediator.send(new GetCategoriesCodeProductQuery()).then(res => {
      this.commodityCategoreis = res;
    });

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

    if (this.getQueryParam('warehouseId')) {
      this.SearchForm.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));
      this.get();
    }
  }
  //--------------------Filter Table need those methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }
  commodityGroupIdSelect(item: any) {

    this.SearchForm.controls.commodityTitle.setValue(item?.code)
    this.isContainsName = false;
  }
  //--------------------------------------------------
  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);
    if (this.SearchForm.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار انتخاب نمایید');
      return;
    }
    var commodityTitle = this.SearchForm.controls.commodityTitle.value

    if (this.isContainsName == true && commodityTitle) {
      commodityTitle = `%_${commodityTitle}%_`
    }

    if (this.isContainsName==false && commodityTitle) {
      commodityTitle = `${commodityTitle}%_`
    }

    await this._mediator.send(new GetCommodityReciptReportsQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.commodityId.value,
      this.SearchForm.controls.warehouseId.value,
      this.SearchForm.controls.documentNo.value,
      this.SearchForm.controls.requestNo.value,
      commodityTitle,
      this.currentPage,
      this.gePageSize(),
      this.searchQueries,
    )).then(async (res) => {

      if (this.currentPage != 0) {
        this.data = res;
        this.Reports_filter = res;
        this.RowsCount = Number(res[0]?.rowsCount)
        // this.CalculateSum();
      }
      else {

        this.exportexcel(res);
      }


    })


    if (searchQueries.length == 0 && (!this.SearchForm.controls.documentNo.value && !this.SearchForm.controls.requestNo.value)) {

      await this._mediator.send(new GetCommodityReportsQuery(
        new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
        new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
        this.SearchForm.controls.commodityId.value,
        this.SearchForm.controls.warehouseId.value,
        commodityTitle,
        this.currentPage,
        this.gePageSize(),
        searchQueries
      )).then(async (response) => {
        this.response2 = response;

        this.CalculateSum();
      })
    }
    else {
      this.CalculateSum(true);
    }
  }
  selectedRows(event: any) {
    this.CalculateSum(event);
  }

  CalculateSum(isSelectedRows :boolean =false) {

      if (isSelectedRows) {
        //  dataLength
        this.Service.calculateLengthTableOrRows().then(
          res => {
            this.dataLength = res;
          }
        )
        //quantity
        this.Service.calculateSumTableOrRows('current_Enter_Quantity').then(
          res => {
            this.current_Enter_Quantity_total = res;

          }

        )
        //itemUnitPrice
        //totalQuantity
        this.Service.calculateSumTableOrRows('current_Exit_Quantity').then(
          res => {
            this.current_Exit_Quantity_total = res;
          }
        )


      }
      else {
        this.dataLength = this.data.length;
        this.current_Enter_Quantity_total = 0;
        this.current_Exit_Quantity_total = 0;
        this.postfix_Quantity_total = 0;
        this.current_Enter_Quantity_total = this.data.reduce((sum: number, current: any) => sum + Number(current.current_Enter_Quantity), 0);
        this.current_Exit_Quantity_total = this.data.reduce((sum: number, current: any) => sum + Number(current.current_Exit_Quantity), 0);

    }

    this.postfix_Quantity_total = (this.current_Enter_Quantity_total - this.current_Exit_Quantity_total) + Number(this.data[0]?.postfix_Quantity);

  }
  ReferenceSelect(item: any) {
    this.SearchForm.controls.accountReferencesId.setValue(item?.id);

  }
  getCommodityById(item: Commodity) {
    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
    this.warehouseTitle = item.title;
  }
  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter)// document.getElementById('report-table-1')?.innerHTML;
    let warehouseTitle = `( ${ this.warehouseTitle == undefined ? "" : this.warehouseTitle } )`;
    let title = `گزارش گردش کالا انبار ${warehouseTitle}
 <br> از تاریخ :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {
      this.Service.onPrint(printContents, title)
    }
  }


  async navigateToRecive(item: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${item.documentNo}&displayPage=archive&documentStauseBaseValue=${item.documentStauseBaseValue}&documentItemId=${item.documentItemId}&CommodityCode=${item.commodityCode}`)
  }
  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data);

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
  async CommoditySerials(item: any) {

    let dialogConfig = new MatDialogConfig();
    let assetsList: any = undefined;


    let searchQueries: SearchQuery[] = []
    searchQueries.push(new SearchQuery({
      propertyName: "Code",
      values: [item.commodityCode],
      comparison: "equal",
      nextOperand: "and "
    }))
    await this._mediator.send(new GetReceiptsCommoditesQuery(true, this.SearchForm.controls.warehouseId.value, "", 0, 50, searchQueries)).then(res => {

      var commodity = res.data[0];

      this._mediator.send(new GetAssetsByDocumentIdQuery(item.documentItemId, commodity.id)).then(res => {

        assetsList = res

        dialogConfig.data = {
          commodityCode: commodity.code,
          commodityId: commodity.id,
          commodityTitle: commodity.title,
          quantity: item.quantity,
          documentItemId: item.id,
          assets: assetsList

        };


        let dialogReference = this.dialog.open(CommoditySerialViewDialog, dialogConfig);
        dialogReference.afterClosed();

      });
    });



  }
}

