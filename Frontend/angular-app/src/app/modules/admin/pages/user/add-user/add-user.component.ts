import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Company} from "../../../entities/company";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetCompaniesQuery} from "../../../repositories/company/queries/get-companies-query";
import {GetUserQuery} from '../../../repositories/user/queries/get-user-query';
import {PageModes} from "../../../../../core/enums/page-modes";
import {UpdateUserCommand} from '../../../repositories/user/commands/update-user-command';
import {AddUserCommand} from "../../../repositories/user/commands/add-user-command";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {BaseValue} from "../../../entities/base-value";
import {FormAction} from "../../../../../core/models/form-action";
import {Person} from "../../../entities/person";
import {ActivatedRoute, Router} from "@angular/router";
import {User} from "../../../entities/user";
import {forkJoin} from "rxjs";
import {GetPersonsQuery} from "../../../repositories/person/queries/get-persons-query";
import {
  GetBaseValuesByUniqueNameQuery
} from "../../../repositories/base-value/queries/get-base-values-by-unique-name-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Role} from "../../../entities/role";
import {GetRolesQuery} from "../../../repositories/role/queries/get-roles-query";
import {FormControl} from "@angular/forms";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent extends BaseComponent {
  companyFormActions: FormAction[] = [];

  companies: Company [] = [];
  blockReasons: BaseValue[] = [];

  persons: Person[] = [];
  selectedPerson!: Person;

  roles: Role[] = [];
  userRoles: Role[] = [];

  tableConfigurations!: TableConfigurations;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private _mediator: Mediator,
  ) {
    super(route, router);
    this.request = new AddUserCommand();

  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.save(),
      PreDefinedActions.add(),
      PreDefinedActions.list(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.save,
      // FormActionTypes.saveandexit,
      FormActionTypes.delete,
      FormActionTypes.list,
    ]
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn('title', 'شرح نقش', TableColumnDataType.Text, '70%'),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true, undefined, true))

    forkJoin([
      this._mediator.send(new GetCompaniesQuery()),
      this.getPersons(),
      this._mediator.send(new GetRolesQuery()),
      this._mediator.send(new GetBaseValuesByUniqueNameQuery('blockedReason'))
    ]).subscribe(async ([
                          companies,
                          persons,
                          roles,
                          blockReasons
                        ]) => {
      this.companies = companies.data;
      this.persons = persons;
      this.roles = roles.data;
      this.blockReasons = blockReasons;
      await this.initialize();
    })

  }

  async initialize(entity?: User) {
    if (entity || this.getQueryParam('id')) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam('id'))
      this.request = new UpdateUserCommand().mapFrom(entity)
    } else {
      this.request = new AddUserCommand();
    }
    this.form.controls['lastOnlineTime'].disable();
    this.form.controls['unitPositionTitle'].disable();
    this.form.controls['failedCount'].disable();
    this.form.controls['blockedReasonBaseId'].disable();

    this.form.valueChanges.subscribe((user: any) => {
      this.userRoles = []
      user.rolesIdList.forEach((roleId: number) => {
        let roleToAdd = this.roles.find(x => x.id === roleId)
        if (roleToAdd) {
          this.userRoles.push(roleToAdd)
          // @ts-ignore
          roleToAdd._selected = true;
        }
      })
      this.userRoles = [...this.userRoles]
    })

    this.form.controls['personId'].valueChanges.subscribe(async (newValue: any) => {
      if (typeof newValue === 'string') await this.getPersons(newValue).then(res => {
        this.persons = res
      })
    })
    this.form.patchValue(this.form.getRawValue())
  }


  async add() {
    await this._mediator.send(<AddUserCommand>this.request).then(res => {
      this.initialize(res)
    });
  }

  async update() {
    await this._mediator.send(<UpdateUserCommand>this.request).then(res => {
      this.initialize(res)
    });
  }

  async get(id?: number) {
    return await this._mediator.send(new GetUserQuery(id ?? 0))
  }


  delete(): any {
  }


  async getPersons(query?: string) {
    let searchQueries: SearchQuery[] = [];
    if (query) {
      searchQueries = [
        new SearchQuery({
          propertyName: 'nationalNumber',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'firstName',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'lastName',
          comparison: 'contains',
          values: [query],
          nextOperand: 'or'
        })
      ]
    }

    if (this.getQueryParam('id')) {
     searchQueries.push( new SearchQuery({
       propertyName: 'id',
       comparison: 'equals',
       values: [+this.getQueryParam('id')],
       nextOperand: 'or'
     }))
    }
    return await this._mediator.send(new GetPersonsQuery(1, 10, searchQueries)).then(res => res.data)
  }

  personDisplayFn(personId: number) {
    let person = this.persons.find(x => x.id === personId) ?? this.selectedPerson
    return person ? [person.firstName, person.lastName, person.nationalNumber].join(' ') : '';
  }


  async navigateToUserList() {
    await this.router.navigateByUrl('/admin/user/list');
  }

  async reset() {
    await this.deleteQueryParam('id')
    return super.reset();
  }

  handleRoleSelection(role: Role) {
    // @ts-ignore
    if (role._selected) {
      this.form.controls['rolesIdList'].push(new FormControl(role.id))
      this.form.patchValue(this.form.getRawValue())
    }
    // @ts-ignore
    if (!role._selected) {
      let roleIdToDelete = this.form.controls['rolesIdList'].controls.find((x: FormControl) => x.value === role.id)
      if (roleIdToDelete) {
        this.form.controls['rolesIdList'].controls.splice(this.form.controls['rolesIdList'].controls.indexOf(roleIdToDelete), 1);
        this.form.patchValue(this.form.getRawValue())
      }
    }
  }

  close(): any {

  }
}
