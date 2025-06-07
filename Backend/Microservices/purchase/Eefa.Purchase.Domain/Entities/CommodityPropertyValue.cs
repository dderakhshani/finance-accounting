using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// مقادیر ویژگی های کالا
    /// </summary>
    public partial class CommodityPropertyValue: DomainBaseEntity
    {
    
        public int CommodityId { get; set; } = default!;
    /// <description>
            /// کد ویژگی گروه
    ///</description>
    
        public int CategoryPropertyId { get; set; } = default!;
    /// <description>
            /// کد آیتم ویژگی مقدار 
    ///</description>
    
        public int? ValuePropertyItemId { get; set; }
    /// <description>
            /// مقدار
    ///</description>
    
        public string? Value { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
    public virtual Commodity Commodity { get; set; } = default!;
    public virtual CommodityCategoryPropertyItem? ValuePropertyItem { get; set; }
    }
}
