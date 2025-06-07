import {Component, Inject} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {AccountReferencesGroup} from "../../../../entities/account-references-group";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

import {PageModes} from 'src/app/core/enums/page-modes';
import {
  CreateAccountReferencesGroupCommand
} from "../../../../repositories/account-reference-group/commands/create-account-references-group-command";
import {
  UpdateAccountReferencesGroupCommand
} from "../../../../repositories/account-reference-group/commands/update-account-references-group-command";
import {AccountManagerService} from "../../../../services/account-manager.service";
import {da} from "date-fns/locale";

@Component({
  selector: 'app-account-references-group-modal',
  templateUrl: './account-references-group-modal.component.html',
  styleUrls: ['./account-references-group-modal.component.scss']
})
export class AccountReferencesGroupModalComponent extends BaseComponent {

  entity!: AccountReferencesGroup
  pageModes = PageModes;

  constructor(
    private _mediator: Mediator,
    private dialogRef: MatDialogRef<AccountReferencesGroupModalComponent>,
    private accountManagerService:AccountManagerService,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super()

    if (data.id) {
      this.entity = data;
      this.entity.parentTitle = <string>this.accountManagerService.accountReferenceGroups.value.find(x => x.id === data.parentId)?.title;
      this.pageMode = PageModes.Update;
    } else {
      this.entity = <AccountReferencesGroup>{parentId: data.parentId, parentTitle:this.accountManagerService.accountReferenceGroups.value.find(x => x.id === data.parentId)?.title}
      this.pageMode = PageModes.Add;
    }
    this.request = new CreateAccountReferencesGroupCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    await this.initialize()

  }

  async initialize() {
    if (this.entity.id) {
      this.request = new UpdateAccountReferencesGroupCommand().mapFrom(this.entity)
    } else {
      let request = new CreateAccountReferencesGroupCommand();
      request.parentId = this.entity.parentId;
      this.request = request;
    }

    this.form.controls['parentTitle']?.disable();
  }


  async add() {
    await this._mediator.send(<CreateAccountReferencesGroupCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }

  async update() {
    await this._mediator.send(<UpdateAccountReferencesGroupCommand>this.request).then(res => {
      this.dialogRef.close(res)
    })
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

  close(): any {
  }

  protected readonly PageModes = PageModes;

}
