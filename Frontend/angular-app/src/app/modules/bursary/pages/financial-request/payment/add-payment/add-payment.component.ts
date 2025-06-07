import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { BaseComponent } from 'src/app/core/abstraction/base.component';
import { PageModes } from 'src/app/core/enums/page-modes';
import { Mediator } from 'src/app/core/services/mediator/mediator.service';
import { GetCodeVoucherGroupsQuery } from 'src/app/modules/accounting/repositories/code-voucher-group/queries/get-code-voucher-groups-query';
import { GetBaseValuesByUniqueNameQuery } from 'src/app/modules/admin/repositories/base-value/queries/get-base-values-by-unique-name-query';
import { AccountReference } from 'src/app/modules/bursary/entities/account-reference';
import { Bank } from 'src/app/modules/bursary/entities/bank';
import { BaseValue } from 'src/app/modules/bursary/entities/base-value';
import { ChequeSheet } from 'src/app/modules/bursary/entities/cheque-sheet';
import { CodeVoucherGroup } from 'src/app/modules/bursary/entities/code-voucher-group';
import { AccountHeads, AccountReferencesGroupEnums, CodeVouchers, CurrencyType, Documents, FinantialReferences } from 'src/app/modules/bursary/entities/enums';
import { GetBanksQuery } from 'src/app/modules/bursary/repositories/bank/queries/get-banks-query';
import { GetChqueSheetsQuery } from 'src/app/modules/bursary/repositories/cheque/queries/get-cheque-sheets-query';
import { CreateFinancialAttachmentCommand } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command';
import { GetReceiptQuery } from 'src/app/modules/bursary/repositories/financial-request/customer-receipt/queries/get-receipt-query';
import { NotificationService } from 'src/app/shared/services/notification/notification.service';
import { SearchQuery } from 'src/app/shared/services/search/models/search-query';
import { PaymentAttachmentsComponent } from './payment-attachments/payment-attachments.component';
import { MatDialog } from '@angular/material/dialog';
import { CreatePaymentCommand } from 'src/app/modules/bursary/repositories/financial-request/request-payments/commands/create-payment-command';
import { CreateChequeSheetCommand } from 'src/app/modules/bursary/repositories/financial-request/receipt-cheque/commands/create-cheque-sheet-command';
import { CreateRequestPaymentCommand } from 'src/app/modules/bursary/repositories/financial-request/request-payments/commands/create-request-payment-command';
import { UpdateRequestPaymentCommand } from 'src/app/modules/bursary/repositories/financial-request/request-payments/commands/update-request-payment-command';
import { FinancialRequest } from 'src/app/modules/bursary/entities/financial-request';
import { UpdatePaymentCommand } from 'src/app/modules/bursary/repositories/financial-request/request-payments/commands/update-payment-command';
import { PaymentArticleComponent } from './payment-article/payment-article.component';

@Component({
  selector: 'app-add-payment',
  templateUrl: './add-payment.component.html',
  styleUrls: ['./add-payment.component.scss']
})
export class AddPaymentComponent extends BaseComponent {

  @ViewChild(PaymentArticleComponent) paymentArticleComponent!: PaymentArticleComponent;

  accountReferences: AccountReference[] = [];
  totalAmount : number = 0;
  public documents = Documents;
  codeVoucherGroups: CodeVoucherGroup[] = [];
  filteredCodeVoucherGroups: CodeVoucherGroup[] = [];
  public finantialReferencesEnums = FinantialReferences;
  showFiller = false;
  documentTypes: BaseValue[] = [];
  currencyTypes: BaseValue[] = [];
  public currencyType = CurrencyType;
  referenceTypes: BaseValue[] = [];
  chequeTypes: BaseValue[] = [];
  finantialReferenceTypes: BaseValue[] = [];
  chequeSheets: ChequeSheet[] = [];
  banks: Bank[] = [];
  attachments: CreateFinancialAttachmentCommand[] = [];
  imageUrls: string[] = [];
  constructor(
    private _mediator: Mediator,
    private router: Router,
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private notificationService:NotificationService
  ) {
    super(route, router);
    this.request = new CreateRequestPaymentCommand();
  }

