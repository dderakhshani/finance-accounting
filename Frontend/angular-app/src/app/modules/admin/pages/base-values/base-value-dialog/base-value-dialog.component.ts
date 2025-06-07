import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {PageModes} from "../../../../../core/enums/page-modes";
import {BaseValue} from "../../../entities/base-value";
import {BaseValueType} from "../../../entities/base-value-type";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CreateBaseValueCommand} from "../../../repositories/base-value/commands/create-base-value-command";
import {UpdateBaseValueCommand} from "../../../repositories/base-value/commands/update-base-value-command";
import {CreateBaseValueTypeCommand} from "../../../repositories/base-value-type/command/create-base-value-type-command";
import {UpdateBaseValueTypeCommand} from "../../../repositories/base-value-type/command/update-base-value-type-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {DeleteBaseValueCommand} from "../../../repositories/base-value/commands/delete-base-value-command";

@Component({
  selector: 'app-base-value-dialog',
  templateUrl: './base-value-dialog.component.html',
  styleUrls: ['./base-value-dialog.component.scss']
})
export class BaseValueDialogComponent extends BaseComponent {
  pageModes = PageModes;

  baseValue!:BaseValue;
  baseValueType!:BaseValueType;


  constructor(
    private _mediator:Mediator,
    private dialogRef: MatDialogRef<BaseValueDialogComponent>,
  @Inject(MAT_DIALOG_DATA) data : any) {
    super();
    this.baseValue = data.baseValue;
    this.baseValueType = data.baseValueType;
    this.pageMode = data.pageMode;
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreateBaseValueCommand()

      if (this.baseValueType) {
        newRequest.baseValueTypeId = this.baseValueType.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateBaseValueCommand().mapFrom(this.baseValue)
    }

  }

  async add() {
    let response = await this._mediator.send(<CreateBaseValueTypeCommand>this.request);
    this.dialogRef.close({
      response:response,
      pageMode:this.pageMode
    })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdateBaseValueTypeCommand>this.request);
    this.dialogRef.close({
      response:response,
      pageMode:this.pageMode
    })
  }

  close(): any {
  }

  async delete() {
    let response = await this._mediator.send(new DeleteBaseValueCommand((<UpdateBaseValueTypeCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response:response,
      pageMode:PageModes.Delete
    })
  }

  get(param?: any): any {
  }

}
