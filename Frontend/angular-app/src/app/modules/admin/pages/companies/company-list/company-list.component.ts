import { Component } from '@angular/core';
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {Company} from "../../../entities/company";
import {Router} from "@angular/router";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetCompaniesQuery} from "../../../repositories/company/queries/get-companies-query";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-company-list',
  templateUrl: './company-list.component.html',
  styleUrls: ['./company-list.component.scss']
})
  export class CompanyListComponent extends BaseComponent {

  companies: Company []=[];
  tableConfigurations!: TableConfigurations;

  listActions:FormAction[] = [
    FormActionTypes.refresh,
    FormActionTypes.add,
    FormActionTypes.edit,

  ]

  constructor(
    private _mediator: Mediator,
    private router: Router
  ) {
    super();
  }

   async ngOnInit(){
     await this.resolve()
  }

  async resolve() {
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select,'2.5%'),
      new TableColumn('index', '', TableColumnDataType.Index,'2.5%'),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('title',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'uniqueName',
        'نام اختصاصی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('uniqueName',TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'expireDate',
        'تاریخ بسته شدن اطلاعات',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('expireDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'maxNumOfUsers',
        'حداکثر تعداد کاربران',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('maxNumOfUsers',TableColumnFilterTypes.Number)
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }

  initialize() {
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


    let request = new GetCompaniesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.companies = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async add(){
    await this.router.navigateByUrl(`admin/companies/add`)
  }

  async update() {
    // @ts-ignore
    let company = this.companies.filter(x => x.selected)[0]
    if (company) {
      await this.navigateToCompany(company)
    }
  }

  async navigateToCompany(company:Company) {
    await this.router.navigateByUrl(`admin/companies/add?id=${company.id}`)
  }
  close(): any {
  }

  delete(): any {
  }


}

