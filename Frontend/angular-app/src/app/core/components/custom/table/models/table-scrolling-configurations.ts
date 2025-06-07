
import {TableOptions} from "./table-options";
import {TablePaginationOptions} from "./table-pagination-options";
import {TableColumnFilterValue} from "./table-column-filter";
import {FormGroup} from "@angular/forms";
import {TableColumnFilterTypes} from "./table-column-filter-types";
import {Column} from "./column";
import {showTools, ToolBar} from "./tool-bar";
import { PrintOptions} from "./print_options";

export class TableScrollingConfigurations {
  public columns!: Column[];
  public toolBar!: ToolBar;
  public options!: TableOptions;
  public pagination!: TablePaginationOptions;
  public filters: TableColumnFilterValue[] = [];
  public includeOnlySelectedItemsLocal : any[] =[];
  public excludeSelectedItemsLocal : any[] =[];
  public includeOnlySelectedItemsFilter : any[] =[];
  public excludeSelectedItemsFilter : any[] =[];
  public selectedItems : any[] =[];
  public printOptions !: PrintOptions
  public sortKeys: string[] = [];
  rowToEdit!: any;
  rowToEditSubscription!: any;
  highlightedRow!: any;
  showOnlyIncludedItems: boolean = false;
  changeRowToEdit(row: FormGroup | null, callback?: any) {
    if (this.rowToEditSubscription) this.rowToEditSubscription.unsubscribe();
    if (callback && row) {
      this.rowToEditSubscription = row.valueChanges.subscribe(() => {
        try {
          callback(row);
        } catch (e) {}
      })
    }
    this.rowToEdit = row;
    this.highlightedRow = row;
  }
  constructor(columns: Column[], options?: TableOptions, pagination?: TablePaginationOptions, toolBar ? : ToolBar ,printOptions ? : PrintOptions) {
    this.columns = columns ?? [];

    this.options = options ?? new TableOptions(false, false);
    this.pagination = pagination ?? new TablePaginationOptions();

    this.toolBar = this.defaultToolBar(toolBar);
    this.printOptions = printOptions ?? new PrintOptions();

  }
  defaultToolBar(toolBar?: ToolBar): ToolBar {
    const defaultShowTools = {
      tableSettings: true,
      includeOnlySelectedItemsLocal: false,
      excludeSelectedItemsLocal: false,
      includeOnlySelectedItemsFilter: false,
      excludeSelectedItemsFilter: false,
      undoLocal: false,
      deleteLocal: false,
      restorePreviousFilter: false,
      refresh: false,
      exportExcel: false,
      fullScreen: true,
      printFile: false,
      removeAllFiltersAndSorts: false,
      showAll: false,
    };
    const { showTools = {}, isLargeSize = false } = toolBar ?? {};
    return {
      showTools: { ...defaultShowTools, ...showTools },
      isLargeSize,
    };
  }


}

export let TableColumnFilterOperands = [
  {
    title: 'مشمول بر',
    name: 'contains',
    value: 'contains',
    label: 'contains',
    dataTypes: [TableColumnFilterTypes.Text]
  },
  {
    title: 'مساوی با',
    name: 'equal',
    value: 'equal',
    label: '=',
    dataTypes: [TableColumnFilterTypes.Text, TableColumnFilterTypes.Number, TableColumnFilterTypes.Money, TableColumnFilterTypes.Date]

  },
  {
    title: 'نا مساوی با',
    name: 'notEqual',
    value: 'notEqual',
    label: '!=',
    dataTypes: [TableColumnFilterTypes.Text, TableColumnFilterTypes.Number, TableColumnFilterTypes.Money]
  },
  {
    title: 'شروع با',
    name: 'startsWith',
    value: 'startsWith',
    label: 'startsWith',
    dataTypes: [TableColumnFilterTypes.Text]
  },
  {
    title: 'پایان با',
    name: 'endsWith',
    value: 'endsWith',
    label: 'endsWith',
    dataTypes: [TableColumnFilterTypes.Text]
  },

  {
    title: 'نامشتمل بر',
    name: 'notContains',
    value: 'notContains',
    label: 'notContains',
    dataTypes: [TableColumnFilterTypes.Text]
  },
  // {
  //   title: 'در مجموعه',
  //   name: 'in',
  //   value: 'in',
  //   label: 'in',
  //   dataTypes: [TableColumnFilterTypes.Text, TableColumnFilterTypes.Number, TableColumnFilterTypes.Money]
  // },
  {
    title: 'مابین',
    name: 'between',
    value: 'between',
    label: 'between',
    dataTypes: [TableColumnFilterTypes.Number, TableColumnFilterTypes.Money, TableColumnFilterTypes.Date]
  },
  {
    title: 'بزرگتر از',
    name: 'greaterThan',
    value: 'greaterThan',
    label: '>=',
    dataTypes: [TableColumnFilterTypes.Number, TableColumnFilterTypes.Money, TableColumnFilterTypes.Date]
  },
  {
    title: 'کوچکتر از',
    name: 'lessThan',
    value: 'lessThan',
    label: '<=',
    dataTypes: [TableColumnFilterTypes.Number, TableColumnFilterTypes.Money, TableColumnFilterTypes.Date]
  },
]


