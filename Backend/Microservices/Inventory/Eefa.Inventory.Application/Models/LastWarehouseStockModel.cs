using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{ 
    public class LastWarehouseStockModel : IMapFrom<WarehouseLayoutQuantity>
    {
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public string WarehouseTitle{ get; set; } = default!;
        public int WarehouseLayoutId { get; set; }
        public int WarehouseLayoutTilte { get; set; }
        public int CommodityId { get; set; } = default!;
        public string CommodityCode { get; set; }
        public string CommodityTitle { get; set; } = default!;         
        public double Quantity { get; set; } = default!;              
    }
}
