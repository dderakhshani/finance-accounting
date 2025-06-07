import {Injectable} from '@angular/core';
import {UserYear} from "../../../modules/identity/repositories/models/user-year";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {ToPersianDatePipe} from "../../../core/pipes/to-persian-date.pipe";
import {SearchQuery} from "../search/models/search-query";

@Injectable({
  providedIn: 'root'
})
export class SSRSReportService {
  applicationUserFullName!: string;
  allowedYears: UserYear[] = [];
  currentYear!: UserYear;
  companyName = "شرکت ایفاسرام (سهامی خاص)";
  yearName = 'سال مالی' ;
  reportTime = new ToPersianDatePipe().transform(new Date().toISOString(),'toPersianDate');
  userName = 'کاربر ایفا';
  _reportType = 1 ;
  _companyId=1;
  constructor(private identityService: IdentityService) {
    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.userName = res.fullName ?? 'کاربر ایفا';
        this.allowedYears = res.years;
        this.currentYear = <UserYear>res.years.find(x => x.id == res.yearId);
        this.yearName = 'سال مالی' + this.currentYear.yearName;
      }
    });
  }
   addParamIfDefined(paramName: string, value: any, isArray: boolean = false): string {
    if (isArray && Array.isArray(value) && value.length > 0) {
      return `&${paramName}=[${value.join(',')}]`;
    } else if (!isArray&& value && value!== '' && value  !== undefined && value !== null) {
      return `&${paramName}=${value}`;
    }
    return '';
  }
   buildConditionsString(conditions?: SearchQuery[]): string {
    if (!conditions || conditions.length === 0) {
      return '';
    }

    return conditions
      .map(condition => {
        let valueString = Array.isArray(condition.values)
          ? condition.values.join(',')
          : (condition.values as string | number).toString();

        return `${condition.propertyName} ${':'} ${valueString}`;
      })
      .join(`&`);
  }

}
