import {Component, Inject} from '@angular/core';
import {BaseValueType} from "../../../entities/base-value-type";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../core/enums/page-modes";
import {CreateBaseValueTypeCommand} from "../../../repositories/base-value-type/command/create-base-value-type-command";
import {UpdateBaseValueTypeCommand} from "../../../repositories/base-value-type/command/update-base-value-type-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {DeleteBaseValueTypeCommand} from "../../../repositories/base-value-type/command/delete-base-value-type-command";

@Component({
  selector: 'app-base-value-types-dialog',
  templateUrl: './base-value-type-dialog.component.html',
  styleUrls: ['./base-value-type-dialog.component.scss']
})
export class BaseValueTypeDialogComponent extends BaseComponent {

  pageModes = PageModes;
  baseValueType!: BaseValueType;

  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<BaseValueTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.baseValueType = data.baseValueType;
    this.pageMode = data.pageMode;
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
   await this.initialize()
  }

 async initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateBaseValueTypeCommand()
      if (this.baseValueType) {
        newRequest.parentId = this.baseValueType.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateBaseValueTypeCommand().mapFrom(this.baseValueType)
    }
  }

  async add() {
    await this._mediator.send(<CreateBaseValueTypeCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async update(entity?: any) {
    await this._mediator.send(<UpdateBaseValueTypeCommand>this.request).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:this.pageMode
      })
    });
  }

  async delete() {
    await this._mediator.send(new DeleteBaseValueTypeCommand((<UpdateBaseValueTypeCommand>this.request).id ?? 0)).then(res => {
      this.dialogRef.close({
        response:res,
        pageMode:PageModes.Delete
      })
    });
  }







  get(id?: number) {
    throw new Error('Method not implemented.');
  }

  close() {
    throw new Error('Method not implemented.');
  }
}

