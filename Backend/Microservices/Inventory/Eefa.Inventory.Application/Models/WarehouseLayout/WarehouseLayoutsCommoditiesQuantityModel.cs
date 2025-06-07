using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application { 
    /// <summary>
    /// تعداد موجودی کالا در هر موقعیت انبار
    /// </summary>
    public class WarehouseLayoutsCommoditiesQuantityModel : IMapFrom<WarehouseLayoutsCommoditiesQuantityView>
    {
        public int Id { get; set; }
        public int? WarehouseId { get; set; }
        public int? WarehouseLayoutId { get; set; }
        public int? CommodityId { get; set; }
        public double? Quantity { get; set; }
       
        public string WarehouseTitle { get; set; }
        public string WarehouseLayoutTitle { get; set; }
        public string CommodityTitle { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityTadbirCode { get; set; }
        public int? WarehouseLayoutCapacity { get; set; }
        public double? WarehouseLayoutAvailableQuantity { get; set; }

    }
    
}
