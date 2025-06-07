using Eefa.Common.Data;
using Eefa.Common.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// تاریخچه انبار ها
    /// </summary>
    /// 
    [Table("WarehouseRequestExit", Schema = "inventory")]
    public partial class WarehouseRequestExit : DomainBaseEntity, IAggregateRoot
    {
        
        public int Commodityld { get; set; } = default!;
    /// <description>
            /// کد چیدمان انبار
    ///</description>
    
        public int WarehouseLayoutQuantityId { get; set; } = default!;
    /// <description>
            /// تعداد
    ///</description>
    
        public double Quantity { get; set; } = default!;

        /// <description>
        /// تعداد درخواستی
        ///</description>

        public double? RequestQuantity { get; set; } = default!;

        
        /// <description>
        /// شماره آیتم در درخواست خروج
        ///</description>

        public int? RequestItemId { get; set; } = default!;

        /// <description>
        /// شماره  درخواست خروج
        ///</description>
        public int RequestId { get; set; } = default!;
        /// <description>
        /// شماره آیتم در سند
        ///</description>
        public int? DocumentItemsId { get; set; } = default!;

        /// <description>
        /// شماره سند
        ///</description>
        public int? DocumentHeadId { get; set; } = default!;

        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }


    }
}
