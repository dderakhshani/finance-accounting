using Eefa.Inventory.Domain.Enum;

namespace Eefa.Inventory.Domain
{  
    public class spWarehouseLayoutRecursive
    {
        public int WarehouseLayoutId { get; set; }
        public int?  ParentId  { get; set; }
        public string ParentChildTitle { get; set; }
        public int Level { get; set; }
        public WarehouseLayoutStatus Status { get; set;}
    }
}
