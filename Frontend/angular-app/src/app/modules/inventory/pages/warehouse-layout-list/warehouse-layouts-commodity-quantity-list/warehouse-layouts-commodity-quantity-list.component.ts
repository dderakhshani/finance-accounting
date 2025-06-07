import { Component, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ChangeWarehouseLayoutsDialogComponent } from './warehouse-layouts-change-dialog/warehouse-layouts-change-dialog.component';
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { WarehouseLayoutsCommodityQuantity } from '../../../entities/warehouse-layouts-commodity-quantity';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { GetWarhouseLayoutByCommodityQuantityQuery } from '../../../repositories/warehouse-layout/queries/warhouse-layouts-report/get-warehouse-layouts-commodity-quantity-query';
import { EditWarehouseLayoutsDialogComponent } from './warehouse-layouts-edit-dialog/warehouse-layouts-edit-dialog.component';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { FormControl, FormGroup } from '@angular/forms';
import { Commodity } from '../../../../commodity/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { GetWarehouseLayoutsQuery } from '../../../repositories/warehouse-layout/queries/get-warehouse-layouts-query';
import { WarehouseLayout } from '../../../entities/warehouse-layout';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-warehouse-layouts-commodity-quantity-list',
  templateUrl: './warehouse-layouts-commodity-quantity-list.component.html',
  styleUrls: ['./warehouse-layouts-commodity-quantity-list.component.scss']
})
export class WarehouseLayoutsCommodityQuantityListComponent extends BaseComponent {

  @ViewChild('buttonDetails', { read: TemplateRef }) buttonDetails!: TemplateRef<any>;
  @ViewChild('buttonHistory', { read: TemplateRef }) buttonHistory!: TemplateRef<any>;
  @ViewChild('buttonChangeLayout', { read: TemplateRef }) buttonChangeLayout!: TemplateRef<any>;
  @ViewChild('buttonEditInventory', { read: TemplateRef }) buttonEditInventory!: TemplateRef<any>;


  totalQuantity: number = 0;
  AuditId: number = 0;
  filterWarehouseLayouts: WarehouseLayout[] = [];
  WarehouseLayouts: WarehouseLayoutsCommodityQuantity[] = [];
  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  SearchForm = new FormGroup({
    commodityId: new FormControl(),
    warehouseId: new FormControl(),
    warehouseLayoutId: new FormControl(),
  });
  constructor(

    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    ) {
    super(route, router);
  }

  async ngOnInit() {

  }
  setAuditId (id:number) {
    this.AuditId = id
  }
  async ngAfterViewInit() {

    await this.resolve();

  }

  async resolve() {

    var ChangeLayout = new TableColumn(
      'buttonChangeLayout',
      'تغییر مکان در انبار',
      TableColumnDataType.Template,
      '5%',
    );
    var EditInventory = new TableColumn(
      'buttonEditInventory',
      'اصلاح موجودی',
      TableColumnDataType.Template,
      '5%',
    );
    var Details = new TableColumn(
      'buttonDetails',
      'جزئیات',
      TableColumnDataType.Template,
      '5%',
    );
    var coHistory = new TableColumn(
      'History',
      'تغییرات',
      TableColumnDataType.Template,
      '5%',
    );
    Details.template = this.buttonDetails;
    ChangeLayout.template = this.buttonChangeLayout;
    EditInventory.template = this.buttonEditInventory;
    coHistory.template = this.buttonHistory;

    //----------------------------------------------------------------------
    let columns: TableColumn[] = [

      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'warehouseLayoutTitle',
        'مکان در انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'quantity',
        'موجودی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'warehouseTitle',
        'عنوان انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)

      ),
      ChangeLayout,
      EditInventory,
      Details,
      coHistory

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.initialize();
    await this.get();

  }

 async initialize() {
    if (this.getQueryParam('commodityId') != undefined) {
      this.SearchForm.controls.commodityId.setValue(this.getQueryParam('commodityId'));
    }
    if (this.getQueryParam('warehouseId') != undefined) {
      this.SearchForm.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));
    }
  }

  async get() {

    let searchQueries: SearchQuery[] = []
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
    if (this.SearchForm.controls.warehouseLayoutId.value != undefined && this.SearchForm.controls.warehouseLayoutId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseLayoutId",
        values: [this.SearchForm.controls.warehouseLayoutId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }


    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetWarhouseLayoutByCommodityQuantityQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.WarehouseLayouts = response.data;
    this.totalQuantity = 0;
    this.WarehouseLayouts.forEach(a => this.totalQuantity += Number(a.quantity));
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }
  async navigateToDetails(ware: WarehouseLayoutsCommodityQuantity) {
   

    var url = `inventory/commodityReceiptReports?commodityId=${ware.commodityId}&warehouseId=${ware.warehouseId}`
   
    await this.router.navigateByUrl(url)

  }
  getWarehouseLayoutId(id: number) {

    this.SearchForm.controls.warehouseLayoutId.setValue(id);

  }
  //---------------------تغییر موقعیت انبار-------------------------
  async ChangeLayout(warehouseLayout: WarehouseLayoutsCommodityQuantity) {


    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      warehouseLayout: warehouseLayout,
    };

    let dialogReference = this.dialog.open(ChangeWarehouseLayoutsDialogComponent, dialogConfig);
        dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
        this.get();
    })
  }
  //---------------------تغییر موجودی انبار-------------------------
  async EditEnventory(warehouseLayout: WarehouseLayoutsCommodityQuantity) {


    let dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      warehouseLayout: warehouseLayout,
    };

    let dialogReference = this.dialog.open(EditWarehouseLayoutsDialogComponent, dialogConfig);
      dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
        this.get();
      })
  }
  async print() {

    let printContents = '';
    if (this.WarehouseLayouts.length > 0) {

      printContents += `<table><thead>
                     <tr>

                       <th>عنوان کالا</th>
                       <th>کد کالا</th>
                       <th>مکان در انبار</th>
                       <th>تعداد</th>
                       <th>انبار</th>
                     </tr>
                   </thead><tbody>`;
      this.WarehouseLayouts.map(data => {
        printContents += `<tr>

                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.warehouseLayoutTitle}</td>
                           <td>${data.quantity}</td>

                           <td>${data.warehouseTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'موجودی کالا در انبار')
    }
  }
  async getWarehouseLayout(searchTerm: string) {

    if (this.SearchForm.controls.warehouseId.value == undefined) {
      this._notificationService.showWarningMessage('ابتدا انبار را انتخاب نمایید');
      return;
    }
    let searchQuery = [
      new SearchQuery({
        propertyName: 'warehouseId',
        comparison: 'equal',
        values: [this.SearchForm.controls.warehouseId.value],
        nextOperand: 'and'
      }),
      new SearchQuery({
        propertyName: 'lastLevel',
        comparison: 'equal',
        values: [true],
        nextOperand: 'and'
      }),

    ]
    if (searchTerm != undefined && searchTerm != '') {
      var k = new SearchQuery({
        propertyName: 'title',
        comparison: 'contains',
        values: [searchTerm],
        nextOperand: 'and'
      })
      searchQuery.push(k);

    }

    await this._mediator.send(new GetWarehouseLayoutsQuery(0, 25, searchQuery)).then(res => {
      this.filterWarehouseLayouts = res.data;
    })

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
