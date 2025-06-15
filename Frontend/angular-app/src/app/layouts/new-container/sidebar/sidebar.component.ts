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
export class SidebarComponent {

  // for route changes
  currentRoute: string = '';

  @Input() isToggled: boolean = false;

  @Input() isToggledSubmenu: boolean = false;

  @Input() navigations!: NavigationItem[];

  @Input() set flatNavigations(navItems: NavigationItem[]) {
    this._flatNavigations = navItems;
    this.activateCurrentMenu()
  };

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
    this.isToggledSubmenu = false;
  }

  collapseOtherSiblings(navItem: NavigationItem) {
    this.navigations.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }



}
