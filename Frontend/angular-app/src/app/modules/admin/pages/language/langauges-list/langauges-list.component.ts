import { Component, OnInit } from '@angular/core';
import {Language} from "../../../entities/language";
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import { Router } from '@angular/router';
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetLanguagesQuery} from "../../../repositories/languages/queries/get-languages-query";
import {DeleteLanguageCommand} from "../../../repositories/languages/commands/delete-language-command";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-langauges-list',
  templateUrl: './langauges-list.component.html',
  styleUrls: ['./langauges-list.component.scss']
})
export class LangaugesListComponent extends BaseComponent {


  languages: Language[] = [];
  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [
    FormActionTypes.add,
    FormActionTypes.edit,
    FormActionTypes.refresh,
    FormActionTypes.delete

  ]

  constructor(
    private _mediator: Mediator,
    private router: Router,
  ) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.add(),
      PreDefinedActions.edit(),
      PreDefinedActions.refresh(),


    ]

  }


  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {
    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'title',
        'عنوان زبان',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('title', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'seoCode',
        'کد اختصاصی',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('seoCode', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'defaultCurrencyBaseTitle',
        'واحد پولی',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('defaultCurrencyBaseTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'img',
        'پرچم',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('img', TableColumnFilterTypes.Text)
      ),
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }

  initialize() {
  }

  async get(id?: number) {
    this.languages = []
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

    let request = new GetLanguagesQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.languages = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async add() {
    await this.router.navigateByUrl(`admin/languages/add`)
  }

  async update() {
    // @ts-ignore
    let language = this.languages.filter(x => x.selected)[0]
    if (language) {
      await this.navigateToLanguage(language)
    }
  }

  async navigateToLanguage(language: Language) {
    await this.router.navigateByUrl(`admin/languages/add?id=${language.id}`)
  }

  close(): any {
  }

//   async delete() {
//     // @ts-ignore
//     let languageToDelete = this.languages.filter(x => x.selected)[0]
//     if (languageToDelete)     await this._mediator.send(new DeleteLanguageCommand(languageToDelete.id))
//
//   }
// }
  async delete() {
    // @ts-ignore
    this.entities.filter(x => x.selected).forEach(async (language) => {
      await this._mediator.send(new DeleteLanguageCommand(language.id)).then(res => {
        this.entities.splice(this.entities.indexOf(language), 1);
        this.entities = [...this.entities]
      });
    })
  }
}
