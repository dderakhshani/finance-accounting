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
import { AddReceiptProductCommand } from '../../../../repositories/receipt/commands/temporary-receipt/add-receipt-product-command';
import { GetProductInputToWarehouse } from '../../../../repositories/receipt/queries/temporary-receipt/get-sina-request-query';
import { GetProductUpdateQuery } from '../../../../repositories/commodity/get-product-lastId-query';
import { GetAllProductNeedUpdateQuery } from '../../../../repositories/commodity/get-product-and-update-query';
import { UpdateProductProperty } from '../../../../repositories/receipt/queries/temporary-receipt/get-sina-update-commodity';



@Component({
  selector: 'app-add-temporary-receipt-product',
  templateUrl: './add-temporary-receipt-product.component.html',
  styleUrls: ['./add-temporary-receipt-product.component.scss']
})
export class AddTemporaryReceiptProductComponent extends BaseComponent {


  public _TemporaryReceipt: Receipt | undefined = undefined;
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;
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
    this.request = new AddReceiptProductCommand()

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

    this.request = new AddReceiptProductCommand();

    this.isSubmitForm = false

  }


  //----------------جستجو درخواست------------------------------------------------
  async get() {
    if (this.form.controls.documentDate.value == undefined) {
      this.Service.showHttpFailMessage('تاریخ رسید را وارد نمایید');
      return
    }
    let date = this.Service.toPersianDate(this.form.controls.documentDate.value);
    let documentDate = this.form.controls.documentDate.value;
    let vocher = this.form.controls.codeVoucherGroupId.value;
    await this._mediator.send(new GetProductInputToWarehouse(date)).then(async (res) => {

     await this.initializeResponse(res);

      this.form.controls.documentDate.setValue(documentDate);
      this.form.controls.codeVoucherGroupId.setValue(vocher);
    

    })
    await this._mediator.send(new UpdateProductProperty(date));
    

  }

  //==========================================================================
  async initializeResponse(res: any) {

    this.isSubmitForm = false;
    this._TemporaryReceipt = res;
    let response = res;
    this.apiItemsCount = this._TemporaryReceipt?.itemsCount;
    let newRequest = new AddReceiptProductCommand().mapFrom(response)
    this.request = newRequest;

  }
  //-------------------------------------------------------------------------


  async add() {

    if (this.form.controls.codeVoucherGroupId.value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را انتخاب نمایید');
      return;

    }
    if (this.form.controls.documentDate.value == undefined) {

      this.Service.showHttpFailMessage('تاریخ رسید را انتخاب کنید');
      return;

    }
    //if (!this.form.controls.creditAccountHeadId.value) {
    //  this.Service.showHttpFailMessage('سرفصل حساب بستانکار را وارد نمایید')
    //  return
    //}
    //else if (!this.form.controls.creditAccountReferenceId.value) {
    //  this.Service.showHttpFailMessage('حساب بستانکار را وارد نمایید')
    //  return
    //}
    var today = new Date(new Date().setHours(0, 0, 0, 0)).toUTCString();
    var documentDate = new Date(new Date(<Date>(this.form.controls.documentDate.value)).setHours(0, 0, 0, 0)).toUTCString()

    if (documentDate == today) {
      this.Service.showHttpFailMessage('ورودی های روزهای گذشته قابل ذخیره سازی می باشد');
      return;
    }
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.Temp);

    let response = await this._mediator.send(<AddReceiptProductCommand>this.request);

    this.isSubmitForm = true;

    this.pageModeTypeUpdate = true;


  }
  async GetProductLast() {

   let response = await this._mediator.send(new GetProductUpdateQuery());

  }
  async GetProductAll() {

    let response = await this._mediator.send(new GetAllProductNeedUpdateQuery());

  }
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);

  }

  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  creditReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }

  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }
  creditAccountHeadIdSelect(item: any) {

    this.form.controls.creditAccountHeadId.setValue(item?.id);

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
