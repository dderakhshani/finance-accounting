using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{

    public partial class WarehouseLayoutCategories : DomainBaseEntity
    {
        public int WarehouseLayoutId { get; set; }
        public int CategoryId { get; set; }



        public virtual CommodityCategory Category { get; set; } = default!;
        public virtual WarehouseLayout WarehouseLayout { get; set; } = default!;

    }


}
