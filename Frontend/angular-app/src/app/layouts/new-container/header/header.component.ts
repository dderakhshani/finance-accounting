import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {TableSettingsService} from "../../../core/components/custom/table/table-details/Service/table-settings.service";
import {Mediator} from "../../../core/services/mediator/mediator.service";
import { Router} from "@angular/router";
import {UserYear} from "../../../modules/identity/repositories/models/user-year";
import {UserRole} from "../../../modules/identity/repositories/models/user-role";
import {Language} from "../../../modules/admin/entities/language";
import {GetLanguagesQuery} from "../../../modules/admin/repositories/languages/queries/get-languages-query";
import * as moment from "jalali-moment";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  @Input() isMinimized : boolean = false;

  @Input() isSettingToggled : boolean = false;

  @Output() settingToggled = new EventEmitter<boolean>();

  @Output() toggled: EventEmitter<boolean> = new EventEmitter<boolean>()

  applicationUserFullName!: string;
  avatarUrl!: string;

  currentDate!: string;
  currentDay!: string;
  currentTime!: string;
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

  async ngOnInit() {
    await this._mediator.send(new GetLanguagesQuery()).then(res => {
      this.languages = res.data
    })
    this.setTime();

  }

  setTime() {
    this.currentDate = moment(new Date()).locale('fa').format('YYYY/M/D');
    this.currentTime = moment(new Date()).locale('fa').format('HH:mm');
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

  getAbbreviationName(fullName: string): string {
    if (!fullName) return '';
    const parts = fullName.trim().split(' ');
    const first = parts[0]?.charAt(0).toUpperCase() || '';
    const last = parts[1]?.charAt(0).toUpperCase() || '';
    return first + "." +  last;
  }

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
