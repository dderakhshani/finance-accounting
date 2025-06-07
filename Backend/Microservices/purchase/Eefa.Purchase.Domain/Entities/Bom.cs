using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// فرمول های ساخت
    /// </summary>
    public partial class Bom: DomainBaseEntity
    {
    
        public int? ParentId { get; set; }
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
    /// <description>
            /// کد سطح
    ///</description>
    
        public string LevelCode { get; set; } = default!;
    /// <description>
            /// کد گروه کالا
    ///</description>
    
        public int CommodityCategoryId { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual CommodityCategory CommodityCategory { get; set; } = default!;
    public virtual Bom? Parent { get; set; }
                public virtual ICollection<BomItem>
    BomItems { get; set; } = default!;
                public virtual ICollection<BomValueHeader>
    BomValueHeaders { get; set; } = default!;
                public virtual ICollection<BomValue>
    BomValues { get; set; } = default!;
                public virtual ICollection<Bom>
    InverseParent { get; set; } = default!;
    }
}
