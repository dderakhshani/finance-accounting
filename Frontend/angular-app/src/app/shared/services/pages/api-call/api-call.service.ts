import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Data } from '@angular/router';
import { Mediator } from '../../../../core/services/mediator/mediator.service';
import { AccountHead } from '../../../../modules/accounting/entities/account-head';
import { CommodityCategory } from '../../../../modules/commodity/entities/commodity-category';
import { IdentityService } from '../../../../modules/identity/repositories/identity.service';
import { warehouseReceiptToAutoVoucher } from '../../../../modules/inventory/entities/autoVoucher';
import { Receipt } from '../../../../modules/inventory/entities/receipt';
import { ReceiptAllStatusModel } from '../../../../modules/inventory/entities/receipt-all-status';
import { GetCommodityCategoriesALLQuery } from '../../../../modules/inventory/repositories/commodity-categories/get-commodity-categories-all-query';
import { ConvertToMechanizedDocumentCommand } from '../../../../modules/inventory/repositories/mechanized-document/commands/convert-mechanized-document-command';
import { CreateAndUpdateAutoVoucher2Command } from '../../../../modules/inventory/repositories/mechanized-document/commands/convert-to-mechanized-document';
import { GetInventoryAccountHeadsQuery } from '../../../../modules/inventory/repositories/personal/get-account-heads-query';
import { GetReceiptALLStatusQuery } from '../../../../modules/inventory/repositories/receipt/queries/receipt/get-receipt-all-status-query';
import { InvoiceAllStatusModel } from '../../../../modules/purchase/entities/invoice-all-status';
import { GetInvoiceALLStatusQuery } from '../../../../modules/purchase/repositories/invoice/queries/invoice/get-invoice-all-status-query';
import { NotificationService } from '../../notification/notification.service';
import { SearchQuery } from '../../search/models/search-query';


@Injectable({
  providedIn: 'root'
})
export class ApiCallService {
  GetCodeVoucherGroupsQuery() {
    throw new Error('Method not implemented.');
  }

  constructor(
    public _mediator: Mediator,
    private _ROUTE: ActivatedRoute,
    private _snackBar: MatSnackBar,
    private identityService: IdentityService,
    public _notificationService: NotificationService,
  ) {

  }

  public AllInvoiceStatus: any[] = [];
  public AllReceiptStatus: any[] = [];
  public AccountHead: AccountHead[] = [];
  public InvoiceStatus: InvoiceAllStatusModel[] = [];//لیست انواع سند
  public ReceiptStatus: ReceiptAllStatusModel[] = [];
  public CommodityCategory: CommodityCategory[] = []


  //پر کردن لیست انواع سند های خرید
  async getInvoiceAllStatus() {

    //اگر لیست قبلا پر نشده بود و خالی بود از اینجا پر شود.
    if (this.AllInvoiceStatus.length == 0) {

      let request = new GetInvoiceALLStatusQuery()
      let response = await this._mediator.send(request);
      this.InvoiceStatus = response.data;
      this.AllInvoiceStatus = response.data;
      if (this.getQueryParam('codeVoucherGroupId') != undefined) {
        this.InvoiceStatus = this.AllInvoiceStatus.filter(a => a.code == this.getQueryParam('codeVoucherGroupId'));
      }

    }
    else if (this.getQueryParam('codeVoucherGroupId') != undefined) {
      this.InvoiceStatus = this.AllInvoiceStatus.filter(a => a.code == this.getQueryParam('codeVoucherGroupId'));
    }
    else {
      this.InvoiceStatus = this.AllInvoiceStatus;
    }


  }

