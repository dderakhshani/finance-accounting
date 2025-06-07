using System.Collections.Generic;
using Eefa.Common.Domain;
using Eefa.Purchase.Domain.Aggregates.InvoiceAggregate;

namespace Eefa.Purchase.Domain.Entities
{
    public partial class Warehouse : DomainBaseEntity
    {
        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        ///</description>

        public string? LevelCode { get; set; }
        /// <description>
        /// عنوان
        ///</description>

        public string Title { get; set; } = default!;
        /// <description>
        /// فعال
        ///</description>

        public bool IsActive { get; set; } = default!;
        /// <description>
        /// کد گروه کالا
        ///</description>

        public int? CommodityCategoryId { get; set; } = default!;
        /// <description>
        /// نقش صاحب سند
        ///</description>
         
        public int? PermissionId { get; set; } = default!;
        public virtual CommodityCategory CommodityCategory { get; set; } = default!;

        public virtual Warehouse? Parent { get; set; }
        public virtual ICollection<Warehouse> InverseParent { get; set; } = default!;
        public virtual ICollection<Stock> Stocks { get; set; } = default!;
        public virtual ICollection<Invoice> Invoice { get; set; } = default!;
        public virtual ICollection<WarehouseLayout> WarehouseLayouts { get; set; } = default!;
    }


}
