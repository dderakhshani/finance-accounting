import { Component } from '@angular/core';
import { MatDialog, MatDialogConfig } from "@angular/material/dialog";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { PageModes } from "../../../../../core/enums/page-modes";
import { FormAction } from "../../../../../core/models/form-action";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { ExtensionsService } from "../../../../../shared/services/extensions/extensions.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { CodeVoucherGroup } from "../../../entities/code-voucher-group";
import {
  DeleteCodeVoucherGroupCommand
} from "../../../repositories/code-voucher-group/command/delete-code-voucher-group-command";
import {
  GetCodeVoucherGroupsQuery
} from "../../../repositories/code-voucher-group/queries/get-code-voucher-groups-query";
import { CodeVoucherGroupDialogComponent } from "./code-voucher-group-dialog/code-voucher-group-dialog.component";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-code-voucher-group',
  templateUrl: './code-voucher-group.component.html',
  styleUrls: ['./code-voucher-group.component.scss']
})
export class CodeVoucherGroupComponent extends BaseComponent {
  entities: CodeVoucherGroup[] = [];
  tableConfigurations!: TableConfigurations;

  listActions: FormAction[] = [
    FormActionTypes.add,
    FormActionTypes.edit,
    FormActionTypes.refresh,
    FormActionTypes.delete
  ]


  constructor(private _mediator: Mediator,
              public extensionsService: ExtensionsService,
              public dialog: MatDialog) {
    super();
  }
  ngAfterViewInit() {
    this.actionBar.actions=[
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete(),

    ]
  }

  async ngOnInit() {
    await this.resolve();
  }

  async resolve() {
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2%'),
      new TableColumn(
        'code',
        'کد',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('code', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'uniqueName',
        'نام یکتا',
        TableColumnDataType.Text,
        '2%',
        true,
        new TableColumnFilter('uniqueName', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'title',
        'عنوان گروه سند',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'lastEditableDate',
        'قابل تغییر تا تاریخ',
        TableColumnDataType.Date,
        '2.5%',
        true,
        new TableColumnFilter('lastEditableDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'isActive',
        'فعال',
        TableColumnDataType.CheckBox,
        '2.5%',
        true,
        new TableColumnFilter('isActive', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isAuto',
        'اتوماتیک',
        TableColumnDataType.CheckBox,
        '2.5%',
        true,
        new TableColumnFilter('isAuto', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isEditable',
        'قابل ویرایش',
        TableColumnDataType.CheckBox,
        '2.5%',
        true,
        new TableColumnFilter('isEditable', TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'disabled',
        'فرمول جایگزینی در حالت خالی بودن',
        TableColumnDataType.Text,
        '3.5%',
        true,
        new TableColumnFilter('disabled', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'extendType',
        'نوع افزونه',
        TableColumnDataType.Text,
        '2.5%',
        true,
        new TableColumnFilter('extendType', TableColumnFilterTypes.Text)
      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));


    await this.get();
  }

  initialize(): any {
  }

  async get(id?: number) {
    this.entities = [];
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

    let request = new GetCodeVoucherGroupsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
  }


  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CodeVoucherGroupDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.entities.push(response)
        this.entities = [...this.entities]
      }
    })
  }

  async update(entity?: CodeVoucherGroup) {
// @ts-ignore
    if (this.entities.filter(x => x.selected)[0] || entity) {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        // @ts-ignore
        entity: this.entities.filter(x => x.selected)[0] ?? entity,
        pageMode: PageModes.Update
      };

      let dialogReference = this.dialog.open(CodeVoucherGroupDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({response, pageMode}) => {
        if (response) {
          if (pageMode === PageModes.Update) {
            let codeVoucherGroup = this.entities.find(x => x.id === response.id)
            if (codeVoucherGroup) {
              Object.keys(response).forEach((key: string) => {
                // @ts-ignore
                codeVoucherGroup[key] = response[key]
              })
            }
          } else if (pageMode === PageModes.Delete) {
            let CodeVoucherGroupToRemove = this.entities.find(x => x.id === response.id);
            if (CodeVoucherGroupToRemove) {
              this.entities.splice(this.entities.indexOf(CodeVoucherGroupToRemove), 1);
              this.entities = [...this.entities]
            }
          }
        }
      })
    }
  }


  async delete() {
    // @ts-ignore
    for (const codeVoucherGroup of this.entities.filter(x => x.selected)) {
      await this._mediator.send(new DeleteCodeVoucherGroupCommand(codeVoucherGroup.id)).then(res => {
        this.entities.splice(this.entities.indexOf(codeVoucherGroup), 1);
        this.entities = [...this.entities]
      });
    }

  }


  close(): any {
  }

}
