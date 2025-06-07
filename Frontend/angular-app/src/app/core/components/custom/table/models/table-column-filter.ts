import {TableColumnFilterTypes} from "./table-column-filter-types";
import {TableColumnFilterOperands} from "./table-configurations";

export class TableColumnFilter {
  public filters: TableColumnFilterValue[] = [];
  public filterType: TableColumnFilterTypes = TableColumnFilterTypes.Text;
  public columnName!: string;
  public useDisplayFnForFilter!: boolean;
  public bindingsList!: any[];


  constructor(columnName: string, filterType: TableColumnFilterTypes,useDisplayFnForFilter: boolean = false,bindingsList :any[] =[] ) {
    this.columnName = columnName;
    this.filterType = filterType;
    this.useDisplayFnForFilter = useDisplayFnForFilter;
    this.bindingsList = bindingsList;
    this.setDefaultFilter()
  }


  setDefaultFilter() {

    this.filters = []
    this.addFilter()

  }
  addFilter() {
    let conditions = TableColumnFilterOperands.filter(x => x.dataTypes.includes(this.filterType));
    let defaultFilter = new TableColumnFilterValue();
    defaultFilter.columnName = this.columnName;
    defaultFilter.filterType = this.filterType;
    defaultFilter.useDisplayFnForFilter = this.useDisplayFnForFilter;
    defaultFilter.bindingsList = this.bindingsList;
    defaultFilter.actualSearchCondition = conditions[0]?.value
    defaultFilter.searchCondition = conditions[0]?.value
    defaultFilter.nextOperand = "and";
    this.filters.push(defaultFilter)
  }
  removeFilter(index:number) {
    this.filters.splice(index,1)
  }
}


export class TableColumnFilterValue {
  public searchValue: any = undefined;
  public multipleSearchValues: any[] = [];
  public bindingsList: any[] = [];
  public searchCondition: any = undefined;
  public actualSearchCondition: any = undefined;
  public columnName!: string;
  public useDisplayFnForFilter!: boolean;
  public filterType: TableColumnFilterTypes = TableColumnFilterTypes.Text;
  public nextOperand: 'and' | 'or' = 'and';
}
