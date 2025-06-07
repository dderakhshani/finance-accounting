import {Component, Inject, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../../core/abstraction/base.component";
import {PageModes} from "../../../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../../../core/services/mediator/mediator.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {CreatePersonPhoneCommand} from "../../../../../repositories/person-phones/commands/create-person-phone-command";
import {UpdatePersonPhoneCommand} from "../../../../../repositories/person-phones/commands/update-person-phone-command";
import {BaseValue} from "../../../../../entities/base-value";

@Component({
  selector: 'app-person-phone-dialog',
  templateUrl: './person-phone-dialog.component.html',
  styleUrls: ['./person-phone-dialog.component.scss']
})
export class PersonPhoneDialogComponent  extends BaseComponent {

  pageModes = PageModes;
  data: any;

  phoneTypes:BaseValue[] = [];
  constructor(
    private _mediator: Mediator,
    public dialogRef: MatDialogRef<PersonPhoneDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data: any
  ) {
    super();
    this.pageMode = data.pageMode;
    this.data = data;
    this.phoneTypes = data.phoneTypes;
    this.request = new CreatePersonPhoneCommand();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.initialize()
  }

  initialize() {
    if (this.pageMode === PageModes.Add) {
      let newRequest = new CreatePersonPhoneCommand();
      newRequest.personId = this.data.personId;
      this.request = newRequest;
    } else if (this.pageMode === PageModes.Update) {
      this.request = new UpdatePersonPhoneCommand().mapFrom(this.data.phoneNumber)
    }
  }

  async add(param?: any) {
    let response = await this._mediator.send(<CreatePersonPhoneCommand>this.request);
    this.dialogRef.close(response)
  }


  async update(param?: any) {
    let response = await this._mediator.send(<UpdatePersonPhoneCommand>this.request);
    this.dialogRef.close(response)
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }

}
