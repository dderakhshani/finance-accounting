import {Injectable, SecurityContext} from '@angular/core';
import {PagesCommonService} from "../../../../../../shared/services/pages/pages-common.service";
import {TableColumnDataType} from "../../models/table-column-data-type";
import {DomSanitizer} from "@angular/platform-browser";
import {TableScrollingConfigurations} from "../../models/table-scrolling-configurations";
import {Column, groupColumn} from "../../models/column";
import {CustomDecimalPipe} from "../pipe/custom-decimal.pipe";
import {ToPersianDatePipe} from "../../../../../pipes/to-persian-date.pipe";
import {CustomFont} from "./pdfmakefonts";
import {IdentityService} from "../../../../../../modules/identity/repositories/identity.service";
import {TableVirtualScrollHelperService} from "./table-virtual-scroll-helper.service";
import * as XLSX from "xlsx";

@Injectable({
  providedIn: 'root'
})
export class PrintPdfService {
  fieldTypes = TableColumnDataType;
  reportData!: any;
  tableConfigurations!: TableScrollingConfigurations;

  fontFace: string = CustomFont.Font.fontFace;

  result: string = '';

  private printWindow: Window | null = null;

  constructor(public Service: PagesCommonService, private sanitizer: DomSanitizer,
              private customDecimal: CustomDecimalPipe,
              private toPersianDate: ToPersianDatePipe,
              private tableHelper: TableVirtualScrollHelperService
  ) {

  }


  generatePDFReport(data: {
                      columns: Column[];
                      rows: any[];
                      selectedRowIds: any[];
                      groupsColumns: groupColumn[];
                      groupsRemainingColumns: groupColumn[];
                      config: TableScrollingConfigurations;
                    }
  ): any {
    this.reportData = data;
    data.rows = this.tableHelper.filterRowSelection(data.rows, data.selectedRowIds);
    const htmlContent = this.buildFullTableHTML(data);
    const title = data.config.printOptions.reportTitle || 'گزارش';
    this.openPrintWindow(htmlContent, title);
  }

