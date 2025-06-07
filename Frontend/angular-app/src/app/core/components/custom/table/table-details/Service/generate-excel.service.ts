import { Injectable } from '@angular/core';
import {TableVirtualScrollHelperService} from "./table-virtual-scroll-helper.service";
import {Column} from "../../models/column";
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";
import * as XLSX from "xlsx";
import {CustomDecimalPipe} from "../pipe/custom-decimal.pipe";
import {ToPersianDatePipe} from "../../../../../pipes/to-persian-date.pipe";
import {TableColumnDataType} from "../../models/table-column-data-type";
@Injectable({
  providedIn: 'root'
})
export class GenerateExcelService {
  fieldTypes = TableColumnDataType;
  constructor(  private customDecimal: CustomDecimalPipe,
                private toPersianDate: ToPersianDatePipe,
                private tableHelper: TableVirtualScrollHelperService
  ) { }
  //اکسل
  generateExcelData(data: {
    columns: Column[];
    rows: any[];
    selectedRowIds: any[];
    groupsColumns: any[];
    groupsRemainingColumns: any[];
    config: TableScrollingConfigurations;
  })  {
    const excelRows: any[][] = [];
    const merges: XLSX.Range[] = [];
    data.rows = this.tableHelper.filterRowSelection(data.rows, data.selectedRowIds);
    // افزودن هدرهای اطلاعاتی
    // if (!data.config.printOptions.hasCustomizeHeaderPage && false) {
    //   // اطلاعات شرکت و عنوان گزارش
    //   excelRows.push(['شرکت ایفا سرام (سهامی خاص)']);
    //   merges.push({ s: { r: 0, c: 0 }, e: { r: 0, c: data.columns.length - 1 } });
    //
    //   excelRows.push([data.config.printOptions.reportTitle]);
    //   merges.push({ s: { r: 1, c: 0 }, e: { r: 1, c: data.columns.length - 1 } });
    //
    //   // محدوده تاریخ
    //   if (data.config.printOptions.dateFrom && data.config.printOptions.dateTo) {
    //     const dateRange = `از تاریخ: ${this.toPersianDate.transform(data.config.printOptions.dateFrom)} تا تاریخ: ${this.toPersianDate.transform(data.config.printOptions.dateTo)}`;
    //     excelRows.push([dateRange]);
    //     merges.push({ s: { r: 2, c: 0 }, e: { r: 2, c: data.columns.length - 1 } });
    //   }
    //
    //   // اطلاعات کاربر
    //   const userInfo = `کاربر چاپ کننده: ${this.tableHelper.userInfo.fullName} - تاریخ چاپ: ${new Date().toLocaleDateString('fa-IR')}`;
    //   excelRows.push([userInfo]);
    //   merges.push({ s: { r: 3, c: 0 }, e: { r: 3, c: data.columns.length - 1 } });
    //
    //   excelRows.push([]); // خط خالی برای فاصله
    //
    // }

    // افزودن گروه‌های ستون‌ها
    if (data.config.options?.showGroupRow) {
      const groupRow: any[] = [];
      let currentCol = 0;

      // ابتدا تمام سلول‌های خالی را با مقدار null پر کنید
      for (let i = 0; i < data.columns.filter(col => col.print).length; i++) {
        groupRow.push(null);
      }

      data.groupsColumns.forEach(group => {
        // قرار دادن نام گروه در موقعیت شروع ستون
        groupRow[currentCol] = group.groupName;

        // اضافه کردن ادغام سلول‌ها
        merges.push({
          s: { r: excelRows.length, c: currentCol },
          e: { r: excelRows.length, c: currentCol + group.colspan - 1 }
        });

        currentCol += group.colspan;
      });

      excelRows.push(groupRow);
    }

    // افزودن عنوان ستون‌ها
    const headers = data.columns
      .filter(col => col.print)
      .map(col => col.title);
    excelRows.push(headers);

    // افزودن داده‌های بدنه
    data.rows.forEach(row => {
      const rowData = data.columns
        .filter(col => col.print)
        .map(col => {
          const rawValue = col.displayFn ? col.displayFn(row) : row[col.field];
          return this.formatExcelValue(col, rawValue);
        });
      excelRows.push(rowData);
    });

    // افزودن ردیف مجموع
    if (data.config.options?.showSumRow) {
      const sumRow = data.columns.map(col => {
        if (!col.print) return null;
        if (col.type === this.fieldTypes.Index) return 'جمع';
        if ([this.fieldTypes.Money, this.fieldTypes.Number].includes(col.type) && col.showSum) {
          const sum = this.tableHelper.calculateColumnSum(col, data.rows);
          return sum;
        }
        return null;
      });
      excelRows.push(sumRow);
    }

    // افزودن گروه‌های باقیمانده
    if (data.config.options?.showGroupRemainingRow) {
      const remainingRow: any[] = [];
      let currentCol = 0;

      // ابتدا تمام سلول‌ها را با مقدار null پر کنید
      const totalColumns = data.columns.filter(col => col.print).length;
      for (let i = 0; i < totalColumns; i++) {
        remainingRow.push(null);
      }

      data.groupsRemainingColumns.forEach(group => {
        const value = this.tableHelper.calculateColumnSumRemainingGroup(group);

        // قرار دادن مقدار در موقعیت صحیح
        remainingRow[currentCol] = value;

        // افزودن ادغام سلول‌ها
        merges.push({
          s: { r: excelRows.length, c: currentCol },
          e: { r: excelRows.length, c: currentCol + group.colspan - 1 }
        });

        currentCol += group.colspan;
      });

      excelRows.push(remainingRow);
    }

    // return { excelRows, merges };
    return this.createExcelFromArray(data.config , data.columns , excelRows , merges)
  }

