using System;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    
    public class WarehouseIOCommodity
    {
        public DateTime DocumentDate { get; set; }
        public int? Mode { get; set; }
        public int? DocumentStauseBaseValue { get; set; }
        public int? yearId { get; set; }
        public int? WarehouseId { get; set; }
        public List<CommodityList> CommodityList { get; set; }
    }
    public class CommodityList
    {
        public string CommodityCode { get; set; }
        public double Quantity { get; set; }
    }
}
