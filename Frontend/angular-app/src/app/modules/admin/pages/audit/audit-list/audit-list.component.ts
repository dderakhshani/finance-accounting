import {Component} from '@angular/core';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ExtensionsService} from "../../../../../shared/services/extensions/extensions.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {Audit} from '../../../entities/audit';
import {GetAuditQuery} from "../../../repositories/audit/queries/get-audit-query";
import {AuditDialogComponent} from "./audit-dialog/audit-dialog.component";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {PageModes} from "../../../../../core/enums/page-modes";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-audit-list',
  templateUrl: './audit-list.component.html',
  styleUrls: ['./audit-list.component.scss']
})
export class AuditListComponent extends BaseComponent {
  entities: Audit[] = [];
  tableConfigurations!: TableConfigurations;



  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]

  constructor(private _mediator: Mediator,
              public extensionsService: ExtensionsService,
              public dialog: MatDialog) {
    super();
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
           PreDefinedActions.refresh(),
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2%'),
      new TableColumn(
        'dateTime',
        'تاریخ',
        TableColumnDataType.Date,
        '2.5%',
        true,
        new TableColumnFilter('dateTime', TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        'menuTitle',
        'فرم',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('menuTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'userId',
        'کد کاربری',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('userId', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'primaryId',
        'شماره سند',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('primaryId', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'type',
        'نوع عملیات',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('type', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'tableName',
        'نام جدول',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('tableName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'subSystem',
        'سیستم',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('subSystem', TableColumnFilterTypes.Text)
      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));
    await this.get();
  }

  async get(id?: number) {
    this.entities = [];
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
          comparison: filter.searchCondition
        }))
      })
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetAuditQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
  }


  async navigateToAudit(entity:Audit) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      // @ts-ignore
      entity: entity,
    };

    let dialogReference = this.dialog.open(AuditDialogComponent, dialogConfig);


  }

  add(param?: any): any {
  }

  close(): any {
  }

  delete(param?: any): any {
  }


  initialize(params?: any): any {
  }


  update(param?: any): any {
  }

}
