import {Injectable} from '@angular/core';
import {HttpService} from "../../../core/services/http/http.service";
import {Router} from "@angular/router";
import {ExtensionsService} from "../../../shared/services/extensions/extensions.service";
import {LoginDTO} from "./dto/login-dto";
import {environment} from "../../../../environments/environment";
import {BehaviorSubject, Observable} from "rxjs";
import {HttpRequest} from "../../../core/services/http/http-request";
import {ServiceResult} from "../../../core/models/service-result";
import {ApplicationUser} from "./application-user";
import {LocalStorageRepository} from "../../../core/services/storage/local-storage-repository.service";
import {NavigationItem} from "../../../layouts/main-container/models/navigation-item";
import {UserProfile} from "./models/user-profile";
import {UserYear} from "./models/user-year";
import {Toastr_Service} from "../../../shared/services/toastrService/toastr_.service";


@Injectable({
  providedIn: 'root'
})
export class IdentityService {
  public isInitialized = false;
  public isTryingToRefreshToken = false;
  readonly _applicationUser = new BehaviorSubject<ApplicationUser>(new ApplicationUser());
  public get applicationUser() {
    return this._applicationUser.value;
  }


  constructor(
    private httpService: HttpService,
    private cookieService: LocalStorageRepository,
    private extensionsService: ExtensionsService,
    private toastr: Toastr_Service,
    private router: Router
  ) {
  }

  public async init() {
    if (!this.isInitialized) {
      this.isInitialized = true;
      return await this.setApplicationUserFromToken()
    }
  }

  isAuthenticated() {
    return this.applicationUser.isAuthenticated
  }

  async setApplicationUserFromToken(token?: string | null) {
    // if user passed in token, decoding and retrieving information, but if not trying to read token from storage
    if (token) {
      this.cookieService.set('token', token, 1);
    } else {
      token = this.cookieService.get('token');
      if (!token) {
        return this.router.navigateByUrl('/authentication');
      }
    }

    let decodedUserInformation = this.decodeJWT(token);

    // if token is expired, trying to refresh token
    // @ts-ignore
    if (decodedUserInformation.exp === 1703724187) {
      // if (decodedUserInformation.exp < (new Date().getTime() / 1000)) {
      return await this.refreshToken();
    }

    // set user information
    let applicationUser = Object.assign(new ApplicationUser(), decodedUserInformation);
    applicationUser.token = token;
    applicationUser.photoUrl = applicationUser.userAvatarReletiveAddress ? environment.apiURL + "/" + applicationUser.userAvatarReletiveAddress : '/assets/images/avatar-placeholder.jpg';

    // set sidebar navigations and header years,roles and companies
    return this.getProfile().subscribe((res) => {


      applicationUser.roles = res.objResult.roles;
      applicationUser.years = res.objResult.years.map(x => {
        x.firstDate = new Date(x.firstDate);
        x.lastDate = new Date(x.lastDate);
        return x;
      });
      applicationUser.companies = res.objResult.companies
      applicationUser.permissions = res.objResult.permissions
      applicationUser.menus = res.objResult.menus;

      applicationUser.navigations = res.objResult.menus.map(menuItem => {
        let navigationItem = new NavigationItem();
        navigationItem.id = menuItem.id;
        navigationItem.title = menuItem.title;
        navigationItem.parentId = menuItem.parentId;
        navigationItem.route = menuItem.formUrl;
        navigationItem.permissionId = menuItem.permissionId;
        navigationItem.children = [];
        navigationItem.imageUrl = menuItem.imageUrl;
        return navigationItem;
      });


      applicationUser.isAuthenticated = true;


      this._applicationUser.next(applicationUser);

      return this.applicationUser;
    });
  }

  login(dto: LoginDTO): Observable<ServiceResult<string>> {

    let request: HttpRequest<LoginDTO> = new HttpRequest<LoginDTO>('/Identity/Login', dto, false);

    return this.httpService.Post<LoginDTO, ServiceResult<string>>(request)
  }

  async logout() {
    // this.httpService.Post<any, ServiceResult<boolean>>(
    //   new HttpRequest<any>('/Identity/LogOut')
    // ).toPromise().then();

    this.cookieService.delete('token');
    this._applicationUser.next(new ApplicationUser());
    await this.router.navigateByUrl('/authentication');
    return location.reload();

  }

