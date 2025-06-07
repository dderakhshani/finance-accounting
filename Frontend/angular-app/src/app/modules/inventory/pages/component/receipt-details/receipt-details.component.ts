import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../core/services/mediator/mediator.service";
import { FormActionTypes } from "../../../../../core/constants/form-action-types";
import { Receipt } from '../../../entities/receipt';

import { PagesCommonService } from '../../../../../shared/services/pages/pages-common.service';

import { NotificationService } from '../../../../../shared/services/notification/notification.service';
import { AttachmentsModel, UploadFileData } from '../../../../../core/components/custom/uploader/uploader.component';
import { environment } from 'src/environments/environment';

import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from '../../../../../core/enums/page-modes';
import { DocumentItemsBomDialog } from '../document-Items-bom-dialog/document-Items-bom-dialog.component';
import { FormAction } from '../../../../../core/models/form-action';
import { GetRecepitAttachmentsQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-attachment-query';
import { GetRecepitGetByDocumentIdQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-by-documentId-query';
import { GetByDocumentNoAndDocumentStauseBaseValueQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-by-documnetNo-baseValue-query';
import { GetRecepitQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-query';
import { GetRecepitListIdQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-list-Id-query';
import { GetReceiptsByInvoiceNoQuery } from '../../../repositories/receipt/queries/receipt/get-receipts-invoceNo-query';
import { GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery } from '../../../repositories/receipt/queries/receipt/get-receipt-by-documnetNo-query';
import { GetAssetsByDocumentIdQuery } from '../../../repositories/assets/queries/get-assets-by-documentId';
import { CommoditySerialViewDialog } from '../commodity-serial-view-dialog/commodity-serial-view-dialog.component';

@Component({
  selector: 'app-receipt-details-receipt',
  templateUrl: './receipt-details.component.html',
  styleUrls: ['./receipt-details.component.scss']
})
export class ReceiptDetailsComponent extends BaseComponent {


  public receipt: Receipt | undefined = undefined;
  public displayPage: string = "";
  public codeVoucherGroup: string = "";
  public panelOpenState: boolean = false;
  public documentItemId = Number(this.getQueryParam('documentItemId'));
  public commodityCode = this.getQueryParam('CommodityCode');


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
  listActions: FormAction[] = [
    FormActionTypes.refresh
  ]
  constructor(
    private router: Router,
    public _mediator: Mediator,
    public dialog: MatDialog,
    private route: ActivatedRoute,
    public Service: PagesCommonService,

    public _notificationService: NotificationService,
  ) {
    super(route, router);

  }

  async ngOnInit() {
    await this.resolve()
  }

  //---------------attachment------------------------
  async getAttachments() {
    await this._mediator.send(new GetRecepitAttachmentsQuery(
      Number(this.getQueryParam('id')),
    )).then(res => {
      this.attachmentIds = res;
    })
  }
  async resolve(params?: any) {
    this.formActions = [
    ];
    await this.initialize()

  }

  async initialize(entity?: any) {
    //-----------------------------------

    this.receipt = await this.getData()

    this.displayPage = this.getQueryParam('displayPage')

  }
  async getData() {

    if (this.getQueryParam('id')) {
      this.getAttachments();
      return await this.get(this.getQueryParam('id'));
    }
    else if (this.getQueryParam('documentId')) {

      return await this._mediator.send(new GetRecepitGetByDocumentIdQuery(this.getQueryParam('documentId')))
    }
    else if (this.getQueryParam('documnetNo') && this.getQueryParam('codeVoucherGroupId')) {
      return await this.getByDocumentNoVoucher(this.getQueryParam('documnetNo'), this.getQueryParam('codeVoucherGroupId'));
    }
    else if (this.getQueryParam('documnetNo') && this.getQueryParam('documentStauseBaseValue')) {
      return await this.getByDocumentNoBase(this.getQueryParam('documnetNo'), this.getQueryParam('documentStauseBaseValue'));
    }
    else if (this.getQueryParam('invoiceNo') && this.getQueryParam('creditAccountReferenceId')) {
      return await this.getReceiptsByInvoiceNo(this.getQueryParam('invoiceNo'), this.getQueryParam('creditAccountReferenceId'), this.getQueryParam('invoiceDate'));
    }


    else {
      return await this.getListId(this.Service.ListId);
    }
  }

  async get(Id: number) {
    return await this._mediator.send(new GetRecepitQuery(Id))
  }

  async getByDocumentNoBase(documnetNo: number, documentStauseBaseValue: number) {

    return await this._mediator.send(new GetByDocumentNoAndDocumentStauseBaseValueQuery(documnetNo, documentStauseBaseValue))
  }
  async getByDocumentNoVoucher(documnetNo: number, codeVoucherGroupId: number) {

    return await this._mediator.send(new GetByDocumentNoAndDocumentCodeVoucherGroupIdQuery(documnetNo, codeVoucherGroupId))
  }
  async getListId(ListId: string[]) {
    var req = new GetRecepitListIdQuery();
    req.ListIds = ListId;
    return await this._mediator.send(<GetRecepitListIdQuery>req);

  }
  async getReceiptsByInvoiceNo(InvoiceNo: string, creditAccountReferenceId: any, invoiceDate: any) {

    return await this._mediator.send(new GetReceiptsByInvoiceNoQuery(InvoiceNo, invoiceDate, creditAccountReferenceId))

  }

  async getBomValue(item: any) {
    let dialogConfig = new MatDialogConfig();
    dialogConfig.data = {
      documentItemsId: item.id,
      allowViewPrice: this.Service.identityService.doesHavePermission('ViewDocumentsAccounting'),
      isReadOnly: true,
      pageMode: PageModes.Update

    };

    this.dialog.open(DocumentItemsBomDialog, dialogConfig);



  }
  async print() {


    if (this.receipt == undefined) {
      return;
    }
    var contains: any = document.getElementsByClassName('details');
    const slide = contains[0] as HTMLElement;


    this.Service.onPrint(slide.innerHTML, 'جرئیات سند')

  }

  async navigateToVoucher() {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${this.receipt?.voucherHeadId}`)
  }
  async navigateToReceiptRials() {

    let url = ''
    if (this.receipt?.documentId) {
      url = `inventory/rialsReceiptDetails?documentId=${this.receipt?.documentId}&isImportPurchase=${this.receipt?.isImportPurchase}`
    }
    else {
      url = `inventory/rialsReceiptDetails?id=${this.receipt?.id}&isImportPurchase=${this.receipt?.isImportPurchase}`
    }

    await this.router.navigateByUrl(url)
  }
  async navigateToHistory(id: number) {


    var url=''
    if (this.displayPage == 'none') {

      url = `inventory/commodityReceiptReportsRial?commodityId=${id}&warehouseId=${this.receipt?.warehouseId}`
    }
    else {
      url = `inventory/commodityReceiptReports?commodityId=${id}&warehouseId=${this.receipt?.warehouseId}`
    }

    this.router.navigateByUrl(url)

  }

  async CommoditySerials(item: any) {

    let dialogConfig = new MatDialogConfig();
    let assetsList: any = undefined;



    await this._mediator.send(new GetAssetsByDocumentIdQuery(item.id, item.commodityId)).then(res => {

      assetsList = res

      dialogConfig.data = {
        commodityCode: item.commodity.code,
        commodityId: item.commodityId,
        commodityTitle: item.commodity.title,
        quantity: item.quantity,
        documentItemId: item.id,
        assets: assetsList

      };


      let dialogReference = this.dialog.open(CommoditySerialViewDialog, dialogConfig);
      dialogReference.afterClosed();

    });


  }
  async update() {

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
