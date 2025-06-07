import {Component} from '@angular/core';
import {FormAction} from "../../../../../core/models/form-action";
import {FormActionTypes} from "../../../../../core/constants/form-action-types";
import {Year} from "../../../entities/year";
import {Router} from "@angular/router";
import {ExtensionsService} from "../../../../../shared/services/extensions/extensions.service";
import {BaseComponent} from "../../../../../core/abstraction/base.component";
import {Mediator} from "../../../../../core/services/mediator/mediator.service";
import {SearchQuery} from "../../../../../shared/services/search/models/search-query";
import {GetYearsQuery} from "../../../repositories/year/queris/get-years-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import {PreDefinedActions} from "../../../../../core/components/custom/action-bar/action-bar.component";
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";

@Component({
  selector: 'app-year-list',
  templateUrl: './year-list.component.html',
  styleUrls: ['./year-list.component.scss']
})
export class YearListComponent extends BaseComponent {
  years: Year[] = [];

  tableConfigurations!: TableConfigurations;

  constructor(private router: Router,
              private _mediator: Mediator,
              public extensionsService: ExtensionsService) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions =[
      PreDefinedActions.edit(),
      PreDefinedActions.add(),
      PreDefinedActions.refresh(),
      PreDefinedActions.delete()
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
        '2.5%'
      ),
      new TableColumn(
        'index',
        'ردیف',
        TableColumnDataType.Index,
        '2.5%'
      ),
      new TableColumn(
        'yearName',
        'نام سال مالی جدید',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('yearName',TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'firstDate',
        'تاریخ شروع سال مالی',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('firstDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'lastDate',
        'تاریخ پایان سال مالی',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('lastDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'lastEditableDate',
        'تاریخ انقضا قابلیت ویرایش',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('lastEditableDate',TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'isEditable',
        'قابل ویرایش',
        TableColumnDataType.CheckBox,
        '10%',
        true,
        new TableColumnFilter('isEditable',TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isCalculable',
        'قابل محاسبه',
        TableColumnDataType.CheckBox,
        '10%',
        true,
        new TableColumnFilter('isCalculable',TableColumnFilterTypes.CheckBox)
      ),
      new TableColumn(
        'isCurrentYear',
        'سال مالی جاری',
        TableColumnDataType.CheckBox,
        '10%',
        true,
        new TableColumnFilter('isCurrentYear',TableColumnFilterTypes.CheckBox)
      ),

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }

  initialize(): any {
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

    let request = new GetYearsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.years = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async add() {
    await this.router.navigateByUrl(`admin/year/add`)
  }

  async update() {
    // @ts-ignore
    let year = this.years.filter(x => x.selected)[0];
    if (year) {
     await this.navigateToYear(year)
    }
  }

  async navigateToYear(year: Year) {
    await this.router.navigateByUrl(`admin/year/add?id=${year.id}`)
  }

  close(): any {
  }

  delete(): any {
  }

}
