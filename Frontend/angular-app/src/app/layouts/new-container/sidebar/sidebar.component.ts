import {Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {TableSettingsService} from "../../../core/components/custom/table/table-details/Service/table-settings.service";
import {Mediator} from "../../../core/services/mediator/mediator.service";
import {NavigationEnd, Router} from "@angular/router";
import {UserYear} from "../../../modules/identity/repositories/models/user-year";
import {UserRole} from "../../../modules/identity/repositories/models/user-role";
import {Language} from "../../../modules/admin/entities/language";
import {FormControl, FormGroup} from "@angular/forms";
import {NavigationItem} from "../../main-container/models/navigation-item";
import {GetLanguagesQuery} from "../../../modules/admin/repositories/languages/queries/get-languages-query";
import * as moment from "jalali-moment";
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {


  // for route changes
  currentRoute: string = '';


  @Input() navigations!: NavigationItem[];

  @Input() set flatNavigations(navItems: NavigationItem[]) {
    this._flatNavigations = navItems;
    this.activateCurrentMenu()
  };

  _flatNavigations: NavigationItem[] = []
  get flatNavigations() {
    return this._flatNavigations
  }

  @Output() sidebarItemClicked: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();

  @Input() isMinimized: boolean = false;

  currentDate!: string;
  currentDay!: string;
  currentHour!: string;

  applicationUserFullName!: string;
  avatarUrl!: string;

  allowedYears: UserYear[] = [];
  allowedRoles: UserRole[] = [];
  languages: Language[] = []
  currentYear!: UserYear;
  form = new FormGroup({
    yearId: new FormControl(),
    roleId: new FormControl(),
    languageId: new FormControl()
  });

  constructor(
    public identityService: IdentityService,
    private tableSettingsService: TableSettingsService,
    private _mediator: Mediator,
    private router: Router,
  ) {


     // listening to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.currentRoute = (event as NavigationEnd).urlAfterRedirects.split('?')[0];
        console.log(this.currentRoute);
      });

    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.applicationUserFullName = res.fullName;
        // this.avatarUrl = res.photoUrl;
        this.avatarUrl = '/assets/images/avatar-placeholder.jpg';
        this.allowedRoles = res.roles;
        this.allowedYears = res.years;
        this.currentYear = <UserYear>res.years.find(x => x.id == res.yearId)
        this.form.patchValue({
          yearId: +res.yearId,
          roleId: +res.roleId,
          languageId: +res.languageId
        });
      }
    });
  }

  activateCurrentMenu() {
    if (this.router.url !== '/') {
      let activeNavigation = this.flatNavigations.find(x => x.route === this.router.url.split('?')[0]);
      if (activeNavigation) {
        let ids: number[] = [];
        // @ts-ignore
        ids.push(activeNavigation.id);
        // @ts-ignore
        while (activeNavigation?.parentId) {
          // @ts-ignore
          ids.unshift(activeNavigation.parentId);
          // @ts-ignore
          activeNavigation = this.flatNavigations.find(x => x.id === activeNavigation.parentId);
        }
        let baseNavigation = this.navigations.find(x => x.id === ids[0]);
        // @ts-ignore
        baseNavigation.showChildren = true;
        // @ts-ignore
        ids.splice(0,1)

        while (ids.length > 0) {
          // @ts-ignore
          baseNavigation = baseNavigation.children.find(x => x.id === ids[0]);
          // @ts-ignore
          baseNavigation?.children?.length > 0 ? baseNavigation.showChildren = true : ''
          // @ts-ignore
          ids.splice(0,1)
        }

      }
    }
  }

  async ngOnInit() {
    await this._mediator.send(new GetLanguagesQuery()).then(res => {
      this.languages = res.data
    })
    this.setTime();

  }

  setTime() {
    this.currentDate = moment(new Date()).locale('fa').format('YYYY/M/D');
    this.currentHour = moment(new Date()).locale('fa').format('HH:mm:ss');
    this.currentDay = moment(new Date()).locale('fa').format('dddd');

    let that = this;
    setTimeout(() => {
      that.setTime();
    }, 1000);
  }

  async handleYearChange(yearId: number) {
    this.tableSettingsService.deleteEntireDatabase('RequestsCache')
    await this.identityService.refreshToken(undefined, undefined, yearId)
  }

  async handleRoleChange(roleId: number) {
    // this.tableSettingsService.deleteEntireDatabase();
    await this.identityService.refreshToken(undefined, roleId, undefined)
  }

  async handleLanguageChange(languageId: number) {
    await this.identityService.refreshToken(undefined, undefined, undefined, languageId)
  }

  closeAllMenus(items: NavigationItem[]) {
    items.forEach(item => {
      item.showChildren = false;
      if (item.children && item.children.length > 0) {
        this.closeAllMenus(item.children);
      }
    });
  }

  itemClicked(item: NavigationItem) {
    this.closeAllMenus(this.navigations);
    this.sidebarItemClicked.emit(item)
  }

  collapseOtherSiblings(navItem: NavigationItem) {
    this.navigations.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }

  // toggle(ev: boolean) {
  //   if (!ev){
  //     this.isMinimized = !ev
  //     this.toggled.emit(!ev)
  //   }
  // }
  openLink( ref: string) {

    const urls: { [key: string]: string } = {
      'kasra': 'http://192.168.2.246/lego.web/Kevlar/Account/Login?ReturnUrl=%2Flego.web%2F',
      'sina': 'http://sina.eefaceram.com/prime/User/LoginView',
      'ERP': 'https://erp.eefaceram.com/'
    };

    const url = urls[ref];

    if (url) {
      window.open(url, '_blank');
    } else {
      console.warn('Invalid reference passed to openLink:', ref);
    }

  }

}
