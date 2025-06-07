using System;
using System.Collections.Generic;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// مواد فرمول ساخت
    /// </summary>
    public partial class BomValueHeader: DomainBaseEntity
    {
    
        public int BomId { get; set; } = default!;
    /// <description>
            /// کد کالا
    ///</description>
    
        public int CommodityId { get; set; } = default!;
    /// <description>
            /// تاریخ فرمول ساخت
    ///</description>
    
        public DateTime BomDate { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual Bom Bom { get; set; } = default!;
    public virtual Commodity Commodity { get; set; } = default!;
                public virtual ICollection<BomValue>
    BomValues { get; set; } = default!;
    }
}