  private openPrintWindow(htmlContent: string, reportTitle: string = 'گزارش', autoPrint: boolean = false, callBackMethod: any = undefined): void {
    if (this.printWindow && !this.printWindow.closed) {
      if (!confirm('پنجره چاپ قبلی هنوز باز است! آیا میخواهید آن را ببندید و پنجره جدید باز کنید؟')) {
        return;
      }
      this.printWindow.close();
      this.printWindow = null;
    }

    const printWindow = window.open('', '_blank');
    if (!printWindow) return;

    this.printWindow = printWindow;

    printWindow.document.write(`
    <html lang="fa">
      <head>
        <title>${reportTitle}</title>
        <style>
          ${this.getPrintStyles()}

       @media print {
  .print-controls, #remove-print-controls { display: none; }

  @page {
    size: A4 landscape;
    margin: 15mm 0;
    counter-increment: page;

    @top-center {
      content: "شرکت ایفا سرام (سهامی خاص)";
      font-family: 'IranYekanBold';
      font-size: 16px;
      color: #000;
      margin: 5mm 0 ;
    }

    @bottom-center {
      content: 'صفحه ' counter(page);
      font-family: 'IranYekanBold';
      font-size: 12px;
      margin-bottom: 5mm;
    }
  }

  @page:first {
    margin-top: 5mm;
    @top-center {
      content: none;
    }
  }
}

        </style>
      </head>
      <body>
        <div class="print-controls" id="remove-print-controls">
           <button onclick="setOrientation('A4 landscape')" class="btn btn-print">📃 خروجی افقی </button>
             <button onclick="setOrientation('A4 portrait')" class="btn btn-print">📃 خروجی عمودی</button>
             <button onclick="downloadExcel('')" class="btn btn-print">📊 خروجی اکسل</button>

          <button onclick="window.close()" class="btn btn-close">❌ بستن</button>
        </div>
        <div class="pdf-print-container">
          ${htmlContent}
        </div>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.17.0/xlsx.full.min.js"></script>
        <script>

            function setOrientation(orientation) {
            let style = document.createElement('style');
           if (orientation === 'A4 landscape') {
             style.innerHTML = "${String(this.getPrintHeaderContainerStylesLandscape())}"; // تغییر استایل برای افقی
            } else if (orientation === 'A4 portrait') {
              style.innerHTML = "${String(this.getPrintHeaderContainerStylesPortrait())}"; // تغییر استایل برای عمودی
            }
            document.head.appendChild(style);
            window.print();
          }

        function downloadExcel(decimals) {
            let table = document.querySelector('.pdf-table-export');
            if (!table) return;
            const now = new Date();
            const formatter = new Intl.DateTimeFormat('fa-IR', {
              year: '2-digit',
              month: '2-digit',
              day: '2-digit',
              hour: '2-digit'
            });

            let formattedDate = formatter.format(now);

            formattedDate = formattedDate.replace(/:/g, '');

            let sheetName = "${reportTitle.substring(0, 31).replace(/[\/\\:*?\[\]]/g, '')
      .trim()}";
            let wb = XLSX.utils.table_to_book(table, { sheet: sheetName });
            let ws = wb.Sheets[sheetName];

            // تنظیم جهت راست به چپ در شیت
            ws["!sheetPr"] = { rightToLeft: true };

            // تنظیمات Workbook برای جهت RTL و زوم
            if (!wb.Workbook) wb.Workbook = {};
            if (!wb.Workbook.Views) wb.Workbook.Views = [{}];
            wb.Workbook.Views[0].RTL = true;
            wb.Workbook.Views[0].zoomScale = 150;
            wb.Workbook.Views[0].zoomScaleNormal = 150;

            // تنظیم قالب عددی با سه رقم اعشار (در صورت درخواست)
            if (decimals === '3') {
              for (let cell in ws) {
                if (ws.hasOwnProperty(cell) && cell[0] !== '!') {
                  if (typeof ws[cell].v === 'number') {
                    ws[cell].z = '0.000';
                  }
                }
              }
            }

            // تنظیم متادیتای فایل اکسل
            wb.Props = {
              Title: sheetName,
              Subject: 'گزارش اکسل',
              Author: "شرکت ایفا سرام (سهامی خاص) -  ${this.tableHelper.userInfo.fullName}",
              CreatedDate: new Date(),
              Locale: "fa-IR"
            };
            const fileName = sheetName + "_" + formattedDate
             .replace(/[^a-zA-Z0-9_\u0600-\u06FF]/g, '')
             .substring(0, 100);
            XLSX.writeFile(wb, fileName + ".xlsx");
            }
          if (${autoPrint}) {
            window.onload = () => {
              window.print();
              setTimeout(() => window.close(), 500);
            };
          }
        </script>
      </body>
    </html>
  `);
    printWindow.document.close();
  }

  private buildFullTableHTML(data: {
    columns: Column[];
    rows: any[];
    groupsColumns: groupColumn[];
    groupsRemainingColumns: groupColumn[];
    config: TableScrollingConfigurations;
  }): string {
    return `
      <div class="pdf-table-container" dir="rtl" xmlns="http://www.w3.org/1999/html">
         ${this.buildHeaderContainer(data.config)}
             <div class="pdf-table-export "  xmlns="http://www.w3.org/1999/html">
            ${this.buildTableHeaderGroups(data.groupsColumns, data.columns, data.config)}
          <table class="pdf-table">
           <thead class="pdf-header-table">
             ${this.buildTableHeader(data.groupsColumns, data.columns, data.config)}
             </thead>
        <tbody class="pdf-body-table">
        ${this.buildTableBody(data.rows, data.columns, data.config)}
        </tbody>
        </table>
        ${this.buildTableFooter(data.rows, data.columns, data.groupsRemainingColumns, data.config)}
        </div>
        <div style="width: 100%;display: flex; flex-direction: column;align-items: center">${data.config.printOptions.customizeHtmlFooter}</div>
        <div class="footer-container" >
        <div class="footer-left">
          <p class="current-user" >کاربر چاپ کننده: ${this.tableHelper.userInfo.fullName}</p>
         </div>
          </div>
      </div>
    `;
  }

