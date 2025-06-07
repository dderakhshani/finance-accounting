import {CdkVirtualScrollViewport} from "@angular/cdk/scrolling";
import {
  AfterViewInit, ChangeDetectorRef,
  Component,
  ElementRef, EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output, Renderer2, SimpleChanges,
  ViewChild, ViewEncapsulation
} from "@angular/core";
import {TableScrollingConfigurations} from "../models/table-scrolling-configurations";
import {Column, groupColumn, TypeFilterOptions, viewMode} from "../models/column";

import {BehaviorSubject, Subject} from "rxjs";
import {ListRange} from "@angular/cdk/collections";
import {TableColumnDataType} from "../models/table-column-data-type";
import {FormControl, FormGroup} from "@angular/forms";

import {TableSettingsService} from "../table-details/Service/table-settings.service";

import {TableColumnFilterTypes} from "../models/table-column-filter-types";
import {SearchComparisonTypes} from "../../../../../shared/services/search/models/search-query";
import {MatMenuTrigger} from "@angular/material/menu";
import {TableColumnFilterOperands} from "../models/table-configurations";

import {debounceTime, distinctUntilChanged, filter} from "rxjs/operators";

import {TabManagerService} from "../../../../../layouts/main-container/tab-manager.service";
import {CustomDecimalPipe} from "../table-details/pipe/custom-decimal.pipe";

import * as XLSX from "xlsx";

import {Toastr_Service} from "../../../../../shared/services/toastrService/toastr_.service";
import {TableVirtualScrollHelperService} from "../table-details/Service/table-virtual-scroll-helper.service";
import {GenerateExcelService} from "../table-details/Service/generate-excel.service";


