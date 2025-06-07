using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// مقادیر موقعیت های انبار
    /// </summary>
    public partial class WarehouseLayoutQuantity: DomainBaseEntity
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
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual Commodity Commodity { get; set; } = default!;
    public virtual WarehouseLayout WarehouseLayout { get; set; } = default!;
    }
}
