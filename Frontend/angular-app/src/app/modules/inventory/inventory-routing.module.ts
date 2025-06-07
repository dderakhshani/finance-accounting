import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { WarehouseComponent } from './pages/warehouse/warehouse.component';
import {
  AddTemporaryReceiptComponent
} from "./pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-group/add-temporary-receipt.component";
import { WarehouseLayoutComponent } from './pages/warehouse-layout/warehouse-layout.component';
import { TemporaryReceiptPrintComponent } from './pages/receipt-operations/add-temporary-receipt/temporary-receipt-print/temporary-receipt-print.component';
import { TemporaryReceiptListComponent } from './pages/receipt-list/temporary-receipt-list/temporary-receipt-list.component';
import { ReceiptListComponent } from './pages/receipt-list/receipt-list.component';
import { DirectReceiptListComponent } from './pages/receipt-list/direct-receipt-list/direct-receipt-list.component';
import { WarehouseLayoutsCommodityHistoryListComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-history-list/warehouse-layouts-commodity-history-list.component';
import { WarehouseLayoutsCommodityQuantityListComponent } from './pages/warehouse-layout-list/warehouse-layouts-commodity-quantity-list/warehouse-layouts-commodity-quantity-list.component';
import { RialsReceiptListComponent } from './pages/receipt-list/Rials-receipt-list/Rials-receipt-list.component';
import { RialsReceiptDetailsComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt-details/Rials-receipt-details.component';
import { RialsReceiptComponent } from './pages/receipt-operations/convert-to-Rials-receipt/Rials-receipt.component';
import { WarehouseStocksAvailableCommodityComponent } from './pages/warehouse-stocks-list/warehouse-stocks-available-commodity/warehouse-stocks-available-commodity.component';
import { PlacementWarehouseDirectReceiptComponent } from './pages/receipt-operations/placement-warehouse-direct-receipt/placement-warehouse-direct-receipt.component';
import { AddTemporaryReceiptSingleRowComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-single-row/add-temporary-receipt-single-row.component';
import { ArchiveReceiptListComponent } from './pages/receipt-list/archive-receipt-list/archive-receipt-list.component';
import { LeaveReceiptListComponent } from './pages/receipt-list/leave-receipt-list/leave-receipt-list.component';
import { MechanizedDocumentComponent } from './pages/receipt-operations/convert-to-mechanized-document/mechanized-document.component';
import { DocumentAccountingDatailsComponent } from './pages/receipt-operations/convert-to-mechanized-document/document-accounting-details/document-accounting-details.component';
import { AddManualTemporaryReceiptComponent } from './pages/receipt-operations/add-temporary-receipt/add-manual-temporary-receipt/add-manual-temporary-receipt.component';
import { requesBuyListComponent } from './pages/request-list/reques-buy-list/reques-buy-list.component';
import { ReceiptDetailsComponent } from './pages/component/receipt-details/receipt-details.component';
import { leavingPartWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-part/leaving-part-warehouse.component';
import { MaterilaReceiptListComponent } from './pages/receipt-list/material-receipt-list/material-receipt-list.component';
import { AddRequestBuyComponent } from './pages/request-operations/add-request-buy/add-request-buy.component';
import { AddRequestCommodityComponent } from './pages/request-operations/add-request-commodity/add-request-commodity.component';
import { RequesReciveCommodityListComponent } from './pages/request-list/reques-recive-commodity-list/reques-recive-commodity-list.component';
import { LeavingAddMarerialWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-material/add-materail-warehouse/leaving-add-material-warehouse.component';
import { UpdateMarerialWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-material/update-materail-warehouse/update-materail-warehouse.component';
import { UnitCommodityQuotaListComponent } from './pages/unitCommodityQuota/unitCommodityQuota-list.component';
import { PersonsDebitedCommodityListComponent } from './pages/persons-debited-commodities/persons-debited-commodities-list.component';
import { AssetsListComponent } from './pages/assets/assets-list/assets-list.component';

import { LeavingCommodityWarehouseComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-commodity/leaving-commodity-warehouse.component';


import { AddStartDocumentReceiptComponent } from './pages/receipt-operations/add-start-document-receipt/add-start-document-receipt.component';

import { MechanizedDocumentConfirmEditComponent } from './pages/receipt-operations/mechanized-document_confirm_edit/mechanized-document_confirm_edit.component';
import { RequestPrintComponent } from './pages/request-operations/request-print/request-print.component';
import { requesBuyMadeListComponent } from './pages/request-list/reques-buy-made-list/reques-buy-made-list.component';
import { AddAccessToWarehouseComponent } from './pages/add-access-to-warehouse/add-access-to-warehouse.component';
import { ExportToExcelTejaratSystemComponent } from './pages/reports/export-to excel/export-to excel-tejarat-system/export-to-excel-tejarat-system.component';
import { GetMonthlyEntryToWarehouseComponent } from './pages/reports/monthly-entry-to-warehouse/monthly-entry-to-warehouse.component';
import { dailyEntryToWarehouseListComponent } from './pages/reports/daily-entry-to-warehouse/daily-entry-to-warehouse-list.component';
import { CommodityReportsComponent } from './pages/reports/available-commodity-reports/commodity-reports/commodity-reports.component';
import { ExportToExcelTadbirSystemComponent } from './pages/reports/export-to excel/export-to excel-tadbir-system/export-to excel-tadbir-system.component';
import { ImportTadbirInventoryExcelValidateComponent } from './pages/reports/import-tadbir-inventory-excel-validate/import-tadbir-inventory-excel-validate.component';
import { CommodityReceiptReportsRialComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports-Rial/commodity-receipt-reports-Rial.component';
import { CommodityReportsRialComponent } from './pages/reports/available-commodity-reports/commodity-reports-Rial/commodity-reports-Rial.component';
import { CommodityReceiptReportsComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports/commodity-receipt-reports.component';
import { warehouseStockReportsComponent } from './pages/reports/warehouse-stocks/warehouse-stock/warehouse-stock.component';
import { warehouseStockReportsRialComponent } from './pages/reports/warehouse-stocks/warehouse-stock-Rial/warehouse-stock-Rial.component';
import { WarehouseReceiptsBookComponent } from './pages/reports/warehouse-receipts-books/warehouse-receipts-book/warehouse-receipts-book.component';
import { WarehouseReceiptsBookRilaComponent } from './pages/reports/warehouse-receipts-books/warehouse-receipts-book-Rial/warehouse-receipts-book-Rial.component';
import { AddRequestReturnCommodityComponent } from './pages/request-operations/add-request-return-commodity/add-request-return-commodity.component';
import { ReceiptsInvoiceComponent } from './pages/reports/receipts-invoice/receipts-invoice.component';
import { AddTemporaryReceiptProductComponent } from './pages/receipt-operations/add-temporary-receipt/add-temporary-receipt-product/add-temporary-receipt-product.component';
import { LeavingProductComponent } from './pages/receipt-operations/leaving-add-commodity-in-warehouse/leaving-product/leaving-product.component';

import { MechanizedReciptListComponent } from './pages/reports/mechanized-receipt-list/mechanized-receipt-list.component';
import { InvoiceBuyComponent } from './pages/receipt-operations/invoice/invoice-buy/invoice-buy.component';
import { InvoiceBuyCurrencyComponent } from './pages/receipt-operations/invoice/invoice-buy-currency/invoice-buy-currency.component';
import { ReceiptForAddExteraCostListComponent } from './pages/receipt-list/receipt-for-add-extera-cost-list/receipt-for-add-extera-cost-list.component';
import { FreightPaysComponent } from './pages/reports/freight-pays/freight-pays.component';
import { MakeProductPriceComponent } from './pages/reports/make-product-price-list/make-product-price-list.component';
import { FilesByPaymentNumberComponent } from './pages/component/files-by-payment-number/files-by-payment-number.component';
import { CommodityCostComponent } from './pages/reports/commodity-cost/commodity-cost.component';
import { CommodityReportsWithWarehouseComponent } from './pages/reports/commodity-reports-with-warehouse-list/commodity-reports-with-warehouse-list.component';
import { WarehouseLayoutsDocumentLeavingHistoryListComponent } from './pages/reports/warehouse-leaving/warehouse-layouts-document-leaving-history-list/warehouse-layouts-document-leaving-history-list.component';
import { WarehouseLayoutsDocumentLeavingHistoryListByPriceComponent } from './pages/reports/warehouse-leaving/warehouse-layouts-document-leaving-history-by-price-list/warehouse-layouts-document-leaving-history-by-price-list.component';
import { WarehouseLayoutsDocumentHistoryListComponent } from './pages/reports/warehouse-enter/warehouse-layouts-document-history/warehouse-layouts-document-history-list.component';
import { WarehouseLayoutsDocumentHistoryListByPriceComponent } from './pages/reports/warehouse-enter/warehouse-layouts-document-history-by-price-list/warehouse-layouts-document-history-by-price-list.component';
import { WarehouseRequestExitListComponent } from './pages/receipt-list/warehouse-request-exit-list/warehouse-request-exit-list.component';
import { RepairCommodityCardexComponent } from './pages/receipt-operations/repair-commodity-cardex/repair-commodity-cardex.component';
import { ContradictionAccountingComponent } from './pages/reports/contradiction-accounting/contradiction-accounting.component';
import { CommodityReportsRialAccountingComponent } from './pages/reports/available-commodity-reports/commodity-reports-Rial-accounting/commodity-reports-Rial-accounting.component';
import { CommodityReceiptReportsRialAccountingComponent } from './pages/reports/cardex-warehouse/commodity-receipt-reports-Rial-accounting/commodity-receipt-reports-Rial-accounting.component';
import {  WarehouseCountFormListComponent} from "./pages/WarehouseCountForm/warehouse-count-form-list/warehouse-count-form-list.component";
import {  WarehouseCountFormDetailComponent} from "./pages/WarehouseCountForm/warehouse-count-form-detail/warehouse-count-form-detail.component";
import {  WarehouseCountFormConflictComponent} from "./pages/WarehouseCountForm/warehouse-count-form-conflict/warehouse-count-form-conflict.component";
import { ArchiveDocumentHeadsByDocumentDateComponent } from './pages/receipt-operations/archive-document-by-date/archive-document-by-date.component';
import {
  WarehouseCountReportComponent
} from "./pages/WarehouseCountForm/Report/warehouse-count-report/warehouse-count-report.component";
import {
  WarehouseCountFormComponent
} from "./pages/WarehouseCountForm/warehouse-count-form/warehouse-count-form.component";
import { AddRequestCommodityMaterialComponent } from './pages/request-operations/add-request-commodity-materail/add-request-commodity-materail.component';
import { LeaveErpJoinReceiptListComponent } from './pages/receipt-list/leave-erp-join-receipt-list/leave-erp-join-receipt-list.component';
import { AddManualTemporaryReceiptSamallComponent } from './pages/receipt-operations/add-temporary-receipt/add-manual-temporary-receipt-small/add-manual-temporary-receipt-small.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Inventory'
    },
    children: [
      {
        path: 'receipt-operations/temporaryReceipt',
        component: AddTemporaryReceiptSingleRowComponent,
        data: {
          title: 'ثبت رسید موقت ',
          id: '#temporaryReceipt',
          isTab: true
        },
      },

      {
        path: 'receipt-operations/temporaryReceiptItems',
        component: AddTemporaryReceiptComponent,
        data: {
          title: 'ثبت گروهی رسید موقت',
          id: '#temporaryReceiptItems',
          isTab: true
        },
      },
      {
        path: 'receipt-operations/temporaryReceiptManual',
        component: AddManualTemporaryReceiptComponent,
        data: {
          title: 'ثبت دستی رسید موقت',
          id: '#temporaryReceiptManual',
          isTab: true
        },
      },
      {
        path: 'temporaryReceiptList',
        component: TemporaryReceiptListComponent,
        data: {
          title: 'فهرست رسیدهای موقت',
          id: '#temporaryReceiptList',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/placementWarehouse',
        component: PlacementWarehouseDirectReceiptComponent,
        data: {
          title: 'جایگذاری کالا در انبار',
          id: '#directReceipt',
          isTab: true
        },
      },
      {
        path: 'receiptList',
        component: ReceiptListComponent,
        data: {
          title: 'فهرست رسیدها',
          id: '#ReceiptList',
          isTab: true
        },
      },
      {
        path: 'directReceiptList',
        component: DirectReceiptListComponent,
        data: {
          title: 'فهرست رسیدهای مستقیم',
          id: '#directReceiptList',
          isTab: true
        },
      },
      {
        path: 'warehouses',
        component: WarehouseComponent,
        data: {
          title: 'انبار ها',
          id: '#Warehouse',
          isTab: true
        },
      },
      {
        path: 'warehouseLayout',
        component: WarehouseLayoutComponent,
        data: {
          title: 'تعریف چیدمان انبار',
          id: '#warehouseLayout',
          isTab: true
        },
      },
      {
        path: 'warehouseLayoutsCommodityHistoryList',
        component: WarehouseLayoutsCommodityHistoryListComponent,
        data: {
          title: 'فهرست  سابقه وجود کالا در انبارها',
          id: '#warehouseLayoutsCommodityHistoryList',
          isTab: true
        },
      },
      {
        path: 'warehouseLayoutsCommodityQuantityList',
        component: WarehouseLayoutsCommodityQuantityListComponent,
        data: {
          title: 'فهرست  موجودی کالا در محل های انبارها',
          id: '#warehouseLayoutsCommodityQuantityList',
          isTab: true
        },
      },
      {
        path: 'receipt-operations/rialsReceipt',
        component: RialsReceiptComponent,
        data: {
          title: 'ریالی کردن رسید مستقیم انبار',
          id: '#rialsReceipt',
          isTab: true
        },
      },
      {
        path: 'rialsReceiptDetails',
        component: RialsReceiptDetailsComponent,
        data: {
          title: 'جزئیات رسید مستقیم در ریالی سازی',
          id: '#rialsReceiptDetails',
          isTab: true
        },
      },
      {
        path: 'rialsReceiptList',
        component: RialsReceiptListComponent,
        data: {
          title: 'فهرست رسید های ریالی',
          id: '#rialsReceiptList',
          isTab: true
        },
      },
      {
        path: 'receiptDetails',
        component: ReceiptDetailsComponent,
        data: {
          title: 'جزئیات سند',
          id: '#receiptDetails',
          isTab: true
        },
      },
      {
        path: 'mechanizedReciptList',
        component: MechanizedReciptListComponent,
        data: {
          title: 'فهرست سندهای مکانیزه',
          id: '#mechanizedReciptList',
          isTab: true
        },
      },

      {
        path: 'receipt-operations/temporaryReceiptPrint',
        component: TemporaryReceiptPrintComponent,
        data: {
          title: 'نسخه چاپی برگه تایید کالا',
          id: '#temporaryReceipt',
          isTab: true
        },
      },
      {
        path: 'warehouseStocksAvailable',
        component: WarehouseStocksAvailableCommodityComponent,
        data: {
          title: 'فهرست موجودی کالا',
          id: '#warehouseStocksAvailable',
          isTab: true
        },
      },
      {
        path: 'receipt-operations/leavingPartWarehouse',
        component: leavingPartWarehouseComponent,
        data: {
          title: 'خروج کالا در انبار قطعات',
          id: '#leavingPartWarehouse',
          isTab: true
        },
      },

      {
        path: 'receipt-operations/leavingMaterialWarehouse',
        component: LeavingAddMarerialWarehouseComponent,
        data: {
          title: 'جابه جایی و ورود کالا در انبار مواد اولیه',
          id: '#leavingMaterialWarehouse',
          isTab: true
        },
      },
      {
        path: 'archiveReceiptList',
        component: ArchiveReceiptListComponent,
        data: {
          title: 'رسیدهای آرشیوی',
          id: '#archiveReceiptList',
          isTab: true
        },
      },
      {
        path: 'documentAccountingDatails',
        component: DocumentAccountingDatailsComponent,
        data: {
          title: 'صدور سند حسابداری',
          id: '#documentAccountingDatails',
          isTab: true
        },
      },
      {
        path: 'leaveReceiptList',
        component: LeaveReceiptListComponent,
        data: {
          title: 'فهرست خروج کالا از انبار',
          id: '#leaveReceiptList',
          isTab: true
        },
      },
      {
        path: 'receipt-operations/mechanizedDocument',
        component: MechanizedDocumentComponent,
        data: {
          title: 'صدور سند مکانیزه حسابداری',
          id: '#mechanizedDocument',
          isTab: true
        },

      },
      {
        path: 'request-operations/requestBuy',
        component: AddRequestBuyComponent,
        data: {
          title: 'ثبت درخواست خرید',
          id: '#requestBuy',
          isTab: true
        },

      },
      {
        path: 'request-list/requesBuyList',
        component: requesBuyListComponent,
        data: {
          title: 'فهرست درخواست های خرید',
          id: '#requesBuyList',
          isTab: true
        },

      },

      {
        path: 'request-operations/requestCommodity',
        component: AddRequestCommodityComponent,
        data: {
          title: 'ثبت درخواست کالا',
          id: '#requestCommodity',
          isTab: true
        },

      },
      {
        path: 'request-list/requestCommodityList',
        component: RequesReciveCommodityListComponent,
        data: {
          title: 'فهرست درخواست های کالا',
          id: '#requestCommodityList',
          isTab: true
        },

      },
      {
        path: 'materilaReceiptList',
        component: MaterilaReceiptListComponent,
        data: {
          title: 'فهرست ورود و خروج کالا انبار مواد اولیه',
          id: '#requesBuyList',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/updateMaterilaReceip',
        component: UpdateMarerialWarehouseComponent,
        data: {
          title: 'ویرایش ورود و خروج کالا مواد اولیه',
          id: '#updateMaterilaReceipt',
          isTab: true
        },

      },
      {
        path: 'leavingConsumableWarehouse',
        component: LeavingCommodityWarehouseComponent,
        data: {
          title: 'خروج کالا از انبار',
          id: '#leavingConsumableWarehouse',
          isTab: true
        },

      },

      {
        path: 'unitCommodityQuotaList',
        component: UnitCommodityQuotaListComponent,
        data: {
          title: 'سهمیه مصرف کالا در واحد های سازمانی',
          id: '#unitCommodityQuotaList',
          isTab: true
        },

      },
      {
        path: 'personsDebitedCommodityList',
        component: PersonsDebitedCommodityListComponent,
        data: {
          title: 'کالاهای واگذاری',
          id: '#personsDebitedCommodityList',
          isTab: true
        },

      },
      {
        path: 'assetsListComponent',
        component: AssetsListComponent,
        data: {
          title: 'فهرست کالاهای اموال',
          id: '#assetsListComponent',
          isTab: true
        },

      },
      {
        path: 'warehouseLayoutsDocumentHistoryList',
        component: dailyEntryToWarehouseListComponent,
        data: {
          title: 'فهرست ورودی و خروجی کالا های روزانه',
          id: '#warehouseLayoutsDocumentHistoryList',
          isTab: true
        },

      },

      {
        path: 'exportToExcelTejaratSystem',
        component: ExportToExcelTejaratSystemComponent,
        data: {
          title: 'خروجی سیستم های تجارت',
          id: '#exportToExcelTejaratSystem',
          isTab: true
        },

      },
      {
        path: 'addStartDocumentReceipt',
        component: AddStartDocumentReceiptComponent,
        data: {
          title: 'ایجاد سند افتتاحیه',
          id: '#addStartDocumentReceipt',
          isTab: true
        },

      },


      {
        path: 'mechanizedDocumentConfirmEdit',
        component: MechanizedDocumentConfirmEditComponent,
        data: {
          title: 'تایید تغییرات سندهای حسابداری',
          id: '#mechanizedDocumentConfirmEdit',
          isTab: true
        },

      },
      {
        path: 'request-operations/RequestPrint',
        component: RequestPrintComponent,
        data: {
          title: 'چاپ درخواست',
          id: '#RequestPrint',
          isTab: true
        },

      },
      {
        path: 'requesBuyMadeList',
        component: requesBuyMadeListComponent,
        data: {
          title: 'رسیدهای ورودی',
          id: '#requesBuyMadeList',
          isTab: true
        },

      },
      {
        path: 'addAccessToWarehouse',
        component: AddAccessToWarehouseComponent,
        data: {
          title: 'دسترسی کاربران به انبارها',
          id: '#addAccessToWarehouse',
          isTab: true
        },

      },
      {
        path: 'monthlyEntryToWarehouse',
        component: GetMonthlyEntryToWarehouseComponent,
        data: {
          title: 'ورودی های به انبار ماهیانه',
          id: '#monthlyEntryToWarehouse',
          isTab: true
        },

      },
      {
        path: 'commodityReports',
        component: CommodityReportsComponent,
        data: {
          title: 'گزارش موجودی مقداری کالا',
          id: '#commodityReports',
          isTab: true
        },

      },

      {
        path: 'commodityReportsRial',
        component: CommodityReportsRialComponent,
        data: {
          title: 'گزارش موجودی ریالی کالا',
          id: '#commodityReportsRial',
          isTab: true
        },

      },
      {
        path: 'warehouseStockReports',
        component: warehouseStockReportsComponent,
        data: {
          title: 'گزارش موجودی استاک انبار',
          id: '#warehouseStockReports',
          isTab: true
        },

      },
      {
        path: 'warehouseStockReportsRial',
        component: warehouseStockReportsRialComponent,
        data: {
          title: 'گزارش موجودی ریالی استاک انبار',
          id: '#warehouseStockReportsRial',
          isTab: true
        },

      },

      {
        path: 'commodityReceiptReports',
        component: CommodityReceiptReportsComponent,
        data: {
          title: 'گزارش  گردش مقداری کالا',
          id: '#commodityReceiptReports',
          isTab: true
        },

      },
      {
        path: 'commodityReceiptReportsRial',
        component: CommodityReceiptReportsRialComponent,
        data: {
          title: 'گزارش  گردش ریالی کالا',
          id: '#commodityReceiptReportsRial',
          isTab: true
        },

      },
      {
        path: 'warehouseReceiptsBook',
        component: WarehouseReceiptsBookComponent,
        data: {
          title: 'دفتر مقداری انبار',
          id: '#warehouseReceiptsBook',
          isTab: true
        },

      },
      {
        path: 'warehouseReceiptsBookRila',
        component: WarehouseReceiptsBookRilaComponent,
        data: {
          title: 'دفتر ریالی انبار',
          id: '#warehouseReceiptsBookRila',
          isTab: true
        },

      },


      {
        path: 'exportToExcelTadbirSystem',
        component: ExportToExcelTadbirSystemComponent,
        data: {
          title: 'خروجی پلاگین رسیدهای تدبیر',
          id: '#exportToExcelTadbirSystem',
          isTab: true
        },

      },
      {
        path: 'importTadbirInventoryExcelValidate',
        component: ImportTadbirInventoryExcelValidateComponent,
        data: {
          title: 'گزارش مقایسه موجودی تدبیر و دانا',
          id: '#importTadbirInventoryExcelValidate',
          isTab: true
        },

      },
      {
        path: 'request-operations/addRequestReturnCommodity',
        component: AddRequestReturnCommodityComponent,
        data: {
          title: 'درخواست برگشت از خرید',
          id: '#addRequestReturnCommodity',
          isTab: true
        },

      },
      {
        path: 'receiptsInvoice',
        component: ReceiptsInvoiceComponent,
        data: {
          title: 'صورتحساب های خرید کالا',
          id: '#receiptsInvoice',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/addTemporaryReceiptProduct',
        component: AddTemporaryReceiptProductComponent,
        data: {
          title: 'ورودی انبار محصول',
          id: '#addTemporaryReceiptProduct',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/leavingProductComponent',
        component: LeavingProductComponent,
        data: {
          title: 'خروجی انبار محصول',
          id: '#leavingProductComponent',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/invoiceBuyComponent',
        component: InvoiceBuyComponent,
        data: {
          title: 'صورتحساب های خرید',
          id: '#invoiceBuyComponent',
          isTab: true
        },

      },
      {
        path: 'receipt-operations/InvoiceBuyCurrencyComponent',
        component: InvoiceBuyCurrencyComponent,
        data: {
          title: 'صورتحساب های ورودی به انبار',
          id: '#InvoiceBuyCurrencyComponent',
          isTab: true
        },

      },
      {
        path: 'RialsReceiptForAddExteraCostList',
        component: ReceiptForAddExteraCostListComponent,
        data: {
          title: 'ورودی های ریالی جهت کرایه حمل ',
          id: '#RialsReceiptForAddExteraCostList',
          isTab: true
        },

      },
      {
        path: 'FreightPaysList',
        component: FreightPaysComponent,
        data: {
          title: 'گزارش پرداختی رانندگان ',
          id: '#FreightPaysList',
          isTab: true
        },

      },
      {
        path: 'MakeProductPrice',
        component: MakeProductPriceComponent,
        data: {
          title: 'گزارش قیمت تمام شده محصول ',
          id: '#MakeProductPrice',
          isTab: true
        },

      },
      {
        path: 'FilesByPaymentNumber',
        component: FilesByPaymentNumberComponent,
        data: {
          title: 'فایل های خزانه داری ',
          id: '#FilesByPaymentNumber',
          isTab: true
        },

      },
      {
        path: 'CommodityCost',
        component: CommodityCostComponent,
        data: {
          title: 'هزینه تمام شده کالاهای انبار',
          id: '#CommodityCost',
          isTab: true
        },

      },
      {
        path: 'CommodityReportsWithWarehouse',
        component: CommodityReportsWithWarehouseComponent,
        data: {
          title: 'گزارش ارزش مقداری  و ریالی انبار',
          id: '#CommodityReportsWithWarehouse',
          isTab: true
        },

      },
      {
        path: 'exportWarehouseReports',
        component: WarehouseLayoutsDocumentLeavingHistoryListByPriceComponent,
        data: {
          title: 'گزارش خروجی ریالی انبارها',
          id: '#exportWarehouseReports',
          isTab: true
        },

      },
      {
        path: 'WarehouseLayoutsDocumentLeavingHistoryList',
        component: WarehouseLayoutsDocumentLeavingHistoryListComponent,
        data: {
          title: 'گزارش خروجی مقداری انبارها',
          id: '#WarehouseLayoutsDocumentLeavingHistoryList',
          isTab: true
        },

      },
      {
        path: 'WarehouseLayoutsDocumentHistoryList',
        component: WarehouseLayoutsDocumentHistoryListComponent,
        data: {
          title: 'گزارش مقداری ورودی انبارها',
          id: '#WarehouseLayoutsDocumentHistoryList',
          isTab: true
        },

      },
      {
        path: 'WarehouseLayoutsDocumentHistoryListByPrice',
        component: WarehouseLayoutsDocumentHistoryListByPriceComponent,
        data: {
          title: 'گزارش ورودی ریالی انبارها',
          id: '#WarehouseLayoutsDocumentHistoryListByPrice',
          isTab: true
        },

      },
      {
        path: 'WarehouseRequestExitList',
        component: WarehouseRequestExitListComponent,
        data: {
          title: 'فهرست درخواست های خروجی ERP',
          id: '#WarehouseRequestExitList',
          isTab: true
        },

      },
      {
        path: 'RepairCommodityCardex',
        component: RepairCommodityCardexComponent,
        data: {
          title: 'بازسازی کاردکس',
          id: '#RepairCommodityCardex',
          isTab: true
        },

      },
      {
        path: 'ContradictionAccounting',
        component: ContradictionAccountingComponent,
        data: {
          title: 'مغایرت انبار و حسابداری',
          id: '#ContradictionAccounting',
          isTab: true
        },

      },
      {
        path: 'CommodityReportsRialAccounting',
        component: CommodityReportsRialAccountingComponent,
        data: {
          title: 'موجودی انبار',
          id: '#CommodityReportsRialAccounting',
          isTab: true
        },

      },
      {
        path: 'commodityReceiptReportsRialAccounting',
        component: CommodityReceiptReportsRialAccountingComponent,
        data: {
          title: 'کاردکس گردش کالا',
          id: '#commodityReceiptReportsRialAccounting',
          isTab: true
        },
      },
      {
        path: 'warehouseCountFormIssuance',
        component: WarehouseCountFormComponent,
        data: {
          title: 'صدور فرم شمارش انبارها',
          id: '#warehouseCountFormIssuance',
          isTab: true
        },
      },
      {
        path: 'WarehouseCountFormList',
        component: WarehouseCountFormListComponent,
        data: {
          title: 'فهرست برگه های شمارش',
          id: '#WarehouseCountFormList',
          isTab: true
        },
      },
      {
        path: 'ArchiveDocumentHeadsByDocumentDate',
        component: ArchiveDocumentHeadsByDocumentDateComponent,
        data: {
          title: 'بایگانی رسید/ حواله انبار',
          id: '#ArchiveDocumentHeadsByDocumentDate',
          isTab: true
        },
      },
      {
        path: 'warehouseCountFormDetail',
        component: WarehouseCountFormDetailComponent,
        data: {
          title: 'جزئیات برگه شمارش',
          id: '#warehouseCountFormDetail@#$',
          isTab: true
        },
      },
      {
        path: 'warehouseCountFormConflict',
        component: WarehouseCountFormConflictComponent,
        data: {
          title: 'مغایرت های برگه شمارش',
          id: '#warehouseCountFormConflict@#$',
          isTab: true
        }
        },
      {
        path: 'warehouseCountReport',
        component: WarehouseCountReportComponent,
        data: {
          title: 'گزارش شمارش انبار',
          id: '#warehouseCountReport@#$',
          isTab: true
        },
      },
      {
        path: 'AddRequestCommodityMaterial',
        component: AddRequestCommodityMaterialComponent,
        data: {
          title: 'خروج مواد اولیه فریت سازی',
          id: 'AddRequestCommodityMaterial@#$',
          isTab: true
        },
      }
      ,
      {
        path: 'LeaveErpJoinReceiptList',
        component: LeaveErpJoinReceiptListComponent,
        data: {
          title: 'فهرست خروجی Erp',
          id: 'AddRequestCommodityMaterial@#$',
          isTab: true
        },
      },
      {
        path: 'receipt-operations/ReceiptSamall',
        component: AddManualTemporaryReceiptSamallComponent,
        data: {
          title: 'برگشت کالا به انبار',
          id: 'AddManualTemporaryReceiptComponentSamall',
          isTab: true
        },
      }
      
      ,
    ],
  },
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InventoryRoutingModule { }



