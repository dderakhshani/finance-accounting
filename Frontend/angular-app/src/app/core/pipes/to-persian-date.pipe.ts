import {Pipe, PipeTransform} from '@angular/core';
import * as moment from "jalali-moment";

@Pipe({
  name: 'toPersianDate'
})
export class ToPersianDatePipe implements PipeTransform {
  transform(value: unknown, ...args: unknown[]): unknown {
    let includeHoursAndSeconds = args[0];
    if (value) {
      return moment(moment.utc(<Date>value).toDate()).locale('fa').format((includeHoursAndSeconds ? 'HH:mm' : '') + ' jYYYY/jMM/jDD');
    } else {
      return ''
    }
  }

}
