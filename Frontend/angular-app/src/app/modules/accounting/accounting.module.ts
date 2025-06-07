import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CodeRowDescriptionComponent} from './pages/base-values/code-row-description/code-row-description.component';
import {CodeVoucherGroupComponent} from './pages/base-values/code-voucher-group/code-voucher-group.component';
import {ComponentsModule} from "../../core/components/components.module";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AngularMaterialModule} from "../../core/components/material-design/angular-material.module";
import {AccountingRoutingModule} from "./accounting-routing.module";
import {
  CodeRowDescriptionDialogComponent
} from './pages/base-values/code-row-description/code-row-description-dialog/code-row-description-dialog.component';
import {
  CodeVoucherGroupDialogComponent
} from './pages/base-values/code-voucher-group/code-voucher-group-dialog/code-voucher-group-dialog.component';
import {
  CodeVoucherExtendTypeComponent
} from "./pages/base-values/code-voucher-extend-type/code-voucher-extend-type.component";
import {
  CodeVoucherExtendTypeDialogComponent
} from "./pages/base-values/code-voucher-extend-type/code-voucher-extend-type-dialog/code-voucher-extend-type-dialog.component";
import {
  AddAccountReferenceComponent
} from './pages/base-values/account-reference/add-account-reference/add-account-reference.component';
import {
  AccountReferencesListComponent
} from './pages/base-values/account-reference/account-references-list/account-references-list.component';
import {AccountHeadComponent} from "./pages/base-values/account-head/account-head.component";

import {AddVoucherHeadComponent} from './pages/actions/voucher-head/add-voucher-head/add-voucher-head.component';
import {VoucherHeadListComponent} from './pages/actions/voucher-head/voucher-head-list/voucher-head-list.component';
import {
  VoucherDetailsListComponent
} from './pages/actions/voucher-head/add-voucher-head/voucher-details-list/voucher-details-list.component';
import {StartVoucherComponent} from './pages/special-operations/start-voucher/start-voucher.component';
import {EndVoucherComponent} from './pages/special-operations/end-voucher/end-voucher.component';
import {LockVoucherHeadComponent} from './pages/special-operations/lock-voucher-head/lock-voucher-head.component';
import {
  RenumberVoucherHeadsComponent
} from './pages/special-operations/renumber-voucher-heads/renumber-voucher-heads.component';
import {
  InsertBetweenVoucherHeadsComponent
} from './pages/special-operations/insert-between-voucher-heads/insert-between-voucher-heads.component';

import {
  AutoVoucherFormulaListComponent
} from './pages/base-values/auto-voucher-formula/auto-voucher-formula-list/auto-voucher-formula-list.component';
import {
  AutoVoucherFormulaDialogComponent
} from './pages/base-values/auto-voucher-formula/auto-voucher-formula-list/auto-voucher-formula-dialog/auto-voucher-formula-dialog.component';
import {
  VoucherFormulaComponent
} from './pages/base-values/auto-voucher-formula/auto-voucher-formula-list/auto-voucher-formula-dialog/voucher-formula/voucher-formula.component';
import {MatIconModule} from "@angular/material/icon";
import {
  VoucherAttachmentsListComponent
} from './pages/actions/voucher-head/add-voucher-head/voucher-attachments-list/voucher-attachments-list.component';

import {AccountHeadReportComponent} from './pages/reporting/account-head-report/account-head-report.component';
import {AccountReviewReportComponent} from './pages/reporting/account-review-report/account-review-report.component';
import {BalanceReportComponent} from './pages/reporting/balance-report/balance-report.component';
import {JournalReportComponent} from './pages/reporting/journal-report/journal-report.component';
import {LedgerReportComponent} from './pages/reporting/ledger-report/ledger-report.component';
import {
  AccountHeadToAccountReferenceReportComponent
} from './pages/reporting/account-head-to-account-reference-report/account-head-to-account-reference-report.component';
import {
  AccountReferenceToAccountHeadReportComponent
} from './pages/reporting/account-reference-to-account-head-report/account-reference-to-account-head-report.component';
import {
  AccountingReportTemplateComponent
} from './components/accounting-report-template/accounting-report-template.component';
import {
  MoadianInvoicesListComponent
} from './pages/special-operations/moadian/moadian-invoices-list/moadian-invoices-list.component';
import {
  AddMoadianInvoiceComponent
} from './pages/special-operations/moadian/add-moadian-invoice/add-moadian-invoice.component';
import {
  MoadianInvoiceDetailsListComponent
} from './pages/special-operations/moadian/add-moadian-invoice/moadian-invoice-details-list/moadian-invoice-details-list.component';
import {
  MoadianInvoiceDetailDialogComponent
} from './pages/special-operations/moadian/add-moadian-invoice/moadian-invoice-detail-dialog/moadian-invoice-detail-dialog.component';
import {
  MoadianExcelImportDialogComponent
} from './pages/special-operations/moadian/moadian-invoices-list/moadian-excel-import-dialog/moadian-excel-import-dialog.component';
import {
  UpdateMoadianInvoicesStatusByIdsDialogComponent
} from './pages/special-operations/moadian/moadian-invoices-list/update-moadian-invoices-status-by-ids-dialog/update-moadian-invoices-status-by-ids-dialog.component';
import {
  MoadianVerificationCodeDialogComponent
} from './pages/special-operations/moadian/moadian-invoices-list/moadian-verification-code-dialog/moadian-verification-code-dialog.component';
import {
  CombineVoucherHeadsDialogComponent
} from './pages/special-operations/combine-voucher-heads-dialog/combine-voucher-heads-dialog.component';
import {
  TempVoucherHeadsListComponent
} from './pages/actions/voucher-head/voucher-head-list/temp-voucher-heads-list/temp-voucher-heads-list.component';
import {QReportComponent} from './pages/reporting/q-report/qreport.component';
import {ProfitAndLossReportComponent} from './pages/reporting/profit-and-loss-report/profit-and-loss-report.component';
import {
  MoveVoucherDetailsComponent
} from './pages/special-operations/move-voucher-details/move-voucher-details.component';
import {AnnualLedgerReportComponent} from './pages/reporting/annual-ledger-report/annual-ledger-report.component';
import {BankReportComponent} from './pages/reporting/bank-report/bank-report.component';
import {
  AccountHeadModalComponent
} from './pages/base-values/account-head/account-head-modal/account-head-modal.component';
import { AccountReferencesGroupComponent } from './pages/base-values/account-reference-group/account-references-group/account-references-group.component';
import { AccountReferencesGroupModalComponent } from './pages/base-values/account-reference-group/account-references-group-modal/account-references-group-modal.component';
import { VoucherHeadHistoryDialogComponent } from './components/voucher-head-history-dialog/voucher-head-history-dialog.component';
import { ApplicationRequestLogsComponent } from './pages/application-request-logs/application-request-logs.component';
import { SalaryExcelImportDialogComponent } from './pages/actions/voucher-head/add-voucher-head/salary-excel-import-dialog/salary-excel-import-dialog.component';
import { SSRSReportDialogComponent } from './components/ssrsreport-dialog/ssrsreport-dialog.component';