@Component({
  selector: 'app-table-virtual-scrolling',
  templateUrl: './table-virtual-scrolling.component.html',
  styleUrls: ['./table-virtual-scrolling.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TableVirtualScrollingComponent implements OnInit, OnChanges, AfterViewInit, OnDestroy {

  @ViewChild(CdkVirtualScrollViewport, {static: true}) cdkViewport!: CdkVirtualScrollViewport;
  @ViewChild('headerTable', {static: true}) headerTable!: ElementRef;
  @ViewChild('bodyTable', {static: true}) bodyTable!: ElementRef;
  @ViewChild('footerTable', {static: true}) footerTable!: ElementRef;
  @Input() tableRows: any[] = [];
  @Input() outPrint: boolean = false;
  @Input() tableConfigurations!: TableScrollingConfigurations;
  tableConfigurationsClone!: TableScrollingConfigurations;
  @Input() requestsIndex!: number;
  @Input() showCurrencyFields!: boolean;
  // Events
  @Output() rowUpdated: EventEmitter<any> = new EventEmitter<any>();
  @Output() rowClicked: EventEmitter<any> = new EventEmitter<any>();
  @Output() rowDoubleClicked: EventEmitter<any> = new EventEmitter<any>();
  @Output() selectedRowChange: EventEmitter<any> = new EventEmitter<any>();
  @Output() onFormEscape: EventEmitter<any> = new EventEmitter<any>();
  @Output() filtersChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output() sortChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output() paginationChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output() rowSelected: EventEmitter<any> = new EventEmitter<any>();
  @Output() rowsSelected: EventEmitter<any> = new EventEmitter<any>();
  @Output() allRowsSelected: EventEmitter<any> = new EventEmitter<any>();
  @Output() formKeydown: EventEmitter<any> = new EventEmitter<any>()
  @Output() optionSelectedEvent: EventEmitter<any> = new EventEmitter<any>()
  @Output() showCurrencyFieldsEvent: EventEmitter<boolean> = new EventEmitter<boolean>()
  @Output() expandedRowIndexEvent: EventEmitter<any> = new EventEmitter<any>()

  @Output() exportData: EventEmitter<any> = new EventEmitter<any>();
  @Output() includeOnlySelectedItemsFilterEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() selectedItemsFilterForPrintEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() excludeSelectedItemsFilterEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() restorePreviousFilterEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() removeAllFiltersAndSortsEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() refreshEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  @Output() tableConfigurationsChangeEvent = new EventEmitter<any>();
  @Output() advancedFilterEvent = new EventEmitter<boolean>();
  displayedColumns!: Column[];
  virtualItems: undefined[] = [];
  showColumns!: Column[];
  printColumns!: Column[];
  groupsColumns!: groupColumn[];
  printGroupsColumns!: groupColumn[];
  groupsRemainingColumns!: groupColumn[];
  printGroupsRemainingColumns!: groupColumn[];
  expandedRowIndex: number | null = null;
  viewMode = viewMode.group;
  selectedGroupColumns: string[] = ['accountHeadCode', 'referenceCode_1'];  //[ 'accountHeadCode', 'referenceName_1'];
  //toolBar

  tableRowsClone: any[] = [];

  dataSlice: any[] = [];
  dataStream$: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
  listRange!: ListRange;
  defaultWidth = 10;


  fieldTypes = TableColumnDataType;
  //RowSpan
  headerRowSpan = 1;
  footerRowSpan = 1;
  // select
  selectedRowIds: any[] = []
  selectAllItem = false;
  indeterminate = false;
  scrollActive = false;
  //sort
  rowsNeedSorting = false;
  //
  excludedItemsLocal!: any[];
  includedItemsLocal!: any[];
  excludedItemsFilter!: any[];
  includedItemsFilter!: any[];
//

//
  labelCheckboxTooltip =
    "انتخاب (Alt & +)  \n" +
    "عدم انتخاب (Alt & -) \n" +
    "انتخاب همه (A & Alt)  \n" +
    "عدم انتخاب همه (Delete & Alt)\n" +
    "اسکرول به سطر بعدی (A & ↓)  \n" +
    "اسکرول به سطر قبلی (A & ↑)  \n" +
    "نمایش کامل سطر (A & →)  \n" +
    "اسکرول به سطر اول (page Up & Alt)  \n" +
    "اسکرول به سطر آخر (page Down & Alt)  \n";
  //changes Dictionary
  private changesDictionary: { [key: string]: any[] } = {
    excludedItemsLocal: [],
    includedItemsLocal: []
  };
  private changesDictionaryFilterAndSortKeys: { [key: string]: any[] } = {
    useBuiltInPagination: [],
    filtersRow_TextInput: [],
    filtersRow_NumberInput: [],
    filtersRow_Date: [],
    filtersRow_NgSelect: [],
    filters: [],
    sortKeys: [],
    excludedItemsFilter: [],
    includedItemsFilter: [],
  };

//changeOrder
  changeOrder: string[] = [];
  changeOrderFilterAndSortKeys: string[] = [];
  //FormGroup

  filterFormDate: FormGroup | any;
  filterFormNumberInput: FormGroup | any;
  filterFormTextInput: FormGroup | any;
  filterFormNgSelect: FormGroup | any;

//

  // ng SelectModule
  customNoItemsText: string = 'موردی یافت نشد'
  fieldFilterTypes = TypeFilterOptions;
  searchTerm$ = new Subject<{ term: string; col: Column }>();
  filterConditionsNgSelect: { [key: string]: any } = {};
  filterConditionsNumberInput: { [key: string]: any } = {};
  filterConditionsTextInput: { [key: string]: any } = {};
  filterConditionsDate: { [key: string]: any } = {};

  // windowEventListeners
  public componentId!: string;
  public windowEventListeners: any[] = [];
  selectedRowIndex: number = 0;

  constructor(private cdr: ChangeDetectorRef,
              private tableSettingsService: TableSettingsService,
              public tabManagerService: TabManagerService,
              private renderer: Renderer2,
              private toastr: Toastr_Service,
              private customDecimalPipe: CustomDecimalPipe,
              private tableHelper: TableVirtualScrollHelperService,
              private generateExcel: GenerateExcelService
  ) {
    this.componentId = this.tabManagerService.activeTab.guid

  }

  async ngOnChanges(changes: SimpleChanges) {
    if (changes['tableRows']) {

      if (this.tableRows.length > 0 && this.tableConfigurations.options.isExternalChange) {
        if (this.tableConfigurations.options.resetFiltersLocal) {
          this.displayedColumns.forEach(column => {
            if (column.filter) {
              column.filter?.setDefaultFilter();
              column.isFiltered = column.filter ? false : column.isFiltered ?? false;
            }
          })
        }
        this.tableConfigurations.highlightedRow = null;
        this.expandedRowIndex = null;
        this.unSelectAll()
        this.tableRowsClone = this.tableRows;
        this.tableRows = this.CheckBuiltInDataManipulation(this.tableRows);
        this.updateTableRows();
        this.sortDataSliceByIndex();
        this.createVirtualItems(this.tableRows.length);
        this.updateSelectAll();
        this.resetViewport();
        this.cdr.detectChanges();

      }
      this.tableConfigurations.highlightedRow = null;
      this.expandedRowIndex = null;
      this.unSelectAll()
    }
    if (changes['showCurrencyFields']) {
      this.showCurrencyRelatedFields(this.showCurrencyFields)
    }
    if (changes['requestsIndex']) {
      const previousIndex = changes['requestsIndex'].previousValue ?? 0;
      const currentIndex = changes['requestsIndex'].currentValue ?? 0;
      if (currentIndex > previousIndex && (currentIndex != 0)) {
        if (this.tableConfigurations.options.useBuiltInPagination) {
          this.requestsIndex = currentIndex;
          if (this.requestsIndex) {
            this.addChangeFilterAndSortKeys('useBuiltInPagination', this.requestsIndex);
          }

        }
      }
    }
    if (changes['tableConfigurations']) {
      await this.loadSavedSettings();
      this.getGroupRow();
      this.getGroupRemainingRow();


    }

  }

  async ngOnInit() {
    this.tableConfigurationsClone = JSON.parse(JSON.stringify(this.tableConfigurations));
    this.initializeFilterForm();
    await this.resolve();
    this.LoadSearchTerm();
  }


  ngAfterViewInit(): void {

    this.renderedRangeStream();
    this.cdr.detectChanges();  // برای اطمینان از اینکه تغییرات اعمال می‌شود
  }


  isScrollActive(): boolean {
    // if (!this.cdkViewport) {
    //   return false;
    // }
    // const element = this.cdkViewport.elementRef.nativeElement;
    // return element.scrollHeight > element.clientHeight;
    return true
  }

  getToolbarWidth(): string {
    let baseWidth = 10;
    // if(this.isScrollActive()){
    //   baseWidth = 10;
    // }

    return `calc(100% - ${baseWidth}px)`;
  }

  createVirtualItems(len: number = 100) {
    this.virtualItems = Array.from({length: len});
  }

  async loadSavedSettings(): Promise<void> {
    try {
      const savedSettings = await this.tableSettingsService.getSettings(window.location.pathname, this.tableConfigurations);
      if (savedSettings) {
        // update columns , options
        this.displayedColumns = [...savedSettings.columns];
        this.tableConfigurations.columns = [...savedSettings.columns];
        this.tableConfigurations.options = savedSettings.options;

        // send event to parent component
        this.tableConfigurationsChangeEvent.emit({
          columns: this.displayedColumns,
          options: this.tableConfigurations.options
        });
      }

    } catch (error) {
      console.error('Error loading settings:', error);
    }
    this.tableRows = this.CheckBuiltInDataManipulation(this.tableRows);
    this.updateTableRows();
    this.sortDataSliceByIndex();
    this.getStyleCell();
    this.getShowColumns();


  }

  initializeFilterForm() {
    this.filterFormTextInput = new FormGroup({});
    this.filterFormNumberInput = new FormGroup({});
    this.filterFormDate = new FormGroup({});
    this.filterFormNgSelect = new FormGroup({});
    this.tableConfigurations?.columns.forEach((column: any) => {
      if (column.typeFilterOptions == TypeFilterOptions.TextInputSearch) {
        this.filterFormTextInput.addControl(column.field, new FormControl(null));
      }
      if (column.typeFilterOptions == TypeFilterOptions.NumberInputSearch) {
        this.filterFormNumberInput.addControl(column.field, new FormControl(null));
      }
      if (column.typeFilterOptions == TypeFilterOptions.Date) {
        this.filterFormDate.addControl(column.field, new FormControl(null));
      }
      if (column.typeFilterOptions == TypeFilterOptions.NgSelect) {
        this.filterConditionsNgSelect[column.field] = {
          propertyNames: column.field,
          searchValues: null
        };
      }
    });
    // this.initializeFormWatchers();
  }

  initializeFormWatchers() {
    this.monitorTextInputFormChanges();
    this.monitorNumberInputFormChanges();
    this.monitorDateFormChanges();
  }

  monitorTextInputFormChanges() {
    let delayTimer: any;
    this.filterFormTextInput.valueChanges.subscribe((formValues: { [key: string]: any }) => {
      if (delayTimer) {
        clearTimeout(delayTimer);
      }
      delayTimer = setTimeout(() => {
        for (const field in formValues) {
          const value = formValues[field];
          if ((!value || value === '') && this.filterConditionsTextInput[field]) {
            // this.filterConditionsTextInput[field] = {
            //   propertyNames: field,
            //   searchValues: [null],
            //   searchCondition: 'contains',
            //   nextOperand: 'and'
            // };
            this.applyTextInputSearchFilters(field);
          }
        }
      }, 2000);
    });
  }

  monitorNumberInputFormChanges() {
    let delayTimer: any;
    this.filterFormNumberInput.valueChanges.subscribe((formValues: { [key: string]: any }) => {
      if (delayTimer) {
        clearTimeout(delayTimer);
      }
      delayTimer = setTimeout(() => {
        for (const field in formValues) {
          const value = formValues[field];
          if ((!value || value === '') && this.filterConditionsNumberInput[field]) {
            // this.filterConditionsNumberInput[field] = {
            //   propertyNames: field,
            //
            //   searchValues: [null],
            //   searchCondition: 'equal',
            //   nextOperand: 'and'
            // };
            this.applyNumberInputSearchFilters(field);
          }
        }
      }, 2000);
    });
  }

  monitorDateFormChanges() {
    let delayTimer: any;
    this.filterFormDate.valueChanges.subscribe((formValues: { [key: string]: any }) => {
      if (delayTimer) {
        clearTimeout(delayTimer);
      }
      delayTimer = setTimeout(() => {
        for (const field in formValues) {
          const value = formValues[field];
          if ((!value || value === '') && this.filterConditionsDate[field]) {

            this.updateDatepickerFilters(field);
          }
        }
      }, 2000);
    });
  }


  async resolve() {
    await this.loadSavedSettings();
    this.displayedColumns = this.tableConfigurations.columns;
    this.SetColumnsDefaultValuesIfNotAlreadySet()

    this.displayedColumns = this.sortColumnsByIndex(this.displayedColumns);
    this.getShowColumns();
    this.getGroupRow();
    this.getGroupRemainingRow();
    this.updateShowCurrencyRelatedFields();
    this.renderedRangeStream();
    this.addShortKeys();
  }

  renderedRangeStream() {

    this.cdkViewport.renderedRangeStream.subscribe((range: ListRange) => {

      this.listRange = range;
      this.updateDataSlice()

    });
  }

  //
  // group-cell-header
  getGroupRow() {
    if (!this.tableConfigurations.options.showGroupRow) {
      return
    }
    const groups = this.getUniqueGroups(this.showColumns);
    this.groupsColumns = this.mergeConsecutiveGroups(groups);
    const printGroups = this.getUniqueGroups(this.printColumns, 'print');
    this.printGroupsColumns = this.mergeConsecutiveGroups(printGroups);

  }

  mergeConsecutiveGroups(groups: any[]): any[] {
    if (!groups || groups.length === 0) return [];
    const mergedGroups: any[] = [];
    let currentGroup = {...groups[0]};
    for (let i = 1; i < groups.length; i++) {
      const group = groups[i];
      if (group.groupId === currentGroup.groupId) {
        currentGroup.colspan += group.colspan;
        currentGroup.width = `${parseFloat(currentGroup.width) + parseFloat(group.width)}%`;
      } else {
        mergedGroups.push(currentGroup);
        currentGroup = {...group};
      }
    }
    mergedGroups.push(currentGroup);
    return mergedGroups;
  }

  getUniqueGroups(columns: any[], mode: 'display' | 'print' = 'display'): any[] {
    const groups: any[] = [];
    let lastGroupId: string | null = null;
    let lastIndex: number | null = null;
    let count = 0;

    columns
      .filter((col) => col[mode])
      .forEach((col, idx, arr) => {

        col.groupId = col.groupId ?? 'empty';
        col.groupName = col.groupName ?? '';

        if (
          col.groupId !== lastGroupId ||
          (lastIndex !== null && col.index !== lastIndex + 1)
        ) {

          if (count > 0 && lastGroupId !== null) {
            const groupColumns = columns.filter(
              (groupCol) =>
                groupCol.groupId === lastGroupId &&
                groupCol[mode] &&
                groupCol.index >= (lastIndex !== null ? lastIndex - count + 1 : 0) &&
                groupCol.index <= (lastIndex ?? 0)
            );
            const totalWidth = columns
              .filter((c) => c[mode])
              .reduce((sum, col) => sum + (col.width ?? this.defaultWidth), 0);
            const groupWidth = groupColumns.reduce(
              (sum, col) => sum + (col.width ?? this.defaultWidth),
              0
            );

            groups.push({
              groupId: lastGroupId,
              groupName: groupColumns[0]?.groupName ?? "",
              colspan: count,
              width: `${(groupWidth / totalWidth) * 100}%`,
            });
          }

          count = 0;
        }

        count++;
        lastGroupId = col.groupId;
        lastIndex = col.index;

        if (idx === arr.length - 1 && count > 0) {
          const groupColumns = columns.filter(
            (groupCol) =>
              groupCol.groupId === lastGroupId &&
              groupCol[mode] &&
              groupCol.index >= (lastIndex !== null ? lastIndex - count + 1 : 0) &&
              groupCol.index <= (lastIndex ?? 0)
          );
          const totalWidth = columns
            .filter((c) => c[mode])
            .reduce((sum, col) => sum + (col.width ?? this.defaultWidth), 0);
          const groupWidth = groupColumns.reduce(
            (sum, col) => sum + (col.width ?? this.defaultWidth),
            0
          );

          groups.push({
            groupId: lastGroupId,
            groupName: groupColumns[0]?.groupName ?? "",
            colspan: count,
            width: `${(groupWidth / totalWidth) * 100}%`,
          });
        }
      });

    return groups;
  }

  // group-cell-remaining
  getGroupRemainingRow() {
    if (!this.tableConfigurations.options.showGroupRemainingRow) {
      return
    }
    this.groupsRemainingColumns = this.getUniqueRemainingGroups(this.showColumns);
    this.printGroupsRemainingColumns = this.getUniqueRemainingGroups(this.printColumns, 'print');

  }

  getUniqueRemainingGroups(columns: any[], mode: 'display' | 'print' = 'display'): any[] {
    const groups: any[] = [];
    let lastGroup: any | null = null;

    const filteredColumns = columns.filter((col) => col[mode]);
    const totalWidth = filteredColumns.reduce((sum, col) => sum + (col.width ?? this.defaultWidth), 0);

    filteredColumns.forEach((col) => {
      col.groupRemainingId = col.groupRemainingId ?? 'empty';
      col.groupRemainingNameOrFn = col.groupRemainingNameOrFn ?? '';

      if (lastGroup && lastGroup.groupId === col.groupRemainingId) {
        lastGroup.colspan++;
        lastGroup.totalGroupWidth += col.width ?? this.defaultWidth;
        lastGroup.width = `${(lastGroup.totalGroupWidth / totalWidth) * 100}%`;
      } else {
        if (lastGroup) {
          groups.push(lastGroup);
        }
        lastGroup = {
          groupId: col.groupRemainingId,
          groupName: col.groupRemainingNameOrFn,
          colspan: 1,
          totalGroupWidth: col.width ?? this.defaultWidth,
          width: `${((col.width ?? this.defaultWidth) / totalWidth) * 100}%`,
        };
      }
    });

    if (lastGroup) {
      groups.push(lastGroup);
    }

    return groups;
  }

  //
  SetColumnsDefaultValuesIfNotAlreadySet() {
    this.displayedColumns.forEach(column => {
      column.display = column.display ?? true;
      column.width = column.width ?? 10;
      column.fontSize = column.fontSize ?? 20;
      column.matTooltipDisabled = column.matTooltipDisabled ?? true;
      column.isSorted = column.isSorted ?? false;
      column.isFiltered = column.isFiltered ?? false;
      column.isHovered_TH = column.isHovered_TH ?? false;
      column.lineStyle = column.lineStyle ?? 'onlyShowFirstLine'
      column.print = column.print ?? true;
      // column.filter = column.filter ?? new TableColumnFilter(column.field ?? '', this.mapDataTypeToFilterType(column.type));
    });

  }

  mapDataTypeToFilterType(dataType: TableColumnDataType): TableColumnFilterTypes {
    switch (dataType) {
      case TableColumnDataType.Text:
        return TableColumnFilterTypes.Text;
      case TableColumnDataType.Number:
        return TableColumnFilterTypes.Number;
      case TableColumnDataType.Money:
        return TableColumnFilterTypes.Money;
      case TableColumnDataType.Date:
        return TableColumnFilterTypes.Date;
      case TableColumnDataType.Select:
        return TableColumnFilterTypes.Select;
      case TableColumnDataType.CheckBox:
        return TableColumnFilterTypes.CheckBox;
      case TableColumnDataType.DropDown:
        return TableColumnFilterTypes.DropDown;
      case TableColumnDataType.AutoComplete:
        return TableColumnFilterTypes.AutoComplete;
      default:
        return TableColumnFilterTypes.Text;
    }
  }


  showCurrencyRelatedFields(show: boolean) {

    this.displayedColumns = this.tableConfigurations.columns.map(column => {
      if (column.isCurrencyField) {
        column.display = show;
      }
      return column;
    });
    this.getStyleCell();
    this.getShowColumns();
    this.getGroupRow();
    this.getGroupRemainingRow();

  }

  updateShowCurrencyRelatedFields() {
    this.showCurrencyFields = this.displayedColumns.filter(column => column.isCurrencyField).some(column => column.display);
    this.showCurrencyFieldsEvent.emit(this.showCurrencyFields);
  }

  CheckBuiltInDataManipulation(tableRows: any[]): any[] {
    if (!tableRows || tableRows.length === 0) {
      return [];
    }
    let rows: any[] = tableRows;

    if (this.tableConfigurations?.options?.useBuiltInSorting) this.rowsNeedSorting = true;
    if (this.tableConfigurations?.options?.useBuiltInSorting) rows = this.getSortedRows(rows);
    if (this.tableConfigurations?.options?.useBuiltInFilters) rows = this.getFilteredRows(rows);
    if (this.tableConfigurations?.options?.useBuiltInPagination) rows = this.getPaginatedRows(rows);
    this.createVirtualItems(rows.length);

    return rows;
  }


  private updateTableRows() {

    this.tableRows = this.updateTableRowsIndex(this.tableRows, true);
    this.tableRows = this.setSelectedItems(this.tableRows, false)
  }

  groupByColumn(columnField: string): any[] {
    if (!columnField) return [];
    const column = this.displayedColumns.find(col => col.field === columnField);
    let columnTitle = columnField
    if (column) {
      column.display = false;
      columnTitle = column.title;
    }
    this.getStyleCell();
    this.getShowColumns();
    this.getGroupRow();
    this.getGroupRemainingRow();


    const groupedData = this.tableRows.reduce((acc, row) => {
      const groupKey = row[columnField];
      if (!acc[groupKey]) {
        acc[groupKey] = {groupKey, columnField, groupName: columnTitle, colspan: this.showColumns.length, items: []};
      }
      acc[groupKey].items.push(row);
      return acc;
    }, {});
    this.createVirtualItems(Object.values(groupedData).length);
    return Object.values(groupedData);
  }

  groupByColumns(columns: string[], level: number = 1, data: any[] = this.tableRows): any[] {
    if (!data) return data;

    if (columns.length === 0) {
      return [{
        groupKey: `grouped_${level}`,
        groupName: '',
        level,
        colspan: this.showColumns.length,
        items: data,
        viewMode: 'default',
        toggleExpandedKey: `grouped_${level}`,
        expanded: false,
      }];
    }

    const currentColumn = columns[0];
    const remainingColumns = columns.slice(1);

    const groupedData = data.reduce((acc, row) => {
      const groupKey = row[currentColumn] ?? currentColumn;
      if (!acc[groupKey]) {
        acc[groupKey] = {
          groupKey,
          groupName: this.getColumnTitle(currentColumn),
          level,
          colspan: this.showColumns.length,
          items: [],
          viewMode: 'group',
          toggleExpandedKey: `${groupKey}_${level}`,
          expanded: true,
        };
      }
      acc[groupKey].items.push(row);
      return acc;
    }, {});

    Object.values(groupedData).forEach((group: any) => {
      if (remainingColumns.length > 0) {
        group.items = this.groupByColumns(remainingColumns, level + 1, group.items);
      }
      group.expanded = !!group.items[0]?.groupKey;


    });
    this.getStyleCell();
    this.getShowColumns();
    this.getGroupRow();
    this.getGroupRemainingRow();
    return Object.values(groupedData);
  }


  getColumnTitle(field: string): string {
    const column = this.displayedColumns.find(col => col.field === field);
    if (column) column.display = false;
    return column ? column.title : field;
  }

  updateDataSlice() {
    const range = this.listRange;

    // const groupedData = this.groupByColumns(this.selectedGroupColumns); // ستون انتخاب‌شده برای گروه‌بندی
    // this.dataSlice = groupedData.slice(range.start, range.end);
    this.dataSlice = this.tableRows.slice(range.start, range.end);
    this.dataStream$.next(this.dataSlice);

  }

  sortColumnsByIndex(columns?: Column[]): Column[] {
    if (!columns) {
      return [];
    }
    return columns.sort((a, b) => (a.index ?? 0) - (b.index ?? 0));
  }

  sortDataSliceByIndex() {
    this.dataSlice.sort((a, b) => {
      const indexA = this.displayedColumns.findIndex(column => column.field === a.field);
      const indexB = this.displayedColumns.findIndex(column => column.field === b.field);
      return indexA - indexB;
    });
    this.dataStream$.next(this.dataSlice);
  }


  getVisibleColumns(): Column[] {
    if (!this.displayedColumns) return [];
    return this.displayedColumns?.filter(column => column.display && column.type !== this.fieldTypes.ForPrint);
  }

  getVisibleColumnsPrint(): Column[] {
    if (!this.displayedColumns) return [];
    const excludedTypes = new Set([
      this.fieldTypes.Select,
      this.fieldTypes.Action,
      this.fieldTypes.Group,
      this.fieldTypes.Tree,
      this.fieldTypes.ExpandRowWithTemplate,
    ]);
    return this.displayedColumns?.filter(column => column.print && !excludedTypes.has(column.type));
  }

  getShowColumns() {

    this.showColumns = this.getVisibleColumns()


    this.printColumns = this.getVisibleColumnsPrint()
      .filter(col => col.print)
      .map(col => ({...col}));
    this.printColumns = this.applyColumnConfigurations(this.printColumns, this.tableConfigurations);
    this.getPrintStyleCell();
  }

  applyColumnConfigurations(columns: Column[], tableConfigurations: TableScrollingConfigurations): Column[] {
    const columnMap = new Map(tableConfigurations.columns.map(col => [col.field, col]));

    return columns.map(column => {
      const originalColumn = columnMap.get(column.field);
      if (originalColumn) {
        return {
          ...column,  // حفظ سایر ویژگی‌های ستون
          sumRowDisplayFn: originalColumn.sumRowDisplayFn,
          filterOptionsFunction: originalColumn.filterOptionsFunction,
          displayFunction: originalColumn.displayFunction,
          displayFn: originalColumn.displayFn,
          asyncOptions: originalColumn.asyncOptions,
          filterOptionsFn: originalColumn.filterOptionsFn,
          groupRemainingNameOrFn: originalColumn.groupRemainingNameOrFn,
          filter: originalColumn.filter,
          template: originalColumn.template,
          expandRowWithTemplate: originalColumn.expandRowWithTemplate
        };
      }
      return column;
    });
  }

  getColumnWidth(column: any, percentWith: number = 100): string {
    const visibleColumns = this.getVisibleColumns();


    if (visibleColumns.length === 0) {
      return '0%';
    }

    const totalWidth = visibleColumns.reduce((sum, col) => sum + (col.width ?? this.defaultWidth), 0);


    return `${((column.width ?? this.defaultWidth) / totalWidth) * percentWith}%`;
  }


//sort
  getColumnCurrentSortDirection(column: Column) {
    return this.tableConfigurations.sortKeys.find(x => x.includes(<string>column.field))?.split(' ')[1]?.toLowerCase();
  }

  getColumnCurrentSortIndex(column: Column): number {
    return this.tableConfigurations.sortKeys.findIndex(x => x.includes(<string>column.field)) + 1;
  }

  handleSort(column: Column) {
    if (!column.sortable) {
      return
    }
    let _key = this.tableConfigurations.sortKeys.find(x => x.includes(<string>column.field));

    if (_key) {

      if (_key.includes('DESC')) {
        this.tableConfigurations.sortKeys[this.tableConfigurations.sortKeys.indexOf(_key)] = column.field + ' ASC';
        this.addChangeFilterAndSortKeys('sortKeys', column.field + ' ASC')
      } else if (_key.includes('ASC')) {
        column.isSorted = !column.isSorted;
        this.tableConfigurations.sortKeys.splice(this.tableConfigurations.sortKeys.indexOf(_key), 1)
      }
    } else {
      column.isSorted = true;
      this.addChangeFilterAndSortKeys('sortKeys', column.field + ' DESC')
      this.tableConfigurations.sortKeys.push(column.field + ' DESC');

    }

    if (this.tableConfigurations.options.useBuiltInSorting) {
      this.rowsNeedSorting = true;
      this.tableRows = this.CheckBuiltInDataManipulation(this.tableRowsClone);
      this.resetViewport();
      this.updateTableRows();
      this.sortDataSliceByIndex();
    } else {
      this.sortChanged.emit(this.tableConfigurations.sortKeys)
    }
  }

  getSortedRows(rows: any[]): any[] {
    let sortedRows = [...rows];
    let defaultSort: string;

    if (this.rowsNeedSorting && this.displayedColumns) {
      let sortKeys = this.tableConfigurations.sortKeys;
      const defaultSortedColumn = this.displayedColumns.find(
        x => x.field === this.tableConfigurations.options.defaultSortKey
      );

      if (sortKeys.length === 0 && this.tableConfigurations.options.hasDefaultSortKey) {
        this.tableConfigurations.options.hasDefaultSortKey = false;
        defaultSort = [
          this.tableConfigurations.options.defaultSortKey,
          this.tableConfigurations.options.defaultSortDirection,
        ].join(' ');
        sortKeys.push(defaultSort);

        if (defaultSortedColumn) defaultSortedColumn.isSorted = true;
      }

      // اعمال مرتب‌سازی چندگانه
      if (sortKeys.length > 0) {
        sortedRows = sortedRows.sort((a, b) => {
          for (const sort of sortKeys) {
            const [sortKey, sortDirection] = sort.split(' ');
            const x = a[sortKey];
            const y = b[sortKey];

            if (x < y) return sortDirection === 'ASC' ? -1 : 1;
            if (x > y) return sortDirection === 'ASC' ? 1 : -1;
          }
          return 0; // اگر مقادیر برای تمامی کلیدها برابر بودند
        });

        // بروزرسانی ستون مرتب‌شده
        sortKeys.forEach(sort => {
          const sortKey = sort.split(' ')[0];
          if (sortKey !== this.tableConfigurations.options.defaultSortKey) {
            this.addChangeFilterAndSortKeys('sortKeys', sort);
          }
          const column = this.displayedColumns.find(col => col.field === sortKey);
          if (column) column.isSorted = true;
        });
      }

      this.rowsNeedSorting = false;
    }

    return sortedRows;
  }


  //filter
  getColumnCurrentFilterIndex(column: Column): number {
    return this.tableConfigurations.filters.findIndex(x => x.columnName === column.field) + 1;
  }

  getColumnFilterOperands(column: Column) {
    if (column.filter && column.filter.filterType) {
      // @ts-ignore
      return TableColumnFilterOperands.filter(x => x.dataTypes.includes(column.filter.filterType));
    }
    return [];


  }

  getFilteredRows(rows: any[]): any[] {

    if (this.tableConfigurations?.filters?.length > 0) {
      let filteredRows = [...rows];
      this.tableConfigurations.filters.forEach((filter, index) => {
        const column = this.displayedColumns.find(x => x.field === filter.columnName);
        let currentFilterRows = [...filteredRows];
        switch (filter.searchCondition) {
          case 'equal':
            currentFilterRows = filteredRows.filter((x: any) => {
              const rawValue = x[filter.columnName];
              const displayValue = filter.useDisplayFnForFilter && column?.displayFn
                ? column.displayFn(rawValue)
                : rawValue;

              // تبدیل به عدد برای مقایسه دقیق
              const numValue = this.parseNumberWithFormat(displayValue);
              const numSearch = this.parseNumberWithFormat(filter.searchValue);

              // مقایسه عددی اگر هر دو معتبر باشند
              if (!isNaN(numValue) && !isNaN(numSearch)) {
                return numValue === numSearch;
              }

              // مقایسه متنی اگر تبدیل به عدد ناموفق بود
              return this.safeToString(displayValue) === this.safeToString(filter.searchValue);
            });
            break;
          case 'notEqual':
            currentFilterRows = filteredRows.filter((x: any) => {
              const rawValue = x[filter.columnName];
              const displayValue = filter.useDisplayFnForFilter && column?.displayFn
                ? column.displayFn(rawValue)
                : rawValue;

              const numValue = this.parseNumberWithFormat(displayValue);
              const numSearch = this.parseNumberWithFormat(filter.searchValue);

              // مقایسه عددی اگر هر دو عدد معتبر باشند
              if (!isNaN(numValue) && !isNaN(numSearch)) {
                return numValue !== numSearch;
              }

              // مقایسه متنی اگر عدد نبودند
              return this.safeToString(displayValue) !== this.safeToString(filter.searchValue);
            });
            break;
          case 'between':
            currentFilterRows = filteredRows.filter((x: any) =>
              +x[filter.columnName] >= +filter.multipleSearchValues[0] &&
              +x[filter.columnName] <= +filter.multipleSearchValues[1]
            );
            break;
          case 'in':
            currentFilterRows = filteredRows.filter((x: any) =>
              filter.multipleSearchValues.includes(x[filter.columnName])
            );
            break;
          case 'greaterThan':
            currentFilterRows = filteredRows.filter((x: any) => +x[filter.columnName] > +filter.searchValue);
            break;
          case 'lessThan':
            currentFilterRows = filteredRows.filter((x: any) => +x[filter.columnName] < +filter.searchValue);
            break;
          case 'contains':
            currentFilterRows = filteredRows.filter((x: any) => {
              const value = filter.useDisplayFnForFilter && column?.displayFn
                ? column.displayFn(x[filter.columnName])?.toString()
                : x[filter.columnName]?.toString()?.toLowerCase();
              return value?.includes(filter.searchValue.toLowerCase());
            });
            break;
          case 'notContains':
            currentFilterRows = filteredRows.filter((x: any) => {
              // دریافت مقدار خام و تبدیل به رشته
              const rawValue = x[filter.columnName];

              // استفاده از displayFn اگر فعال باشد
              const displayValue = filter.useDisplayFnForFilter && column?.displayFn
                ? column.displayFn(rawValue)
                : rawValue;

              // تبدیل به رشته استاندارد با مدیریت null/undefined
              const stringValue = this.safeToString(displayValue);
              const searchTerm = this.safeToString(filter.searchValue);

              // بررسی عدم وجود
              return !stringValue.includes(searchTerm);
            });
            break;

          default:
            console.warn(`Unsupported search condition: ${filter.searchCondition}`);
        }

        // ترکیب نتایج بر اساس nextOperand
        if (index > 0 && filter.nextOperand === 'or') {
          filteredRows = [...new Set([...filteredRows, ...currentFilterRows])]; // ترکیب با OR
        } else {
          filteredRows = currentFilterRows; // ترکیب با AND (پیش‌فرض)
        }
      });

      return filteredRows;
    } else {
      // بازگشت به آرایه اصلی در صورت نبود فیلتر
      return rows;
    }
  }

// تابع کمکی برای تبدیل ایمن به رشته
  private safeToString(value: any): string {
    if (value === null || value === undefined) return '';

    // اگر عدد فرمت‌دار بود، ابتدا به عدد تبدیل سپس به رشته برگردان
    const numValue = this.parseNumberWithFormat(value);
    if (!isNaN(numValue)) {
      return numValue.toString();
    }

    return String(value).trim().toLowerCase();
  }

  private parseNumberWithFormat(value: any): number {
    if (value === null || value === undefined) return NaN;

    // تبدیل به رشته و حذف کاراکترهای غیرعددی (مثل کاما، واحد پولی و ...)
    const stringValue = String(value)
      .replace(/[^\d.-]/g, '') // حذف همه کاراکترها به جز اعداد، نقطه و علامت منفی
      .trim();

    // تبدیل به عدد
    return parseFloat(stringValue) || NaN;
  }

  updateFilters(menuTrigger?: MatMenuTrigger) {
    this.tableConfigurations.filters = [];
    this.displayedColumns.forEach(column => {
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
          this.addChangeFilterAndSortKeys('filters', filter)

          this.tableConfigurations.filters.push(filter)
        }

      })
    })

    this.tableConfigurations.pagination.pageIndex = 0;
    if (this.tableConfigurations.options.useBuiltInFilters) {
      this.tableRows = this.CheckBuiltInDataManipulation(this.tableRowsClone);
      this.resetViewport();
      this.updateTableRows();
      this.sortDataSliceByIndex();
    } else {

      this.filtersChanged.emit(this.tableConfigurations.filters);
    }


  }

