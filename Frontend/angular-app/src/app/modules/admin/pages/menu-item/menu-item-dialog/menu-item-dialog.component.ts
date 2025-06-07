import {Component, Inject} from '@angular/core';
import {MenuItem} from "../../../entities/menu-item";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {CreateMenuItemCommand} from "../../../repositories/menu-item/command/create-menu-item-command";
import {UpdateMenuItemCommand} from "../../../repositories/menu-item/command/update-menu-item-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {DeleteMenuItemCommand} from "../../../repositories/menu-item/command/delete-menu-item-command";
import {environment} from "../../../../../../environments/environment";
import { UploadFileCommand } from "../../../../../shared/repositories/files/upload-file-command";
import { Permission } from '../../../entities/permission';
import { GetPermissionsQuery } from '../../../repositories/permission/queries/get-permissions-query';

@Component({
  selector: 'app-menu-items-dialog',
  templateUrl: './menu-item-dialog.component.html',
  styleUrls: ['./menu-item-dialog.component.scss']
})
export class MenuItemDialogComponent extends BaseComponent {

  pageModes = PageModes;
  menuItem!: MenuItem;
  permissions: Permission[] = [];

  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<MenuItemDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.menuItem = data.menuItem;
    this.pageMode = data.pageMode;
    this.request = new CreateMenuItemCommand();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    await this._mediator.send(new GetPermissionsQuery()).then(res => {
      this.permissions = res.data;
    })

    await this.initialize()
  }
  initialize() {

    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateMenuItemCommand()
      if (this.menuItem) {
        newRequest.parentId = this.menuItem.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateMenuItemCommand().mapFrom(this.menuItem)
    }

  }


  async add() {
    await this._mediator.send(<CreateMenuItemCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async update(entity?: any) {
    await this._mediator.send(<UpdateMenuItemCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteMenuItemCommand((<UpdateMenuItemCommand>this.request).id ?? 0)).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:PageModes.Delete
      })
    });
  }

  async onPhotoInput(event: any) {

    var files = event.target.files[0];

    await this._mediator.send(new UploadFileCommand(event.target.files[0])).then(response => {
      //this.form.controls['imageUrlReletiveAddress'].setValue(environment.FileServerAddress + response);
      this.form.controls['imageUrl'].setValue(response);
    })
  }


  getPermissionTitleById(id: number) {

    return this.permissions.find(x => x.id === id)?.title ?? ''
  }



  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
}

