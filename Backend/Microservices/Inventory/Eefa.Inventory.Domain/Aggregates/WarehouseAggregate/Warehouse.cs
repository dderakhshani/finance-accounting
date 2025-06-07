using Eefa.Common;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    using Eefa.Common.Data;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("Warehouses", Schema = "inventory")]
    public partial class Warehouse : DomainBaseEntity, IAggregateRoot, IHierarchical
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
        public int? AccountHeadId { get; set; } = default!;
        public int? AccountRererenceGroupId { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public int? Sort { get; set; } = default!;
        public bool Countable { get; set; } = default!;

        /// <description>
        /// نقش صاحب سند
        ///</description>

        //public virtual CommodityCategory CommodityCategory { get; set; } = default!;
        //public virtual Warehouse? Parent { get; set; }
        //public virtual ICollection<Warehouse> InverseParent { get; set; } = default!;
        //public virtual ICollection<Stock> Stocks { get; set; } = default!;
        //public virtual ICollection<Receipt> Receipt { get; set; } = default!;
        //public virtual ICollection<WarehouseLayout> WarehouseLayouts { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }


}
