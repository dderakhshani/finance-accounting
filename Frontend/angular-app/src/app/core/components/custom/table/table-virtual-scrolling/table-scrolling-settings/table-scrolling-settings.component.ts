import {Component, Inject, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";
import {TableOptions} from "../../models/table-options";
import {Column} from "../../models/column";
import {TableColumnDataType} from "../../models/table-column-data-type";
import {DecimalFormat} from "../../models/decimal-format";
import {TableSettingsService} from "../../table-details/Service/table-settings.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";
import {MatSlideToggleChange} from "@angular/material/slide-toggle";
import {FontWeights} from "../../models/font-weights";
import {FontFamilies} from "../../models/font-families";
import {Toastr_Service} from "../../../../../../shared/services/toastrService/toastr_.service";

@Component({
  selector: 'app-table-scrolling-settings',
  templateUrl: './table-scrolling-settings.component.html',
  styleUrls: ['./table-scrolling-settings.component.scss']
})
export class TableScrollingSettingsComponent implements OnInit, OnChanges {
  tableConfigurations!: TableScrollingConfigurations;
  tableConfigurationsClone!: TableScrollingConfigurations;

  options!: TableOptions;
  displayedColumns!: Column[];
  maxWidth = 20;
  minWidth = 0;
  thumbLabelWidth = true
  step = 1;
  maxFontSize = 30;
  minFontSize = 8;
  thumbLabelFontSize = true
  fieldTypes = TableColumnDataType;
  decimalFormats = Object.values(DecimalFormat);
  DecimalFormatLabelMapping: { [key in DecimalFormat]: string } = {
    [DecimalFormat.None]: 'بدون فرمت',
    [DecimalFormat.Default]: 'پیش‌فرض',
    [DecimalFormat.Rounded]: 'رند شده',
    [DecimalFormat.OneDecimal]: 'یک رقم اعشار',
    [DecimalFormat.TwoDecimals]: 'دو رقم اعشار',
    [DecimalFormat.ThreeDecimals]: 'سه رقم اعشار',
    [DecimalFormat.FourDecimals]: 'چهار رقم اعشار'
  };

  fontWeights = Object.values(FontWeights);
  fontWeightsLabelMapping: { [key in FontWeights]: string } = {
    [FontWeights.thin]: 'بسیار نازک',
    [FontWeights.extraLight]: 'نزدیک به نازک',
    [FontWeights.light]: 'سبک',
    [FontWeights.normal]: 'پیش‌فرض',
    [FontWeights.medium]: 'متوسط',
    [FontWeights.semiBold]: 'نیمه‌پررنگ',
    [FontWeights.bold]: 'پررنگ',
    [FontWeights.extraBold]: 'خیلی پررنگ',
    [FontWeights.black]: 'سیاه‌ترین و ضخیم‌ترین'
  };
  fontFamilies = Object.values(FontFamilies);
  fontFamiliesLabelMapping: { [key in FontFamilies]: string } = {
    [FontFamilies.IranSans]: 'ایران سنس',
    [FontFamilies.Vazir]: 'وزیر',
    [FontFamilies.Nazanin]: 'نازنین',
    [FontFamilies.IranYekanBold]: 'ایران یکان - پررنگ',
    [FontFamilies.IranYekanExtraBold]: 'ایران یکان - خیلی پررنگ',
    [FontFamilies.IranYekanRegular]: 'ایران یکان - معمولی'
  }

  constructor(private tableSettingsService: TableSettingsService,
              private toastr: Toastr_Service,
              public dialogRef: MatDialogRef<TableScrollingSettingsComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {
    this.tableConfigurations = data.tableConfigurations;
    this.tableConfigurationsClone = data.tableConfigurationsClone;
    this.displayedColumns = JSON.parse(JSON.stringify(data.tableConfigurations.columns));

    this.options = data.tableConfigurations.options;

  }

  async ngOnInit(): Promise<void> {


  }


  ngOnChanges(changes: SimpleChanges) {


  }

  drop(event: CdkDragDrop<string[]>) {
    const previousColumn = this.displayedColumns[event.previousIndex];
    const currentColumn = this.displayedColumns[event.currentIndex];
    if (previousColumn.isDisableDrop || currentColumn.isDisableDrop) {
      return;
    }

    moveItemInArray(this.displayedColumns, event.previousIndex, event.currentIndex);

    this.displayedColumns = this.displayedColumns.map((column, index) => {
      column.index = index;
      return column;
    });

  }


  changeMatTooltipDisabledToggle(event: MatSlideToggleChange, column: Column) {

    column.matTooltipDisabled = !event.checked
  }

  changeLineStyleToggle(event: MatSlideToggleChange, column: Column) {

    column.lineStyle = event.checked ? 'default' : 'onlyShowFirstLine'

  }

  async rest(): Promise<void> {
    const tableConfigurationsClone = this.tableConfigurationsClone;
    this.tableConfigurations.columns = this.tableConfigurations.columns.filter((column: any) =>
      this.tableConfigurationsClone.columns.some((configColumn: any) => configColumn.field === column.field)
    );
    tableConfigurationsClone.columns.forEach((column: any, index: number) => {
      const originalColumn = this.tableConfigurations.columns[index];
      column.sumRowDisplayFn = originalColumn.sumRowDisplayFn;
      column.filterOptionsFunction = originalColumn.filterOptionsFunction;
      column.displayFunction = originalColumn.displayFunction;
      column.displayPrintFun = originalColumn.displayPrintFun;
      column.displayFn = originalColumn.displayFn;
      column.asyncOptions = originalColumn.asyncOptions;
      column.filterOptionsFn = originalColumn.filterOptionsFn;
      column.groupRemainingNameOrFn = originalColumn.groupRemainingNameOrFn;
      column.isSorted = originalColumn.isSorted;
      column.isHovered_TH = originalColumn.isHovered_TH;
      if (column.filterOptionsFn) {
        column.filterOptionsFn = this.tableConfigurations.columns.find((c: any) => c.field === column.field)?.filterOptionsFn;
      }
      if (column.filteredOptions) {
        column.filteredOptions = this.tableConfigurations.columns.find((c: any) => c.field === column.field)?.filteredOptions;
      }

      column.filter = this.tableConfigurations.columns.find((c: any) => c.field === column.field)?.filter;


    });
    const originalOptions = this.tableConfigurations.options;
    tableConfigurationsClone.options.exportOptions.customExportCallbackFn = originalOptions.exportOptions?.customExportCallbackFn || null;

    tableConfigurationsClone.sortKeys = this.tableConfigurations.sortKeys;
    tableConfigurationsClone.filters = this.tableConfigurations.filters;
    this.tableConfigurations = tableConfigurationsClone;
    this.displayedColumns = tableConfigurationsClone.columns;
    this.options = tableConfigurationsClone.options;
    this.options.isLoadingTable = false;


  }

  sortColumnsByIndex(columns?: Column[]): Column[] {
    if (!columns) {
      return [];
    }
    return columns.sort((a, b) => (a.index ?? 0) - (b.index ?? 0));
  }

  async delete() {
    try {
      await this.tableSettingsService.deleteSettings(window.location.pathname);
      await this.rest();
      setTimeout(() => {
        this.dialogRef.close({
          columns: this.displayedColumns,
          options: this.options
        });
      }, 100);
    } catch (error) {
      console.error("Error while deleting settings:", error);

    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }

  async save(): Promise<void> {
    this.options.isLoadingTable = false;
    this.tableConfigurations.options = this.options;
    const updatedColumns = this.displayedColumns.map(column => {
      const {
        sumRowDisplayFn,
        filterOptionsFunction,
        displayFunction,
        displayFn,
        filterOptionsFn,
        asyncOptions,
        groupRemainingNameOrFn,
        filter,
        ...rest
      } = column;

      return rest;
    });

    this.tableConfigurations.columns = this.tableConfigurations.columns.map(existingColumn => {
      const updatedColumn = updatedColumns.find(updated => updated.field === existingColumn.field);
      return updatedColumn ? {...existingColumn, ...updatedColumn} : existingColumn;
    });

    const cols = this.sortColumnsByIndex(this.tableConfigurations.columns)
    this.options.hasDefaultSortKey = this.tableConfigurationsClone.options.hasDefaultSortKey;
    await this.tableSettingsService.saveSettings(window.location.pathname, cols, this.options);

    this.dialogRef.close({
      columns: this.tableConfigurations.columns,
      options: this.options
    })


  }


  handleDisplayColumn(column: Column): void {
    if (column.isDisableDrop) return;


    if (column.display) {
      const activeDisplayColumns = this.displayedColumns.filter(c => c.display).length;

      // حداقل باید سه ستون برای نمایش باقی بماند
      if (activeDisplayColumns <= 3) {
        this.toastr.showToast({
          message: 'حداقل سه ستون برای نمایش باید وجود داشته باشد',
          title: 'اخطار',
          type: 'warning'
        });
        return;
      }
    }


    column.display = !column.display;
    if(!column.display) column.print = column.display


  }
  handlePrintColumn(column: Column): void {
    if (column.isDisableDrop) return;

    const activePrintColumns = this.displayedColumns.filter(c => c.print).length;

    if (column.print && activePrintColumns <= 3) {
      this.toastr.showToast({
        message: 'حداقل سه ستون برای پرینت باید وجود داشته باشد',
        title: 'اخطار',
        type: 'warning'
      });
      return;
    }

    column.print = !column.print;

  }
}



