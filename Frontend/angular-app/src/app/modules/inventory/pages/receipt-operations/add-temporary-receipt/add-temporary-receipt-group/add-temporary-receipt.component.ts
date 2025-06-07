
import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Warehouse } from '../../../../entities/warehouse';
import { Receipt } from '../../../../entities/receipt';
import { GetTemporaryRecepitQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-query';
import { Form, FormArray, FormGroup } from '@angular/forms';
import { AddItemsCommand } from '../../../../repositories/receipt/commands/receipt-items/add-receipt-items-command';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { ReceiptAllStatusModel } from '../../../../entities/receipt-all-status';
import { GetTemporaryRecepitByDocumentNoQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-documentId-query';
import { MatDialog, MatDialogConfig, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AddGeroupTemporaryReceiptCommand } from '../../../../repositories/receipt/commands/temporary-receipt/add-group-temporary-receipt-command';
import { getPurchaseRequestByIdQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-arani-request-query';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { TemporaryReceiptItemsComponent } from './temporary-receipt-items/temporary-receipt-items.component';
import { GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery } from '../../../../repositories/receipt/queries/temporary-receipt/get-temporary-receipt-by-warehouseId-query';

@Component({
  selector: 'app-add-temporary-receipt',
  templateUrl: './add-temporary-receipt.component.html',
  styleUrls: ['./add-temporary-receipt.component.scss']
})
export class AddTemporaryReceiptComponent extends BaseComponent {

  public rowCount: number = 1;
  child: any;
  @ViewChild(TemporaryReceiptItemsComponent)
  set appShark(child: TemporaryReceiptItemsComponent) {
    this.child = child
  };

  documentTags: string[] = [];
  warehouses: Warehouse[] = [];
  public _TemporaryReceipt: Receipt | undefined = undefined;
  public PublicpurchaseRequest = new AddGeroupTemporaryReceiptCommand()
  public publicreceiptDocumentItems: AddItemsCommand[] = [];

  public countRowsSave: number = 0;
  public isSubmit: boolean = false;
  public codeVoucherGroup: string = "";//برای ارسال به  کامچونت فرزند
  public temp_warehouseId: any;
  public temp_codeVoucherGroupId: any;
  public temp_documentDate: any;



  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    public Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,

  ) {
    super(route, router);
    this.request = new AddGeroupTemporaryReceiptCommand()

  }


  async ngOnInit() {

    await this.resolve()
  }


  async resolve(params?: any) {
    this.formActions = [
      FormActionTypes.add,
      FormActionTypes.list,
    ];
    this.request = new AddGeroupTemporaryReceiptCommand();
    await this.initialize()
  }

  async initialize() {



  }
  //--------------------------------------------------------------------------------
  //----------------جستجو درخواست---------------------------------------------------
  async getRequest(type: any) {



    //--------------شماره درخواست تکراری زده نشود--
    var duplicateRequestNo: boolean = false;
    for (let item of this.form.controls['receiptDocumentItems']?.controls) {
      if (item.controls.requestNo.value == this.form.controls.requestNumber.value) {
        duplicateRequestNo = true;
      }
    }
    if (this.form.controls.requestNumber.value != undefined && duplicateRequestNo) {
      this.Service.showWarrningMessage('این شماره درخواست قبلا جستجو شده است');
    }
    if (this.form.controls.requestNumber.value == undefined) {

      this.Service.showHttpFailMessage('شماره درخواست خرید را وارد نمایید');

    }
    if (this.form.controls.codeVoucherGroupId.value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را بدرستی انتخاب نمایید');
      return;

    }
    if (this.form.controls.documentDate.value == undefined) {

      this.Service.showHttpFailMessage('تاریخ رسید را انتخاب نمایید');
      return;

    }
    if (this.form.controls.warehouseId.value == undefined) {

      this.Service.showHttpFailMessage('انبار را انتخاب نمایید');
      return;

    }
    else {
      this.SetTempValue();
      if (type === 'erp') {
        this.getRequestErpDetails().then(q => {
          this.GetTempValue();

        });
      }
      else {
        this.getRequestDetails().then(q => {
          this.GetTempValue();
        });
      }

    }
  }
  //-----------------------------جستجو درخواست -----------------------------------------
  async getRequestDetails() {



    await this._mediator.send(new GetTemporaryRecepitByDocumentWithWarehouseIdNoQuery(this.form.controls['requestNumber']?.value, this.form.controls['warehouseId']?.value)).then(async (res) => {

      if (res.items.length == 0) {
        this.Service.showHttpFailMessage('کالا مربوط به این درخواست در سیستم ثبت نشده است ، ابتدا کالا را ثبت نمایید.')
      }

      this.initializeResponse(res);

    })

  }
  //-----------------------------جستجو در سیستم آرانی------------------------------------
  async getRequestErpDetails() {



    await this._mediator.send(new getPurchaseRequestByIdQuery(this.form.controls['requestNumber']?.value)).then(async (res) => {

      if (res.items.length == 0) {
        this.Service.showHttpFailMessage('کالا مربوط به این درخواست در سیستم ثبت نشده است ، ابتدا کالا را ثبت نمایید.')
      }

      this.initializeResponse(res);

    })

  }
  async initializeResponse(purchaseRequestResponse: any) {

    var referenceId: number | undefined = undefined;
    var accountReferencesGroupId: number | undefined = undefined;
    var invoiceNo: string | undefined = undefined;


    var purchaseRequest = new AddGeroupTemporaryReceiptCommand().mapFrom(purchaseRequestResponse)



    //-------------------بدست آوردن آیدی آخرین تامین کننده ثبت شده------------------------
    if (this.formItems.length >= 1) {


      var Index = 0;
      var arrayControl = this.form.get('receiptDocumentItems') as FormArray;
      var lastControls = arrayControl.at(Index) as FormGroup;

      referenceId = lastControls.controls['creditAccountReferenceId'].value;

      accountReferencesGroupId = lastControls.controls['creditAccountReferenceGroupId'].value;
      invoiceNo = lastControls.controls['invoiceNo'].value


    }
    //-----------------افزودن اطلاعات تامین کننده و شماره فاکتور به صورت پیش فرش---------
    let index = 0
    purchaseRequest.receiptDocumentItems.forEach(a => {
      a.selected = false;
      a.invoiceNo = a.invoiceNo == undefined ? invoiceNo : a.invoiceNo;
      a.id = this.formItems.length + index;
      a.requestDate = purchaseRequestResponse.requestDate;
      a.creditAccountReferenceGroupId = Number(accountReferencesGroupId);
      a.requesterReferenceId = purchaseRequestResponse.requesterReferenceId;
      a.followUpReferenceId = purchaseRequestResponse.requesterReferenceId;
      a.requesterReferenceTitle = purchaseRequestResponse.requesterReferenceTitle;
      a.creditAccountReferenceId = referenceId != undefined ? Number(referenceId) : undefined;

      index++;

    })

    //-------------سرچ های بعدی---------------------------------------------------------
    if ((<AddGeroupTemporaryReceiptCommand>this.request).requestId) {
      purchaseRequest.receiptDocumentItems.forEach(item => {

        var ff: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];

        ff.insert(0, this.createForm(item, true));
        this.child.AddItems();
      })

    }

    //--------------اولین سرچ----------------------------------------------------------
    else {
      let newRequest = purchaseRequest;

      newRequest.requestNumber = this.form.controls['requestNumber']?.value;
      newRequest.creator = this.identityService.applicationUser.fullName;

      this.request = newRequest;
      this.PublicpurchaseRequest = purchaseRequest;
    }
  }


  get formItems(): FormArray {

    return this.form.get('receiptDocumentItems') as FormArray;
  }
  //================================================================
  //---------------------ذخیره سازی---------------------------------
  async add() {

    if (this.form.controls['warehouseId'].value == undefined) {

      this.Service.showHttpFailMessage('انبار مورد نظر را انتخاب نمایید');
      return;

    }
    if (this.form.controls['codeVoucherGroupId'].value == undefined) {

      this.Service.showHttpFailMessage('نوع رسید را بدرستی انتخاب نمایید');
      return;

    }

    for (let item of this.form.controls['receiptDocumentItems']?.controls) {

      if (item.controls.selected.value != true) {
        var receiptDocumentItems = new AddItemsCommand();
        receiptDocumentItems.assets = item.controls.assets.value;
        receiptDocumentItems.quantity = item.controls.quantity.value;
        receiptDocumentItems.requestNo = item.controls.requestNo.value;
        receiptDocumentItems.invoiceNo = item.controls.invoiceNo.value;
        receiptDocumentItems.commodityId = item.controls.commodityId.value;
        receiptDocumentItems.requestDate = item.controls.requestDate.value;
        receiptDocumentItems.mainMeasureId = item.controls.mainMeasureId.value;
        receiptDocumentItems.isWrongMeasure = item.controls.isWrongMeasure.value;
        receiptDocumentItems.documentMeasureId = item.controls.documentMeasureId.value;
        receiptDocumentItems.followUpReferenceId = item.controls.followUpReferenceId.value;
        receiptDocumentItems.requesterReferenceId = item.controls.requesterReferenceId.value;
        receiptDocumentItems.creditAccountReferenceId = item.controls.creditAccountReferenceId.value;
        receiptDocumentItems.creditAccountReferenceGroupId = item.controls.creditAccountReferenceGroupId.value;
        receiptDocumentItems.quantityChose = item.controls.quantityChose.value;
        receiptDocumentItems.description = item.controls.description.value;
        

        let letRequest = new AddGeroupTemporaryReceiptCommand();
        letRequest.warehouseId = this.form.controls['warehouseId'].value;
        letRequest.documentDate = this.form.controls['documentDate'].value;
        letRequest.requestNumber = this.form.controls['requestNumber']?.value;
        letRequest.codeVoucherGroupId = this.form.controls['codeVoucherGroupId']?.value;
        letRequest.documentDescription = this.form.controls['documentDescription']?.value;
        letRequest.partNumber = this.form.controls['partNumber']?.value;
        letRequest.isDocumentIssuance = this.form.controls['isDocumentIssuance']?.value;
        letRequest.receiptDocumentItems.push(receiptDocumentItems);


        var tagstring: string = await this.Service.TagConvert(this.documentTags);
        letRequest.tags = tagstring;

        this.isSubmit = true

        //---------------------ثبت-------------------------------------
        await this._mediator.send(<AddGeroupTemporaryReceiptCommand>letRequest).then(a => {
          item.controls.selected.setValue(true);
          letRequest.id = a.id;
          item.controls.documentHeadId.setValue(a.id);
          item.controls.documentNo.setValue(a.documentNo);
          this.countRowsSave = this.countRowsSave + 1;
          this.isSubmit = false;

        });

      }

    }//End For
  }

  WarehouseIdSelect(item: Warehouse) {

    this.form.controls.warehouseId.setValue(item?.id);

  }
  codeVoucherGroupSelect(item: ReceiptAllStatusModel) {
    this.form.controls.codeVoucherGroupId.setValue(item?.id);
    this.codeVoucherGroup = item.uniqueName;

    if (item?.id == undefined) {
      this.WarehouseIdSelect(new Warehouse());
    }
  }
  async navigateToReceiptList() {

    await this.router.navigateByUrl('/inventory/temporaryReceiptList')
  }
  async reset() {

    this.countRowsSave = 0;
    this.publicreceiptDocumentItems = [];
    this.SetTempValue();
    super.reset();
    this.GetTempValue()

    this.isSubmit = false;

  }
  SetTempValue() {

    this.temp_warehouseId = this.form.controls['warehouseId'].value;
    this.temp_documentDate = this.form.controls['documentDate'].value;
    this.temp_codeVoucherGroupId = this.form.controls['codeVoucherGroupId'].value;

  }
  GetTempValue() {

    this.form.controls.requestNumber.setValue(undefined)
    this.form.controls['warehouseId'].setValue(this.temp_warehouseId);
    this.form.controls['documentDate'].setValue(this.temp_documentDate);
    this.form.controls['codeVoucherGroupId'].setValue(this.temp_codeVoucherGroupId);

  }
  async get(Id: number) {
    return await this._mediator.send(new GetTemporaryRecepitQuery(Id))
  }

  TagSelect(tagstring: string[]) {

    this.documentTags = tagstring;


  }
  async print() {


      let printContents = '';
     let i = 1;
      if (this.form.controls['receiptDocumentItems']?.controls.length > 0) {

        printContents += `<table><thead>
                     <tr>
                       <th>ردیف</th>
                       <th>شماره رسید</th>
                       <th>شماره درخواست</th>
                       <th>تاریخ</th>
                       <th>نام کالا</th>
                       <th>کد کالا</th>
                       <th>مقدار</th>
                       <th>شماره صورتحساب</th>

                     </tr>
                   </thead><tbody>`;
        for (let data of this.form.controls['receiptDocumentItems']?.controls) {

          printContents += `<tr>
                           <td>${i}</td>
                           <td>${data.controls.documentNo.value}</td>
                           <td>${data.controls.requestNo.value}</td>
                           <td>${this.Service.toPersianDate(this.form.controls.documentDate.value)}</td>
                           <td>${data.controls.commodityTitle.value}</td>
                           <td>${data.controls.commodityCode.value}</td>
                           <td>${data.controls.quantity.value}</td>
                           <td>${data.controls.invoiceNo.value}</td>

                        </tr>`;
          i++;
          }
          printContents += '</tbody></table>'
          this.Service.onPrint(printContents, 'رسیدهای موقت گروهی')
        }


  }

    close(): any {
    }

    delete (param ?: any): any {

    }

  async update() {

    }


  }
