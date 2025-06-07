import {Component, Input} from '@angular/core';
import {Router} from "@angular/router";
import {User} from "../../../entities/user";
import {ExtensionsService} from "../../../../../shared/services/extensions/extensions.service";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {GetUsersByRoleIdQuery} from '../../../repositories/user/queries/get-users-by-role-id-query';
import {GetUsersQuery} from "../../../repositories/user/queries/get-users-query";
import {DeleteUsersByRoleIdCommand} from "../../../repositories/user/commands/delete-user-by-role-id-query";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent extends BaseComponent {

  @Input() set roleId(value: number) {
    this._roleId = value;
    if (this._roleId)
      this.get()
  }


  _roleId!: number | null;
  _userId!: number | null;
  users: User[] = [];
  tableConfigurations!: TableConfigurations;



  constructor(
    private router: Router,
    private _mediator: Mediator,
    public extensionsService: ExtensionsService
  ) {
    super()
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.edit(),
      PreDefinedActions.add().setTitle("افزودن کاربر"),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),
    ]
  }


  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    let columns: TableColumn[] = [
      new TableColumn(
        'selected',
        '',
        TableColumnDataType.Select,
      ),
      new TableColumn(
        'index',
        'ردیف',
        TableColumnDataType.Index,
      ),
      new TableColumn(
        'fullName',
        'نام و نام خانوادگی',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('fullName',TableColumnFilterTypes.Text),

      ),
      new TableColumn(
        'username',
        'نام کاربری',
        TableColumnDataType.Text,
        '15%',
        true,
          new TableColumnFilter('username',TableColumnFilterTypes.Text),

      ),

      new TableColumn(
        'unitPosition',
        'واحد',
        TableColumnDataType.Text,
        '10%',
        true,
          new TableColumnFilter('unitPosition',TableColumnFilterTypes.Text),

      ),
      new TableColumn(
        'role',
        'نقش',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('role',TableColumnFilterTypes.Text),

      ),
      new TableColumn(
        'failedCount',
        'ورود ناموفق',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('failedCount',TableColumnFilterTypes.Number),

      ),
      new TableColumn(
        'isBlocked',
        'وضعیت انسداد کاربر',
        TableColumnDataType.CheckBox,
        '5%',
        true,
        new TableColumnFilter('isBlocked',TableColumnFilterTypes.CheckBox),

      ),
      new TableColumn(
        'blockedReason',
        'علت وضعیت کاربر',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('blockedReason',TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'lastOnlineTime',
        'آخرین زمان ورود',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('lastOnlineTime',TableColumnFilterTypes.Date),
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false,true))
    await this.get();
  }
  initialize() {
    throw new Error('Method not implemented.');
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

    if (this._roleId) {
      let request = new GetUsersByRoleIdQuery(this._roleId)
      let response = await this._mediator.send(request);
      this.users = response.data;
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
    } else {
      let request = new GetUsersQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
      let response = await this._mediator.send(request);
      this.users = response.data;
      response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);
    }

  }
  //
  async delete() {
    if (this._roleId) {
      // @ts-ignore
      let request = new DeleteUsersByRoleIdCommand(this._roleId,this.users.filter(x => x.selected).map(x => x.id));
      let response = await this._mediator.send(request);
      await this.get();
    }
      // } else {
      //   await this.userService.deleteUsersByRoleId(1, 30).toPromise().then(res => {
      //     this.users = res.objResult.data;
      //   })
  }


  async navigateToUser(user: User) {
    await this.router.navigateByUrl(`admin/user/add?id=${user.id}`)
  }



  async add() {
    await this.router.navigateByUrl(`admin/user/add`)
  }

  async update() {
    // @ts-ignore
    let user = this.users.filter(x => x.selected)[0];
    if (user) {
      await this.router.navigateByUrl(`admin/user/add?id=${user.id}`)
    }
  }

  close() {
    throw new Error('Method not implemented.');
  }
}
