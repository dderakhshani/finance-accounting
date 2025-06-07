using System;

namespace Eefa.Inventory.Domain.Common
{
    public static class ConstantValues
    {
        public static class AccountingApi
        {
            public static string Url => $"http://localhost:50002/api/accounting/VouchersHead/AddAutoVoucher2";
        }
        public static class AccountingUpdateApi
        {
            public static string Url => $"http://localhost:50002/api/accounting/VouchersHead/UpdateAutoVoucher";
        }
        public static class AraniService
        {
            public static string UrlPurchase => $"http://192.168.2.147/services/api/GetBazarganiDarkhastByShomareDarkhast.ashx?ShomareDarkhast";
            public static string UrlRequestCommodity => $"http://192.168.2.147/Services/Api/GetDarkhastKalaAndItems.ashx?id";
            public static string UrlReturnCommodity => $" http://192.168.2.147/Services/Api/GetKhoroojKalaAndItems.ashx?id";

            //---------------پیشوند اضافه شده در کد ملی پرسنل اضافه شده توسط سیستم آرانی
            public static string Prefix => "000";

        }
        public static class SinaService
        {
            public static string UrlGetProduct => $"http://sina.eefaceram.com/prime/Sales/Api/ApiGetAllSaleProduct?";
            public static string GetInputProduct => $"http://sina.eefaceram.com/prime/Logestics/Home/GetReportReceipt?";

            //http://sina.eefaceram.com/prime/Sales/Api/ApiGetExitSaleProduct?DocumentDate=1403/02/01
            public static string GetOutProduct => $"http://sina.eefaceram.com/prime/Sales/Api/ApiGetExitSaleProduct?";
            public static string GetFilesByPaymentNumber => $"http://sina.eefaceram.com/prime/accounting/api/GetFilesByPaymentNumberAsync?";
        }
        public static class CodeVoucherGroupValues
        {
            public static string SchemaName => "inventory";
            public static int ViewIdRemoveAddWarehouse => 122;

            //ثبت سند مکانیزه رسید انبار قطعات
            public static string DirectReceiptAutoVoucher => "InventoryReceiptAutoVoucher";

            //========================رسید موقت===================================
            // انبار قطعات
            public static string TemporaryReceipt => "UtilitiesTemporaryReceipt";//5023
            //انبار مواد اولیه
            public static string MaterailInventoryTemporaryReceipt => "MaterailTemporaryReceipt";//5123

            //انبار اموال
            public static string EstateTemporaryReceipt => "EstateTemporaryReceipt";//5223

            //انبار مصرفی
            public static string ConsumptionInventoryTemporaryReceipt => "ConsumptionInventoryTemporaryReceipt";//5323
            //انبار امانی
            public static string loanTemporaryReceipt => "loanTemporaryReceipt";//5423
            //مرجوعی
            public static string ReturnTemporaryReceipt => "ReturnTemporaryReceipt";//5523

            //اصلاحیه
            public static string InventoryModification => "InventoryModification";//5823

            public static string ProductTemporaryReceipt => "ProductTemporaryReceipt";//5623

            //========================رسید مستقیم===================================
            // انبار قطعات
            public static string UtilitiesDirectReceipt => "UtilitiesDirectReceipt";//5033
            //انبار مواد اولیه
            public static string MaterailDirectReceipt => "MaterailDirectReceipt";//5133

            //انبار اموال
            public static string EstateDirectReceipt => "EstateDirectReceipt";//5133

            //انبار مصرفی
            public static string ConsumptionDirectReceipt => "ConsumptionDirectReceipt";//5333

            //انبار مواد نیم ساخته
            public static string SemiFinishedMaterialsDirectReceip => "Semi-finishedMaterialsDirectReceipt";//5334

            //انبار امانی
            public static string loanDirectReceipt => "loanDirectReceipt";//5433
            //مرجوعی
            public static string ReturnDirectReceipt => "ReturnDirectReceipt";//5533

            //اصلاحیه
            public static string InventoryModificationReceipt => "InventoryModificationReceipt";//5823

            

            //ورودی محصول تولیدی
            public static string ProductDirectReceipt => "ProductDirectReceipt";//5633


            //========================رسید ریالی===================================
            // انبار قطعات
            public static string AmountUtilitiesReceipt => "AmountUtilitiesReceipt";//5043
            // انبار مواد اولیه
            public static string AmountMaterailReceipt => "AmountMaterailReceipt";//5143

            //انبار اموال
            public static string AmountEstateReceipt => "AmountEstateReceipt";//5143

            //انبار مصرفی
            public static string AmountConsumptionReceipt => "AmountConsumptionReceipt";//5343

            //انبار امانی
            public static string AmountloanReceipt => "AmountloanReceipt";//5443

