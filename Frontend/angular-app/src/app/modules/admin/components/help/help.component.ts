import {AfterViewInit, Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {IdentityService} from "../../../identity/repositories/identity.service";
import {TabManagerService} from "../../../../layouts/main-container/tab-manager.service";
import {NavigationItem} from "../../../../layouts/main-container/models/navigation-item";
import {FormControl} from "@angular/forms";
import {Observable} from "rxjs";
import {map, startWith} from "rxjs/operators";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import Editor from '../../../../../assets/CKEditore-5/ckeditor';
import {GetHelpsQuery} from "../../repositories/help/queries/get-helps-query";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";


@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.scss']
})

export class HelpComponent implements OnInit, AfterViewInit {
  currentMenuId: number = 0;
  activeMenus: NavigationItem[] = []

  helpNavigations:NavigationItem[] = [];
  helpFlatNavigations:NavigationItem[] = [];
  isToggledHelp:boolean = false;

  currentMenuHelps: any[] = []
  helpSearchFormControl = new FormControl('');
  lastLevelMenus: NavigationItem[] = [];
  filteredUserMenus?: Observable<NavigationItem[]>;


  isLoading = false;

  editor: any;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: any,
    private _mediator: Mediator,
    private identityService: IdentityService,
    private tabManagerService: TabManagerService,
    private dialogRef: MatDialogRef<HelpComponent>,
  ) {
    this.lastLevelMenus = this.identityService.applicationUser.flatNavigations.filter(x => x.children?.length === 0);

    let activeTab = tabManagerService.tabs.find(x => x.active);
    let menu = this.lastLevelMenus.find(x => x.route === activeTab?.instanceRoute)
    if (menu) this.currentMenuId = <number>menu.id;

    this.identityService._applicationUser.subscribe(res => {
    });
    this.helpNavigations =  JSON.parse(JSON.stringify(this.identityService.applicationUser.navigations));
    this.helpFlatNavigations =  JSON.parse(JSON.stringify(this.identityService.applicationUser.flatNavigations));
  }

  ngOnInit() {
  }

  async ngAfterViewInit() {
    let editorEl = document.getElementById("editor-viewer");
    // @ts-ignore
    Editor.create(editorEl, {
      toolbar: {
        items: [],
      }
    }).then(res => {
      this.editor = res;
      this.editor.enableReadOnlyMode("editor-viewer")
    });

    await this.initialize()
    this.filteredUserMenus = this.helpSearchFormControl.valueChanges.pipe(
      startWith(''),
      map(value => this._filter(value || '')),
    );
  }

  async initialize() {
    this.isLoading = true
    this.activeMenus = []
    this.currentMenuHelps = []
    let currentMenu = this.getMenuById(this.currentMenuId);
    if (currentMenu) {
      this.activeMenus.push(currentMenu)
      let parentId = currentMenu.parentId;

      while (parentId) {
        let parentMenu = this.getMenuById(parentId);
        if (parentMenu) this.activeMenus.unshift(parentMenu)
        parentId = parentMenu?.parentId;
      }
    }
    await this.getCurrentMenuHelps()
  }

  async getCurrentMenuHelps() {
    this._mediator.send(new GetHelpsQuery(0, 0, [new SearchQuery({
      propertyName: 'menuItemId',
      comparison: 'equals',
      values: [this.currentMenuId]
    })])).then(res => {
      if (res.data.length != 0){
        this.isLoading = false
        this.editor.setData(res.data[0].contents)
      }
      else {
        this.isLoading = false
        this.editor.setData('راهنمای این صفحه درحال ساخت است.')
      }
    })
  }

  getMenuById(id: number) {
    return this.identityService.applicationUser.flatNavigations.find(x => x.id === id)
  }

  private _filter(value: string): NavigationItem[] {
    const filterValue = value;

    return this.lastLevelMenus.filter(option => this.menuOptionsDisplayFunction(<number>option.id).includes(filterValue));
  }

  close() {
    this.dialogRef.close()
  }

  handleSearchedMenuSelection(id: number) {
    this.currentMenuId = id;
    this.initialize()
  }

  async navigateToHelp(item: NavigationItem){
    if (item.id) await this.handleSearchedMenuSelection(item.id);
  }

  menuOptionsDisplayFunction(id: number) {
    let menu = this.getMenuById(id);
    if (menu) {
      let menuTitle = menu?.title
      let parent = this.getMenuById(<number>menu?.parentId)
      while (parent?.parentId) {
        parent = this.getMenuById(<number>parent?.parentId);
      }

      return parent ? parent?.title + " / " + menuTitle : menuTitle;

    } else return ""
  }
}
