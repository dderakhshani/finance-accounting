import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NavigationItem} from "../../../../../../layouts/main-container/models/navigation-item";

@Component({
  selector: 'app-help-sidebar-item',
  templateUrl: './help-sidebar-item.component.html',
  styleUrls: ['./help-sidebar-item.component.scss']
})
export class HelpSidebarItemComponent implements OnInit {
  rtl:boolean= true;
  @Input()  helpSidebarItem!: NavigationItem;
  @Input() isHelpSubSidebar: boolean = false;
  @Input() helpSubLevelCounter: number = 0;
  @Output() helpSidebarItemClick: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();
  @Input() isToggledHelp:boolean = false
  @Output() helpCollapseSiblings: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();

  constructor() { }

  ngOnInit(): void {
  }
  collapseAllChildsExceptActiveItem(navItem:NavigationItem) {
    this.helpSidebarItem.children.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }
  toggleSidebarItemHelp(navItem: NavigationItem) {
    if(navItem.children.length > 0) {
      navItem.showChildren = !navItem.showChildren;
      this.helpCollapseSiblings.emit(navItem)
    } else {
      // this.router.navigateByUrl(navItem.Route);
      this.helpSidebarItemClick.emit(navItem)
    }
  }
}