//Paginate
  goToFirstPage() {
    this.tableConfigurations.pagination.pageIndex = 0;
    this.paginationChanged.emit()
  }

  getPaginatedRows(rows: any[]) {
    this.tableConfigurations.options.isExternalChange = false;
    let paginatedRows = rows;

    this.tableConfigurations.pagination.totalItems = paginatedRows?.length;

    let fromIndex = this.tableConfigurations.pagination.pageIndex * this.tableConfigurations.pagination.pageSize;
    let toIndex = fromIndex + this.tableConfigurations.pagination.pageSize;
    paginatedRows = paginatedRows.slice(fromIndex, toIndex);

    return paginatedRows;
  }

  handlePagination(event: any) {
    this.selectAllItem = false;
    this.indeterminate = false;
    this.tableConfigurations.pagination.pageIndex = event.pageIndex;
    this.tableConfigurations.pagination.pageSize = event.pageSize;
    if (this.includedItemsLocal?.length > 0) {
      this.tableConfigurations.includeOnlySelectedItemsLocal = [...this.tableConfigurations.includeOnlySelectedItemsLocal, ...this.includedItemsLocal];
    }
    if (this.excludedItemsLocal?.length > 0) {
      this.tableConfigurations.excludeSelectedItemsLocal = [...this.tableConfigurations.excludeSelectedItemsLocal, ...this.excludedItemsLocal];
    }
    if (this.excludedItemsFilter?.length > 0) {
      this.tableConfigurations.excludeSelectedItemsFilter = [...this.tableConfigurations.excludeSelectedItemsFilter, ...this.excludedItemsFilter];
    }
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    if (selectCol) {
      this.tableConfigurations.selectedItems = selectCol
    }
    if (this.includedItemsFilter?.length > 0) {
      this.tableConfigurations.includeOnlySelectedItemsFilter = [...this.tableConfigurations.includeOnlySelectedItemsFilter, ...this.includedItemsFilter];
    }

    if (this.tableConfigurations.options.useBuiltInPagination) {

      this.tableRows = this.CheckBuiltInDataManipulation(this.tableRowsClone);
      this.resetViewport();
      this.updateTableRows();
      this.sortDataSliceByIndex();
      return;
    } else {
      this.paginationChanged.emit(this.tableConfigurations.pagination)
    }
  }

  addShortKeys() {
    //

    let windowRenderer = this.renderer.listen('window', 'keyup', (event) => {
      if (this.tabManagerService.activeTab.guid !== this.componentId) return;
      if (event.altKey) {
        event.preventDefault();
        event.stopPropagation();
        switch (event.code.toLowerCase()) {
          case 'keys':

            event.preventDefault();
            break;
          case 'keyr':

            event.preventDefault();
            break;
          case 'keym':

            event.preventDefault();
            break;
          case 'keyn':

            break;
          case 'keyv':

            break;
          case 'keyu':
            if (this.tableConfigurations.rowToEdit != null) {

            }
            break;
          case 'keya':
            this.selectAll(true);
            this.moveToBottom();
            break;

          case 'delete':

            this.unSelectAll();
            this.moveToTop();
            break;
          case 'arrowup':
            if (this.tableConfigurations.rowToEdit) {

            } else {
              this.moveToPreviousRow();
            }
            break;
          case 'arrowdown':
            if (this.tableConfigurations.rowToEdit) {
            } else {
              this.moveToNextRow();
            }
            break;
          case 'arrowright':
            this.getStyleCell();
            break;
          case 'arrowleft':

            this.onRowDoubleClick(this.getSelectedRow());

            break;
          case 'enter': // عملکرد برای کلید اینتر
            this.onRowDoubleClick(this.getSelectedRow());
            break;
          //   numpadenter
          case 'numpadenter':
            this.onRowDoubleClick(this.getSelectedRow());
            break;
          case 'pageup': // اسکرول به بالا
            this.moveToTop();
            break;
          case 'pagedown': // اسکرول به پایین
            this.moveToBottom();
            break;
          case 'numpadadd':
            if (this.tableConfigurations.rowToEdit) {

            } else {

              this.handleNumpadAdd();

            }
            break;
          case 'numpadsubtract':
            if (this.tableConfigurations.rowToEdit) {

            } else {

              this.handleNumpadSubtract();
            }
            break;


          default:
            this.getStyleCell();
            break;

        }

      }

    })

    this.windowEventListeners.push(windowRenderer)

  }

  handleNumpadAdd() {
    this.changeStyleForMove();
    const currentIndex = this.tableRows.findIndex(row => row === this.tableConfigurations.highlightedRow);
    const hasSelectedItem = this.tableRows.some((row: any) => row.selected === true);
    if (currentIndex >= 0) {
      const nextIndex = currentIndex + 1;
      if (nextIndex < this.tableRows.length) {
        this.tableRows[currentIndex].selected = true;
        this.selectedRowIndex = nextIndex;
        this.tableConfigurations.highlightedRow = this.tableRows[nextIndex];
        this.scrollIfNotInView(this.selectedRowIndex);
      } else {
        this.tableRows[currentIndex].selected = true;
        this.selectedRowIndex = currentIndex;
        this.tableConfigurations.highlightedRow = this.tableRows[currentIndex];
        this.scrollIfNotInView(this.selectedRowIndex);
      }
    } else if (hasSelectedItem) {
      let index = this.findLastIndex(this.tableRows, (row: any) => row.selected === true);
      const nextIndex = index + 1;
      if (nextIndex < this.tableRows.length) {
        this.tableRows[nextIndex].selected = true;
        this.selectedRowIndex = nextIndex;
        this.tableConfigurations.highlightedRow = this.tableRows[nextIndex];
        this.scrollIfNotInView(this.selectedRowIndex);
      }
    } else if (this.tableRows.length > 0) {

      this.tableRows[0].selected = true;
      this.selectedRowIndex = 0;
      this.tableConfigurations.highlightedRow = this.tableRows[0];
      this.scrollIfNotInView(this.selectedRowIndex);
    }

    this.updateSelectAll();
  }

  handleNumpadSubtract() {
    this.changeStyleForMove();
    const currentIndex = this.tableRows.findIndex(row => row === this.tableConfigurations.highlightedRow);
    const hasSelectedItem = this.tableRows.some((row: any) => row.selected === true);
    if (currentIndex >= 0) {
      const prevIndex = currentIndex - 1;
      if (this.tableRows[currentIndex].selected) {
        this.tableRows[currentIndex].selected = false;
      }
      if (prevIndex >= 0) {
        if (this.tableRows[prevIndex].selected) {
          this.selectedRowIndex = prevIndex;
        } else {
          this.selectedRowIndex = this.findLastIndex(this.tableRows, (row: any) => row.selected === true)
        }
        if (this.selectedRowIndex >= 0) {

          this.tableRows[this.selectedRowIndex].selected = true;
          this.tableConfigurations.highlightedRow = this.tableRows[this.selectedRowIndex];
          this.scrollToRow(this.selectedRowIndex);
        }
      } else {
        this.selectedRowIndex = this.findLastIndex(this.tableRows, (row: any) => row.selected === true)
        if (this.selectedRowIndex >= 0) {
          this.tableRows[this.selectedRowIndex].selected = true;
          this.tableConfigurations.highlightedRow = this.tableRows[this.selectedRowIndex];
          this.scrollToRow(this.selectedRowIndex);
        }

      }
    } else if (hasSelectedItem) {
      let index = this.findLastIndex(this.tableRows, (row: any) => row.selected === true);
      const prevIndex = index - 1;
      if (index >= 0) {
        this.tableRows[index].selected = false;
      }
      if (prevIndex >= 0) {
        this.selectedRowIndex = prevIndex;
        this.tableConfigurations.highlightedRow = this.tableRows[prevIndex];
        this.scrollToRow(this.selectedRowIndex);
      }
    } else if (this.tableRows.length > 0) {
      this.tableRows[0].selected = true;
      this.selectedRowIndex = 0;
      this.tableConfigurations.highlightedRow = this.tableRows[0];
      this.scrollToRow(this.selectedRowIndex);
    }

    this.updateSelectAll();
  }


