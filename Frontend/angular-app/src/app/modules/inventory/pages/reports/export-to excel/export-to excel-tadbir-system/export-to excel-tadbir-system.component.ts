import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { FormControl, FormGroup } from '@angular/forms';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import * as XLSX from 'xlsx';
import { GeTadbirExportQuery } from '../../../../repositories/reports/get-export-tadbir-reports-query';
import { spGetDocumentItemForTadbir } from '../../../../entities/spGetDocumentItemForTadbir';
import { Warehouse } from '../../../../entities/warehouse';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
@Component({
  selector: 'app-export-to excel-tadbir-system',
  templateUrl: './export-to excel-tadbir-system.component.html',
  styleUrls: ['./export-to excel-tadbir-system.component.scss']
})
export class ExportToExcelTadbirSystemComponent extends BaseComponent {

  IslargeSize: boolean = false;

  responce: spGetDocumentItemForTadbir[] = [];
  Reports_filter: spGetDocumentItemForTadbir[] = [];
  ReceiptAllStatus: ReceiptAllStatusModel[] = [];

  SearchForm = new FormGroup({
    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),
    warehouseId: new FormControl(),
    documentNo: new FormControl(),
    codeVoucherGroupId: new FormControl(),
  });
  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,
    public ApiCallService: ApiCallService,
  ) {
    super(route, router);

  }
  async ngOnInit() {
    await this.resolve()
  }

  async resolve(params?: any) {

    await this.initialize()
  }


  async initialize(entity?: any) {

    await this.getReceiptAllStatus();

  }

  //------------------------------------------------------
  async get() {

    await this._mediator.send(new GeTadbirExportQuery(
      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.SearchForm.controls.codeVoucherGroupId.value,
      this.SearchForm.controls.documentNo.value,
      this.SearchForm.controls.warehouseId.value,

    )).then(async (res) => {
      this.responce = res;
      this.Reports_filter = res;
    })

  }
  ChangePage(pageMumber: number[]) {
    this.responce = this.Reports_filter.filter(function (d, ix) { return pageMumber.indexOf(ix) >= 0; });
  }
  exportexcel(): void {
    var fileName = 'ExcelSheet.xlsx';
    /* pass here the table id */

    let data = this.Reports_filter.map((x: any) => {
      let y: any = {};
      y['شماره حواله'] = x.documentNo
      y['تاریخ'] = x.date
      y['نوع سند'] = x.documentType
      y['کد کالا'] = x.commodityCode
      y['تعداد'] = x.quantity
      y['کد انبار'] = x.tadbirCode
      y['کد تحویل گیرنده'] = x.tahvilCode
      y['کد حساب درخواست کننده'] = x.darkhastCode
      y['کد شناور درخواست کننده'] = x.shenavarCode
      y['قیمت'] = x.price

      return y
    })

    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(data);

    /* generate workbook and add the worksheet */
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

    /* save to file */
    XLSX.writeFile(wb, fileName);

  }
  WarehouseIdSelect(item: Warehouse) {

    this.SearchForm.controls.warehouseId.setValue(item?.id);

  }
  async getReceiptAllStatus() {

    await this.ApiCallService.getReceiptAllInventoryStatus().then(res => {

      this.ReceiptAllStatus = res;
    });

  }
  codeVoucherGroupSelect(item: any) {

    this.SearchForm.controls.codeVoucherGroupId.setValue(item);

  }
  async reset() {

  }


  async update() {

  }
  delete() {


  }
  async edit() {
  }
  close(): any {
  }
  add(param?: any) {

  }




}
