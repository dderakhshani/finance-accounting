import {Component} from '@angular/core';
import {CodeRowDescription} from "../../../entities/code-row-description";
import {CodeRowDescriptionDialogComponent} from "./code-row-description-dialog/code-row-description-dialog.component";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {ExtensionsService} from "../../../../../shared/services/extensions/extensions.service";
import {Router} from "@angular/router";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {
  GetCodeRowDescriptionsQuery
} from "../../../repositories/code-row-description/queries/code-row-descriptions-query";
import {PageModes} from "../../../../../core/enums/page-modes";
import {FormControl} from "@angular/forms";
import {Year} from "../../../../admin/entities/year";
import {
  DeleteCodeRowDescriptionCommand
} from "../../../repositories/code-row-description/commands/delete-code-row-description-command";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-code-row-description',
  templateUrl: './code-row-description.component.html',
  styleUrls: ['./code-row-description.component.scss']
})
export class CodeRowDescriptionComponent extends BaseComponent {

  entities: CodeRowDescription[] = [];
  tableConfigurations!: TableConfigurations;

  listActions: FormAction[] = [
    FormActionTypes.add,
    FormActionTypes.edit,
    FormActionTypes.refresh,
    FormActionTypes.delete
  ]

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
      PreDefinedActions.delete(),    ]
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
        '5%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text)
      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));


    await this.get();
  }

  initialize(): any {
  }

  async get(id?: number) {
    this.entities =[];
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

    let request = new GetCodeRowDescriptionsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    response.totalCount ? this.tableConfigurations.pagination.totalItems = response.totalCount : '';
  }

  async add() {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      pageMode: PageModes.Add
    };

    let dialogReference = this.dialog.open(CodeRowDescriptionDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({response, pageMode}) => {
      if (response && pageMode === PageModes.Add) {
        this.entities.push(response)
        this.entities = [...this.entities]
      }
    })
  }

  async update(entity?:CodeRowDescription) {
// @ts-ignore
    if (entity || this.entities.filter(x => x.selected)[0]) {
      let dialogConfig = new MatDialogConfig();
      dialogConfig.data = {
        // @ts-ignore
        entity: entity ?? this.entities.filter(x => x.selected)[0],
        pageMode: PageModes.Update
      };

      let dialogReference = this.dialog.open(CodeRowDescriptionDialogComponent, dialogConfig);

      dialogReference.afterClosed().subscribe(({response, pageMode}) => {
        if (response) {
          if (pageMode === PageModes.Update) {
            let codeRowDescription = this.entities.find(x => x.id === response.id)
            if (codeRowDescription) {
              Object.keys(response).forEach((key: string) => {
                // @ts-ignore
                codeRowDescription[key] = response[key]
              })
            }
          } else if (pageMode === PageModes.Delete) {
            let codeRowDescriptionToRemove = this.entities.find(x => x.id === response.id);
            if (codeRowDescriptionToRemove) {
              this.entities.splice(this.entities.indexOf(codeRowDescriptionToRemove), 1);
              this.entities = [...this.entities]
            }
          }
        }
      })
    }
  }


  async delete() {
    // @ts-ignore
    this.entities.filter(x => x.selected).forEach(async (codeRowDescription) => {
      await this._mediator.send(new DeleteCodeRowDescriptionCommand(codeRowDescription.id)).then(res => {
        this.entities.splice(this.entities.indexOf(codeRowDescription),1);
        this.entities = [...this.entities]
      });
    })

  }


  close(): any {
  }
}
