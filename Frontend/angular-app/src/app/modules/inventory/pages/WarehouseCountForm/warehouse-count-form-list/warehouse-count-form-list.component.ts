import {Component, TemplateRef, ViewChild} from '@angular/core';
import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {TableScrollingConfigurations} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {WarehouseCountFormHead} from "../../../entities/warehouse-count-form-head";
import {
  GetWarehouseCountFormQuery
} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-query";
import {Tab} from "../../../../../layouts/main-container/models/tab";
import {Subscription} from "rxjs";
import {BaseTable} from "../../../../../core/abstraction/base-table";

@Component({
  selector: 'app-warehouse-count-form-list',
  templateUrl: './warehouse-count-form-list.component.html',
  styleUrls: ['./warehouse-count-form-list.component.scss']
})
export class WarehouseCountFormListComponent extends BaseTable {
  @ViewChild('rowBtn1', {read: TemplateRef}) rowBtn1!: TemplateRef<any>;
  @ViewChild('expandRowWithTemplate', {read: TemplateRef}) expandRowWithTemplate!: TemplateRef<any>;
  WarehouseCountForms: WarehouseCountFormHead[] = [];
  selectWarehouseCountForm: WarehouseCountFormHead[] = [];

  subscriptionChangTab$!: Subscription;

  constructor(
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog,
  ) {
    super(route, router);
  }

  ngAfterViewInit() {
    const column = this.tableConfigurations.columns.find((col: any) => col.field === 'btn');
    if (column) {
      column.template = this.rowBtn1
    }

    const columnEexpand = this.tableConfigurations.columns.find((col: any) => col.field === 'children');
    if (columnEexpand) {
      columnEexpand.expandRowWithTemplate = this.expandRowWithTemplate;
    }
  }

  async ngOnInit() {
    await this.resolve()
  }

  ngOnDestroy() {
    super.ngOnDestroy();
    this.subscriptionChangTab$.unsubscribe();
  }

  async resolve() {
    this.subscriptionChangTab$ = this.tabManagerService.tabChanged.subscribe(async (tab: Tab) => {
      if (tab.guid == this.componentId) {
        await this.handleRemoveAllFiltersAndSorts(this.tableConfigurations)
      }
    })
    this.columns = [

      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'children',
        title: '',
        width: 1,
        type: TableColumnDataType.ExpandRowWithTemplate,
        display: true,
        lineStyle: 'onlyShowFirstLine',
        expandRowWithTemplate: this.expandRowWithTemplate
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
        field: 'formNo',
        title: 'شماره فرم',
        width: 3,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None,
        filter: new TableColumnFilter('formNo', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },

      {
        ...this.defaultColumnSettings,
        index: 3,
        field: 'formDate',
        title: 'تاریخ صدور فرم',
        width: 3,
        type: TableColumnDataType.Date,
        filter: new TableColumnFilter('formDate', TableColumnFilterTypes.Date),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
        field: 'warehouseLayoutTitle',
        title: 'نام انبار',
        width: 3.5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('warehouseLayoutTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,

      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'counterUserName',
        title: 'کاربر شمارنده',
        width: 3,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('counterUserName', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'confirmerUserName',
        title: 'کاربر تایید کننده',
        width: 3,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('confirmerUserName', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'formStateMessage',
        title: 'وضعیت فرم',
        width: 3,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('formStateMessage', TableColumnFilterTypes.Text),
        lineStyle: 'default',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'btn',
        title: 'عملیات',
        width: 2,
        type: TableColumnDataType.Template,
        template: this.rowBtn1,
        sortable: false,
        lineStyle: 'default',
        print:false

      }
    ];
    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar)
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.exportOptions.showExportButton = true;
    this.tableConfigurations.options.hasDefaultSortKey = true;
    this.tableConfigurations.options.defaultSortKey = 'formNo';
    this.tableConfigurations.options.defaultSortDirection = 'ASC';

    await this.get();
  }

  initialize() {
  }

  async get() {
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
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetWarehouseCountFormQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    this.requestsIndex++;
    this.tableConfigurations.options.isLoadingTable = true;
    await this._mediator.send(request).then(response => {

      this.WarehouseCountForms = this.groupData(response.data);

      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
  }

  groupData(data: any): any[] {
    const map = new Map();
    const result: any[] = [];

    // First pass: Create a map of all items
    data.forEach((item: any) => {
      map.set(item.id, {...item, children: []});
    });

    data.forEach((item: any) => {
      if (item.parentId) {
        const parent = map.get(item.parentId);
        if (parent) {
          parent.children.push(map.get(item.id));
        }
      } else {
        result.push(map.get(item.id));
      }
    });
    return result
  }

  async update() {
  }

  async add() {
  }

  close(): any {
  }

  delete(): any {
  }

  async handleExcludeSelectedItemsEvent(ids: number[]) {
    this.excludedRows = [new SearchQuery({
      propertyName: 'id',
      values: ids,
      comparison: 'notIn',
      nextOperand: 'and'
    })];
    await this.get();
  }

  handleRowsSelected(selectWarehouseCountForm: any) {
    if (!this.selectWarehouseCountForm) {
      this.selectWarehouseCountForm = [];
      return
    }
    this.selectWarehouseCountForm = selectWarehouseCountForm;
  }

  handleOptionSelected(event: { typeFilterOptions: any, query: any }) {
    this.tableConfigurations.pagination.pageIndex = 0;
    if (event.typeFilterOptions == TypeFilterOptions.NgSelect) {

    }
    if (event.typeFilterOptions == TypeFilterOptions.NumberInputSearch || event.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
      this.updateInputSearchFilters(event.query)
    }
  }

  updateInputSearchFilters(filterConditions: { [key: string]: any }) {
    this.filterConditionsInputSearch = {...this.filterConditionsInputSearch, ...filterConditions};
    this.get()
  }

  async handleRemoveAllFiltersAndSorts(config: TableScrollingConfigurations) {

    this.tableConfigurations.columns = config.columns;
    this.tableConfigurations.options = config.options;
    this.selectedItemsFilterForPrint = [];
    this.excludedRows = []
    this.requestsList = this.requestsList.slice(0, 1);
    this.requestsIndex = -1;
    this.filterConditionsInputSearch = {};
    await this.get()
  }

  showDetail(selectWarehouseCountForm?: any) {
    return this.router.navigateByUrl(`/inventory/warehouseCountFormDetail?headerId=${+selectWarehouseCountForm.id}`);
  }

  showConflict(selectWarehouseCountForm?: any) {
    return this.router.navigateByUrl(`/inventory/warehouseCountFormConflict?headerId=${+selectWarehouseCountForm.id}`);
  }

  showReport(row: any) {
    return this.router.navigateByUrl(`/inventory/warehouseCountReport?headerId=${+row.id}`);
  }

  protected readonly length = length;
}
