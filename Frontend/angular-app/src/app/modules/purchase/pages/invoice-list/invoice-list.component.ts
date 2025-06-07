import { Component, TemplateRef, ViewChild } from '@angular/core';
import { invoice } from "../../entities/invoice";
import { Router } from "@angular/router";
import { FormAction } from "../../../../core/models/form-action";
import { FormActionTypes } from "../../../../core/constants/form-action-types";
import { BaseComponent } from "../../../../core/abstraction/base.component";
import { Mediator } from "../../../../core/services/mediator/mediator.service";

import { SearchQuery } from "../../../../shared/services/search/models/search-query";
import {
  TableConfigurations

} from "../../../../core/components/custom/table/models/table-configurations";


import { InvoiceAllStatusModel } from '../../entities/invoice-all-status';

import { FormControl, FormGroup } from '@angular/forms';
import { LoaderService } from '../../../../core/services/loader.service';

import { GetInvoiceALLStatusQuery } from '../../repositories/invoice/queries/invoice/get-invoice-all-status-query';
import { GetInvoicesQuery } from '../../repositories/invoice/queries/invoice/get-invoices-query';
import { NotificationService } from '../../../../shared/services/notification/notification.service';
import {TableColumnFilter} from "../../../../core/components/custom/table/models/table-column-filter";
import {TableColumnDataType} from "../../../../core/components/custom/table/models/table-column-data-type";
import {TableColumnFilterTypes} from "../../../../core/components/custom/table/models/table-column-filter-types";
import {TableOptions} from "../../../../core/components/custom/table/models/table-options";
import {TableColumn} from "../../../../core/components/custom/table/models/table-column";
import { PagesCommonService } from '../../../../shared/services/pages/pages-common.service';


@Component({
  selector: 'app-invoice-list',
  templateUrl: './invoice-list.component.html',
  styleUrls: ['./invoice-list.component.scss']
})
export class InvoiceListComponent extends BaseComponent {

  @ViewChild('buttonVisibility', { read: TemplateRef }) buttonVisibility!: TemplateRef<any>;

  Invoices: invoice[] = [];
  InvoiceAllStatus:InvoiceAllStatusModel[]=[];
  InvoiceAllStatusCode: string = '2270';
  tableConfigurations!: TableConfigurations;

  SearchForm = new FormGroup({
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [
    FormActionTypes.refresh,
    FormActionTypes.print,

  ]

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
  ) {
    super();
  }

  async ngOnInit() {
    await this.resolve()
  }

  async resolve() {

    await this.getInvoiceAllStatus();

    let colbuttonVisibility = new TableColumn(
      'buttonVisibility',
      'مشاهده',
      TableColumnDataType.Template,
      '5%',
      false

    );
    colbuttonVisibility.template = this.buttonVisibility;
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
        'شماره صورتحساب',
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
        '20%',
        true,
        new TableColumnFilter('totalItemPrice', TableColumnFilterTypes.Number)
      ),
      new TableColumn(
        'documentDescription',
        'توضیحات',
        TableColumnDataType.Text,
        '10%',
        true,
        new TableColumnFilter('documentDescription', TableColumnFilterTypes.Text)
      ),
      colbuttonVisibility
    ]
    this.tableConfigurations = new TableConfigurations(columns, new TableOptions(false, true))
    //--------------------------------------------------
    await this.get();

  }

  initialize() {
  }

  async get(codeVoucherGroupId?: string) {
    let searchQueries: SearchQuery[] = []
    if (this.tableConfigurations.filters) {
      this.tableConfigurations.filters.forEach(filter => {
        searchQueries.push(new SearchQuery(
          {
            propertyName: filter.columnName,
            values: filter.multipleSearchValues.length > 0 ? filter.multipleSearchValues : [filter.searchValue],
            comparison: filter.searchCondition
          }
        ))
      })
    }


    let orderByProperty = '';
    if (this.tableConfigurations.sortKeys) {
      this.tableConfigurations.sortKeys.forEach((key, index) => {
        orderByProperty += index ? `,${key}` : key
      })
    }


    let request = new GetInvoicesQuery(Number(codeVoucherGroupId),
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
  async getInvoiceAllStatus() {

    let request = new GetInvoiceALLStatusQuery()
    let response = await this._mediator.send(request);
    this.InvoiceAllStatus = response.data;


  }
  async navigateToInvoice(Invoice: invoice) {
    await this.router.navigateByUrl(`purchase/invoiceDetails?id=${Invoice.id}`)
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
