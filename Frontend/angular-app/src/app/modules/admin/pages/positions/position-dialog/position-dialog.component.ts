import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../core/enums/page-modes";
import {BaseValueType} from "../../../entities/base-value-type";
import {CreatePositionCommand} from "../../../repositories/position/command/create-position-command";
import {UpdatePositionCommand} from "../../../repositories/position/command/update-position-command";
import {DeletePositionCommand} from "../../../repositories/position/command/delete-position-command";

@Component({
  selector: 'app-positions-dialog',
  templateUrl: './position-dialog.component.html',
  styleUrls: ['./position-dialog.component.scss']
})
export class PositionDialogComponent extends BaseComponent {


  pageModes = PageModes;
  node!: BaseValueType;


  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<PositionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super();
    this.node = data.node;
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
      let newRequest = new CreatePositionCommand()
      if (this.node) {
        newRequest.parentId = this.node.id;
      }
      this.request = newRequest;
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdatePositionCommand().mapFrom(this.node)
    }
  }

  async add() {
    let response = await this._mediator.send(<CreatePositionCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdatePositionCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async delete() {
    let response = await this._mediator.send(new DeletePositionCommand((<UpdatePositionCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response: response,
      pageMode: PageModes.Delete
    })
  }

  close(): any {
  }


  get(param?: any): any {
  }


}