//  findLastIndex
  findLastIndex(array: any[], predicate: (value: any, index: number, obj: any[]) => boolean): number {
    for (let i = array.length - 1; i >= 0; i--) {
      if (predicate(array[i], i, array)) {
        return i;
      }
    }
    return -1;
  }

  moveToPreviousRow() {
    this.changeStyleForMove();
    const currentIndex = this.tableRows.findIndex(row => row === this.tableConfigurations.highlightedRow);
    if (currentIndex === -1) {
      this.tableConfigurations.highlightedRow = this.tableRows[0];
      this.scrollToTop();  // اسکرول به اولین آیتم
    } else if (currentIndex > 0) {
      const newIndex = currentIndex - 1;
      this.tableConfigurations.highlightedRow = this.tableRows[newIndex];
      this.scrollToRow(newIndex);
    } else {
      this.tableConfigurations.highlightedRow = this.tableRows[this.tableRows.length - 1];
      this.scrollToBottom(); // اسکرول به آخرین آیتم
    }
  }

  moveToNextRow() {
    this.changeStyleForMove();
    const currentIndex = this.tableRows.findIndex(row => row === this.tableConfigurations.highlightedRow);
    if (currentIndex === -1) {
      this.tableConfigurations.highlightedRow = this.tableRows[0];
      this.scrollToTop();
    } else if (currentIndex < this.tableRows.length - 1) {
      const newIndex = currentIndex + 1;
      this.tableConfigurations.highlightedRow = this.tableRows[newIndex];
      this.scrollIfNotInView(newIndex);
    } else {
      this.tableConfigurations.highlightedRow = this.tableRows[0];
      this.scrollToTop();
    }

  }

  changeStyleForMove() {
    this.displayedColumns.forEach(column => {
      const oStyle = {
        'width': this.getColumnWidth(column) + ' !important',
        'font-size': column.fontSize + 'px',
        'font-weight': column.fontWeight,
        'font-family': column.fontFamily + ' !important',
        'overflow': 'hidden !important',
        'white-space': 'nowrap !important',
        'text-overflow': 'ellipsis !important',
        'height': this.tableConfigurations.options.itemSize + 'px',
      };
      column.style = {
        ...column.style, ...oStyle
      };
    });
  }

  moveToTop() {
    this.changeStyleForMove();
    this.tableConfigurations.highlightedRow = this.tableRows[0];
    this.scrollToTop();
  }

  moveToBottom() {
    this.changeStyleForMove();
    this.tableConfigurations.highlightedRow = this.tableRows[this.tableRows.length - 1];
    this.scrollToBottom();
  }

  scrollIfNotInView(index: number) {
    if (!this.cdkViewport) return;
    const renderedRange = this.cdkViewport.getRenderedRange();
    if (index < renderedRange.start - 6 || index >= renderedRange.end - 6) {
      const rowHeight = this.tableConfigurations.options.itemSize || 40;
      const scrollPosition = rowHeight * index;
      this.cdkViewport.scrollToOffset(scrollPosition, 'smooth');
    }
  }

  scrollToRow(index: number) {
    if (!this.cdkViewport) return;
    let offset = 0
    const rowHeight = this.tableConfigurations.options.itemSize || 40;
    const renderedRange = this.cdkViewport.getRenderedRange();
    const rowCount = this.getVisibleRowCount() - 2;

    if (index < renderedRange.start + 5 || index >= renderedRange.end) {
      offset = rowHeight * (index - rowCount);
      this.cdkViewport.scrollToOffset(offset, 'smooth');
    }


  }


  scrollToBottom(): void {
    if (this.cdkViewport) {
      const element = this.cdkViewport.elementRef.nativeElement;
      this.renderer.setProperty(element, 'scrollTop', element.scrollHeight);
    }
  }

  scrollToTop(): void {
    if (this.cdkViewport) {
      const element = this.cdkViewport.elementRef.nativeElement;
      this.renderer.setProperty(element, 'scrollTop', 0);
    }
  }

  getVisibleRowCount(): number {
    if (!this.cdkViewport) return 0;

    const viewportHeight = this.cdkViewport.getViewportSize();
    const scrollOffset = this.cdkViewport.measureScrollOffset();
    const rowHeight = this.tableConfigurations.options.itemSize || 40;


    const firstVisibleRowIndex = Math.floor(scrollOffset / rowHeight);
    const lastVisibleRowIndex = Math.min(
      Math.ceil((scrollOffset + viewportHeight) / rowHeight),
      this.tableRows.length
    );


    const visibleRowCount = lastVisibleRowIndex - firstVisibleRowIndex;

    return visibleRowCount;
  }

  getSelectedRow() {
    if (!this.tableConfigurations.highlightedRow) {
      this.toastr.showToast({message: 'لطفا یک ردیف انتخاب کنید.', title: '', type: 'warning'});
      return
    }
    return this.tableConfigurations.highlightedRow;
  }

  //
  onRowClick(row: any) {
    this.tableConfigurations.highlightedRow = row;
    this.rowSelected.emit(row);
    this.rowClicked.emit(row);
  }

  onRowDoubleClick(row: any) {
    if (this.tableConfigurations?.options?.editRowOnDoubleClick) {
      this.tableConfigurations.changeRowToEdit(row, this.rowUpdated);
      this.selectedRowChange.emit(row)
    }
    this.rowDoubleClicked.emit(row);
  }

