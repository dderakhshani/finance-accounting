import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {
  CreatePersonBankAccountCommand
} from "../../../../../repositories/person-bank-accounts/commands/create-person-bank-account-command";
import {
  UpdatePersonBankAccountCommand
} from "../../../../../repositories/person-bank-accounts/commands/update-person-bank-account-command";
import {BaseValue} from "../../../../../entities/base-value";

@Component({
  selector: 'app-person-bank-account-dialog',
  templateUrl: './person-bank-account-dialog.component.html',
  styleUrls: ['./person-bank-account-dialog.component.scss']
})
export class PersonBankAccountDialogComponent extends BaseComponent {

  pageModes = PageModes;
  data: any;
  accountTypes: BaseValue[] = [];
  constructor(
    private _mediator: Mediator,
    public dialogRef: MatDialogRef<PersonBankAccountDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.pageMode = data.pageMode;
    this.data = data;
    this.accountTypes = data.accountTypes;
    this.request = new CreatePersonBankAccountCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreatePersonBankAccountCommand();
      newRequest.personId = this.data.personId;
      this.request = newRequest;
    } else if (this.pageMode === PageModes.Update) {
      this.request = new UpdatePersonBankAccountCommand().mapFrom(this.data.personAddress)
    }
  }

  async add(param?: any) {
    let response = await this._mediator.send(<CreatePersonBankAccountCommand>this.request);
    this.dialogRef.close(response)
  }


  async update(param?: any) {
    let response = await this._mediator.send(<UpdatePersonBankAccountCommand>this.request);
    this.dialogRef.close(response)
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

}
