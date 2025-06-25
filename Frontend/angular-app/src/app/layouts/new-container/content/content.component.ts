import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {NavigationItem} from "../models/navigation-item";

import {Router} from "@angular/router";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {TabManagerService} from "./tab-manager.service";
import {animate, style, transition, trigger} from "@angular/animations";
import {MatDialog} from "@angular/material/dialog";
import {HelpComponent} from "../../../modules/admin/components/help/help.component";
import {NotificationService} from "../../../core/services/Notification/Notification.Service";


@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss'],
  animations: [
    trigger('fadeAnimation', [
      transition(':enter', [
        style({ opacity: 0 }), animate('500ms', style({ opacity: 1 }))]
      ),
      transition(':leave',
        [style({ opacity: 1 }), animate('0ms', style({ opacity: 0 }))]
      )
    ])
  ]
})
export class ContentComponent implements OnInit {
  navigations:NavigationItem[] = [];
  flatNavigations:NavigationItem[] = [];
  isToggled:boolean = false;

  // @ts-ignore
  constructor(
    private cdr: ChangeDetectorRef,
    private identityService: IdentityService,
    private router: Router,
    public tabManagerService: TabManagerService,
    private dialog: MatDialog,
    private _notificationService: NotificationService,
  ) {

    this.identityService._applicationUser.subscribe(res => {
      this.navigations = res.navigations;
      this.flatNavigations = res.flatNavigations;
    });
  }

  ngOnInit(): void {
    setTimeout(()=>{
      this._notificationService.startConnection();
    },1000);
  }
  async navigateToRoute(item: NavigationItem){
    await this.router.navigateByUrl(item.route);
    // this.isToggled = !this.isToggled
  }
  // ngAfterViewChecked() {
  //   this.cdr.detectChanges();
  // }
  openCurrentTabHelp() {
    this.dialog.open(HelpComponent, {
      autoFocus: false,
    })
  }
}