// selectedItemsFilterForPrintEvent
  emitSelectedItemsForPrint() {
    this.updateRowsSelected();
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    if (selectCol.length === 0) {
      if (this.tableConfigurations.options.isSelectedMode) {
        selectCol = this.tableRows.filter(x => x.id !== 0);
      } else {
        selectCol = [];
      }

    }

    let ids = selectCol.map((a: any) => a.id);
    this.selectedRowIds = ids;
    this.selectedItemsFilterForPrintEvent.emit(ids)
  }

  // cdk-virtual-scroll-viewport
  scrollIndexChanged($event: number) {

  }

  handleTableSizeChange($event: boolean) {
    this.tableConfigurations.toolBar.isLargeSize = $event;
    this.checkViewportSize()
  }

  checkViewportSize() {
    setTimeout(
      () => {
        this.cdkViewport.checkViewportSize();
      }, 0
    );
  }

  private resetViewport() {
    this.cdkViewport.scrollToIndex(0);
    if (this.listRange) {
      if (this.listRange.end < this.tableConfigurations.pagination.pageSize) {
        this.listRange = {
          start: 0,
          end: this.tableConfigurations.pagination.pageSize > 100 ? 100 : this.tableRows.length
        }
      }
      this.updateDataSlice();
    }
    this.checkViewportSize();
  }

  gettableHeight() {
    let baseHeight = 50;
    if (this.tableConfigurations.options.showFilterRow) {
      baseHeight += 40;
    }
    if (this.tableConfigurations.options.showSumRow) {
      baseHeight += 40;
    }
    if (this.tableConfigurations.options.showTopSettingMenu) {
      baseHeight += 40;
    }
    if (this.tableConfigurations.options.showGroupRow) {
      baseHeight += 40;
    }
    if (this.tableConfigurations.options.showGroupRemainingRow) {
      baseHeight += 40;
    }
    if (this.tableConfigurations?.options?.usePagination) {
      baseHeight += 65;
    }
    return {
      'height': `calc(100% - ${baseHeight}px)`
    }
      ;
  }

  trackByFn(index: number, item: any) {

    return item ? item.id || item : index;
  }

  // select
  selectAll(isChecked: boolean) {
    this.tableRows.forEach(row => {
      row.selected = isChecked;
    });
    this.emitSelectedItemsForPrint();
    this.allRowsSelected.emit();
    this.updateSelectAll();

  }

  unSelectAll() {
    this.selectAll(false);
    this.updateSelectAll();
  }


  selectItem(row: any) {
    this.emitSelectedItemsForPrint();
    this.rowSelected.emit(row);
    this.updateSelectAll();

  }

  updateRowsSelected() {
    let selectCol = this.tableRows.filter(x => x.selected === true);
    this.rowsSelected.emit(selectCol);
  }

  private updateSelectAll() {

    if (!this.tableRows || this.tableRows.length === 0) {
      this.selectAllItem = false;
      this.indeterminate = false;
      return;
    }

    this.selectAllItem = this.tableRows.every(item => item.selected);
    this.indeterminate = this.tableRows.some(item => item.selected) && !this.selectAllItem;
  }


  removeAllFiltersAndSorts($event: boolean) {

    this.displayedColumns.forEach(column => {
        let clonedColumn = this.tableConfigurationsClone.columns.find(cloneCol => cloneCol.field === column.field);
        column.isFiltered = false;
        column.isSorted = false;

        if (column.filter && typeof column.filter.setDefaultFilter === 'function') {
          column.filter?.setDefaultFilter();
        } else {
          // console.warn('filter is not defined or setDefaultFilter is not a function for column:', column);

          if (clonedColumn && clonedColumn.filter) {
            column.filter = clonedColumn.filter;
          } else {
            console.warn(`No matching filter found in tableConfigurationsClone for column: ${column.field}`);
          }
        }
      }
    )
    this.tableConfigurations.columns = this.displayedColumns
    this.tableConfigurations.filters = this.tableConfigurationsClone.filters;
    this.tableConfigurations.sortKeys = this.tableConfigurationsClone.sortKeys;
    this.tableConfigurations.options.hasDefaultSortKey = this.tableConfigurationsClone.options.hasDefaultSortKey;
    this.tableConfigurations.options.defaultSortKey = this.tableConfigurationsClone.options.defaultSortKey;
    this.tableConfigurations.options.defaultSortDirection = this.tableConfigurationsClone.options.defaultSortDirection;

    this.tableConfigurations.includeOnlySelectedItemsFilter = []
    this.tableConfigurations.excludeSelectedItemsFilter = []
    this.tableConfigurations.selectedItems = []
    this.excludedItemsFilter = [];
    this.includedItemsFilter = [];
    this.changeOrderFilterAndSortKeys = this.changeOrderFilterAndSortKeys.filter(item => item !== 'includedItemsFilter');
    this.changeOrderFilterAndSortKeys = this.changeOrderFilterAndSortKeys.filter(item => item !== 'excludedItemsFilter');
    this.requestsIndex = 0;
    this.tableConfigurations.pagination.pageIndex = 0;
    this.tableConfigurations?.columns.forEach((column: any) => {

      if (column.typeFilterOptions == TypeFilterOptions.NgSelect) {
        this.filterConditionsNgSelect[column.field] = {
          propertyNames: column.field,
          searchValues: null
        };
      }
    });
    this.changesDictionaryFilterAndSortKeys = {
      useBuiltInPagination: [],
      filtersRow_TextInput: [],
      filtersRow_NumberInput: [],
      filtersRow_Date: [],
      filtersRow_NgSelect: [],
      filters: [],
      sortKeys: [],
      excludedItemsFilter: [],
      includedItemsFilter: []
    };
    this.changeOrderFilterAndSortKeys = [];
    this.filterConditionsNumberInput = {};
    this.filterConditionsTextInput = {};
    this.filterConditionsDate = {};
    this.filterFormTextInput.reset();
    this.filterFormNumberInput.reset();
    this.filterFormDate.reset();
    this.removeAllFiltersAndSortsEvent.emit(this.tableConfigurations)
  }


  setSelectedItems(rows: any[], selected: boolean): any[] {
    if (!rows) return [];
    return rows.map(row => {
      row.selected = selected;
      return row;
    });
  }

  updateTableRowsIndex(rows: any[], usePagination ?: boolean): any[] {
    if (!rows) return [];
    if (usePagination) {
      return rows.map((row, index) => {
        row.rowIndex = index + 1 + (this.tableConfigurations.pagination.pageIndex * this.tableConfigurations.pagination.pageSize);
        return row;
      });
    }
    return rows.map((row, index) => {
      row.rowIndex = index + 1;
      return row;
    });
  }

  private removeExcludedItemsLocalFromRows(rows: any[]): any[] {
    if (!rows) return [];
    return rows.filter(row => !this.excludedItemsLocal.some(excluded => excluded.id === row.id));
  }

  public setRowsIncludedItemsLocal() {
    let rows: any[] = this.includedItemsLocal;
    if (this.excludedItemsLocal?.length > 0) rows = this.removeExcludedItemsLocalFromRows(rows)
    rows = this.getSortedRows(rows)
    rows = this.getFilteredRows(rows)
    rows = this.sortRowsByIndex(rows);
    // rows = this.updateTableRowsIndex(rows, false)
    this.tableRows = this.setSelectedItems(rows, false);
    this.createVirtualItems(this.tableRows.length);
    this.updateSelectAll();
    this.resetViewport();
    this.emitSelectedItemsForPrint();
  }

  includeOnlySelectedItemsLocal(event: boolean) {
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    this.tableConfigurations.options.isSelectedMode = true;
    this.tableConfigurations.options.showFilterRow = false;

    if (selectCol.length) {
      if (this.tableConfigurations.includeOnlySelectedItemsLocal.length > 0 || this.includedItemsLocal?.length > 0) {
        const newItems = selectCol.filter(item => !this.includedItemsLocal.some(included => included.id === item.id));
        this.includedItemsLocal = [...this.includedItemsLocal, ...newItems];
        this.addChange('includedItemsLocal', newItems);
      } else {
        this.includedItemsLocal = selectCol;
        this.addChange('includedItemsLocal', selectCol);
      }
    }
    this.setRowsIncludedItemsLocal()
  }

  excludeSelectedItemsLocal(event: boolean) {
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    if (selectCol.length) {
      if (this.tableConfigurations.excludeSelectedItemsLocal.length || this.excludedItemsLocal?.length > 0) {
        const newItems = selectCol.filter(item => !this.excludedItemsLocal.some(included => included.id === item.id));

        this.excludedItemsLocal = [...this.excludedItemsLocal, ...newItems];
        this.addChange('excludedItemsLocal', newItems);

      } else {
        this.excludedItemsLocal = selectCol;
        this.addChange('excludedItemsLocal', selectCol);
      }
    }
    this.setRowsIncludedItemsLocal()
  }

  excludeSelectedItemsFilter($event: any) {
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    if (selectCol.length) {
      if (this.tableConfigurations.excludeSelectedItemsFilter.length || this.excludedItemsFilter?.length > 0) {
        const newItems = selectCol.filter(item => !this.excludedItemsFilter.some(included => included.id === item.id));
        this.excludedItemsFilter = [...this.excludedItemsFilter, ...newItems];
        this.addChangeFilterAndSortKeys('excludedItemsFilter', newItems)
      } else {
        this.excludedItemsFilter = selectCol;
        this.addChangeFilterAndSortKeys('excludedItemsFilter', selectCol)
      }
      let ids = this.excludedItemsFilter.map((a: any) => a.id);
      this.excludeSelectedItemsFilterEvent.emit(ids)
    }


  }

  includeOnlySelectedItemsFilter($event: any) {
    let selectCol = this.tableRows.filter(x => x.selected === true && x.id !== 0);
    if (selectCol.length) {
      if (this.includedItemsFilter?.length > 0) {
        const newItems = selectCol.filter(item => !this.includedItemsFilter.some(included => included.id === item.id));
        const newItemsOtherPage = this.tableConfigurations.selectedItems.filter(item => !this.includedItemsFilter.some(included => included.id === item.id));
        this.includedItemsFilter = [...this.includedItemsFilter, ...newItems, ...newItemsOtherPage];
        this.addChangeFilterAndSortKeys('includedItemsFilter', [...newItems, ...newItemsOtherPage])
      } else {
        this.includedItemsFilter = [...selectCol, ...this.tableConfigurations.selectedItems];
        this.addChangeFilterAndSortKeys('includedItemsFilter', [...selectCol, ...this.tableConfigurations.selectedItems])
      }
      let ids = this.includedItemsFilter.map((a: any) => a.id);
      this.includeOnlySelectedItemsFilterEvent.emit(ids)
    }
  }

  restorePreviousFilter(e: any) {

    this.undoLastChangeFilterAndSortKeys()


  }

  showAll($event: any) {
    this.tableRows = this.tableRowsClone;
    this.unSelectAll();
    this.updateSelectAll();
    this.tableConfigurations.options.isSelectedMode = false;
    this.tableConfigurations.options.showFilterRow = this.tableConfigurationsClone.options.showFilterRow;
    this.selectedItemsFilterForPrintEvent.emit([])
    this.resetViewport()

  }

  handleRefresh($event: any) {
    this.refreshEvent.emit(true)
  }

  undoItemsLocal($event: any) {
    this.undoLastChange();
    this.updateDataSlice();
    this.updateSelectAll();

  }

  deleteItemsLocal($event: any) {
    this.changeOrder = [];
    this.changesDictionary = {
      excludedItemsLocal: [],
      includedItemsLocal: []
    }
    this.includedItemsLocal = [];
    this.excludedItemsLocal = [];


    this.updateDataSlice();
    this.updateSelectAll();
  }


  private addChange(type: 'excludedItemsLocal' | 'includedItemsLocal', value: any) {
    if (!this.changesDictionary[type].includes(value)) {
      this.changesDictionary[type].push(value);
      this.changeOrder.push(type);
    }
  }


  private undoLastChange() {
    if (this.changeOrder.length > 0) {
      const lastChangeType = this.changeOrder.pop();
      if (lastChangeType) {
        const lastValue = this.changesDictionary[lastChangeType].pop();

        if (lastChangeType === 'includedItemsLocal') {
          this.includedItemsLocal = this.includedItemsLocal.filter(row =>
            !lastValue.some((x: any) => x.id === row.id)
          );
          this.setRowsIncludedItemsLocal()
        }
        if (lastChangeType === 'excludedItemsLocal') {
          this.excludedItemsLocal = this.excludedItemsLocal.filter(row =>
            !lastValue.some((x: any) => x.id === row.id)
          );
          this.tableRows = this.tableRows.concat(lastValue);
          // added lastValue to tableRows and sort with rowIndex
          let rows = this.tableRows;
          // rows = this.updateTableRowsIndex(rows, false)
          rows = this.sortRowsByIndex(rows);
          rows = this.getSortedRows(rows)
          rows = this.getFilteredRows(rows)
          rows = this.setSelectedItems(rows, false);

          this.tableRows = rows;
          this.createVirtualItems(this.tableRows.length);
          this.updateSelectAll();
          this.resetViewport()
        }

      }
    }
  }

  private addChangeFilterAndSortKeys(type: 'useBuiltInPagination' | 'filtersRow_TextInput' | 'filtersRow_NgSelect' | 'filtersRow_NumberInput' | 'filtersRow_Date' | 'filters' | 'sortKeys' | 'excludedItemsFilter' | 'includedItemsFilter', value: any) {
    if (!this.changesDictionaryFilterAndSortKeys[type]) {
      this.changesDictionaryFilterAndSortKeys[type] = [];
    }
    if (!this.changesDictionaryFilterAndSortKeys[type].includes(value)) {
      this.changesDictionaryFilterAndSortKeys[type].push(value);
      this.changeOrderFilterAndSortKeys.push(type);
    }
  }


  private undoLastChangeFilterAndSortKeys() {
    if (this.changeOrderFilterAndSortKeys.length > 0) {
      const lastChangeType = this.changeOrderFilterAndSortKeys.pop(); // نوع آخرین تغییر
      if (lastChangeType) {
        const lastValue = this.changesDictionaryFilterAndSortKeys[lastChangeType].pop(); // مقدار آخرین تغییر
        if (lastChangeType === 'useBuiltInPagination') {
          this.handleUseBuiltInPaginationAndRestoreFiltersEvent('useBuiltInPagination')
        }
        if (lastChangeType === 'filters') {
          const lastFilter = this.tableConfigurations.filters.pop();


          const column = this.displayedColumns.find(column => column.field === lastFilter?.columnName);
          if (column) {
            column.isFiltered = false;
            // // @ts-ignore
            // column.filter.filters = this.tableConfigurationsClone.columns.find(cloneCol => cloneCol.field === column.field)?.filter?.filters;
          }
          this.handleUseBuiltInPaginationAndRestoreFiltersEvent(lastChangeType, column)

        }
        if (lastChangeType === 'sortKeys') {
          const lastSort = this.tableConfigurations.sortKeys.pop();
          if (lastSort) {
            const [field, sortDirection] = lastSort.split(' ');
            const column = this.displayedColumns.find(col => col.field === field);
            if (column) {
              column.isSorted = false;
            }
            this.handleUseBuiltInPaginationAndRestoreFiltersEvent(lastChangeType, column)
          }
        }
        if (lastChangeType === 'excludedItemsFilter') {
          this.excludedItemsFilter = this.excludedItemsFilter.filter(row =>
            !lastValue.some((x: any) => x.id === row.id)
          );
          this.handleUseBuiltInPaginationAndRestoreFiltersEvent()
        }
        if (lastChangeType === 'includedItemsFilter') {
          this.includedItemsFilter = this.includedItemsFilter.filter(row =>
            !lastValue.some((x: any) => x.id === row.id)
          );
          this.handleUseBuiltInPaginationAndRestoreFiltersEvent()
        }
        if (lastChangeType === 'filtersRow_NgSelect') {
          if (lastValue) {
            const propertyName = lastValue.propertyNames;
            const control = this.filterConditionsNgSelect[propertyName];
            if (control) {
              const previousValue =
                this.changesDictionaryFilterAndSortKeys[lastChangeType].length > 0
                  ? this.changesDictionaryFilterAndSortKeys[lastChangeType][
                  this.changesDictionaryFilterAndSortKeys[lastChangeType].length - 1
                    ]
                  : null;
              if (previousValue && previousValue.propertyNames?.includes(propertyName)) {
                this.filterConditionsNgSelect[propertyName] = previousValue;
              } else {
                this.filterConditionsNgSelect[propertyName].searchValues = null;
              }
            } else {
              console.warn(`کنترلی با نام ${propertyName} در فرم یافت نشد.`);
            }
          }
          if (!this.tableConfigurations.options.useBuiltInPagination) {
            this.restorePreviousFilterEvent.emit()

          } else {
            return;
          }

        }
        if (lastChangeType === 'filtersRow_TextInput') {
          if (lastValue && lastValue.propertyNames?.length > 0) {
            const propertyName = lastValue.propertyNames[0]; // فرض بر این است که اولین propertyName استفاده شود.
            const control = this.filterFormTextInput.controls[propertyName];
            if (control) {
              const previousValue =
                this.changesDictionaryFilterAndSortKeys[lastChangeType].length > 0
                  ? this.changesDictionaryFilterAndSortKeys[lastChangeType][
                  this.changesDictionaryFilterAndSortKeys[lastChangeType].length - 1
                    ]
                  : null;
              if (previousValue && previousValue.propertyNames?.includes(propertyName)) {

                const searchValue = previousValue.searchValues?.[0] || '';
                control.patchValue(searchValue);
                this.filterConditionsTextInput[propertyName].searchValues = searchValue;
                this.setRestorePreviousFilterEvent(TypeFilterOptions.TextInputSearch, propertyName, searchValue)

              } else {
                control.patchValue(null);
                this.filterConditionsTextInput[propertyName].searchValues = [null];
                this.setRestorePreviousFilterEvent(TypeFilterOptions.TextInputSearch, propertyName, null)
              }
            } else {
              console.warn(`کنترلی با نام ${propertyName} در فرم یافت نشد.`);
            }
          }
        }
        if (lastChangeType === 'filtersRow_NumberInput') {
          if (lastValue && lastValue.propertyNames?.length > 0) {
            const propertyName = lastValue.propertyNames[0]; // فرض بر این است که اولین propertyName استفاده شود.
            const control = this.filterFormNumberInput.controls[propertyName];
            if (control) {
              const previousValue =
                this.changesDictionaryFilterAndSortKeys[lastChangeType].length > 0
                  ? this.changesDictionaryFilterAndSortKeys[lastChangeType][
                  this.changesDictionaryFilterAndSortKeys[lastChangeType].length - 1
                    ]
                  : null;
              if (previousValue && previousValue.propertyNames?.includes(propertyName)) {

                const searchValue = previousValue.searchValues?.[0] || '';
                control.patchValue(searchValue);
                this.filterConditionsNumberInput[propertyName].searchValues = searchValue;
                this.setRestorePreviousFilterEvent(TypeFilterOptions.NumberInputSearch, propertyName, searchValue, 'equal')
              } else {
                control.patchValue(null);
                this.filterConditionsNumberInput[propertyName].searchValues = [null];
                this.setRestorePreviousFilterEvent(TypeFilterOptions.NumberInputSearch, propertyName, null, 'equal')
              }
            } else {
              console.warn(`کنترلی با نام ${propertyName} در فرم یافت نشد.`);
            }
          }

        }
        if (lastChangeType === 'filtersRow_Date') {
          if (lastValue && lastValue.propertyNames?.length > 0) {
            const propertyName = lastValue.propertyNames[0]; // فرض بر این است که اولین propertyName استفاده شود.
            const control = this.filterFormDate.controls[propertyName];
            if (control) {
              const previousValue =
                this.changesDictionaryFilterAndSortKeys[lastChangeType].length > 0
                  ? this.changesDictionaryFilterAndSortKeys[lastChangeType][
                  this.changesDictionaryFilterAndSortKeys[lastChangeType].length - 1
                    ]
                  : null;
              if (previousValue && previousValue.propertyNames?.includes(propertyName)) {

                const searchValue = previousValue.searchValues?.[0] || '';
                control.patchValue(searchValue);
                this.filterConditionsDate[propertyName].searchValues = searchValue;
                this.setRestorePreviousFilterEvent(TypeFilterOptions.Date, propertyName, searchValue, 'between')
              } else {
                control.patchValue(null);
                this.filterConditionsDate[propertyName].searchValues = [null];
                this.setRestorePreviousFilterEvent(TypeFilterOptions.Date, propertyName, null, 'between')
              }
            } else {
              console.warn(`کنترلی با نام ${propertyName} در فرم یافت نشد.`);
            }
          }
        }
      }
    } else {
      // if(this.tableConfigurations.options.useBuiltInPagination){
      //   this.restorePreviousFilterEvent.emit()
      //   return
      // }
      this.removeAllFiltersAndSorts(true)
      return
    }

  }

  private handleUseBuiltInPaginationAndRestoreFiltersEvent(type?: string, column?: Column): void {
    if (!this.tableConfigurations.options.useBuiltInPagination) {
      this.restorePreviousFilterEvent.emit();
    } else {
      if (type == 'filters') {
        let columnFilter = column?.filter;
        if (columnFilter && typeof columnFilter.setDefaultFilter === 'function') {
          columnFilter.setDefaultFilter();
        } else {
          console.warn('filter is not defined or setDefaultFilter is not a function for column:', columnFilter);
        }

        this.updateFilters()
      }
      if (type == 'sortKeys' && column) {
        this.rowsNeedSorting = true;
        this.tableRows = this.CheckBuiltInDataManipulation(this.tableRowsClone);
        this.resetViewport();
        this.updateTableRows();
        this.sortDataSliceByIndex();

      }
      if (type == 'useBuiltInPagination') {
        this.restorePreviousFilterEvent.emit();
      }
    }
  }

  setRestorePreviousFilterEvent(typeFilterOptions: any, propertyName: string, searchValue: any, searchCondition: string = 'contains') {
    if (!this.tableConfigurations.options.useBuiltInPagination) {
      this.restorePreviousFilterEvent.emit({
        typeFilterOptions: typeFilterOptions,
        filterConditions: {
          propertyNames: propertyName,
          searchValues: [searchValue],
          searchCondition: searchCondition,
          nextOperand: 'and'
        }

      })
    }

  }

  sortRowsByIndex(rows: any[]) {
    return rows.sort((a, b) => a.rowIndex - b.rowIndex);
  }

  onMouseOver(column: Column) {
    column.isHovered_TH = true;

  }

  onMouseLeave(column: Column) {
    column.isHovered_TH = false;

  }


  getExcludeSelectedItemsFilter() {
    return this.changeOrderFilterAndSortKeys.filter(item => item === 'excludedItemsFilter')
  }

  getCountBadgeRestorePreviousFilter() {
    if (this.tableConfigurations.options.useBuiltInPagination) {
      return this.changeOrderFilterAndSortKeys.length
    }
    return this.requestsIndex
  }

  getIncludeOnlySelectedItemsFilter() {

    return this.changeOrderFilterAndSortKeys.filter(item => item === 'includedItemsFilter');
  }

  getColumnSelectSum() {
    const selectCols = this.getSelectedItems();
    if (selectCols.length > 0) {
      return selectCols.length;
    } else {
      return this.tableRows.length;
    }
  }

  getSelectedItems() {
    return this.tableRows.filter(item => item.selected === true);
  }

  calculateColumnSum(column: Column): number {
    const selectCols = this.getSelectedItems();
    if (selectCols.length > 0) {
      column.sumColumnValue = selectCols.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0);

    } else {
      column.sumColumnValue = this.tableRows.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0)

    }
    return column.sumColumnValue || 0;


  }

  calculateColumnSumRemainingGroup(group: groupColumn): string {
    if (typeof group.groupName === 'string') {

      return group.groupName;
    } else if (typeof group.groupName === 'function') {

      const result = group.groupName();
      const numericResult = parseFloat(result);


      if (!isNaN(numericResult)) {
        const formattedValue = this.customDecimalPipe.transform(
          numericResult,
          'default',
        );

        if (numericResult < 0) {
          return `${formattedValue}`;
        }
        return formattedValue || '';
      }

      return result;
    }


    return 'Unknown';
  }

  async handleTableConfigurationsChange(config: any) {

    this.tableConfigurations.options = config.options;


    this.displayedColumns = this.sortColumnsByIndex(config.columns);
    this.tableConfigurations.columns = this.sortColumnsByIndex(config.columns);
    this.updateShowCurrencyRelatedFields()

    this.SetColumnsDefaultValuesIfNotAlreadySet();

    this.getStyleCell();
    this.getShowColumns();
    this.getGroupRow();
    this.getGroupRemainingRow();
    this.tableConfigurationsChangeEvent.emit({
      columns: this.displayedColumns,
      options: this.tableConfigurations.options
    });


  }

  getStyleCell() {

    this.displayedColumns.forEach(column => {
      const oStyle = {
        'width': this.getColumnWidth(column),
        'font-size': column.fontSize + 'px',
        'font-weight': column.fontWeight,
        'font-family': column.fontFamily + ' !important',
        ...(column.lineStyle === 'default'
          ? {
            'overflow': 'hidden',
            'white-space': 'unset',
            'text-overflow': 'unset',
          }
          : {
            'overflow': 'hidden',
            'white-space': 'nowrap',
            'text-overflow': 'ellipsis',
          })
      }
      column.style = {
        ...column.style, ...oStyle

      };
      column.headerStyle = oStyle;
      column.footerStyle = oStyle;
    });

  }

  getPrintStyleCell(percentWith: number = 100) {
    this.printColumns.forEach(column => {
      const oStyle = {
        'width': this.getColumnWidth(column, percentWith),
        'font-size': Math.floor(column.fontSize) + 'px',
        'font-weight': column.fontWeight,
        'font-family': column.fontFamily + ' !important',
        'overflow': 'hidden',
        'white-space': 'unset',
        'text-overflow': 'unset',
        'padding-right': '3px',
        'text-align': 'center',
        // ...(column.type === this.fieldTypes.Text
        //   ? {
        //     'text-align': 'right',
        //   }
        //   : {
        //     'text-align': 'center',
        //   })

      }
      column.style = {
        ...column.style, ...oStyle

      };
      column.headerStyle = oStyle;
      column.footerStyle = oStyle;
    });

  }

  handleFormKeydown(event: any, columnField: string, element?: any, isMoney?: boolean) {

    this.formKeydown.emit({key: event, columnField: columnField})
  }


  getOptionTitleParts(option: any, titleKeys: string[]): { mainTitle: string, subTitle: string } {

    const mainTitle = titleKeys[0] && option[titleKeys[0]] ? option[titleKeys[0]] : '';
    const subTitle = titleKeys[1] && option[titleKeys[1]] ? option[titleKeys[1]] : '';
    return {mainTitle, subTitle};
  }

  trackByFnNgSelect(index: number, item: any) {
    return item ? item.id || item : index;
  }

  onChange(event: any, col: Column) {
    if (event) {
      this.filterConditionsNgSelect[col.field] = {

        propertyNames: col.field,
        searchValues: event.id
      };
    } else {
      col.filterOptionsFn ? col.filterOptionsFn() : '';
      this.filterConditionsNgSelect[col.field] = {

        propertyNames: col.field,
        searchValues: 'clear'
      };
    }
    let query: any = {};

    Object.keys(this.filterConditionsNgSelect).forEach((key) => {
      const value = this.filterConditionsNgSelect[key];
      if (value) {
        query[key] = {
          searchValues: value.searchValues,
          propertyNames: value.propertyNames,
        };
      }
    });
    this.addChangeFilterAndSortKeys('filtersRow_NgSelect', JSON.parse(JSON.stringify(this.filterConditionsNgSelect[col.field])))
    this.loadFilteredData(TypeFilterOptions.NgSelect, query);

  }

  private LoadSearchTerm() {
    this.searchTerm$
      .pipe(
        filter((event) => event.term.length >= 3),
        debounceTime(300),
        distinctUntilChanged((prev, curr) => prev.term === curr.term)
      )
      .subscribe((event) => {
        if (!event.col.filterOptionsFn) return
        event.col.filterOptionsFn(event.term);
      });
  }

  searchWithColumnFn(column: any) {
    return (term: string, item: any) => {
      return this.customSearchFn(term, item, column);
    };
  }

  customSearchFn(term: string, item: any, col: Column): boolean {
    if (!term || !item || !col.optionsTitleKey) {
      return false;
    }
    const normalizedTerm = term.trim().toLowerCase();
    return col.optionsTitleKey.some((key: string) => {
      const value = this.getValueByKey(item, key);
      return value && value.toLowerCase().includes(normalizedTerm);
    });
  }

  getValueByKey(item: any, key: string): string {
    return key.split('.').reduce((obj, keyPart) => obj?.[keyPart], item) || '';
  }

  onSearch(event: { term: string; items: any[] }, col: Column) {
    this.searchTerm$.next({term: event.term, col});
  }

  // input search


  applyTextInputSearchFilters(col_field: string): void {
    const filterValues = this.filterFormTextInput.get(col_field).value;


    if (filterValues) {
      this.filterConditionsTextInput[col_field] = {
        propertyNames: [col_field],
        searchValues: [filterValues],
        searchCondition: 'contains',
        nextOperand: 'and'
      };
    } else {
      this.filterConditionsTextInput[col_field] = {
        propertyNames: [col_field],
        searchValues: null,
        searchCondition: 'contains',
        nextOperand: 'and'
      };
    }

    this.addChangeFilterAndSortKeys('filtersRow_TextInput', JSON.parse(JSON.stringify(this.filterConditionsTextInput[col_field])))
    this.loadFilteredData(TypeFilterOptions.TextInputSearch, this.filterConditionsTextInput);
  }

  applyNumberInputSearchFilters(col_field: string): void {
    const filterValues = this.filterFormNumberInput.get(col_field).value;
    if (filterValues) {
      const numberValue = parseFloat(filterValues.replace(/,/g, ''));
      this.filterConditionsNumberInput[col_field] = {
        propertyNames: [col_field],
        searchValues: [numberValue],
        searchCondition: 'equal',
        nextOperand: 'and'
      };
    } else {
      this.filterConditionsNumberInput[col_field] = {
        propertyNames: [col_field],
        searchValues: [null],
        searchCondition: 'equal',
        nextOperand: 'and'
      };
    }

    this.addChangeFilterAndSortKeys('filtersRow_NumberInput', JSON.parse(JSON.stringify(this.filterConditionsNumberInput[col_field])))
    this.loadFilteredData(TypeFilterOptions.NumberInputSearch, this.filterConditionsNumberInput);
  }

