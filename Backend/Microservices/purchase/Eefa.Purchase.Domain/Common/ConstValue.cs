namespace Eefa.Purchase.Domain.Common
{
    public static class ConstantValues
    {
        public static class CodeVoucherGroup
        {

            //ثبت قرارداد
            public static string ContractVoucherGroup => "RegistertheContract";
            //ثبت پیش فاکتور
            public static string PreInvoice => "RegistertheFactor";
            //حذف و بایگانی قرارداد
            public static string ArchiveContract => "ArchiveContract";
            //حذف و بایگانی پیش فاکتور
            public static string ArchiveFactor => "ArchiveFactor";
           
            
            public static string Code => "25";


        }
        public static class AccountReferenceGroup
        {
            //-----------تامین کننده
            public static string ProviderCode => "12";
            //----------تامین کننده و خریدار
            public static string ProviderAndBuyerCode => "18";
            //----------تامین کننده داخلی
            public static string InternalProvider => "01";
            //---------تامین کننده خارجی
            public static string ExternalProvider => "02";

            //---------کارکنان
            public static string Personal => "19";
            public static int accountReferenceGroupId_personnel => 32;

        }
        public static class BaseValue
        {

            //فرمول ساخت شماره سریال قرارداد
            public static string UtilityDocument => "UtilityDocumentNumbering";
            //--------------------from basevalue table on value column  تنظیمات خرید ------------------------
            public static int InvoiceALLBaseValue => 337;
            //بررسی نشده
            public static int NotChecked => 28599;
            //-----------------------------------------------------------
            public static int currencyBaseValueId => 308;
            public static string currencyIRR => "IRR";
            //-----------------------------------------------
            public static int DocumentTagBaseValueId => 331;

            //--------------------------روش نرخ گذاری-----------------------------
            //--------------------------روش نرخ گذاری-----------------------------
            public static int BasedOnWeightedAverage => 28529;
            //---------------------مالیات ارزش افزوده-----------------------------
            public static string vatDutiesTax => "Vat";
            

        }
        
       

    }
   
}
