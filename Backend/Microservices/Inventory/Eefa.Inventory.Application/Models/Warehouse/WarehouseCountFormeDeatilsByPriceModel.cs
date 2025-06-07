using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain.Aggregates.WarehouseAggregate;
using Eefa.Inventory.Domain.Enum;
using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{

    public class WarehouseCommodityWithPriceModel
    {
        public int Id { get; set; }
        public int warehouseId { get; set; }
        public int WarehouseLayoutQuantityId { get; set; }
        public int WarehouseLayoutId { get; set; }
        public WarehouseLayoutStatus LastWarehouseLayoutStatus { get; set; }    
        public string CommodityName { get; set; }
        public string CommodityCompactCode { get; set; }
        public string CommodityCode { get; set; }
        public string MeasureTitle { get; set; }
        public int CommodityId { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public double SystemQuantity { get; set; }
        public double? Price { get; set; }


    }
}
