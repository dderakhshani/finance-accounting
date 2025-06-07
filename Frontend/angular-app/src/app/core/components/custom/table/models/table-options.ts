import { TableExportOptions } from "./table-export-options";
import {Input} from "@angular/core";

export class TableOptions {
  editRowOnDoubleClick: boolean = false;
  usePagination: boolean = false;
  useBuiltInPagination: boolean = false;
  tableHeight: string = '100%';
  useBuiltInFilters: boolean = false;
  useBuiltInSorting: boolean = false;
  hasDefaultSortKey: boolean = false;
  defaultSortKey: string = 'id';
  defaultSortDirection: 'ASC' | 'DESC' = 'ASC';

  resetFiltersLocal: boolean = false;
  showSumRow: boolean = false;
  showFilterRow: boolean = false;
  showGroupRow: boolean = false;
  showGroupRemainingRow: boolean = false;
  showAdvancedFilter: boolean = false;
  sumLabel: string = 'جمع کل';
  exportOptions: TableExportOptions = new TableExportOptions();
  showTopSettingMenu: boolean = false;
  lazyLoading: boolean = false;
  isVirtualScrollEnabled: boolean = false;
  isLoadingTable: boolean = false;
  itemSize: number = 40;
  isSelectedMode: boolean = false;
  isExternalChange: boolean = true;
  printSumRow: boolean = false;
  printGroupRemainingRow: boolean = false;

  constructor(editableRows: boolean = false, usePagination: boolean = false, tableHeight: string = '100%', useBuiltInPagination: boolean = false, useBuiltInFilters: boolean = false, useBuiltInSorting: boolean = false,isVirtualScrollEnabled:boolean = false
    , isLoadingTable: boolean = false , itemSize: number = 40) {
    this.editRowOnDoubleClick = editableRows;
    this.usePagination = usePagination;
    this.tableHeight = tableHeight;
    this.useBuiltInPagination = useBuiltInPagination;
    this.useBuiltInFilters = useBuiltInFilters;
    this.useBuiltInSorting = useBuiltInSorting;
    this.isVirtualScrollEnabled = isVirtualScrollEnabled;
    this.isLoadingTable = isLoadingTable;
    this.itemSize = itemSize;

  }
}