  private buildHeaderContainer(config: TableScrollingConfigurations): string {
    let dateRangeHtml = '';
    let headerHtml = '';
    if (!config.printOptions.hasCustomizeHeaderPage) {
      let dateRangeHtml = ''
      if (config.printOptions.dateFrom && config.printOptions.dateTo) {
        this.tableHelper.yearsLabel = this.tableHelper.genYearLabel(config.printOptions.dateFrom, config.printOptions.dateTo)
        dateRangeHtml =
          `<div class="date-from-to">
         از تاریخ : ${this.toPersianDate.transform(config.printOptions.dateFrom)}
         تا تاریخ : ${this.toPersianDate.transform(config.printOptions.dateTo)}
       </div>`;
      }


      headerHtml = `
    <div class="header-container">
      <div class="header-right">
        <div class="fig-logo">
          <img src="/assets/images/Reportlogo.jpg" class="logo" alt="لوگو">
        </div>
        ${dateRangeHtml}
      </div>
      <div class="header-center">
        <p class="company-name" style="{margin-bottom: 5px}">شرکت ایفا سرام (سهامی خاص)</p>
        <p class="current-year">${config.printOptions.reportTitle}</p>
        <p class="current-year">
          سال مالی: ${this.tableHelper.yearsLabel ?? this.tableHelper.userInfo.years.find((x: any) => x.id == this.tableHelper.userInfo.yearId)?.yearName}
        </p>
      </div>
      <div class="header-left">
        <p class="current-user"> &nbsp;</p>
        <p class="date-report">تاریخ چاپ گزارش: ${new Date().toLocaleDateString('fa-IR')}</p>
      </div>
    </div>`;
    } else
      headerHtml = config.printOptions.customizeHtmlHeader;
    return headerHtml
  }

  private buildTableHeaderGroups(
    groups: any[],
    columns: Column[],
    config: TableScrollingConfigurations
  ): string {
    return `
           ${config.options?.showGroupRow ? this.buildGroupHeader(groups) : ''}
    `;
  }

  private buildTableHeader(
    groups: any[],
    columns: Column[],
    config: TableScrollingConfigurations
  ): string {
    return `
          ${this.buildColumnsHeader(columns)}
    `;
  }

  private buildGroupHeader(groups: any[]): string {
    return `
 <table class="pdf-table">
           <thead class="pdf-header-table">
      <tr class="pdf-group-header-row">
        ${groups
      .map(
        (group) => `
          <th class="pdf-group-cell"
              colspan="${group.colspan}"
              style="width: ${group.width}">
            ${this.sanitizeContent(group.groupName)}
          </th>
        `
      )
      .join('')}
      </tr>
      </thead>
      </table>
    `;
  }

  private buildColumnsHeader(columns: Column[]): string {
    return `
      <tr class="pdf-column-header-row">
        ${columns
      .map((col) => {
        if (!col.print) return '';
        return `
              <th class="pdf-column-header-cell"
                  style="${this.styleToString(col.headerStyle || {})}">
                <div class="pdf-header-content">
                  ${this.sanitizeContent(col.title)}
                </div>
              </th>
            `;
      })
      .join('')}
      </tr>
    `;
  }

  private buildTableBody(
    rows: any[],
    columns: Column[],
    config: TableScrollingConfigurations
  ): string {

    return `
          ${rows
      .map(
        (row, index) => `
                <tr class="pdf-body-row"
                  style="${this.styleToString(row.style || {})} ,height: ${config.options?.itemSize || 40}px ">
                  ${columns
          .map((col) => this.buildBodyCell(col, row, index))
          .join('')}
                </tr>
             `
      )
      .join('')}


    `;
  }

  private buildBodyCell(col: Column, row: any, index: number): string {
    if (!col.print) return '';

    const rawValue = col.displayFn?.(row) ?? row[col.field];
    const formattedValue = this.formatCellValue(col, rawValue ?? '');
    const tooltip = this.sanitizeContent(rawValue);
    const cellStyle = this.styleToString(col.style || {});
    return `
      <td class="pdf-body-cell"
          title="${tooltip}"
          style="${cellStyle}"
          data-column-type="${col.type}">
        ${formattedValue}
      </td>
    `;
  }

