using System;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
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

        public string Name { get; set; } = default!;


    }
}
