import { Component, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";

import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';

import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { WarehouseStock } from '../../../entities/warehouse-stock';
import { GetWarehouseStockQuery } from '../../../repositories/warehouse-stocks/get-warehouse-layouts-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { Commodity } from '../../../../commodity/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import { AttachmentsModel, UploadFileData } from '../../../../../core/components/custom/uploader/uploader.component';

import { HttpEvent, HttpEventType } from '@angular/common/http';
import { environment } from '../../../../../../environments/environment';
import { UploadFileService } from '../../../../../core/services/upload-file.service';

@Component({
  selector: 'app-warehouse-stocks-available-commodity',
  templateUrl: './warehouse-stocks-available-commodity.component.html',
  styleUrls: ['./warehouse-stocks-available-commodity.component.scss']
})
export class WarehouseStocksAvailableCommodityComponent extends BaseComponent {
  @ViewChild('buttonDetails', { read: TemplateRef }) buttonDetails!: TemplateRef<any>;
  @ViewChild('txtcommodityCode', { read: TemplateRef }) txtcommodityCode!: TemplateRef<any>;
  WarehouseStocks: WarehouseStock[] = [];

  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  SearchForm = new FormGroup({
    commodityId: new FormControl(),
    warehouseId: new FormControl(),

  });
  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    private uploadFileService: UploadFileService,

    public _notificationService: NotificationService,
    ) {
    super(route, router);
  }
  async ngOnInit() {

  }

  async ngAfterViewInit() {
   await this.resolve()
  }

  async resolve() {

    var Details = new TableColumn(
      'buttonDetails',
      'جزئیات',
      TableColumnDataType.Template,
      '5%',
    );
    let colcommodityCode = new TableColumn(
      'commodityCode',
      'کد کالا',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
    );
    Details.template = this.buttonDetails;
    colcommodityCode.template = this.txtcommodityCode;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [

      colcommodityCode,
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)

      ),

      new TableColumn(
        'quantity',
        'تعداد اصلی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'reservedQuantity',
        'تعداد در حال رزرو',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('reservedQuantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'availableQuantity',
        'تعداد موجود',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('availableQuantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'warehouseTitle',
        'عنوان انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)

      ),
      Details

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();

  }


  initialize() {
  }

  async get() {
    let searchQueries: SearchQuery[] = []

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
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetWarehouseStockQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.WarehouseStocks = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  async print() {

    let printContents = '';
    if (this.WarehouseStocks.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>کد کالا</th>
                       <th>عنوان کالا</th>
                       <th>تعداد اصلی</th>
                       <th>تعداد در حال رزرو</th>
                       <th>تعداد موجود</th>
                       <th>انبار</th>
                     </tr>
                   </thead><tbody>`;
      this.WarehouseStocks.map(data => {
        printContents += `<tr>

                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.quantity}</td>
                           <td>${data.reservedQuantity}</td>
                           <td>${data.availableQuantity}</td>
                           <td>${data.warehouseTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'موجودی کلی کالا')
    }
  }
  getCommodityById(item: Commodity) {


    this.SearchForm.controls.commodityId.setValue(item?.id);
  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async navigateToDetails(ware: WarehouseStock) {
    await this.router.navigateByUrl(`inventory/warehouseLayoutsCommodityQuantityList?commodityId=${ware.commodityId}&warehouseId=${ware.warehouseId}`)

  }
  async navigateToHistory(ware: WarehouseStock) {

    var url = `inventory/commodityReceiptReports?commodityId=${ware.commodityId}&warehouseId=${ware.warehouseId}`
    this.router.navigateByUrl(url)

  }
  async update() {
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }


}
