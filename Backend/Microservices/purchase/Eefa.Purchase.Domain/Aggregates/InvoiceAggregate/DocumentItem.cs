using System.Collections.Generic;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Domain.Aggregates.InvoiceAggregate
{
    /// <summary>
    /// ریز اقلام اسناد
    /// </summary>
    public partial class DocumentItem : DomainBaseEntity
    {

        public int DocumentHeadId { get; set; } = default!;
        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        /// <description>
        /// سریال کالا
        ///</description>

        public string? CommoditySerial { get; set; }
        public string? Description { get; set; }
        
        /// <description>
        /// قیمت واحد 
        ///</description>

        public double UnitPrice { get; set; } = default!;

        /// <description>
        /// قیمت در سیستم  درخواست 
        ///</description>

        public long UnitBasePrice { get; set; } = default!;
        
        /// <description>
        /// قیمت پایه
        ///</description>

        public double ProductionCost { get; set; } = default!;
        public double Weight { get; set; } = default!;
        /// <description>
        /// تعداد براساس واحد اصلی
        ///</description>
        
        public double Quantity { get; set; } = default!;
        /// <summary>
        /// تعداد براساس واحد ورودی کالا
        /// </summary>
        public double? SecondaryQuantity { get; set; } = default!;
        
        public int? CurrencyBaseId { get; set; }
        /// <summary>
        /// واحد تحویل گرفته شده کالا
        /// </summary>
        public int DocumentMeasureId { get; set; }
        /// <summary>
        /// ارتباط با جدول ضریب تبدیل واحد ها
        /// </summary>
        public int? MeasureUnitConversionId { get; set; }
        /// <summary>
        /// واحد اصلی کالا
        /// </summary>
        public int MainMeasureId { get; set; }
        public double? ConversionRatio { get; set; }
        /// <description>
        /// نرخ ارز
        ///</description>

        public double? CurrencyPrice { get; set; }
        /// <description>
        /// تخفیف
        ///</description>

        public long Discount { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>
        public bool? IsWrongMeasure { get; set; } = default!;
        /// <summary>
        /// تعداد کالای باقی مانده از درخواست خرید
        /// </summary>
        public double? RemainQuantity { get; set; } = default!;
        public virtual Commodity Commodity { get; set; }
        public virtual Invoice DocumentHead { get; set; } = default!;
        public virtual MeasureUnit DocumentMeasure { get; set; }
        public virtual IEnumerable<WarehouseHistory> WarehouseHistories { get; set; } = default!;
        public virtual IEnumerable<WarehouseLayout> WarehouseLayout { get; set; } = default!;

        public virtual Year Year { get; set; } = default!;
       
        
    }
}
