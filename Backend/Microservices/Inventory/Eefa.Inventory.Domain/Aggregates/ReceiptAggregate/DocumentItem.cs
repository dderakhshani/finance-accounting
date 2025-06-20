﻿using System.Collections.Generic;
using Eefa.Common.Data;
using Eefa.Common.Domain;
using System.ComponentModel.DataAnnotations.Schema;
namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// ریز اقلام اسناد
    /// </summary>
    /// 
    /// 
    [Table("DocumentItems", Schema = "common")]
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

        public string CommoditySerial { get; set; }
        public string Description { get; set; }
        
        /// <description>
        /// قیمت واحد 
        ///</description>

        public double UnitPrice { get; set; } = default!;
        public double? UnitPriceWithExtraCost { get; set; } = default!;
        

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

        /// <summary>
        /// تعداد کالای باقی مانده از درخواست خرید
        /// </summary>
        public double? RemainQuantity { get; set; } = default!;

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
        public double? CurrencyRate { get; set; }

        public int? BomValueHeaderId { get; set; }

        public int? RequestDocumentItemId { get; set; }

        
        /// <description>
        /// تخفیف
        ///</description>
        public long Discount { get; set; } = default!;
        
        public bool? IsWrongMeasure { get; set; } = default!;
       
        public virtual Receipt DocumentHead { get; set; } = default!;
       

    }
}
