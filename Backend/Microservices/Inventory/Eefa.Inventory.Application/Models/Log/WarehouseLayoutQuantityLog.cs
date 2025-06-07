namespace Eefa.Inventory.Application
{

    public  class WarehouseLayoutQuantitiesLog
    {
    /// <description>
            /// شناسه
    ///</description>
    
        public int WarehouseLayoutId { get; set; } = default!;
    /// <description>
            /// کد کالا
    ///</description>
    
        public int CommodityId { get; set; } = default!;
    /// <description>
            /// تعداد
    ///</description>
    
        public double Quantity { get; set; } = default!;
    
    }
}
