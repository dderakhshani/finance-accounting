import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Receipt } from "../../../entities/receipt";
import { ActivatedRoute, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';
import { TableConfigurations } from '../../../../../core/components/custom/table/models/table-configurations';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';


import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { DocumentState } from '../../../entities/documentState';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { TableColumnFilter } from "../../../../../core/components/custom/table/models/table-column-filter";
import { TableColumnDataType } from "../../../../../core/components/custom/table/models/table-column-data-type";
import { TableColumnFilterTypes } from "../../../../../core/components/custom/table/models/table-column-filter-types";
import { TableOptions } from "../../../../../core/components/custom/table/models/table-options";
import { TableColumn } from "../../../../../core/components/custom/table/models/table-column";
import { ConfirmDialogComponent, ConfirmDialogIcons } from '../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';

import { DocumentHeadExtraCostDialogComponent } from '../../component/documents-extra-cost-dialog/documents-extra-cost-dialog.component';
import { GetTemporaryRecepitQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { GetTotalExtraCostQuery } from '../../../repositories/documentHeadExtraCost/queries/get-total-extera-Costs-query';
import { UpdateExtraCostCommand } from '../../../repositories/receipt/commands/reciept/update-temporary-receipt-command';


@Component({
  selector: 'app-receipt-for-add-extera-cost-list',
  templateUrl: './receipt-for-add-extera-cost-list.component.html',
  styleUrls: ['./receipt-for-add-extera-cost-list.component.scss']
})
export class ReceiptForAddExteraCostListComponent extends BaseComponent {

  @ViewChild('txtCommodity', { read: TemplateRef }) txtCommodity!: TemplateRef<any>;
  @ViewChild('txtDocumentId', { read: TemplateRef }) txtDocumentId!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;
  @ViewChild('txtCurrencyPrice', { read: TemplateRef }) txtCurrencyPrice!: TemplateRef<any>;
  @ViewChild('txtdebitReferenceTitle', { read: TemplateRef }) txtdebitReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtcreditReferenceTitle', { read: TemplateRef }) txtcreditReferenceTitle!: TemplateRef<any>;
  @ViewChild('txtCodeVoucherGroupTitle', { read: TemplateRef }) txtCodeVoucherGroupTitle!: TemplateRef<any>;


  Receipts: Receipt[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  tableConfigurations!: TableConfigurations;



  SearchForm = new FormGroup({

    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
  ]


  constructor(
    private router: Router,
    public dialog: MatDialog,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnInit() {

  }
  async ngAfterViewInit() {

    await this.resolve()
  }
  async resolve() {

    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '10%',
      false

    )
    let colSelected = new TableColumn(
      'selected',
      'انتخاب',
      TableColumnDataType.Template,
      '5%',
      false
    );

    let colCommodityTitle = new TableColumn(
      'commodityTitle',
      '    کالا  ',
      TableColumnDataType.Template,
      '24%',
      true,
      new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
    );
    let colCodeVoucherGroupTitle = new TableColumn(
      'codeVoucherGroupTitle',
      'نوع رسید',
      TableColumnDataType.Template,
      '5%',
      true,
      new TableColumnFilter('codeVoucherGroupTitle', TableColumnFilterTypes.Text)
    );
    let colcreditreferenceTitle = new TableColumn(
      'creditReferenceTitle',
      'حساب بستانکار',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
    );
    let colredebitferenceTitle = new TableColumn(
      'debitReferenceTitle',
      'حساب بدهکار',
      TableColumnDataType.Template,
      '15%',
      true,
      new TableColumnFilter('debitReferenceTitle', TableColumnFilterTypes.Text)
    );

    let colExtraCost = new TableColumn(
      'extraCost',
      'کرایه حمل',
      TableColumnDataType.Money,
      '10%',
      true,
      new TableColumnFilter('extraCost', TableColumnFilterTypes.Money)
    )



    colTagArray.template = this.buttonTagArray;
    colCommodityTitle.template = this.txtCommodity;
    colredebitferenceTitle.template = this.txtdebitReferenceTitle;
    colcreditreferenceTitle.template = this.txtcreditReferenceTitle;
    colCodeVoucherGroupTitle.template = this.txtCodeVoucherGroupTitle;


    //--------------------------------------------------------------------------
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
        'شماره رسید',
        TableColumnDataType.Number,
        '10%',
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

      colCommodityTitle,
      colredebitferenceTitle,
      colcreditreferenceTitle,

    ]


    columns.push(new TableColumn(
      'isFreightChargePaid',
      'کرایه حمل برعهده ماست؟',
      TableColumnDataType.CheckBox,
      '10%',
      true,
      new TableColumnFilter('isFreightChargePaid', TableColumnFilterTypes.Money)
    ))
    columns.push(colExtraCost)
    columns.push(colCodeVoucherGroupTitle)

    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();

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




    if (this.SearchForm.controls.creditAccountReferenceId.value != undefined && this.SearchForm.controls.creditAccountReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountReferenceId",
        values: [this.SearchForm.controls.creditAccountReferenceId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.creditAccountReferenceGroupId.value != undefined && this.SearchForm.controls.creditAccountReferenceGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "creditAccountReferenceGroupId",
        values: [this.SearchForm.controls.creditAccountReferenceGroupId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.debitAccountReferenceId.value != undefined && this.SearchForm.controls.debitAccountReferenceId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountReferenceId",
        values: [this.SearchForm.controls.debitAccountReferenceId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }
    if (this.SearchForm.controls.debitAccountReferenceGroupId.value != undefined && this.SearchForm.controls.debitAccountReferenceGroupId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "debitAccountReferenceGroupId",
        values: [this.SearchForm.controls.debitAccountReferenceGroupId.value],
        comparison: 'equal',
        nextOperand: "and"
      }))
    }




    searchQueries.push(new SearchQuery({
      propertyName: "documentStauseBaseValue",
      values: [DocumentState.Direct, DocumentState.invoiceAmount, DocumentState.registrationAccounting],
      comparison: "in",
      nextOperand: "and "
    }))

    searchQueries.push(new SearchQuery({
      propertyName: "isImportPurchase",
      values: [false],
      comparison: "equal",
      nextOperand: "and"
    }))


    searchQueries.push(new SearchQuery({
      propertyName: "codeVoucherGroupId",
      values: [2322, 2334, 2330],
      comparison: "in",
      nextOperand: "and"
    }))


    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }

    let request = new GetReceiptsQuery(0,
      false,
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Receipts = response.data;
    this.tableConfigurations.options.showSumRow = true;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

    this.tableConfigurations.options.sumLabel = 'جمع کل';
    this.tableConfigurations.options.showSumRow = true;

  }


  async navigateToReceipt(Receipt: Receipt) {
    if (Receipt.documentStauseBaseValue != DocumentState.Direct) {
      await this.router.navigateByUrl(`inventory/rialsReceiptDetails?id=${Receipt.id}&isImportPurchase=false&editType=4`)
    }
    else {
      this.openDocumentHeadExtraCostDialog(Receipt);
    }

  }
  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item?.id);

  }
  getListIds(receipt: any): number[] {

    var listIds: number[] = [];
    listIds.push(Number(receipt.id))

    return listIds;
  }
  async openDocumentHeadExtraCostDialog(item: Receipt) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      documentHeadIds: this.getListIds(item)
    };

    let dialogReference = this.dialog.open(DocumentHeadExtraCostDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      if (response) {
         this.onAddExteraCost(item);

      }
    })

  }
   onAddExteraCost(receipt: any) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'یادآوری',
        message: 'هزینه ها به قیمت تمام شده اضافه شود؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async res => {

      let entity = await this.getReceipt(receipt.id)
      entity.extraCost = Number(await this.getExteraCostRial(receipt));
      entity.isFreightChargePaid = true;
      this.request = new UpdateExtraCostCommand().mapFrom(entity);
      let response = await this._mediator.send(<UpdateExtraCostCommand>this.request);

    });
     this.get();

  }
  async getExteraCostRial(item: Receipt) {


      let request = new GetTotalExtraCostQuery();
      request.listIds = this.getListIds(item);

      let cost = await this._mediator.send(request);
    return cost;
  }
  async getReceipt(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
  }
  async print() {

    let printContents = '';
    if (this.Receipts.length > 0) {

      printContents += `<table><thead>
                     <tr>
                       <th>شماره درخواست</th>
                       <th>شماره رسید</th>
                       <th>تاریخ رسید</th>
                       <th>تامین کننده</th>
                       <th>عنوان کالا</th>
                       <th>تعداد</th>
                       <th>مبلغ کرایه</th>
                     </tr>
                   </thead><tbody>`;
      this.Receipts.map(data => {
        printContents += `<tr>
                           <td>${data.requestNo}</td>
                           <td>${data.documentNo}</td>
                           <td>${this.Service.toPersianDate(data.documentDate)}</td>
                           <td>${data.creditReferenceTitle}</td>
                           <td>${data.commodityTitle}</td>
                           <td>${data.quantity}</td>
                           <td>${data.extraCost}</td>
                        </tr>`;
      })
      printContents += '</tbody></table>'
      this.Service.onPrint(printContents, 'رسیدهای مستقیم')
    }
  }


  //--------------------------------------------------------------
  async update() {

  }

  async add() {

  }



  close(): any {
  }

  delete(): any {
  }


}
