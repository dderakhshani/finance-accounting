using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// مقادیر موقعیت های انبار
    /// </summary>
    [Table("WarehouseLayoutQuantities", Schema = "inventory")]
    public partial class WarehouseLayoutQuantity: DomainBaseEntity, IAggregateRoot
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
    
    //public virtual Commodity Commodity { get; set; } = default!;
    //public virtual WarehouseLayout WarehouseLayout { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}
