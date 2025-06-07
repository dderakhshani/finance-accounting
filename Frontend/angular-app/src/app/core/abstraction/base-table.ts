import {BaseComponent} from "./base.component";
import {FontFamilies} from "../components/custom/table/models/font-families";
import {ToolBar} from "../components/custom/table/models/tool-bar";
import {SearchQuery} from "../../shared/services/search/models/search-query";
import {TableScrollingConfigurations} from "../components/custom/table/models/table-scrolling-configurations";
import {FontWeights} from "../components/custom/table/models/font-weights";
import {Column, TypeFilterOptions} from "../components/custom/table/models/column";

export abstract class BaseTable extends BaseComponent{

  requestsIndex: number = 0;
  requestsList: string[] = [];
  saveRequests: any;

  rowData: any[] = [];
  rowDataClone: any[] = [];
  subRowData: any[] = [];
  isLoadingSubTable :boolean=false;
  excludedRows: any = [];
  toolBar: ToolBar = {
    showTools: {
      tableSettings: true,
      includeOnlySelectedItemsLocal: false,
      excludeSelectedItemsLocal: false,
      includeOnlySelectedItemsFilter: false,
      excludeSelectedItemsFilter: false,
      undoLocal: false,
      deleteLocal: false,
      restorePreviousFilter: false,
      refresh: true,
      exportExcel: false,
      fullScreen: true,
      printFile: true,
      removeAllFiltersAndSorts: true,
      showAll: true
    },
    isLargeSize: false
  }
  filterConditionsInputSearch: { [key: string]: any } = {};
  selectedItemsFilterForPrint: any = [new SearchQuery({
    propertyName: 'id',
    values: [],
    comparison: 'in',
    nextOperand: 'and'
  })];
  tableConfigurations!: TableScrollingConfigurations;
  columns!: Column[]
  defaultColumnSettings = {
    class: '',
    style: {},
    display: true,
    print: true,
    sortable: true,
    filter: undefined,
    displayFunction: undefined,
    disabled: false,
    options: [],
    optionsValueKey: 'id',
    optionsTitleKey: [],
    filterOptionsFunction: undefined,
    filteredOptions: [],
    asyncOptions: undefined,
    showSum: false,
    sumColumnValue: 0,
    matTooltipDisabled: true,
    fontSize: 12,
    fontFamily: FontFamilies.IranSans,
    fontWeight: FontWeights.normal,
    isCurrencyField: false,
    isDisableDrop: false,
    typeFilterOptions: TypeFilterOptions.None,
    lineStyle: 'onlyShowFirstLine',
  };

}
