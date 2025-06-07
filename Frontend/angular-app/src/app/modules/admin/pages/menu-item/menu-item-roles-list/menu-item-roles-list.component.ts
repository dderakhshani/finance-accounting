import {Component, Input} from '@angular/core';
import {Role} from "../../../entities/role";
import {TableConfigurations} from "../../../../../core/components/custom/table/models/table-configurations";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetMenuItemsRolesQuery} from "../../../repositories/menu-item/queries/get-menu-items-roles-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";

@Component({
  selector: 'app-menu-item-roles-list',
  templateUrl: './menu-item-roles-list.component.html',
  styleUrls: ['./menu-item-roles-list.component.scss']
})
export class MenuItemRolesListComponent extends BaseComponent {

  @Input() set menuItemPermissionId(value: number) {
    this._menuItemPermissionId = value;
    if (this._menuItemPermissionId)
      this.get()
  }

  _menuItemPermissionId!: number
  roles: Role[] = [];
  tableConfigurations!: TableConfigurations;


  constructor(private _mediator: Mediator) {
    super();
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    let columns: TableColumn[] = [
      new TableColumn(
        "selected",
        "",
        TableColumnDataType.Select,
        "5%",
      ),
      new TableColumn(
        "index",
        "ردیف",
        TableColumnDataType.Index,
        "5%",
      ),
      new TableColumn(
        "title",
        "عنوان",
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        "uniqueName",
        "نام یکتا",
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('uniqueName', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        "description",
        "شرح",
        TableColumnDataType.Text
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }

  async get() {
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

    let request = new GetMenuItemsRolesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty, this._menuItemPermissionId)
    let response = await this._mediator.send(request)
    this.roles = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
  }

  add(): any {
  }

  close(): any {
  }

  delete(): any {
  }

  initialize(params?: any): any {
  }

  update(): any {
  }

  navigateToUser($event: any) {

  }
}
