import {EventEmitter, Injectable} from '@angular/core';
import {ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router} from "@angular/router";
import {Tab} from "../models/tab";
import {filter, map, pairwise} from "rxjs/operators";
import {IdentityService} from "../../../modules/identity/repositories/identity.service";
import {tr} from "date-fns/locale";

@Injectable({
  providedIn: 'root'
})
export class TabManagerService {

  public tabs: Tab[] = [];

  public get activeTab(): Tab {
    return <Tab>this.tabs.find(tab => tab.active);
  }


  public currentUrl!: string;
  public previousUrl!: string;

  public tabChanged = new EventEmitter<Tab>();
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private identityService: IdentityService
  ) {
    window.addEventListener('keydown', (event) => {

      if (event.altKey && event.code.toLowerCase() === 'keyw') {
        this.closeTab(this.activeTab)
      }
      if (event.altKey && event.code.toLowerCase() === 'keyq') {
        let currentTabIndex = this.tabs.indexOf(this.activeTab)
        if (currentTabIndex < this.tabs.length - 1)
          this.openTab(this.tabs[currentTabIndex + 1])
        else
          this.openTab(this.tabs[0])

      }
    })
    this.identityService._applicationUser.subscribe(x => {
      if (!x.isAuthenticated) this.closeAllTabs()
    })
    this.router.events
      .pipe(
        filter((evt: any) => evt instanceof NavigationEnd),
        map((event: NavigationEnd) => {
          let route = this.route.snapshot;
          while (route.firstChild) {
            route = route.firstChild;
          }
          if (event.url.includes('cid')) {
            this.previousUrl = this.currentUrl;
            this.currentUrl = event.url;
          }

          return {route: route, event: event};
        })
      )
      .subscribe(async ({route, event}) => {
        if (route.data.isTab) {
          // console.log(route)
          let menu = this.identityService.applicationUser.flatNavigations.find(x => x.route === this.router.routerState.snapshot.url)
          // Generate a GUID here
          const guid =
            (this.router.routerState.snapshot.url.includes('?cid=') ? this.router.routerState.snapshot.url?.split('?cid=')[1].substring(0,36) : undefined ) ??
            (this.router.routerState.snapshot.url.includes('&cid=') ? this.router.routerState.snapshot.url?.split('&cid=')[1].substring(0,36) : undefined ) ??
            this.generateGuid();
          let newTab = <Tab>{
            title: menu?.title ?? route.data.title,
            component: route.component,
            id: menu?.id ?? route.data.id,
            guid: guid,
            active: false,
            instanceRoute: this.router.routerState.snapshot.url,
            isLoading: false
          }
          let rewrittenParams: any = {}

          if (!newTab.instanceRoute.includes('cid')) {
            newTab.actualRoute = newTab.instanceRoute;
            let baseRoute = newTab.instanceRoute.split('?')[0];
            let params = newTab.instanceRoute.split('?')[1];

            if (params) {
              params.split('&').forEach(x => {
                rewrittenParams[x.split('=')[0]] = x.split('=')[1];
              })
            }

            rewrittenParams.cid = guid;

            return await this.router.navigate([baseRoute], {queryParams: rewrittenParams, queryParamsHandling: 'merge'});


          } else {
            if (this.activeTab ? this.activeTab.guid !== newTab.guid : true) {
              let baseUrl = 'http://localhost';
              let url = new URL(baseUrl + newTab.instanceRoute)
              url.searchParams.delete('cid')
              newTab.actualRoute = url.pathname + url.search
              // console.log(newTab)
              await this.openTab(newTab)
            }
          }
          return this.tabChanged.emit(newTab)


        }
      });
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c) => {
      const r = (Math.random() * 16) | 0;
      const v = c === 'x' ? r : (r & 0x3) | 0x8;
      return v.toString(16);
    });
  }

  async openTab(tab?: Tab) {
    if (tab && !tab?.active) {

      this.deactivateAllTabs();
      tab.active = true;
      let existingTab = this.tabs.find(x => x.actualRoute === tab.actualRoute);
      if (existingTab) {
        existingTab.active = true;
        await this.router.navigateByUrl(<string>existingTab.instanceRoute ?? '/');

      } else {
        this.tabs.push(tab);
        await this.router.navigateByUrl(<string>tab.instanceRoute ?? '/');

      }


    }
  }

  async closeTab(tab: Tab) {
    let index = this.tabs.indexOf(tab)
    this.tabs.splice(index, 1);
    this.tabs = [...this.tabs]
    tab.component = null;
    if (this.tabs.length > 0 && tab.active) {
      let tabToBeActivated = this.tabs[index] ? this.tabs[index] : this.tabs[index - 1]
      await this.openTab(tabToBeActivated)
    } else if (this.tabs.length == 0) {
      // if (this.router.routerState.snapshot.url !== '/dashboard') {

      await this.router.navigateByUrl('/');
      // }
    }
    // @ts-ignore
    tab = null;
  }

  deactivateAllTabs() {
    this.tabs.filter(x => x.active).forEach(x => x.active = false);
  }


  closeAllTabs() {
    this.tabs = []
  }

  toggleCurrentTabLoading(value: boolean) {
    let activeTab = <Tab>this.tabs.find(x => x.active);
    activeTab.isLoading = value;
  }
}
