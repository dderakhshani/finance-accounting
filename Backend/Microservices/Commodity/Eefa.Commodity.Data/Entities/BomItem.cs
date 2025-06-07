using Eefa.Common.Domain;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BomItem : DomainBaseEntity
    {
        public int BomId { get; set; } = default!;
        public int? SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;

        public virtual Bom Bom { get; set; } = default!;
        public virtual Commodity Commodity { get; set; } = default!;
        
    }
}
