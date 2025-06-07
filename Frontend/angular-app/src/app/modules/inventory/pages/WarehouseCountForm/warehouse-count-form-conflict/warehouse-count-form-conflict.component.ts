import { Component, OnInit } from '@angular/core';
import { TableScrollingConfigurations} from "../../../../../core/components/custom/table/models/table-scrolling-configurations";
import {Column, TypeFilterOptions} from "../../../../../core/components/custom/table/models/column";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {NotificationService} from "../../../../../shared/services/notification/notification.service";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {DecimalFormat} from "../../../../../core/components/custom/table/models/decimal-format";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TablePaginationOptions} from "../../../../../core/components/custom/table/models/table-pagination-options";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {WarehouseCountFormConflict} from "../../../entities/warehouse-count-form-conflict";
import { GetWarehouseCountFormConflictQuery} from "../../../repositories/warehouse-count-form/quereis/get-warehouse-count-form-conflict-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {WarehouseLayoutCountFormIssuanceDialogComponent} from "../../warehouse-layout/warehouse-layout-count-form-issuance-dialog/warehouse-layout-count-form-issuance-dialog.component";
import {UpdateStateWarehouseCountFormCommand} from "../../../repositories/warehouse-count-form/commands/update-state-warehouse-count-form";
import { GetChildWarehouseCountFormHeadQuery} from "../../../repositories/warehouse-count-form/quereis/get-child-warehouse-count-form-head-query";
import {PrintOptions} from "../../../../../core/components/custom/table/models/print_options";
import {BaseTable} from "../../../../../core/abstraction/base-table";

