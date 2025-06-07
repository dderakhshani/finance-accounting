import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../../entities/warehouse';
import { Receipt } from '../../../../entities/receipt';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { DocumentState } from '../../../../entities/documentState';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';

import { LeavingProductCommand } from '../../../../repositories/warehouse-layout/commands/leaving-warehouse/leaving-product-command';
import { GetProductLeaveWarehouse } from '../../../../repositories/receipt/queries/receipt/get-product-leave-warehouse-query';
import { UpdateProductProperty } from '../../../../repositories/receipt/queries/temporary-receipt/get-sina-update-commodity';
import { AddItemsProductCommand } from '../../../../repositories/receipt/commands/receipt-items/add-receipt-items-product-command';
import { ArchiveDocumentHeadsByDocumentDateCommand } from '../../../../repositories/receipt/commands/reciept/archive-documentHeads-by-documentDate';

@Component({
  selector: 'app-leaving-product',
  templateUrl: './leaving-product.component.html',
  styleUrls: ['./leaving-product.component.scss']
})
export class LeavingProductComponent extends BaseComponent {


  public _TemporaryReceipt: Receipt | undefined = undefined;
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;
  public countRowsSave: number = 0;
  public countRowsReamove: number = 0;
  public rowCount: number | undefined = 0;
  public apiItemsCount: number | undefined = 0;
  constructor(

    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public ServiceAPI: ApiCallService,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.pageModeTypeUpdate = false;
    this.request = new LeavingProductCommand()

  }
  async ngOnInit() {
    await this.resolve()
  }


  //--------------------------------------------------
  async ngAfterViewInit() {

  }

  async resolve(params?: any) {
    this.formActions = [

      FormActionTypes.list,
    ];
    await this.initialize()

  }


  async initialize(entity?: any) {


    this.pageModeTypeUpdate = false;

    this.request = new LeavingProductCommand();

    this.isSubmitForm = false

  }


  //----------------جستجو درخواست------------------------------------------------
  async get() {
    if (this.form.controls.documentDate.value == undefined) {
      this.Service.showHttpFailMessage('تاریخ سند را وارد نمایید');
      return
    }



    let date = this.Service.toPersianDate(this.form.controls.documentDate.value);
    let documentDate = this.form.controls.documentDate.value;
    let vocher = this.form.controls.codeVoucherGroupId.value;
    await this._mediator.send(new GetProductLeaveWarehouse(date)).then(async (res) => {

      await this.initializeResponse(res);

      this.form.controls.documentDate.setValue(documentDate);
      this.form.controls.codeVoucherGroupId.setValue(vocher);

    })



  }

  //==========================================================================
  async initializeResponse(res: any) {
    this.countRowsSave = 0;
    this.rowCount = 0;
    this.apiItemsCount = 0;
    this.isSubmitForm = false;

    this._TemporaryReceipt = res;
    let response = res;
    this.rowCount = this._TemporaryReceipt?.items.length;
    this.apiItemsCount = this._TemporaryReceipt?.itemsCount;
    let newRequest = new LeavingProductCommand().mapFrom(response)
    this.request = newRequest;


  }
  //-------------------------------------------------------------------------


  async add() {

    if (this.form.controls.codeVoucherGroupId.value == undefined) {

      this.Service.showHttpFailMessage('نوع سند را انتخاب نمایید');
      return;

    }
    if (this.form.controls.documentDate.value == undefined) {

      this.Service.showHttpFailMessage('تاریخ سند را انتخاب کنید');
      return;

    }
    //if (!this.form.controls.debitAccountHeadId.value) {
    //  this.Service.showHttpFailMessage('سرفصل حساب بدهکار را وارد نمایید')
    //  return
    //}
    //else if (!this.form.controls.debitAccountReferenceId.value) {
    //  this.Service.showHttpFailMessage('حساب بدهکار  را وارد نمایید')
    //  return
    //}


    var today = new Date(new Date().setHours(0, 0, 0, 0)).toUTCString();
    var documentDate = new Date(new Date(<Date>(this.form.controls.documentDate.value)).setHours(0, 0, 0, 0)).toUTCString()

    if (documentDate == today) {
      this.Service.showHttpFailMessage('خروجی های روزهای گذشته قابل ذخیره سازی می باشد');
      return;
    }
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.Leave);

    let request = new ArchiveDocumentHeadsByDocumentDateCommand()
      request.warehouseId = 1,
      request.fromDate = this.form.controls['documentDate'].value,
      request.toDate = this.form.controls['documentDate'].value,
      request.documentStatuesBaseValue = this.form.controls['documentStauseBaseValue']?.value

    await this._mediator.send(<ArchiveDocumentHeadsByDocumentDateCommand>request).then(a => {
      ;

      for (let item of this.form.controls['receiptDocumentItems']?.controls) {
        var receiptDocumentItems = new AddItemsProductCommand();

        receiptDocumentItems.quantity = item.controls.quantity.value;
        receiptDocumentItems.requestNo = item.controls.requestNo.value;
        receiptDocumentItems.commodityId = item.controls.commodityId.value;
        receiptDocumentItems.mainMeasureId = item.controls.mainMeasureId.value;
        receiptDocumentItems.documentMeasureId = item.controls.documentMeasureId.value;
        receiptDocumentItems.description = item.controls.description.value;
        receiptDocumentItems.warehouseId = item.controls.warehouseId.value;

        receiptDocumentItems.unitPrice = item.controls.unitPrice.value;
        receiptDocumentItems.unitBasePrice = item.controls.unitBasePrice.value;
        receiptDocumentItems.productionCost = item.controls.productionCost.value;
        receiptDocumentItems.currencyBaseId = item.controls.currencyBaseId.value;
        receiptDocumentItems.currencyPrice = item.controls.currencyPrice.value;
        receiptDocumentItems.discount = item.controls.discount.value;


        let letRequest = new LeavingProductCommand();

        letRequest.documentDate = this.form.controls['documentDate'].value;
        letRequest.codeVoucherGroupId = this.form.controls['codeVoucherGroupId']?.value;
        letRequest.documentStauseBaseValue = this.form.controls['documentStauseBaseValue']?.value;
        letRequest.debitAccountReferenceId = this.form.controls['debitAccountReferenceId']?.value;
        letRequest.debitAccountReferenceGroupId = this.form.controls['debitAccountReferenceGroupId']?.value;
        letRequest.debitAccountHeadId = this.form.controls['debitAccountHeadId']?.value;
        letRequest.isDocumentIssuance = this.form.controls['isDocumentIssuance']?.value;
        letRequest.receiptDocumentItems.push(receiptDocumentItems);

        this._mediator.send(<LeavingProductCommand>letRequest).then(a => {
          this.countRowsSave = this.countRowsSave + 1;
        })
        
      }
    });

    this.isSubmitForm = true;

    this.pageModeTypeUpdate = true;


  }

  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);

  }

  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }

  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);



  }

  async navigateToReceiptList() {

    await this.router.navigateByUrl('inventory/receiptList')

  }
  async reset() {

    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false

    super.reset();




  }

  //----------------حذف گروهی----------------------------------------
  delete() {

    if (this._TemporaryReceipt?.items != undefined) {
      this._TemporaryReceipt.items = this._TemporaryReceipt?.items.filter(a => a.selected != true);
    }

  }
  async edit() {
  }
  close(): any {
  }
  update(param?: any) {
    throw new Error('Method not implemented.');
  }

}
