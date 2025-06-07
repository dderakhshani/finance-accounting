import { ActivatedRoute, Router } from "@angular/router";
import { MatDialog } from '@angular/material/dialog';
import { Receipt } from '../../../entities/receipt';
import { Warehouse } from '../../../entities/warehouse';
import { FormControl, FormGroup } from '@angular/forms';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Commodity } from '../../../../commodity/entities/commodity';
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableConfigurations } from '../../../../../core/components/custom/table/models/table-configurations';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { GetAllReceiptItemsCommodityQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-items-commodity-query';
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { DomSanitizer } from "@angular/platform-browser";



@Component({
  selector: 'app-reques-buy-made-list',
  templateUrl: './reques-buy-made-list.component.html',
  styleUrls: ['./reques-buy-made-list.component.scss']
})

export class requesBuyMadeListComponent extends BaseComponent {


  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;

  totalInput: number = 0;
  totalQuantity: number = this.getQueryParam('totalQuantity');
  Receipts: Receipt[] = [];
  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]
  constructor(
    public _mediator: Mediator
    , private router: Router
    , private route: ActivatedRoute
    , private sanitizer: DomSanitizer
    , public Service: PagesCommonService
    , public _notificationService: NotificationService,

  ) {
    super(route, router);
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

    let columns: TableColumn[] = [
      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'serialNumber',
        'سریال',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('serialNumber', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'documentNo',
        'شماره',
        TableColumnDataType.Number,
        '10%',
        true

      ),
      new TableColumn(
        'documentDate',
        'تاریخ سند',
        TableColumnDataType.Date,
        '10%',
        true,
        new TableColumnFilter('documentDate', TableColumnFilterTypes.Date)
      ),

      new TableColumn(
        'requesterReferenceTitle',
        'درخواست دهنده',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('requesterReferenceTitle', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'commodityCode',
        'کد کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Money,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Money)
      ),
      new TableColumn(
        'codeVoucherGroupTitle',
        'نوع سند',
        TableColumnDataType.Text,
        '8%',
        true,
        new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
      ),

      colbuttonVisibility

    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();


  }

  async initialize() {

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

    searchQueries.push(new SearchQuery({
      propertyName: "requestNo",
      values: [this.getQueryParam('documentNo')],
      comparison: "equal",
      nextOperand: "and"
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "warehouseId",
      values: [this.getQueryParam('warehouseId')],
      comparison: "equal",
      nextOperand: "and"
    }))
    searchQueries.push(new SearchQuery({
      propertyName: "commodityCode",
      values: [this.getQueryParam('commodityCode')],
      comparison: "equal",
      nextOperand: "and"
    }))

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }


    let request = new GetAllReceiptItemsCommodityQuery(
      0,
      undefined,
      undefined,
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    this.totalInput = 0;
    this.Receipts.forEach(a => this.totalInput += Number(a.quantity));
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }



  async navigateToArchive(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receiptDetails?id=${Receipt.id}&displayPage=archive`)
  }



  async print() {

    let printContents = '';
    let documentNo = this.getQueryParam('documentNo');
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره سند</th>
                       <th>تاریخ سند</th>
                       <th>درخواست دهنده</th>
                       <th>پیگیری کننده</th>
                       <th>عنوان کالا</th>
                       <th>کد کالا</th>
                       <th>تعداد</th>

                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.requesterReferenceTitle}</td>
                           <td>${data.followUpReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.commodityCode}</td>
                           <td>${data.quantity}</td>

                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, `تعداد ورودی درخواست ${documentNo}`)
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
