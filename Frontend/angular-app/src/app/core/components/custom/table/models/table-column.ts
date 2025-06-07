import {TableColumnDataType} from "./table-column-data-type";
import {TableColumnFilter} from "./table-column-filter";
import {TemplateRef} from "@angular/core";

export class TableColumn {

  constructor(
    name: string,
    title: string,
    type: TableColumnDataType,
    width: string = '',
    sortable: boolean = false,
    filter?: TableColumnFilter,
    displayFunction?: Function,
    disabled: boolean = false,
    show: boolean = true,
    options: any[] = [],
    optionsValueKey: string = 'id',
    optionsTitleKey: string[] = ['title'],
    filterOptionsFunction?: Function,
    filteredOptions: any[] = [],
    asyncOptions?: Function,
    lineStyle: 'default' | 'onlyShowFirstLine' = "default",
    showSumRow:boolean = true,
    truncateTextLimit?: number
  ) {
    this.name = name;
    this.title = title;
    this.type = type;
    this.width = width;
    this.sortable = sortable;
    filter ? this.filter = filter : undefined;
    this.disabled = disabled;
    this.show = show;
    this.displayFn = displayFunction;
    this.options = options;
    this.optionsValueKey = optionsValueKey;
    this.optionsTitleKey = optionsTitleKey;
    this.filterOptionsFn = filterOptionsFunction;
    this.filteredOptions = filteredOptions;
    this.lineStyle = lineStyle;
    this.showSumRow = showSumRow;
    this.truncateTextLimit = truncateTextLimit;
  }

  public title!: string;
  public name!: string;
  public type!: TableColumnDataType;
  public filter!: TableColumnFilter;

  public template!: TemplateRef<any>;

  public width!: string;
  public sortable: boolean = true;
  public disabled: boolean = false;
  public show: boolean = true;
  public tempShowStatus: boolean = true;
  public isSorted: boolean = false
  public isFiltered: boolean = false
  public groupName!: string;
  public groupId!: string;
  public lineStyle!: 'default' | 'onlyShowFirstLine';
  public showSumRow: boolean = true;
  public truncateTextLimit: number | undefined = undefined;
  public sumRowDisplayFn : Function | any;
  private _options: any[] = [];
  public get options() {
    return this._options;
  };

  public set options(items: any[]) {
    this._options = items;
    this.filteredOptions = items
  };

  public _filteredOptions: any[] = [];
  public get filteredOptions() {
    return this._filteredOptions.slice(0, 50)
  };

  public set filteredOptions(items: any[]) {
    this._filteredOptions = items
  };

  public optionsValueKey!: string;
  public optionsTitleKey!: string[];
  public filterOptionsFn!: Function | any;
  public displayFn!: Function | any;

}
