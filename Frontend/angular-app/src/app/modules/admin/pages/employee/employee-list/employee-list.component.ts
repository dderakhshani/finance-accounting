import {Component} from '@angular/core';
import {Employee} from "../../../entities/employee";
import {Router} from "@angular/router";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetEmployeesQuery} from "../../../repositories/employee/queries/get-employees-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent extends BaseComponent{


  employees:Employee[] = [];
  tableConfigurations!:TableConfigurations;


  constructor(
    private _mediator: Mediator,
    private  router:Router,

  ) {
    super()
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.edit(),
      PreDefinedActions.add().setTitle("افزودن کارمند"),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve(){
    let columns: TableColumn[] = [
      new TableColumn('selected','',TableColumnDataType.Select,'2.5%'),
      new TableColumn('index','',TableColumnDataType.Index,'2.5%'),
      new TableColumn(
        'fullName',
        'نام',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('fullName',TableColumnFilterTypes.Text),
        (employee:Employee) => {
          return [employee.firstName,employee.lastName].join(' ');
        },
      ),
      new TableColumn(
        'nationalNumber',
        'کد ملی',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('nationalNumber',TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'employeeCode',
        'کد کارمندی',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('employeeCode',TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'accountReferenceCode',
        'کد تفصیل',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('accountReferenceCode',TableColumnFilterTypes.Number)
      ),

      new TableColumn(
        'employmentDate',
        'تاریخ استخدام',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('employmentDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'leaveDate',
        'تاریخ ترک کار',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('leaveDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'floating',
        'درحال جابه جایی',
        TableColumnDataType.CheckBox,
        '10%',
        true,
        new TableColumnFilter('floating',TableColumnFilterTypes.CheckBox)
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

    let request = new GetEmployeesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.employees = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async update() {
    // @ts-ignore
    let employee = this.employees.filter(x => x.selected)[0]
    if (employee) {
      await this.navigateToEmployee(employee)
    }
  }

  async add() {
    await this.router.navigateByUrl(`admin/person/add`)
  }

  async navigateToEmployee(employee:Employee) {
    await this.router.navigateByUrl(`admin/person/add?id=${employee.personId}`)
  }

  close(): any {
  }

  delete(): any {
  }


}
