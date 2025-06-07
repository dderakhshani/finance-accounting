import {AfterViewInit, Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {TableScrollingSettingsComponent} from "../table-scrolling-settings/table-scrolling-settings.component";
import {DataDisplayMode, ToolBar} from "../../models/tool-bar";
import {Column, groupColumn} from "../../models/column";
import {PrintPdfService} from "../../table-details/Service/print-pdf.service";
import {TableColumnDataType} from "../../models/table-column-data-type";

@Component({
  selector: 'app-table-toolbar',
  templateUrl: './table-toolbar.component.html',
  styleUrls: ['./table-toolbar.component.scss']
})
export class TableToolbarComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() toolBar !: ToolBar
  @Input() tableConfigurations!: TableScrollingConfigurations;
  @Input() tableConfigurationsClone!: TableScrollingConfigurations;
  @Input() tableRows: any[] = [];
  @Input() selectedRowIds: any[] = [];
  @Input() groupsColumns!: groupColumn[];
  @Input() groupsRemainingColumns!: groupColumn[];
  @Input() printColumns!: Column[];
  @Input() outPrint:boolean = false;
  @Input() countBadgeFiltersAndSorts: number = 0;
  @Input() countBadgeExcludeSelectedItemsLocal: number = 0;
  @Input() countBadgeIncludeOnlySelectedItemsLocal: number = 0;
  @Input() countBadgeIncludeOnlySelectedItemsFilter: number = 0;
  @Input() countBadgeExcludeSelectedItemsFilter: number = 0;
  @Input() countBadgeUndoLocal: number = 0;
  @Input() countBadgeRestorePreviousFilter: number = 0;

  @Output() isLargeSizeEvent = new EventEmitter<boolean>();
  @Output() removeAllFiltersAndSortsEvent = new EventEmitter<boolean>();
  @Output() showAllEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() clearSelectedItemsEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() includeOnlySelectedItemsLocalEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() excludeSelectedItemsLocalEvent: EventEmitter<any> = new EventEmitter<any>();

  @Output() excludeSelectedItemsFilterEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() undoItemsLocalEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() deleteItemsLocalEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() restorePreviousFilterEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() refreshEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() tableConfigurationsChangeEvent = new EventEmitter<any>();
  dataDisplayMode: DataDisplayMode = DataDisplayMode.showAll;
  activeButton: string = 'showAll';
  fieldTypes = TableColumnDataType;
  constructor(public dialog: MatDialog, private printService: PrintPdfService) {
  }


  ngOnChanges(changes: SimpleChanges) {
    if (changes['toolBar']) {
      if (this.toolBar) {
        this.defaultToolBar()
      }
    }
    if (changes['countBadgeFiltersAndSorts']) {
      if (this.countBadgeFiltersAndSorts > 0) {
        this.dataDisplayMode = DataDisplayMode.showAllWithFilter
      } else {
        this.dataDisplayMode = DataDisplayMode.showAll
      }
    }
    if (changes['countBadgeIncludeOnlySelectedItemsLocal']) {
      if (this.countBadgeIncludeOnlySelectedItemsLocal === 0
        &&this.countBadgeExcludeSelectedItemsLocal === 0
        && this.toolBar ) {
        this.showAll()
      }
    }
    if (changes['countBadgeRestorePreviousFilter']) {
      if ( this.countBadgeRestorePreviousFilter === 0 && this.toolBar) {
        this.showAll()
      }
      if (this.countBadgeRestorePreviousFilter > 0) {
        this.dataDisplayMode = DataDisplayMode.showAllWithFilter
      } else {
        this.dataDisplayMode = DataDisplayMode.showAll
      }
    }
    if (changes['countBadgeExcludeSelectedItemsFilter']) {
      if (this.countBadgeExcludeSelectedItemsFilter > 0) {
        this.dataDisplayMode = DataDisplayMode.showAllWithFilter
      } else {
        this.dataDisplayMode = DataDisplayMode.showAll
      }
    }
    if (changes['outPrint']) {
      if (this.toolBar && this.outPrint) {
        this.printFile()
      }
    }


  }

  ngOnInit(): void {
    // this.defaultToolBar();
  }

  ngAfterViewInit(): void {

  }


  defaultToolBar() {
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
      fullScreen: false,
      printFile: false,
      removeAllFiltersAndSorts: false,
      showAll: false
    };

    // const {showTools = {}, isLargeSize = false} = this.toolBar;

    // this.toolBar = {
    //   showTools: {...defaultShowTools, ...showTools},
    //   isLargeSize
    // };
    this.toolBar.showTools.excludeSelectedItemsLocal = false
    this.toolBar.showTools.includeOnlySelectedItemsFilter = false
    this.toolBar.showTools.undoLocal = false
    this.toolBar.showTools.deleteLocal = false


  }


  onTableSettings() {
    this.activeButton = 'tableSettings';
    let dialogConfig = new MatDialogConfig();
    dialogConfig.width = '80vw'
    dialogConfig.maxHeight = '100vh'

    // حذف TemplateRef از columns قبل از ارسال
    let columnsWithoutTemplates = this.tableConfigurations.columns.map(column => {
      const { template, expandRowWithTemplate, ...columnWithoutTemplate } = column;
      return columnWithoutTemplate; // برگرداندن ستون‌ها بدون TemplateRef
    });

    columnsWithoutTemplates = columnsWithoutTemplates.filter(column => column.type !== this.fieldTypes.ForPrint)

    dialogConfig.data = {
      tableConfigurations: { ...this.tableConfigurations, columns: columnsWithoutTemplates },
      tableConfigurationsClone: this.tableConfigurationsClone
    };

    let dialogReference = this.dialog.open(TableScrollingSettingsComponent, dialogConfig);

    dialogReference.afterClosed().subscribe((result) => {
      if (result) {
        // دوباره اضافه کردن TemplateRef به ستون‌ها بعد از دریافت تنظیمات جدید
        result.columns.forEach((newColumn :Column,index : any) => {

          newColumn.template = this.tableConfigurations.columns.find((c: any) => c.field === newColumn.field)?.template;
          newColumn.expandRowWithTemplate =this.tableConfigurations.columns.find((c: any) => c.field === newColumn.field)?.expandRowWithTemplate;

        });

        this.tableConfigurations.options = result.options;
        this.tableConfigurations.columns = result.columns;
        this.tableConfigurationsChangeEvent.emit(result);
      }
    });
  }


  includeOnlySelectedItemsLocal() {
    this.activeButton = 'includeOnlySelectedItemsLocal';
    this.toolBar.showTools.excludeSelectedItemsLocal = true;
    this.toolBar.showTools.includeOnlySelectedItemsLocal = false;

    this.toolBar.showTools.undoLocal = true;
    this.toolBar.showTools.deleteLocal = true;
    this.toolBar.showTools.excludeSelectedItemsFilter = false
    this.toolBar.showTools.includeOnlySelectedItemsFilter = false
    this.toolBar.showTools.refresh = false;
    this.toolBar.showTools.removeAllFiltersAndSorts = false;
    this.toolBar.showTools.restorePreviousFilter = false;
    this.dataDisplayMode = DataDisplayMode.includeOnlySelectedItemsLocal
    this.includeOnlySelectedItemsLocalEvent.emit(true);

  }

  excludeSelectedItemsLocal() {
    this.activeButton = 'excludeSelectedItemsLocal';
    this.excludeSelectedItemsLocalEvent.emit(true);
  }



  excludeSelectedItemsFilter() {
    this.activeButton = 'excludeSelectedItemsFilter';
    this.excludeSelectedItemsFilterEvent.emit(true);
  }

    showAll() {
    if (this.activeButton !== 'showAll' ) {
      this.activeButton = 'showAll';
      this.toolBar.showTools.excludeSelectedItemsLocal = this.tableConfigurationsClone.toolBar.showTools.excludeSelectedItemsLocal;
      this.toolBar.showTools.includeOnlySelectedItemsLocal = this.tableConfigurationsClone.toolBar.showTools.includeOnlySelectedItemsLocal;
      this.toolBar.showTools.undoLocal = this.tableConfigurationsClone.toolBar.showTools.undoLocal;
      this.toolBar.showTools.deleteLocal = this.tableConfigurationsClone.toolBar.showTools.deleteLocal;
      this.toolBar.showTools.excludeSelectedItemsFilter = this.tableConfigurationsClone.toolBar.showTools.excludeSelectedItemsFilter;
      this.toolBar.showTools.refresh = this.tableConfigurationsClone.toolBar.showTools.refresh;
      this.toolBar.showTools.removeAllFiltersAndSorts = this.tableConfigurationsClone.toolBar.showTools.removeAllFiltersAndSorts;
      this.toolBar.showTools.restorePreviousFilter = this.tableConfigurationsClone.toolBar.showTools.restorePreviousFilter;
      if (this.countBadgeRestorePreviousFilter > 0) {
        this.dataDisplayMode = DataDisplayMode.showAllWithFilter
      } else {
        this.dataDisplayMode = DataDisplayMode.showAll
      }

      this.showAllEvent.emit(true);
    }

  }

  removeAllFiltersAndSorts() {
    this.activeButton = 'removeAllFiltersAndSorts';
    this.removeAllFiltersAndSortsEvent.emit(true);

  }


  refresh() {
    this.activeButton = 'refresh';
    this.refreshEvent.emit(true);

  }


  exportExcel() {
    this.activeButton = 'exportExcel';
  }


  fullScreen() {
    this.activeButton = 'fullScreen';
    this.toolBar.isLargeSize = !this.toolBar.isLargeSize;
    this.isLargeSizeEvent.emit(this.toolBar.isLargeSize);

  }


  printFile() {
    this.activeButton = 'printFile';
    const reportData = {
      columns: this.printColumns,
      rows: this.tableRows,
      selectedRowIds: this.selectedRowIds,
      groupsColumns: this.groupsColumns,
      groupsRemainingColumns: this.groupsRemainingColumns,
      config: this.tableConfigurations
    };

    this.printService.generatePDFReport(reportData)
  }


  undoLocal() {


    this.activeButton = 'undoLocal';
    this.undoItemsLocalEvent.emit(true);

  }
  deleteLocal() {


    this.activeButton = 'deleteLocal';
    this.deleteItemsLocalEvent.emit(true);
    this.showAll()

  }


  restorePreviousFilter() {
    this.restorePreviousFilterEvent.emit(true)

  }
}

