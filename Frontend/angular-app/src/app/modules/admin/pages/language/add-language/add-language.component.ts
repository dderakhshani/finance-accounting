import {Component, OnInit} from '@angular/core';
import {BaseValue} from "../../../entities/base-value";
import {environment} from "../../../../../../environments/environment";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {PageModes} from "../../../../../core/enums/page-modes";
import {CreateLanguageCommand} from "../../../repositories/languages/commands/create-language-command";
import {UpdateLanguageCommand} from "../../../repositories/languages/commands/update-language-command";
import {GetLanguageQuery} from "../../../repositories/languages/queries/get-language-query";
import {Language} from "../../../entities/language";
import {GetBaseValueQuery} from "../../../repositories/base-value/queries/get-base-value-query";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {forkJoin} from "rxjs";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {DeleteLanguageCommand} from "../../../repositories/languages/commands/delete-language-command";
import {DeleteCompanyCommand} from "../../../repositories/company/commands/delete-company-command";

@Component({
  selector: 'app-add-language',
  templateUrl: './add-language.component.html',
  styleUrls: ['./add-language.component.scss']
})
export class AddLanguageComponent extends BaseComponent {


  fileServerAddress = environment.apiURL;

  currencies!: BaseValue[];
  directions!: BaseValue[];
  addLanguageForm: any;


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private _mediator: Mediator
  ) {
    super(route, router);
    this.request = new CreateLanguageCommand();
  }

  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),

    ]

  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {


    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery("CurrencyType")),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery("languagedirection"))
    ]).subscribe(async ([
                          currenciesTypeResponse,
                          languageDirectionResponse
                        ]) => {
      this.currencies = currenciesTypeResponse;
      this.directions = languageDirectionResponse;
      await this.initialize()
    })
  }


  async initialize(entity?: Language) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdateLanguageCommand().mapFrom(entity);
    } else {
      this.request = new CreateLanguageCommand()
    }
  }

  async get(id: number) {
    return await this._mediator.send(new GetLanguageQuery(id))
  }

  async add() {
    let response = await this._mediator.send(<CreateLanguageCommand>this.request)
    await this.initialize(response)
  }

  async update() {
    let response = await this._mediator.send(<UpdateLanguageCommand>this.request)
    await this.initialize(response)
  }

  async navigateToLanguageList() {
    await this.router.navigateByUrl('/admin/languages/list')
  }

  close(): any {
  }

  async delete() {
    await this._mediator.send(new DeleteCompanyCommand(this.form.controls['id'].value))
  }

  onPhotoInput($event: Event) {

  }

  async reset() {
    await this.deleteQueryParam("id")
    return super.reset();
  }
}
