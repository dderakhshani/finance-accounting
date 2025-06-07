import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {MenuItem} from "../../entities/menu-item";
import {GetMenuItemsQuery} from "../../repositories/menu-item/queries/get-menu-items-query";
import {MenuItemDialogComponent} from "./menu-item-dialog/menu-item-dialog.component";
import {PageModes} from "../../../../core/enums/page-modes";

@Component({
  selector: 'app-menu-item',
  templateUrl: './menu-item.component.html',
  styleUrls: ['./menu-item.component.scss']
})
export class MenuItemComponent extends BaseComponent {


  menuItems: MenuItem[] = [];
  menuItemPermissionId!: number

  constructor(
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    await this.get()
  }

  async get() {
    await this._mediator.send(new GetMenuItemsQuery(0, 0, undefined, "id ASC")).then(res => {

      this.menuItems = res.data;
    })
  }

  add(menuItem: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      menuItem: menuItem,
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(MenuItemDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.menuItems.push(response)
        this.menuItems = [...this.menuItems]
      }
    })
  }

  update(menuItem: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      menuItem: menuItem,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(MenuItemDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let menuItemToUpdate = this.menuItems.find(x => x.id === response.id)
          if (menuItemToUpdate) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              menuItemToUpdate[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let menuItemToRemove = this.menuItems.find(x => x.id === response.id)
          if (menuItemToRemove) {
            this.menuItems.splice(this.menuItems.indexOf(menuItemToRemove), 1)
            this.menuItems = [...this.menuItems]
          }
        }
      }
    })
  }

  handleMenuRoles(menuItem: MenuItem) {
    this.menuItemPermissionId = menuItem.permissionId
  }

  delete() {

  }

  async initialize() {

  }

  close() {
  }
}
