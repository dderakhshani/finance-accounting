import { Component, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

import { FormArray, FormControl, FormGroup } from '@angular/forms';

import { AddInvoiceCommand } from '../../../repositories/invoice/commands/invoice/add-invoice-command';
import { IdentityService } from '../../../../identity/repositories/identity.service';
import { LoaderService } from '../../../../../core/services/loader.service';
import { Mediator } from '../../../../../core/services/mediator/mediator.service';
import { BaseComponent } from '../../../../../core/abstraction/base.component';
import { FormActionTypes } from '../../../../../core/constants/form-action-types';
import { GetWarehousesQuery } from '../../../../inventory/repositories/warehouse/queries/get-warehouses-query';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { AddInvoiceItemCommand } from '../../../repositories/invoice/commands/invoice-item/add-invoice-item-command';
import { Warehouse } from '../../../../inventory/entities/warehouse';
import { invoice } from '../../../entities/invoice';
import { Commodity } from '../../../../commodity/entities/commodity';
import { PageModes } from '../../../../../core/enums/page-modes';
import { UpdateInvoiceCommand } from '../../../repositories/invoice/commands/invoice/update-invoice-command';
import { GetInvoiceQuery } from '../../../repositories/invoice/queries/invoice/get-invoice-query';
import { GeVatDutiesTaxValueQuery } from '../../../repositories/base-value/ge-vat-duties-tax-query';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';

import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { AddIncoiceItemsComponent } from '../add-invoice-items/add-invoice-items.component';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { AttachmentsModel, UploadFileData } from '../../../../../core/components/custom/uploader/uploader.component';
import { environment } from '../../../../../../environments/environment';
import { GetRecepitAttachmentsQuery } from '../../../../inventory/repositories/receipt/queries/receipt/get-receipt-attachment-query';


@Component({
  selector: 'app-add-factor',
  templateUrl: './add-factor.component.html',
  styleUrls: ['./add-factor.component.scss']
})
export class AddFactorComponent extends BaseComponent {

  child: any;
  @ViewChild(AddIncoiceItemsComponent)
  set appShark(child: AddIncoiceItemsComponent) {
    this.child = child
  };

  documentTags: string[] = [];
  warehouses: Warehouse[] = [];
  //------------Attachment------------------------
  attachmentAssets: number[] = [];
  public attachmentsModel: AttachmentsModel = {
    typeBaseId: this.Service.AttachmentReceipt100,
    title: 'قرارداد خرید',
    description: 'فایل قرارداد خرید',
    keyWords: 'AttachmentReceipt',
  };
  public isUploading !: boolean;
  public attachmentIds: number[] = [];
  public imageUrls: UploadFileData[] = [];
  public baseUrl: string = environment.fileServer + "/";

  //----------call by uploder component-------------
  set files(values: string[]) {
    this.imageUrls = [];

    values.forEach((value: any) => {
      this.imageUrls.push(value);
    })
  }
  //-------------------------------------------
  //-------------------یونیک آیدی نوع سند پیش فاکتور-----------

  InvoiceAllStatusUnicCode: string = 'RegistertheFactor';

  public _TemporaryInvoice: invoice | undefined = undefined;

  public commodities: Commodity[] = [];
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;

  //---------------شمارش تعداد سطرهای کالا -----------------
  public rowCount: number = 1;


  constructor(private route: ActivatedRoute,
    private router: Router,
    private identityService: IdentityService,
    public Service: PagesCommonService,
    public _notificationService: NotificationService,
    private _mediator: Mediator,
    public ApiCallService: ApiCallService,
  ) {
    super(route, router);


    this.pageModeTypeUpdate = false;
    this.request = new AddInvoiceCommand()


  }
  //---------------attachment------------------------
  async getAttachments() {
    await this._mediator.send(new GetRecepitAttachmentsQuery(
      Number(this.getQueryParam('id')),
    )).then(res => {
      this.attachmentIds = res;
    })
  }

  setAttachmentIds() {
    var attachmentIds: FormArray = <FormArray>this.form.controls['attachmentIds'];
    attachmentIds.clear();
    this.imageUrls.forEach(res => {
      let formGroupArray = new FormControl(res.id);
      attachmentIds.push(formGroupArray);
    })
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
    //پر کردن لیست انبارها
    await this._mediator.send(new GetWarehousesQuery(0, 0,
      [
        new SearchQuery({
          propertyName: 'IsActive',
          comparison: 'equal',
          values: [true]
        })
      ]

    )).then(async (res) => {

      this.warehouses = res.data
    })

    this.request = new AddInvoiceCommand();

    this.disableControls()


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

    this.disableControls();


    //---------------------------------به دست آوردن آیدی کد قرارداد----------------

    await this.getInvoiceAllStatus();
    await this.getAttachments();

  }
  async PageModesUpdate(entity?: any) {

    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new UpdateInvoiceCommand().mapFrom(entity);
      this._TemporaryInvoice = entity;



      if (this._TemporaryInvoice?.tagArray)
        this.documentTags = this._TemporaryInvoice?.tagArray


      //پرکردن دیتا ها جهت ارسال به کامپونت فرزند

      this.child.vatDutiesTax = Number(this._TemporaryInvoice?.vatDutiesTax);
      this.child.totalItemPrice = Number(this._TemporaryInvoice?.totalItemPrice);
      this.child.totalProductionCost = Number(this._TemporaryInvoice?.totalProductionCost);

    }


    this.pageMode = PageModes.Update;
    this.pageModeTypeUpdate = true;
  }

  async PageModesAdd() {
    this.pageMode = PageModes.Add;
    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false
    this.request = new AddInvoiceCommand();
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
      this.request = new AddInvoiceCommand().mapFrom(entity);
      this._TemporaryInvoice = entity;

      if (this._TemporaryInvoice?.tagArray)
        this.documentTags = this._TemporaryInvoice?.tagArray

    }
    this.pageModeTypeUpdate = false;
    this.pageMode = PageModes.Update;
    this.isSubmitForm = false

    //پرکردن دیتا ها جهت ارسال به کامپونت فرزند

    this.child.vatDutiesTax = Number(this._TemporaryInvoice?.vatDutiesTax);
    this.child.totalItemPrice = Number(this._TemporaryInvoice?.totalItemPrice);
    this.child.totalProductionCost = Number(this._TemporaryInvoice?.totalProductionCost);

    this.form.controls['documentNo'].setValue(undefined);
    this.form.controls['documentDate'].setValue(undefined);
    this.form.controls['expireDate'].setValue(undefined);
    this.form.controls['invoiceNo'].setValue(undefined);
    this.disableControls();
  }
  //-----------------دریافت آیدی نوع سند پیش فاکتور--------------
  async getInvoiceAllStatus() {

    await this.ApiCallService.getInvoiceAllStatus().then(b => {

      let id = this.ApiCallService.InvoiceStatus.find(a => a.uniqueName == this.Service.PreInvoice)?.id
      this.form.controls.codeVoucherGroupId.setValue(id);
    })

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
    if (this.form.controls['documentDate'].value == undefined) {

      this.Service.showHttpFailMessage('تاریخ قرارداد را وارد نمایید');
      return;

    }
    if (this.form.controls['creditAccountReferenceId'].value == undefined) {

      this.Service.showHttpFailMessage('تامین کننده را انتخاب نمایید');
      return;

    }
    if (this.form.controls['invoiceNo'].value == undefined) {

      this.Service.showHttpFailMessage('شماره قرارداد را انتخاب نمایید.');
      return;

    }
    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);
    this.form.controls.isManual.setValue(true);

    this.form.controls.vatDutiesTax.setValue(this.child.vatDutiesTax);

    this.setAttachmentIds();

    await this._mediator.send(<AddInvoiceCommand>this.request).then(response => {
      this.form.controls['documentNo'].setValue(response.documentNo);
      this.isSubmitForm = true;
    });

  }
  get InvoiceDocumentItems(): FormArray {

    return this.form.controls['invoiceDocumentItems'] as FormArray;
  }

  public commodityCategorylevelCode: any = undefined;
  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);
    this.commodityCategorylevelCode = item?.levelCode;
    this.AddFirstRow();
  }
  ReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);

  }

  requesterReferenceSelect(item: any) {

    this.form.controls.requesterReferenceId.setValue(item?.id);

  }

  followUpReferenceIdSelect(item: any) {

    this.form.controls.followUpReferenceId.setValue(item?.id);

  }

  async navigateToInvoiceList() {

    await this.router.navigateByUrl('/purchase/factorList')
  }
  async reset() {
    this.form.controls['warehouseId'].value = undefined;
    this.deleteQueryParam('id');
    this.child.vatDutiesTax = 0;
    this.child.totalItemPrice = 0;
    this.child.totalProductionCost = 0;
    this.isSubmitForm = false;
    this._TemporaryInvoice = undefined;
    this.rowCount = 1;
    return super.reset();
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
  get formItems(): FormArray {

    return this.form.get('receiptDocumentItems') as FormArray;
  }
  async get(Id: number) {
    return await this._mediator.send(new GetInvoiceQuery(Id))
  }

  async update() {
    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);
    this.form.controls.vatDutiesTax.setValue(this.child.vatDutiesTax);

    this.setAttachmentIds();

    let response = await this._mediator.send(<UpdateInvoiceCommand>this.request);
    this.isSubmitForm = true;
    return await this.initialize(response);
  }
  async edit() {
  }
  close(): any {
  }

  delete(param?: any): any {
  }
}
