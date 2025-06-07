import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from "../../../../core/abstraction/base.component";
import {CorrectionRequest} from "../../../admin/entities/correction-request";
import {TableConfigurations} from "../../../../core/components/custom/table/models/table-configurations";
import {FormControl} from "@angular/forms";
import {Mediator} from "../../../../core/services/mediator/mediator.service";
import {IdentityService} from "../../../identity/repositories/identity.service";
import {Router} from "@angular/router";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {SearchQuery} from "../../../../shared/services/search/models/search-query";
import {
  GetAllCorrectionRequestsQuery
} from "../../../admin/repositories/correction-request/queries/get-all-correction-requests-query";
import {PreDefinedActions} from "../../../../core/components/custom/action-bar/action-bar.component";
import {
  SubmitCorrectionRequestCommand
} from "../../../admin/repositories/correction-request/commands/submit-correction-request-command";
import {MatTabChangeEvent} from "@angular/material/tabs";

@Component({
  selector: 'app-created-correction-requests',
  templateUrl: './created-correction-requests.component.html',
  styleUrls: ['./created-correction-requests.component.scss']
})
export class CreatedCorrectionRequestsComponent extends BaseComponent {
  selectedTabIndex: number = 0;
  entities: CorrectionRequest[] = [];
  createdRecords: CorrectionRequest[] = [];
  tableConfigurations!: TableConfigurations;
  userId!: number;


  statuses = [
    {
      title: 'در انتظار واکنش',
      value: 0
    },
    {
      title: 'تایید شده',
      value: 1
    },
    {
      title: 'رد شده',
      value: 2
    },
    {
      title: 'خطا',
      value: 4
    },
  ]

  statusFormControl = new FormControl(0);

  constructor(
    private mediator: Mediator,
    private identityService: IdentityService,
    private router: Router
  ) {
    super();
  }

  ngAfterViewInit() {
    this.preDefinedActionsBar();
  }

  async ngOnInit() {
    await this.resolve();
    await this.getUser();
  }

  async getUser() {
    this.identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.userId = res.id;
      }
    });
  }


  async resolve() {



    let columns: TableColumn[] = [
      new TableColumn('selected', '', TableColumnDataType.Select, '2.5%'),
      new TableColumn('index', 'ردیف', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'description',
        'توضیحات',
        TableColumnDataType.Text,
        '40%',
        true,
        new TableColumnFilter('description', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'documentId',
        'شماره رسید',
        TableColumnDataType.Number,
        '10%',
        true,
        new TableColumnFilter('documentId', TableColumnFilterTypes.Number),
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'createdAt',
        'تاریخ ایجاد',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('createdAt', TableColumnFilterTypes.Date),
      ),
      new TableColumn(
        'createdUserName',
        'ایجاد کننده',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('createdUserName', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'verifierUserName',
        'مسئول تایید/رد',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('verifierUserName', TableColumnFilterTypes.Text),
      ),
      new TableColumn(
        'modifiedAt',
        'تاریخ تایید/رد',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('modifiedAt', TableColumnFilterTypes.Date),
      ),
      new TableColumn(
        'status',
        'وضعیت درخواست',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('status', TableColumnFilterTypes.Text),
        (cr: CorrectionRequest) => {
          return cr.status == 1 ? "تایید شده" : (cr.status == 2 ? "رد شده" : "در انتظار واکنش")
        }
      )
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    await this.get();
  }



  async get() {

    let searchQueries: SearchQuery[] = []

    if (this.userId) {

      searchQueries.push(new SearchQuery({
        propertyName: 'createdById',
        values: [Number(this.userId)],
        comparison: 'equal',
        nextOperand: 'and'
      }))

    if (this.tableConfigurations.filters) {
      searchQueries.push(new SearchQuery({
        propertyName: 'status',
        values: [this.statusFormControl.value],
        comparison: 'equal',
        nextOperand: 'and'
      }))

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
    this.isLoading = true;
    let request = new GetAllCorrectionRequestsQuery(this.tableConfigurations.pagination.pageIndex + 1, this.tableConfigurations.pagination.pageSize, searchQueries, orderByProperty)
    let response = await this.mediator.send(request);
    this.createdRecords = response.data;

    this.tableConfigurations.pagination.totalItems = response.totalCount;
    this.isLoading = false;
    }
  }


  async navigateToRequestPage(request: CorrectionRequest) {
    await this.router.navigateByUrl(request.viewUrl + `?id=${request.documentId}&tableId=${request.id}`);
  }

  async submitCorrectionRequest(isAccepted: boolean) {
    // @ts-ignore
    let selectedCorrections = this.entities.filter(x => x.selected === true);
    for (let cr of selectedCorrections) {
      await this.mediator.send(new SubmitCorrectionRequestCommand(cr.id, isAccepted))
    }
    return await this.get();
  }
  preDefinedActionsBar() {
    setTimeout(() => {
      if (this.actionBar) {
        this.actionBar.actions = [
          PreDefinedActions.delete().setTitle('رد').setIcon('cancel'),

          PreDefinedActions.refresh()
        ];
      }
    }, 0);
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
