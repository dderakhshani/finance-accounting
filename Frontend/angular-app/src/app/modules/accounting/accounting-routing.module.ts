import {NgModule} from '@angular/core';
import {RouterModule, Routes} from "@angular/router";
import {CodeRowDescriptionComponent} from "./pages/base-values/code-row-description/code-row-description.component";
import {CodeVoucherGroupComponent} from "./pages/base-values/code-voucher-group/code-voucher-group.component";
import {
  CodeVoucherExtendTypeComponent
} from "./pages/base-values/code-voucher-extend-type/code-voucher-extend-type.component";
import {
  AddAccountReferenceComponent
} from "./pages/base-values/account-reference/add-account-reference/add-account-reference.component";
import {
  AccountReferencesListComponent
} from "./pages/base-values/account-reference/account-references-list/account-references-list.component";
import {AccountHeadComponent} from "./pages/base-values/account-head/account-head.component";

import {VoucherHeadListComponent} from "./pages/actions/voucher-head/voucher-head-list/voucher-head-list.component";
import {AddVoucherHeadComponent} from "./pages/actions/voucher-head/add-voucher-head/add-voucher-head.component";
import {StartVoucherComponent} from "./pages/special-operations/start-voucher/start-voucher.component";
import {EndVoucherComponent} from "./pages/special-operations/end-voucher/end-voucher.component";
import {
  RenumberVoucherHeadsComponent
} from "./pages/special-operations/renumber-voucher-heads/renumber-voucher-heads.component";
import {LockVoucherHeadComponent} from "./pages/special-operations/lock-voucher-head/lock-voucher-head.component";
import {
  InsertBetweenVoucherHeadsComponent
} from "./pages/special-operations/insert-between-voucher-heads/insert-between-voucher-heads.component";


import {
  AutoVoucherFormulaListComponent
} from "./pages/base-values/auto-voucher-formula/auto-voucher-formula-list/auto-voucher-formula-list.component";
import {AccountHeadReportComponent} from "./pages/reporting/account-head-report/account-head-report.component";
import {AccountReviewReportComponent} from "./pages/reporting/account-review-report/account-review-report.component";
import {BalanceReportComponent} from "./pages/reporting/balance-report/balance-report.component";
import {JournalReportComponent} from "./pages/reporting/journal-report/journal-report.component";
import {LedgerReportComponent} from "./pages/reporting/ledger-report/ledger-report.component";
import {
  AccountHeadToAccountReferenceReportComponent
} from "./pages/reporting/account-head-to-account-reference-report/account-head-to-account-reference-report.component";
import {
  AccountReferenceToAccountHeadReportComponent
} from "./pages/reporting/account-reference-to-account-head-report/account-reference-to-account-head-report.component";
import {
  MoadianInvoicesListComponent
} from "./pages/special-operations/moadian/moadian-invoices-list/moadian-invoices-list.component";
import {
  AddMoadianInvoiceComponent
} from "./pages/special-operations/moadian/add-moadian-invoice/add-moadian-invoice.component";
import {AccountingModuleResolver} from "./accounting-module.resolver";
import {QReportComponent} from './pages/reporting/q-report/qreport.component';
import {ProfitAndLossReportComponent} from './pages/reporting/profit-and-loss-report/profit-and-loss-report.component';
import {AnnualLedgerReportComponent} from './pages/reporting/annual-ledger-report/annual-ledger-report.component';
import {BankReportComponent} from './pages/reporting/bank-report/bank-report.component';
import {
  AccountReferencesGroupComponent
} from "./pages/base-values/account-reference-group/account-references-group/account-references-group.component";
import {ApplicationRequestLogsComponent} from "./pages/application-request-logs/application-request-logs.component";
import {
  AddAutoVoucherFormulaComponent
} from './pages/base-values/auto-voucher-formula/add-auto-voucher-formula/add-auto-voucher-formula.component';
import {VoucherDetailsReportComponent} from "./pages/reporting/voucher-details-report/voucher-details-report.component";
import {LedgerReport2Component} from "./pages/reporting/ledger-report2/ledger-report2.component";


