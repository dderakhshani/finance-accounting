import { Component } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { TableConfigurations } from "../../../../../core/components/custom/table/models/table-configurations";
import { Router } from "@angular/router";
import { PreDefinedActions } from "../../../../../core/components/custom/action-bar/action-bar.component";

import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableOptions } from 'src/app/core/components/custom/table/models/table-options';

import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { Help } from "../../../entities/help";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { GetHelpsQuery } from "../../../repositories/help/queries/get-helps-query";
import { DeleteHelpCommand } from "../../../repositories/help/command/delete-help-command";

@Component({
  selector: 'app-helps-list',
  templateUrl: './helps-list.component.html',
  styleUrls: ['./helps-list.component.scss']
})
export class HelpsListComponent extends BaseComponent {
  helps: Help[] = []

  tableConfigurations!: TableConfigurations;

  constructor(
    private _mediator: Mediator,
    private router: Router
  ) {
    super()
    this.isLoading = true;
  }
ngAfterViewInit() {
  this.actionBar.actions=[
    PreDefinedActions.refresh(),
    PreDefinedActions.add().setTitle("افزودن راهنما"),
    PreDefinedActions.edit().setTitle("ویرایش راهنما"),
    PreDefinedActions.delete().setTitle("حذف راهنما")
  ]
}

  async ngOnInit(){
    await this.resolve()
  }

  async resolve(){
    let tableColumns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'menuTitle',
        'عنوان صفحه',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('menuTitle', TableColumnFilterTypes.Text)
      )
    ]
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, false))

    await this.get()
  }

  initialize(){
  }

  async get(id?: number) {
    if (!this.isLoading) this.isLoading = true

    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters){
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery({
          propertyName: filter.columnName,
          values: [filter.searchValue],
          comparison: filter.searchCondition
        }))
      })
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key , index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetHelpsQuery(this.tableConfigurations.pagination.pageIndex + 1,
          this.tableConfigurations.pagination.pageSize,
          searchQueries, orderByProperty,true)
    let response = await this._mediator.send(request);
    this.helps = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

    this.isLoading = false
  }

  async add() {
    await this.router.navigateByUrl("/admin/helps/add")
  }

  async update(entity?: any) {
    //@ts-ignore
    let help = this.helps.filter(x => x.selected)[0];
    if (help){
      await this.router.navigateByUrl(`/admin/helps/add?id=${help.id}`)
    }
  }

  async navigateToHelpEditor(help: any) {
    await this.router.navigateByUrl(`/admin/helps/add?id=${help.id}`)
  }

  async delete() {
    //@ts-ignore
    let help = this.helps.filter(x => x.selected)[0];
    let request = new DeleteHelpCommand(help.id)
    await this._mediator.send(request);
  }

  close(): any {
  }
}
