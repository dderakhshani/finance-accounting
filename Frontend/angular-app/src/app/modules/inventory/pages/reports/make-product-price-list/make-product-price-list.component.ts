import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Data, Router } from "@angular/router";
import { FormAction } from "../../../../../core/models/form-action";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { SearchQuery } from "../../../../../shared/services/search/models/search-query";
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { BaseSetting } from '../../../entities/base';
import { GetMakeProductPriceQuery } from '../../../repositories/reports/get-make-product-price-reports';
import { MakeProductPrice, MakeProductPriceForDocument } from '../../../entities/makeProductPrice';
import { CreateMakeProductPriceCommand } from '../../../repositories/receipt/commands/reciept/create-make-product-price-reports';
import { CreateAndUpdateAutoVoucher2Command } from '../../../repositories/mechanized-document/commands/convert-to-mechanized-document';




@Component({
  selector: 'app-make-product-price-list',
  templateUrl: './make-product-price-list.component.html',
  styleUrls: ['./make-product-price-list.component.scss']
})
export class MakeProductPriceComponent extends BaseSetting {

  MakeProductPrice: MakeProductPrice | undefined = undefined;
  SearchForm = new FormGroup({

    fromDate: new FormControl(new Date(this.Service.identityService.getActiveYearStartDate())),
    toDate: new FormControl(new Date(this.Service.identityService.getActiveYearlastDate())),

  });

  listActions: FormAction[] = [
    FormActionTypes.refresh

  ]

  constructor(
    private router: Router,
    public _mediator: Mediator,
    public dialog: MatDialog,
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


    await this.get();

    await this.initialize()
  }


  async initialize() {


  }

  async get(searchQueries: SearchQuery[] = []) {

    searchQueries = this.GetChildFilter(searchQueries);
    let request = new GetMakeProductPriceQuery(

      new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0)),
      new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1)),
      this.currentPage,
      this.pageSize,

      searchQueries)
    await this._mediator.send(request).then(res => {


      if (this.currentPage != 0) {
        this.MakeProductPrice = res.data[0];
        this.request = new CreateMakeProductPriceCommand().mapFrom(this.MakeProductPrice);
        this.data = this.MakeProductPrice.makeProductPriceReport;

        this.Reports_filter = this.MakeProductPrice.makeProductPriceReport;
        this.CalculateSum();
        if (this.currentPage == 1) {
          this.RowsCount = res.totalCount;

        }
      }
      else {

        this.exportexcel(res.data);
      }

    })

  }
  selectedRows(event: any) {
    this.isDisplayFooter = !event
  }

  CalculateSum() {


  }

  filterTable(data: any) {

    this.data = data
    this.CalculateSum();
  }

  async print() {

    let printContents = await this.Service.GetMasTableHtmlAndData(this.Reports_filter);
    var title = `گزارش تولید محصول از تاریخ :${this.Service.toPersianDate(this.SearchForm.controls.fromDate.value)} تا تاریخ ${this.Service.toPersianDate(this.SearchForm.controls.toDate.value)}`
    if (this.data.length > 0) {

      this.Service.onPrint(printContents, title)
    }
  }

  async exportexcel(data: any[]) {

    this.Service.onExportToExcel(data)

  }
  async add() {

    var requestVoucher2 = new CreateAndUpdateAutoVoucher2Command();
    var dataList: MakeProductPriceForDocument[] = []



    let from = new Date(new Date(<Date>(this.SearchForm.controls.fromDate.value)).setHours(0, 0, 0, 0));
    let to = new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(24, 0, 0, -1));
    let DocumentDate = new Date(new Date(<Date>(this.SearchForm.controls.toDate.value)).setHours(0, 0, 0, 0));
    let dd = this.MakeProductPrice?.lastDate;
    //if (dd != undefined) {
    //  const totalDays = (dd.getDate()+dd. + - from.getTime()) ;
    //  if (totalDays != 1) {
    //    this.Service.showHttpFailMessage('تاریخ شروع برای ثبت سند حتما باید یک روز پس از تاریخ آخرین سند ثبت شده باشد' + totalDays.toString());
    //    return;
    //  }

    //}




    this.data.forEach(a => {

      dataList.push({
        DocumentDate: DocumentDate.toUTCString(),
        CodeVoucherGroupId: '2447',
        CodeVoucherGroupTitle: "سند مکانیزه قیمت تمام شده محصول",
        DateFrom: from.toUTCString(),
        DateTo: to.toUTCString(),
        Title: 'سرامیک سایز ' + a.size + ' ضخامت ' + a.thickness,
        TotalCost: Math.round(a.total).toString(),
        TotalSalery: Math.round(a.salary).toString(),
        TotalRawMaterials: Math.round(a.rawMaterial).toString(),
        TotalOverhead: Math.round(a.overload).toString(),
      });
    }



    )

    requestVoucher2.dataList = dataList;
    requestVoucher2.VocherHeadId = '0';
    await this._mediator.send(<CreateAndUpdateAutoVoucher2Command>requestVoucher2).then(a => {
      ;
      let req = new CreateMakeProductPriceCommand();
      req.fromDate = from;
      req.toDate = to;
       this._mediator.send(<CreateMakeProductPriceCommand>req);
    })

  };

  async navigateToVoucher() {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${this.MakeProductPrice?.voucherId}`)
  }
}
