using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{ 
    public class AccessToWarehouseModel : IMapFrom<AccessToWarehouse>
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; } = default!;

        public int UserId { get; set; } = default!;

        public string TableName { get; set; } = default!;

        public string Title { get; set; } = default!;
    }
}
