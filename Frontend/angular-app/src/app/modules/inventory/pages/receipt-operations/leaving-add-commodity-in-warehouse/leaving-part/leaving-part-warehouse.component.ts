import { Router } from "@angular/router";
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatAccordion, MatExpansionModule } from '@angular/material/expansion';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { RequestCommodityWarehouse, RequestItemCommodity } from '../../../../entities/request-commodity-warehouse';
import { leavingWarehouseRequestByIdQuery } from '../../../../repositories/receipt/queries/receipt/get-request-commodity-warehouse-query';
import { LeavingPartWarehouseCommand, LeavingPartWarehouseDocumentItem } from '../../../../repositories/warehouse-layout/commands/leaving-warehouse/leaving-part-warehouse-command';
import { WarehouseLayoutsAddInventoryDialogComponent } from './warehouse-layouts-add-inventory-dialog/warehouse-layouts-add-inventory-dialog.component';
import { Warehouse } from "../../../../entities/warehouse";
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from "../../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command";
@Component({
  selector: 'app-leaving-part-warehouse',
  templateUrl: './leaving-part-warehouse.component.html',
  styleUrls: ['./leaving-part-warehouse.component.scss'],

})
export class leavingPartWarehouseComponent extends BaseComponent {
  @ViewChild(MatAccordion) accordion: MatAccordion | undefined;
  public requests: RequestCommodityWarehouse | undefined = undefined;

  iSsubmitForm: number = 0;
  valid: boolean = true;
  panelOpenState = true;
  isShowSubmit = false;

  form = new FormGroup({
    requestNumber: new FormControl('', Validators.required),
    warehouseId: new FormControl(),
    debitAccountReferenceId: new FormControl(),
    debitAccountReferenceGroupId: new FormControl(),
    isDocumentIssuance: new FormControl(true),
    debitAccountHeadId: new FormControl(),
    documentNo: new FormControl()
  });

  constructor(
    public _mediator: Mediator,
    private router: Router,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    public dialog: MatDialog

  ) {
    super();

  }

  async ngOnInit() {
    await this.resolve()
    this.Service.ListId = [];
  }

  async resolve() {
    if (this.getQueryParam('requestNumber'))
      this.form.controls['requestNumber'].setValue(this.getQueryParam('requestNumber'));

    if (this.getQueryParam('WarehouseId'))
      this.form.controls['warehouseId'].setValue(this.getQueryParam('WarehouseId'));

    if (this.getQueryParam('requestNumber') && this.getQueryParam('WarehouseId'))
      this.getData();
  }

  initialize() {
  }

  //    <!--StatusId :
  //1	در دست اقدام
  //2	تایید سرپرست
  //3	تایید مدیر واحد
  //4	تایید انبار
  //5	جمع آوری
  //6	آماده ارسال
  //7	ارسال شده
  //8	  تحویل شده **
  //9	  ** در انتظار داغی
  //10	** داغی دریافت شده
  //11	** پایان فرایند تحویل
  //12	** ابطال درخواست
  //13	آرشیو
  //14	خروج پیک
  //15	برگشت پیک
  //16	شروع جمع آوری
  //17	پایان جمع آوری
  //18	** تغییر وضعیت داغی قطعه-- >
  async get() {


    await this._mediator.send(new leavingWarehouseRequestByIdQuery(this.form.controls.requestNumber.value, this.form.controls.warehouseId.value,)).then(res => {
      this.requests = res
      this.form.controls.documentNo.setValue(this.requests?.request?.documentNo);
      this.isShowSubmit = false;
      this.isShowSubmit = ((this.requests?.request?.statusId >= 8 && this.requests?.request?.statusId < 13) || this.requests?.request?.statusId == 18)

      //-----------------بروز رسانی تعداد قابل انتخاب از هر موقعیت-----------------
      this.requests.items.forEach(a => {

        var length = a.layouts.length;
        let qu: number = a.quantity - a.quantityExit;

        if (length == 1) {
          a.layouts.forEach(b => {


            b.totalQuantity = qu;
            b.disabled = Number(b.totalQuantity) <= 0 ? false : true;


          })
        }
        else {

          a.layouts.forEach(b => {
            b.totalQuantity = null;
            b.disabled = qu <= 0 ? false : true
          });

        }

      })
    })
  }

