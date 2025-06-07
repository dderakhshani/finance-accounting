export interface ToolBar {
  showTools :showTools,
  isLargeSize: boolean;
}
export interface showTools {
  tableSettings: boolean;

  includeOnlySelectedItemsLocal: boolean;
  excludeSelectedItemsLocal: boolean;
  includeOnlySelectedItemsFilter: boolean;
  excludeSelectedItemsFilter: boolean;
  undoLocal : boolean;
  deleteLocal : boolean;
  restorePreviousFilter : boolean;
  refresh: boolean;
  exportExcel: boolean;
  fullScreen: boolean;
  printFile: boolean;
  removeAllFiltersAndSorts: boolean;
  showAll: boolean;
}
export enum DataDisplayMode {
  showAll = 'نمایش همه',
  includeOnlySelectedItemsLocal= 'نمایش موارد انتخاب شده',
  showAllWithFilter = 'نمایش همه با فیلتر',

}
