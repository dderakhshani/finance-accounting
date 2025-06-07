import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";
import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { BaseComponent } from '../../../../core/abstraction/base.component';
import { Mediator } from '../../../../core/services/mediator/mediator.service';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import { WarehouseLayoutsCommodityQuantity } from '../../../inventory/entities/warehouse-layouts-commodity-quantity';
import { GetWarhouseLayoutByCommodityQuantityQuery } from '../../../inventory/repositories/warehouse-layout/queries/warhouse-layouts-report/get-warehouse-layouts-commodity-quantity-query';

@Component({
  selector: 'app-warehouse-layouts-commodity-quantity',
  templateUrl: './warehouse-layouts-commodity-quantity.component.html',
  styleUrls: ['./warehouse-layouts-commodity-quantity.component.scss']
})
export class WarehouseLayoutsCommodityQuantityComponent extends BaseComponent {
  WarehouseLayouts: WarehouseLayoutsCommodityQuantity[] = [];
  tableConfigurations!: TableConfigurations;
  @Input() commodityId!: number;


  constructor(
    public _mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    public dialog: MatDialog) {
    super(route, router);
  }

  async ngOnInit() {

  }

  async ngAfterViewInit() {

    await this.resolve();

  }

  async resolve() {



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

      )

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    //--------------------------------------------------
    await this.initialize();
    await this.get();

  }

 async initialize() {

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
    if (this.commodityId != undefined) {
      searchQueries.push(new SearchQuery({
        propertyName: "commodityId",
        values: [this.commodityId],
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
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

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
