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

@Component({
  selector: 'app-add-request-commodity',
  templateUrl: './add-request-commodity.component.html',
  styleUrls: ['./add-request-commodity.component.scss']
})
export class AddRequestCommodityComponent extends BaseComponent {

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
      FormActionTypes.add,
      FormActionTypes.list,
    ];


    this.request = new AddReceiptCommand();

    this.disableControls()
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


    this.setValue();
  }

  async setValue() {

    this.form.controls.documentNo.setValue(undefined);

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
    this.form.controls.documentStauseBaseValue.setValue(DocumentState.requestRecive);
    this.form.controls.requestDate.setValue(this.form.controls.documentDate.value);
    await this._mediator.send(<AddReceiptCommand>this.request).then(response => {
      this.form.controls['documentNo'].setValue(response.documentNo);
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
    

    this.AddFirstRow();

  }

  requesterReferenceSelect(item: any) {

    this.form.controls.requesterReferenceId.setValue(item?.id);
    
  }
  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);
  }
  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }
  
  //-----------------دریافت آیدی نوع سند قرارداد------------------------------------
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    this.form.controls.viewId.setValue(item.viewId);
    this.form.controls.debitAccountHeadId.setValue(item?.defultDebitAccountHeadId);
    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }
    if (item.uniqueName == this.Service.RequestReceiveConsumption) {
      this.IsConsumable= true;
      this.IsAsset = false;
    }
    if (item.uniqueName == this.Service.RequesReceiveAssets) {
      this.IsAsset = true;
      this.IsConsumable = false;
    }

  }
  async navigateToReceiptList() {

      await this.router.navigateByUrl(`/inventory/request-list/requestCommodityList?codeVoucherGroupId=${this.form.controls['codeVoucherGroupId'].value}`)


  }

  async reset() {
    this.isSubmitForm = false;
    this.SetTempValue();
    await this.deleteQueryParam("id")
    super.reset();
    this.GetTempValue();
    this.rowCount = 1;
    this.AddFirstRow();

  }
  SetTempValue() {
    this.temp_warehouseId = this.form.controls['warehouseId'].value;
    this.temp_codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;
    this.temp_documentDate = this.form.controls['documentDate'].value;
  }
  GetTempValue() {
    this.form.controls['warehouseId'].setValue(this.temp_warehouseId);
    this.form.controls['codeVoucherGroupId'].setValue(this.temp_codeVoucherGroupId);
    this.form.controls['documentDate'].setValue(this.temp_documentDate);

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
      else {
        //اگر نوع انبار تغییر کرد داده های کالاهای قبلی باید پاک شود.
        var list: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];
        list.reset();
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