  async refreshToken(companyId?: number, roleId?: number, yearId?: number, languageId?: number) {
    if (!this.isTryingToRefreshToken) {
      this.isTryingToRefreshToken = true
      let body = {
        companyId: companyId ?? this.applicationUser.companyId,
        roleId: roleId ?? this.applicationUser.roleId,
        yearId: yearId ?? this.applicationUser.yearId,
        languageId: languageId ?? this.applicationUser.languageId
      };
      let request = new HttpRequest<any>('/identity/refreshToken', body);
      return await this.httpService.Post<any, ServiceResult<string>>(request).toPromise().then(async (res) => {
        this.setApplicationUserFromToken(res.objResult)
        this.isTryingToRefreshToken = false;
        await this.router.navigateByUrl('/');
        return location.reload();

      }).catch(() => {
        this.cookieService.delete('token');
        this.router.navigateByUrl('/authentication');
      });
    }
  }

  private getProfile(): Observable<ServiceResult<UserProfile>> {
    let request = new HttpRequest<any>('/identity/profile');
    return this.httpService.Get<ServiceResult<UserProfile>>(request);
  }

  decodeJWT(token: string): string {

    let base64Url = token.split('.')[1];
    let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    let jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  }


  doesHavePermission(permissionuniqueName: string) {
    let permission = this.applicationUser?.permissions?.find(x => x.uniqueName === permissionuniqueName)
    return permission ? true : false
  }


  getActiveYearStartDate(): Date {
    let date = this.applicationUser.years.find(x => x.id == this.applicationUser.yearId)?.firstDate;
    if (date) {
      return date;
    } else {
      return new Date()
    }

  }
  getActiveYearlastDate(): Date {
    let date = this.applicationUser.years.find(x => x.id == this.applicationUser.yearId)?.lastDate;
    if (date) {
      return date;
    } else {
      return new Date()
    }

  }

  getActiveYearEndDate() {
    let date = this.applicationUser.years.find(x => x.id == this.applicationUser.yearId)?.lastDate;
    if (date) {
      return date;
    } else {
      return new Date()
    }
  }
  findYearIdsByDates(DateFrom: any, DateTo: any, showToast: boolean = true): number[] {
    const userYears: UserYear[] = this.applicationUser.years;
    const currentYearId = this.applicationUser.years.find(x => x.isCurrentYear)?.id;
    const yearIds: number[] = [];
    let message = '';
    // تبدیل تاریخها و بررسی اعتبار
    const fromDate = new Date(DateFrom);
    const toDate = new Date(DateTo);
    if (isNaN(fromDate.getTime()) || isNaN(toDate.getTime())) {
      console.error('تاریخهای ورودی نامعتبر هستند!');
      if (showToast) {
        this.toastr.showToast({message: 'تاریخهای ورودی نامعتبر هستند!', title: '', type: 'warning'});
      }
      return [];
    }

    // محاسبه همپوشانی سالها
    const utcDateFrom = fromDate.getTime();
    const utcDateTo = toDate.getTime();
    if (utcDateFrom > utcDateTo) {
      message = 'تاریخ شروع از پایان بزرگ تر است.';
      this.toastr.showToast({message: message, title: '', type: 'warning'});
      return [];
    }

    // جمعآوری شناسههای سالهای همپوشان
    userYears.forEach((year) => {
      const yearStart = new Date(year.firstDate).getTime();
      const yearEnd = new Date(year.lastDate).getTime();
      if (utcDateFrom <= yearEnd && utcDateTo >= yearStart) {
        yearIds.push(year.id);
      }
    });

    // حذف شناسههای تکراری
    const uniqueYearIds = [...new Set(yearIds)];

    // نمایش پیامهای اطلاعرسانی
    if (showToast) {

      if (currentYearId && !uniqueYearIds.includes(currentYearId)) {
        if (uniqueYearIds.length === 1) {
          message = 'این بازه زمانی خارج از سال مالی جاری است.';
          // this.toastr.showToast({message: message, title: '', type: 'info'});
        } else if (uniqueYearIds.length > 1) {
          message = 'این بازه زمانی انتخابی بیش از یک سال است.';
          this.toastr.showToast({message: message, title: '', type: 'info'});
        }
      } else if (uniqueYearIds.length > 1) {
        message = 'این بازه زمانی انتخابی بیش از یک سال است.';
        this.toastr.showToast({message: message, title: '', type: 'info'});
      }
    }

    return uniqueYearIds;
  }
  getToken() {
    let token = this.cookieService.get('token');
    return token;
  }

}