const routes: Routes = [
  {
    path: '',
    resolve: {init: AccountingModuleResolver},
    data: {
      title: 'Accounting'
    },
    children: [
      {
        path: 'logs',
        component: ApplicationRequestLogsComponent,
        data: {
          title: 'accounting logs',
          id: '#logs',
          isTab: true
        },
      },
      {
        path: 'accountHead',
        component: AccountHeadComponent,
        data: {
          title: 'سرفصل حساب ها',
          id: '#accountHead',
          isTab: true
        },
      },
      {
        path: 'accountReferences',
        data: {
          title: 'تفصیل شناور',
          id: '#accountReferences',
          isTab: true
        },
        children: [
          {
            path: 'add',
            component: AddAccountReferenceComponent,
            data: {
              title: 'تعریف تفصیل شناور',
              id: '#addAccountReference',
              isTab: true
            },
          },
          {
            path: 'list',
            component: AccountReferencesListComponent,
            data: {
              title: 'فهرست تفصیل شناور',
              id: '#accountReferencesList',
              isTab: true
            },
          }
        ]
      },
      {
        path: 'accountReferenceGroup',
        component: AccountReferencesGroupComponent,
        data: {
          title: 'گروه های تفصیل شناور',
          id: '#accountReferenceGroup',
          isTab: true
        },
      },
      {
        path: 'codeRowDescription',
        component: CodeRowDescriptionComponent,
        data: {
          title: 'شرح استاندارد',
          id: '#codeRowDescription',
          isTab: true
        },
      },
      {
        path: 'codeVoucherExtendType',
        component: CodeVoucherExtendTypeComponent,
        data: {
          title: 'انواع سندهای خاص',
          id: '#codeVoucherExtendType',
          isTab: true
        },
      },
      {
        path: 'codeVoucherGroup',
        component: CodeVoucherGroupComponent,
        data: {
          title: 'گروه سند',
          id: '#codeVoucherGroup',
          isTab: true
        },
      },
      {
        path: 'autoVoucherFormula',
        component: AutoVoucherFormulaListComponent,
        data: {
          title: 'فرمول ها',
          id: '#autoVoucherFormula',
          isTab: true
        },
        children: [
          {
            path: 'add',
            component: AddAutoVoucherFormulaComponent,
            data: {
              title: 'تعریف فرمول',
              id: '#addAutoVoucherFormula',
              isTab: true
            },
          },
          {
            path: 'list',
            component: AutoVoucherFormulaListComponent,
            data: {
              title: 'لیست فرمول',
              id: '#autoVoucherFormulas',
              isTab: true
            },
          }
        ]
      },
      {
        path: 'voucherHead',
        data: {
          title: 'عملیات حسابداری',
          id: '#voucherHead',
          isTab: false
        },
        children: [
          {
            path: 'add',
            component: AddVoucherHeadComponent,
            data: {
              title: 'درج / صدور سند',
              id: '#addVoucherHead',
              isTab: true
            },
          },
          {
            path: 'list',
            component: VoucherHeadListComponent,
            data: {
              title: 'فهرست اسناد',
              id: '#voucherHeadList',
              isTab: true
            },
          }
        ]
      },
      {
        path: 'reporting',
        data: {
          title: 'گزارشات حسابداری',
          id: '#accountingReports',
          isTab: false
        },
        children: [
          {
            path: 'voucherDetails',
            component: VoucherDetailsReportComponent,
            data: {
              title: 'گزارش تراز',
              id: '#voucherDetailReport',
              isTab: true
            },
          },
          {
            path: 'accountHeadReport',
            component: AccountHeadReportComponent,
            data: {
              title: 'سرفصل حساب',
              id: '#accountHeadReport',
              isTab: true
            },
          },
          {
            path: 'accountReviewReport',
            component: AccountReviewReportComponent,
            data: {
              title: 'مرور حساب ها',
              id: '#accountReviewReport',
              isTab: true
            },
          },
          {
            path: 'balanceReport',
            component: BalanceReportComponent,
            data: {
              title: 'گزارش تراز',
              id: '#balanceReport',
              isTab: true
            },
          },
          {
            path: 'journalReport',
            component: JournalReportComponent,
            data: {
              title: 'دفتر روزنامه',
              id: '#journalReport',
              isTab: true
            },
          },
          {
            path: 'ledgerReport',
            component: LedgerReportComponent,
            data: {
              title: 'ریز گردش',
              id: '#ledgerReport',
              isTab: true
            },
          },
          {
            path: 'ledgerReport2',
            component: LedgerReport2Component,
            data: {
              title: 'ریز گردش 2',
              id: '#ledgerReport2',
              isTab: true
            },
          },
          {
            path: 'q-Report',
            component: QReportComponent,
            data: {
              title: 'تفصیلی',
              id: '#q-Report',
              isTab: true
            },
          },
          {
            path: 'profitandlossreport',
            component: ProfitAndLossReportComponent,
            data: {
              title: 'سود وزیان',
              id: '#profitandlossreport',
              isTab: true
            },
          },
          {
            path: 'annualledgerreport',
            component: AnnualLedgerReportComponent,
            data: {
              title: 'دفتر کل',
              id: '#annualledgerreport',
              isTab: true
            },
          },
          {
            path: 'bankreport',
            component: BankReportComponent,
            data: {
              title: 'گزارش بانک',
              id: '#bankreport',
              isTab: true
            },
          },
          {
            path: 'accountHeadToAccountReferenceReport',
            component: AccountHeadToAccountReferenceReportComponent,
            data: {
              title: 'حساب به عطف',
              id: '#accountHeadToReferenceReport',
              isTab: true
            },
          },
          {
            path: 'accountReferenceToAccountHeadReport',
            component: AccountReferenceToAccountHeadReportComponent,
            data: {
              title: 'عطف به حساب',
              id: '#referenceToAccountHeadReport',
              isTab: true
            },
          },
        ]
      },
      {
        path: 'specialOperations',
        data: {
          title: 'عملیات ویژه',
          id: '#specialOperations',
          isTab: false
        },
        children: [
          {
            path: 'startVoucher',
            component: StartVoucherComponent,
            data: {
              title: 'سند افتتاحیه',
              id: '#startVoucher',
              isTab: true
            }
          },
          {
            path: 'endVoucher',
            component: EndVoucherComponent,
            data: {
              title: 'عملیات آخر سال',
              id: '#endVoucher',
              isTab: true
            }
          },
          {
            path: 'renumber',
            component: RenumberVoucherHeadsComponent,
            data: {
              title: 'شماره گذاری مجدد اسناد',
              id: '#renumber',
              isTab: true
            }
          },
          {
            path: 'lockVoucher',
            component: LockVoucherHeadComponent,
            data: {
              title: ' قفل کردن اسناد ',
              id: '#lockVoucher',
              isTab: true
            }
          },
          {
            path: 'insertBetweenVouchers',
            component: InsertBetweenVoucherHeadsComponent,
            data: {
              title: '  درج میان اسناد  ',
              id: '#insertBetweenVouchers',
              isTab: true
            }
          },
          {
            path: 'moadianInvoicesList',
            component: MoadianInvoicesListComponent,
            data: {
              title: 'فهرست صورتحساب ها',
              id: '#moadianInvoicesList',
              isTab: true
            }
          },
          {
            path: 'addMoadianInvoice',
            component: AddMoadianInvoiceComponent,
            data: {
              title: 'ثبت صورتحساب',
              id: '#addMoadianInvoice',
              isTab: true
            }
          },
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountingRoutingModule {
}
