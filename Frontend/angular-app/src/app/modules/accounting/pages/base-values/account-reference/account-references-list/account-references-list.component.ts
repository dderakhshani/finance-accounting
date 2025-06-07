import {Component, Input} from '@angular/core';
import {AccountReference} from "../../../../entities/account-reference";
import {FormActionTypes} from "../../../../../../core/constants/form-action-types";
import {Router} from "@angular/router";
import {NotificationService} from "../../../../../../shared/services/notification/notification.service";
import {BaseComponent} from "../../../../../../core/abstraction/base.component";
import {TableConfigurations} from "../../../../../../core/components/custom/table/models/table-configurations";
import {SearchQuery} from "../../../../../../shared/services/search/models/search-query";
import {Mediator} from "../../../../../../core/services/mediator/mediator.service";
import {
  GetAccountReferencesQuery
} from "../../../../repositories/account-reference/queries/get-account-references-query";
import {PreDefinedActions} from "../../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../../core/components/custom/table/models/table-column";
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { UpdateDepositPersonCommand } from 'src/app/modules/admin/repositories/person/commands/update-deposit-person-command';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-account-references-list',
  templateUrl: './account-references-list.component.html',
  styleUrls: ['./account-references-list.component.scss']
})
export class AccountReferencesListComponent extends BaseComponent {
  @Input() _groupId!: number;
  @Input() set groupId(groupId: number) {
    this._groupId = groupId;
    this.get()
  }

  get groupId(): number {
    return this._groupId;
  }

  selectedRowIndex: number = -1;

  accountReferences: AccountReference[] = [];

  tableConfigurations!: TableConfigurations;


  constructor(
    private router: Router,
    private notificationService: NotificationService,
    private _mediator: Mediator,
    public dialog: MatDialog
  ) {
    super();
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.refresh(),
      PreDefinedActions.add()
    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {

    let tableColumns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'code',
        'کد تفصیل',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('code', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'depositId',
        'شناسه واریز',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('depositId', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'nationalNumber',
        'کد ملی',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('nationalNumber', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'employeeCode',
        'کد پرسنلی',
        TableColumnDataType.Text,
        '2.5%',        
        true,
        new TableColumnFilter('employeeCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'description',
        'شرح',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('description', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'status',
        'وضعیت',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('status', TableColumnFilterTypes.Text)
      ),
    ]
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true))

    this.formActions = [
      // FormActionTypes.edit,
      FormActionTypes.refresh,
      // FormActionTypes.delete
    ];

    await this.get();
  }


  initialize(): any {
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

    this.accountReferences = [];

    if (this.groupId) {
      searchQueries.push(new SearchQuery({
        propertyName:'accountReferencesGroupsIdList',
        comparison: 'inList',
        values:[this.groupId]
      }))
      if (!orderByProperty) orderByProperty = 'code ASC'
      await this._mediator.send(new GetAccountReferencesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)).then(res => {
        this.accountReferences = res.data;
        if (res.totalCount) this.tableConfigurations.pagination.totalItems = res.totalCount
      })
    } else {
      await this._mediator.send(new GetAccountReferencesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)).then(res => {
        this.accountReferences = res.data;
        if (res.totalCount) this.tableConfigurations.pagination.totalItems = res.totalCount
      })
    }
  }




  async setDepositIdForCustomerByReferenceCode() {

    let customers =this.accountReferences;

    if (customers == null || customers.length == 0)
      return this.notificationService.showFailureMessage("هیچ تفصیلی برای ایجاد شناسه انتخاب نشده است", 0);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف',
        message: 'آیا از ایجاد شناسه برای این شخص مطمئن هستید ؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: {title: 'بله', show: true}, cancel: {title: 'خیر', show: true}
        }
      }
    });


    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let setDeposits = new UpdateDepositPersonCommand();
        let numbers = customers.filter((x: any) => x.selected).map(item => item.code);
        setDeposits.accountReferences = ((numbers as any))
        this.isLoading = true;
        await this._mediator.send(<UpdateDepositPersonCommand>setDeposits);
        await this.get()
        this.isLoading = false;
      }
    });
  }




  async add() {
    await this.router.navigateByUrl("/accounting/accountReferences/add")
  }

  async update(entity?: any) {
    // @ts-ignore
    let accountReference = this.accountReferences.filter(x => x._selected)[0];
    if (accountReference) {
      await this.router.navigateByUrl(`/accounting/accountReferences/add?id=${accountReference.id}`)
    }
  }

  delete(): any {

  }


  async navigateToAccountReferenceDetail(accountReference: AccountReference) {
    await this.router.navigateByUrl(`/accounting/accountReferences/add?id=${accountReference.id}`)
  }

  close(): any {
  }


}
