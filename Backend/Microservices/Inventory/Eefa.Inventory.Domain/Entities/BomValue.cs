using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// مقادیر فرمول ساخت
    /// </summary>
    public partial class BomValue: DomainBaseEntity
    {
    
        public int BomValueHeaderId { get; set; } = default!;
    /// <description>
            /// کد کالای مصرفی
    ///</description>
    
        public int UsedCommodityId { get; set; } = default!;
    
    /// <description>
            /// مقدار
    ///</description>
    
        public float Value { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;
    }
}
