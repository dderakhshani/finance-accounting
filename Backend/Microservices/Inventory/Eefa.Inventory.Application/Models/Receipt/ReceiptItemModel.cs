using System;
using System.Collections.Generic;
using AutoMapper;
using Eefa.Common;

using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    /// <summary>
    /// ریز اقلام اسناد
    /// </summary>
    public record ReceiptItemModel : IMapFrom<DocumentItem>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DocumentItem, ReceiptItemModel>();
            profile.CreateMap<DocumentItemsBom, ReceiptItemModel>();

        }
        public int? Id { get; set; }
        public int? DocumentHeadId { get; set; } 
        public int? DocumentItemId { get; set; } = default!;
        public int? DocumentNo { get; set; } = default!;
        public string RequestNo { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string MeasureTitle { get; set; } = default!;


        /// <description>
        /// کد سال
        ///</description>

        public int YearId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        /// <description>
        /// سریال کالا
        ///</description>

        public string CommoditySerial { get; set; }
        /// <description>
        /// قیمت واحد 
        ///</description>

        public double UnitPrice { get; set; } = default!;
        public double? UnitPriceWithExtraCost { get; set; } = default!;

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
        public double? QuantityUsed { get; set; } = default!;
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
        public double? CurrencyRate { get; set; }
        /// <description>
        /// تخفیف
        ///</description>

        public long Discount { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>
        ///
        public bool? IsWrongMeasure { get; set; }
        /// <summary>
        /// تعداد کالای باقی مانده از درخواست خرید
        /// </summary>
        public double? RemainQuantity { get; set; } = default!;
        public string Description { get; set; }
        public int? BomValueHeaderId { get; set; }
        //اجازه دریافت کالا را دارد؟
        public bool? HasPermissionReceive { get; set; }
        //اجازه ویرایش تعداد را دارد؟
        public bool? HasPermissionEditQuantity { get; set; }
        public bool selected { get; set; } = false;
        public int  AssetsSerialsCount { get; set; } = 0;
        public double? CommodityQuota { get; set; } = 0;
        public double? CommodityQuotaUsed { get; set; } = 0;

        public bool? IsImportPurchase { get; set; }

        
        public  ReceiptCommodityModel Commodity { get; set; } = default!;
        public List<MeasureUnitConversionModel> MeasureUnitConversions { get; set; } = default!;
        public List<CommodityMeasureUnitModel> CommodityMeasureUnits { get; set; } = default!;
        public  List<WarehouseHistoryModel> WarehouseHistories { get; set; } = default!;
        public WarehouseLayoutQuantityModel WarehouseLayoutQuantity { get; set; } = default!;
        public List<WarehouseLayoutsCommoditiesModel> Layouts { get; set; } = default!;
        public List<AssetsSerialModel> AssetsSerials { get; set; } = default!;
        public List<spGetConsumptionCommodityByRequesterReferenceId> ConsumptionCommodity { get; set; } = default!;


    }
   
}
