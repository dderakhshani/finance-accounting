import { Component, OnInit } from '@angular/core';

// @ts-ignore

import { FormControlDirective, FormControlName } from "@angular/forms";
import { RepositoryService } from "./core/services/data/repository.service";
import { HostListener } from "@angular/core";
import { IdentityService } from './modules/identity/repositories/identity.service';
import { Router } from '@angular/router';

const originFormControlNgOnChanges = FormControlDirective.prototype.ngOnChanges;
FormControlDirective.prototype.ngOnChanges = function () {

  // @ts-ignore
  originFormControlNgOnChanges.apply(this, arguments);
  // @ts-ignore
  if (!this.form.nativeElement) this.form.nativeElement = this.valueAccessor?._elementRef?.nativeElement ?? this.valueAccessor?._element?.nativeElement ?? this.valueAccessor?.elementRef?.nativeElement ?? this.valueAccessor?._maskService?._formElement;
};

const originFormControlNameNgOnChanges = FormControlName.prototype.ngOnChanges;
FormControlName.prototype.ngOnChanges = function () {
  // @ts-ignore
  originFormControlNameNgOnChanges.apply(this, arguments);
  // @ts-ignore
  if (!this.control.nativeElement) this.control.nativeElement = this.valueAccessor?._elementRef?.nativeElement ?? this.valueAccessor?._element?.nativeElement ?? this.valueAccessor?.elementRef?.nativeElement ?? this.valueAccessor?._maskService?._formElement;
  // @ts-ignore
};


@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'EefaCeram-Panel';

  constructor(
    private repository: RepositoryService,
    private identity: IdentityService,
    private router: Router
  ) {
  }

  async ngOnInit() {

  }

  private lastkey = '';

  @HostListener('window:keydown', ['$event'])
  async keyEvent(event: KeyboardEvent ) {

    if (event.key == 'Control' || event.key == 'Alt') {
      this.lastkey = event.key;
    }
    else if (this.lastkey == 'Control' && (event.key == 'q' || event.key == 'Q' || event.key == 'ض')) {
      if (this.identity.doesHavePermission("Qreport")) {
        this.lastkey = '';
        await this.router.navigateByUrl("/accounting/reporting/q-Report");
      } else {
        this.lastkey = '';
        alert("دسترسی لازم را ندارید.");
      }
    }
    else if (this.lastkey == 'Control' && (event.key == 'b' || event.key == 'B' || event.key == 'ذ')) {


      if (this.identity.doesHavePermission("warehouseReceiptsBookRila")) {
        this.lastkey = '';
        await this.router.navigateByUrl("/inventory/receiptList");
      } else {
        this.lastkey = '';
        alert("دسترسی لازم را ندارید.");
      }

    }
    else if (this.lastkey == 'Control' && (event.key == 'y' || event.key == 'Y' || event.key == 'غ')) {

      if (this.identity.doesHavePermission("Receipts-Menu")) {
        this.lastkey = '';
        await this.router.navigateByUrl("/inventory/warehouseReceiptsBookRila");
      } else {
        this.lastkey = '';
        alert("دسترسی لازم را ندارید.");
      }

    }
    else if (this.lastkey == 'Alt' && (event.key == 'w' || event.key == 'W' || event.key == 'ص')) {

      if (this.identity.doesHavePermission("BuyAnd Sales")) {
        this.lastkey = '';
        await this.router.navigateByUrl("/inventory/commodityReceiptReports");
      } else {
        this.lastkey = '';
        alert("دسترسی لازم را ندارید.")
      }

    }
    else if (this.lastkey == 'Alt' && (event.key == 'C' || event.key == 'c' || event.key == 'ز')) {

      if (this.identity.doesHavePermission("CommodityReports")) {
        this.lastkey = '';
        await this.router.navigateByUrl("/inventory/commodityReports");
      } else {
        this.lastkey = '';
        alert("دسترسی لازم را ندارید.");
      }

    }
    else {
      this.lastkey = '';
    }
  }
  @HostListener('window:keyup', ['$event'])
  async keyUpEvent(event: KeyboardEvent) {


    const Elements = event.target as HTMLElement;
    const tabIndex = Elements?.parentElement?.tabIndex == undefined ? Elements.tabIndex : Elements?.parentElement?.tabIndex;
    switch (event?.key?.toLowerCase()) {

      case 'insert': {
        this.GetSubmitButton('Save');

        break;
      }
      case 'enter': {
        this.GetSubmitButton('Search');

        break;
      }


    }


  }


  async GetSubmitButton(Id: string) {
    var html = document.getElementsByClassName('current-tab');

    for (let i = 0; i < html.length; i++) {
      const slide = html[i] as HTMLElement;
      var buttons = slide?.getElementsByTagName('button');

      for (let j = 0; j < buttons.length; j++) {

        const button = buttons[j] as HTMLElement;
        if (button.id == Id) {
          button.click();


        }
      }
    }

  }

}