  getData() {

    if (!this.form.controls.warehouseId.value) {
      this.Service.showHttpFailMessage('انبار مورد نظر را انتخاب نمایید');
      return;
    }
    this.iSsubmitForm = 0;
    this.debitReferenceSelect(undefined);

    this.get();

  }
  async onSave() {


    if (!this.form.controls.debitAccountReferenceId.value) {
      this.Service.showHttpFailMessage('حساب تحویل گیرنده را انتخاب نمایید');
      return;
    }
    this.valid = true;
    await this.GetValidation();

    if (this.valid) {

      await this.GetResponse().then(a => {
        if (this.iSsubmitForm > 0) {
          this.get();
        }
      });

    }
  }
  async GetValidation() {

    return await this.requests?.items.forEach(a => {
      var length = a.layouts.length;
      var sumQuantity: number = 0;


      a.layouts.forEach(l => {
        //اگر تعداد محل های قابل اتخاب انبار از یکی بیشتر بود
        if (length > 1) {

          sumQuantity = sumQuantity + Number(l.totalQuantity);
          if (sumQuantity > Number(a.quantity)) {
            this.Service.showHttpFailMessage('تعداد خروجی  کالا  ' + l.commodityTitle + ' مغایرت  با تعداد درخواستی دارد');
            this.valid = false;

          }
        }
        else if (Number(l.totalQuantity) > Number(a.quantity)) {
          this.Service.showHttpFailMessage('تعداد خروجی  کالا  ' + l.commodityTitle + ' مغایرت  با تعداد درخواستی دارد');
          this.valid = false;

        }
        else if (!l.allowOutput) {
          this.Service.showHttpFailMessage('اجازه خروج کالا ' + l.commodityTitle + ' وجود ندارد ');
          this.valid = false;

        }
      })

    });
  }
  async GetResponse() {
    this.iSsubmitForm = 0;

    var request = new LeavingPartWarehouseCommand();
    request.request_No = this.form.controls.requestNumber.value;
    request.documentNo = Number(this.requests?.request?.documentNo);
    request.debitAccountReferenceGroupId = this.form.controls.debitAccountReferenceGroupId.value;
    request.debitAccountReferenceId = this.form.controls.debitAccountReferenceId.value;
    request.debitAccountHeadId = this.form.controls.debitAccountHeadId.value;
    request.warehouseId = this.form.controls.warehouseId.value;

    await this.requests?.items.forEach(a => {
      a.layouts.forEach(async l => {
        if (Number(l.totalQuantity) > 0) {



          let documentItem: LeavingPartWarehouseDocumentItem = {
            warehouseLayoutQuantitiyId: l.id,
            quantity: l.totalQuantity,
            commodityId: l.commodityId,
            requestItemId: a.requestItemId,
            quantityTotal: a.quantityTotal,
            description: a.description,
            quantityLayout: a.quantity
          }

          request.warehouseDocumentItem.push(documentItem)




        }

      })

    }
    );

    return await this._mediator.send(<LeavingPartWarehouseCommand>request).then(res => {
      this.iSsubmitForm = this.iSsubmitForm + 1;

      this.form.controls.documentNo.setValue(res.documentNo);

      let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
      UpdateAvgPrice.documentId = this.requests?.request?.documentId;
      this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
    })
  }

  async AddInventory(requestItemCommodity: RequestItemCommodity) {


    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      ItemCommodity: requestItemCommodity,
    };

    let dialogReference = this.dialog.open(WarehouseLayoutsAddInventoryDialogComponent, dialogConfig);
    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {
      this.get();
    })
  }

  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);



  }
  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);
  }
  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }

  async navigateToHistory(commodityId: number) {

    var url = `inventory/commodityReceiptReports?commodityId=${commodityId}&warehouseId=${this.form.controls.warehouseId.value}`
    this.router.navigateByUrl(url)

  }
  async navigateToreceiptDetails(item: any) {
    await this.router.navigateByUrl(`inventory/receiptDetails?documnetNo=${item.documentNo}&displayPage=archive&isImportPurchase=false&documentStauseBaseValue=44`)
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
