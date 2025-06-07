import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BursaryRoutingModule } from './bursary-routing.module';
import { AddChequeComponent } from './pages/cheque/add-cheque/add-cheque.component';
import { ChequeListComponent } from './pages/cheque/cheque-list/cheque-list.component';
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {ComponentsModule} from "../../core/components/components.module";
import { AddInvoicesComponent } from './pages/financial-request/payment/add-request-payment/add-invoices/add-invoices.component';
import { AddRequestPaymentComponent } from './pages/financial-request/payment/add-request-payment/add-request-payment.component';
import { AddReceiptComponent } from './pages/financial-request/receive/add-receipt/add-receipt.component';
import { CustomerReceiptArticleComponent } from './pages/financial-request/receive/add-receipt/customer-receipt-article/customer-receipt-article.component';
import { DetailComponent } from './pages/financial-request/receive/list/detail/detail.component';
import { ListComponent } from './pages/financial-request/receive/list/list.component';
import { AddReceiveChequeComponent } from './pages/financial-request/receive/cheque/add-receive-cheque.component';
import { ReceiveChequeList } from './pages/financial-request/receive/cheque/cheque-list/receive-cheque-list.component';
import { ReceiptsListInsertedByCustomersComponent } from './pages/financial-request/receive/receipts-list-inserted-by-customers/receipts-list-inserted-by-customers.component';
import { AttachmentDialogComponent } from './pages/financial-request/receive/receipts-list-inserted-by-customers/attachment-dialog/attachment-dialog.component';
import { RemoveReceiptWithMessageDialogComponent } from './pages/financial-request/receive/receipts-list-inserted-by-customers/remove-receipt-with-message-dialog/remove-receipt-with-message-dialog.component';
import { CustomerReceiptAttachmentComponent } from './pages/financial-request/receive/add-receipt/customer-receipt-attachment/customer-receipt-attachment.component';
import { ShowAttachmentComponent } from './pages/financial-request/receive/list/show-attachment/show-attachment.component';
import { AccumulateReceiptComponent } from './pages/financial-request/receive/accumulate-receipt/accumulate-receipt.component';
import { CounterDateTimeComponent } from './pages/financial-request/receive/add-receipt/counter-date-time/counter-date-time.component';
import { ReportChequeComponent } from './pages/cheque/report-cheque/report-cheque.component';
import { AddChequeAttachmentsComponent } from './pages/financial-request/receive/cheque/cheque-attachments/add-cheque-attachments.component';
import { SelectReferenceComponent } from './pages/cheque/report-cheque/select-reference/select-reference.component';
import { ChequeAttachmentsComponent } from './pages/cheque/report-cheque/cheque-attachments/cheque-attachments.component';
import { ShowChequeSheetAttachmentsComponent } from './pages/cheque/report-cheque/show-cheque-sheet-attachments/show-cheque-sheet-attachments.component';
import { ShowChequeDetailComponent } from './pages/cheque/report-cheque/show-cheque-detail/show-cheque-detail.component';
import { AddPaymentComponent } from './pages/financial-request/payment/add-payment/add-payment.component';
import { PaymentAttachmentsComponent } from './pages/financial-request/payment/add-payment/payment-attachments/payment-attachments.component';
import { PaymentArticleComponent } from './pages/financial-request/payment/add-payment/payment-article/payment-article.component';
import { SelectDateComponent } from './pages/cheque/report-cheque/select-date/select-date.component';
import { PaymentReceiveListComponent } from './pages/financial-request/payment/payment-receive-list/payment-receive-list.component';
import { BankBalanceDialogComponent } from './pages/financial-request/receive/list/bank-balance-dialog/bank-balance-dialog.component';
import { BankAccountsListComponent } from './pages/bank/bank-accounts-list/bank-accounts-list.component';
import { ChequeBookComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-book.component';
import { ChequeBooksSheetsComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-books-sheets/cheque-books-sheets.component';
import { CancelChequeBooksSheetsDialogComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-books-sheets/cancel-cheque-books-sheets-dialog/cancel-cheque-books-sheets-dialog.component';
import { ChequeBookDialogComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-book-dialog/cheque-book-dialog.component';
import { AddSayyadNoChequeBooksSheetsDialogComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-books-sheets/add-sayyad-no-cheque-books-sheets-dialog/add-sayyad-no-cheque-books-sheets-dialog.component';
import { ChequeBookSheetComponent } from './pages/bank/bank-accounts-list/cheque-book/cheque-book-sheet/cheque-book-sheet.component';
import { PayableDocumentsComponent } from './pages/payable-documents/payable-documents.component';
import { AddPayableDocumentComponent } from './pages/payable-documents/add-payable-document/add-payable-document.component';
import { PayableItemsComponent } from './pages/payable-documents/add-payable-document/payable-items/payable-items.component';
import {NgSelectModule} from "@ng-select/ng-select";
import { PayableItemDetailComponent } from './pages/payable-documents/add-payable-document/payable-item-detail/payable-item-detail.component';
import {ScrollingModule} from "@angular/cdk/scrolling";


@NgModule({
  declarations: [
    AddChequeComponent,
    ChequeListComponent,
    AddRequestPaymentComponent,
    AddInvoicesComponent,
    ListComponent,
    AddReceiptComponent,
    AddReceiveChequeComponent,
    ReceiveChequeList,
    DetailComponent,
    CustomerReceiptArticleComponent,
    ReceiptsListInsertedByCustomersComponent,
    AttachmentDialogComponent,
    RemoveReceiptWithMessageDialogComponent,
    CustomerReceiptAttachmentComponent,
    ShowAttachmentComponent,
    AccumulateReceiptComponent,
    CounterDateTimeComponent,
    ReportChequeComponent,
    AddChequeAttachmentsComponent,
    ChequeAttachmentsComponent,
    SelectReferenceComponent,
    ShowChequeSheetAttachmentsComponent,
    ShowChequeDetailComponent,
    AddPaymentComponent,
    PaymentAttachmentsComponent,
    PaymentArticleComponent,
    SelectDateComponent,
    PaymentReceiveListComponent,
    BankBalanceDialogComponent,
    BankAccountsListComponent,
    ChequeBookComponent,
    ChequeBooksSheetsComponent,
    CancelChequeBooksSheetsDialogComponent,
    ChequeBookDialogComponent,
    AddSayyadNoChequeBooksSheetsDialogComponent,
    ChequeBookSheetComponent,
    PayableDocumentsComponent,
    AddPayableDocumentComponent,
    PayableItemsComponent,
    PayableItemDetailComponent,

  ],
    imports: [
        CommonModule,
        BursaryRoutingModule,
        ComponentsModule,
        AngularMaterialModule,
        ReactiveFormsModule,
        FormsModule,
        NgSelectModule,
        ScrollingModule,

    ]
})
export class BursaryModule { }