  private formatExcelValue(col: Column, value: any): any {
    switch (col.type) {
      case this.fieldTypes.Date:
        return this.toPersianDate.transform(value);
      case this.fieldTypes.Money:
      case this.fieldTypes.Number:
        return value;
      case this.fieldTypes.CheckBox:
        return value ? 'بله' : 'خیر';
      default:
        return value || '';
    }
  }
  createExcelFromArray( config :TableScrollingConfigurations , columns :Column[],  excelRows: any[][], merges: XLSX.Range[] = []) {
    const reportTitle = config.printOptions.reportTitle || 'گزارش';
    const sheetName = reportTitle.substring(0, 31).replace(/[\/\\:*?[\]]/g, '').trim() || 'گزارش';
    const now = new Date();
    const formatter = new Intl.DateTimeFormat('fa-IR', {
      year: '2-digit',  // فقط دو رقم آخر سال
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit'
    });
    let formattedDate = formatter.format(now);
    // پردازش برای حذف کاراکترهای اضافی
    formattedDate = formattedDate.replace(/:/g, '');

    const ws: XLSX.WorkSheet = XLSX.utils.aoa_to_sheet(excelRows);
    ws['!merges'] = merges;

    // تنظیم عرض ستون‌ها
    ws['!cols'] = columns.map(col => ({ width: this.calculateColumnWidth(col) }));
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, sheetName);

    // تنظیمات جهت‌گیری راست به چپ
    if (!wb.Workbook) wb.Workbook = {};
    wb.Workbook.Views = [{ RTL: true }];

    // تنظیمات متادیتا
    wb.Props = {
      Title: reportTitle,
      Subject: 'گزارش اکسل',
      Author: "شرکت ایفا سرام (سهامی خاص)",
      CreatedDate: new Date()
    };
    const fileName = sheetName + "_" + formattedDate
      .replace(/[^a-zA-Z0-9_\u0600-\u06FF]/g, '')
      .substring(0, 100);
    XLSX.writeFile(wb, fileName + ".xlsx");

  }
  public calculateColumnWidth(col: Column): number {
    const DEFAULT_WIDTH = 15;
    if (!col.headerStyle?.width) return DEFAULT_WIDTH;

    const widthMatch = col.headerStyle.width.toString().match(/(\d+)px/);
    if (!widthMatch) return DEFAULT_WIDTH;

    const pxWidth = parseInt(widthMatch[1], 10);
    return Math.round(pxWidth / 7); // تبدیل پیکسل به واحدهای اکسل
  }
}
