import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../identity/repositories/identity.service";
import { Receipt } from '../../../entities/receipt';
import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';
import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { UploadFileData } from '../../../../../core/components/custom/uploader/uploader.component';
import { environment } from 'src/environments/environment';
import { GetRecepitAttachmentsQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-attachment-query';
import { GetCorrectionRequestByIdQuery } from '../../../repositories/receipt/queries/receipt/get-correction-request-by-Id-query';
import { CorrectionRequest } from '../../../entities/CorrectionRequest';

import { UpdateQuantityCommand } from '../../../repositories/receipt/commands/reciept/update-quantity-command';
import { ApiCallService } from '../../../../../shared/services/pages/api-call/api-call.service';
import { SearchQuery } from '../../../../../shared/services/search/models/search-query';
import { GetRecepitGetByDocumentIdQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-by-documentId-query';
import { MechanizedDocumentEditingCommand } from '../../../repositories/receipt/commands/reciept/mechanized-document-editing-command';
import { UpdateCorrectionRequestCommand } from '../../../repositories/receipt/commands/reciept/update-correction-request';
import { ConvertToRialsReceiptAfterDocumentCommand } from '../../../repositories/receipt/commands/reciept/convert-Rials-receipt-after-document-command';
import { BulkVoucherStatusUpdateCommand } from '../../../../accounting/repositories/voucher-head/commands/bulk-voucher-status-update-command';
import { GetAllReceiptGroupbyInvoiceQuery } from '../../../repositories/reports/get-receipts-groupby-invoice-query';
import { UpdateAvgPriceAfterChangeBuyPriceCommand } from '../../../repositories/receipt/commands/reciept/update-avg-price-after-change-buy-price-command';


@Component({
  selector: 'app-mechanized-document_confirm_edit',
  templateUrl: './mechanized-document_confirm_edit.component.html',
  styleUrls: ['./mechanized-document_confirm_edit.component.scss']
})
export class MechanizedDocumentConfirmEditComponent extends BaseComponent {



  public Receipts: Receipt[] = [];
  public displayPage: string = "";
  public isSubmitForm: boolean = false;
  public receipt: Receipt | undefined = undefined;
  public payLoad = new ConvertToRialsReceiptAfterDocumentCommand();
  public oldPayLoad = new ConvertToRialsReceiptAfterDocumentCommand();
  public correctionRequest: CorrectionRequest | undefined = undefined;
  public messsage = ''
  public editType: number | undefined = undefined



  //------------Attachment--------------------------

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

  constructor(
    private router: Router,
    private _mediator: Mediator,
    private route: ActivatedRoute,
    private Service: PagesCommonService,
    private ApiCallService: ApiCallService,
    private identityService: IdentityService,
    public _notificationService: NotificationService,


  ) {
    super(route, router);
    this.request = new ConvertToRialsReceiptAfterDocumentCommand()



  }

  async ngOnInit() {
    await this.resolve()
  }

  //---------------attachment------------------------
  async getAttachments() {
    await this._mediator.send(new GetRecepitAttachmentsQuery(
      Number(this.receipt?.id),
    )).then(res => {
      this.attachmentIds = res;
    })
  }

  async resolve(params?: any) {


    await this.initialize()

  }

  async initialize(entity?: any) {

    await this.getCorrectionRequestById().then(async res => {
      this.correctionRequest = res;
      this.messsage = this.correctionRequest.verifierDescription;
      this.payLoad = <ConvertToRialsReceiptAfterDocumentCommand>JSON.parse(this.correctionRequest?.payLoad);

      this.oldPayLoad = <ConvertToRialsReceiptAfterDocumentCommand>JSON.parse(this.correctionRequest?.oldData);
      this.request = new ConvertToRialsReceiptAfterDocumentCommand().mapFrom(JSON.parse(this.correctionRequest?.payLoad));


      await this.get(Number(this.correctionRequest.documentId)).then(async a => {
        this.receipt = a;


        this.getAttachments();
        await this.onComputing(this.oldPayLoad);
        await this.onComputing(this.payLoad);

        this.editType = this.payLoad.editType
        setTimeout(() => {
          this.form.controls.vatDutiesTax.setValue(this.payLoad.vatDutiesTax);
          this.form.controls.totalItemPrice.setValue(this.payLoad.totalItemPrice);
          this.form.controls.totalProductionCost.setValue(this.payLoad.totalProductionCost);
          this.form.controls.correctionRequestId.setValue(this.getQueryParam('tableId'));
        }, 5)
      });


    });



  }


  async get(Id: number) {
    return await this._mediator.send(new GetRecepitGetByDocumentIdQuery(Id))

  }
  async getCorrectionRequestById() {
    return await this._mediator.send(new GetCorrectionRequestByIdQuery(this.getQueryParam('tableId')))

  }


  async update() {

    //if (this.isSubmitForm == false) {

    //  await this._mediator.send(<ConvertToRialsReceiptAfterDocumentCommand>this.request).then(res => {

    //    this.isSubmitForm = true;
    //    var i: number = 0
    //    this.payLoad.receiptDocumentItems.forEach(async (item: any) => {

    //      var q = this.receipt?.items[i].quantity;

    //      //اگر تعداد کالا تغییر کرده باشد ، برود استاک انبار و بقیه چیزهای مربوط را درست کند.
    //      if (Number(q) != item.quantity) {
    //        this.UpdateQuantity(item);
    //      }
    //      i++;

    //    })
    //    if (i == this.payLoad.receiptDocumentItems.length) {

    //      //بروزرسانی و گرفتن اطلاعات جدید
    //      this.getListInvoice();

    //    }
    //  });
    //}

    //سند را باز و بسته کردن جهت ویرایش
    let requestBulkVoucher = new BulkVoucherStatusUpdateCommand();

    requestBulkVoucher.status = 1;//('unlock') = 1;
    let voucherIds: number[] = [this.receipt?.voucherHeadId];
    requestBulkVoucher.voucherIds = voucherIds;

    await this._mediator.send(requestBulkVoucher).then(async () => {

      await this._mediator.send(<ConvertToRialsReceiptAfterDocumentCommand>this.request).then(async () => {

        requestBulkVoucher.status = 3 //('lock') = 3;
        await this._mediator.send(requestBulkVoucher).then(a => {
          let UpdateAvgPrice = new UpdateAvgPriceAfterChangeBuyPriceCommand();
          UpdateAvgPrice.documentId = this.receipt?.documentId;

          this._mediator.send(<UpdateAvgPriceAfterChangeBuyPriceCommand>UpdateAvgPrice);
        })



      });


    })


  }
  async UpdateQuantity(item: any) {
    var request = new UpdateQuantityCommand();
    request.id = item.id;
    request.quantity = item.quantity;
    await this._mediator.send(<UpdateQuantityCommand>request);
  }

  //-----------------محاسبه قیمت های کل--------------------------
  onComputing(payLoad: ConvertToRialsReceiptAfterDocumentCommand) {


    payLoad.totalItemPrice = 0;
    payLoad.totalProductionCost = 0;

  
    payLoad.receiptDocumentItems.forEach((item: any) => {
     
     
      
      payLoad.totalProductionCost = Number(payLoad.totalProductionCost) + Number(item.productionCost);
      console.log('payLoad.totalProductionCost', payLoad.totalProductionCost)

    })

    let vat = this.receipt?.vatPercentage;
    if (payLoad.vatDutiesTax) {
      payLoad.vatDutiesTax = Math.trunc((Number(vat) * Number(payLoad.totalProductionCost)) / 100);
    }

    payLoad.totalItemPrice = Number(payLoad.totalProductionCost) + Number(payLoad.vatDutiesTax);

    if (payLoad.isFreightChargePaid == true) {
      payLoad.totalItemPrice = payLoad.totalItemPrice + Number(payLoad.extraCost);
     
    }

  }
  extraCostAccountReferenceSelect(item: any) {

    this.form.controls.extraCostAccountReferenceId.setValue(item?.id);
    this.form.controls.extraCostAccountReferenceGroupId.setValue(item.accountReferenceGroupId);

  }
  async getListInvoice() {

    let searchQueries: SearchQuery[] = []
    let request = new GetAllReceiptGroupbyInvoiceQuery(
      undefined,
      undefined,
      this.getQueryParam('id'),
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      0,
      0,
      searchQueries, '')

    await this._mediator.send(request).then(res => {
      this.Receipts = [];
      this.Receipts = res.data;

      this.ApiCallService.updateCreateAddAutoVoucher2(this.Receipts, 'edit').then(async r => {

        await this.updateCorrectionRequest(1)
        this.receipt = await this.get(Number(this.correctionRequest?.documentId));
      });
    });


    //searchQueries.push(new SearchQuery({
    //  propertyName: "VoucherHeadId",
    //  values: [this.receipt?.voucherHeadId],
    //  comparison: 'equal',
    //  nextOperand: "and"
    //}))

    //let request = new GetAllReceiptGroupbyInvoiceQuery(
    //  new Date(new Date(<Date>(this.receipt?.documentDate)).setHours(0, 0, 0, 0)),
    //  new Date(new Date(<Date>(this.receipt?.documentDate)).setHours(0, 0, 0, 0)),
    //  undefined,
    //  0,
    //  25,
    //  searchQueries, '')
    //let response = await this._mediator.send(request);

    //this.Receipts = response.data;

  }
  async updateCorrectionRequest(status: number) {
    var request = new UpdateCorrectionRequestCommand();
    request.documentId = Number(this.correctionRequest?.documentId);
    request.status = status;
    await this._mediator.send(<UpdateCorrectionRequestCommand>request);
  }
  async navigateToVoucher() {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${this.receipt?.voucherHeadId}`)
  }
  add() {

  }
  async edit() {
  }

  async reset() {

  }
  close(): any {
  }

  delete(param?: any): any {
  }

}
