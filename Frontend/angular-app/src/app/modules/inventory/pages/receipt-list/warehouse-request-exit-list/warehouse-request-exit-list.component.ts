import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";

import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { MatDialog } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";
import { WarehouseRequestExitView } from '../../../entities/warehouseRequestExit';
import { GetWarehouseRequestExitViewQuery } from '../../../repositories/reports/get-warehouse-request-exit';



@Component({
  selector: 'app-warehouse-request-exit-list',
  templateUrl: './warehouse-request-exit-list.component.html',
  styleUrls: ['./warehouse-request-exit-list.component.scss'],
  providers: [DatePipe]
})
export class WarehouseRequestExitListComponent extends BaseComponent {

  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;
  Receipts: WarehouseRequestExitView[] = [];

  SearchForm = new FormGroup({

    requestNo: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });



  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService
  ) {
    super();
  }

  async ngOnInit() {

  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {
    let colbuttonVisibility = new TableColumn(
      'buttonVisibility',
      'مشاهده',
      TableColumnDataType.Template,
      '5%',
      false

    );
    colbuttonVisibility.template = this.buttonVisibility;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'requestNo',
        'شماره درخواست ',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'statusTitle',
        'وضعیت',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('statusTitle', TableColumnFilterTypes.Text)

      ),
      
      new TableColumn(
        'documentNo',
        'شماره حواله',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),

      new TableColumn(
        'documentDate',
        'تاریخ حواله',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),
      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)
      ),
      
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '15%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'requestQuantity',
        'تعداد کل درخواست',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('requestQuantity', TableColumnFilterTypes.Number)
      ),
     
      new TableColumn(
        'exitQuantity',
        'تعداد حواله',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('exitQuantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'remainedQuantity',
        'تعداد مانده',
        TableColumnDataType.Number,
        '8%',
        true,
        new TableColumnFilter('remainedQuantity', TableColumnFilterTypes.Number)
      ),
      colbuttonVisibility
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();

  }

  initialize() {
  }

  onSearch() {
    if (this.SearchForm.valid) {
      this.get();
    }
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

    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value!="") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }


    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    var param: number[] = [];
    param.push(Number(this.Service.CodeArchiveReceipt));

    let request = new GetWarehouseRequestExitViewQuery(
                                      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
                                      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
                                      this.tableConfigurations.pagination.pageIndex + 1,
                                      this.tableConfigurations.pagination.pageSize,
                                      searchQueries)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  async navigateToReceipt(item: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${item.id}&displayPage=archive`)
  }

  async update() {
  }

  async add() {

  }
  close(): any {
  }

  delete(): any {

  }



}
