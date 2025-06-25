import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {NavigationItem} from "../../../main-container/models/navigation-item";

@Component({
  selector: 'app-sidebar-item',
  templateUrl: './sidebar-item.component.html',
  styleUrls: ['./sidebar-item.component.scss']
})
export class SidebarItemComponent implements OnInit {

  rtl: boolean = false;
  // inputs
  @Input() isSubSidebar: boolean = false;
  @Input() isToggled: boolean = false
  @Input() isToggledSubmenu: boolean = false
  @Input() currentRoute!: string;
  @Input() subLevelCounter: number = 0;
  @Input() sidebarItem!: NavigationItem;

  // outputs
  @Output() sidebarItemClick: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();
  @Output() unToggledSubmenu: EventEmitter<boolean> = new EventEmitter<boolean>()
  @Output() collapseSiblings: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();

  ngOnInit(): void {
    this.rtl = document.getElementsByTagName("html")[0].getAttribute('dir') === 'rtl';
  }

  collapseAllChildsExceptActiveItem(navItem: NavigationItem) {
    this.sidebarItem.children.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }

  toggleSidebarItem(navItem: NavigationItem) {
    if (navItem.children.length > 0) {
      navItem.showChildren = !navItem.showChildren;
      this.collapseSiblings.emit(navItem)
    } else {
      // this.router.navigateByUrl(navItem.Route);
      this.sidebarItemClick.emit(navItem)
    }
    if (this.sidebarItem.showChildren) this.unToggledSubmenu.emit(true)
    else {
      this.unToggledSubmenu.emit(false)
    }
  }
}