            //مرجوعی
            public static string AmountDirectReceipt => "AmountReturn";//5543

            //ورودی محصول تولیدی
            public static string AmountProduct => "AmountProduct";//5643
            //========================سند افتتاحیه===================================
            // انبار قطعات
            public static string UtilitiesStartReceipt => "UtilitiesStartReceipt";//5045
            // انبار مواد اولیه
            public static string MaterailInventoryStartReceipt => "MaterailInventoryStartReceipt";//5145

            //انبار اموال
            public static string EstateStartReceipt => "EstateStartReceipt";//5145

            //انبار مصرفی
            public static string ConsumptionInventoryStartReceipt => "ConsumptionInventoryStartReceipt";//5345

            //انبار امانی
            public static string loanStartReceipt => "loanStartReceipt";//5445

            //مرجوعی
            public static string ReturnStartReceipt => "ReturnStartReceipt";//5545

            //ورودی محصول تولیدی
            public static string ProductStartReceipt => "ProductStartReceipt";//5645

            //========================سند کانیزه===================================
            // انبار قطعات
            public static string AccountingUtilitiesReceipt => "AccountingUtilitiesReceipt";//5053
            //انبار مواد اولیه
            public static string AccountingMaterailReceipt => "AccountingMaterailReceipt";//5153

            //انبار اموال
            public static string AccountingEstateReceipt => "AccountingEstateReceipt";//5253

            //انبار مصرفی
            public static string AccountingConsumptionReceipt => "AccountingConsumptionReceipt";//5353

            //مرجوعی
            public static string AccountingDirectReceipt => "AccountingDirectReceipt";//5553

            //ورودی محصول تولیدی
            public static string AccountingProductReceipt => "AccountingProductReceipt";//5653


            //========================آرشیو===================================
            // انبار قطعات
            public static string ArchiveUtilitiesReceipt => "ArchiveUtilitiesReceipt";//5013
            //انبار مواد اولیه
            public static string ArchiveMaterailReceipt => "ArchiveMaterailReceipt";//5113

            //انبار اموال
            public static string ArchiveEstateReceipt => "ArchiveEstateReceipt";//5213

            //انبار مصرفی
            public static string ArchiveConsumptionReceipt => "ArchiveConsumptionReceipt";//5313

            //انبار امانی
            public static string ArchiveloanReceipt => "ArchiveloanReceipt";//5413

            //مرجوعی
            public static string ArchiveDirectReceipt => "ArchiveDirectReceipt";//5513

            //ورودی محصول تولیدی
            public static string ArchiveProduct => "ArchiveProduct";//5613


            //=================================خروج از انبار========================
            //-سند خروج قطعات کالا
            public static string RemoveCommodityWarhouse => "RemoveUtilitiesWarhouse";//5063
            //خروج کالا از انبار مواد اولیه
            public static string RemoveMaterialWarhouse => "RemoveMaterialWarhouse";//5163
            //خروج کالا از انبار مواد مصرفی
            public static string RemoveConsumptionWarhouse => "RemoveConsumptionWarhouse";//5263
            // خروج کالا از انبار اموال
            public static string RemoveAssetsWarhouse => "RemoveAssetsWarhouse";//5363

            // جابجایی کالا از انبار مواد اولیه
            public static string ChangeMaterialWarhouse => "ChangeMaterialWarhouse";//5193
            //رسید محصول انبار مواد اولیه
            //جابه جایی انبار با فرمول ساخت
            public static string ProductReceiptWarehouse => "ProductReceiptWarehouse";//5194

            //انبار امانی
            public static string RemoveloanReceipt => "RemoveloanReceipt";//5463
            //مرجوعی
            public static string RemoveReturnReceipt => "RemoveReturnReceipt";//5563

            //ورودی محصول تولیدی
            public static string RemoveProductReceipt => "RemoveProductReceipt";//5563


            //========================رسید ریالی خروج===================================
            // انبار قطعات
            public static string AmountUtilitiesLeave => "AmountUtilitiesLeave";//5044
                                                                                // انبار مواد اولیه
            public static string AmountMaterailLeave => "AmountMaterailLeave";//5144

            //انبار اموال
            public static string AmountEstateLeave => "AmountEstateLeave";//5144

            //انبار مصرفی
            public static string AmountConsumptionLeave => "AmountConsumptionLeave";//5344

            //انبار امانی
            public static string AmountloanReceiptLeave => "AmountloanReceiptLeave";//5444

            //مرجوعی
            public static string AmountReturnReceipt => "AmountReturnReceipt";//5544


            //ورودی محصول تولیدی
            public static string AmountProductReceiptLeave => "AmountProductReceiptLeave";//5644
            //========================سند کانیزه خروج===================================
                                                                                          // انبار قطعات
            public static string AccountingUtilitieLeave => "AccountingUtilitieLeave";//5054
                                                                                      //انبار مواد اولیه
            public static string AccountingMaterailLeave => "AccountingMaterailLeave";//5154

