using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{

    [Table("AccessToWarehouse", Schema = "inventory")]
    public partial class AccessToWarehouse : DomainBaseEntity, IAggregateRoot, IAuditable
    {
        public int WarehouseId { get; set; } = default!;
       
        public int UserId { get; set; } = default!;
       
        public string TableName { get; set; } = default!;
        
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
   

}
