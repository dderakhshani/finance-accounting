import {TableColumnDataType} from "./table-column-data-type";
import {TableColumnFilter} from "./table-column-filter";
import {TemplateRef} from "@angular/core";

export interface Column {
  index: number;
  field: string;
  title: string;
  width: number;
  type: TableColumnDataType,
  display: boolean;
  isDisableDrop: boolean;
  matTooltipDisabled: boolean;
  fontSize: number;
  fontWeight ?: any;
  fontFamily ?: any;
  lineStyle: 'default' | 'onlyShowFirstLine';
  digitsInfo?: string;
  print?: boolean;

  sumRowDisplayFn?: Function,
  sumColumnValue?: number,
  showSum: boolean,

  isCurrencyField: boolean,

  class?: string;
  style?: any;
  headerStyle?: any;
  footerStyle?: any;
  isSorted?: boolean,
  isFiltered?: boolean,
  isHovered_TH?: boolean,
  sortable?: boolean,
  filter?: TableColumnFilter | null,
  displayFunction?: Function | null,
  displayPrintFun?: Function ,

  groupName?: string;
  groupId?: string;
  groupStyle?: any;
  groupRemainingId?: string;
  groupRemainingNameOrFn?: string | Function;

  disabled?: boolean,

  options?: any[],

  filterOptionsFunction?: Function | null,
  filteredOptions?: any,
  asyncOptions?: Function | null,
  template?: TemplateRef<any>,
  expandRowWithTemplate ? : TemplateRef<any>,
  typeFilterOptions?: TypeFilterOptions,
  optionsValueKey?: string,
  optionsSelectTitleKey?: string,
  optionsTitleKey?: string[];
  filterOptionsFn?: Function | any;
  displayFn?: Function | any;
}


export enum viewMode {
  default = 'default',
  group = 'group',
  tree = 'tree'
}
export interface FilterCondition {
  searchValues: any;
  propertyNames: any;
  searchCondition: string;
  nextOperand: string;
}
export interface Condition {
  searchValues: any;
  propertyNames: string;
  searchCondition: string;
  nextOperand: string;
}


export interface FilteredOptions {
  request?: any;
  pageIndex?: number;
  pageSize?: number;
  conditions?: Condition[];
  orderByProperty?: string;
}

export enum TypeFilterOptions {
  TextInputSearch,
  NumberInputSearch,
  NgSelect,
  Date,
  None
}
export interface groupColumn {
  groupId?: string;
  groupName?: string | Function;
  colspan?: number;
  width?:string;

}
