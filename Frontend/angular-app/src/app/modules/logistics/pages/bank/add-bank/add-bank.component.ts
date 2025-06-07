import { Component } from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {BaseValue} from "../../../../admin/entities/base-value";
import {AddBankCommand} from "../../../repositories/bank/commands/add-bank-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {UpdateBankCommand} from "../../../repositories/bank/commands/update-bank-command";
import {GetBankByIdQuery} from "../../../repositories/bank/queries/get-bank-by-id-query";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Bank} from "../../../entities/bank";
import {ActivatedRoute, Router} from "@angular/router";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";

@Component({
  selector: 'app-add-bank',
  templateUrl: './add-bank.component.html',
  styleUrls: ['./add-bank.component.scss']
})
export class AddBankComponent extends BaseComponent{
  bankTypes!:BaseValue[];
  constructor(
    private _mediator: Mediator,
    private router:Router,
    private route:ActivatedRoute

  ) {
    super(route,router);
    this.request = new AddBankCommand();
  }


  async ngOnInit() {
    await this.resolve();
    await this.initialize()
  }
  async resolve() {
    await this._mediator.send(new GetBaseValuesByUniqueNameQuery('bankType')).then(res => {
      this.bankTypes = res
    })
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
    ]
  }
  async initialize(entity?:Bank) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateBankCommand().mapFrom(entity);

      this.form.controls['creator'].disable();
    }
  }

  async add() {
    let response = await this._mediator.send(<AddBankCommand>this.request)
    this.request = new UpdateBankCommand().mapFrom(response)

    await this.addQueryParam('id',response.id)
  }
  async update() {
    let response = await this._mediator.send(<UpdateBankCommand>this.request)
    this.request = new UpdateBankCommand().mapFrom(response)
  }
  async get(id: number) {
    return await this._mediator.send(new GetBankByIdQuery(id))
  }
  delete(): any {
  }
  close(): any {
  }


}
