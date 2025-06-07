import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { AddTemporaryReceiptComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-group/add-temporary-receipt.component';
import { InventoryRoutingModule } from "./inventory-routing.module";
import { MatFormFieldModule } from "@angular/material/form-field";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { ComponentsModule } from "../../core/components/components.module";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { AngularMaterialModule } from "../../core/components/material-design/angular-material.module";
import { TemporaryReceiptItemsComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-group/temporary-receipt-items/temporary-receipt-items.component';
import { WarehouseComponent } from './pages/warehouse/warehouse.component';
import { WarehouseDialogComponent } from './pages/warehouse/warehouse-dialog/warehouse-dialog.component';
import { WarehouseLayoutComponent } from './pages/warehouse-layout/warehouse-layout.component';
import { AddWarehouseLayoutDialogComponent } from './pages/warehouse-layout/add-warehouse-layout-dialog/add-warehouse-layout-dialog.component';
import { UpdateWarehouseLayoutDialogComponent } from './pages/warehouse-layout/update-warehouse-layout-dialog/update-warehouse-layout-dialog.component';
import { TemporaryReceiptPrintComponent } from './pages/receipt-operations/add-temporary-receipt/temporary-receipt-print/temporary-receipt-print.component';
import { LayoutListComponent } from './pages/component/layout-tree-list/layout-list.component';
import { ComboTreeComponent } from './pages/component/combo-tree/combo-tree.component';
import { ReceiptListComponent } from './pages/receipt-list/receipt-list.component';
import { TemporaryReceiptListComponent } from './pages/receipt-list/temporary-receipt-list/temporary-receipt-list.component';
import { DirectReceiptListComponent } from './pages/receipt-list/direct-receipt-list/direct-receipt-list.component';
import { WarehouseLayoutsCommodityHistoryListComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-history-list/warehouse-layouts-commodity-history-list.component';
import { WarehouseLayoutsCommodityQuantityListComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-quantity-list/warehouse-layouts-commodity-quantity-list.component';
import { ChangeWarehouseLayoutsDialogComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-quantity-list/warehouse-layouts-change-dialog/warehouse-layouts-change-dialog.component';
import { RialsReceiptListComponent } from './pages/receipt-list/Rials-receipt-list/Rials-receipt-list.component';
import { RialsReceiptComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt.component';
import { RialsReceiptDetailsComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt-details/Rials-receipt-details.component';
import { RialsReceiptItemsComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt-details/Rials-receipt-items/Rials-receipt-items.component';
import { WarehouseStocksAvailableCommodityComponent } from './pages/warehouse-stocks-list/warehouse-stocks-available-commodity/warehouse-stocks-available-commodity.component';
import { RecieptWarhouseLayoutComponent } from './pages/receipt-operations/placement-warehouse-direct-receipt/direct-warhouse-layout/direct-warhouse-layout.component';
import { PlacementWarehouseDirectReceiptComponent } from './pages/receipt-operations/placement-warehouse-direct-receipt/placement-warehouse-direct-receipt.component';
import { TemporaryReceiptItemsSingleRowComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-single-row/temporary-receipt-items/temporary-receipt-items-single-row.component';
import { AddTemporaryReceiptSingleRowComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-single-row/add-temporary-receipt-single-row.component';
import { ArchiveReceiptListComponent } from './pages/receipt-list/archive-receipt-list/archive-receipt-list.component';
import { LeaveReceiptListComponent } from './pages/receipt-list/leave-receipt-list/leave-receipt-list.component';
import { EditWarehouseLayoutsDialogComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-quantity-list/warehouse-layouts-edit-dialog/warehouse-layouts-edit-dialog.component';
import { MechanizedDocumentComponent } from './pages/receipt-operations/convert-to-mechanized-document/mechanized-document.component';
import { DocumentAccountingDatailsComponent } from './pages/receipt-operations/convert-to-mechanized-document/document-accounting-details/document-accounting-details.component';
import { ComboSearchComponent } from './pages/component/combo-search/combo-search.component';
import { AddManualTemporaryReceiptComponent } from './pages/receipt-operations/add-temporary-receipt/add-manual-temporary-receipt/add-manual-temporary-receipt.component';
import { ComboAccountRefrenceComponent } from './pages/component/combo-account-refrence/combo-account-refrence.component';
import { ComboTagComponent } from './pages/component/combo-tag/combo-tag.component';
import { ComboCommodityComponent } from './pages/component/combo-commodity/combo-commodity.component';

import { requesBuyListComponent } from './pages/request-list/reques-buy-list/reques-buy-list.component';
import { ManualItemsComponent } from './pages/component/add-manual-items/add-manual-items.component';
import { ComboWarhouseTreeComponent } from './pages/component/combo-warhouse-tree/combo-warhouse-tree.component';
import { ReceiptDetailsComponent } from './pages/component/receipt-details/receipt-details.component';
import { ComboInvoiceComponent } from './pages/component/combo-invoice/combo-invoice.component';
import { ComboCodeVoucherGroupsComponent } from './pages/component/combo-code-voucher-groups/combo-code-voucher-groups.component';
import { AddCommoditySerialDialog } from './pages/component/commodity-serial-dialog/add-commodity-serial-dialog.component';

import { ComboCommodityBomsComponent } from './pages/component/combo-commodity-boms/combo-commodity-boms.component';
import { leavingPartWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-part/leaving-part-warehouse.component';
import { WarehouseLayoutsAddInventoryDialogComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-part/warehouse-layouts-add-inventory-dialog/warehouse-layouts-add-inventory-dialog.component';
import { MaterilaReceiptListComponent } from './pages/receipt-list/material-receipt-list/material-receipt-list.component';
import { AddRequestBuyComponent } from './pages/request-operations/add-request-buy/add-request-buy.component';
import { AddRequestCommodityComponent } from './pages/request-operations/add-request-commodity/add-request-commodity.component';
import { RequesReciveCommodityListComponent } from './pages/request-list/reques-recive-commodity-list/reques-recive-commodity-list.component';
import { LeavingAddMarerialWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-material/add-materail-warehouse/leaving-add-material-warehouse.component';
import { MaterialItemsComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-material/material-items/material-items.component';
import { UpdateMarerialWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-material/update-materail-warehouse/update-materail-warehouse.component';

import { UnitCommodityQuotaDialogComponent } from './pages/unitCommodityQuota/unitCommodityQuota-dialog/unitCommodityQuota-dialog.component';
import { UnitCommodityQuotaListComponent } from './pages/unitCommodityQuota/unitCommodityQuota-list.component';
import { ComboSearchMultiperItemsComponent } from './pages/component/combo-search-multiper-items/combo-search-multiper-items.component';
import { PersonsDebitedCommodityListComponent } from './pages/persons-debited-commodities/persons-debited-commodities-list.component';
import { UpdateReturnToWarehousePersonsDebitedDialogComponent } from './pages/persons-debited-commodities/return-to-warehouse-persons-debited-commodities-dialog/return-to-warehouse-persons-debited-commodities-dialog.component';
import { UpdateNewPersonsDebitedDialogComponent } from './pages/persons-debited-commodities/return-new-person-persons-debited-commodities-dialog/return-new-persons-debited-commodities-dialog.component';
import { AssetsListComponent } from './pages/assets/assets-list/assets-list.component';
import { AssetsCommoditySerialDialog } from './pages/assets/assets-list/assets-commodity-serial-dialog/assets-commodity-serial-dialog.component';

import { ComboAccountHeadTreeComponent } from './pages/component/combo-account-head-tree/combo-account-head-tree.component';
import { DocumentItemsBomDialog } from './pages/component/document-Items-bom-dialog/document-Items-bom-dialog.component';
import { LeavingCommodityWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-commodity/leaving-commodity-warehouse.component';


import { AddStartDocumentReceiptComponent } from './pages/receipt-operations/add-start-document-receipt/add-start-document-receipt.component';


import { CommodityCategoriesTreeComponent } from './pages/component/combo-commodity-categories-tree/combo-commodity-categories-tree.component';

import { UpdateAttachmentAssetsDialogComponent } from './pages/persons-debited-commodities/asset-attachments-dialog/asset-attachments-dialog.component';
import { MechanizedDocumentConfirmEditComponent } from './pages/receipt-operations/mechanized-document_confirm_edit/mechanized-document_confirm_edit.component';
import { AuditListComponent } from './pages/component/audit-list/audit-list.component';
import { RequestPrintComponent } from './pages/request-operations/request-print/request-print.component';
import { requesBuyMadeListComponent } from './pages/request-list/reques-buy-made-list/reques-buy-made-list.component';
import { AddAccessToWarehouseComponent } from './pages/add-access-to-warehouse/add-access-to-warehouse.component';
import { ExportToExcelTejaratSystemComponent } from './pages/reports/export-to excel/export-to excel-tejarat-system/export-to-excel-tejarat-system.component';
import { GetMonthlyEntryToWarehouseComponent } from './pages/reports/monthly-entry-to-warehouse/monthly-entry-to-warehouse.component';
import { dailyEntryToWarehouseListComponent } from './pages/reports/daily-entry-to-warehouse/daily-entry-to-warehouse-list.component';
import { CommodityReportsComponent } from './pages/reports/available-commodity-reports/commodity-reports/commodity-reports.component';

import { TableFilterComponent } from './pages/component/table-filter/table-filter.component';
import { tablePaggingComponent } from './pages/component/table-pagging/table-pagging.component';
import { ExportToExcelTadbirSystemComponent } from './pages/reports/export-to excel/export-to excel-tadbir-system/export-to excel-tadbir-system.component';
import { UploaderExcelComponent } from './pages/component/uploader-excel/uploader-excel.component';
import { ImportTadbirInventoryExcelValidateComponent } from './pages/reports/import-tadbir-inventory-excel-validate/import-tadbir-inventory-excel-validate.component';
import { CommodityReceiptReportsRialComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports-Rial/commodity-receipt-reports-Rial.component';
import { CommodityReportsRialComponent } from './pages/reports/available-commodity-reports/commodity-reports-Rial/commodity-reports-Rial.component';
import { CommodityReceiptReportsComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports/commodity-receipt-reports.component';
import { warehouseStockReportsComponent } from './pages/reports/warehouse-stocks/warehouse-stock/warehouse-stock.component';

import { WarehouseReceiptsBookComponent } from './pages/reports/warehouse-receipts-books/warehouse-receipts-book/warehouse-receipts-book.component';
import { WarehouseReceiptsBookRilaComponent } from './pages/reports/warehouse-receipts-books/warehouse-receipts-book-Rial/warehouse-receipts-book-Rial.component';
import { AddRequestReturnCommodityComponent } from './pages/request-operations/add-request-return-commodity/add-request-return-commodity.component';
import { ReceiptsInvoiceComponent } from './pages/reports/receipts-invoice/receipts-invoice.component';
import { AddTemporaryReceiptProductComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-product/add-temporary-receipt-product.component';
import { TemporaryReceiptItemsProductComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-product/temporary-receipt-items-product/temporary-receipt-items-product.component';
import { LeavingProductComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-product/leaving-product.component';
import { LeavingItemsProductComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-product/leaving-receipt-items-product/leaving-receipt-items-product.component';
import { UpdateQuantityDialogComponent } from './pages/component/update-quantity-dialog/update-quantity-dialog.component';

import { ComboAddSelectBaseValueComponent } from './pages/component/combo-add-select-base-value/combo-add-select-base-value.component';
import { NgxMaskModule } from 'ngx-mask';
import { correctionRequestListComponent } from './pages/component/correction-request/correction-request.component';
import { ButtonSearchComponent } from './pages/component/button-search/button-search.component';
import { MechanizedReciptListComponent } from './pages/reports/mechanized-receipt-list/mechanized-receipt-list.component';
import { WarehouseLayoutsCommodityHistoryComponent } from './pages/reports/warehouse-layouts-commodity-history/warehouse-layouts-commodity-history.component';
import { InvoiceBuyComponent } from './pages/receipt-operations/invoice/invoice-buy/invoice-buy.component';
import { InvoiceBuyCurrencyComponent } from './pages/receipt-operations/invoice/invoice-buy-currency/invoice-buy-currency.component';
import { DocumentHeadExtraCostDialogComponent } from './pages/component/documents-extra-cost-dialog/documents-extra-cost-dialog.component';
import { warehouseStockReportsRialComponent } from './pages/reports/warehouse-stocks/warehouse-stock-Rial/warehouse-stock-Rial.component';



import { ReceiptForAddExteraCostListComponent } from './pages/receipt-list/receipt-for-add-extera-cost-list/receipt-for-add-extera-cost-list.component';
import { FreightPaysComponent } from './pages/reports/freight-pays/freight-pays.component';

import { SplitCommodityQuantityDialogComponent } from './pages/component/split-quantity-dialog/split-quantity-dialog.component';
import { MakeProductPriceComponent } from './pages/reports/make-product-price-list/make-product-price-list.component';
import { FilesByPaymentNumberComponent } from './pages/component/files-by-payment-number/files-by-payment-number.component';
import { CommoditySerialViewDialog } from './pages/component/commodity-serial-view-dialog/commodity-serial-view-dialog.component';
import { CommodityCostComponent } from './pages/reports/commodity-cost/commodity-cost.component';
import { UpdateBarcodeDialogComponent } from './pages/persons-debited-commodities/update-barcode-dialog/update-barcode-dialog.component';
import { CommodityReportsWithWarehouseComponent } from './pages/reports/commodity-reports-with-warehouse-list/commodity-reports-with-warehouse-list.component';
import { RialsDebitDetailsDialogComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt-details/Rials-debit-details-dialog/Rials-debit-details-dialog.component';
import { WarehouseLayoutsDocumentLeavingHistoryListComponent } from './pages/reports/warehouse-leaving/warehouse-layouts-document-leaving-history-list/warehouse-layouts-document-leaving-history-list.component';
import { WarehouseLayoutsDocumentLeavingHistoryListByPriceComponent } from './pages/reports/warehouse-leaving/warehouse-layouts-document-leaving-history-by-price-list/warehouse-layouts-document-leaving-history-by-price-list.component';
import { WarehouseLayoutsDocumentHistoryListComponent } from './pages/reports/warehouse-enter/warehouse-layouts-document-history/warehouse-layouts-document-history-list.component';
import { WarehouseLayoutsDocumentHistoryListByPriceComponent } from './pages/reports/warehouse-enter/warehouse-layouts-document-history-by-price-list/warehouse-layouts-document-history-by-price-list.component';
import { WarehouseRequestExitListComponent } from './pages/receipt-list/warehouse-request-exit-list/warehouse-request-exit-list.component';
import { ComboSina_FinancialOperationNumberComponent } from './pages/component/combo-sina_financialOperationNumber/combo-Sina_FinancialOperationNumber.component';
import { RepairCommodityCardexComponent } from './pages/receipt-operations/repair-commodity-cardex/repair-commodity-cardex.component';
import { ContradictionAccountingComponent } from './pages/reports/contradiction-accounting/contradiction-accounting.component';
import { CommodityReportsRialAccountingComponent } from './pages/reports/available-commodity-reports/commodity-reports-Rial-accounting/commodity-reports-Rial-accounting.component';
import { CommodityReceiptReportsRialAccountingComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports-Rial-accounting/commodity-receipt-reports-Rial-accounting.component';
import {
  WarehouseLayoutCountFormIssuanceDialogComponent
} from "./pages/warehouse-layout/warehouse-layout-count-form-issuance-dialog/warehouse-layout-count-form-issuance-dialog.component";
import {
  WarehouseCountFormListComponent
} from "./pages/WarehouseCountForm/warehouse-count-form-list/warehouse-count-form-list.component";
import {
  WarehouseCountFormDetailComponent
} from "./pages/WarehouseCountForm/warehouse-count-form-detail/warehouse-count-form-detail.component";
import {
  WarehouseCountFormConflictComponent
} from "./pages/WarehouseCountForm/warehouse-count-form-conflict/warehouse-count-form-conflict.component";
import {
  ArchiveDocumentHeadsByDocumentDateComponent
} from "./pages/receipt-operations/archive-document-by-date/archive-document-by-date.component";
import { WarehouseCountReportComponent } from './pages/WarehouseCountForm/Report/warehouse-count-report/warehouse-count-report.component';
import {ExcelImportDialogComponent} from "./pages/WarehouseCountForm/excel-import-dialog/excel-import-dialog.component";
import { WarehouseCountFormComponent } from './pages/WarehouseCountForm/warehouse-count-form/warehouse-count-form.component';
import { AddRequestCommodityMaterialComponent } from './pages/request-operations/add-request-commodity-materail/add-request-commodity-materail.component';
import { LeaveErpJoinReceiptListComponent } from './pages/receipt-list/leave-erp-join-receipt-list/leave-erp-join-receipt-list.component';
import { AddManualTemporaryReceiptSamallComponent } from './pages/receipt-operations/add-temporary-receipt/add-manual-temporary-receipt-small/add-manual-temporary-receipt-small.component';

@NgModule({
  declarations: [
    AddRequestCommodityMaterialComponent,
    LeaveErpJoinReceiptListComponent,
    AddTemporaryReceiptComponent,
    TemporaryReceiptItemsComponent,
    WarehouseComponent,
    WarehouseDialogComponent,
    WarehouseLayoutComponent,
    AddWarehouseLayoutDialogComponent,
    UpdateWarehouseLayoutDialogComponent,
    LayoutListComponent,
    ReceiptListComponent,
    TemporaryReceiptListComponent,
    DirectReceiptListComponent,
    PlacementWarehouseDirectReceiptComponent,
    RecieptWarhouseLayoutComponent,
    TemporaryReceiptPrintComponent,
    WarehouseLayoutsCommodityHistoryListComponent,
    WarehouseLayoutsCommodityQuantityListComponent,
    ChangeWarehouseLayoutsDialogComponent,
    RialsReceiptListComponent,
    RialsReceiptDetailsComponent,
    RialsReceiptItemsComponent,
    ReceiptDetailsComponent,
    RialsReceiptComponent,
    WarehouseStocksAvailableCommodityComponent,
    leavingPartWarehouseComponent,
    AddTemporaryReceiptSingleRowComponent,
    TemporaryReceiptItemsSingleRowComponent,
    ArchiveReceiptListComponent,
    WarehouseLayoutsAddInventoryDialogComponent,
    DocumentAccountingDatailsComponent,
    LeaveReceiptListComponent,
    EditWarehouseLayoutsDialogComponent,
    MechanizedDocumentComponent,
    AddManualTemporaryReceiptComponent,
    ManualItemsComponent,
    AddRequestBuyComponent,
    requesBuyListComponent,
    ComboInvoiceComponent,
    ComboCodeVoucherGroupsComponent,
    AddCommoditySerialDialog,
    LeavingAddMarerialWarehouseComponent,
    MaterialItemsComponent,
    ComboCommodityBomsComponent,
    MaterilaReceiptListComponent,
    UpdateMarerialWarehouseComponent,
    AddRequestCommodityComponent,
    RequesReciveCommodityListComponent,
    LeavingCommodityWarehouseComponent,
    UnitCommodityQuotaDialogComponent,
    UnitCommodityQuotaListComponent,
    ComboSearchMultiperItemsComponent,
    PersonsDebitedCommodityListComponent,
    UpdateReturnToWarehousePersonsDebitedDialogComponent,
    UpdateNewPersonsDebitedDialogComponent,
    AssetsListComponent,
    AssetsCommoditySerialDialog,
    dailyEntryToWarehouseListComponent,
    ComboAccountHeadTreeComponent,
    UpdateQuantityDialogComponent,
    DocumentItemsBomDialog,
    MechanizedReciptListComponent,
    ExportToExcelTejaratSystemComponent,
    AddStartDocumentReceiptComponent,

    WarehouseLayoutsDocumentHistoryListByPriceComponent,//ورودی به انبار
    WarehouseLayoutsDocumentHistoryListComponent,
    WarehouseLayoutsDocumentLeavingHistoryListByPriceComponent,//خروجی از انبار
    WarehouseLayoutsDocumentLeavingHistoryListComponent,

    UpdateAttachmentAssetsDialogComponent,
    MechanizedDocumentConfirmEditComponent,
    AuditListComponent,
    RequestPrintComponent,
    requesBuyMadeListComponent,
    AddAccessToWarehouseComponent,
    GetMonthlyEntryToWarehouseComponent,
    CommodityReportsComponent,
    CommodityReceiptReportsComponent,
    ExportToExcelTadbirSystemComponent,
    UploaderExcelComponent,
    ImportTadbirInventoryExcelValidateComponent,
    CommodityReceiptReportsRialComponent,
    CommodityReportsRialComponent,
    warehouseStockReportsComponent,
    warehouseStockReportsRialComponent,
    WarehouseReceiptsBookComponent,
    WarehouseReceiptsBookRilaComponent,
    AddRequestReturnCommodityComponent,
    ReceiptsInvoiceComponent,
    AddTemporaryReceiptProductComponent,
    TemporaryReceiptItemsProductComponent,
    LeavingProductComponent,
    LeavingItemsProductComponent,
    InvoiceBuyComponent,
    ComboAddSelectBaseValueComponent,
    correctionRequestListComponent,
    ButtonSearchComponent,
    InvoiceBuyCurrencyComponent,
    DocumentHeadExtraCostDialogComponent,
    ComboSina_FinancialOperationNumberComponent,
    ReceiptForAddExteraCostListComponent,
    FreightPaysComponent,
    SplitCommodityQuantityDialogComponent,
    MakeProductPriceComponent,
    FilesByPaymentNumberComponent,
    CommoditySerialViewDialog,
    CommodityCostComponent,
    UpdateBarcodeDialogComponent,
    CommodityReportsWithWarehouseComponent,
    RialsDebitDetailsDialogComponent,
    WarehouseRequestExitListComponent,
    RepairCommodityCardexComponent,
    ContradictionAccountingComponent,
    CommodityReportsRialAccountingComponent,
    CommodityReceiptReportsRialAccountingComponent,
    WarehouseLayoutCountFormIssuanceDialogComponent,
    WarehouseCountFormListComponent,
    WarehouseCountFormDetailComponent,
    WarehouseCountFormConflictComponent,
    CommodityReceiptReportsRialAccountingComponent,
    ArchiveDocumentHeadsByDocumentDateComponent,
    WarehouseCountReportComponent,
    ExcelImportDialogComponent,
    WarehouseCountFormComponent,
    AddManualTemporaryReceiptSamallComponent
  ],
  exports: [
    ComboTreeComponent,
    ComboSearchComponent,
    ComboAccountRefrenceComponent,
    ComboTagComponent,
    ComboCommodityComponent,
    ComboWarhouseTreeComponent,
    CommodityCategoriesTreeComponent,
    ComboSina_FinancialOperationNumberComponent,
    WarehouseLayoutsCommodityHistoryComponent,
    TableFilterComponent,
    tablePaggingComponent,

  ],
  imports: [
    CommonModule,
    InventoryRoutingModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    FormsModule,
    MatCardModule,
    ComponentsModule,
    MatInputModule,
    MatButtonModule,
    AngularMaterialModule,
    NgxMaskModule.forRoot({
      dropSpecialCharacters: true
    })


  ],
})
export class InventoryModule { }
