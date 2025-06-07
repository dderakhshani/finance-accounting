import {CustomFont} from "../pdfmakefonts";
import {TableColumnDataType} from "../../../models/table-column-data-type";


declare function postMessage(message: any): void;



interface WorkerRequest {
  type: string;
  payload: any;
}

interface WorkerResponse {
  type: string;
  data: any;
}

addEventListener('message', ({ data }: MessageEvent<WorkerRequest>) => {
  try {
    if (data.type !== 'GENERATE_PDF') return;

    // شبیه‌سازی پردازش زمان‌بر
    const result = complexPDFGeneration(data.payload);

    const response: WorkerResponse = {
      type: 'PDF_GENERATED',
      data: result
    };

    postMessage(response);
  } catch (error) {
    const errorResponse: WorkerResponse = {
      type: 'ERROR',
      data: error instanceof Error ? error.message : 'خطای ناشناخته'
    };
    postMessage(errorResponse);
  }
});

function complexPDFGeneration(payload: any): string {
  const service = CustomFont;
  const fontFace = service.Font.fontFace;

  // پیاده‌سازی منطق تولید PDF

   return `
      <div class="pdf-table-container" dir="rtl" >
<p> سلام رضا </p>
${buildFullTableHTML(payload)}
      </div>
    `;
}
function buildFullTableHTML(data: {
  columns: any[];
  rows: any[];
  groupsColumns: any[];
  groupsRemainingColumns: any[];
  config: any;
}): string {

  return `
      <div class="pdf-table-container" dir="rtl" xmlns="http://www.w3.org/1999/html">


            ${buildHeaderContainer(data.groupsColumns, data.columns, data.config)}
    ${buildTableHeaderGroups(data.groupsColumns, data.columns, data.config)}
  <table class="pdf-table">
   <thead class="pdf-header-table">
     ${buildTableHeader(data.groupsColumns, data.columns, data.config)}
     </thead>

<tbody class="pdf-body-table">
${buildTableBody(data.rows, data.columns, data.config)}
</tbody>
</table>


      </div>
    `;
}
function buildHeaderContainer(
  groups: any[],
  columns: any[],
  config: any
): string {
  return `
         <div class="header-container">

      <div class="header-right">

      <div class="fig-logo">

        <img src="/assets/images/Reportlogo.jpg" class="logo" alt="لوگو">
        </div>

        <div class="date-from-to">
        از تاریخ : ${new Date().toLocaleDateString('fa-IR')}
        تا تاریخ : ${new Date().toLocaleDateString('fa-IR')}
        </div>

      </div>
      <div class="header-center">
       <p class="company-name">
        شرکت ایفا سرام (سهامی خاص)
</p>
<p class="current-year">
        سال مالی: ${config.options?.defaultSortKey}
        </p>
      </div>
      <div class="header-left">

      <p class="current-user">
      کاربر چاپ کننده : ${config.options?.defaultSortKey}
        </p>
        <p class="date-report">
        تاریخ چاپ گزارش: ${new Date().toLocaleDateString('fa-IR')}
        </p>
      </div>
    </div>
    `;
}

function buildTableHeaderGroups(
  groups: any[],
  columns: any[],
  config: any
): string {
  return `
           ${config.options?.showGroupRow ? buildGroupHeader(groups) : ''}
    `;
}

function buildTableHeader(
  groups: any[],
  columns: any[],
  config: any
): string {
  return `
          ${buildColumnsHeader(columns)}
    `;
}

function buildGroupHeader(groups: any[]): string {
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
            ${sanitizeContent(group.groupName)}
          </th>
        `
    )
    .join('')}
      </tr>
      </thead>
      </table>
    `;
}

function buildColumnsHeader(columns: any[]): string {
  return `
      <tr class="pdf-column-header-row">
        ${columns
    .map((col) => {
      if (!col.display) return '';
      return `
              <th class="pdf-column-header-cell"
                  style="${styleToString(col.headerStyle || {})}">
                <div class="pdf-header-content">
                  ${sanitizeContent(col.title)}
                </div>
              </th>
            `;
    })
    .join('')}
      </tr>
    `;
}

function buildTableBody(
  rows: any[],
  columns: any[],
  config: any
): string {
  return `

          ${rows
    .map(
      (row, index) => `
                <tr class="pdf-body-row"
                    style="height: ${config.options?.itemSize || 40}px">
                  ${columns
        .map((col) => buildBodyCell(col, row, index))
        .join('')}
                </tr>
              `
    )
    .join('')}


    `;
}

function buildBodyCell(col: any, row: any, index: number): string {
  if (!col.display) return '';

  const rawValue = col.displayFn?.(row) ?? row[col.field];
  const formattedValue = formatCellValue(col, rawValue);
  const tooltip = sanitizeContent(rawValue);
  const cellStyle = styleToString(col.style || {});
  return `
      <td class="pdf-body-cell"
          title="${tooltip}"
          style="${cellStyle}"
          data-column-type="${col.type}">
        ${formattedValue}
      </td>
    `;
}

