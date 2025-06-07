import { Component, OnInit } from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Person} from "../../../entities/person";
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {Account} from "../../../entities/account";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {Router} from "@angular/router";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetPersonsQuery} from "../../../repositories/person/queries/get-persons-query";
import {GetAccountsQuery} from "../../../repositories/account/queries/get-accounts-query";

@Component({
  selector: 'app-accounts-list',
  templateUrl: './accounts-list.component.html',
  styleUrls: ['./accounts-list.component.scss']
})
export class AccountsListComponent extends BaseComponent {

  accounts: Account[] = [];
  tableConfigurations!: TableConfigurations;


  constructor(
    private _mediator: Mediator,
    private router: Router
  ) {
    super();

  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.edit(),
      PreDefinedActions.add().setTitle('افزودن حساب'),
      PreDefinedActions.refresh()
    ];
  }

  async ngOnInit() {
    await this.resolve()
  }
  async resolve() {

    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select,'2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index,'2.5%'),
      new TableColumn(
        'fullName',
        'نام',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('fullName',TableColumnFilterTypes.Text),
        (person:Person) => {
          return [person.firstName,person.lastName].join(' ');
        },
      ),
      new TableColumn(
        'nationalNumber',
        'کد ملی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('nationalNumber',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'accountReferenceCode',
        'کد حسابداری',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('accountReferenceCode',TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'legalBaseTitle',
        'نوع شخص',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('legalBaseTitle',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'governmentalBaseTitle',
        'ماهیت شخص',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('governmentalBaseTitle',TableColumnFilterTypes.Text)
      ),
      new TableColumn('action', '', TableColumnDataType.Action,'2.5%'),

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }

  async get(id?: number) {
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition,
          nextOperand: filter.nextOperand
        }))
      })
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetAccountsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.accounts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async update() {
    // @ts-ignore
    let person = this.people.filter(x => x.selected)[0]
    if (person) {
      await this.navigateToAccount(person)
    }
  }

  async add() {
    await this.router.navigateByUrl(`admin/accountManagement`)
  }

  async navigateToAccount(person:Person) {
    await this.router.navigateByUrl(`admin/accountManagement?id=${person.id}`)
  }
  close(): any {
  }

  delete(param?: any): any {
  }

  initialize(params?: any): any {
  }

}
