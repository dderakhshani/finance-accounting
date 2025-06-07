import { Component, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from "../../../../../../core/abstraction/base.component";
import { ActivatedRoute, Router } from "@angular/router";
import { Mediator } from "../../../../../../core/services/mediator/mediator.service";
import { IdentityService } from "../../../../../identity/repositories/identity.service";
import { Receipt } from '../../../../entities/receipt';
import { GetRecepitQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-query';
import { PagesCommonService } from '../../../../../../shared/services/pages/pages-common.service';
import { GetReceiptsByInvoiceNoQuery } from '../../../../repositories/receipt/queries/receipt/get-receipts-invoceNo-query';
import { GetRecepitGetByDocumentIdQuery } from '../../../../repositories/receipt/queries/receipt/get-receipt-by-documentId-query';

import { GetByDocumentIdByVoucherHeadIdQuery } from '../../../../repositories/receipt/queries/receipt/get-by-documentId-by-voucherHeadId-query';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { PageModes } from '../../../../../../core/enums/page-modes';
import { DocumentItemsBomDialog } from '../../../component/document-Items-bom-dialog/document-Items-bom-dialog.component';


@Component({
  selector: 'app-document-accounting-details',
  templateUrl: './document-accounting-details.component.html',
  styleUrls: ['./document-accounting-details.component.scss']
})
export class DocumentAccountingDatailsComponent extends BaseComponent {


  public receipt: Receipt | undefined = undefined;

  public displayPage: string = "";

  constructor(private route: ActivatedRoute,
    private router: Router,
    public dialog: MatDialog,
    private identityService: IdentityService,
    private Service: PagesCommonService,
    private _mediator: Mediator) {
    super(route, router);


  }

  async ngOnInit() {
    await this.resolve()
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
      return await this.get(this.getQueryParam('id'));
    }
    else if (this.getQueryParam('documentId')) {

      return await this.getRecepitGetByDocumentIdQuery(this.getQueryParam('documentId'));
    }
    else if (this.getQueryParam('voucherId')) {
        return await this._mediator.send(new GetByDocumentIdByVoucherHeadIdQuery(this.getQueryParam('voucherId')))
      }
      else {
        return await this.getListInvoice(this.getQueryParam('invoiceNo'), this.getQueryParam('date'), this.getQueryParam('creditAccountReferenceId'));
      }
  }

  async get(Id: number) {
    return await this._mediator.send(new GetRecepitQuery(Id))
  }
  async getListInvoice(invoiceNo: string, fromDate: Date, creditAccountReferenceId: number) {

    let response = await this._mediator.send(new GetReceiptsByInvoiceNoQuery(invoiceNo, fromDate, creditAccountReferenceId))

    return response

  }
  async getRecepitGetByDocumentIdQuery(documentId: number) {
    return await this._mediator.send(new GetRecepitGetByDocumentIdQuery(documentId))
  }
  async navigateToRecive(item: any) {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?id=${item.documentHeadId}&isImportPurchase=${item.isImportPurchase}`)
  }
  async navigateToVoucher() {
    await this.router.navigateByUrl(`accounting/voucherHead/add?id=${this.receipt?.voucherHeadId}`)
  }
  async navigateToHistory(id: number) {

    var url = `inventory/commodityReceiptReports?commodityId=${id}&warehouseId=${this.receipt?.warehouseId}`
    this.router.navigateByUrl(url)

  }
  async navigateToRialReceipt() {
    await this.router.navigateByUrl(`inventory/rialsReceiptDetails?documentId=${this.receipt?.documentId}&isImportPurchase=${this.receipt?.isImportPurchase}&editType=${3}`)
  }

  async print() {

    let printContents = `

        <div style="display:flex;flex-wrap: wrap;text-align: right;">
            <div style="flex:30%">
                  <span class="font-11 line-height">نوع سند</span>
                :
                <label>${this.receipt?.codeVoucherGroupTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">تاریخ مالی</span>
                :
                <label>${this.Service.toPersianDate(this.receipt?.invoiceDate)}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">شماره مالی </span>
                :
                <label class="btn-link">${this.receipt?.documentId}</label>
            </div>


            <div style="flex:30%">
                  <span class="font-11 line-height">سرفصل حساب بستانکار</span>
                :
                <label>${this.receipt?.creditAccountHeadTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">گروه حساب بستانکار</span>
                :
                <label>${this.receipt?.creditAccountReferenceGroupTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">حساب بستانکار</span>
                :
                <label>${this.receipt?.creditReferenceTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">سرفصل حساب  بدهکار</span>
                :
                <label>${this.receipt?.debitAccountHeadTitle}</label>
            </div>


            <div style="flex:30%">
                  <span class="font-11 line-height">گروه حساب بدهکار</span>
                :
                <label>${this.receipt?.debitAccountReferenceGroupTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">حساب بدهکار</span>
                :
                <label>${this.receipt?.debitReferenceTitle}</label>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">انبار تحویل گیرنده</span>
                :
                <label>${this.receipt?.warehouseTitle}</label>
            </div>



            <div style="flex:30%">

                  <span class="font-11 line-height">شماره عملیات مالی</span>
                :
                <label>${this.receipt?.financialOperationNumber}</label>
            </div>


            <div style="flex:90%">

                  <span class="font-11 line-height">شرح</span>
                :
                <label>${this.receipt?.documentDescription}</label>
            </div>

        </div>
        <div style="display:flex;flex-wrap: wrap;text-align: right;">

            <div style="flex:30%">

                  <span class="font-11 line-height">هزینه به جمع کل اضافه شود؟</span> :

                <span> ${this.receipt?.isFreightChargePaid == true ? 'بله' : 'خیر'}</span>

            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">مبلغ مالیات ارزش افزوده</span> :

                <span >
                    ${this.receipt?.vatDutiesTax!=undefined? this.receipt?.vatDutiesTax?.toLocaleString():''}
                </span> <span class="font-12 line-height"> ریال </span>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">هزینه اضافی</span> :

                <span >
                    ${this.receipt?.extraCost != undefined ?this.receipt?.extraCost?.toLocaleString():''}
                </span> <span class="font-12 line-height"> ریال </span>
            </div>
            <div style="flex:30%">
                  <span class="font-11 line-height">جمع قیمت تمام شده</span> :

                <span >
                    ${this.receipt?.totalProductionCost != undefined ?this.receipt?.totalProductionCost?.toLocaleString():''}
                </span><span class="font-12 line-height"> ریال </span>
            </div>

            <div style="flex:60%">
                  <span class="font-11 line-height">جمع کل قابل پرداخت</span> :

                <span >
                    ${this.receipt?.totalItemPrice != undefined ?this.receipt?.totalItemPrice?.toLocaleString():''}
                </span> <span class="font-12 line-height"> ریال </span>

            </div>


        </div>

    `

    let printtable = `<table><thead>
                     <tr>
                          <th>ردیف</th>
                          <th>${(this.receipt?.documentStauseBaseValue == 43 || this.receipt?.documentStauseBaseValue == 53) ? 'شماره رسید' : 'شماره حواله'} </th>
                          <th>کد کالا</th>
                          <th>نام کالا </th>
                          <th>واحد کالا</th>
                          <th>مقدار کالا</th>
                          <th>
                              قیمت واحد
                          </th>
                          <th>
                              مبلغ کل ریالی
                          </th>
                     </tr>
                   </thead><tbody>`;
    let i = 1;


    var totalQuantity: any = 0

    this.receipt?.items.map(data => {
      printtable += `<tr>
                        <td>${i}</td>
                         <td>${data?.documentNo}</td>
                         <td>${data?.commodity?.code}</td>
                         <td>${data?.commodity?.title}</td>
                         <td>${data?.commodity?.measureTitle}</td>
                         <td>${data.quantity}</td>
                         <td>${data.unitPrice?.toLocaleString()}</td>
                         <td>${data.totalPrice?.toLocaleString()}</td>
                        </tr>`
      i++;

      totalQuantity += Number(data.quantity)
    })
    printtable += `<tr>
                        <td colspan="5">جمع کل</td>

                        <td>${totalQuantity.toLocaleString()}</td>
                        <td></td>
                        <td  colspan="2">${this.receipt?.totalProductionCost.toLocaleString()}</td>

                  </tr>

                  </tbody></table>`

    printContents = printContents + printtable
    this.Service.onPrint(printContents, 'ریز سند حسابداری شماره ' + this.receipt?.voucherNo)
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
  async update() {

  }


  async add() {



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