  //پر کردن لیست انواع سند های انبار
  async getReceiptAllStatus(Code: string) {

    //اگر لیست قبلا پر نشده بود و خالی بود از اینجا پر شود.
    if (this.AllReceiptStatus.length == 0) {

      let request = new GetReceiptALLStatusQuery(Code)
      await this._mediator.send(request).then(response => {
        this.ReceiptStatus = response.data;
        this.AllReceiptStatus = response.data;


      });


    }
    if (this.getQueryParam('codeVoucherGroupId') != undefined) {

      this.ReceiptStatus = this.AllReceiptStatus.filter(a => a.id == Number(this.getQueryParam('codeVoucherGroupId')));
    }
    else if (this.getQueryParam('GroupCode') != undefined) {
      this.ReceiptStatus = this.AllReceiptStatus.filter(a => a.code == this.getQueryParam('GroupCode'));
    }
    else {
      this.ReceiptStatus = this.AllReceiptStatus;
    }
  }
  //پر کردن لیست انواع سند های انبار
  async getReceiptAllInventoryStatus() {


    if (this.AllReceiptStatus.length == 0) {
      await this.getReceiptAllStatus('');
    }
    //فقط سندهای مربوط به
    return this.AllReceiptStatus;
    //  .filter(b =>
    //  b.code.substring(0, 2) == '50' ||
    //  b.code.substring(0, 2) == '51' ||
    //  b.code.substring(0, 2) == '52' ||
    //  b.code.substring(0, 2) == '53' ||
    //  b.code.substring(0, 2) == '54' ||
    //  b.code.substring(0, 2) == '55' ||
    //  b.code.substring(0, 2) == '56' ||
    //  b.code.substring(0, 2) == '57'
    //);
  }

  async getReceiptStatusAmountReceipt() {

    if (this.AllReceiptStatus.length == 0) {
      await this.getReceiptAllStatus('');
    }
    //فقط سندهای مربوط به
    return this.AllReceiptStatus.filter(b =>
      b.code.substring(2, 3) == '43' ||
      b.code.substring(2, 3) == '44'

    );
  }
  //پر کردن انواع
  public async getAccountHead(isLastLevel: boolean = false) {


    //اگر لیست قبلا پر نشده بود و خالی بود از اینجا پر شود.
    if (this.AccountHead.length < 2) {
      var filter: SearchQuery[] = [];
      if (isLastLevel) {
        filter.push(
          new SearchQuery({
            propertyName: 'LastLevel',
            comparison: 'equal',
            values: [true],
            nextOperand: "and"
          }))

      }

      await this._mediator.send(new GetInventoryAccountHeadsQuery(0, 0, filter)).then(async (res) => {
        this.AccountHead = res


      })
      return;
    }

  }
  public async GetCommodityCategoriesALL() {
    if (this.CommodityCategory.length == 0) {
      await this._mediator.send(new GetCommodityCategoriesALLQuery()).then(async (res) => {
        this.CommodityCategory = res
        return this.CommodityCategory;
      })
    }
    return;

  }

  getNewCodeVoucherGroupId(codeVoucherGroupId: number, documentStauseBaseValue: number): number {
    var codeVoucherGroupCode = this.AllReceiptStatus.find(a => a.id == codeVoucherGroupId)?.code;

    var NewCode = codeVoucherGroupCode?.substring(0, 2) + documentStauseBaseValue.toString();

    codeVoucherGroupId = Number(this.AllReceiptStatus.find(a => a.code == NewCode)?.id);
    return codeVoucherGroupId;
  }
  public getQueryParam(param: string) {

    return this._ROUTE.snapshot.queryParams[param]
  }
  /// ثبت و ویرایش سند مکانیزه------------------------------------------------

