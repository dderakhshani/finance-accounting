using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    public partial class BomItem: DomainBaseEntity
    {
        public int BomId { get; set; } = default!;
        public int SubCategoryId { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    

    public virtual Bom Bom { get; set; } = default!;
    public virtual CommodityCategory SubCategory { get; set; } = default!;
    }
}
