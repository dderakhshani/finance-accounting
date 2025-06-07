import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {CodeVoucherExtendType} from "../../../../entities/code-voucher-extend-type";
import {
  CreateCodeVoucherExtendTypeCommand
} from "../../../../repositories/code-voucher-extend-type/commands/create-code-voucher-extend-type-command";
import {
  UpdateCodeVoucherExtendTypeCommand
} from "../../../../repositories/code-voucher-extend-type/commands/update-code-voucher-extend-type-command";
import {
  DeleteCodeVoucherExtendTypeCommand
} from "../../../../repositories/code-voucher-extend-type/commands/delete-code-voucher-extend-type-command";

@Component({
  selector: 'app-code-voucher-exception-type-dialog',
  templateUrl: './code-voucher-extend-type-dialog.component.html',
  styleUrls: ['./code-voucher-extend-type-dialog.component.scss']
})
export class CodeVoucherExtendTypeDialogComponent extends BaseComponent {
  entity!:CodeVoucherExtendType;
  pageModes = PageModes

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<CodeVoucherExtendTypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;
  }

  async ngOnInit() {
    await this.resolve()
  }
  resolve(): any {
    if (this.pageMode === PageModes.Add) {
      this.request = new CreateCodeVoucherExtendTypeCommand();
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateCodeVoucherExtendTypeCommand().mapFrom(this.entity)
    }
  }
  initialize(params?: any): any {
  }

  async add() {
    let response = await this._mediator.send(<CreateCodeVoucherExtendTypeCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdateCodeVoucherExtendTypeCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }
  async delete() {
    let response = await this._mediator.send(new DeleteCodeVoucherExtendTypeCommand((<UpdateCodeVoucherExtendTypeCommand>this.request).id ?? 0));
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
