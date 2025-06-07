import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom//table/models/table-configurations";
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';
import { FormControl, FormGroup } from '@angular/forms';

import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import { ArchiveReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/archive-temporary-receipt-command';
import { MatDialog } from '@angular/material/dialog';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { UpdateIsPlacementCompleteCommand } from '../../../repositories/receipt/commands/reciept/update-IsPlacementComplete-receipt';
import { GetReceiptALLBaseValueQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-all-base-value-query';
import { BaseValueModel } from '../../../entities/base-value';
import { GetRequesterReferenceWarhouse } from '../../../repositories/personal/get-requester-reference-query';
import { AccountReference } from '../../../../accounting/entities/account-reference';
import { Warehouse } from '../../../entities/warehouse';



@Component({
  selector: 'app-direct-receipt-list',
  templateUrl: './direct-receipt-list.component.html',
  styleUrls: ['./direct-receipt-list.component.scss']
})
export class DirectReceiptListComponent extends BaseComponent {

  @ViewChild('buttonIsPlacementComplete', { read: TemplateRef }) buttonIsPlacementComplete!: TemplateRef<any>;

  @ViewChild('buttonMore', { read: TemplateRef }) buttonMore!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  ReceiptBaseValue: BaseValueModel[] = [];
  accountReferences: AccountReference[] = [];
  tableConfigurations!: TableConfigurations;
  panelOpenState = true;


  SearchForm = new FormGroup({
    requestNo: new FormControl(),
    documentNo: new FormControl(),
    warehouseId: new FormControl(),
    documentStateBaseId: new FormControl(),
    accountReferencesId: new FormControl(),
    isPlacementComplete: new FormControl(true),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
  });

  listActions: FormAction[] = [
    FormActionTypes.refresh

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public dialog: MatDialog,
    public _notificationService: NotificationService,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService
  ) {
    super();
  }
  async ngAfterViewInit() {
    await this.resolve()
  }
  async ngOnInit() {

    await this.ReferenceFilter('');
  }

  async resolve() {
    await this.ApiCallService.getReceiptAllStatus(this.Service.CodeDirectReceipt.toString());

    let colIsPlacementComplete = new TableColumn(
      'isPlacementComplete',
      'جایگذاری شده',
      TableColumnDataType.Template,
      '10%',
      true,
      new TableColumnFilter('isPlacementComplete', TableColumnFilterTypes.CheckBox)
    )
    let colMore = new TableColumn(
      'colMore',
      'بیشتر',
      TableColumnDataType.Template,
      '10%',
      false
    );
    colMore.template = this.buttonMore;

    colIsPlacementComplete.template = this.buttonIsPlacementComplete;
    let columns: TableColumn[] = [

      new TableColumn('index', '', TableColumnDataType.Index, '2.5%'),
      new TableColumn(
        'selected',
        'انتخاب',
        TableColumnDataType.Select,
        '5%',
        true,
        new TableColumnFilter('selected', TableColumnFilterTypes.Select)

      ),
      new TableColumn(
        'serialNumber',
        'سریال',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('serialNumber', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'requestNo',
        'شماره درخواست',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('requestNo', TableColumnFilterTypes.Text)

      ),
      new TableColumn(
        'documentNo',
        'شماره رسید',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Number)

      ),
      new TableColumn(
        'documentDate',
        'تاریخ رسید',
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
        'creditReferenceTitle',
        'تامین کننده',
        TableColumnDataType.Text,
        '20%',
        true,
        new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
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
        '20%',
        true,
        new TableColumnFilter('commodityCode', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'measureTitle',
        'واحد کالا',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('measureTitle', TableColumnFilterTypes.Text)
      ),

      new TableColumn(
        'quantity',
        'تعداد',
        TableColumnDataType.Number,
        '5%',
        true,
        new TableColumnFilter('quantity', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'documentStateBaseTitle',
        'وضعیت',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('documentStateBaseTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'warehouseTitle',
        'انبار',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('warehouseTitle', TableColumnFilterTypes.Text)
      ),
      colIsPlacementComplete,
      colMore,
      /*colRirect,*/
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();
    await this.getReceiptBaseValue();
  }

  initialize() {
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
    if (this.SearchForm.controls.isPlacementComplete.value != undefined) {
      searchQueries.push(new SearchQuery({
        propertyName: "isPlacementComplete",
        values: [this.SearchForm.controls.isPlacementComplete.value],
        comparison: "equal",
        nextOperand: "and"
      }))
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
    if (this.SearchForm.controls.documentStateBaseId.value != undefined && this.SearchForm.controls.documentStateBaseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "documentStateBaseId",
        values: [this.SearchForm.controls.documentStateBaseId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.accountReferencesId.value != undefined && this.SearchForm.controls.accountReferencesId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "requesterReferenceId",
        values: [this.SearchForm.controls.accountReferencesId.value],
        comparison: 'equal',
        nextOperand: 'and'
      }))
    }
    if (this.SearchForm.controls.warehouseId.value != undefined && this.SearchForm.controls.warehouseId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "warehouseId",
        values: [this.SearchForm.controls.warehouseId.value],
        comparison: 'equal',
        nextOperand: 'and'
      }))
    }

    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }



    let request = new GetReceiptsQuery(Number(this.Service.CodeDirectReceipt),
      undefined,
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  async Edit(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceipt?id=${Receipt.id}`)
  }
  async navigateToReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/receipt-operations/placementWarehouse?id=${Receipt.id}&warehouseId=${Receipt.warehouseId}`)
  }
  async navigateToReturnReceipt(Receipt: Receipt) {
    await this.router.navigateByUrl(`inventory/request-operations/addRequestReturnCommodity?documentNo=${Receipt.documentNo}&warehouseId=${Receipt.warehouseId}`)
  }
  WarehouseIdSelect(item: Warehouse) {
    this.SearchForm.controls.warehouseId.setValue(item?.id);
  }
  async ReferenceFilter(searchTerm: string) {
    var filter: any = undefined;
    if (searchTerm != '') {
      filter = [
        new SearchQuery({
          propertyName: 'code',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'or'
        }),
        new SearchQuery({
          propertyName: 'title',
          comparison: 'contains',
          values: [searchTerm],
          nextOperand: 'or'
        }),

      ]
    }

    await this._mediator.send(new GetRequesterReferenceWarhouse(
      this.Service.CodeDirectReceipt,
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      0, 25, filter)).then(res => {

        this.accountReferences = res.data
      })

  }
  //-----------------Print--------------------------------------------------------
  async ConfirmPageTemporaryReceiptALL() {
    await this.printList();

    if (this.Receipts.filter(a => a.selected).length== 0) {


      this.Service.showHttpFailMessage('لیست رسیدهای چاپی را انتخاب نمایید')
    }
    else if (this.SearchForm.controls.accountReferencesId.value == undefined) {


      this.Service.showHttpFailMessage('درخواست دهنده مورد نظر را انتخاب کنید')
    }
    else {
      await this.router.navigateByUrl(`inventory/receipt-operations/temporaryReceiptPrint?accountReferencesId=${this.SearchForm.controls.accountReferencesId.value}`);
    }
  }
  async printList() {

    var responce = this.Receipts.filter(a => a.selected)
    if (responce.length > 0) {

      this.Service.ListId = [];
      responce.forEach(async res => {
        if (res.selected) {
          this.Service.ListId.push(res.id.toString());
        }

      })

    }
  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره رسید</th>
                       <th>شماره صورتحساب</th>
                       <th>تاریخ رسید</th>
                       <th>درخواست دهنده</th>
                       <th>تامین کننده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>وضعیت</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${data.invoiceNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.requesterReferenceTitle}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.documentStateBaseTitle}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای مستقیم')
    }
  }

  async archive(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید حذف / بایگانی',
        message: `آیا مطمئن به بایگانی این شماره رسید ` + Receipt.documentNo + ` می باشید؟`,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(Receipt.id));
        this.Receipts = this.Receipts.filter(a => a.id != Receipt.id);
      }
    });

  }
  async NotConfirm(Receipt: Receipt) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'تایید مرجوعی و خروج',
        message: `این  شماره رسید  ` + Receipt.documentNo + ` بدون صدور فاکتور فروش مرجوع می گردد و مورد تایید جهت مرجوعی می باشد`,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async result => {
      if (result == true) {

        let response = await this._mediator.send(new ArchiveReceiptCommand(Receipt.id));
        this.Receipts = this.Receipts.filter(a => a.id != Receipt.id);
      }
    });

  }
  async onIsPlacementComplete(item: Receipt, ob: MatCheckboxChange) {
    item.isPlacementComplete = ob.checked;

    var request_ = new UpdateIsPlacementCompleteCommand();
    request_.id = item.id;
    request_.isPlacementComplete = item.isPlacementComplete;

    await this._mediator.send(<UpdateIsPlacementCompleteCommand>request_).then(res => {
    });

  }
  //------------بدست آوردن حالت های مختلف تایید یک رسید--------------------
  async getReceiptBaseValue() {
    let request = new GetReceiptALLBaseValueQuery()
    let response = await this._mediator.send(request);
    this.ReceiptBaseValue = response.data;
  }
  ReferenceSelect(item: any) {

    this.SearchForm.controls.accountReferencesId.setValue(item);
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
