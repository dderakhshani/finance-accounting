using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1585;&#1740;&#1586; &#1575;&#1602;&#1604;&#1575;&#1605; &#1575;&#1587;&#1606;&#1575;&#1583;
    /// </summary>
    public partial class DocumentItems : BaseEntity
    {
        public DocumentItems()
        {
            DocumentItemsBoms = new HashSet<DocumentItemsBom>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سرفصل سند
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
//کد سال
        /// </summary>
        public int YearId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        /// <summary>
//لوکیشن کالا در انبار 
        /// </summary>
        public int? WarehouseLayoutId { get; set; }

        /// <summary>
//نوع ورود1 خروج -1
        /// </summary>
        public int? IOMode { get; set; }

        /// <summary>
//سریال کالا
        /// </summary>
        public string? CommoditySerial { get; set; }

        /// <summary>
//قیمت در سیستم  درخواست 
        /// </summary>
        public long UnitBasePrice { get; set; } = default!;

        /// <summary>
//قیمت واحد 
        /// </summary>
        public long UnitPrice { get; set; } = default!;

        /// <summary>
//قیمت پایه
        /// </summary>
        public long ProductionCost { get; set; } = default!;

        /// <summary>
//وزن کالا 
        /// </summary>
        public double Weight { get; set; } = default!;

        /// <summary>
//تعداد
        /// </summary>
        public double Quantity { get; set; } = default!;

        /// <summary>
//تعداد/مقدار باقی مانده 
        /// </summary>
        public double? RemainQuantity { get; set; }

        /// <summary>
//تعداد / مقدار فرعی
        /// </summary>
        public double? SecondaryQuantity { get; set; }

        /// <summary>
//نوع ارز
        /// </summary>
        public int? CurrencyBaseId { get; set; }

        /// <summary>
//مبلغ ارز
        /// </summary>
        public long? CurrencyPrice { get; set; }

        /// <summary>
//نرخ واحد تبدیل ارز
        /// </summary>
        public long? CurrencyRate { get; set; }

        /// <summary>
//تخفیف
        /// </summary>
        public long Discount { get; set; } = default!;

        /// <summary>
//شرح کالا
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//واحد شمارش اصلی کالا
        /// </summary>
        public int MainMeasureId { get; set; } = default!;

        /// <summary>
//واحد شمارش فعلی کالا
        /// </summary>
        public int DocumentMeasureId { get; set; } = default!;

        /// <summary>
//اعلام اشتباه بودن واحد کالا 
        /// </summary>
        public bool? IsWrongMeasure { get; set; }

        /// <summary>
//ضریب تبدیل به واحد اصلی
        /// </summary>
        public int? MeasureUnitConversionId { get; set; }

        /// <summary>
//نرخ تبدیل-فعلا استفاده نمیشود  
        /// </summary>
        public double? ConversionRatio { get; set; }

        /// <summary>
//شماره فرمول ساخت 
        /// </summary>
        public int? BomValueHeaderId { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual DocumentHeads DocumentHead { get; set; } = default!;
        public virtual MeasureUnits DocumentMeasure { get; set; } = default!;
        public virtual MeasureUnitConversions MeasureUnitConversion { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Years Year { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBoms { get; set; } = default!;
    }
}
