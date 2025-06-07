using AutoMapper;
using Eefa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application.Commands.Receipt.Update
{
    public class ReceiptDocumentItemUpdate : IMapFrom<DocumentItem>
    {
        public int Id { get; set; }
        public int CommodityId { get; set; } = default!;

        /// <description>
        /// سریال کالا
        ///</description>

        public string CommoditySerial { get; set; }
        public double? SecondaryQuantity { get; set; } = default!;

        /// <description>
        /// قیمت واحد 
        ///</description>
        public int MainMeasureId { get; set; }
        public double ConversionRatio { get; set; }
        public long UnitPrice { get; set; } = default!;
        /// <description>
        /// قیمت در سیستم  درخواست 
        ///</description>

        public long? UnitBasePrice { get; set; } = default!;

        /// <description>
        /// قیمت پایه
        ///</description>

        public long ProductionCost { get; set; } = default!;


        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;

        public int? CurrencyBaseId { get; set; }

        /// <description>
        /// نرخ ارز
        ///</description>

        public int? CurrencyPrice { get; set; }
        public int DocumentMeasureId { get; set; }
        public int? MeasureUnitConversionId { get; set; }
        public string Description { get; set; }
        public bool IsWrongMeasure { get; set; } = default!;

        public AssetsModel Assets { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ReceiptDocumentItemUpdate, Domain.DocumentItem>()
                .IgnoreAllNonExisting();
        }

    }
}

