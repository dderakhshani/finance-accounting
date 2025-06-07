using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    public partial class Stock : DomainBaseEntity
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
    

        public virtual Commodity Commodity { get; set; } = default!;
        public virtual Warehouse Warehous { get; set; } = default!;
       
    }
}


