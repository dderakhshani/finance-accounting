using Eefa.Common.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// تاریخچه انبار ها
    /// </summary>
    ///  using Eefa.Common.Data;


    [Table("WarehouseHistories", Schema = "inventory")]
    public partial class WarehouseHistory : DomainBaseEntity, IAggregateRoot
    {
        
        public int Commodityld { get; set; } = default!;
    /// <description>
            /// کد چیدمان انبار
    ///</description>
    
        public int WarehouseLayoutId { get; set; } = default!;
        public int? WarehousesId { get; set; } = default!;
        public int? DocumentHeadId { get; set; } = default!;

        public double? AvailableCount { get; set; }
        /// <description>
        /// تعداد
        ///</description>

        public double Quantity { get; set; } = default!;
        public double? AVGPrice { get; set; } = default!;

        
        /// <description>
        /// نوع عملیات
        ///</description>

        public int Mode { get; set; } = default!;
    /// <description>
            /// شماره آیتم در سند
    ///</description>
    
        public int? DocumentItemId { get; set; }

        /// <description>
        ///شماره درخواست خروج
        ///</description>
        ///
        public int? RequestNo { get; set; }

       
    }
}
