using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{

    public record WarehouseCountFormReportModel
    {
        public int Id { get; set; }
        public int WarehouseLayoutQuantitiesId { get; set; }
        public string CountedQuantitiesString { get; set; }
        public IEnumerable<double?> CountedQuantitList { get; set; }
        public Dictionary<string, double?> CountedQuantities { get; set; } = new();
        public WarehouseLayoutStatus LastWarehouseLayoutStatus { get; set; }        
        public string CommodityName { get; set; }
        public string CommodityCompactCode { get; set; }
        public int CommodityId { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public double SystemQuantity { get; set; }
        public int NumberCounted { get; set; }
    }

}
