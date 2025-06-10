import {ChangeDetectorRef, Component, Input, OnInit} from '@angular/core';
import {TabManagerService} from "../main-container/tab-manager.service";
import {NavigationItem} from "../main-container/models/navigation-item";
import {IdentityService} from "../../modules/identity/repositories/identity.service";
import {Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {NotificationService} from "../../core/services/Notification/Notification.Service";
import {HelpComponent} from "../../modules/admin/components/help/help.component";

@Component({
  selector: 'app-new-container',
  templateUrl: './new-container.component.html',
  styleUrls: ['./new-container.component.scss']
})
export class NewContainerComponent implements OnInit {

  @Input() isShowSidebar: boolean = false;

  onToggleSidebar(): void {
    this.isShowSidebar = !this.isShowSidebar;
  }

  navigations:NavigationItem[] = [];
  flatNavigations:NavigationItem[] = [];

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
  }

}
