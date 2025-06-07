using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    //[HasUniqueIndex]
    public partial class WarehousesCategories : DomainBaseEntity
    {
        public int WarehouseId { get; set; }

        //[UniqueIndex]
        public int CommodityCategoryId { get; set; }
       
        
    }


}
