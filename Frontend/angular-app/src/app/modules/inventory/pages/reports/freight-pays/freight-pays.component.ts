import { Component, HostListener, TemplateRef, ViewChild } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";

import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { FormControl, FormGroup } from '@angular/forms';
import { Warehouse } from '../../../entities/warehouse';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Commodity } from '../../../../accounting/entities/commodity';
import { GetReceiptsQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-query';
import { Receipt } from '../../../entities/receipt';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { DocumentState } from '../../../entities/documentState';
import { GetRecepitInvoiceQuery } from '../../../repositories/reports/get-receipt-invoice-query';
import { BaseSetting } from '../../../entities/base';
import * as XLSX from 'xlsx';
import { GetFreightPaysQuery } from '../../../repositories/reports/get-freight-pays-query';
@Component({
  selector: 'app-freight-pays',
  templateUrl: './freight-pays.component.html',
  styleUrls: ['./freight-pays.component.scss']
})
export class FreightPaysComponent extends BaseSetting {



  ReceiptAllStatus: ReceiptAllStatusModel[] = [];

  creditSumAll: number = 0;
  debitSumAll: number = 0;

  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({

    creditAccountReferenceId: new FormControl(),
    creditAccountReferenceGroupId: new FormControl(),
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });
  ToggleForPrint: boolean = false;
  listActions: FormAction[] = [

    FormActionTypes.refresh,

  ]

  constructor(
    private router: Router,
    public _mediator: Mediator,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    public Service: PagesCommonService,
    public ApiCallService: ApiCallService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
  }

  async ngOnInit() {

    this.resolve();
  }


  async ngAfterViewInit() {

  }
  async resolve() {


  }


  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);

    await this._mediator.send(new GetFreightPaysQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.creditAccountReferenceId.value,
      this.currentPage,
      this.gePageSize(),
      searchQueries)).then(res => {


        if (this.currentPage != 0) {
          this.data = res.data;
          this.Reports_filter = res.data;
          this.CalculateSum();
          this.RowsCount = res.totalCount;
        }
        else {

          this.exportexcel(res.data);
        }

      })
  }
  selectedRows(event: any) {
    this.CalculateSum(event);
  }
  CalculateSum(isSelectedRows:boolean =false) {


    this.creditSumAll = 0;
    this.debitSumAll = 0;

      if (isSelectedRows) {
        this.Service.calculateLengthTableOrRows().then(
          res => {
            this.dataLength = res;
          }
        )
        //credit
        this.Service.calculateSumTableOrRows('credit').then(
          res => {
            this.creditSumAll = res;

          }

        )
        //debit
        this.Service.calculateSumTableOrRows('debit').then(
          res => {
            this.debitSumAll = res;

          }
        )
      }
      else {
        this.data.forEach(a => this.creditSumAll += Number(a.credit ? a.credit : 0));

        this.data.forEach(a => this.debitSumAll += Number(a.debit? a.debit : 0));
        this.dataLength = this.data.length;
      }



  }
  //--------------------Filter Table need thoes methods-
  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }

  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);
    var title = `صورتحساب رانندگان :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, title)
    }
  }

  getCommodityById(item: Commodity) {

    this.SearchForm.controls.commodityId.setValue(item?.id);

  }
  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?documentId=${item.documentId}&displayPage=none`)

  }
  async navigateToVoucher(Receipt: Receipt) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${Receipt.voucherHeadId}`)
  }

  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }


}
