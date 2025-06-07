import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../entities/warehouse';
import { Receipt } from '../../../entities/receipt';
import { AddReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/add-receipt-command';
import { LoaderService } from '../../../../../core/services/loader.service';
import { FormArray, FormControl } from '@angular/forms';
import { Commodity } from '../../../../commodity/entities/commodity';
import { AddItemsCommand } from '../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { PageModes } from '../../../../../core/enums/page-modes';
import { UpdateTemporaryReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/update-temporary-receipt-command';
import { GetTemporaryRecepitQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { DocumentState } from '../../../entities/documentState';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { ManualItemsComponent } from '../../component/add-manual-items/add-manual-items.component';
import { GetTemporaryRecepitByDocumentNoQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-documentId-query';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-warehouseId-query';

@Component({
  selector: 'app-add-request-return-commodity',
  templateUrl: './add-request-return-commodity.component.html',
  styleUrls: ['./add-request-return-commodity.component.scss']
})
export class AddRequestReturnCommodityComponent extends BaseComponent {

  documentTags: string[] = [];

  warehouses: Warehouse[] = [];
  public _Receipt: Receipt | undefined = undefined;

  public commodities: Commodity[] = [];
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;
  public IsConsumable: boolean = false;
  public IsAsset: boolean = false;
  public temp_warehouseId: any;
  public temp_codeVoucherGroupId: any;
  public temp_documentDate: any;
  public codeVoucherGroupId: number = 2407
  public type: number=0;
  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;
  child: any;
  @ViewChild(ManualItemsComponent)
  set appShark(child: ManualItemsComponent) {
    this.child = child
  };
  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    public ApiCallService: ApiCallService,
    private _mediator: Mediator
  ) {
    super(route, router);


    this.pageModeTypeUpdate = false;
    this.request = new AddReceiptCommand()

  }


  async ngOnInit() {


  }
  async ngAfterViewInit() {
    await this.resolve()
  }

  async resolve(params?: any) {
    this.formActions = [

      FormActionTypes.list,
      FormActionTypes.add
    ];


    this.request = new AddReceiptCommand();


    await this.initialize()
  }
  async initialize(entity?: any) {
    //اگر در حالت ویرایش بودیم
    if (entity || this.getQueryParam('id')) {
      await this.PageModesUpdate(entity);
    }
    else {
      //اگر در حالت ثبت بودیم
      await this.PageModesAdd();
    }

    this.disableControls();

  }
  async PageModesUpdate(entity?: any) {
    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new UpdateTemporaryReceiptCommand().mapFrom(entity);
      this._Receipt = entity;

      if (this._Receipt?.tagArray)
        this.documentTags = this._Receipt?.tagArray


    }

    this.rowCount = 1;
    this.pageMode = PageModes.Update;
    this.pageModeTypeUpdate = true;
  }

  async PageModesAdd() {
    this.pageMode = PageModes.Add;
    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false

    this.request = new AddReceiptCommand();

    if (this.getQueryParam('documentNo') != undefined) {
      this.form.controls.requestNumber.setValue(this.getQueryParam('documentNo'));
      await this.getRequest(this.type);

    }
    await this.getRequest(this.type);

  }
  //----------------جستجو درخواست------------------------------------------------
  async getRequest(type: number) {

    if (this.form.controls.requestNumber.value == undefined && type != 3) {

      if (type == 1) {
        this.Service.showHttpFailMessage('شماره درخواست خرید وارد نمایید');
      }
      else if (type == 2) {
        this.Service.showHttpFailMessage('شماره رسید وارد نمایید');
      }
    }
    else {
      await this.getRequestDetails(this.form.controls.requestNumber.value, type)
      await this.getRequestDetails(this.form.controls.requestNumber.value, type)
      await this.setValue();

    }

  }

  async getRequestDetails(documentNo: number,type:number) {

    //درخواست خرید
    if (type == 1) {
      await this._mediator.send(new GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery(documentNo, 0)).then(async (res) => {

        this._Receipt = res;
        let response = res;
        this.request = new AddReceiptCommand().mapFrom(response)
        this.form.controls.requestNumber.setValue(documentNo);
        this.form.controls.debitAccountReferenceId.setValue(undefined);
        this.form.controls.debitAccountReferenceGroupId.setValue(undefined);

      })
    }
    //رسید انبار
    if (type == 2) {
      await this._mediator.send(new GetTemporaryRecepitByDocumentNoQuery(documentNo)).then(async (res) => {

        this._Receipt = res;
        let response = res;
        this.request = new AddReceiptCommand().mapFrom(response)
        this.form.controls.requestNumber.setValue(documentNo);

      })
    }
    if (type == 3) {

      await this._mediator.send(new GetTemporaryRecepitByDocumentNoQuery(0)).then(async (res) => {

        this._Receipt = res;
        let response = res;
        this.request = new AddReceiptCommand().mapFrom(response)
        this.form.controls.requestNumber.setValue(documentNo);
        this.form.reset()

      })
    }
  }

  async setValue() {


    this.form.controls.documentNo.setValue(undefined);


    this.disableControls();

    this.identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {
        this.form.controls.followUpReferenceId.setValue(res.accountReferenceId);
        this.form.controls.requesterReferenceId.setValue(res.accountReferenceId);
      }
    });
  }
  //-----------------------------------------------
  disableControls() {

    this.form.controls['documentNo'].disable();

  }
  //----------------ذخیره------------------------------
  async add() {
    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار تحویل گیرنده را انتخاب نمایید');
      return;

    }

    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);
    this.form.controls.isManual.setValue(true);
    this.form.controls.codeVoucherGroupId.setValue(this.codeVoucherGroupId);
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.requestRecive);
    this.form.controls.requestDate.setValue(this.form.controls.documentDate.value);

    await this._mediator.send(<AddReceiptCommand>this.request).then(response => {
      this.form.controls['documentNo'].setValue(response.documentNo);
      this.navigateToRecive(response)
      this.isSubmitForm = true;
    });

  }
  get receiptDocumentItems(): FormArray {

    return this.form.controls['receiptDocumentItems'] as FormArray;
  }

  public commodityCategorylevelCode: any = undefined;

  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);
    this.commodityCategorylevelCode = item?.levelCode;

  }

  requesterReferenceSelect(item: any) {

    this.form.controls.requesterReferenceId.setValue(item?.id);
  }

  //-----------------دریافت آیدی نوع سند قرارداد------------------------------------
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    this.form.controls.viewId.setValue(item.viewId);
    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }
  }
  async navigateToReceiptList() {

    await this.router.navigateByUrl(`/inventory/request-list/requestCommodityList?codeVoucherGroupId=${this.codeVoucherGroupId}`)


  }
  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item.accountReferenceGroupId);
  }
  async navigateToRecive(Receipt: Receipt) {
    var codeVoucherGroupCode = this.ApiCallService.AllReceiptStatus.find(a => a.id == Receipt?.codeVoucherGroupId)?.code;
    var NewCode = codeVoucherGroupCode?.substring(0, 2) + (Number(this.Service.CodeLeaveReceipt)).toString();
    var codeVoucherGroupId = this.ApiCallService.AllReceiptStatus.find(a => a.code == NewCode)?.id;

    await this.router.navigateByUrl(`inventory/leavingConsumableWarehouse?documentNo=${Receipt.documentNo}&codeVoucherGroupId=${codeVoucherGroupId}`)

  }
  async reset() {
    this.isSubmitForm = false;

    await this.deleteQueryParam("id")
    super.reset();

    this.rowCount = 1;
    this.AddFirstRow();

  }


  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }
  //----------------------------------------------
  async AddItems() {

    this.child.AddItems();
    this.rowCount = this.rowCount + 1

  }
  //----------------فقط در قسمت انتخاب انبار صدا زده می شود
  //-----------------در هنگامی که اولین بار است که انبار انتخاب می شود یک سطر برای کالا اضافه شود.
  async AddFirstRow() {

    if (!this.pageModeTypeUpdate) {
      if (this.rowCount == 1) {

        await this.AddItems();
      }

    }

  }
  async get(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
  }

  async update() {
    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);


    let response = await this._mediator.send(<UpdateTemporaryReceiptCommand>this.request);
    this.isSubmitForm = true;
    this.pageModeTypeUpdate = true;
    return await this.initialize(response);
  }
  close(): any {
  }

  delete(param?: any): any {
  }



  async edit() {
  }
}
