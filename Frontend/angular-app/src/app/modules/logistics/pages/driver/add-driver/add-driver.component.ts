import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {BaseValue} from "../../../../admin/entities/base-value";
import {ActivatedRoute, Router} from "@angular/router";
import {AddDriverCommand} from "../../../repositories/driver/commands/add-driver-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Driver} from "../../../entities/driver";
import {UpdateDriverCommand} from "../../../repositories/driver/commands/update-driver-command";
import {GetDriverByIdQuery} from "../../../repositories/driver/queries/get-driver-by-id-query";
import {Person} from "../../../../admin/entities/person";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../../admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchPersonQuery} from "../../../../admin/repositories/person/queries/search-person-query";

@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.scss']
})
export class AddDriverComponent extends BaseComponent{
  driveTypes!:BaseValue[];
  persons: Person[] = [];
  constructor(
    private _mediator: Mediator,
    private router:Router,
    private route:ActivatedRoute

  ) {
    super(route,router);
    this.request = new AddDriverCommand();

  }

  async ngOnInit() {
    await this.resolve();
    await this.initialize();
  }
  async resolve() {

    await this._mediator.send(new GetBaseValuesByUniqueNameQuery('driveType')).then(res => {
      this.driveTypes = res
    })
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
    ]
  }
  async initialize(entity?:Driver) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateDriverCommand().mapFrom(entity);

      this.form.controls['creator'].disable();
    }
  }

  async add() {
    let response = await this._mediator.send(<AddDriverCommand>this.request)
    this.request = new UpdateDriverCommand().mapFrom(response)

  }
  async update() {
    let response = await this._mediator.send(<UpdateDriverCommand>this.request)
    this.request = new UpdateDriverCommand().mapFrom(response)
  }

  async get(id: number) {
    return await this._mediator.send(new GetDriverByIdQuery(id))
  }

  delete(): any {
  }
  close(): any {
  }

  async showDriverList() {
    await this.router.navigateByUrl('/logistics/driver/list')
  }

  async searchPerson(searchTerm: string) {
    await this._mediator.send(new SearchPersonQuery(searchTerm)).then(res => {
      this.persons = res
    })
  }

  getPersonDisplayName(personId:number) {
    let person = this.persons.find(x => x.id === personId);
    if (person) {
      return [person.firstName,person.lastName].join(' ')
    } else {
      return ''
    }
  }

  getDriverInformationByPersonId(personId:number) {
    //let response = this._mediator.send(new GetDriverByIdQuery(personId))
  //  if(response) {
    //  this.pageMode = PageModes.Update;
     //  this.request = new UpdateDriverCommand().mapFrom(response)
    // }
  }



}
