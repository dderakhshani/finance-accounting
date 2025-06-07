import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'money'
})
export class MoneyPipe implements PipeTransform {

  transform(value: number, ...args: unknown[]): unknown {
    let num: any;
    num = +value;
    if (!num || num === 0) return '';
    if (num - Math.floor(num) !== 0) num = num.toFixed(2)

    num = num.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return num
  }
}
