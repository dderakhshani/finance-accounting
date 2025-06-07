import {Component, TemplateRef, ViewChild} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {ActivatedRoute, Router} from "@angular/router";
import {BaseValue} from "../../../entities/base-value";
import {CreatePersonCommand} from "../../../repositories/person/commands/create-person-command";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {forkJoin} from "rxjs";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {CountryDivision} from "../../../entities/countryDivision";
import {GetCountryDivisionsQuery} from "../../../repositories/country-division/queries/get-country-divisions-query";
import {GetPersonQuery} from "../../../repositories/person/queries/get-person-query";
import {Person} from "../../../entities/person";
import {PageModes} from "../../../../../core/enums/page-modes";
import {UpdatePersonCommand} from "../../../repositories/person/commands/update-person-command";
import {environment} from "../../../../../../environments/environment";
import {UploadFileCommand} from "../../../../../shared/repositories/files/upload-file-command";
import {AccountReferencesGroup} from "../../../../accounting/entities/account-references-group";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {
  GetAccountReferencesGroupsQuery
} from "../../../../accounting/repositories/account-reference-group/queries/get-account-references-groups-query";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {IdentityService} from "../../../../identity/repositories/identity.service";
import {PersonEmployeeComponent} from "./person-employee/person-employee.component";
import {Toastr_Service} from "../../../../../shared/services/toastrService/toastr_.service";

@Component({
  selector: 'app-add-person',
  templateUrl: './add-person.component.html',
  styleUrls: ['./add-person.component.scss']
})
export class AddPersonComponent extends BaseComponent {
  @ViewChild('personEmployee', {read: TemplateRef}) personEmployee!: TemplateRef<any>;
  displayMode: 'default' | 'sales' = 'default';
  defaultPhotoUrl = '/assets/images/avatar-placeholder.jpg';
  pageModes = PageModes;
  genders: BaseValue[] = [];
  personLegals: BaseValue[] = [];
  personGovernmental: BaseValue[] = [];

  countryDivisions: CountryDivision[] = [];
  accountReferencesGroups: AccountReferencesGroup[] = [];


  legalBaseValueId!: number;
  nonGovernmentalBaseValueId!: number;
  maleBaseValueId!: number;

  employeeChild!: PersonEmployeeComponent

  @ViewChild(PersonEmployeeComponent)
  set saveEmployee(child: PersonEmployeeComponent) {
    this.employeeChild = child
  }

  constructor(
    private _mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    public identityService: IdentityService,
    private _toaster:Toastr_Service,
  ) {
    super(route, router);
    this.request = new CreatePersonCommand()
  }


  async ngOnInit() {
    await this.resolve()
  }

  ngAfterViewInit() {
    this.loadingSubscription = this.loadingFinished.subscribe(() => {
      this.actionBar.actions = [
        PreDefinedActions.save(),
        PreDefinedActions.add(),
        PreDefinedActions.list(),
        <Action>{
          title: 'نمایش تفصیل شناور',
          color: 'primary',
          type: ActionTypes.custom,
          uniqueName: 'navigateToAccountReference',
          disabled: !this.form?.value?.accountReferenceId,
          icon: 'account_balance_wallet'
        },

      ]
    })
  }

  async resolve() {
    this.isLoading = true;
    if (this.getQueryParam('dm')) this.displayMode = this.getQueryParam('dm')

    forkJoin([
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('gender')),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('PersonTypes')),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('PersonGovernment')),
      this._mediator.send(new GetCountryDivisionsQuery()),
      this._mediator.send(new GetAccountReferencesGroupsQuery(0, 0, [new SearchQuery({
        propertyName: 'isEditable',
        comparison: 'equals',
        values: [true]
      })])),
    ]).subscribe(async ([
                          genders,
                          personLegals,
                          personGovernmental,
                          countryDivisions,
                          accountReferencesGroups,
                        ]) => {
      this.genders = genders;
      this.personLegals = personLegals;
      this.personGovernmental = personGovernmental;
      this.accountReferencesGroups = accountReferencesGroups.data;
      this.countryDivisions = countryDivisions;

      this.legalBaseValueId = <number>(personLegals.find(x => x.uniqueName === "Legal")?.id)
      this.nonGovernmentalBaseValueId = <number>(personGovernmental.find(x => x.uniqueName === "Non Governmental")?.id)
      this.maleBaseValueId = <number>(genders.find(x => x.uniqueName === "Man")?.id)
      await this.initialize()
    })
  }

  async initialize(entity?: Person) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'));
      this.request = new UpdatePersonCommand().mapFrom(entity);
      this.form.controls['accountReferenceCode']?.disable();
      this.form.controls['accountReferenceGroupId']?.disable();
    } else {
      this.pageMode = PageModes.Add;
      let request = new CreatePersonCommand();
      request.legalBaseId = this.personLegals.find(x => x.id !== this.legalBaseValueId)?.id;
      request.governmentalBaseId = this.nonGovernmentalBaseValueId;
      request.genderBaseId = this.maleBaseValueId;
      request.taxIncluded = true;
      this.request = request;

    }
    this.isLoading = false;
  }

  override async submit() {
    if (this.form.dirty) {

      if (this.pageMode === PageModes.Add) {
        await this.add()
      } else {
        await this.update()
      }

      await this.employeeChild.submit()
    }else {
       this._toaster.showToast({
        type: 'info',
        message: 'تغییری برای ذخیره سازی وجود ندارد '
      });
      return;
    }
  }

  async add() {
    let response = await this._mediator.send(<CreatePersonCommand>this.request)
    return await this.initialize(response);
  }

  async get(personId: number) {
    return await this._mediator.send(new GetPersonQuery(personId))
  }

  async update() {
    let response = await this._mediator.send(<UpdatePersonCommand>this.request);
    return await this.initialize();
  }

  delete(param?: any) {

  }

  close() {

  }


  accountReferenceGroupDisplayFn(groupId: number) {
    let city = this.accountReferencesGroups.find(x => x.id === groupId)
    return city ? [city.code, city.title].join(' ') : '';
  }

  countryDivisionDisplayFn(countryDivisionId: number) {
    let city = this.countryDivisions.find(x => x.id === countryDivisionId)
    return city ? [city.ostanTitle, city.shahrestanTitle].join(', ') : '';
  }

  async navigateToPersonList() {
    await this.router.navigateByUrl('/admin/person/list')
  }

  async onPhotoInput(event: any) {
    await this._mediator.send(new UploadFileCommand(event.target.files[0])).then(response => {
      this.form.controls['profileImageReletiveAddress'].setValue(response);
      this.form.controls['profileUrl'].setValue(environment.apiURL + "/" + response);
    })
  }

  async onSignatureInput(event: any) {
    await this._mediator.send(new UploadFileCommand(event.target.files[0])).then(response => {
      this.form.controls['signatureImageReletiveAddress'].setValue(response);
      this.form.controls['signatureUrl'].setValue(environment.apiURL + "/" + response);
    })
  }

  async reset() {
    await this.deleteQueryParam("id")
    return super.reset();
  }

  handleCustomAction(action: Action) {
    if (action.uniqueName === "navigateToAccountReference") {
      this.router.navigateByUrl("/accounting/accountReferences/add?id=" + this.form?.value?.accountReferenceId)
    }
  }
}
