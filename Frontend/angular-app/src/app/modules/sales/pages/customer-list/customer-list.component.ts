import {Component} from '@angular/core';
import {Customer} from "../../entities/Customer";
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {GetCustomersQuery} from "../../repositories/customers/queries/get-customers-query";
import {
  TableConfigurations
} from "../../../../core/components/custom/table/models/table-configurations";
import {BaseValue} from "../../../admin/entities/base-value";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {Router} from "@angular/router";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TablePaginationOptions} from "../../../../core/components/custom/table/models/table-pagination-options";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import {IdentityService} from "../../../identity/repositories/identity.service";

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.scss']
})
export class CustomerListComponent extends BaseComponent {
  customers: Customer[] = [];
  tableConfigurations!: TableConfigurations;
  basevalues: BaseValue[] = []

  constructor(
    private mediator: Mediator,
    private router: Router,
    public identityService: IdentityService,
  ) {
    super();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.edit().setShow(this.identityService.doesHavePermission("AddCustomers")),
      PreDefinedActions.add().setShow(this.identityService.doesHavePermission("AddCustomers")),
      PreDefinedActions.refresh(),
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    let columns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'id',
        'شناسه',
        TableColumnDataType.Number,
        '25%',
        true,
        new TableColumnFilter('id', TableColumnFilterTypes.Number),
      ),
      new TableColumn(
        'firstNameLastName',
        'نام و نام خانوادگی',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('fullName', TableColumnFilterTypes.Text),
        (customer: Customer) => {
          return [customer.firstName, customer.lastName].join(' ')
        }
      ),
      new TableColumn(
        'fullName',
        'نام کامل',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('fullName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'nationalNumber',
        'کد ملی',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('nationalNumber', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'address',
        'آدرس',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('address', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'phoneNumbers',
        'تلفن',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('phoneNumbers', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'customerCode',
        'کد مشتری',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('customerCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'accountReferenceCode',
        'کد تفصیل',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('accountReferenceCode', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'depositId',
        'شناسه پرداخت',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('depositId', TableColumnFilterTypes.Text)
      ),


      new TableColumn(
        'accountReferenceGroupCode',
        'گروه تفصیل',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('accountReferenceGroupCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'customerTypeUniqueName',
        'گروه قیمتی',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('customerTypeUniqueName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'currentExpertCode',
        'کد کارشناس',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('currentExpertCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'isActive',
        'وضعیت',
        TableColumnDataType.CheckBox,
        '25%',
        true,
      ),
      new TableColumn(
        'createdAt',
        'تاریخ ثبت',
        TableColumnDataType.Date,
        '25%',
        true,
      )
      ,
      new TableColumn(
        'createdBy',
        'ثبت کننده',
        TableColumnDataType.Text,
        '25%',
        true,
      )
    ]


    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true));
    await this.get()

  }

  async get(param?: any) {

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
    return await this.mediator.send(new GetCustomersQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)).then(res => {
      this.customers = res.data;
      res.totalCount && (this.tableConfigurations.pagination.totalItems = res.totalCount);

    })
  }


  async navigateToCustomer(customer: Customer) {
    if (this.identityService.doesHavePermission("EditCustomers")) {
      await this.router.navigateByUrl(`/sales/customers/add?id=${customer.personId}&dm=sales`)
    }
  }


  async update() {
    // @ts-ignore
    let customer = this.customers.filter(x => x.selected)[0]
    if (customer) {
      await this.navigateToCustomer(customer)
    }
  }

  async add() {
    await this.router.navigateByUrl(`/sales/customers/add?dm=sales`)
  }

  close(): any {
  }

  delete(param?: any): any {
  }

  initialize(params?: any): any {
  }

}
