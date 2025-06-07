using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{


    using Eefa.Common.Data;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    [Table("WarehousesCodeVoucherGroups", Schema = "inventory")]
    public partial class WarehousesCodeVoucherGroups : DomainBaseEntity, IAuditable
    {
        public int WarehouseId { get; set; }
       
        public int CodeVoucherGroupId { get; set; }
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }

    }


}