  async updateCreateAddAutoVoucher2(Receipts: Receipt[], ActionType: string) {

    var request = new ConvertToMechanizedDocumentCommand();
    var requestVoucher2 = new CreateAndUpdateAutoVoucher2Command();
    var dataList: warehouseReceiptToAutoVoucher[] = []
    var notValid: boolean = true;
    try {

      Receipts.forEach(a => {

        if ((a.selected == true && a.voucherHeadId == 0 && ActionType == 'insert') || ActionType == 'edit') {
          if (ActionType == 'insert') {
            a.voucherHeadId = 1 //'🕒';
          }
          
          dataList.push({

            DocumentNo: a.documentNo != undefined ? a.documentNo.toString() : '',
            FinancialOperationNumber: a.financialOperationNumber != undefined ? a.financialOperationNumber : '',
            Tag: a.tags,
            InvoiceNo: a.invoiceNo != undefined ? a.invoiceNo : '',
            DocumentDate: a.invoiceDate != undefined ? a.invoiceDate.toString() : '',

            CodeVoucherGroupId: a.documentStauseBaseValue != undefined ? a.documentStauseBaseValue.toString() : '',
            CodeVoucherGroupTitle: a.codeVoucherGroupTitle,

            DebitAccountHeadId: a.debitAccountHeadId != undefined ? a.debitAccountHeadId.toString() : '',
            DebitAccountReferencesGroupId: a.debitAccountReferenceGroupId != undefined ? a.debitAccountReferenceGroupId.toString() : '',
            DebitAccountReferenceId: a.debitAccountReferenceId != undefined ? a.debitAccountReferenceId.toString() : '',

            CreditAccountHeadId: a.creditAccountHeadId != undefined ? a.creditAccountHeadId.toString() : '',
            CreditAccountReferencesGroupId: a.creditAccountReferenceGroupId != undefined ? a.creditAccountReferenceGroupId.toString() : '',
            CreditAccountReferenceId: a.creditAccountReferenceId != undefined ? a.creditAccountReferenceId.toString() : '',

            ToTalPriceMinusVat: a.totalProductionCost.toString(),
            VatDutiesTax: a.vatDutiesTax.toString(),
            PriceMinusDiscountPlusTax: a.totalItemPrice.toString(),
            TotalItemPrice: a.totalItemPrice.toString(),

            TotalQuantity: "0",
            TotalWeight: "0",

            DocumentId: a.documentId != undefined ? a.documentId.toString() : '',
            DocumentIds: a.documentIds,
            VocherHeadId: a.voucherHeadId > 0 ? a.voucherHeadId.toString() : "0",
            VoucherRowDescription: a.codeVoucherGroupTitle.includes('خرید') ? a.creditReferenceTitle + ' طی شماره رسید :' + Array.from(new Set(a.invoiceNo.split('-'))).toString() + '-  (' + a.documentDescription + ')' :
            a.codeVoucherGroupTitle + " طی شماره حواله " + Array.from(new Set(a.invoiceNo.split('-'))).toString() ,
            DebitAccountHeadTitle: a.debitAccountHeadTitle,
            CreditAccountHeadTitle: a.creditAccountHeadTitle,
            //------------------------------------------------------------------
            ExtraCost: a.extraCost != undefined && a.extraCost > 0 && a.isImportPurchase == false ? a.extraCost.toString() : "0",//در خرید های وارداتی هزینه ارسال نشود

            CurrencyFee: a.currencyRate.toString(),
            CurrencyTypeBaseId: a.currencyBaseId.toString(),
            CurrencyAmount: a.currencyPrice.toString(),

            ExtraCostAccountHeadId: a.extraCostAccountHeadId != undefined ? a.extraCostAccountHeadId.toString() : '',
            ExtraCostAccountReferenceGroupId: a.extraCostAccountReferenceGroupId != undefined ? a.extraCostAccountReferenceGroupId.toString() : '',
            ExtraCostAccountReferenceId: a.extraCostAccountReferenceId != undefined ? a.extraCostAccountReferenceId.toString() : '',
            ExtraCostAccountHeadTitle: a.extraCostAccountHeadTitle != undefined ? a.extraCostAccountHeadTitle.toString() : '',
            IsFreightChargePaid: a.isFreightChargePaid == true ? '1' : '0'

          })
        }

      })
    } catch (e: any) {

      console.log('e',e)
      this._notificationService.showFailureMessage("اشکال در اطلاعات رسیدهای ریالی", 0)
      notValid = false;
    }
    if (notValid) {
      requestVoucher2.dataList = dataList;
      requestVoucher2.ActionType = ActionType
      requestVoucher2.VocherHeadId = dataList[0]?.VocherHeadId
      await this._mediator.send(<CreateAndUpdateAutoVoucher2Command>requestVoucher2).then(req => {

        this._mediator.send(<ConvertToMechanizedDocumentCommand>request).then(res => {

          Receipts.forEach(a => {

            if (a.selected == true) {
              a.selected = false;
              a.voucherHeadId = '✔✔'

            }

          })

        })


      })

    }


  }
}
