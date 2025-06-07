import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {TableConfigurations} from "../../../../../../core/components/custom/table/models/table-configurations";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {Customer} from "../../../../../sales/entities/Customer";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {GetCustomersQuery} from "../../../../../sales/repositories/customers/queries/get-customers-query";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PersonCustomerDialogComponent} from "./person-customer-dialog/person-customer-dialog.component";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {PageModes} from "../../../../../../core/enums/page-modes";
import {IdentityService} from "../../../../../identity/repositories/identity.service";

@Component({
  selector: 'app-person-customer',
  templateUrl: './person-customer.component.html',
  styleUrls: ['./person-customer.component.scss']
})
export class PersonCustomerComponent extends BaseComponent {

  @Input() personId!: number;
  tableConfigurations!: TableConfigurations

  constructor(
    private mediator: Mediator,
    private dialog: MatDialog,
    private identityService: IdentityService
  ) {
    super()
  }

  ngAfterViewInit() {

    this.actionBar.actions = [

      PreDefinedActions.add().setShow(this.identityService.doesHavePermission("AddCustomers")),
      PreDefinedActions.edit().setShow(this.identityService.doesHavePermission("EditCustomers")),
      PreDefinedActions.delete().setShow(this.identityService.doesHavePermission("DeleteCustomers")),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    let columns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'fullName',
        'نام کامل',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('fullName', TableColumnFilterTypes.Text)
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

    searchQueries.push(new SearchQuery({
      propertyName: 'personId',
      comparison: 'equals',
      values: [this.personId],
      nextOperand: 'and'
    }))
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
      this.entities = res.data;
      res.totalCount && (this.tableConfigurations.pagination.totalItems = res.totalCount);

    })
  }

  navigateToCustomer(customer: any) {
    // let config = new MatDialogConfig()
    // config.data = {
    //   customerId: customer.id
    // }
    // this.dialog.open(PersonCustomerDialogComponent,config)
    let entityToUpdate = customer
    let config = new MatDialogConfig()
    config.data = {
      customerId: entityToUpdate.id,
      pageMode: PageModes.Update
    }
    let dialogRef = this.dialog.open(PersonCustomerDialogComponent, config)
    dialogRef.afterClosed().subscribe(async x => {
      await this.get()
    })
  }

  add(): any {
    let config = new MatDialogConfig()
    config.data = {
      pageMode: PageModes.Add,
      personId: this.personId
    }
    let dialogRef = this.dialog.open(PersonCustomerDialogComponent, config)
    dialogRef.afterClosed().subscribe(x => {
      this.get()
    })
  }

  update(param?: any): any {
    let entityToUpdate = this.entities.find(x => x.selected)
    let config = new MatDialogConfig()
    config.data = {
      customerId: entityToUpdate.id,
      pageMode: PageModes.Update
    }
    let dialogRef = this.dialog.open(PersonCustomerDialogComponent, config)
    dialogRef.afterClosed().subscribe(x => {
      this.get()
    })
  }

  close(): any {
  }

  delete(param?: any): any {
    let entityToUpdate = this.entities.find(x => x.selected)
    let config = new MatDialogConfig()
    config.data = {
      customerId: entityToUpdate.id,
      pageMode: PageModes.Delete
    }
    let dialogRef = this.dialog.open(PersonCustomerDialogComponent, config)
    dialogRef.afterClosed().subscribe(x => {
      this.get()
    })
  }


  initialize(params?: any): any {
  }


}
