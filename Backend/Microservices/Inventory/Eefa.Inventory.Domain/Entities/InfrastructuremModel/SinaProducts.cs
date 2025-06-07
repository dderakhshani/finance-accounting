using System;

namespace Eefa.Inventory.Domain
{
    public class SinaProduct
    {
        public int? Id { get; set; }//id سینا
        public string ProductCode { get; set; } // کد تدبیر
        public string UnitSaleType { get; set; } // واحد کالا
        public string ProductName { get; set; } // نام کالا
        public double? Amount { get; set; }
        public double? Price { get; set; }
    }



    // ورود.خروج
    public class SinaProducingProduct
    {
        public string ProductCode { get; set; } // کد تدبیر
        public double Quantity { get; set; } // متراژ
        public string WarehouseCode { get; set; } //  ثابت بود
        public string FactorNo { get; set; }
        public float Wieght { get; set; }
    }


    public class SinaProducingInputProduct
    {
        public int? CommodityId { get; set; }
        public object RegDatePersian { get; set; }
        public string LineName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Thickness { get; set; }
        public string SizeName { get; set; }
        public int SizeBaseId { get; set; }
        public string GlazeName { get; set; }
        public string Grade { get; set; }
        public int RawGradesId { get; set; }
        public string BoxName { get; set; }
        public int QuantityTemp { get; set; }
        public int BoxCountTile { get; set; }
        public float FactorNumber { get; set; }
        public float QuantitySum { get; set; }
        public float Total { get; set; }
        public string TadbirName { get; set; }
        public string TadbirCode { get; set; }
        public string PolishName { get; set; }
        public string TileBrandName { get; set; }
        public string TileBrandUniqueName { get; set; }
        public int PalletBoxCount { get; set; }
        public string PrintTypeNamePersian { get; set; }
        public string TadbirNameTemp { get; set; }
        public string BodyName { get; set; }
        public object PersianDate { get; set; }
        public string ProductNameTemp { get; set; }
        public object LocationRow { get; set; }
        public object LocationLine { get; set; }
        public object Location { get; set; }
        public object QuantityS { get; set; }
        public object FactorNumberTow { get; set; }
        public object PaletBoxCount { get; set; }
        public object PaletNumber { get; set; }
        public object QuantitySumAll { get; set; }
        public object QuantityOld { get; set; }
        public float AllQuantitySum { get; set; }
        public object type { get; set; }
        public object BarCode { get; set; }
        public int LocationId { get; set; }
        public int Quantity { get; set; }
        public object FactorName { get; set; }
        public object Line { get; set; }
        public object UserName { get; set; }
        public int AllQuantityBox { get; set; }
        public bool IsUsedWaste { get; set; }
        public int OtherProperties { get; set; }
        public object TypeParking { get; set; }
        public object GradeTN { get; set; }
        public object SizeTN { get; set; }
        public object GlazeTN { get; set; }
        public object ShapeTC { get; set; }
        public object GradeTC { get; set; }
        public object SizeTC { get; set; }
        public object ProductCodeTemp { get; set; }
        public int BoxNameBaseId { get; set; }
        public int FiringTypeBaseId { get; set; }
        public object TrackingCode { get; set; }
        public object TejaratSystemCode { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SectionRowCell { get; set; }
        public object Radiance { get; set; }
        public object MarpackId { get; set; }
        public DateTime RegDate { get; set; }
        public int Type { get; set; }
        public object InputOutputType { get; set; }
        public object RelatedOutputId { get; set; }
        public object Barcode { get; set; }
        public object UserId { get; set; }
        public object QuantityCell { get; set; }
        public object GradeId { get; set; }
        public object FactorId { get; set; }
        public object RequestNumber { get; set; }
        public object QuantityMeter { get; set; }
        public object ConfirmationDate { get; set; }
        public object SaleProduct2Id { get; set; }
        public object Confirmation { get; set; }
        public object FiscalYear { get; set; }
        public object Wieght { get; set; }

    }


    public class FilesByPaymentNumberResult
    {
        public FilesByPaymentNumber[] FilesByPaymentNumbers { get; set; }
    }

    public class FilesByPaymentNumber
    {
        public object FullName { get; set; }
        public string TypeName { get; set; }
        public DateTime UploadDate { get; set; }
        public object VerifierUserId { get; set; }
        public object VerifyDate { get; set; }
        public bool IsUsed { get; set; }
        public object UseDescription { get; set; }
        public int Id { get; set; }
        public int RequestPaymentId { get; set; }
        public object RequestPaymentInvoiceId { get; set; }
        public string FileName { get; set; }
        public int Type { get; set; }
        public int UserId { get; set; }
        public object IsDelete { get; set; }
        public bool IsVerified { get; set; }
        public object DocumentFileId { get; set; }
        public object CheckedUsers { get; set; }
        public object RequestPayment { get; set; }
        public object DocumentFile { get; set; }
        public object RequestPaymentInvoice { get; set; }
    }



}
