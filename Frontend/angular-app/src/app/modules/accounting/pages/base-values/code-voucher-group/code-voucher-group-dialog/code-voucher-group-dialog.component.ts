import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CodeVoucherGroup} from "../../../../entities/code-voucher-group";
import {CodeVoucherExtendType} from "../../../../entities/code-voucher-extend-type";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {
  CreateCodeVoucherGroupCommand
} from "../../../../repositories/code-voucher-group/command/create-code-voucher-group-command";
import {
  UpdateCodeVoucherGroupCommand
} from "../../../../repositories/code-voucher-group/command/update-code-voucher-group-command";
import {
  DeleteCodeVoucherGroupCommand
} from "../../../../repositories/code-voucher-group/command/delete-code-voucher-group-command";
import {
  GetCodeVoucherExtendTypesQuery
} from "../../../../repositories/code-voucher-extend-type/queries/get-code-voucher-extend-types-query";


@Component({
  selector: 'app-code-voucher-group-dialog',
  templateUrl: './code-voucher-group-dialog.component.html',
  styleUrls: ['./code-voucher-group-dialog.component.scss']
})
export class CodeVoucherGroupDialogComponent extends BaseComponent {

  entity!: CodeVoucherGroup;
  codeVoucherExtendTypes: CodeVoucherExtendType[] = [];
  pageModes = PageModes;


  constructor( private _mediator:Mediator,
               private dialogRef: MatDialogRef<CodeVoucherGroupDialogComponent>,
               @Inject(MAT_DIALOG_DATA) data : any
  ) {
    super();
    this.entity = data.entity;
    this.pageMode = data.pageMode;
  }


  async ngOnInit() {
    await this.resolve()
  }
  async resolve() {
    await this._mediator.send(new GetCodeVoucherExtendTypesQuery()).then(res => {
      this.codeVoucherExtendTypes = res.data
    })

    if (this.pageMode === PageModes.Add) {
      this.request = new CreateCodeVoucherGroupCommand();
    }
    if (this.pageMode === PageModes.Update) {
      this.request = new UpdateCodeVoucherGroupCommand().mapFrom(this.entity)
    }
  }
  initialize(params?: any): any {
  }



  async add() {
    let response = await this._mediator.send(<CreateCodeVoucherGroupCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }

  async update(entity?: any) {
    let response = await this._mediator.send(<UpdateCodeVoucherGroupCommand>this.request);
    this.dialogRef.close({
      response: response,
      pageMode: this.pageMode
    })
  }
  async delete() {
    let response = await this._mediator.send(new DeleteCodeVoucherGroupCommand((<UpdateCodeVoucherGroupCommand>this.request).id ?? 0));
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
