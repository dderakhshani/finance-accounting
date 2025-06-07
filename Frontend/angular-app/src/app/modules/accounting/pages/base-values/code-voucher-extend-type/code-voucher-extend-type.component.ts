import {Component} from '@angular/core';
import {CodeVoucherExtendType} from "../../../entities/code-voucher-extend-type";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {
  CodeVoucherExtendTypeDialogComponent
} from "./code-voucher-extend-type-dialog/code-voucher-extend-type-dialog.component";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ExtensionsService} from "../../../../../shared/services/extensions/extensions.service";
import {Router} from "@angular/router";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {PageModes} from "../../../../../core/enums/page-modes";
import {
  GetCodeVoucherExtendTypesQuery
} from "../../../repositories/code-voucher-extend-type/queries/get-code-voucher-extend-types-query";
import {
  DeleteCodeVoucherExtendTypeCommand
} from "../../../repositories/code-voucher-extend-type/commands/delete-code-voucher-extend-type-command";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-code-voucher-exception-type',
  templateUrl: './code-voucher-extend-type.component.html',
  styleUrls: ['./code-voucher-extend-type.component.scss']
})
export class CodeVoucherExtendTypeComponent extends BaseComponent {
  entities: CodeVoucherExtendType[] = [];
  tableConfigurations!: TableConfigurations;
  //
  // listActions: FormAction[] = [
  //   FormActionTypes.add,
  //   FormActionTypes.edit,
  //   FormActionTypes.refresh,
  //   FormActionTypes.delete
  // ]

  constructor(private _mediator: Mediator,
              public extensionsService: ExtensionsService,
              public dialog: MatDialog,
              private router: Router) {
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
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'title',
        'عنوان',
        TableColumnDataType.Text,
        '75%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text),
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

    let request = new GetCodeVoucherExtendTypesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
  }


  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CodeVoucherExtendTypeDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.entities.push(response)
        this.entities = [...this.entities]
      }
    })
  }

  async update(codeVoucherExtendType?: CodeVoucherExtendType) {
// @ts-ignore
    if (this.entities.filter(x => x.selected)[0] || codeVoucherExtendType) {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        // @ts-ignore
        entity: this.entities.filter(x => x.selected)[0] ?? codeVoucherExtendType,
        pageMode: PageModes.Update
      };

      let dialogReference = this.dialog.open(CodeVoucherExtendTypeDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({response, pageMode}) => {
        if (response) {
          if (pageMode === PageModes.Update) {
            let CodeVoucherExtendType = this.entities.find(x => x.id === response.id)
            if (CodeVoucherExtendType) {
              Object.keys(response).forEach((key: string) => {
                // @ts-ignore
                CodeVoucherExtendType[key] = response[key]
              })
            }
          } else if (pageMode === PageModes.Delete) {
            let CodeVoucherExtendTypeToRemove = this.entities.find(x => x.id === response.id);
            if (CodeVoucherExtendTypeToRemove) {
              this.entities.splice(this.entities.indexOf(CodeVoucherExtendTypeToRemove), 1);
              this.entities = [...this.entities]
            }
          }
        }
      })
    }
  }


  async delete() {
    // @ts-ignore
    this.entities.filter(x => x.selected).forEach(async (CodeVoucherExtendType) => {
      await this._mediator.send(new DeleteCodeVoucherExtendTypeCommand(CodeVoucherExtendType.id)).then(res => {
        this.entities.splice(this.entities.indexOf(CodeVoucherExtendType), 1);
        this.entities = [...this.entities]
      });
    })

  }

  close(): any {
  }

}
