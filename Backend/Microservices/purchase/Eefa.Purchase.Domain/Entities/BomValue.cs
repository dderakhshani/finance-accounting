using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// مقادیر فرمول ساخت
    /// </summary>
    public partial class BomValue: DomainBaseEntity
    {
    
        public int BomValueHeaderId { get; set; } = default!;
    /// <description>
            /// کد کالای مصرفی
    ///</description>
    
        public int UsedCommodityId { get; set; } = default!;
    /// <description>
            /// کد فرمول ساخت زیر مجموعه
    ///</description>
    
        public int SubBomId { get; set; } = default!;
    /// <description>
            /// مقدار
    ///</description>
    
        public double Value { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual BomValueHeader BomValueHeader { get; set; } = default!;
    public virtual Bom SubBom { get; set; } = default!;
    public virtual Commodity UsedCommodity { get; set; } = default!;
    }
}