function styleToString(style: { [key: string]: string | number }): string {
  if (!style || typeof style !== 'object') return '';

  return Object.keys(style)
    .map(key => {
      const cssKey = key.replace(/([A-Z])/g, '-$1').toLowerCase();
      const cssValue = style[key].toString().replace('!important', ' !important');
      return `${cssKey}: ${cssValue};`;
    })
    .join(' ');
}
//
function formatCellValue(col: any, value: any): string {
  let fieldTypes = TableColumnDataType;
  switch (col.type) {
    case fieldTypes.Index:
      return `${value}`;

    case fieldTypes.Money:
      return `${formatNumber(value, col.digitsInfo)} `;
    case fieldTypes.Number:
      return `${formatNumber(value, col.digitsInfo)} `;

    case fieldTypes.Date:
    // return `${toPersianDate(value)} `;

    case fieldTypes.CheckBox:
      return `${value ? 'بله' : 'خیر'}`;

    default:
      return sanitizeContent(value);
  }
}

// function buildTableFooter(
//   rows: any[],
//   columns: any[],
//   groups: any[],
//   config: any
// ): string {
//   return `
//     <table class="pdf-footer-table pdf-table">
//       <tfoot>
//         ${config.options?.showSumRow ? buildSumFooter(rows, columns) : ''}
//       ${config.options?.showGroupRemainingRow ? buildRemainingFooter(groups) : ''}
//       </tfoot>
//     </table>
//   `;
// }
//
// function buildSumFooter(rows: any[], columns: any[]): string {
//   let fieldTypes = TableColumnDataType;
//
//   return `
//     <tr class="pdf-sum-footer-row">
//       ${columns.map(col => {
//
//     if (col.type === fieldTypes.Index) {
//       return `
//           <td class="pdf-sum-footer-cell"
//               style="${styleToString(col.footerStyle || {})}"
//               data-column-type="${col.type}">
//             ${sanitizeContent(getColumnSelectSum(rows))}
//           </td>
//
//         `;
//     } else if ((col.type === fieldTypes.Money || col.type === fieldTypes.Number) && col.showSum && col.display) {
//
//
//       const cellContent = col.display && col.showSum
//         ? col.sumRowDisplayFn?.() ?? calculateColumnSum(rows, col)
//         : '';
//
//       const cellFormattedValue = formatNumber(+cellContent, col.digitsInfo);
//       return `
//           <td class="pdf-sum-footer-cell"
//               style="${styleToString(col.footerStyle || {})}"
//               data-column-type="${col.type}">
//             ${sanitizeContent(cellFormattedValue)}
//           </td>
//
//         `;
//     } else {
//       return `
//           <td class="pdf-sum-footer-cell"
//               style="${styleToString(col.footerStyle || {})}"
//               data-column-type="${col.type}">
//
//           </td>
//
//         `;
//     }
//   }).join('')}
//
//     </tr>
//   `;
// }
//
// function buildRemainingFooter(groups: any[]): string {
//   return `
//     <tr class="pdf-remaining-footer-row">
//       ${groups.map(group => `
//         <td class="pdf-group-footer-cell"
//             colspan="${group.colspan}"
//             style="width: ${group.width}">
//           ${sanitizeContent(calculateColumnSumRemainingGroup(group))}
//         </td>
//       `).join('')}
//     </tr>
//   `;
// }
//
function sanitizeContent(content: any): string {

  return content || '';
}

function getPrintHeaderContainerStyles(fontFace: any): string {

  return `
     ${fontFace}
    .header-container {
     page-break-before: avoid;
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 15px;
  height : 115px;
  width: 95%;
  padding: 5px;
  direction: rtl;
  font-family: 'IranYekan', Tahoma, Arial, sans-serif;
}
p , div{
  font-family: 'IranYekanBold', Tahoma, Arial, sans-serif;
  }

.header-right,
.header-center,
.header-left {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-around;
  text-align: center;
  height: 100%;
}

.header-center{
margin-top : 5px;
margin-bottom : 5px;
 justify-content: space-between;
}
.header-right {
margin-top : 30px;
  align-items: flex-start;
  text-align: right;
}

.header-left {
margin-top : 30px;
  align-items: flex-start;
  text-align: right;
}

.fig-logo {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 5px;
}

.logo {
  width: 100%;
  max-height: 50px;
}

.date-from-to {
  font-size: 14px;
  font-weight: bold;
  color: #333;
}

.company-name {
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 5px;
}

.current-year {
  font-size: 16px;
  font-weight: normal;
  color: #555;
}

.header-left {
  font-size: 14px;
  font-weight: bold;
  color: #333;
}

@media print {
@page {
    size: A4 landscape;
    margin: 10mm;
  }


  .pdf-table-container {
    page-break-inside: auto;
  }

  tr {
    page-break-inside: avoid;
    page-break-after: auto;
  }


  thead {
    display: table-header-group;
  }

  tfoot {
    display: table-footer-group;
  }


  .pdf-group-header-row {
    page-break-after: avoid;
  }

  .pdf-sum-footer-row {
    page-break-before: avoid;
  }


  .pdf-body-row {
    height: auto !important;
    min-height: 40px;
  }


  table {
    border-collapse: collapse;
    width: 100%;
    table-layout: fixed;
  }

  td, th {
    padding: 4px;
    vertical-align: center;
    word-wrap: break-word;
  }
}

    `
}

