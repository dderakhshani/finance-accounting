using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    public partial class BomItem: DomainBaseEntity
    {
        public int BomId { get; set; } = default!;
        public int? SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        
       
    }
}