@Component({
  selector: 'app-warehouse-count-form-conflict',
  templateUrl: './warehouse-count-form-conflict.component.html',
  styleUrls: ['./warehouse-count-form-conflict.component.scss']
})
export class WarehouseCountFormConflictComponent extends BaseTable {
  outPrint: boolean = false;
  formNo!: string;
  formDate!: string;
  confirmerUser!: string;
  confirmUserId!: number;
  countUser!: string;
  warehouseLayoutTitle!: string;
  warehouseCountFormStatus!: string;
  warehouseCountFormConflict: WarehouseCountFormConflict[] = [];
  parentId: number = 0;
  warehouseLayoutId: number = 0;
  formState!: number;
  headerId!: number;
  rowsSelected:any[]=[];
  warehouseId:number=0;
  constructor(
    private _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    private notificationService: NotificationService
  ) {
    super(route, router);
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add().setTitle('صدور فرم شمارش مرحله بعد'),//.setDisable(this.formState>2),
    ];
  }

  ngOnInit(params?: any): void {
    this.headerId = this.getQueryParam('headerId')
    this.resolve();
    this.getHeader();
    if (this.headerId) {
      this.get(+this.headerId)
    } else {
      this.get()
    }
  }

  resolve(params?: any) {
    this.columns = [
      {
        ...this.defaultColumnSettings,
        index: 0,
        field: 'selected',
        title: '#',
        width: 1,
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
        index: 1,
        field: 'id',
        title: 'شماره',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.None ,
        filter: new TableColumnFilter('id', TableColumnFilterTypes.Number),
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        lineStyle: 'onlyShowFirstLine',
        display:false
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
        field: 'commodityCompactCode',
        title: 'کد کوتاه کالا',
        width: 3,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityCompactCode', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
        style:{
          'text-align': 'left !important'
        }
      },
      {
        ...this.defaultColumnSettings,
        index: 2,
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
        index: 3,
        field: 'commodityName',
        title: 'نام کالا',
        width: 5,
        type: TableColumnDataType.Text,
        filter: new TableColumnFilter('commodityName', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 4,
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
        index: 5,
        field: 'systemQuantity',
        title: 'موجودی سیستمی ',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('systemQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        print:false
      },
      {
        ...this.defaultColumnSettings,
        index: 5,
        field: 'measureTitle',
        title: 'واحد',
        width: 1.5,
        type: TableColumnDataType.Text,
        digitsInfo: DecimalFormat.TwoDecimals,
        filter: new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 6,
        field: 'countedQuantity',
        title: 'موجودی شمارش شده',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('countedQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
      },
      {
        ...this.defaultColumnSettings,
        index: 7,
        field: 'conflictQuantity',
        title: 'مغایرت',
        width: 2,
        type: TableColumnDataType.Number,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('conflictQuantity', TableColumnFilterTypes.Number),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.NumberInputSearch,
        displayFn:(x:any)=>{
          return x.countedQuantity -  x.systemQuantity
        },

      },
      {
        ...this.defaultColumnSettings,
        index: 8,
        field: 'description',
        title: 'توضیحات',
        width: 3.5,
        type: TableColumnDataType.Text,
        digitsInfo: DecimalFormat.Default,
        filter: new TableColumnFilter('description', TableColumnFilterTypes.Text),
        lineStyle: 'onlyShowFirstLine',
        typeFilterOptions: TypeFilterOptions.TextInputSearch,
      },
    ];

    this.tableConfigurations = new TableScrollingConfigurations(this.columns, new TableOptions(false, true), new TablePaginationOptions(), this.toolBar, new PrintOptions('گزارش کسری اضافی انبار'));
    this.tableConfigurations.options.showTopSettingMenu = true;
    this.tableConfigurations.options.usePagination = true;
    this.tableConfigurations.options.showFilterRow = true;
    this.tableConfigurations.printOptions.hasCustomizeHeaderPage = true;
    this.selectedItemsFilterForPrint.hasCustomizeHeaderPage = true;
  }


  initialize(params?: any) {
    throw new Error('Method not implemented.');
  }

  add(param?: any) {
    throw new Error('Method not implemented.');
  }

  addNewCountForm() {
    if (this.rowsSelected.length==0)
    return this.notificationService.showFailureMessage("جهت صدور فرم شمارش اقلام مورد نظر را انتخاب نمایید.");

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      warehouseLayoutId: this.warehouseLayoutId,
      pageMode: PageModes.Add,
      parentId: this.parentId,
      formStatus: 0,
      selectedCommodity:this.rowsSelected,
      warehouseId:this.warehouseId
    };
   this.dialog.open(WarehouseLayoutCountFormIssuanceDialogComponent, dialogConfig);
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
    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }
    let request = new GetWarehouseCountFormConflictQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    if (this.headerId)
      request.warehouseCountFormHeadId = this.headerId;

    request.pageSize = this.tableConfigurations.pagination.pageSize;
    request.pageIndex = this.tableConfigurations.pagination.pageIndex;

    this.tableConfigurations.options.isLoadingTable = true;
    this._mediator.send(request).then(response => {
      this.warehouseCountFormConflict = response.data;
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
      response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
      this.tableConfigurations.options.isLoadingTable = false;

    }).catch(() => {
      this.tableConfigurations.options.isLoadingTable = false;
    });
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

  getHeader() {
    let warehouseCountFormHead = new GetChildWarehouseCountFormHeadQuery()
    if (this.headerId)
      warehouseCountFormHead.parentId = this.headerId;
    this._mediator.send(warehouseCountFormHead).then(response => {
      if (response.length == 1) {
        let res = response[0];
        this.parentId = res.id;
        this.warehouseLayoutId = res.warehouseLayoutId;
        this.formState = res.formState;
        this.formNo = res.formNo;
        this.confirmerUser = res.confirmerUserName;
        this.confirmUserId = res.confirmerUserId;
        this.countUser = res.counterUserName;
        this.formDate = res.formDate;
        this.warehouseLayoutTitle = res.warehouseLayoutTitle;
        this.warehouseCountFormStatus = res.formStateMessage;
        this.warehouseId=res.warehouseId;
        this.actionBar.actions[0].setDisable(response[0].formState > 2);
        this.actionBar.actions[1].setDisable(response[0].formState > 3);
      }
      else {
        let maxId = response.length > 0 ? Math.max(...response.map((item: any) => item.id)) : null;
        let lastChild = response.find((x: any) => x.id == maxId);
        this.parentId = lastChild.parentId;
        this.warehouseLayoutId = lastChild.warehouseLayoutId;
        this.formNo = lastChild.formNo;
        this.confirmerUser = lastChild.confirmerUserName;
        this.confirmUserId = lastChild.confirmerUserId;
        this.countUser = lastChild.counterUserName;
        this.formDate = lastChild.formDate;
        this.warehouseLayoutTitle = lastChild.warehouseLayoutTitle;
        this.warehouseCountFormStatus = lastChild.formStateMessage;
        this.formState = lastChild.formState;
        this.warehouseId=lastChild.warehouseId;
        this.actionBar.actions[0].setDisable(true);
      }
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

  handelRowsSelected(event: any) {
    this.rowsSelected = event
  }
}

