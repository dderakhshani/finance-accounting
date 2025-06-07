import { Injectable } from '@angular/core';
import {Column, groupColumn} from "../../models/column";
import {CustomDecimalPipe} from "../pipe/custom-decimal.pipe";
import {ToPersianDatePipe} from "../../../../../pipes/to-persian-date.pipe";
import {TableColumnDataType} from "../../models/table-column-data-type";
import {IdentityService} from "../../../../../../modules/identity/repositories/identity.service";
@Injectable({
  providedIn: 'root'
})
export class TableVirtualScrollHelperService {
  fieldTypes = TableColumnDataType;
  userInfo !:any;
  yearsLabel!:string;
  constructor( private customDecimal: CustomDecimalPipe,
               private toPersianDate: ToPersianDatePipe,
               public identityService: IdentityService,

               ) {

    setTimeout(
      () => {
        this.userInfo = this.identityService.applicationUser;
      },1000
    )
  }
  getSelectedItems( rows: any[]) {
    return rows.filter(item => item.selected === true);
  }

  getColumnSelectSum( rows: any[]) {
    const selectCols = this.getSelectedItems(rows);
    if (selectCols.length > 0) {
      return selectCols.length;
    } else {
      return rows.length;
    }
  }

  calculateColumnSum(column: Column , rows: any[]): number {
    const selectCols = this.getSelectedItems(rows);
    if (selectCols.length > 0) {
      column.sumColumnValue = selectCols.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0);
    } else {
      column.sumColumnValue = rows.reduce((sum, item) => sum + (item[column.field || ''] || 0), 0)
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
        const formattedValue = this.customDecimal.transform(
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



  filterRowSelection(rows: any[], selectedRowIds: any[]): any[] {
    if (!selectedRowIds || selectedRowIds.length === 0) {
      return rows;
    }
    const selectedSet = new Set(selectedRowIds);
    return rows
      .filter(row => selectedSet.has(row.id))
      .map((row, index) => ({ ...row, rowIndex: index+1 }));
  }
  genYearLabel(dateFrom:any ,dateTo :any):string{
    let ids = this.identityService.findYearIdsByDates(dateFrom, dateTo , false) || [];
    let yearName = this.userInfo.years.filter((x: any) => ids.includes(x.id)).map((x: any) => x.yearName).join(',');

    return yearName
  }
}