  private styleToString(style: { [key: string]: string | number }): string {
    if (!style || typeof style !== 'object') return '';
    return Object.keys(style)
      .map(key => {
        const cssKey = key.replace(/([A-Z])/g, '-$1').toLowerCase();
        const cssValue = style[key].toString().replace('!important', ' !important');
        return `${cssKey}: ${cssValue};`;
      })
      .join(' ');
  }

  private formatCellValue(col: Column, value: any): string {

    switch (col.type) {
      case this.fieldTypes.Index:
        return `${value ?? ''}`;
      case this.fieldTypes.Text:
        return `${value ?? ''} `;
      case this.fieldTypes.Money:
        return `${this.customDecimal.transform(value, col.digitsInfo)} `;
      case this.fieldTypes.Number:
        return `${this.customDecimal.transform(value, col.digitsInfo)} `;

      case this.fieldTypes.Date:
        return `${this.toPersianDate.transform(value)} `;

      case this.fieldTypes.CheckBox:
        return `${value ? 'بله' : 'خیر'}`;
      case this.fieldTypes.Template:
        return col.displayPrintFun
          ? `${col.displayPrintFun(value)}` : '';
      default:
        return this.sanitizeContent(value ?? '');
    }
  }

  private buildTableFooter(
    rows: any[],
    columns: Column[],
    groups: any[],
    config: TableScrollingConfigurations
  ): string {
    return `
    <table class="pdf-footer-table pdf-table">
      <tfoot>
        ${config.options?.printSumRow ? this.buildSumFooter(columns, rows, config) : ''}
        ${config.options?.printGroupRemainingRow ? this.buildRemainingFooter(groups) : ''}
      </tfoot>
    </table>
  `
  }

  private buildSumFooter(columns: Column[], rows: any[], config: TableScrollingConfigurations): string {
    return `
    <tr class="pdf-sum-footer-row">
      ${columns.map((col, index) => {

            // ${this.sanitizeContent(this.tableHelper.getColumnSelectSum(rows))}
      if (col.type === this.fieldTypes.Index) {
        return `
          <td class="pdf-sum-footer-cell"
              style="${this.styleToString(col.footerStyle || {})}"
              data-column-type="${col.type}">
               ${config.options.sumLabel}
          </td>
        `;
      } else if ((col.type === this.fieldTypes.Money || col.type === this.fieldTypes.Number) && col.showSum && col.display) {
        const cellContent = col.display && col.showSum
          ? col.sumRowDisplayFn?.() ?? this.tableHelper.calculateColumnSum(col, rows)
          : '';

        const cellFormattedValue = this.customDecimal.transform(+cellContent, col.digitsInfo);
        return `
          <td class="pdf-sum-footer-cell"
              style="${this.styleToString(col.footerStyle || {})}"
              data-column-type="${col.type}">
            ${this.sanitizeContent(cellFormattedValue)}
          </td>

        `;
      }  else {
        return `
          <td class="pdf-sum-footer-cell"
              style="${this.styleToString(col.footerStyle || {})}"
              data-column-type="${col.type}">

          </td>

        `;
      }
    }).join('')}

    </tr>
  `;
  }

  private buildRemainingFooter(groups: any[]): string {
    return `
    <tr class="pdf-remaining-footer-row">
      ${groups.map(group => `
        <td class="pdf-group-footer-cell"
            colspan="${group.colspan}"
            style="width: ${group.width}">
          ${this.sanitizeContent(this.tableHelper.calculateColumnSumRemainingGroup(group))}
        </td>
      `).join('')}
    </tr>
  `;
  }

  private sanitizeContent(content: any): string {
    return this.sanitizer.sanitize(SecurityContext.HTML, content) || '';
  }

