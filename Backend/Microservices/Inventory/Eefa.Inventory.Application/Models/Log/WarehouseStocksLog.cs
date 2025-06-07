namespace Eefa.Inventory.Application
{

    public  class WarehouseStocksLog

    {
    
        public int WarehousId { get; set; } = default!;
    /// <description>
            /// کد کالا
    ///</description>
    
        public int CommodityId { get; set; } = default!;
    /// <description>
            /// تعداد
    ///</description>
    
        public double Quantity { get; set; } = default!;
    /// <description>
            /// موجودی منطقی
    ///</description>
    
        public double ReservedQuantity { get; set; } = default!;
    

       
    }
}


