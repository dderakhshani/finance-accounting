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
import { BaseValueModel } from '../../../entities/base-value';
import { MatDialog } from '@angular/material/dialog';
import { FormControl, FormGroup } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetAllReceiptItemsCommodityQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-items-commodity-query';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { Commodity } from '../../../../accounting/entities/commodity';
import { Warehouse } from '../../../entities/warehouse';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';
import { ErpDarkhastJoinDocumentHeadsQuery } from '../../../repositories/reports/get-erpDarkhast-join-documentHeads';
import { spErpDarkhastJoinDocumentHeads } from '../../../entities/spErpDarkhastJoinDocumentHeads';



@Component({
  selector: 'app-leave-erp-join-receipt-list',
  templateUrl: './leave-erp-join-receipt-list.component.html',
  styleUrls: ['./leave-erp-join-receipt-list.component.scss'],
  providers: [DatePipe]
})
export class LeaveErpJoinReceiptListComponent extends BaseComponent {

 
  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;


  totalQuantity: number = 0;
  Receipts: spErpDarkhastJoinDocumentHeads[] = [];
  ReceiptBaseValue: BaseValueModel[] = [];



  SearchForm = new FormGroup({
    requestNo: new FormControl(),
    documentNo: new FormControl(),
    warehouseId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
   
  });



  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    public datepipe: DatePipe,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
  ) {
    super();
  }

  async ngOnInit() {

  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {
    await this.ApiCallService.getReceiptAllStatus('');

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
        'documentNo',
        'شماره حواله',

        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),

      new TableColumn(
        'requestDate',
        'تاریخ حواله',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('requestDate', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'statusTitle',
        'وضعیت',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('statusTitle', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)

      ),

      colbuttonVisibility,
     
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------

    await this.get();

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

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

    if (this.SearchForm.controls.requestNo.value != undefined && this.SearchForm.controls.requestNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requestNo",
        values: [this.SearchForm.controls.requestNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.documentNo.value != undefined && this.SearchForm.controls.documentNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentNo",
        values: [this.SearchForm.controls.documentNo.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
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

    let request = new ErpDarkhastJoinDocumentHeadsQuery(
      
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.requestNo.value,
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    

    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }
  
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره حواله</th>
                       <th>تاریخ حواله</th>
                       <th>وضعیت</th>
                       <th>انبار</th>
                       
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.requestDate}</td>
                           <td>${data.statusTitle}</td>
                           <td>${data.warehouseTitle}</td>
                          
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'حواله های خروج از انبار')
    }
  }
  
 
  async navigateToReceipt(Receipt: any) {
    if (Receipt.documentNo != undefined) {
      await this.router.navigateByUrl(`inventory/receipt-operations/leavingPartWarehouse?requestNumber=${Receipt.requestNo}&documentNo=${Receipt.documentNo}&WarehouseId=${Receipt.warehouseId}`)
    }
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      await this.router.navigateByUrl(`inventory/receipt-operations/leavingPartWarehouse?requestNumber=${Receipt.requestNo}&WarehouseId=${this.SearchForm.controls.warehouseId.value}`)
    }
    else {
      this.Service.showHttpFailMessage('انبار مورد نظر را انتخاب نمایید');
      return;
    }
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