            //انبار اموال
            public static string AccountingEstateLeave => "AccountingEstateLeave";//5254

            //انبار مصرفی
            public static string AccountingConsumptionLeave => "AccountingConsumptionLeave";//5354

            //انبار امانی
            public static string AccountingloanReceiptLeave => "AccountingloanReceiptLeave";//5454
            //مرجوعی
            public static string AccountingReturnReceipt => "AccountingReturnReceipt";//5554

            //ورودی محصول تولیدی
            public static string AccountingProductReceiptLeave => "AccountingProductReceiptLeave";//5654

            //========================سند کانیزه افتتاحیه===================================
            // انبار قطعات
            public static string AccountingUtilitieStart => "AccountingUtilitieStart";//5055
                                                                                      //انبار مواد اولیه
            public static string AccountingMaterailStart => "AccountingMaterailStart";//5155

            //انبار اموال
            public static string AccountingEstateStart => "AccountingEstateStart";//5255

            //انبار مصرفی
            public static string AccountingConsumptionStart => "AccountingConsumptionStart";//5355

            //انبار امانی
            public static string AccountingloanReceiptStart => "AccountingloanReceiptStart";//5455
            //مرجوعی
            public static string AccountingReturnReceiptStart => "AccountingReturnReceiptStart";//5555

            //ورودی محصول تولیدی
            public static string AccountingProductReceiptStart => "AccountingProductReceiptStart";//5655

            //=======================درخواست دریافت کالا=============================

            //-درخواست دریافت کالا قطعات
            public static string RequestReceiveUtilities => "RequestReceiveUtilities";//5073
            //انبار مواد اولیه
            public static string RequestReceiveMaterail => "RequestReceiveMaterail";//5173
            //-درخواست دریافت کالا مصرفی
            public static string RequestReceiveConsumption => "RequestReceiveConsumption";//5273
            //-درخواست دریافت کالا اموال
            public static string RequesReceiveAssets => "RequesReceiveAssets";//5273
            //-درخواست دریافت کالا امانی
            public static string RequesReceiveBorrow => "RequesReceiveBorrow";//5473
            //مرجوعی
            public static string RequesReceiveReturnCommodity => "RequesReceiveReturnCommodity";//5573

            //=======================بایگانی درخواست دریافت کالا=============================

            //-درخواست دریافت کالا قطعات
            public static string ArchiveRequestUtilitiesReceipt => "ArchiveRequestUtilitiesReceipt";//5012
            //-درخواست دریافت کالا مصرفی
            public static string ArchiveRequestMaterailReceipt => "ArchiveRequestMaterailReceipt";//5252
            //-درخواست دریافت کالا اموال
            public static string ArchiveRequestEstateReceipt => "ArchiveRequestEstateReceipt";//5212
            //-درخواست دریافت کالا مصرفی
            public static string ArchiveRequestConsumptionReceipt => "ArchiveRequestConsumptionReceipt";//5312
            //-درخواست دریافت کالا امانی
            public static string ArchiveRequesReceiveBorrow => "ArchiveRequesReceiveBorrow";//5412

            //مرجوعی
            public static string ArchiveRequesReceiveReturnCommodity => "ArchiveRequesReceiveReturnCommodity";//5512


            //=======================درخواست خرید کالا=============================

            //- قطعات
            public static string RequestBuyUtilities => "RequestBuyUtilities";//5083
            //مواد اولیه
            public static string RequestBuyeMaterail => "RequestBuyeMaterail";//5183

            //-اموال
            public static string RequestBuyEstate => "RequestBuyEstate";//5283
            //-مصرفی
            public static string RequestBuyConsumption => "RequestBuyConsumption";//5383

            //=======================درخواست خرید کالا بایگانی=============================

            //- قطعات
            public static string ArchiveRequestBuyUtilitiesReceipt => "ArchiveRequestBuyUtilitiesReceipt";//5011
            //مواد اولیه
            public static string ArchiveRequestBuyMaterailReceipt => "ArchiveRequestBuyMaterailReceipt";//5111

            //-اموال
            public static string ArchiveRequestBuyEstateReceipt => "ArchiveRequestBuyEstateReceipt";//5211
            //-مصرفی
            public static string ArchiveRequestBuyConsumptionReceipt => "ArchiveRequestBuyConsumptionReceipt";//5311


            //=======================اصلاح موجودی=============================

          
            public static string Transfer => "Transfer";//57103


        }
        public static class AccountReferenceGroup
        {
            //-----------سفارشات خارجى
            public static string ProviderCode => "51";

            //---------تامین کننده خارجی
            public static string ExternalProvider => "23";

           
            public static string InternalProvider => "21";
            public static string InternalProvider1 => "22";

