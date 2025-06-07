import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from "@angular/router";

import {
  TableConfigurations
} from "../../../../../core/components/custom/table/models/table-configurations";
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { FormAction } from '../../../../../core/models/form-action';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { FormControl, FormGroup } from '@angular/forms';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { Receipt } from '../../../entities/receipt';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { GetRecepitInvoiceQuery } from '../../../repositories/reports/get-receipt-invoice-query';
import { BaseSetting } from '../../../entities/base';
@Component({
  selector: 'app-receipts-invoice',
  templateUrl: './receipts-invoice.component.html',
  styleUrls: ['./receipts-invoice.component.scss']
})
export class ReceiptsInvoiceComponent extends BaseSetting {



  ReceiptAllStatus: ReceiptAllStatusModel[] = [];
  totalItemUnitPrice: number = 0;
  sumAll: number = 0;


  tableConfigurations!: TableConfigurations;
  SearchForm = new FormGroup({
    invoceNo: new FormControl(),
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

    await this._mediator.send(new GetRecepitInvoiceQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.invoceNo.value,
      this.SearchForm.controls.creditAccountReferenceId.value,
      this.currentPage,
      this.gePageSize(),
      searchQueries)).then(res => {


        if (this.currentPage != 0) {
          this.data = res;
          this.Reports_filter = res;
          this.CalculateSum();
          this.RowsCount = Number(res[0]?.rowsCount)
        }
        else {

          this.exportexcel(res);
        }

      })
  }
  selectedRows(event: any) {
    this.CalculateSum(event);
  }
  CalculateSum( isSelectedRows:boolean =false ) {
    this.totalItemUnitPrice = 0;

    this.sumAll = 0;
    if (isSelectedRows) {
      //  dataLength
      this.Service.calculateLengthTableOrRows().then(
        res => {
          this.dataLength = res;
        }
      )
      //totalItemUnitPrice
      this.Service.calculateSumTableOrRows('totalItemPrice').then(
        res => {
          this.totalItemUnitPrice = res;

        }
      )
      //sumAll
      this.Service.calculateSumTableOrRows('totalProductionCost').then(
        res => {
          this.sumAll = res;
        }
      )

    }else {



      this.data.forEach(a => this.totalItemUnitPrice += Number(a.totalItemPrice));

      this.data.forEach(a => this.sumAll += Number(a.totalProductionCost));
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
    var title = `گزارش صورتحساب های خرید:${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, title)
    }
  }


  async navigateToRecive(item: any) {

    await this.router.navigateByUrl(`inventory/receiptDetails?documentId=${item.documentId}&displayPage=none`)

  }
  async navigateToVoucher(Receipt: Receipt) {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${Receipt.voucherHeadId}`)
  }

  debitReferenceSelect(item: any) {

    this.SearchForm.controls.debitAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.SearchForm.controls.creditAccountReferenceId.setValue(item?.id);
    this.SearchForm.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }


}