  private getPrintHeaderContainerStylesPortrait(): string {
    return "@page { size: A4 portrait; td,th{border-color: #000000}} ${this.fontFace} .pdf-table-export{ display: flex;flex-direction: column;justify-content: center;align-items: center;}  .header-container { page-break-before: avoid; display: flex; justify-content: space-between; align-items: center; height: 115px; width: 90%; padding: 5px; direction: rtl; font-family: 'IranYekan', Tahoma, Arial, sans-serif; } p, div { font-family: 'IranYekanBold', Tahoma, Arial, sans-serif; } .header-right, .header-center, .header-left { display: flex; flex-direction: column; align-items: center; justify-content: space-around; text-align: center; height: 100%; } .header-center { margin-top: 5px; margin-bottom: 5px; justify-content: space-between; } .header-right { margin-top: 15px; align-items: flex-start; text-align: right; } .header-left { margin-top: 15px; align-items: flex-start; text-align: right; } .fig-logo { display: flex; justify-content: center; align-items: center; margin-bottom: 5px; } .logo { width: 100%; max-height: 30px; } .date-from-to { font-size: 13px; font-weight: bold; color: #333; } .company-name { font-size: 18px; font-weight: bold; margin-bottom: 5px; } .current-year { font-size: 15px; font-weight: normal; color: #555; } .header-left { font-size: 13px; font-weight: bold; color: #333; }";
  }

  private getPrintHeaderContainerStylesLandscape(): string {
    return "@page { size: A4 landscape; td,th{border-color: #000000}} ${this.fontFace} .pdf-table-export{ display: flex;flex-direction: column;justify-content: center;align-items: center;}  .header-container { page-break-before: avoid; display: flex; justify-content: space-between; align-items: center; height: 115px; width: 90%; padding: 5px; direction: rtl; font-family: 'IranYekan', Tahoma, Arial, sans-serif; } p, div { font-family: 'IranYekanBold', Tahoma, Arial, sans-serif; } .header-right, .header-center, .header-left { display: flex; flex-direction: column; align-items: center; justify-content: space-around; text-align: center; height: 100%; } .header-center { margin-top: 5px; margin-bottom: 5px; justify-content: space-between; } .header-right { margin-top: 15px; align-items: flex-start; text-align: right; } .header-left { margin-top: 15px; align-items: flex-start; text-align: right; } .fig-logo { display: flex; justify-content: center; align-items: center; margin-bottom: 5px; } .logo { width: 100%; max-height: 50px; } .date-from-to { font-size: 14px; font-weight: bold; color: #333; } .company-name { font-size: 18px; font-weight: bold; margin-bottom: 5px; } .current-year { font-size: 16px; font-weight: normal; color: #555; } .header-left { font-size: 14px; font-weight: bold; color: #333; }"
  }

