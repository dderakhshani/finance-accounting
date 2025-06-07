import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddChequeComponent } from "./pages/cheque/add-cheque/add-cheque.component";
import { AddRequestPaymentComponent } from "./pages/financial-request/payment/add-request-payment/add-request-payment.component";
import { AccumulateReceiptComponent } from "./pages/financial-request/receive/accumulate-receipt/accumulate-receipt.component";
import { AddReceiptComponent } from "./pages/financial-request/receive/add-receipt/add-receipt.component";
import { AddReceiveChequeComponent } from "./pages/financial-request/receive/cheque/add-receive-cheque.component";
import { ListComponent } from "./pages/financial-request/receive/list/list.component";
import { ReceiptsListInsertedByCustomersComponent } from "./pages/financial-request/receive/receipts-list-inserted-by-customers/receipts-list-inserted-by-customers.component";
import { ReportChequeComponent } from "./pages/cheque/report-cheque/report-cheque.component";
import { AddPaymentComponent } from "./pages/financial-request/payment/add-payment/add-payment.component";
import { PaymentReceiveListComponent } from "./pages/financial-request/payment/payment-receive-list/payment-receive-list.component";
import {BankAccountsListComponent} from "./pages/bank/bank-accounts-list/bank-accounts-list.component";
import {ChequeBookComponent} from "./pages/bank/bank-accounts-list/cheque-book/cheque-book.component";
import {
  ChequeBookSheetComponent
} from "./pages/bank/bank-accounts-list/cheque-book/cheque-book-sheet/cheque-book-sheet.component";
import {PayableDocumentsComponent} from "./pages/payable-documents/payable-documents.component";
import {
  AddPayableDocumentComponent
} from "./pages/payable-documents/add-payable-document/add-payable-document.component";



const routes: Routes = [
  {
    path: '',
    data: {
      title: 'bursary',
    },
    children: [
      {
        path: 'bank',
        data: {
          title: "حساب های بانکی",
          id: '#bank',
          isTab: true,
        },

        children: [
          {
            path: 'bankAccountsList',
            component: BankAccountsListComponent,
            data: {
              title: "حساب های بانکی",
              id: '#bankAccountsList',
              isTab: true,

            },
          },
          {
            path: 'chequeBook',
            component: ChequeBookComponent,
            data: {
              title: "دسته چک",
              id: '#chequeBook',
              isTab: true,

            },
          },
          {
            path: 'chequeBookSheet',
            component: ChequeBookSheetComponent,
            data: {
              title: "چک ها",
              id: '#chequeBookSheet',
              isTab: true,
            },
          },

        ],


      },
      {
        path: 'payableDocument',
        data: {
          title: "اسناد پرداختی",
          id: '#payableDocument',
          isTab: true,
        },

        children: [
          {
            path: 'list',
            component: PayableDocumentsComponent,
            data: {
              title: "لیست اسناد پرداختی",
              id: '#payableDocuments',
              isTab: true,

            },
          },
          {
            path: 'add',
            component: AddPayableDocumentComponent,
            data: {
              title: "ثبت اسناد پرداختی",
              id: '#addPayableDocuments',
              isTab: true,

            },
          },
        ]
      },
      {
        path: 'cheques',
        component: AddChequeComponent,
        data: {
          title: "چک ها",
          id: "#cheques",
          isTab: true,
        },
      },

      {
        path: "requestPayments",
        component: AddRequestPaymentComponent,
        data: {
          title: "دستور پرداخت ها",
          id: "#requestPayments",
          isTab: true,
        },
      },



      {
        path: "bursaryDocuments",
        component: ListComponent,
        data: {
          title: "اسناد خزانه داری",
          id: "#bursaryDocuments",
          isTab: true,
        },
      },


      {
        path: 'customerReceive',
        data: {
          title: "دریافت ها",
          id: '#customerReceive',
          isTab: true,
        },

        children: [
          {
            path: 'accumulateReceipts',
            component: AccumulateReceiptComponent,
            data: {
              title: 'دریافت تجمیعی',
              id: '#accumulateReceipts',
              isTab: true,

            },
          },

          {
            path: 'customerReceipt',
            component: AddReceiptComponent,
            data: {
              title: 'رسید دریافت',
              id: '#customerReceipt',
              isTab: true,

            },
          },

          {
            path: "receiptCheques",
            component: AddReceiveChequeComponent,
            data: {
              title: "ثبت چک های دریافتی",
              id: "#receiptCheques",
              isTab: true,
            }
          },
          {
            path: "reportCheques",
            component: ReportChequeComponent,
            data: {
              title: "لیست چک های ثبت شده",
              id: "#reportCheques",
              isTab: true,
            }
          },
          {
            path: "accumulateReceipts",
            component: AddReceiptComponent,
            data: {
              title: "رسید دریافت",
              id: "#accumulateReceipts",
              isTab: true,
            },
          },

          {
            path: "receiptsListInsertedByCustomers",
            component: ReceiptsListInsertedByCustomersComponent,
            data: {
              title: "رسید های ثبت شده مشتری",
              id: "#receiptsListInsertedByCustomers",
              isTab: true,
            },
          },
        ],
      },

{
  path: 'requestPayments',
  data: {
    title: "پرداخت ها",
    id: '#requestPayments',
    isTab: true,
  },
  children:[
    {
      path: 'addpayment',
      component: AddPaymentComponent,
      data: {
        title: 'رسید پرداخت',
        id: '#addpayment',
        isTab: true,

      },
    },
    {
      path: 'paymentlist',
      component: PaymentReceiveListComponent,
      data: {
        title: 'دفتر عملیات پرداخت',
        id: '#paymentlist',
        isTab: true,

      },
    },

  ]
},


    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class BursaryRoutingModule { }
