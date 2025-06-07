using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Domain
{  
    public class WarehouseCountFormReport
    {
        public int WarehouseLayoutQuantitiesId { get; set; }
        public string CommodityCompactCode { get; set; }
        public string CommodityCode { get; set; }
        public string MeasureTitle { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public double SystemQuantity { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public double? CountedQuantity1 { get; set; }
        public double? CountedQuantity2 { get; set; }
        public double? CountedQuantity3 { get; set; }
        public double? CountedQuantity4 { get; set; }
        public double? CountedQuantity5 { get; set; }
        public double? FinalQuantity { get; set; }
        public double? DiscrepancyQuantity { get; set; }







    }
}