  private getPrintStyles(): string {
    return `${this.fontFace} ${this.getPrintHeaderContainerStylesLandscape()} body{font-family:'IranYekanBold',Tahoma,Arial,sans-serif;direction:rtl;text-align:right;width:100%;background:white;}*{padding:0;margin:0;vertical-align:middle;text-align:center;box-sizing:border-box;}.pdf-table-export{display:flex;flex-direction:column;justify-content:center;align-items:center;}.print-controls{display:flex;justify-content:center;gap:20px;margin-bottom:20px;margin-top:20px;}.btn{display:flex;justify-content:center;align-items:center;width:150px;height:40px;font-size:16px;cursor:pointer;border:none;border-radius:5px;}.btn-print{font-family:'IranYekanBold',Tahoma,Arial,sans-serif;background-color:#4CAF50;color:white;}.btn-close{font-family:'IranYekanBold',Tahoma,Arial,sans-serif;background-color:#000;color:white;}table{table-layout:fixed;border-collapse:collapse;border-spacing:0;padding:0;page-break-inside:auto;}.pdf-table-container{width:100%;display:flex;flex-direction:column;justify-content:center;align-items:center;}.pdf-header-content{min-height:30px;display:flex;justify-content:center;align-items:center;}.pdf-column-header-cell,.pdf-group-cell{background-color:#6d72ed;color:#fff;border:1px solid #000;vertical-align:middle;}.pdf-group-cell{height:30px;}.pdf-table{width:95%;border-collapse:collapse;page-break-inside:auto;}.pdf-body-table td{border:1px solid #000;vertical-align:middle;}.pdf-footer-table td,.pdf-sum-footer-cell,.pdf-group-footer-cell{background-color:#6d72ed;color:#fff;font-weight:bold;border:1px solid #000;vertical-align:middle;min-height:30px;}.pdf-remaining-footer-row,.pdf-sum-footer-row{min-height:30px;height:30px;}.page-number::after{content:"صفحه " counter(page);}@media print{-webkit-print-color-adjust:exact!important;print-color-adjust:exact!important;.pdf-table-container{page-break-inside:auto;}tr{page-break-inside:avoid;page-break-after:auto;}thead{display:table-header-group;}tfoot{display:table-footer-group;}.pdf-group-header-row{page-break-after:avoid;}.pdf-sum-footer-row{page-break-before:avoid;}.pdf-body-row{height:auto!important;min-height:40px;}table{border-collapse:collapse;width:100%;table-layout:fixed;}td,th{padding:4px;vertical-align:center;word-wrap:break-word;}body{-webkit-print-color-adjust:exact!important;print-color-adjust:exact!important;}.pdf-column-header-cell,.pdf-group-cell{background-color:#6d72ed!important;color:#fff!important;-webkit-print-color-adjust:exact!important;print-color-adjust:exact!important;}.pdf-footer-table td,.pdf-sum-footer-cell,.pdf-group-footer-cell{background-color:#6d72ed!important;color:#fff!important;-webkit-print-color-adjust:exact!important;print-color-adjust:exact!important;}@media print and (not (-webkit-print-color-adjust:exact)){.pdf-column-header-cell,.pdf-group-cell{background-color:#fff!important;color:#000!important;}.pdf-footer-table td,.pdf-sum-footer-cell,.pdf-group-footer-cell{background-color:#fff!important;color:#000!important;}}}.footer-container{display:flex;align-items: center;width: 100%;margin-top:15px} .footer-left{ display:flex; justify-content: flex-end;width: 95%;} .current-user{margin:0;font-size:14px;} `;
  }

  // فعلا به نتجیه ای نرسیدم برای استفاده از web worker
  // و pwa هم رو نتونستم به درستی نصب کنم
  async generatePDFReportWitWebWorker(data: {
    columns: Column[];
    rows: any[];
    groupsColumns: groupColumn[];
    groupsRemainingColumns: groupColumn[];
    config: TableScrollingConfigurations;
  }): Promise<void> {
    return new Promise((resolve, reject) => {
      const worker = new Worker(new URL('./pdf.worker', import.meta.url));

      // مدیریت پاسخ Worker
      worker.onmessage = ({data: response}) => {
        // اطمینان از وجود داده معتبر
        if (!response) {
          worker.terminate();
          reject('داده دریافتی نامعتبر است');
          return;
        }

        // پردازش نهایی و باز کردن پنجره
        try {

          // const htmlContent = this.buildFullTableHTML(data);
          const htmlContent = response.data;
          if (response.type === 'PDF_GENERATED') {
            // this.openPrintWindow(htmlContent, ' 2گزارش PDF');
          } else {
            alert('خطا در تولید گزارش');
          }
          worker.terminate();
          resolve();

        } catch (error) {
          worker.terminate();
          reject(error);
        }
      };

      // مدیریت خطاهای Worker
      worker.onerror = (error) => {
        console.error('خطا در Worker:', error);
        worker.terminate();
        reject(error);
      };
      // ارسال داده با ساختار مشخص
      worker.postMessage({
        type: 'GENERATE_PDF',
        payload: {
          columns: data.columns,
          rows: data.rows,
          groups: data.groupsColumns,
          config: data.config
        }
      });
      // تایم‌اوت برای جلوگیری از انتظار بی‌پایان
      setTimeout(() => {
        worker.terminate();
        reject('زمان تولید گزارش به پایان رسید');
      }, 30000); // 30 ثانیه تایم‌اوت
    });
  }

}
