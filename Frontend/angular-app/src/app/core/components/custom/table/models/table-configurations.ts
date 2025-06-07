import {EventEmitter} from "@angular/core";
import {FormGroup} from "@angular/forms";
import {TableColumnFilter, TableColumnFilterValue} from "./table-column-filter";
import {TableColumnFilterTypes} from "./table-column-filter-types";
import {TablePaginationOptions} from "./table-pagination-options";
import {TableOptions} from "./table-options";
import {TableColumn} from "./table-column";

export class TableConfigurations {
  public columns!: TableColumn[];
  public options!: TableOptions;
  public pagination!: TablePaginationOptions;
  public filters: TableColumnFilterValue[] = [];
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

  constructor(columns: TableColumn[], options?: TableOptions, pagination?: TablePaginationOptions) {
    this.columns = columns ?? [];
    this.options = options ?? new TableOptions(false, false);
    this.pagination = pagination ?? new TablePaginationOptions();
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
    dataTypes: [TableColumnFilterTypes.Text, TableColumnFilterTypes.Number, TableColumnFilterTypes.Money, TableColumnFilterTypes.Date, TableColumnFilterTypes.CheckBox]

  },
  {
    title: 'نا مساوی با',
    name: 'notEqual',
    value: 'notEqual',
    label: '!=',
    dataTypes: [TableColumnFilterTypes.Text, TableColumnFilterTypes.Number, TableColumnFilterTypes.Money , TableColumnFilterTypes.CheckBox]
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

