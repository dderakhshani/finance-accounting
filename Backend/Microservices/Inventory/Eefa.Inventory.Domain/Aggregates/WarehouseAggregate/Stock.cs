using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    using Eefa.Common.Data;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("WarehouseStocks", Schema = "inventory")]
    public partial class WarehouseStocks : DomainBaseEntity, IAggregateRoot
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

        public double? Price { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}


