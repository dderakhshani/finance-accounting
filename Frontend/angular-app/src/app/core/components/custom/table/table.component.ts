import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter, HostListener,
  Input,
  OnInit,
  Output,
  Renderer2,
  ViewChild
} from '@angular/core';
import {TableColumnFilterOperands, TableConfigurations} from "./models/table-configurations";
import {AbstractControl, FormArray, FormControl, FormGroup} from "@angular/forms";
import * as XLSX from "xlsx";
import * as moment from "jalali-moment";
import {TableColumnDataType} from "./models/table-column-data-type";
import {TableColumnFilterTypes} from "./models/table-column-filter-types";
import {TableColumn} from "./models/table-column";
import {SearchComparisonTypes} from "../../../../shared/services/search/models/search-query";
import {MatMenuTrigger} from "@angular/material/menu";
import {MatSort} from "@angular/material/sort";
import {Mediator} from 'src/app/core/services/mediator/mediator.service';
import {Router} from "@angular/router";


@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss'],
})
export class TableComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort) sort!: MatSort;

  shownRows: any = []
  datasource!: any;


  @Input() public height: string = '100%';

  // Metadata
  fieldTypes = TableColumnDataType;
  selectAllControl = new FormControl();
  tableColumnFilterTypes = TableColumnFilterTypes;
  @Input() areRowsSimpleArray: boolean | undefined = undefined;
  @Input() request: any | undefined = undefined;
  rowsNeedSorting = false;
  start: number = 0;
  pageSize: number = 100;
  end: number = this.pageSize + this.start;
  isLoading: boolean = false;
  isSelectedList : boolean = false
  sendRequest = true;
  excludedItems!: any[] | AbstractControl[];
  includedItems!: any[] | AbstractControl[];
  realTotalItemsCount = 0;


  constructor(
    private mediator: Mediator,
  ) {
  }

  ngOnInit(): void {

  }

  ngAfterViewInit() {
    this.pageSize = this.request?.pageSize ? this.request?.pageSize : 100;

  }


  // Inputs

  @Input() public tableConfigurations!: TableConfigurations;
  private rows: any[] = [];

  @Input()
  public set tableRows(rows: any[] | AbstractControl[] | FormArray) {

    // let selectColumn = this.columns.find(x => x.type === TableColumnDataType.Select)
    // if (this.areRowsSimpleArray && selectColumn) rows = (rows as any[]).map(x => { // @ts-ignore
    //   x[selectColumn.name] = false;})

    if (rows) {
      if (this.areRowsSimpleArray === undefined) this.areRowsSimpleArray = Array.isArray(rows);
      this.rows = Array.isArray(rows) ? rows : (rows as any)?.controls;
    }

    if (this.tableConfigurations?.options?.useBuiltInSorting) this.rowsNeedSorting = true;
    this.setRows()
  };


  // Row Queries
  public get tableRows(): any[] {
    this.setRows();
    return this.shownRows;
  }

  async onTableScroll(e: any) {
    const tableViewHeight = e.target.offsetHeight // viewport
    const tableScrollHeight = e.target.scrollHeight // length of all table
    const scrollLocation = e.target.scrollTop; // how far user scrolled

    // If the user has scrolled within 200px of the bottom, add more data
    const buffer = 200;
    const limit = tableScrollHeight - tableViewHeight - buffer;
    if (scrollLocation > limit) {
      if (this.sendRequest == true) {
        this.sendRequest = false;
        this.isLoading = true;
        this.request.pageIndex++;
        this.request.pageSize = this.pageSize;
        let data = await this.mediator.send(this.request);
        this.tableConfigurations.pagination.pageSize += this.pageSize;
        //@ts-ignore
        const newData = data.data;
        const firstObject = newData.shift();

        this.tableRows = this.tableRows.concat(newData);
        this.isLoading = false;
        this.sendRequest = true;
      }
    }
  }


  handlePagination(event: any) {

    this.selectAllControl.setValue(false);
    this.tableConfigurations.pagination.pageIndex = event.pageIndex;
    this.tableConfigurations.pagination.pageSize = event.pageSize;

    this.paginationChanged.emit(this.tableConfigurations.pagination)
    if (this.tableConfigurations.options.useBuiltInPagination) return this.setRows()
  }

  public setRows() {
    let rows: any[] = this.rows;

    if (this.includedItems?.length > 0) {
      rows = this.includedItems;
      if (this.excludedItems?.length > 0) rows = this.removeExcludedItemsFromRows(rows)
      rows = this.getSortedRows(rows)
      rows = this.getFilteredRows(rows)
      if (this.tableConfigurations?.options?.useBuiltInPagination) rows = this.getPaginatedRows(rows);
    } else {
      if (this.excludedItems?.length > 0) rows = this.removeExcludedItemsFromRows(rows)
      if (this.tableConfigurations?.options?.useBuiltInSorting) rows = this.getSortedRows(rows);
      if (this.tableConfigurations?.options?.useBuiltInFilters) rows = this.getFilteredRows(rows);
      if (this.tableConfigurations?.options?.useBuiltInPagination) rows = this.getPaginatedRows(rows);
    }
    this.shownRows = rows;
  }

  public get tableHeaders() {
    return this.tableConfigurations.columns.filter(x => x.show !== false).map(column => {
      return column.name
    })
  }

  public get tableGroups() {
    let groups: any[] = [];
    this.columns.forEach(col => {
      if (col.type !== TableColumnDataType.Select) {
        let lastGroup = groups[groups.length - 1]

        if (col.groupId) {
          if (lastGroup?.id === col.groupId) {
            lastGroup.colSpan++
          } else {
            groups.push({
              name: col.groupName,
              id: col.groupId,
              colSpan: 1
            })
          }
        } else {
          lastGroup?.id === 'empty' ? lastGroup.colSpan++ : groups.push({
            name: '',
            id: 'empty',
            colSpan: 1
          });
        }
      }
    })
    groups.unshift({
      name: '',
      id: 'tableSettings',
      colSpan: 1
    })
    return groups;
  }

  public get tableGroupIds() {
    let groupIds = [];
    groupIds.push(...this.tableGroups.map(x => x.id))
    return groupIds;
  }

  public get columns(): TableColumn[] {
    return this.tableConfigurations.columns.filter(x => x.show !== false) as TableColumn[]
  }

  public get settingsColumns() {
    let columns = this.tableConfigurations.columns.filter(x => x.type !== TableColumnDataType.Index && x.type !== TableColumnDataType.Select)
    columns = columns.map(x => {
      if (x.show !== true && x.show !== false) {
        x.show = true;
      }
      return x;
    })
    return columns;
  }

  getPaginatedRows(rows: any[]) {
    let paginatedRows = rows;
    this.tableConfigurations.pagination.totalItems = rows?.length;
    let fromIndex = this.tableConfigurations.pagination.pageIndex * this.tableConfigurations.pagination.pageSize;
    let toIndex = fromIndex + this.tableConfigurations.pagination.pageSize;
    paginatedRows = rows?.slice(fromIndex, toIndex);
    return paginatedRows;
  }

  getFilteredRows(rows: any[]) {
    if (this.tableConfigurations?.filters?.length > 0) {
      let filteredRows = rows;
      this.tableConfigurations.filters.forEach(filter => {
        let column = this.columns.find(x => x.name === filter.columnName);

        if (filter.searchCondition === 'equal')
          filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => x[filter.columnName] == filter.searchValue) : (rows as FormGroup[])?.filter((x: any) => x.controls[filter.columnName]?.value == filter.searchValue);

        if (filter.searchCondition === 'greaterThan')
          filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => +x[filter.columnName] > +filter.searchValue) : (rows as FormGroup[])?.filter((x: any) => +x.controls[filter.columnName]?.value > +filter.searchValue);
        if (filter.searchCondition === 'lessThan')
          filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => +x[filter.columnName] < +filter.searchValue) : (rows as FormGroup[])?.filter((x: any) => +x.controls[filter.columnName]?.value < +filter.searchValue);
        if (filter.searchCondition === 'contains')

          if (filter.useDisplayFnForFilter) filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => column?.displayFn(x[filter.columnName])?.toString()?.includes(filter.searchValue)) : rows?.filter((x: any) => column?.displayFn(x.controls[filter.columnName]?.value)?.toString()?.includes(filter.searchValue));
          else
            filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => x[filter.columnName]?.toString()?.toLowerCase()?.includes(filter.searchValue?.toLowerCase())) : rows?.filter((x: any) => x.controls[filter.columnName]?.value?.toString()?.toLowerCase()?.includes(filter.searchValue?.toLowerCase()));
        if (filter.searchCondition === 'notContains')
          filteredRows = this.areRowsSimpleArray ? rows.filter((x: any) => !x[filter.columnName]?.toString()?.toLowerCase()?.includes(filter.searchValue?.toLowerCase())) : rows?.filter((x: any) => !x.controls[filter.columnName]?.value?.toString()?.toLowerCase()?.includes(filter.searchValue?.toLowerCase()));
      })
      return filteredRows;
    } else {
      return rows
    }
  }

  getSortedRows(rows: any[]) {
    let sortedRows = rows;
    if (this.rowsNeedSorting) {
      let sortKeys = this.tableConfigurations.sortKeys;
      if (sortKeys.length === 0 && this.tableConfigurations.options.hasDefaultSortKey) {
        this.tableConfigurations.options.hasDefaultSortKey = false;
        sortKeys.push([this.tableConfigurations.options.defaultSortKey, this.tableConfigurations.options.defaultSortDirection].join(' '))
        let defaultSortedColumn = this.columns.find(x => x.name === this.tableConfigurations.options.defaultSortKey)
        if (defaultSortedColumn) defaultSortedColumn.isSorted = true
      }

      if (sortKeys.length > 0) {
        sortKeys.forEach(sort => {
          let sortKey = sort.split(' ')[0]
          let sortDirection = sort.split(' ')[1]
          if (this.areRowsSimpleArray) {
            sortedRows = rows.sort(function (a: any, b: any) {
              let x = a[sortKey];
              let y = b[sortKey];
              return sortDirection === 'ASC' ? ((x < y) ? -1 : ((x > y) ? 1 : 0)) : ((y < x) ? -1 : ((y > x) ? 1 : 0));
            })
          } else {
            sortedRows = rows.sort(function (a: FormGroup, b: FormGroup) {
              let x = a.value[sortKey];
              let y = b.value[sortKey];
              return sortDirection === 'ASC' ? ((x < y) ? -1 : ((x > y) ? 1 : 0)) : ((y < x) ? -1 : ((y > x) ? 1 : 0));
            })
          }
        })
      }
      this.rowsNeedSorting = false;
    }
    return sortedRows;
  }


  // Table Queries
  getOptionTitle(option: any, keys: any[]) {
    let titles: any[] = [];
    keys.forEach(key => {
      titles.push(option[key]);
    });
    return titles.join(' ')
  }

  getSelectFormControlName(row: FormGroup, column: TableColumn) {
    let selectRow = row.get(column.name);
    if (!selectRow) {
      row.addControl(column.name, new FormControl(false))
    }
    return column.name;
  }

  getColumnFilterOperands(column: TableColumn) {
    return TableColumnFilterOperands.filter(x => x.dataTypes.includes(column.filter.filterType))
  }

  // Events
  @Output() rowClicked: EventEmitter<any | FormGroup> = new EventEmitter<any | FormGroup>();
  @Output() rowUpdated: EventEmitter<any | FormGroup> = new EventEmitter<any | FormGroup>();
  @Output() rowDoubleClicked: EventEmitter<any | FormGroup> = new EventEmitter<any | FormGroup>();
  @Output() selectedRowChange: EventEmitter<any | FormGroup> = new EventEmitter<any | FormGroup>();
  @Output() onFormEscape: EventEmitter<any | FormGroup> = new EventEmitter<any | FormGroup>();
  @Output() filtersChanged: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  @Output() sortChanged: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  @Output() paginationChanged: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  @Output() rowSelected: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  @Output() allRowsSelected: EventEmitter<any | FormArray> = new EventEmitter<any | FormArray>();
  @Output() formKeydown: EventEmitter<any> = new EventEmitter<any>()


  @Output() exportData: EventEmitter<any> = new EventEmitter<any>();

  @Output() includeOnlySelectedItemsEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() clearSelectedItemsEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() excludeSelectedItemsEvent: EventEmitter<any> = new EventEmitter<any>();

  // Event Handlers
  onRowClick(row: FormGroup) {
    this.tableConfigurations.highlightedRow = row;
    this.rowClicked.emit(row)
  }

  onRowDoubleClick(row: FormGroup) {
    if (this.tableConfigurations?.options?.editRowOnDoubleClick) {
      this.tableConfigurations.changeRowToEdit(row, this.rowUpdated);
      this.selectedRowChange.emit(row)
    }
    this.rowDoubleClicked.emit(row);
  }

  onFormEscapeHandler() {
    this.onFormEscape.emit(this.tableConfigurations.rowToEdit);
    this.tableConfigurations.changeRowToEdit(null);
    this.selectedRowChange.emit(null);
  }

  handleFormKeydown(event: any, columnName: string, element?: any, isMoney?: boolean) {

    this.formKeydown.emit({key: event.code, columnName: columnName})


    // if (isMoney == true) {

    //   var input = event.srcElement.value;
    //   let sp = input.split(',');
    //   let result = "";
    //   for (let index = 0; index < sp.length; index++) {
    //     result += sp[index];
    //   }
    //   let dot = result.split('.');
    //   let flag = false;
    //   if (dot.length > 1) {
    //     result += event.key;
    //     flag = true;
    //   }
    //   if (result && result != 'undefined') {
    //     let final = parseFloat(result).toLocaleString();
    //     if (flag == true) {
    //       final = final.substring(0, final.length - 1);
    //     }
    //     let numfinal = parseFloat(result);
    //     if (flag == true) {
    //       numfinal = parseFloat(numfinal.toString().substring(0, numfinal.toString().length - 1));
    //     }
    //     element.value[columnName]=numfinal;
    //     this.rowUpdated.emit(element);
    //     event.srcElement.value = final;
    //   }
    // }
  }

  // Actions
  updateFilters(menuTrigger?: MatMenuTrigger) {
    this.tableConfigurations.filters = [];
    this.columns.forEach(column => {
      if (column.filter?.filters?.filter(x => x.searchValue)?.length === 0 || column.filter?.filters?.filter(x => x.multipleSearchValues?.length > 0)?.length === 0) column.isFiltered = false;
      column?.filter?.filters.forEach(filter => {
        if (filter.searchValue || filter.multipleSearchValues?.length > 0) {
          filter.searchCondition = filter.actualSearchCondition;
          menuTrigger?.closeMenu();
          // TODO make sure to add other multiple value search comparison types like inList whenever added the feature to table filters
          if (filter.searchCondition === SearchComparisonTypes.between) {
            filter.searchValue = undefined;
          }
          column.isFiltered = true;
          if (filter.filterType === TableColumnFilterTypes.Date && filter.searchCondition === 'between' && (filter.multipleSearchValues[1]._d ?? filter.multipleSearchValues[1]).getTime().toString().endsWith('0')) {
            filter.multipleSearchValues[1]._d = new Date((filter.multipleSearchValues[1]._d ?? filter.multipleSearchValues[1]).getTime() + 86399999);
          }
          if (filter.filterType === TableColumnFilterTypes.Date && filter.searchCondition === 'equal' && filter.searchValue._d.getTime().toString().endsWith('0')) {
            filter.multipleSearchValues[0] = filter.searchValue._d;
            filter.multipleSearchValues[1] = new Date(filter.searchValue._d.getTime() + 86399999);
            filter.searchCondition = 'between'
          }

          if (filter.searchCondition === 'in') {
            let values = filter.searchValue?.split(',');
            if (column.type === TableColumnDataType.Number) values = values.map((x: any) => +x);
            filter.multipleSearchValues = values
          }

          this.tableConfigurations.filters.push(filter)
        }

      })
    })
    this.goToFirstPage()
    this.filtersChanged.emit(this.tableConfigurations.filters);
    if (this.tableConfigurations.options.useBuiltInFilters) return this.setRows()
  }

  removeFilter(column: TableColumn) {
    column.isFiltered = false;
    let filterToRemove = this.tableConfigurations.filters.find(x => x.columnName === column.filter.columnName);
    if (filterToRemove) this.tableConfigurations.filters.splice(this.tableConfigurations.filters.indexOf(filterToRemove), 1);
    this.goToFirstPage()
    this.filtersChanged.emit(this.tableConfigurations.filters);
    if (this.tableConfigurations.options.useBuiltInFilters) return this.setRows()
  }

  handleSort(column: TableColumn) {
    let key = this.tableConfigurations.sortKeys.find(x => x.includes(column.name))
    if (key) {
      if (key.includes('DESC')) {
        this.tableConfigurations.sortKeys[this.tableConfigurations.sortKeys.indexOf(key)] = column.name + ' ASC'
      } else if (key.includes('ASC')) {
        column.isSorted = false;
        this.tableConfigurations.sortKeys.splice(this.tableConfigurations.sortKeys.indexOf(key), 1)
      }
    } else {
      column.isSorted = true;
      this.tableConfigurations.sortKeys.push(column.name + ' DESC');
    }
    this.sortChanged.emit(this.tableConfigurations.sortKeys)
    if (this.tableConfigurations.options.useBuiltInSorting) {
      this.rowsNeedSorting = true;
      return this.setRows()
    }
  }

  getColumnCurrentSortDirection(column: TableColumn) {
    return this.tableConfigurations.sortKeys.find(x => x.includes(column.name))?.split(' ')[1]?.toLowerCase();
  }


  selectAll(column: TableColumn) {
    let rows = this.tableRows;
    if (!this.areRowsSimpleArray) {
      rows.forEach((control: AbstractControl) => {
        (control as FormGroup).controls[column.name].setValue(this.selectAllControl.value)
      })

    } else {
      rows.forEach((row) => {
        row[column.name] = this.selectAllControl.value
      })
    }
    this.allRowsSelected.emit()
  }

  // Exports
  exportExcel() {
    let rows = this.tableRows;

    let validColumns = this.tableConfigurations.columns.filter(x => x.type !== TableColumnDataType.Select && x.type !== TableColumnDataType.Index);

    let items: any[] = [];


    items = this.areRowsSimpleArray ? rows : rows.map((x: FormGroup) => x.value)
    items = items.map((x: any) => {
      let y: any = {};
      validColumns.forEach(c => {
        if (c.type === TableColumnDataType.Date) {
          y[c.title] = x[c.name] ? moment(moment.utc(<Date>x[c.name]).toDate()).locale('fa').format('jYYYY/jMM/jDD') : '';

        } else if (c.type === TableColumnDataType.Money) {
          y[c.title] = x[c.name];
        } else {
          y[c.title] = c.displayFn ? c.displayFn(x) : x[c.name]
        }
      })
      return y;
    })

    this.createExcelFromArray(items);
  }

  customExportFunctionHandler() {
    let items: any[] = this.tableConfigurations.options.exportOptions.customExportCallbackFn(this.tableRows)
    this.createExcelFromArray(items)
  }

  // Sum Row
  calculateColumnSum(columnName: string) {
    let rows = [];
    let selectColumn = this.columns.find(x => x.type === this.fieldTypes.Select);
    if (this.areRowsSimpleArray) {
      rows = this.rows;
    } else {
      rows = this.rows.map(x => x.getRawValue());
    }
    if (rows?.length > 0) {

      if (selectColumn) {
        let selectedRows = rows?.filter(x => x[(<TableColumn>selectColumn).name])
        if (selectedRows?.length > 0) rows = selectedRows;
      }

      return rows.reduce((accum, curr) => +accum + (+curr[columnName] ?? 0), 0);
    } else {
      return "";
    }
  }


  createExcelFromArray(items: any[]) {

    let fileName = 'ExcelSheet.xlsx';

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(items);

    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    if (!wb.Workbook) wb.Workbook = {};
    if (!wb.Workbook.Views) wb.Workbook.Views = [];
    if (!wb.Workbook.Views[0]) wb.Workbook.Views[0] = {};
    wb.Workbook.Views[0].RTL = true;


    XLSX.writeFile(wb, fileName);
  }


  export(format: string) {
    this.exportData.emit(format)
  }


  excludeSelectedItems() {
    // @ts-ignore
    let selectCol: TableColumn = this.tableConfigurations.columns.find(x => x.type === TableColumnDataType.Select)
    if (selectCol) {
      this.excludedItems = this.areRowsSimpleArray ? this.rows.filter(x => x[selectCol.name] === true) : (this.rows as AbstractControl[]).filter(x => x.value[selectCol.name] === true);
      let ids = this.excludedItems.map((a: any) => a.id);
      this.excludeSelectedItemsEvent.emit(ids);
      this.excludedItems.forEach((x: any) => {
        this.areRowsSimpleArray ? x[selectCol.name] = false : x.controls[selectCol.name].setValue(false, {emitEvent: false})
      })
    }
    if (this.excludedItems?.length > 0) {
      this.realTotalItemsCount = JSON.parse(JSON.stringify(this.tableConfigurations.pagination.totalItems));
      this.tableConfigurations.pagination.totalItems = this.realTotalItemsCount - this.excludedItems.length;
      this.setRows();
    }
  }

  clearExcludedItems() {
    this.clearSelectedItemsEvent.emit();
    if (this.excludedItems && this.excludedItems?.length > 0) {
      (this.excludedItems as any[] | AbstractControl[]).forEach(x => {
        // @ts-ignore
        let selectCol: TableColumn = this.tableConfigurations.columns.find(x => x.type === TableColumnDataType.Select)
        this.areRowsSimpleArray ? x[selectCol.name] = false : x.controls[selectCol.name].setValue(false, {emitEvent: false});
      })
      this.excludedItems = [];
      this.tableConfigurations.pagination.totalItems = this.realTotalItemsCount;
      this.setRows();
    }
  }

  includeOnlySelectedItems() {
    // @ts-ignore
    let selectCol: TableColumn = this.tableConfigurations.columns.find(x => x.type === TableColumnDataType.Select)
    if (selectCol) {
      this.includedItems = this.areRowsSimpleArray ? this.rows.filter(x => x[selectCol.name] === true) : (this.rows as AbstractControl[]).filter(x => x.value[selectCol.name] === true);

      let ids = this.includedItems.map((a: any) => a.id);
      this.includeOnlySelectedItemsEvent.emit(ids);
      this.includedItems.forEach((x: any) => {
        this.areRowsSimpleArray ? x[selectCol.name] = false : x.controls[selectCol.name].setValue(false, {emitEvent: false})
      })
    }
    if (this.includedItems?.length > 0) {
      this.isSelectedList = true;
      this.realTotalItemsCount = JSON.parse(JSON.stringify(this.tableConfigurations.pagination.totalItems));
      this.tableConfigurations.pagination.totalItems = this.includedItems.length;
      this.setRows();
    }
  }

  clearIncludeOnlySelectedItems() {
    if (this.includedItems && this.includedItems?.length > 0) {

      (this.includedItems as any[] | AbstractControl[]).forEach(x => {
        // @ts-ignore
        let selectCol: TableColumn = this.tableConfigurations.columns.find(x => x.type === TableColumnDataType.Select)
        this.areRowsSimpleArray ? x[selectCol.name] = false : x.controls[selectCol.name].setValue(false, {emitEvent: false});
      })
      this.isSelectedList = false;
      this.includedItems = [];
      this.tableConfigurations.pagination.totalItems = this.realTotalItemsCount;
      this.setRows();
    }
  }

  removeExcludedItemsFromRows(rows: any[] | AbstractControl[]) {
    if (this.areRowsSimpleArray) {
      rows = rows.filter((el: any) => !(this.excludedItems as any[]).includes(el));
    } else {
      rows = rows.filter((el: any) => !(this.excludedItems as AbstractControl[]).includes(el));
    }

    return rows;
  }

  getControlFirstErrorMessage(control: any) {
    if (control.errors) {
      let keys = Object.keys(control.errors);
      if (keys?.length > 0) {
        return control?.errors[keys[0]]
      }
    }
    return '';
  }

  goToFirstPage() {
    this.tableConfigurations.pagination.pageIndex = 0;
    this.paginationChanged.emit()
  }

  goToLastPage() {
    let lastPageIndex = (this.tableConfigurations.pagination.totalItems / this.tableConfigurations.pagination.pageSize) + (
      (this.tableConfigurations.pagination.totalItems % this.tableConfigurations.pagination.pageSize) > 0 ? 1 : 0
    )
    lastPageIndex = Math.floor(lastPageIndex)
    this.tableConfigurations.pagination.pageIndex = lastPageIndex - 1;

  }

  removeAllFiltersAndSorts() {
    this.tableConfigurations.filters = []
    this.tableConfigurations.sortKeys = []
    this.tableConfigurations.columns.filter(x => x.isFiltered).forEach(col => {
      col.isFiltered = false;
      col.isSorted = false;
      col.filter.setDefaultFilter();
    })
    this.clearExcludedItems();
    this.clearIncludeOnlySelectedItems()
    this.clearSelectedItemsEvent.emit();

    this.goToFirstPage()

  }

  updateColumnsShowStatus() {
    this.tableConfigurations.columns.forEach(x => {
      x.show = x.tempShowStatus
    })
  }

  protected readonly TableColumnDataType = TableColumnDataType;
  trackByFn(index: number, item: any): number {
    return item.id; // unique identifier
  }

  scrollToTop() {
    let element = <HTMLElement>document.getElementById("appTableWrapper")
    element.scrollTop = 0;
  }

  scrollToBottom() {
    let element = <HTMLElement>document.getElementById("appTableWrapper")
    element.scrollTop = element.scrollHeight;
  }
}
