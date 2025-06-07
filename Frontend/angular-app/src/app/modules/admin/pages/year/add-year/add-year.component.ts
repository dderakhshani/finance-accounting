import {Component} from '@angular/core';
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {ActivatedRoute, Router} from "@angular/router";
import {Year} from "../../../entities/year";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CreateYearCommand} from "../../../repositories/year/command/create-year-command";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetYearQuery} from "../../../repositories/year/queris/get-year-query";
import {UpdateYearCommand} from "../../../repositories/year/command/update-year-command";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";


@Component({
  selector: 'app-add-year',
  templateUrl: './add-year.component.html',
  styleUrls: ['./add-year.component.scss']
})
export class AddYearComponent extends BaseComponent {




  constructor(
    private _mediator:Mediator,
    private route: ActivatedRoute,
    private router:Router)
  {
    super(route,router);
    this.request = new CreateYearCommand();
  }

  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),
      PreDefinedActions.delete()
      ]
  }

  async ngOnInit() {
    await this.resolve();

  }

  async resolve() {
    await this.initialize()
  }


  async initialize(entity?:Year) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateYearCommand().mapFrom(entity);
    }
    else {
    }
  }



  async get(id:number){
    return await this._mediator.send(new GetYearQuery(id))
  }

  async add() {
    let response = await this._mediator.send(<CreateYearCommand>this.request)
    await this.initialize(response)
  }
  async update() {
    let response = await this._mediator.send(<UpdateYearCommand>this.request)
    await this.initialize(response)
  }

  async navigateToYearList() {
    await this.router.navigateByUrl('/admin/year/list')
  }

  close(): any {
  }

  delete(): any {
  }

  async reset() {
    await this.deleteQueryParam("id")
    return super.reset();
  }

}
