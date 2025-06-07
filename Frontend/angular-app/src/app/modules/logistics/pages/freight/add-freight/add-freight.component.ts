import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {AddFreightCommand} from "../../../repositories/freight/commands/add-freight-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ActivatedRoute, Router} from "@angular/router";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {Freight} from "../../../entities/freight";
import {UpdateFreightCommand} from "../../../repositories/freight/commands/update-freight-command";

@Component({
  selector: 'app-add-freight',
  templateUrl: './add-freight.component.html',
  styleUrls: ['./add-freight.component.scss']
})
export class AddFreightComponent extends BaseComponent{

  constructor(
    private _mediator: Mediator,
    private router:Router,
    private route:ActivatedRoute
  ) {
    super(route,router);
    this.request = new AddFreightCommand();
  }

  async ngOnInit(){
    await this.resolve();
    // await this.initialize();
  }

  resolve(): any {
    this.formActions = [
    FormActionTypes.add,
    FormActionTypes.save,
  ]
  }

  async initialize(entity ?: Freight) {
    // if (entity || this.getQueryParam('id')) {
    //   this.pageMode = PageModes.Update;
    //   if (!entity) entity = await this.get(this.getQueryParam('id'));
    //
    //   this.request = new UpdateFreightCommand().mapFrom(entity);
    //
    //   this.form.controls['creator'].disable();
    // }
  }

  async add() {

    let response = await this._mediator.send(<AddFreightCommand>this.request)
    this.request = new UpdateFreightCommand().mapFrom(response)
  }

  async get(id: number){

  }

  async update(){
    let response = await this._mediator.send(<AddFreightCommand>this.request)
    this.request = new UpdateFreightCommand().mapFrom(response)
  }

  close(): any {
  }

  delete(): any {
  }
}