//updateDatepickerFilters
  updateDatepickerFilters(col_field: string) {
    let query: any = {};
    const date = this.filterFormDate.get(col_field).value;
    if (date && date._d) {
      this.filterConditionsDate[col_field] = {
        propertyNames: [col_field],
        searchValues: [date._d, new Date(date._d.getTime() + 86399999)],

        searchCondition: 'between',
        nextOperand: 'and'
      };
    } else {
      this.filterConditionsDate[col_field] = {
        propertyNames: [col_field],
        searchValues: [null],
        searchCondition: 'between',
        nextOperand: 'and'
      };
    }
    this.addChangeFilterAndSortKeys('filtersRow_Date', JSON.parse(JSON.stringify(this.filterConditionsDate[col_field])))
    this.loadFilteredData(TypeFilterOptions.Date, this.filterConditionsDate);
  };


  loadFilteredData(typeFilterOptions: TypeFilterOptions, query: any): void {
    this.tableConfigurations.pagination.pageIndex = 0;
    this.optionSelectedEvent.emit({
      typeFilterOptions: typeFilterOptions,
      query: query
    })

  }

  advancedFilter() {
    this.advancedFilterEvent.emit(true)

  }

  // Exports
  exportExcel() {
    const reportData = {
      columns: this.printColumns,
      rows: this.tableRows,
      selectedRowIds: this.selectedRowIds,
      groupsColumns: this.printGroupsColumns,
      groupsRemainingColumns: this.printGroupsRemainingColumns,
      config: this.tableConfigurations
    };
    setTimeout(() => {
      this.generateExcel.generateExcelData(reportData);
    }, 100)

  }


  customExportFunctionHandler() {

    let filteredData = [];

    if (this.selectedRowIds && this.selectedRowIds.length > 0) {
      filteredData = this.tableRows.filter(row => this.selectedRowIds.includes(row.id))
    } else {
      filteredData = this.tableConfigurations.options.useBuiltInPagination ? this.tableRowsClone : this.tableRows;
    }
    const items: any[] = this.tableConfigurations.options.exportOptions.customExportCallbackFn(filteredData);


    this.createExcelFromArray2(items);
  }


  createExcelFromArray2(items: any[]) {
    const reportTitle = this.tableConfigurations.printOptions.reportTitle || 'گزارش';

    let sheetName = reportTitle
      .substring(0, 31)
      .replace(/[\/\\:*?\[\]]/g, '')
      .trim();
    if (!sheetName) sheetName = 'گزارش';

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(items);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();

    XLSX.utils.book_append_sheet(wb, ws, sheetName);

    if (!wb.Workbook) wb.Workbook = {};
    wb.Workbook.Views = [{
      RTL: true,
    }];
    wb.Props = {
      Title: sheetName,
      Subject: 'گزارش اکسل',
      Author: "شرکت ایفا سرام (سهامی خاص)",
      CreatedDate: new Date()
    };
    XLSX.writeFile(wb, `${reportTitle}.xlsx`, {bookType: 'xlsx'});
  }


  export(format: string) {
    this.exportData.emit(format)
  }

  async toggleTemplate(row: any, index: number) {
    // await this.loadSavedSettings();
    this.expandedRowIndex = this.expandedRowIndex === index ? null : index;
    if (this.expandedRowIndex !== null) {
      this.expandedRowIndexEvent.emit(row);
    } else {
      this.expandedRowIndexEvent.emit(null);
    }
  }

  ngOnDestroy() {
    this.searchTerm$.complete();
    this.windowEventListeners.forEach((x: any) => {
      x();
    })
  }


}


