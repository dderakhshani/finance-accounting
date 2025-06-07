import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CodeRowDescription} from "../../../../entities/code-row-description";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {
  CreateCodeRowDescriptionCommand
} from "../../../../repositories/code-row-description/commands/create-code-row-description-command";
import {
  UpdateCodeRowDescriptionCommand
} from "../../../../repositories/code-row-description/commands/update-code-row-description-command";
import {
  DeleteCodeRowDescriptionCommand
} from "../../../../repositories/code-row-description/commands/delete-code-row-description-command";

@Component({
  selector: 'app-code-row-description-dialog',
  templateUrl: './code-row-description-dialog.component.html',
  styleUrls: ['./code-row-description-dialog.component.scss']
})
export class CodeRowDescriptionDialogComponent extends BaseComponent {
  entity!: CodeRowDescription;
  pageModes = PageModes;

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<CodeRowDescriptionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;
  }

  async ngOnInit() {
    await this.resolve()
  }

  resolve(): any {
    if (this.pageMode === PageModes.Add) {
      this.request = new CreateCodeRowDescriptionCommand();
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateCodeRowDescriptionCommand().mapFrom(this.entity)
    }
  }

  initialize(): any {
  }

  async add() {
    let response = await this._mediator.send(<CreateCodeRowDescriptionCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdateCodeRowDescriptionCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  close(): any {
  }

  async delete() {
    let response = await this._mediator.send(new DeleteCodeRowDescriptionCommand((<UpdateCodeRowDescriptionCommand>this.request).id ?? 0));
    this.dialogRef.close({
      response: response,
      pageMode: PageModes.Delete
    })
  }

  get(param?: any): any {
  }
}
