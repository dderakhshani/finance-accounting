import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import * as moment from "jalali-moment";

import {UserYear} from "../../../modules/identity/repositories/models/user-year";
import {UserRole} from "../../../modules/identity/repositories/models/user-role";
import {FormControl, FormGroup} from "@angular/forms";
import {Language} from "../../../modules/admin/entities/language";
import {Mediator} from "../../../core/services/mediator/mediator.service";
import {GetLanguagesQuery} from "../../../modules/admin/repositories/languages/queries/get-languages-query";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Output() toggled: EventEmitter<boolean> = new EventEmitter<boolean>()
  @Input() isToggled:boolean = false;


  currentDate!: string;
  currentDay!: string;
  currentHour!: string;

  applicationUserFullName!: string;
  avatarUrl: string = '/assets/images/avatar-placeholder.jpg';

  allowedYears: UserYear[] = [];
  allowedRoles: UserRole[] = [];
  languages: Language[] = []

  form = new FormGroup({
    yearId: new FormControl(),
    roleId: new FormControl(),
    languageId: new FormControl()
  });

  constructor(
    public identityService: IdentityService,
    private _mediator: Mediator
  ) {

    identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.applicationUserFullName = res.fullName;
        this.avatarUrl = res.photoUrl ? res.photoUrl : this.avatarUrl;
        this.allowedRoles = res.roles;
        this.allowedYears = res.years;

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
    this.currentHour = moment(new Date()).locale('fa').format('HH:mm:ss');
    this.currentDay = moment(new Date()).locale('fa').format('dddd');

    let that = this;
    setTimeout(() => {
      that.setTime();
    }, 1000);
  }

  async handleYearChange(yearId: number) {
    await this.identityService.refreshToken(undefined, undefined, yearId)
  }

  async handleRoleChange(roleId: number) {
    await this.identityService.refreshToken(undefined, roleId, undefined)
  }

  async handleLanguageChange(languageId: number) {
    await this.identityService.refreshToken(undefined, undefined, undefined, languageId)
  }

  toggle() {
    this.toggled.emit()
  }
}
