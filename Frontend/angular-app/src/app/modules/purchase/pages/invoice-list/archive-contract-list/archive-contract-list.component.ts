import { Component, TemplateRef, ViewChild } from '@angular/core';
import { invoice } from "../../../entities/invoice";
import { Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";

import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { AccountReference } from '../../../../accounting/entities/account-reference';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent, ConfirmDialogIcons } from 'src/app/core/components/material-design/confirm-dialog/confirm-dialog.component';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { LoaderService } from '../../../../../core/services/loader.service';

import { InvoiceAllStatusModel } from '../../../entities/invoice-all-status';
import { BaseValueModel } from '../../../../inventory/entities/base-value';
import { GetInvoicesQuery } from '../../../repositories/invoice/queries/invoice/get-invoices-query';
import { GetInvoiceALLStatusQuery } from '../../../repositories/invoice/queries/invoice/get-invoice-all-status-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';

import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../../core/components/custom/table/models/table-column";


@Component({
  selector: 'app-archive-contract-list',
  templateUrl: './archive-contract-list.component.html',
  styleUrls: ['./archive-contract-list.component.scss'],
  providers: [DatePipe]
})
export class ArchiveContractListComponent extends BaseComponent {

  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;
  @ViewChild('buttonTagArray', { read: TemplateRef }) buttonTagArray!: TemplateRef<any>;

  Invoices: invoice[] = [];

  InvoiceBaseValue: BaseValueModel[] = [];
  accountReferences: AccountReference[] = [];
  InvoiceAllStatusUnicCode: string = 'ArchiveContract';
  SearchForm = new FormGroup({
   fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    accountReferencesId: new FormControl(),
    invoiceNo: new FormControl(),
  });



  tableConfigurations!: TableConfigurations;


  listActions: FormAction[] = [

    FormActionTypes.refresh,
    FormActionTypes.print
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
    // await this.resolve()
  }

  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve() {
    await this.ApiCallService.getInvoiceAllStatus();

    let colbuttonVisibility = new TableColumn(
      'buttonVisibility',
      'مشاهده',
      TableColumnDataType.Template,
      '5%',
      false

    );
    let colTagArray = new TableColumn(
      'buttonTagArray',
      'برچسب',
      TableColumnDataType.Template,
      '5%',
      false

    );
    colTagArray.template = this.buttonTagArray;
    colbuttonVisibility.template = this.buttonVisibility;
    //----------------------------------------------------------------------
    let columns: TableColumn[] = [


      new TableColumn(
        'documentNo',
        'شماره',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Date)

      ),
      new TableColumn(
        'invoiceNo',
        'شماره قرارداد',
        TableColumnDataType.Text,
        '5%',
        true,
        new TableColumnFilter('documentNo', TableColumnFilterTypes.Date)

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
        'commodityTitle',
        'عنوان کالا',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('commodityTitle', TableColumnFilterTypes.Text)
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
        'creditReferenceTitle',
        'تامین کننده',
        TableColumnDataType.Text,
        '25%',
        true,
        new TableColumnFilter('creditReferenceTitle', TableColumnFilterTypes.Text)
      ),
      new TableColumn(
        'totalItemPrice',
        'مبلغ کل',
        TableColumnDataType.Money,
        '10%',
        true,
        new TableColumnFilter('totalItemPrice', TableColumnFilterTypes.Number)
      ),

      colTagArray,
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
    if (this.SearchForm.controls.accountReferencesId.value != undefined && this.SearchForm.controls.accountReferencesId.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "referenceId",
        values: [this.SearchForm.controls.accountReferencesId.value],
        comparison: "equal",
        nextOperand: "and"
      }))
    }

    if (this.SearchForm.controls.invoiceNo.value != undefined && this.SearchForm.controls.invoiceNo.value != "") {
      searchQueries.push(new SearchQuery({
        propertyName: "invoiceNo",
        values: [this.SearchForm.controls.invoiceNo.value],
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

    let id = this.ApiCallService.InvoiceStatus.find(a => a.uniqueName == this.Service.ArchiveContract)?.id

    let request = new GetInvoicesQuery(Number(id),
      undefined,
     new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.tableConfigurations.pagination.pageIndex + 1,
      this.tableConfigurations.pagination.pageSize,
      searchQueries, orderByProperty)
    let response = await this._mediator.send(request);
    this.Invoices = response.data;
    response.totalCount && (this.tableConfigurations.pagination.totalItems = response.totalCount);

  }


  ReferenceSelect(id: number) {
    this.SearchForm.controls.accountReferencesId.setValue(id);


  }

  async navigateToInvoice(Invoice: invoice) {
    await this.router.navigateByUrl(`purchase/invoiceDetails?id=${Invoice.id}`)
  }

  close(): any {
  }

  delete(): any {

  }
  async update() {


  }

  async add() {

  }


}