            public static int AccountHeadExternalProvider => 3215;


            //---------کارکنان
            public static string Personal => "19";
            public static int accountReferenceGroupId_personnel => 38;
            //-----------کد گروه انبار در حسابداری
            public static int WarehouseAccountReference => 55;

            //این کد غلط است فقط چون سیستم آقای ملک پور با یک آیدی پر شود از این کد استفاده شده است و کد درست برایش نداریم
            public static int UnitId => 119;
        }
        public static class ConstBaseValue
        {
            //برچسبهای سیستم انبار
            public static int InventoryTagSystem => 331;
            public static string viewAccounting => "گزارش به آقای محققی";
            //فرمول ساخت شماره سریال قرارداد
            public static string UtilityDocument => "UtilityDocumentNumbering";
            //--------------------from basevalue table on value column  انبار قطعات ------------------------
            public static int BaseValueWarehousePartTypeId => 336;
            //مورد تایید کارشناس
            public static int ApprovedByExpert => 28554;
            //نیاز به تست روی کار
            public static int NeedToTest => 28558;
            //عدم حضور کارشناس مربوطه
            public static int AbsenceRelevantExpert => 28559;
            //بررسی نشده
            public static int NotChecked => 28562;
            //عدم تایید کارشناس
            public static int NotConfirm => 28700;

            //---------------------------------------------------------------

            public static int MenId => 1;
            public static int WomenId => 4;
            //----------------------------------------------------------------
            public static int governmentalBaseId => 28277;
            public static int legalBaseId => 28277;

            //-----------------------------------------------------------------
            public static int currencyBaseValueId => 308;
            public static int CommodityGroupsValueId => 360;
            
            public static string currencyIRR => "IRR";
            //------------------------------------------------------------------
            public static int DocumentTagBaseValueId => 331;

            //--------------------------روش نرخ گذاری---------------------------
            public static int BasedOnWeightedAverage => 28529;
            //---------------------مالیات ارزش افزوده---------------------------
            public static string vatDutiesTax => "Vat";

            //---------------روش های استهلاک--------------------------------------
            public static int DepreciationTypeBaseValueId => 338;
            //-------------------------------------------------------------------
            public static DateTime DocumnetDateUtc => Convert.ToDateTime(DateTime.Now.ToShortDateString()).ToUniversalTime();

        }
        public static class WarehouseEntryModeDescription
        {
            public static string NONE => "تفاوتی ندارد";
            public static string LIFO => "آخرین ورودی ، اولین خروجی";
            public static string FIFO => "اولین ورودی ، اولین خروجی";

        }
        public static class CommodityGroups
        {
            public static string CodeAssetGroup => "w";
        }
        
       
        public static class WarehouseLayoutQuarantine
        {
            public static int Id => 36;
        }
        public static class WarehouseInvoiceNoEnam
        {
            public static string LeavePar => "LP";
            public static string LeaveMaterail => "LM";
            public static string LeaveCommodity => "LC";
            public static string LeaveProduct => "LR";

        }

        public static class AccessToWarehouseEnam
        {
            public static string Warehouses => "Warehouses";
            public static string CodeVoucherGroups => "CodeVoucherGroups";
        }

    }
    public enum WarehouseEntryMode
    {
        NONE = 0,
        LIFO = 1,
        FIFO = 2
    }

    public enum WarehouseHistoryMode
    {
        Enter = 1,
        Exit = -1,
    }
    public enum AraniWarehouseIdEnum
    {
        AnbarGhatat = 27,
        AnbarMasrafi = 27,
        AnbarDaghi=32,
        AnbarZakhire=16

    }
    public enum DocumentStateEnam
    {
        Temp = 23,//رسید موقت
        Direct = 33,//رسید مستقیم
        invoiceAmount = 43,//ریالی کردن ورودی
        invoiceAmountLeave = 44,//ریالی کردن خروجی
        invoiceAmountStart = 45,//ریالی کردن سند افتتاحیه
        registrationAccounting = 53,//ثبت سند مکانیزه حسابداری
        registrationAccountingLeave = 54,//ثبت سند مکانیزه حسابداری خروجی
        registrationAccountingStart = 55,//ثبت سند مکانیزه افتتاحیه
        Leave = 63,//خروج کالا
        Transfer = 64,//خروج جهت انتقال بین انبار ها 
        requestReceive = 73,//درخواست دریافت کالا
        requestBuy = 83,//درخواست خرید
        archiveReceipt = 13,//آرشیو
        archiveRequest = 12,//آرشیو درخواست
        archiveBuy = 11,//آرشیو خرید
        InventoryModification = 103,//اصلاح موجودی
        changeWarehouse = 93,
        product = 94
    }
    
}
