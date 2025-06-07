import {Component} from '@angular/core';
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {GetVoucherHeadsQuery} from "../../repositories/voucher-head/queries/get-voucher-heads-query";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {TableConfigurations} from "../../../../core/components/custom/table/models/table-configurations";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {RequestLog} from "../../repositories/logs/request-log";
import {ToPersianDatePipe} from "../../../../core/pipes/to-persian-date.pipe";
import {
  Action,
  ActionTypes,
  PreDefinedActions
} from "../../../../core/components/custom/action-bar/action-bar.component";
import {GetApplicationRequestLogsQuery} from "../../repositories/logs/get-events-query";

@Component({
  selector: 'app-application-request-logs',
  templateUrl: './application-request-logs.component.html',
  styleUrls: ['./application-request-logs.component.scss']
})
export class ApplicationRequestLogsComponent extends BaseComponent {
  tableConfigurations!: TableConfigurations;

  constructor(
    private _mediator: Mediator
  ) {
    super()
  }

  ngAfterViewInit() {
    this.actionBar.actions = [
      PreDefinedActions.refresh(),
      <Action>{
        title: 'کپی Request',
        color: 'primary',
        type: ActionTypes.custom,
        uniqueName: 'copyRequest',
        disabled: false,
        icon: 'content_copy'
      },
      <Action>{
        title: 'کپی Response',
        color: 'primary',
        type: ActionTypes.custom,
        uniqueName: 'copyResponse',
        disabled: false,
        icon: 'content_copy'
      }
    ]
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    let tableColumns = [
      new TableColumn('selected', '', TableColumnDataType.Select, ''),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, ''),


      new TableColumn(
        'requestType',
        'نوع',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('requestType', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'status',
        'وضعیت',
        TableColumnDataType.Number,
        '',
        true,
        new TableColumnFilter('status', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'createdBy',
        'ارسال کننده',
        TableColumnDataType.Text,
        '',
        true,
        new TableColumnFilter('createdBy', TableColumnFilterTypes.Text),
      ),

      new TableColumn(
        'createdAt',
        'تاریخ',
        TableColumnDataType.Date,
        '',
        true,
        new TableColumnFilter('createdAt', TableColumnFilterTypes.Date),
        (log: RequestLog) => {
          return new ToPersianDatePipe().transform(log.createdAt, true)
        }
      ),
    ];
    this.tableConfigurations = new TableConfigurations(tableColumns, new TableOptions(false, true));

    return await this.get();
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

    if (!orderByProperty) orderByProperty = 'createdAt DESC'
    let request = new GetApplicationRequestLogsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.entities = response.data;
    if (response.totalCount) this.tableConfigurations.pagination.totalItems = response.totalCount;
  }


  async handleCustomClick(action: Action) {
    let selectedRow = this.entities.find(x => x.selected === true)
    if (action.uniqueName == 'copyRequest') {
      await navigator.clipboard.writeText(selectedRow.requestJSON);
    }
    if (action.uniqueName == 'copyResponse') {
      await navigator.clipboard.writeText(selectedRow.responseJSON);
    }
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
