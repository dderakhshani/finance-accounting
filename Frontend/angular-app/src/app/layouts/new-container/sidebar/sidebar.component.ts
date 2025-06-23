import {Component, EventEmitter, Input, OnChanges, OnInit, Output} from '@angular/core';
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {NavigationEnd, Router} from "@angular/router";
import {NavigationItem} from "../../main-container/models/navigation-item";
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent{

  // for route changes
  currentRoute: string = '';

  // inputs
  @Input() isToggled: boolean = false;
  @Input() isToggledSubmenu: boolean = false;
  @Input() navigations!: NavigationItem[];
  @Input() set flatNavigations(navItems: NavigationItem[]) {
    this._flatNavigations = navItems;
  };

  // outputs
  @Output() closeSidebar : EventEmitter<boolean> = new EventEmitter();
  @Output() sidebarItemClicked: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();

  _flatNavigations: NavigationItem[] = []
  get flatNavigations() {
    return this._flatNavigations
  }

  constructor(
    public identityService: IdentityService,
    private router: Router,
  ) {
     // listening to route changes
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.currentRoute = (event as NavigationEnd).urlAfterRedirects.split('?')[0];
      });
  }

  itemClicked(item: NavigationItem) {
    this.closeAllMenus(this.navigations);
    this.sidebarItemClicked.emit(item)
  }

  closeAllMenus(items: NavigationItem[]) {
    items.forEach(item => {
      item.showChildren = false;
      if (item.children && item.children.length > 0) {
        this.closeAllMenus(item.children);
      }
      this.isToggledSubmenu = false;
    });
  }

  collapseOtherSiblings(navItem: NavigationItem) {
    this.navigations.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }
}
