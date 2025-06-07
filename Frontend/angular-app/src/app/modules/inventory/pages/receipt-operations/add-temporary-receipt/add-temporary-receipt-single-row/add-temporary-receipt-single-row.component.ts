import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { PageModes } from "../../../../../../core/enums/page-modes";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../../entities/warehouse';
import { Receipt } from '../../../../entities/receipt';
import { UpdateTemporaryReceiptCommand } from '../../../../repositories/receipt/commands/temporary-receipt/update-temporary-receipt-command';
import { GetTemporaryRecepitQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { AddReceiptCommand } from '../../../../repositories/receipt/commands/temporary-receipt/add-receipt-command';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { GetTemporaryRecepitByDocumentNoQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-documentId-query';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { DocumentState } from '../../../../entities/documentState';
import { ApiCallService } from '../../../../../../shared/services/pages/api-call/api-call.service';
import { getPurchaseRequestByIdQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-arani-request-query';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-warehouseId-query';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
@Component({
  selector: 'app-add-temporary-receipt-single-row',
  templateUrl: './add-temporary-receipt-single-row.component.html',
  styleUrls: ['./add-temporary-receipt-single-row.component.scss']
})
export class AddTemporaryReceiptSingleRowComponent extends BaseComponent {

  documentTags: string[] = [];

  public _TemporaryReceipt: Receipt | undefined = undefined;
  public pageModeTypeUpdate: boolean = false;
  public isSubmitForm: boolean = false;
  public EditType: number = 0;
  public temp_warehouseId: any;
  public temp_codeVoucherGroupId: any;
  public temp_documentDate: any;
  //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
  //در حالتی که از سیستم آرانی درخواست ها خوانده شود و در سیستم ایفا ثبت نشده باشد.
  public commodityId: number | undefined = undefined;

  public codeVocher: string = '';

  constructor(
   
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public ServiceAPI: ApiCallService,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.pageModeTypeUpdate = false;
    this.request = new AddReceiptCommand()

  }
  async ngOnInit() {
    await this.resolve()
  }

  
  //--------------------------------------------------
  async ngAfterViewInit() {

  }

  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.list,
    ];
    await this.initialize()

  }


  async initialize(entity?: any) {

   
    if (entity || this.getQueryParam('id')) {

      this.initializeUpdate();
      

    }
    else if (this.getQueryParam('documentNo') != undefined) {

      this.form.controls.requestNumber.setValue(this.getQueryParam('documentNo'));
      this.form.controls.warehouseId.setValue(this.getQueryParam('warehouseId'));
      await this.getRequestDetails().then(res => {
        this.pageModeTypeUpdate = true;
        
      });


    }
    else {

      this.pageModeTypeUpdate = false;

      this.request = new AddReceiptCommand();
    }
   

  }
  async initializeUpdate(entity?: any) {
    if (!entity) entity = await this.get(this.getQueryParam('id'))
    {
      this.request = new UpdateTemporaryReceiptCommand().mapFrom(entity);
      this._TemporaryReceipt = entity;
      if (this._TemporaryReceipt?.tagArray)
        this.documentTags = this._TemporaryReceipt?.tagArray



      this.ServiceAPI.getInvoiceAllStatus().then(res => {

        var ss = this.ServiceAPI.ReceiptStatus.find(a => a.id == entity.codeVoucherGroupId)?.uniqueName;
        this.codeVocher = ss != undefined ? ss : '';

      })


    }
    this.pageMode = PageModes.Update;
    this.pageModeTypeUpdate = true;
    this.disableControls();
  }

  //----------------جستجو درخواست------------------------------------------------
  async getRequest(type: any) {
    if (this.form.controls.requestNumber.value != undefined) {
      if (type === 'erp') {
        this.getRequestErpDetails();
      }
      else {
        this.getRequestDetails();
      }

    }
    else {
      this.Service.showHttpFailMessage('شماره درخواست خرید وارد نمایید');
    }

  }
  //====================جستجو درخواست=======================================

  async getRequestDetails() {
    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار تحویل گیرنده را انتخاب نمایید');
      return;

    }
    await this._mediator.send(new GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery(this.form.controls['requestNumber']?.value, this.form.controls.warehouseId.value)).then(async (res) => {

      this.initializeResponse(res);
    })
  }
  //-----------------------------جستجو در سیستم آرانی------------------------------------
  async getRequestErpDetails() {

    await this._mediator.send(new getPurchaseRequestByIdQuery(this.form.controls['requestNumber']?.value)).then(async (res) => {
      this.SetTempValue();
      this.initializeResponse(res);
      this.GetTempValue()


    })

  }
  //==========================================================================
  async initializeResponse(res: any) {

    
    this._TemporaryReceipt = res;
    let response = res;
    let newRequest = new AddReceiptCommand().mapFrom(response)
    newRequest.requestNumber = this._TemporaryReceipt?.requestNo;
    newRequest.codeVoucherGroupId = this._TemporaryReceipt?.codeVoucherGroupId;

    this.request = newRequest;

    this.form.controls['documentNo'].setValue(undefined);
    this.form.controls['codeVoucherGroupId'].setValue(newRequest.codeVoucherGroupId);
    this.form.controls['documentDescription'].setValue(response.documentDescription);
   


    this.disableControls()
  }
  //-------------------------------------------------------------------------

  disableControls() {
    this.form.controls['creator'].disable();
    this.form.controls['documentNo'].disable();
    this.form.controls['requesterReferenceTitle'].disable();
    this.form.controls['followUpReferenceTitle'].disable();

    this.form.controls['requestNumber']?.value
    if ((this.getQueryParam('EditType') == '1')) {
      //ویرایش اموال
      this.EditType = 1;
      this.form.disable();
    }
    if ((this.getQueryParam('EditType') == '2')) {
      //ویرایش تاریخ
      this.EditType = 2;
      this.form.disable();
      this.form.controls['documentDate'].enable();
    }
    if ((this.getQueryParam('EditType') == '3')) {
      //ویرایش نوع سند
      this.EditType = 3;
      this.form.disable();
      this.form.controls['codeVoucherGroupId'].enable();
    }
    if ((this.getQueryParam('EditType') == '4')) {
      //ویرایش شرح کالا
      this.EditType = 4;
      this.form.disable();
     
      var arrayControl: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];
      arrayControl.controls.forEach(res => {

        var formItem: FormGroup = <FormGroup>res;
        formItem.controls['description'].enable();

      })
    }
    if ((this.getQueryParam('EditType') == '5')) {

      //ویرایش  کالا
      this.EditType = 5;
      this.form.disable();

      
    }
    if ((this.getQueryParam('EditType') == '6')) {
      //ویرایش  انبار
      this.EditType = 6;
      this.form.disable();
      this.form.controls['warehouseId'].enable();

    }
  }
  
  async add() {
    
    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار تحویل گیرنده را انتخاب نمایید');
      return;

    }
    if (this.form.controls.codeVoucherGroupId.value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را انتخاب نمایید');
      return;

    }
    if (this.form.controls.documentDate.value == undefined) {

      this.Service.showHttpFailMessage('تاریخ رسید را انتخاب کنید');
      return;

    }
   
    //--------------------------------------------------
    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);

    this.form.controls.documentStauseBaseValue.setValue(DocumentState.Temp);

    let response = await this._mediator.send(<AddReceiptCommand>this.request);

    this.isSubmitForm = true;
    this.form.controls['documentNo'].setValue(response.documentNo);
    this.form.controls['requestNumber'].setValue(undefined);
   
  }

  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);

  }

  ReferenceSelect(item: any) {

    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {

    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    this.codeVocher = item?.uniqueName;

    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }
  }
  invoiceNoSelect(invoice: any) {

    //-----------درحالتی که از لیست پیشنهادی قراردادها یا پیش فاکتورهای انتخاب شود
    if (invoice?.documentNo != undefined) {
      this.form.controls.invoiceNo.setValue(invoice?.documentNo);
      this.form.controls.creditAccountReferenceId.setValue(invoice?.creditAccountReferenceId);
      this.form.controls.creditAccountReferenceGroupId.setValue(invoice.creditAccountReferenceGroupId);
      var invoiceItem = invoice?.items;

      var arrayControl: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];
      arrayControl.controls.forEach(res => {


        var formItem: FormGroup = <FormGroup>res;
        var item = invoiceItem.find((a: { commodityId: any; }) => a.commodityId == formItem.controls.commodityId.value);
        if (item != undefined) {
          formItem.controls.unitPrice.setValue(item.unitPrice);
          formItem.controls.unitBasePrice.setValue(item.unitPrice);
        }
      })
    }
    else {//در حالتی که روی کد خود input چیزی تایپ شود.
      this.form.controls.invoiceNo.setValue(invoice)
    }

  }

  TagSelect(tagstring: string[]) {

    this.documentTags = tagstring;


  }

  async navigateToReceiptList() {

    await this.router.navigateByUrl('inventory/temporaryReceiptList')

  }
  async reset() {
    await this.deleteQueryParam("id")
    this.pageModeTypeUpdate = false;
    this.isSubmitForm = false
    this.SetTempValue();
    super.reset();
    this.GetTempValue()



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
  async get(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
  }
  
  async update() {
   
    
    //--------------------------------------------------
    var tagstring: string = await this.Service.TagConvert(this.documentTags);
    this.form.controls.tags.setValue(tagstring);

    let response = await this._mediator.send(<UpdateTemporaryReceiptCommand>this.request);
    
    return await this.initialize(response);
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

}
