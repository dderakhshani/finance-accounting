import {Component} from '@angular/core';
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {Year} from "../../../entities/year";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {CreateCompanyCommand} from "../../../repositories/company/commands/create-company-command";
import {PageModes} from "../../../../../core/enums/page-modes";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetCompanyQuery} from "../../../repositories/company/queries/get-company-query";
import {Company} from "../../../entities/company";
import {UpdateCompanyCommand} from "../../../repositories/company/commands/update-company-command";
import {DeleteCompanyCommand} from "../../../repositories/company/commands/delete-company-command";
import {FormControl, FormGroup} from "@angular/forms";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";

@Component({
  selector: 'app-companies',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.scss']
})
export class AddCompanyComponent extends BaseComponent {

  constructor(
    private route: ActivatedRoute,
    private router:Router,
    private _mediator: Mediator
  ) {
    super(route,router)
    this.request = new CreateCompanyCommand()
  }

  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),


    ]
  }

  async ngOnInit() {
    await this.resolve()
  }



  async resolve() {


    await this.initialize()
  }

  async initialize(entity?:Company) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateCompanyCommand().mapFrom(entity)
    } else {
      this.request = new CreateCompanyCommand()
    }
  }

  async get(id: number) {
    return await this._mediator.send(new GetCompanyQuery(id));
  }

  async add() {
    let response = await this._mediator.send(<CreateCompanyCommand>this.request)
    await this.initialize(response)
  }

  async update() {
    let response = await this._mediator.send(<UpdateCompanyCommand>this.request)
    await this.initialize(response)
  }

  close(): any {
  }

  async delete() {
    await this._mediator.send(new DeleteCompanyCommand(this.form.controls['id'].value))
  }

  async navigateToCompaniesList() {
    await this.router.navigateByUrl('/admin/companies/list')
  }
 async reset() {
    await this.deleteQueryParam("id")
   return super.reset();
 }
}
