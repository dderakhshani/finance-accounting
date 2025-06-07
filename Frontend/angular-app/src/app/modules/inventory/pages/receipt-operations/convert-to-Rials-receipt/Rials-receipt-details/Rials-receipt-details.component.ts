import { Component, ElementRef, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { environment } from 'src/environments/environment';
import { Receipt } from '../../../../entities/receipt';
import { ActivatedRoute, Router } from "@angular/router";
import { PageModes } from "../../../../../../core/enums/page-modes";
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../../core/constants/form-action-types";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { RialsReceiptItemsComponent } from './Rials-receipt-items/Rials-receipt-items.component';
import { NotificationService } from '../../../../../../shared/services/notification/notification.service';
import { AttachmentsModel, UploadFileData } from '../../../../../../core/components/custom/uploader/uploader.component';
import {
  ConfirmDialogComponent,
  ConfirmDialogIcons
} from '../../../../../../core/components/material-design/confirm-dialog/confirm-dialog.component';
import {
  ConvertToRialsReceiptCommand
} from '../../../../repositories/receipt/commands/reciept/convert-Rials-receipt-command';
import {
  GetRecepitAttachmentsQuery
} from '../../../../repositories/receipt/queries/receipt/get-receipt-attachment-query';
import { GetRecepitQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-query';
import { GetRecepitListIdQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-list-Id-query';
import { GetCostImportReceiptsQuery } from '../../../../repositories/receipt/queries/receipt/get-cost-import-receipts';
import {
  GetRecepitGetByDocumentIdQuery
} from '../../../../repositories/receipt/queries/receipt/get-receipt-by-documentId-query';
import {
  DocumentHeadExtraCostDialogComponent
} from '../../../component/documents-extra-cost-dialog/documents-extra-cost-dialog.component';
import {
  GetTotalExtraCostQuery
} from '../../../../repositories/documentHeadExtraCost/queries/get-total-extera-Costs-query';
import {
  ComboAddSelectBaseValueComponent
} from '../../../component/combo-add-select-base-value/combo-add-select-base-value.component';
import * as XLSX from 'xlsx';
import {
  UpdateAvgPriceAfterChangeBuyPriceCommand
} from '../../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';

import { RialsDebitDetailsDialogComponent } from "./Rials-debit-details-dialog/Rials-debit-details-dialog.component";


@Component({
  selector: 'app-Rials-receipt-details-receipt',
  templateUrl: './Rials-receipt-details.component.html',
  styleUrls: ['./Rials-receipt-details.component.scss'],

})
export class RialsReceiptDetailsComponent extends BaseComponent {

  child: any;

  @ViewChild(RialsReceiptItemsComponent) set appShark(_child: RialsReceiptItemsComponent) {
    this.child = _child
  };

  public childDescriptin: any;

  @ViewChild(ComboAddSelectBaseValueComponent) set appDescriptin(_childDescriptin: ComboAddSelectBaseValueComponent) {
    this.childDescriptin = _childDescriptin
  };

  isSingleReceipt: number = this.Service.ListId.length;
  allowNotEditInvoice: boolean = false;
  IsDisableDescriptin: boolean = false;
  editType: string = this.getQueryParam('editType');
  isImportPurchase = this.getQueryParam('isImportPurchase')
  title: string = '';
  calculationType: number = this.isImportPurchase=='true' ?101:100;
  //محاسبات سیستمی انجام شود=100
  //101=محاسبات دستی انجام شود
  public ListId: string[] = [];



  //-----------------Tag----------------------------

  documentTags: string[] = [];
  //------------Attachment--------------------------
  attachmentAssets: number[] = [];
  public attachmentsModel: AttachmentsModel = {
    typeBaseId: this.Service.AttachmentReceipt100,
    title: 'رسید ریالی انبار',
    description: 'رسید ریالی انبار',
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

  //--------------------------------------------------

  public receipt: Receipt | undefined = undefined;

  //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
  //در حالتی که از سیستم آرانی درخواست ها خوانده شود و در سیستم ایفا ثبت نشده باشد.
  public commodityId: number | undefined = undefined;

  //----------------------------------------------------


  constructor(
    private router: Router,
    public dialog: MatDialog,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private Service: PagesCommonService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {
    super(route, router);
    this.request = new ConvertToRialsReceiptCommand()
  }


  async ngOnInit() {


    await this.resolve()
  }

  async resolve(params?: any) {
    this.formActions = [

      FormActionTypes.save,
      FormActionTypes.refresh
    ];
    this.ListId = this.Service.ListId;
    await this.initialize()
    this.Service.ListId = [];


  }

  //---------------attachment------------------------
  async getAttachments() {

    await this._mediator.send(new GetRecepitAttachmentsQuery(
      Number(this.receipt?.id),
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

  //--------------------------------------------------
  async initialize(entity?: any) {


    await this.getData().then(response => {
      this.request = new ConvertToRialsReceiptCommand().mapFrom(response);
      this.receipt = response;
      this.pageMode = PageModes.Update;

    });
    await this.getAttachments();

    this.setChildValue();


  }

  setChildValue() {
    if (this.receipt?.tagArray)
      this.documentTags = this.receipt?.tagArray
    this.getExteraCostRial();
    if (this.receipt != undefined) {

      setTimeout(() => {
        //پرکردن دیتا ها جهت ارسال به کامپونت فرزند

        this.child.vatDutiesTax = Number(this.receipt?.vatDutiesTax == undefined ? 0 : this.receipt?.vatDutiesTax);
        this.child.totalItemPrice = Number(this.receipt?.totalItemPrice == undefined ? 0 : this.receipt?.totalItemPrice);
        this.child.totalProductionCost = Number(this.receipt?.totalProductionCost == undefined ? 0 : this.receipt?.totalProductionCost);
        this.child.vatPercentage = Number(this.receipt?.vatPercentage == undefined ? 0 : this.receipt?.vatPercentage);
        /* this.child.extraCost = Number(this.receipt?.extraCost == undefined ? 0 : this.receipt?.extraCost);*/
        this.child.voucherHeadId = Number(this.receipt?.voucherHeadId == undefined ? 0 : this.receipt?.voucherHeadId);
        this.child.isImportPurchase = this.receipt?.isImportPurchase;
        this.child.isNegative = Number(this.receipt?.extraCostCurrency) < 0 ? true : false;
        this.child.isVate = Number(this.receipt?.vatDutiesTax) > 0 ? true : false;
        this.child.isFreightChargePaid = this.receipt?.isFreightChargePaid;
        if (this.child.isNegative == true) {
          this.form.controls.extraCostCurrency.setValue(-1 * Number(this.receipt?.extraCostCurrency))

        }

        //---------------------------------
        //پر کردن اولین کالا برای اینکه بتوانیم پیش فاکتور آن را پیدا کنیم.
        this.commodityId = this.receipt?.items[0]?.commodityId;

        if (this.isImportPurchase == 'true') {
          /* this.child.onComputing('unitPrice');*/
          this.child.ImportPurchaseComputeCostRate();

        } else {
          this.child.ImportPurchaseComputeCostRial()
          // this.child.onComputing('productionCost');

        }

        this.form.controls['extraCost'].disable();

        if (this.receipt?.voucherHeadId > 0 || this.Service.identityService.doesHavePermission('allowNotEditInvoice')) {
          this.allowNotEditInvoice = true;
          this.IsDisableDescriptin = true;

        }
        this.form.controls.editType.setValue(this.editType);
        switch (this.editType) {
          case '1': {
            this.title = 'ویرایش تعداد کالا'
            this.onEditOuntity();
            break;
          }
          case '2': {
            this.title = 'ویرایش شرح'
            this.onEditDescription();
            break;
          }
          case '3': {
            this.title = 'ویرایش ریالی'
            this.onEditAmount();
            break;
          }
          case '4': {
            this.title = 'ویرایش هزینه ریالی'
            break;
          }
          default: {
            this.onEditble();
            break;
          }
        }


      }, 10);
      //------------------------End Timer
      
      if (!this.receipt?.extraCostAccountReferenceId) {
        this.form.controls.extraCostAccountReferenceId.setValue(32277);
        this.form.controls.extraCostAccountReferenceGroupId.setValue(53);
        this.form.controls.extraCostAccountHeadId.setValue(this.Service.extraCostAccountHeadId);
      }


    }
  }

  ImportPurchaseComputeCostRate() {
    this.child.ImportPurchaseComputeCostRate();
  }
  ImportPurchaseComputeCostRial() {
    this.child.ImportPurchaseComputeCostRial();
  }
  async getData() {

    if (this.getQueryParam('id')) {
      return await this.get(this.getQueryParam('id'));
    } else if (this.getQueryParam('documentId')) {

      return await this.getRecepitGetByDocumentIdQuery(this.getQueryParam('documentId'));
    } else {
      return await this.getListId(this.ListId);
    }

  }

  onEditDescription() {

    
    this.form.disable();
    this.form.controls.documentDescription.enable()

  }

  onEditble() {
    if (this.receipt?.voucherHeadId > 0) {
      
      this.form.disable();
    }
  }

  onEditOuntity() {
    this.form.disable();
   
    this.child.form.controls.forEach((control: any) => {

      (control as FormGroup).controls.quantity.enable();
    })


  }

  onEditAmount() {
    this.form.enable();
    this.allowNotEditInvoice = false;
  }

  async update() {

    if (!this.form.controls.debitAccountHeadId.value) {
      this.Service.showHttpFailMessage('سرفصل حساب بدهکار را وارد نمایید')
      return
    } else if (!this.form.controls.creditAccountHeadId.value) {
      this.Service.showHttpFailMessage('سرفصل حساب بستانکار را وارد نمایید')
      return
    } else if (this.form.controls.extraCost.value > 0 && this.isImportPurchase == 'false' && !this.form.controls.extraCostAccountReferenceGroupId.value && !this.form.controls.extraCostAccountHeadId.value) {
      this.Service.showHttpFailMessage('در صورت ورود هزینه اضافی سر فصل حساب های مرتبط را انتخاب نمایید')
      return
    }
    let Warnning = this.onValidationPrice()

    if (Warnning != '') {
      await this.AlterWarnning(Warnning)

    } else {
      this.edit();
    }


  }

  async edit() {
    this.setAttachmentIds();
    var tagstring: string = await this.Service.TagConvert(this.documentTags);

    this.form.controls.tags.setValue(tagstring);
    this.form.controls.vatDutiesTax.setValue(this.child.vatDutiesTax);
    this.form.controls.totalItemPrice.setValue(this.child.totalItemPrice);
    this.form.controls.totalProductionCost.setValue(this.child.totalProductionCost);
    this.form.controls.isFreightChargePaid.setValue(this.child.isFreightChargePaid);

    if (this.child.isNegative) {

      this.form.controls.isNegative.setValue(true);
    }
    if (this.child.isImportPurchase == true) {
      this.form.controls.extraCost.setValue(this.child.extraCostRialTemp);
    }

    if (this.form.controls.voucherHeadId.value == undefined || this.form.controls.voucherHeadId.value == 0) {
      await this._mediator.send(<ConvertToRialsReceiptCommand>this.request).then(response => {


        this.receipt?.items.forEach(item => {
          
          let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
          UpdateAvgPrice.documentItemId = item.id;
          this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
        });
        

      });

    } else {
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        data: {
          title: 'هشدار',
          message: `<li class="font-12" >
                    برای این رسیدهای سند مکانیزه صادر شده است بنابراین تغییرات وارد لیست درخواست تغییرات می شود
                    </li>
                    <li class="font-12">
                    پس از تایید کارشناس مربوطه ، تغییرات به صورت مکانیزه در سند حسابداری و در رسید های ریالی اعمال می گردد
                    </li>`,
          icon: ConfirmDialogIcons.warning,
          actions: {
            confirm: { title: 'درخواست تغییرات ارسال شود', show: true }, cancel: { title: 'ذخیره نشود', show: true }
          }
        }
      });
      dialogRef.afterClosed().subscribe(async res => {
        if (res == true) {
          await this._mediator.send(<ConvertToRialsReceiptCommand>this.request);
        }
      });
    }
    //برای سندهایی که قبلا به حسابداری ارسال شده اند مسیر فراخوانی ای پی آی متفاوت است داخل خود کامند مسیر مشخص شده است

  }

  async get(Id: number) {
    return await this._mediator.send(new GetRecepitQuery(Id))
  }

  async getListId(ListId: string[]) {


    var req = new GetRecepitListIdQuery();
    req.ListIds = ListId;

    return await this._mediator.send(<GetRecepitListIdQuery>req);

  }

  async getRecepitGetByDocumentIdQuery(documentId: number) {
    return await this._mediator.send(new GetRecepitGetByDocumentIdQuery(documentId))
  }

  TagSelect(tagstring: string[]) {
    this.documentTags = tagstring;
  }


  debitReferenceSelect(item: any) {

    this.form.controls.debitAccountReferenceId.setValue(item?.id);
    this.form.controls.debitAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);
  }
  financialOperationNumberSelect(item: any) {
    this.form.controls.financialOperationNumber.setValue(item?.paymentNumber);
  }

  async creditReferenceSelect(item: any) {
    this.form.controls.creditAccountReferenceId.setValue(item?.id);
    this.form.controls.creditAccountReferenceGroupId.setValue(item?.accountReferenceGroupId);


    this.getExteraCostCurrency();


  }

  async openDocumentHeadExtraCostDialog(item: any) {

    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      documentHeadIds: this.getListdocumentHeadIds()
    };

    let dialogReference = this.dialog.open(DocumentHeadExtraCostDialogComponent, dialogConfig);

    dialogReference.afterClosed().subscribe(({ response, pageMode }) => {

      this.onAddExteraCost();


    })

  }

  getListdocumentHeadIds(): number[] {
    var listIds: number[] = [];
    this.receipt?.items.forEach(item => {
      listIds.push(Number(item.documentHeadId))
    });

    return listIds;
  }

  async getExteraCostRial() {

    if (this.isImportPurchase == 'false') {
      let request = new GetTotalExtraCostQuery();
      request.listIds = this.getListdocumentHeadIds();

      let cost = await this._mediator.send(request);
      this.form.controls.extraCost.setValue(cost)
      this.child.extraCost = Number(cost);
      if (Number(cost) > 0 && this.receipt) {
        this.receipt.isFreightChargePaid = true;
        this.child.onComputing('unitPrice');
      }

    }
  }

  async getExteraCostCurrency() {

    if (this.isImportPurchase == 'true') {

      this._mediator.send(new GetCostImportReceiptsQuery(this.form.controls.creditAccountReferenceId.value)).then(res => {
        if (Number(res) > 0) {
          this.form.controls.extraCostRialTemp.setValue(res);

          this.child.extraCostRialTemp = res;
        } else {
          this.form.controls.extraCostRialTemp.setValue(this.form.controls.extraCost.value);
        }


      })

    }
  }

  debitAccountHeadIdSelect(item: any) {

    this.form.controls.debitAccountHeadId.setValue(item?.id);

  }

  creditAccountHeadIdSelect(item: any) {

    this.form.controls.creditAccountHeadId.setValue(item?.id);

  }

  extraCostAccountReferenceSelect(item: any) {

    this.form.controls.extraCostAccountReferenceId.setValue(item?.id);
    this.form.controls.extraCostAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }

  extraCostAccountHeadIdSelect(item: any) {

    this.form.controls.extraCostAccountHeadId.setValue(item?.id);

  }

  baseValue(item: any) {

    this.form.controls.documentDescription.setValue(item);
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

          if (invoice.vatDutiesTax > 0) {
            this.child.isVate = true;

          } else {
            this.child.isVate = false;
          }

        }
      })

    } else {//در حالتی که روی کد خود input چیزی تایپ شود.
      this.form.controls.invoiceNo.setValue(invoice)
    }

  }

  onAddExteraCost() {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'یادآوری',
        message: 'هزینه ها به قیمت تمام شده اضافه شود؟',
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async res => {

      this.child.isFreightChargePaid = res

      await this.getExteraCostRial();
      this.child.extraCost = this.form.controls.extraCost.value;
      await this.child.onComputing('unitPrice');
      this.update();
    });

  }

  onValidationPrice(): string {
    var arrayControl: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];
    let validate = '';
    arrayControl.controls.forEach(res => {
      var formItem: FormGroup = <FormGroup>res;
      let unitPrice = Number(formItem.controls.unitPrice.value);
      let basePrice = formItem.controls.unitBasePrice.value;
      let MaxunitPrice = (basePrice * 20 / 100) + basePrice
      let MinUnitPrice = basePrice - (basePrice * 20 / 100);
      if (basePrice > 0 && (unitPrice > MaxunitPrice || unitPrice < MinUnitPrice)) {
        validate += `<li class="font-12"> نرخ کالا  ${formItem.controls.commodityTitle.value} با نرخ تخمینی کالا تفاوت بسیار زیادی دارد
        </li>
        `
      }
    })
    return validate;
  }

  async AlterWarnning(warnning: String) {

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'هشدار نرخ کالا',
        message: warnning,
        icon: ConfirmDialogIcons.warning,
        actions: {
          confirm: { title: 'با همین شرایط ذخیره شود', show: true }, cancel: { title: 'ذخیره نشود', show: true }
        }
      }
    });
    dialogRef.afterClosed().subscribe(async res => {
      if (res == true) {
        this.edit();
      }
    });

  }


  async print() {

    let printContents = '';

    let tableContents = await this.tableContents();


    printContents += `<hr />
                      <div style="width:100%;display:flex;text-align:center;font-size:14px;">
                          <div style="width:20%;">
                              <p>تاریخ مالی :${this.Service.toPersianDate(this.form.controls.invoiceDate.value)}</p>
                              <p>شماره صورتحساب :${this.form.controls.invoiceNo.value != null ? this.form.controls.invoiceNo.value : ''}</p>
                          </div>
                         <div style="width:40%;">

                              <p>نوع عملیات :${this.receipt?.codeVoucherGroupTitle}</p>
                          </div>
                          <div style="width:40%;">
                              <p>طرف حساب :${this.receipt?.creditReferenceTitle}</p>

                          </div>
                      </div>
                    ${tableContents}
          <div style="margin-top:15px; width:100%;height:75px;display:flex;text-align:center;font-size:14px; border: solid;border-width: 1px;">
            <b style="margin:5px;">توضیحات  : </b>
            <b style="margin:5px;">${this.form.controls.documentDescription.value}</b>

          </div>
            <div style="margin-top:15px; width:100%;display:flex;text-align:center;font-size:14px;">
            <div style="width: 24%;height:75px;text-align:start; border:solid;border-width:1px; padding:15px;"><b>نام و امضا حسابدار انبار</b></div>
            <div style="width: 24%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا سرپرست انبار</b></div>
            <div style="width: 24%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا بازرگانی</b></div>
            <div style="width: 23%; height: 75px; text-align: start; border: solid; border-width: 1px; padding: 15px;"><b>نام و امضا اتوماسیون بایگانی</b></div>
        </div>
         `
    this.Service.onPrint(printContents, 'عملیات خرید و فروش')
  }

  async tableContents() {


    let printtable = `<table><thead>
                     <tr>

                        <th>ردیف</th>
                        <th>تاریخ</th>
                        <th>شماره رسید</th>
                        <th>کد کالا</th>
                        <th>نام کالا</th>
                        <th>مقدار</th>
                        <th>قیمت واحد</th>
                        <th>مبلغ کل</th>
                        <th>فی ارز</th>
                        <th>جمع ارزی</th>
                        <th>قیمت تمام شده</th>
                        <th>شماره درخواست</th>
                        <th>توضیحات</th>
                     </tr>
                   </thead><tbody>`;
    let i = 1;
    var arrayControl: FormArray = <FormArray>this.form.controls['receiptDocumentItems'];

    arrayControl.controls.forEach(res => {
      var formItem: FormGroup = <FormGroup>res;
      printtable += `<tr>
                           <td>${i}</td>
                           <td>${this.Service.toPersianDate(this.form.controls.invoiceDate.value)}</td>
                           <td>${formItem.controls.documentNo.value}</td>
                           <td>${formItem.controls.commodityCode.value}</td>
                           <td>${formItem.controls.commodityTitle.value}</td>
                           <td>${formItem.controls.quantity.value}</td>
                           <td>${formItem.controls.unitPrice.value > 0 ? (formItem.controls.unitPrice.value).toLocaleString() : ''}</td>
                           <td>${formItem.controls.productionCost.value > 0 ? (formItem.controls.productionCost.value).toLocaleString() : ''}</td>
                           <td>${formItem.controls.currencyPrice.value > 0 ? (formItem.controls.currencyPrice.value).toLocaleString() : ''}</td>
                           <td>${formItem.controls.sumCurrencyPrice.value > 0 ? (formItem.controls.sumCurrencyPrice.value).toLocaleString() : ''}</td>
                           <td>${formItem.controls.unitPriceWithExtraCost.value > 0 ? (formItem.controls.unitPriceWithExtraCost.value).toLocaleString() : ''}</td>
                           <td>${formItem.controls.requestNo.value}</td>
                           <td>${formItem.controls.description.value != undefined ? formItem.controls.description.value : ''}</td>

                        </tr>`
      i++;
    })
    var extraCostCurrency = (this.child.isNegative == false ? Number(this.form.controls.extraCostCurrency.value) : -1 * Number(this.form.controls.extraCostCurrency.value))
    var sum = extraCostCurrency + this.child.totalProductionCost;
    printtable += `<tr>
                        <td colspan="9">قیمت کل</td>
                        <td colspan="4">${sum.toLocaleString()}</td>

                  </tr>
                  </tbody></table>`
    return printtable
  }

  exportexcel(): void {
    const parser = new DOMParser();
    var fileName = 'ExcelSheet.xlsx';

    let element = this.tableContents().then(a => {
      var doc = parser.parseFromString(a, "text/html");


      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(doc);


      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');

      /* save to file */
      XLSX.writeFile(wb, fileName);
    });


  }

  async navigateToVoucher() {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${this.receipt?.voucherHeadId}`)
  }

  async filesByPaymentNumber() {
    await this.router.navigateByUrl(`inventory/FilesByPaymentNumber?PaymentNumber=${this.receipt?.financialOperationNumber}`)
  }
  OpenCostDetailList() {
    let dialogConfig = new MatDialogConfig();
    let creditAccountReferenceId = [];
    creditAccountReferenceId.push(this.form.controls.creditAccountReferenceId.value);
    dialogConfig.data = {
      ReferenceId1: creditAccountReferenceId,
    };
    let dialogRef = this.dialog.open(RialsDebitDetailsDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(res => {
      this.form.controls.extraCostRialTemp.setValue(res);
    })

  }

  add() {

  }

  async reset() {

  }

  close(): any {
  }

  delete(param?: any): any {
  }

  
}
