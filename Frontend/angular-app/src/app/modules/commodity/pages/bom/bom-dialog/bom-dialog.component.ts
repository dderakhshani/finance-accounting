import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CreateBomCommand} from "../../../repositories/bom/commands/create-bom-command";
import {UpdateBomCommand} from "../../../repositories/bom/commands/update-bom-command";
import Bom from "../../../entities/bom";
import {DeleteBomCommand} from "../../../repositories/bom/commands/delete-bom-command";
import { CreateBomItemCommand } from '../../../repositories/bom-item/commands/create-bom-item-command';
import { FormArray } from '@angular/forms';
import { GetBomQuery } from '../../../repositories/bom/queries/get-bom-command';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';

@Component({
  selector: 'app-bom-dialog',
  templateUrl: './bom-dialog.component.html',
  styleUrls: ['./bom-dialog.component.scss']
})
export class BomDialogComponent extends BaseComponent {
  public entity!: Bom;
  public pageModes = PageModes;
  private displayPage: string = '';
  public PageTitle: string = 'فرمول ساخت';

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<BomDialogComponent>,
    public _notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;
    this.displayPage = data.displayPage;
    this.request = new CreateBomCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    
    if (this.pageMode === PageModes.Add) {
      this.request = new CreateBomCommand();
      this.PageTitle = 'افزودن فرمول ساخت'
    }
   else if (this.pageMode === PageModes.Update && this.displayPage != 'copy') {
      
      this._mediator.send(new GetBomQuery(this.entity.id)).then(res => {
        this.entity = res;
        this.request = new UpdateBomCommand().mapFrom(this.entity)
      })
      this.PageTitle = 'ویرایش فرمول ساخت'
    }
    else if (this.pageMode === PageModes.Update && this.displayPage == 'copy') {

      this._mediator.send(new GetBomQuery(this.entity.id)).then(res => {
        this.entity = res;
        this.request = new CreateBomCommand().mapFrom(this.entity)
      })
      this.pageMode = PageModes.Add
      this.PageTitle = 'رونوشت فرمول ساخت'
    }
  }

  initialize(): any {
  }

  async add() {
    if (this.form.valid) {
      let response = await this._mediator.send(<CreateBomCommand>this.request);
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    }
    
  }

  async update(entity?: any) {
    if (this.form.valid) {
      let response = await this._mediator.send(<UpdateBomCommand>this.request);
      this.dialogRef.close({
        response: response,
        pageMode: this.pageMode
      })
    }
  }

  

  async delete() {
    let response = await this._mediator.send(new DeleteBomCommand((<UpdateBomCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response: response,
      pageMode: PageModes.Delete
    })
  }
  CommodityCategoryIdSelect(id:any) {

    this.form.controls.commodityCategoryId.setValue(id);
    

  }
  
  get(param?: any): any {
  }
  close(): any {
  }
}
