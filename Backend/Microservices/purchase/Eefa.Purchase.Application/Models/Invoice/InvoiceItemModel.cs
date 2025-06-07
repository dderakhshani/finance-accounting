using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;
using Eefa.Purchase.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Application.Models.Receipt
{
    
    /// <summary>
    /// ریز اقلام اسناد
    /// </summary>
    public record InvoiceItemModel : IMapFrom<DocumentItem>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentItem, InvoiceItemModel>();
            profile.CreateMap<ReceiptItemsView, InvoiceItemModel>().ForMember(o => o.UnitPrice, opt => opt.MapFrom(src => src.ItemUnitPrice));
           


        }
        public int Id { get; set; } = default!;
        public int DocumentHeadId { get; set; } = default!;
        public int? DocumentItemId { get; set; } = default!;
        public int? DocumentNo { get; set; } = default!;

        public string RequestNo { get; set; } = default!;
       

        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        /// <description>
        /// سریال کالا
        ///</description>

        public string? CommoditySerial { get; set; }
        /// <description>
        /// قیمت واحد 
        ///</description>

        public double UnitPrice { get; set; } = default!;
        public double TotalPrice { get { return UnitPrice * Quantity; } } 
        /// <description>
        /// قیمت در سیستم  درخواست 
        ///</description>

        public long? UnitBasePrice { get; set; } = default!;
        /// <description>
        /// قیمت پایه
        ///</description>

        public double ProductionCost { get; set; } = default!;

        public double Weight { get; set; } = default!;
        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
      
        public double? QuantityChose { get; set; }
        public double QuantityUsed { get; set; } = default!;
        public int? CurrencyBaseId { get; set; }
        public string CurrencyBaseTitle { get; set; }
        public int DocumentMeasureId { get; set; }
        public string DocumentMeasureTitle { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public int MainMeasureId { get; set; }
        public double? ConversionRatio { get; set; }
        public double? SecondaryQuantity { get; set; } = default!;
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
        ///
        /// <summary>
        /// تعداد کالای باقی مانده از درخواست خرید
        /// </summary>
        public double? RemainQuantity { get; set; } = default!;
        public bool? IsWrongMeasure { get; set; } = default!;
        public string Description { get; set; }
        public  InvoiceCommodityModel Commodity { get; set; } = default!;
        public List<MeasureUnitConversionModel> MeasureUnitConversions { get; set; } = default!;
        public List<CommodityMeasureUnitModel> CommodityMeasureUnits { get; set; } = default!;
       

        
    }
   
}
