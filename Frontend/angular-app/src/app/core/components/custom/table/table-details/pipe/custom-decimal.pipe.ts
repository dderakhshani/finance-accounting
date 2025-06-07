import { Pipe, PipeTransform } from '@angular/core';
import {DecimalFormat} from "../../models/decimal-format";
import {DecimalPipe} from "@angular/common";

@Pipe({
  name: 'customDecimal'
})
export class CustomDecimalPipe  implements PipeTransform {
  constructor(private decimalPipe: DecimalPipe) {}

  transform(value: number, format: string = 'default', locale: string = 'fa-IR'): string | null {
    if (format === DecimalFormat.None) {
      return value !== null && value !== undefined ? value.toString() : null;
    }
    let digitsInfo: string | undefined;

    switch (format) {
      case DecimalFormat.Default:
        digitsInfo = undefined;  // پیش فرض
        break;
      case DecimalFormat.Rounded:
        digitsInfo = '1.0-0';  // بدون اعشار
        break;
      case DecimalFormat.OneDecimal:
        digitsInfo = '1.1-1';  // یک رقم اعشار
        break;
      case DecimalFormat.TwoDecimals:
        digitsInfo = '1.1-2';  // دو رقم اعشار
        break;
      case DecimalFormat.ThreeDecimals:
        digitsInfo = '1.1-3';  // سه رقم اعشار
        break;
      case DecimalFormat.FourDecimals:
        digitsInfo = '1.1-4';  // چهار رقم اعشار
        break;
      default:
        digitsInfo = undefined;  // پیش‌فرض با چهار رقم اعشار
    }
    const formattedValue = this.decimalPipe.transform(Math.abs(value), digitsInfo);
    if (value < 0 && formattedValue !== null) {
      return `(${formattedValue})`;
    }else if(value === null){
      return '';
    }
    return formattedValue
  }

}
