import {Component} from '@angular/core';
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PermissionDialogComponent} from "./permission-dialog/permission-dialog.component";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {Permission} from "../../entities/permission";
import {GetPermissionsQuery} from "../../repositories/permission/queries/get-permissions-query";
import {PageModes} from "../../../../core/enums/page-modes";

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.scss']
})
export class PermissionsComponent extends BaseComponent {

  permissions: Permission[] = [];

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
    await this._mediator.send(new GetPermissionsQuery(0, 0, undefined, "id ASC")).then(res => {
      this.permissions = res.data;
    })
  }

  add(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Add
    };
    let dialogReference = this.dialog.open(PermissionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.permissions.push(response)
        this.permissions = [...this.permissions]
      }
    })
  }

  update(node: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      node: node,
      pageMode: PageModes.Update
    };

    let dialogReference = this.dialog.open(PermissionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response) {
        if (pageMode === PageModes.Update) {
          let permission = this.permissions.find(x => x.id === response.id)
          if (permission) {
            Object.keys(response).forEach((key: string) => {
              // @ts-ignore
              permission[key] = response[key]
            })
          }
        } else if (pageMode === PageModes.Delete) {
          let permissionToRemove = this.permissions.find(x => x.id === response.id)
          if (permissionToRemove) {
            this.permissions.splice(this.permissions.indexOf(permissionToRemove), 1)
            this.permissions = [...this.permissions]
          }
        }
      }
    })
  }


  initialize() {

  }


  delete() {

  }

  close() {

  }
}