import { AddAutoVoucherFormulaComponent } from './pages/base-values/auto-voucher-formula/add-auto-voucher-formula/add-auto-voucher-formula.component';
import { CreateFormulaComponent } from './pages/base-values/auto-voucher-formula/add-auto-voucher-formula/create-formula/create-formula.component';
import { CreateConditionComponent } from './pages/base-values/auto-voucher-formula/add-auto-voucher-formula/create-condition/create-condition.component';
import {AngJsoneditorModule} from "@maaxgr/ang-jsoneditor";
import { VoucherDetailsReportComponent } from './pages/reporting/voucher-details-report/voucher-details-report.component';
import { LedgerReport2Component } from './pages/reporting/ledger-report2/ledger-report2.component';
import { LedgerReportAdvancedFilterComponent } from './pages/reporting/ledger-report2/ledger-report-advanced-filter/ledger-report-advanced-filter.component';

@NgModule({
  declarations: [
    AccountHeadComponent,
    CodeRowDescriptionComponent,
    CodeVoucherExtendTypeComponent,
    CodeVoucherGroupComponent,
    CodeRowDescriptionDialogComponent,
    CodeVoucherGroupDialogComponent,
    CodeVoucherExtendTypeDialogComponent,
    AddAccountReferenceComponent,
    AccountReferencesListComponent,
    AddAutoVoucherFormulaComponent,

    AddVoucherHeadComponent,
    VoucherHeadListComponent,
    VoucherDetailsListComponent,
    StartVoucherComponent,
    EndVoucherComponent,
    LockVoucherHeadComponent,
    RenumberVoucherHeadsComponent,
    InsertBetweenVoucherHeadsComponent,

    AutoVoucherFormulaListComponent,
    AutoVoucherFormulaDialogComponent,
    VoucherFormulaComponent,
    VoucherAttachmentsListComponent,
    AccountHeadReportComponent,
    AccountReviewReportComponent,
    BalanceReportComponent,
    JournalReportComponent,
    LedgerReportComponent,
    AccountHeadToAccountReferenceReportComponent,
    AccountReferenceToAccountHeadReportComponent,
    AccountingReportTemplateComponent,
    MoadianInvoicesListComponent,
    AddMoadianInvoiceComponent,
    MoadianInvoiceDetailsListComponent,
    MoadianInvoiceDetailDialogComponent,
    MoadianExcelImportDialogComponent,
    UpdateMoadianInvoicesStatusByIdsDialogComponent,
    MoadianVerificationCodeDialogComponent,
    CombineVoucherHeadsDialogComponent,
    TempVoucherHeadsListComponent,
    QReportComponent,
    ProfitAndLossReportComponent,
    MoveVoucherDetailsComponent,
    AnnualLedgerReportComponent,
    BankReportComponent,
    AccountHeadModalComponent,
    AccountReferencesGroupComponent,
    AccountReferencesGroupModalComponent,
    VoucherHeadHistoryDialogComponent,
    ApplicationRequestLogsComponent,
    SalaryExcelImportDialogComponent,
    CreateFormulaComponent,
    CreateConditionComponent,
    SSRSReportDialogComponent,
    VoucherDetailsReportComponent,
    LedgerReport2Component,
    LedgerReportAdvancedFilterComponent,
  ],
  imports: [
    CommonModule,
    ComponentsModule,
    ReactiveFormsModule,
    AngularMaterialModule,
    AccountingRoutingModule,
    FormsModule,
    MatIconModule,
    AngJsoneditorModule
  ],
  providers: [
  ]
})


export class AccountingModule {
}
