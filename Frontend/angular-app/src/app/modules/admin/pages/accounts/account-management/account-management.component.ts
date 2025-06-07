import {Component, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {FormControl, FormGroup} from "@angular/forms";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {BaseValue} from "../../../entities/base-value";
import {PageModes} from "../../../../../core/enums/page-modes";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {Person} from "../../../entities/person";
import {UpdatePersonCommand} from "../../../repositories/person/commands/update-person-command";
import {CreatePersonCommand} from "../../../repositories/person/commands/create-person-command";
import {UpdateAccountCommand} from "../../../repositories/account/commands/update-account-command";
import {Account} from "../../../entities/account";
import {CreateAccountCommand} from "../../../repositories/account/commands/create-account-command";

@Component({
  selector: 'app-account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.scss']
})
export class AccountManagementComponent extends BaseComponent {
  pageModes = PageModes;

  form = new FormGroup({
    id: new FormControl(),
    title: new FormControl(),
    code: new FormControl(),
    accountReferenceTypeBaseId: new FormControl(),
    generateAccountCode: new FormControl(),
  })

  personForm = new FormGroup({
    id: new FormControl(),
    personTypeId: new FormControl(),
    firstName: new FormControl(),
    lastName: new FormControl(),
    fatherName: new FormControl(),
    nationalNumber: new FormControl(),
    economicCode: new FormControl(),
  })
  accountReferenceTypes: BaseValue[] = [];
  personTypes: BaseValue[] = []


  showPersonPrompt = true;
  isPersonLegal!: boolean | undefined;

  constructor(
    private _mediator: Mediator,
  ) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('personAccountReferenceTypeBaseId')),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('PersonTypes')),
    ]).subscribe(async ([
                          accountReferenceTypes,
                          personTypes
                        ]) => {
      this.accountReferenceTypes = accountReferenceTypes;
      this.personTypes = personTypes;
      await this.initialize()
    })
  }

  async initialize(entity?: Account) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      else this.request = new UpdateAccountCommand().mapFrom(entity);

    } else {
      this.pageMode = PageModes.Add;
      this.request = new CreateAccountCommand()
    }
  }

  add(param?: any): any {
    this.form.controls['id']?.setValue(1)
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  get(param?: any): any {
  }


  update(param?: any): any {
  }

  handleCodeGeneration(generateCode: boolean) {
    if (generateCode) this.form.controls['code'].disable();
    else {
      this.form.controls['code'].enable();
      this.form.controls['accountReferenceTypeBaseId'].setValue(null)
    }
  }

  handlePersonTypeSelection() {
    let personTypeId = this.personForm.controls['personTypeId']?.value;
    if (personTypeId) {
      this.showPersonPrompt = false;
      this.personTypes.find(x => x.id === personTypeId)?.uniqueName?.toLowerCase() === 'legal' ? this.isPersonLegal = true : this.isPersonLegal = false
    }
  }

  clearPerson() {
    this.personForm.reset()
    this.isPersonLegal = undefined;
    this.showPersonPrompt = true;
  }


  reset(): any {
    this.clearPerson()
    return super.reset();
  }
}