  async ngOnInit(params?: any) {
    await this.resolve()
  }

  async resolve(params?: any) {

    var chequeFilter = [new SearchQuery({
      propertyName: 'isUsed',
      comparison: 'equal',
      values: [false],
    })];


    this.isLoading = true;
    forkJoin({
      documentTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("BursaryDocumentTypeBaseId")),
      currencyTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("CurrencyType")),
      referenceTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("FinantialReferenceTypeBaseId")),
      finantialReferenceTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("FinantialReferenceTypeBaseId")),
      codeVoucherGroups: this._mediator.send(new GetCodeVoucherGroupsQuery()),
      chequeSheets: this._mediator.send(new GetChqueSheetsQuery(0, 25, chequeFilter)),
      banks: this._mediator.send(new GetBanksQuery()),
      chequeTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("ChequeType")),
    }).subscribe(async (res) => {
      this.codeVoucherGroups = res.codeVoucherGroups.data;
      this.filteredCodeVoucherGroups = res.codeVoucherGroups.data;
      this.documentTypes = res.documentTypes;
      this.currencyTypes = res.currencyTypes;
      this.referenceTypes = res.referenceTypes;
      this.finantialReferenceTypes = res.finantialReferenceTypes;
      this.chequeSheets = res.chequeSheets.data;
      this.banks = res.banks.data;
      this.chequeTypes = res.chequeTypes;

      await this.initialize();
      this.isLoading = false;
    });

  }

  async initialize(entity?: any) {
    if (entity || this.getQueryParam("id")) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam("id"));
      this.request = new UpdateRequestPaymentCommand().mapFrom(<FinancialRequest>entity);
    } else {
      this.pageMode = PageModes.Add;
      this.request = this.createDefaultRequest();
    }

  }



  createDefaultRequest() {

    let request = new CreateRequestPaymentCommand();
    request.codeVoucherGroupId = CodeVouchers.BursaryReceiveDocument;
    request.documentDate = new Date();
    let payment = new CreatePaymentCommand();
    let attachment = new CreateFinancialAttachmentCommand();

    payment.chequeSheet = new CreateChequeSheetCommand();
    payment.documentTypeBaseId = Documents.Remittance;
    payment.debitAccountHeadId = AccountHeads.AccountHeadCode_2601;
    payment.debitAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_02;
    payment.creditAccountHeadId = AccountHeads.AccountHeadCode_2304;
    payment.isRial = true;
    payment.financialReferenceTypeBaseId = this.finantialReferencesEnums.PaymentReceipt;
    request.financialRequestPartials.push(payment);

    request.attachments.push(attachment);

    return request
  }


  async add(param?: any) {


    if (this.form.value.description == null || this.form.value.description.length == 0)
      this.fillDescription();

    let error = this.checkvalidations();
    if (error)return;

    var command = <CreateRequestPaymentCommand>this.form.getRawValue();
    command.financialRequestPartials[0].description = command.financialRequestPartials[0].paymentCode;

    command.financialRequestPartials.forEach((item: any) => {
        item.description = item.paymentCode;

    })

    // شرط عجیب غریب اقای ملک پور
    if (command.financialRequestPartials[0].documentTypeBaseId == this.documents.Cash && command.financialRequestPartials[0].isRial == false)
        command.financialRequestPartials[0].nonRialStatus = 1; // 1= مشتری به صرافی  /// According to Mr.MalekPour

    (<CreateRequestPaymentCommand>this.request).financialRequestPartials = command.financialRequestPartials;

    (<CreateRequestPaymentCommand>this.request).attachments = this.attachments;
    if (!error){
  this.isLoading = true;

try {

    await this._mediator.send(<CreateRequestPaymentCommand>this.request).then(response=>{


      this.isLoading = false;
      this.attachments = [];
      this.isLoading = false;
    });
  }
  catch{
    this.isLoading = false;
  }



    }
    this.isLoading = false;

  }
  async submit(){

    let error = this.checkvalidations();
    if (error)return;

      await super.submit();
      this.reset();
    }


    checkvalidations():boolean {

      var isError = false;
      this.form.value.financialRequestPartials.forEach((item: any) => {

        if (item.documentTypeBaseId == this.documents.ChequeSheet)
        item.amount = item.chequeSheet.totalCost;


        if (item.creditAccountReferenceId == null && item.isCreditAccountHead != true){
          this.notificationService.showFailureMessage("حساب تفصیل واریز کننده وارد نشده است",0);
          isError = true;
          return ;
        }

        if (item.debitAccountReferenceId == null && item.isDebitAccountHead != true){
          this.notificationService.showFailureMessage("حساب تفصیل دریافت کننده وارد نشده است",0);
          isError = true;
          return ;
        }



        if (item.creditAccountReferenceGroupId == null && item.isCreditAccountHead != true){
          this.notificationService.showFailureMessage(" گروه تفصیل  واریز کننده وارد نشده است",0);
          isError = true;
          return;
        }


        if ( item.debitAccountReferenceGroupId == null && item.isDebitAccountHead != true){
          this.notificationService.showFailureMessage(" گروه تفصیل دریافت کننده وارد نشده است",0);
          isError = true;
          return;
        }


        if (item.creditAccountHeadId == null || item.debitAccountHeadId == null){
           this.notificationService.showFailureMessage(" سرفصل حساب دریافت کننده یا واریز کننده وارد نشده است",0);
           isError = true;
           return;
        }

        if (item.amount == null || item.amount == 0){
          this.notificationService.showFailureMessage(" مبلغ به درستی وارد نشده است ",0);
          isError = true;
          return;
        }

        if (item.financialReferenceTypeBaseId == null){
          this.notificationService.showFailureMessage("  نوع مبنا به درستی وارد نشده است ",0);
          isError = true;
          return;
        }

        // if (item.description == null || item.description == "" ){
        //   this.notificationService.showFailureMessage("  توضیحات به درستی وارد نشده است ",0);
        //   isError = true;
        //   return;
        // }

        if (item.paymentCode == null && item.documentTypeBaseId != this.documents.ChequeSheet){
          this.notificationService.showFailureMessage("  کد رهگیری به درستی وارد نشده است ",0);
          isError = true;
          return;
        }

      });
    return isError;
    }


    async get(id: number) {
      this.isLoading = true;
      let response = await this._mediator.send(new GetReceiptQuery(id));
      this.isLoading = false;
      return response;
    }


    async update(param?: any) {

      let error = this.checkvalidations();
      if (error)return;

      var command = <UpdatePaymentCommand>this.form.getRawValue();

      var currencyTypeId = command.financialRequestPartial[0].currencyTypeBaseId;

      command.financialRequestPartial.forEach((item :any) => {
        item.currencyTypeBaseId = currencyTypeId;
      });

      (<UpdatePaymentCommand>this.request).financialRequestPartial = command.financialRequestPartial;
      (<UpdatePaymentCommand>this.request).financialRequestAttachments = this.attachments;
      if (!error){
      this.isLoading = true;
      await this._mediator.send(<UpdatePaymentCommand>this.request);
      this.isLoading = false;
      this.reset();
    }
    }




    codeVoucherDisplayFn(codeVoucherGroupId: number) {
      return (
        this.codeVoucherGroups.find((x) => x.id === codeVoucherGroupId)?.title ?? "");
    }

    delete(param?: any) {

    }

    close() {

    }


    addPayment() {


      // const payment = <FormGroup>this.createForm(new CreatePaymentCommand(), true);
      // this.form.controls["financialRequestPartials"].push(payment);
      // const chequeSheet = <FormGroup>this.createForm(new CreateChequeSheetCommand(), true);
      // (<FormGroup>payment).controls['chequeSheet'] = chequeSheet;
      // let indexPayment = this.form.controls.financialRequestPartials.length - 1
      // let latsPayment = this.form.value.financialRequestPartials[indexPayment - 1];

      // payment.patchValue({
      //   debitAccountHeadId: latsPayment.debitAccountHeadId,
      //   debitAccountReferenceGroupId: latsPayment.debitAccountReferenceGroupId,
      //   debitAccountReferenceId: latsPayment.debitAccountReferenceId,

      // });




      let data = {
        documentTypes : this.documentTypes,
        currencyTypes : this.currencyTypes,
        referenceTypes : this.referenceTypes,
        chequeTypes : this.chequeTypes,
        finantialReferenceTypes: this.finantialReferenceTypes,
        chequeSheets: this.chequeSheets,
        banks : this.banks
      }


      const dialogRef = this.dialog.open(PaymentArticleComponent, {
        width: '80%',
        height: '80%',
        data: { data: JSON.stringify(data) },
      });

      dialogRef.afterClosed().subscribe(paymentArticle => {



      });




    }


    addAttachments() {
      const dialogRef = this.dialog.open(PaymentAttachmentsComponent, {
        width: '80%',
        height: '80%',
        data: { data: JSON.stringify(this.form.value.financialRequestAttachments) },
      });

      dialogRef.afterClosed().subscribe(files => {

        files.forEach((item: any) => {
          this.attachments.push(item);
        });


      });
    }

    removeItem(index: any) {
      (<FormArray>this.form.controls.financialRequestPartials).removeAt(index);
    }

    fillDescription() {

      let des = "";
      let paymentCode = "";
      if (this.form.value.financialRequestPartials[0].isRial == true && this.form.value.financialRequestPartials.length == 1 && this.form.value.financialRequestPartials[0].documentTypeBaseId != this.documents.ChequeSheet) {

        paymentCode = this.form.value.financialRequestPartials[0].paymentCode;
        des = "بابت دریافت شماره ردیف |: و به شماره پیگیری  " + paymentCode;
      }
      else if (this.form.value.financialRequestPartials[0].isRial == true && this.form.value.financialRequestPartials.length >1){
        this.form.value.financialRequestPartials.forEach((item:any) => {

          paymentCode += item.paymentCode + " و ";
        });
        paymentCode = paymentCode.slice(0,-2);
        des = "بابت دریافت تجمیعی شماره ردیف |: و به شماره پیگیری  " + paymentCode +" (پرداخت تجمیعی) ";
      }

      else if (this.form.value.financialRequestPartials[0].documentTypeBaseId == this.documents.Cash && this.form.value.financialRequestPartials[0].isRial == this.currencyType.NonRial){

        paymentCode = this.form.value.financialRequestPartials[0].paymentCode;
        let amount = this.form.value.financialRequestPartials[0].currencyAmount;
        let currencyTypeId = this.form.value.financialRequestPartials[0].currencyTypeBaseId;
        let currencyTypeTitle = this.paymentArticleComponent.currencyTypes.find(x => x.id == currencyTypeId);
        let currencyFee = this.form.value.financialRequestPartials[0].currencyFee;
        des = " بابت دریافت از " + amount + " "+ currencyTypeTitle?.title + " به نرخ  "+currencyFee+" از محل ارز حاصل از صادرات طی فرم عملیات مالی           پرداخت نقدی به صندوق ارزی";
      }

      else if (this.form.value.financialRequestPartials[0].documentTypeBaseId == this.documents.ChequeSheet){
        var chequeNumber = this.form.value.financialRequestPartials[0].chequeSheet.sheetSeqNumber;
        des = "بابت دریافت چک ضمانت به شماره "+chequeNumber+" شماره ردیف |: "
      }

      else {

        let accountReference = this.form.value.financialRequestPartials[0].creditAccountReferenceId;
        let accountReferenceTitle = this.paymentArticleComponent.creditAccountReferences.find(x => x.id == accountReference);
        paymentCode = this.form.value.financialRequestPartials[0].paymentCode;
        let amount = this.form.value.financialRequestPartials[0].currencyAmount;
        let currencyTypeId = this.form.value.financialRequestPartials[0].currencyTypeBaseId;
        let currencyTypeTitle = this.paymentArticleComponent.currencyTypes.find(x => x.id == currencyTypeId);
        let currencyFee = this.form.value.financialRequestPartials[0].currencyFee;
        des = "بابت دریافت از " + accountReferenceTitle?.title +" طبق نامه شماره " + paymentCode + " به مبلغ " + amount + " " + currencyTypeTitle?.title + " به نرخ "+  currencyFee + " ارز                  " + "طی فرم عملیات مالی                 دریافت گردید";
      }



      this.form.patchValue({
        description: des
      })
    }


}

