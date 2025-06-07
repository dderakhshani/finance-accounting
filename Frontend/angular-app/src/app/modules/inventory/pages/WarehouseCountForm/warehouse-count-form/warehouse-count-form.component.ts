import {AfterViewInit, Component, OnInit} from '@angular/core';
import {BaseTable} from "../../../../../core/abstraction/base-table";
import {ActivatedRoute, Router} from "@angular/router";
import {TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {
  TableScrollingConfigurations
} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {
  GetWarehouseCommodityWithPriceQuery
} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-commodity-withPrice-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {Warehouse} from "../../../entities/warehouse";
import {GetWarehousesQuery} from "../../../repositories/warehouse/queries/get-warehouses-query";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import { WarehouseLayoutCountFormIssuanceDialogComponent} from "../../warehouse-layout/warehouse-layout-count-form-issuance-dialog/warehouse-layout-count-form-issuance-dialog.component";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {MatCheckboxChange} from "@angular/material/checkbox";

@Component({
  selector: 'app-warehouse-count-form',
  templateUrl: './warehouse-count-form.component.html',
  styleUrls: ['./warehouse-count-form.component.scss']
})
export class WarehouseCountFormComponent extends BaseTable implements AfterViewInit {
  selectedOptionValue: number = 0;
  warehouseId: number = 0;
  warehouses: Warehouse[] = [];
  private rowsSelected: any[] = [];
  private warehouseTitle: string | undefined;
  zeroSystemQuantity: boolean = false;

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router,
    private notificationService: NotificationService
  ) {
    super(route, router);
  }

  ngOnInit(): void {
    this.resolve();
  }

  ngAfterViewInit() {

  }

  resolve(params?: any) {
    this.getWarehouses();
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 2,
        type: TableColumnDataType.Select,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
        digitsInfo: DecimalFormat.Default,

      },
      {
        ...this.defaultColumnSettings,
        index: 1,
        field: 'rowIndex',
        title: 'ردیف',
        width: 1,
        type: TableColumnDataType.Index,
        isDisableDrop: true,
        lineStyle: 'onlyShowFirstLine',
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'commodityCompactCode',
        title: 'کد کوتاه کالا',
        width: 2,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityCompactCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        style:{
          'text-align': 'left !important'
        }
      }, {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'commodityCode',
        title: 'کد کالا',
        width: 3,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        style:{
          'text-align': 'left !important'
        }
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'commodityName',
        title: 'نام کالا',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityName', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'warehouseLayoutTitle',
        title: 'محل انبار',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'systemQuantity',
        title: 'تعداد سیستمی',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.TwoDecimals,
        filter: new TableColumnFilter('systemQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        print: false
      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'measureTitle',
        title: 'واحد',
        width: 2,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'price',
        title: 'قیمت',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Rounded,
        filter: new TableColumnFilter('price', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        print: false
      }
    ];
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش انبار'));
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.toolBar.showTools.includeOnlySelectedItemsLocal = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'price';
    this.tableConfigurations.options.defaultSortDirection = 'DESC';
    this.tableConfigurations.sortKeys = ['price DESC'];
    this.tableConfigurations.pagination.pageSize = 50000;
  }

  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  get(param?: any) {
    let searchQueries: SearchQuery[] = JSON.parse(JSON.stringify(this.excludedRows));
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
    if (this.filterConditionsInputSearch) {
      Object.keys(this.filterConditionsInputSearch).forEach(key => {
        const filter = this.filterConditionsInputSearch[key];
        if (filter && filter.propertyNames && filter.searchValues && filter.searchValues[0]) {
          filter.propertyNames.forEach((propertyName: string) => {
            searchQueries.push(new SearchQuery({
              propertyName: propertyName,
              values: filter.searchValues,
              comparison: filter.searchCondition,
              nextOperand: filter.nextOperand
            }));
          });
        }
      });
    }
if(!this.zeroSystemQuantity){
  searchQueries.push(new SearchQuery({
    propertyName: 'systemQuantity',
    values: [0],
    comparison: 'notEqual',
    nextOperand: 'and'
  }));
}
else
  searchQueries=searchQueries.filter(x=>x.propertyName!='systemQuantity');

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetWarehouseCommodityWithPriceQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    if (this.warehouseId)
      request.warehouseId = this.warehouseId;

    this.tableConfigurations.options.isLoadingTable = true;
    this._mediator.send(request).then(response => {
      this.rowDataClone = response.data;
      this.rowData = this.rowDataClone.filter((x => x.systemQuantity != 0))
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
  }

  update(param?: any) {
    throw new Error('Method not implemented.');
  }

  delete(param?: any) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }

  updateInputSearchFilters(filterConditions: { [key: string]: any }) {
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    this.get()

  }

  handleOptionSelected(event: { typeFilterOptions: any, query: any }) {
    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {

    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      this.updateInputSearchFilters(event.query)
    }
  }

  handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {
    this.tableConfigurations.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.requestsList = this.requestsList.slice(0, 1);
    this.requestsIndex = -1;
    this.filterConditionsInputSearch = {};
    this.get()
  }

  getWarehouses(value?: any) {
    let conditions: SearchQuery[] = []
    if (value) {
      conditions.push(new SearchQuery({
        propertyName: 'title',
        values: [value],
        comparison: 'contain',
        nextOperand: 'and'
      }))
    }
    this._mediator.send(new GetWarehousesQuery(0, 0, conditions)).then(res => {
      this.warehouses = res.data.filter(x => x.countable);
    })
  }

  async handleWarehouseChange(id: number) {
    this.warehouseId = id;
    this.get();
    this.warehouseTitle = this.warehouses.find(x => x.id === id)?.title
    this.tableConfigurations.printOptions.reportTitle = `گزارش ` + this.warehouseTitle;
  }

  addNewCountForm() {
    // @ts-ignore
    let warehouseLayoutId: number = 0;
    if (this.rowsSelected.length == 0)
      return this.notificationService.showFailureMessage("جهت صدور فرم شمارش اقلام مورد نظر را انتخاب نمایید.");
    warehouseLayoutId = this.rowsSelected.reduce((min, item) => (item.warehouseLayoutId < min.value ? item : min), this.rowsSelected[0].warehouseLayoutId);

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      selectedCommodity: this.rowData.filter(x => x.selected),
      formStatus: 0,
      warehouseLayoutId: warehouseLayoutId,
      basedOnLocation:false,
      warehouseId: this.warehouseId,
    };
    this.dialog.open(WarehouseLayoutCountFormIssuanceDialogComponent, dialogConfig);
  }

  handelRowsSelected(event: any) {
    this.rowsSelected = event
  }


  handleZeroSystemQuantity($event: MatCheckboxChange) {
    this.zeroSystemQuantity = $event.checked;
    this.get();

  }
}
