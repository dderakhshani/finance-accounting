import { Component, ViewChild } from "@angular/core";
import { FormArray, FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { forkJoin } from "rxjs";
import { BaseComponent } from "src/app/core/abstraction/base.component";
import { PageModes } from "src/app/core/enums/page-modes";
import { Mediator } from "src/app/core/services/mediator/mediator.service";
import { GetCodeVoucherGroupsQuery } from "src/app/modules/accounting/repositories/code-voucher-group/queries/get-code-voucher-groups-query";

import { BaseValue } from "src/app/modules/admin/entities/base-value";
import { GetBaseValuesByUniqueNameQuery } from "src/app/modules/admin/repositories/base-value/queries/get-base-values-by-unique-name-query";
import { AccountReference } from "src/app/modules/bursary/entities/account-reference";
import { AccountingDocument } from "src/app/modules/bursary/entities/accounting-document";
import { Bank } from "src/app/modules/bursary/entities/bank";
import { ChequeSheet } from "src/app/modules/bursary/entities/cheque-sheet";
import { CodeVoucherGroup } from "src/app/modules/bursary/entities/code-voucher-group";
import { AccountHeads, AccountReferencesGroupEnums, CodeVouchers, CurrencyType, Documents, FinantialReferences } from "src/app/modules/bursary/entities/enums";
import { FinancialRequest } from "src/app/modules/bursary/entities/financial-request";
import { CreateAccountingDocumentCommand } from "src/app/modules/bursary/repositories/accounting-document/commands/create-accounting-document-command";
import { GetBanksQuery } from "src/app/modules/bursary/repositories/bank/queries/get-banks-query";
import { GetChqueSheetsQuery } from "src/app/modules/bursary/repositories/cheque/queries/get-cheque-sheets-query";
import { CreateCustomerReceiptCommand } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-customer-receipt-command";
import { CreateFinancialAttachmentCommand } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-financial-attachment-command";
import { CreateReceiptCommand } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/create-receipt-command";
import { UpdateCustomerReceiptCommand } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/update-customer-receipt-command";
import { GetReceiptQuery } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/queries/get-receipt-query";
import { CreateChequeSheetCommand } from "src/app/modules/bursary/repositories/financial-request/receipt-cheque/commands/create-cheque-sheet-command";
import { SearchQuery } from "src/app/shared/services/search/models/search-query";
import { CustomerReceiptAttachmentComponent } from "./customer-receipt-attachment/customer-receipt-attachment.component";
import { CustomerReceiptArticleComponent } from "./customer-receipt-article/customer-receipt-article.component";
import { NotificationService } from "src/app/shared/services/notification/notification.service";
import { ConfirmDialogComponent, ConfirmDialogIcons } from "src/app/core/components/material-design/confirm-dialog/confirm-dialog.component";
import { UpdateUpdateVoucherHeadIdCommand } from "src/app/modules/bursary/repositories/financial-request/customer-receipt/commands/update-voucherHead-id-command";
import { MoneyPipe } from "src/app/core/pipes/money.pipe";
import { GetLastReceiptInfoQuery } from "src/app/modules/bursary/repositories/financial-request/request-payments/queries/get-last-receipt-info-query";
import { number } from "echarts";



export enum DocumentTypes {
  RialDoc = 28509,
  NonRialDoc = 28510,
  ChequeDoc = 28511,
  PoseDoc = 28512,
  CashInvoiceDoc = 28513,
}

@Component({
  selector: "app-add-receipt",
  templateUrl: "./add-receipt.component.html",
  styleUrls: ["./add-receipt.component.scss"],
})
export class AddReceiptComponent extends BaseComponent {

  @ViewChild(CustomerReceiptArticleComponent) customerReceiptArticleComponent!: CustomerReceiptArticleComponent;

  accountReferences: AccountReference[] = [];
  lastReceiptInfo: FinancialRequest | undefined = undefined;
  totalAmount: number = 0;
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
    private notificationService: NotificationService,
    private pipe: MoneyPipe
  ) {
    super(route, router);
    this.request = new CreateCustomerReceiptCommand();
  }

  async ngOnInit(params?: any) {
    await this.resolve();

  }

  async resolve(params?: any) {

    var chequeFilter = [new SearchQuery({
      propertyName: 'isUsed',
      comparison: 'equal',
      values: [false],
    })];


    this.isLoading = true;
    forkJoin({
      lastReceiptInfo: this._mediator.send(new GetLastReceiptInfoQuery()),
      documentTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("BursaryDocumentTypeBaseId")),
      currencyTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("CurrencyType")),
      referenceTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("FinantialReferenceTypeBaseId")),
      finantialReferenceTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("FinantialReferenceTypeBaseId")),
      codeVoucherGroups: this._mediator.send(new GetCodeVoucherGroupsQuery()),
      chequeSheets: this._mediator.send(new GetChqueSheetsQuery(0, 25, chequeFilter)),
     // banks: this._mediator.send(new GetBanksQuery()),
      chequeTypes: this._mediator.send(new GetBaseValuesByUniqueNameQuery("ChequeType")),
    }).subscribe(async (res) => {
      this.codeVoucherGroups = res.codeVoucherGroups.data;
      this.lastReceiptInfo = res.lastReceiptInfo;
      this.filteredCodeVoucherGroups = res.codeVoucherGroups.data;
      this.documentTypes = res.documentTypes;
      this.currencyTypes = res.currencyTypes;
      this.referenceTypes = res.referenceTypes;
      this.finantialReferenceTypes = res.finantialReferenceTypes;
      this.chequeSheets = res.chequeSheets.data;
     // this.banks = res.banks.data;
      this.chequeTypes = res.chequeTypes;

      await this.initialize();
      this.isLoading = false;
    });

  }

  async initialize(entity?: any) {
    if (entity || this.getQueryParam("id")) {
      this.pageMode = PageModes.Update;
      if (!entity) entity = await this.get(this.getQueryParam("id"));
      this.request = new UpdateCustomerReceiptCommand().mapFrom(<FinancialRequest>entity);
      // this.form.controls['documentDate'].disable()
      // this.isDisableDocumentDate = true;
    } else {
      this.pageMode = PageModes.Add;
      this.request = this.createDefaultRequest();
      // this.form.controls['documentDate'].enable();
      // this.isDisableDocumentDate = false;
    }

  }

  getFinancialRequestDetailsSum() {
    var items = this.form.value.financialRequestDetails.map((x: any) => x.amount).reduce((partialSum: number, a: number) => partialSum + a, 0);
    return this.pipe.transform(items);
  }

  createDefaultRequest() {

    let request = new CreateCustomerReceiptCommand();
    request.isBursaryDocument = true;
    request.repeatNo = 1;
    request.codeVoucherGroupId = CodeVouchers.BursaryReceiveDocument;
    request.documentDate = new Date();
    let receipt = new CreateReceiptCommand();
    let attachment = new CreateFinancialAttachmentCommand();

    receipt.chequeSheet = new CreateChequeSheetCommand();
    receipt.documentTypeBaseId = Documents.Remittance;

    receipt.debitAccountHeadId = AccountHeads.AccountHeadCode_2601;
    receipt.debitAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_1001;
    receipt.creditAccountReferenceGroupId = AccountReferencesGroupEnums.AccountReferencesGroupCode_31;
    receipt.creditAccountHeadId = AccountHeads.AccountHeadCode_2304;
    
    receipt.isRial = true;
    receipt.financialReferenceTypeBaseId = this.finantialReferencesEnums.PaymentReceipt;
    request.financialRequestDetails.push(receipt);

    request.attachments.push(attachment);

    return request
  }

  async add(param?: any) {


    // if (this.form.value.description != null && this.form.value.description.length > 0){


    //   const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    //     data: {
    //       title: 'تایید توضیحات',
    //       message: 'آیا مایلید از توضیحات خود استفاده کنید؟ در صورت تایید ،  توضیحات شما در دفتر حساب نیز نمایش داده خواهد شد',
    //       icon: ConfirmDialogIcons.warning,
    //       actions: {
    //         confirm: { title: 'بله', show: true }, cancel: { title: 'خیر', show: true }
    //       }
    //     }
    //   });
    //   dialogRef.afterClosed().subscribe(async result => {
    //     if (result == false)
    //      await this.fillDescription();
    //   });
    // }

    if (this.form.value.description == null || this.form.value.description.length == 0)
      this.fillDescription();

    let error = this.checkvalidations();
    if (error) return;

    var command = <CreateCustomerReceiptCommand>this.form.getRawValue();
    command.financialRequestDetails[0].description = command.financialRequestDetails[0].paymentCode;

    command.financialRequestDetails.forEach((item: any) => {
      item.description = item.paymentCode;
    })

    // شرط عجیب غریب اقای ملک پور
    if (command.financialRequestDetails[0].documentTypeBaseId == this.documents.Cash && command.financialRequestDetails[0].isRial == false)
      command.financialRequestDetails[0].nonRialStatus = 1; // 1= مشتری به صرافی  /// According to Mr.MalekPour

    (<CreateCustomerReceiptCommand>this.request).financialRequestDetails = command.financialRequestDetails;

    (<CreateCustomerReceiptCommand>this.request).attachments = this.attachments;
    if (!error) {
      this.isLoading = true;

      try {

        var isDocument = (<CreateCustomerReceiptCommand>this.request).isBursaryDocument;

        var repeat = (<CreateCustomerReceiptCommand>this.request).repeatNo ?? 0;
      
        for (let i = 1; i <= repeat ; i++) {

        await this._mediator.send(<CreateCustomerReceiptCommand>this.request).then(async response => {

          if (isDocument)
            await this.addAccountingDocuments(response);

          this.isLoading = false;
          this.attachments = [];
          this.isLoading = false;
        });
      }
    }
      catch {
        this.isLoading = false;
      }
    }
    this.isLoading = false;
  }


  async addAccountingDocuments(request: any) {

    let accountingDoc = new CreateAccountingDocumentCommand();

    request.financialRequestDetails.forEach((detail: any) => {
      let ad !: AccountingDocument;



      const obj = ad || {};

      obj.DocumentNo = request.documentNo;
      obj.CodeVoucherGroupId = request.codeVoucherGroupId;
      obj.Amount = detail.amount ?? detail.chequeSheet?.totalCost;
      obj.CreditAccountHeadId = detail.creditAccountHeadId;
      obj.CreditAccountReferenceGroupId = detail.creditAccountReferenceGroupId == undefined ? null : detail.creditAccountReferenceGroupId;;
      obj.CreditAccountReferenceId = detail.creditAccountReferenceId == undefined ? null : detail.creditAccountReferenceId;
      obj.ReferenceName = " " + detail.creditAccountReferenceTitle + " ",
        obj.ReferenceCode = detail.creditAccountReferenceCode,
        obj.DebitAccountHeadId = detail.debitAccountHeadId;
      obj.DebitAccountReferenceGroupId = detail.debitAccountReferenceGroupId == undefined ? null : detail.debitAccountReferenceGroupId;
      obj.DebitAccountReferenceId = detail.debitAccountReferenceId == undefined ? null : detail.debitAccountReferenceId;;
      obj.DocumentDate = this.stripTimeFromDate(request.documentDate);
      obj.DocumentId = request.id;
      obj.DocumentTypeBaseId = detail.documentTypeBaseId;
      obj.SheetUniqueNumber = detail?.sheetUniqueNumber;
      obj.CurrencyAmount = detail.currencyAmount ?? 0;
      obj.IsRial = detail.nonRialStatus != null && detail.nonRialStatus > 0 ? false : true;
      obj.CurrencyFee = obj.IsRial == true ? 0 : detail.currencyFee;
      obj.CurrencyTypeBaseId = detail.currencyTypeBaseId;
      obj.NonRialStatus = detail.nonRialStatus ?? 0;
      obj.SheetUniqueNumber = detail.chequeSheetId != null && detail.chequeSheetId != 0 ? detail?.sheeSeqNumber : "0";
      obj.ChequeSheetId = detail.chequeSheetId;
      obj.Description = request.description;
      obj.BesCurrencyStatus = detail.besCurrencyStatus;
      obj.BedCurrencyStatus = detail.bedCurrencyStatus;
      accountingDoc.dataList.push(obj);
    });



    this.isLoading = true;

    let result;
    try {
      result = await this._mediator.send(<CreateAccountingDocumentCommand>accountingDoc);
    } catch {
      this.isLoading = false;
    }
    let voucherHeadIdRequest = new UpdateUpdateVoucherHeadIdCommand();


    voucherHeadIdRequest.receiveIds.push(request.id)

    // get the voucherHeadId
    voucherHeadIdRequest.voucherHeadId = (<any>result)[0].voucherHeadId;

    //  update voucherHeadId and isPending = false

    var isVoucherHeadId;
    try {
      isVoucherHeadId = await this._mediator.send(<UpdateUpdateVoucherHeadIdCommand>voucherHeadIdRequest);
    } catch {
      this.isLoading = false;
    }

    this.isLoading = false;
  }

  stripTimeFromDate(date: Date): Date {
    const strippedDate = new Date(date);
    strippedDate.setHours(0, 0, 0, 0);
    return strippedDate;
  }


  async submit() {

    let error = this.checkvalidations();
    if (error) return;

    await super.submit();
    this.reset();
  }

  checkvalidations(): boolean {

    // if (this.form.value.financialRequestDetails[0].description == null || this.form.value.financialRequestDetails[0].description == "")
    // this.form.controls.financialRequestDetails.controls[0].patchValue({
    //     descriptoin : this.form.value.financialRequestDetails[0].paymentCode
    // });

    var isError = false;
    this.form.value.financialRequestDetails.forEach((item: any) => {

      if (item.documentTypeBaseId == this.documents.ChequeSheet)
        item.amount = item.chequeSheet.totalCost;


      if (item.creditAccountReferenceId == null && item.isCreditAccountHead != true) {
        this.notificationService.showFailureMessage("حساب تفصیل واریز کننده وارد نشده است", 0);
        isError = true;
        return;
      }

      if (item.debitAccountReferenceId == null && item.isDebitAccountHead != true) {
        this.notificationService.showFailureMessage("حساب تفصیل دریافت کننده وارد نشده است", 0);
        isError = true;
        return;
      }



      if (item.creditAccountReferenceGroupId == null && item.isCreditAccountHead != true) {
        this.notificationService.showFailureMessage(" گروه تفصیل  واریز کننده وارد نشده است", 0);
        isError = true;
        return;
      }


      if (item.debitAccountReferenceGroupId == null && item.isDebitAccountHead != true) {
        this.notificationService.showFailureMessage(" گروه تفصیل دریافت کننده وارد نشده است", 0);
        isError = true;
        return;
      }


      if (item.creditAccountHeadId == null || item.debitAccountHeadId == null) {
        this.notificationService.showFailureMessage(" سرفصل حساب دریافت کننده یا واریز کننده وارد نشده است", 0);
        isError = true;
        return;
      }

      if (item.amount == null || item.amount == 0) {
        this.notificationService.showFailureMessage(" مبلغ به درستی وارد نشده است ", 0);
        isError = true;
        return;
      }

      if (item.financialReferenceTypeBaseId == null) {
        this.notificationService.showFailureMessage("  نوع مبنا به درستی وارد نشده است ", 0);
        isError = true;
        return;
      }

      // if (item.description == null || item.description == "" ){
      //   this.notificationService.showFailureMessage("  توضیحات به درستی وارد نشده است ",0);
      //   isError = true;
      //   return;S
      // }

      if (item.paymentCode == null && item.documentTypeBaseId != this.documents.ChequeSheet) {
        this.notificationService.showFailureMessage("  کد رهگیری به درستی وارد نشده است ", 0);
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
    if (error) return;

    var command = <UpdateCustomerReceiptCommand>this.form.getRawValue();


    var currencyTypeId = command.financialRequestDetails[0].currencyTypeBaseId;

    command.financialRequestDetails.forEach((item: any) => {

      item.currencyTypeBaseId = currencyTypeId;
    
        item.description = item.paymentCode
     
    });

    this.fillDescription();

    (<UpdateCustomerReceiptCommand>this.request).financialRequestDetails = command.financialRequestDetails;
    (<CreateCustomerReceiptCommand>this.request).attachments = this.attachments;
    if (!error) {
      this.isLoading = true;
      await this._mediator.send(<UpdateCustomerReceiptCommand>this.request);
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

  addReceipt() {


    let command = new CreateReceiptCommand();

    command.isRial = true;
    command.documentTypeBaseId = this.documents.Remittance;
    command.financialReferenceTypeBaseId = this.finantialReferencesEnums.PaymentReceipt;

    command.debitAccountHeadId = this.form.controls.financialRequestDetails.value[0].debitAccountHeadId;
    command.debitAccountReferenceGroupId = this.form.controls.financialRequestDetails.value[0].debitAccountReferenceGroupId;
    command.creditAccountReferenceGroupId = this.form.controls.financialRequestDetails.value[0].creditAccountReferenceGroupId;
    command.creditAccountHeadId = this.form.controls.financialRequestDetails.value[0].creditAccountHeadId;
    command.bedCurrencyStatus = this.form.controls.financialRequestDetails.value[0].bedCurrencyStatus;
    command.besCurrencyStatus = this.form.controls.financialRequestDetails.value[0].besCurrencyStatus;

    const receipt = <FormGroup>this.createForm(command, true);

    this.form.controls["financialRequestDetails"].push(receipt);
    const chequeSheet = <FormGroup>this.createForm(new CreateChequeSheetCommand(), true);
    (<FormGroup>receipt).controls['chequeSheet'] = chequeSheet;
    let indexReceipt = this.form.controls.financialRequestDetails.length - 1
    let latsReceipt = this.form.value.financialRequestDetails[indexReceipt - 1];

    receipt.patchValue({
      debitAccountHeadId: latsReceipt.debitAccountHeadId,
      debitAccountReferenceGroupId: latsReceipt.debitAccountReferenceGroupId,
      debitAccountReferenceId: latsReceipt.debitAccountReferenceId,

    });

  }

  addAttachments() {
    const dialogRef = this.dialog.open(CustomerReceiptAttachmentComponent, {
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


  removeItem(index: any): void {

    const formArray = <FormArray>this.form.controls.financialRequestDetails;

    const item = formArray.at(index);

    item?.get('isDeleted')?.setValue(true);

    // حذف آیتم از FormArray
    <FormArray>this.form.controls.financialRequestDetails.removeAt(index);
  }

  fillDescription() {

    let des = "";
    let paymentCode = "";
    if (this.form.value.financialRequestDetails[0].isRial == true && this.form.value.financialRequestDetails.length == 1 && this.form.value.financialRequestDetails[0].documentTypeBaseId != this.documents.ChequeSheet) {

      paymentCode = this.form.value.financialRequestDetails[0].paymentCode;
      if (this.form.value.documentNo > 0)
        des = `بابت دریافت شماره ردیف ${this.form.value.documentNo} و به شماره پیگیری  ` + paymentCode;
      else
        des = "بابت دریافت شماره ردیف |: و به شماره پیگیری  " + paymentCode;

    }
    else if (this.form.value.financialRequestDetails[0].isRial == true && this.form.value.financialRequestDetails.length > 1) {
      this.form.value.financialRequestDetails.forEach((item: any) => {

        paymentCode += item.paymentCode + " و ";
      });
      paymentCode = paymentCode.slice(0, -2);
      // paymentCode = this.form.value.financialRequestDetails[0].paymentCode;
      des = "بابت دریافت تجمیعی شماره ردیف |: و به شماره پیگیری  " + paymentCode + " (پرداخت تجمیعی) ";
    }

    else if (this.form.value.financialRequestDetails[0].documentTypeBaseId == this.documents.Cash && this.form.value.financialRequestDetails[0].isRial == this.currencyType.NonRial) {

      paymentCode = this.form.value.financialRequestDetails[0].paymentCode;
      let amount = this.form.value.financialRequestDetails[0].currencyAmount;
      let currencyTypeId = this.form.value.financialRequestDetails[0].currencyTypeBaseId;
      let currencyTypeTitle = this.customerReceiptArticleComponent.currencyTypes.find(x => x.id == currencyTypeId);
      let currencyFee = this.form.value.financialRequestDetails[0].currencyFee;

      des = " بابت دریافت از " + amount + " " + currencyTypeTitle?.title + " به نرخ  " + currencyFee + " از محل ارز حاصل از صادرات طی فرم عملیات مالی           پرداخت نقدی به صندوق ارزی";

    }

    else if (this.form.value.financialRequestDetails[0].documentTypeBaseId == this.documents.ChequeSheet) {
      var chequeNumber = this.form.value.financialRequestDetails[0].chequeSheet.sheetSeqNumber;
      des = "بابت دریافت چک  به شماره " + chequeNumber + " شماره ردیف |: "
    }

    else {
      let accountReference = this.form.value.financialRequestDetails[0].creditAccountReferenceId;
      let accountReferenceTitle = this.customerReceiptArticleComponent.creditAccountReferences.find(x => x.id == accountReference);
      paymentCode = this.form.value.financialRequestDetails[0].paymentCode;
      let amount = this.form.value.financialRequestDetails[0].currencyAmount;
      let currencyTypeId = this.form.value.financialRequestDetails[0].currencyTypeBaseId;
      let currencyTypeTitle = this.customerReceiptArticleComponent.currencyTypes.find(x => x.id == currencyTypeId);
      let currencyFee = this.form.value.financialRequestDetails[0].currencyFee;
      if (this.form.value.description == null || this.form.value.description.length == 0)
        des = "بابت دریافت از " + accountReferenceTitle?.title + " طبق نامه شماره " + paymentCode + " به مبلغ " + amount + " " + currencyTypeTitle?.title + " به نرخ " + currencyFee + " ارز                  " + "طی فرم عملیات مالی                 دریافت گردید";
      else
        des = this.form.value.description;
    }

    this.form.patchValue({
      description: des
    })
  }

}
