import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, NavigationEnd, Router, RoutesRecognized } from "@angular/router";
import { Tab } from "../models/tab";
import { filter, map } from "rxjs/operators";
import { TabManagerService } from "../tab-manager.service";

@Component({
  selector: 'app-tabs',
  templateUrl: './tabs.component.html',
  styleUrls: ['./tabs.component.scss']
})


export class TabsComponent implements OnInit {

  constructor(
    public tabManagerService: TabManagerService, private router: Router
  ) {

  }


  ngOnInit(): void {
  }

  private lastkey = '';
  @HostListener('window:keydown', ['$event'])
  async keyEvent(event: KeyboardEvent) {
    if (event.key == 'Control') {
      this.lastkey = 'Control';
    }
    else if (this.lastkey == 'Control' && event.code == 'Backquote') {
      let currenctrout = this.router.url;
      if (currenctrout != '/accounting/reporting/accountReviewReport') {
        let tab = this.tabManagerService.tabs.filter(a => a.active == true);
        this.tabManagerService.closeTab(tab[0])
      }
      else {
        document.getElementById('back')?.click();
      }
    }
    else {
      this.lastkey = '';
    }
  }
}
