import { Component, Input, TemplateRef, ViewChild } from '@angular/core';
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import { BaseComponent } from '../../../../core/abstraction/base.component';
import { Mediator } from '../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../shared/services/search/models/search-query';
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import { WarehouseStock } from '../../../inventory/entities/warehouse-stock';
import { GetWarehouseStockQuery } from '../../../inventory/repositories/warehouse-stocks/get-warehouse-layouts-query';

@Component({
  selector: 'app-warehouse-stocks-commodity',
  templateUrl: './warehouse-stocks-commodity.component.html',
  styleUrls: ['./warehouse-stocks-commodity.component.scss']
})
export class WarehouseStocksCommodityComponent extends BaseComponent {
  
  WarehouseStocks: WarehouseStock[] = [];
  @Input() commodityId!: number;
  tableConfigurations!: TableConfigurations;


  constructor(
    public _mediator: Mediator,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    ) {
    super();
  }
  async ngOnInit() {
    
  }

  async ngAfterViewInit() {
   await this.resolve()
  }

  async resolve() {

   
    let columns: TableColumn[] = [



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

      )

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    this.tableConfigurations.options.exportOptions.showExportButton = false;
    await this.get();

  }

  initialize() {
  }

  async get() {
    let searchQueries: SearchQuery[] = []
    if (this.commodityId != undefined ) {
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

    let request = new GetWarehouseStockQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.WarehouseStocks = response.data;
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
