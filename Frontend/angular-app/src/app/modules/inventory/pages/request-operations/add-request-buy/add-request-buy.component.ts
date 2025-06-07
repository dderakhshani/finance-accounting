
import { Receipt } from '../../../entities/receipt';
import { ActivatedRoute, Router } from "@angular/router"
import { Warehouse } from '../../../entities/warehouse';
import { FormArray, FormControl } from '@angular/forms';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { DocumentState } from '../../../entities/documentState';
import { PageModes } from '../../../../../core/enums/page-modes';
import { Commodity } from '../../../../commodity/entities/commodity';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ReceiptAllStatusModel } from '../../../entities/receipt-all-status';
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { AddItemsCommand } from '../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { AddReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/add-receipt-command';
import { UpdateTemporaryReceiptCommand } from '../../../repositories/receipt/commands/temporary-receipt/update-temporary-receipt-command';
import { GetTemporaryRecepitQuery } from '../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { TableComponent } from '../../../../../core/components/custom/table/table.component';
import { ManualItemsComponent } from '../../component/add-manual-items/add-manual-items.component';

@Component({
  selector: 'app-add-request-buy',
  templateUrl: './add-request-buy.component.html',
  styleUrls: ['./add-request-buy.component.scss']
})
export class AddRequestBuyComponent extends BaseComponent {

  documentTags: string[] = [];
  warehouses: Warehouse[] = [];
  public commodities: Commodity[] = [];
  public temp_warehouseId: any;
  public temp_documentDate: any;
  public temp_codeVoucherGroupId: any;
  public temp_requesterReferenceId: any;
  public isSubmitForm: boolean = false;
  public pageModeTypeUpdate: boolean = false;
  public _Receipt: Receipt | undefined = undefined;
  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;

  child: any;
  @ViewChild(ManualItemsComponent)
  set appShark(child: ManualItemsComponent) {
    this.child = child
  };
  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
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
      FormActionTypes.add,
      FormActionTypes.list,
    ];

    this.request = new AddReceiptCommand();

    await this.initialize()
  }
  async initialize(entity?: any) {

    //اگر در حالت ویرایش بودیم
    if (entity || (this.getQueryParam('id') && this.getQueryParam('displayPage') == 'edit')) {
      await this.PageModesUpdate(entity);

    }
    //در حالت کپی بودیم
    else if ((this.getQueryParam('id') && this.getQueryParam('displayPage') == 'copy')) {
      this.PageModesCopy();
    }
    else {
      //اگر در حالت ثبت بودیم
      await this.PageModesAdd();
    }
  }
  async PageModesUpdate(entity?: any) {
    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new UpdateTemporaryReceiptCommand().mapFrom(entity);
      this._Receipt = entity;

      if (this._Receipt?.tagArray)
        this.documentTags = this._Receipt?.tagArray

    }

    
    this.pageMode = PageModes.Update;
    this.pageModeTypeUpdate = true;
  }

  async PageModesAdd() {
    this.pageMode = PageModes.Add;
    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false



    this.identityService._applicationUser.subscribe(res => {
      if (res.isAuthenticated) {

        this.form.controls.followUpReferenceId.setValue(res.accountReferenceId);
        this.form.controls.requesterReferenceId.setValue(res.accountReferenceId);

      }
    });
  }
  async PageModesCopy(entity?: any) {

    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new AddReceiptCommand().mapFrom(entity);
      this._Receipt = entity;

      if (this._Receipt?.tagArray)
        this.documentTags = this._Receipt?.tagArray

    }
    this.pageModeTypeUpdate = false;
    this.pageMode = PageModes.Update;
    this.isSubmitForm = false
    this.form.controls['documentNo'].setValue(undefined);
    this.form.controls['documentDate'].setValue(undefined);
    this.form.controls['expireDate'].setValue(undefined);
  }


  //----------------ذخیره------------------------------
  async add() {
    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار تحویل گیرنده را انتخاب نمایید');
      return;

    }
    if (this.form.controls['documentDate'].value == undefined) {

      this.Service.showHttpFailMessage('تاریخ درخواست را وارد نمایید');
      return;

    }

    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);
    this.form.controls.isManual.setValue(true);
    this.form.controls.requestDate.setValue(this.form.controls.documentDate.value);
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.requestBuy);
    let response = await this._mediator.send(<AddReceiptCommand>this.request);
    this.form.controls['documentNo'].setValue(response.documentNo);
  }
  get receiptDocumentItems(): FormArray {

    return this.form.controls['receiptDocumentItems'] as FormArray;
  }

  public commodityCategorylevelCode: any = undefined;
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);
    this.commodityCategorylevelCode = item?.levelCode;
    this.AddFirstRow();

  }
  ReferenceSelect(item: any) {
    this.form.controls.creditAccountReferenceId.setValue(item?.id);
  }
  requesterReferenceSelect(item: any) {
    this.form.controls.requesterReferenceId.setValue(item?.id);
  }
  followUpReferenceIdSelect(item: any) {
    this.form.controls.followUpReferenceId.setValue(item?.id);
  }
  //-----------------دریافت آیدی نوع سند قرارداد------------------------------------
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }
  }
  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/request-list/requesBuyList')
  }

  async reset() {
    this.pageMode = PageModes.Add;
    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false
    await this.deleteQueryParam("id")
    this.SetTempValue();
    super.reset();
    this.GetTempValue();
    this.rowCount = 1;
    this.AddFirstRow();

  }
  SetTempValue() {

    this.temp_warehouseId = this.form.controls['warehouseId'].value;
    this.temp_documentDate = this.form.controls['documentDate'].value;
    this.temp_codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;
    this.temp_requesterReferenceId = this.form.controls['requesterReferenceId'].value;

  }
  GetTempValue() {

    this.form.controls['warehouseId'].setValue(this.temp_warehouseId);
    this.form.controls['documentDate'].setValue(this.temp_documentDate);
    this.form.controls['codeVoucherGroupId'].setValue(this.temp_codeVoucherGroupId);
    this.form.controls['requesterReferenceId'].setValue(this.temp_requesterReferenceId);

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
  async onRequsetType(ob: any) {


    if (this.form.controls.documentDate.value == undefined) {
      this._notificationService.showWarningMessage('تاریخ سند را ابتدا انتخاب نمایید');
      this.form.controls.isImportPurchase.setValue(undefined);
      return
    }

    let month = 0;
    if (this.form.controls.documentDate.value._d != undefined) {
      month = this.form.controls.documentDate.value._d.getMonth() + 1;
    }
    else {
      month = this.form.controls.documentDate.value.getMonth() + 1;
    }
    if (ob.value == true) {
      this.form.controls.expireDate.setValue(new Date(new Date().setMonth(month + this.Service.TimeImportPurchases)))
    }
    else {
      this.form.controls.expireDate.setValue(new Date(new Date().setMonth(month + this.Service.TimeInternalPurchases)))
    }
  }
  close(): any {
  }

  delete(param?: any): any {
  }



  async edit() {
  }
}
