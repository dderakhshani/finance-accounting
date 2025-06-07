using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{ 
    public class WarehouseStockModel: IMapFrom<WarehouseStocks>
    {
        public int Id { get; set; } = default!;
        public int WarehouseId { get; set; } = default!;
        public string WarehouseTitle{ get; set; } = default!;
        /// <description>
        /// کد کالا
        ///</description>

        public int CommodityId { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string CommodityTadbirCode { get; set; } = default!;
        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        /// <description>
        /// موجودی منطقی
        ///</description>

        public double ReservedQuantity { get; set; } = default!;

        public double AvailableQuantity { get; set; } = default!;
        public double? Price { get; set; } = default!;

    }
}