function getPrintStyles(): string {
  let fontFace = CustomFont.Font.fontFace;
  return `

       @page {
            size: A4 landscape;

            margin: 10mm;


          }


          ${fontFace}
          ${getPrintHeaderContainerStyles(fontFace)}
          body {
            font-family: 'IranYekanBold', Tahoma, Arial, sans-serif;
            direction: rtl;
            text-align: right;
            width: 100%;

             background: white;
          }

        *{
        padding: 0;
        margin: 0;
        vertical-align: middle;
        text-align: center;
         box-sizing: border-box;
        }
table {

  table-layout: fixed;
  border-collapse: collapse;
  border-spacing: 0;
  padding: 0;

 page-break-inside: auto;
}
      .pdf-table-container {
        width: 100%;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;


      }


      .pdf-header-content{




        min-height: 30px;
        display: flex;
        justify-content: center;
        align-items: center;
      }
      .pdf-column-header-cell ,.pdf-group-cell{
  background-color: #6d72ed;
  color : white;
     border: 1px solid #dee2e6;
    vertical-align: middle;
}
      .pdf-group-cell{
      height: 30px;
      }
      .pdf-table{
        width: 95%;
        border-collapse: collapse;
        page-break-inside: auto;
      }

      .pdf-body-table td {

        border: 1px solid #dee2e6;
        vertical-align: middle;
      }

      .pdf-footer-table td ,.pdf-sum-footer-cell ,.pdf-group-footer-cell{
        background-color: #6d72ed;
        color : white;
        font-weight: bold;
         border: 1px solid #dee2e6;
        vertical-align: middle;
         min-height: 30px;
      }
.pdf-remaining-footer-row , .pdf-sum-footer-row{
 min-height: 30px;
 height: 30px;
}
.page-number::after {
        content: "صفحه " counter(page);
      }
    `;
}
//
//
// function getSelectedItems(tableRows: any[]): any[] {
//   if (!tableRows) return [];
//   return tableRows.filter(item => item.selected === true);
// }
//
// function getColumnSelectSum(tableRows: any[]) {
//   const selectCols = getSelectedItems(tableRows);
//   if (selectCols.length > 0) {
//     return selectCols.length;
//   } else {
//     return tableRows.length;
//   }
// }
//
// function calculateColumnSum(tableRows: any[], column: any): number {
//   const selectCols = getSelectedItems(tableRows);
//   if (selectCols.length > 0) {
//     column.sumColumnValue = selectCols.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0);
//
//   } else {
//     column.sumColumnValue = tableRows.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0)
//
//   }
//   return column.sumColumnValue || 0;
// }
//
// function calculateColumnSumRemainingGroup(group: groupColumn): string {
//   if (typeof group.groupName === 'string') {
//
//     return group.groupName;
//   } else if (typeof group.groupName === 'function') {
//
//     const result = group.groupName();
//     const numericResult = parseFloat(result);
//
//
//     if (!isNaN(numericResult)) {
//       const formattedValue = formatNumber(
//         numericResult,
//         'default',
//       );
//
//       if (numericResult < 0) {
//         return `${formattedValue}`;
//       }
//       return formattedValue || '';
//     }
//
//     return result;
//   }
//
//
//   return 'Unknown';
// }

function formatNumber(value: any, format = 'default', locale = 'fa-IR') {
  if (value === null || value === undefined) return null;

  let digitsInfo;
  switch (format) {
    case 'None':
      return value.toString();
    case 'Rounded':
      digitsInfo = 0;
      break;
    case 'OneDecimal':
      digitsInfo = 1;
      break;
    case 'TwoDecimals':
      digitsInfo = 2;
      break;
    case 'ThreeDecimals':
      digitsInfo = 3;
      break;
    case 'FourDecimals':
      digitsInfo = 4;
      break;
    default:
      digitsInfo = 2; // پیش‌فرض دو رقم اعشار
  }

  // اصلاح مقدار عددی
  const absValue = Number(Math.abs(value).toFixed(digitsInfo));
  const formattedValue = new Intl.NumberFormat(locale).format(absValue);

  return value < 0 ? `(${formattedValue})` : formattedValue;
}

// function toPersianDate(value: any, includeHoursAndSeconds: boolean = false): string {
//     if (!value) return '';
//
//     return moment(moment.utc(new Date(value)).toDate())
//         .locale('fa')
//         .format((includeHoursAndSeconds ? 'HH:mm ' : '') + 'jYYYY/jMM/jDD');
// }


