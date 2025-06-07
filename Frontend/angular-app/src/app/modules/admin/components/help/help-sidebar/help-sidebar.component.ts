import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NavigationItem } from "../../../../../layouts/main-container/models/navigation-item";

@Component({
  selector: 'app-help-sidebar',
  templateUrl: './help-sidebar.component.html',
  styleUrls: ['./help-sidebar.component.scss']
})
export class HelpSidebarComponent implements OnInit {
  @Input() helpNavigations!: NavigationItem[];

  @Input() set helpFlatNavigations(navItems: NavigationItem[]) {
    this._helpFlatNavigations = navItems;
  };

  _helpFlatNavigations: NavigationItem[] = []
  get helpFlatNavigations() {
    return this._helpFlatNavigations
  }

  @Input() isToggledHelp: boolean = false;

  @Output() helpSidebarItemClicked: EventEmitter<NavigationItem> = new EventEmitter<NavigationItem>();

  @Output() toggled: EventEmitter<boolean> = new EventEmitter<boolean>()

  constructor() {
  }

  ngOnInit() {
  }

  toggle() {
    this.toggled.emit()
  }

  helpItemClicked(item: NavigationItem) {
    this.helpSidebarItemClicked.emit(item)
  }

  helpCollapseSiblings(navItem: NavigationItem) {
    this.helpNavigations.filter(x => x.id != navItem.id).forEach(x => x.showChildren = false)
  }
}
