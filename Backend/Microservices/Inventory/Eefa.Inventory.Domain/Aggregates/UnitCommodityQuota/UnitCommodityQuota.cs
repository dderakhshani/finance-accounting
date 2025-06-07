using Eefa.Common.Data;
using Eefa.Common.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Inventory.Domain
{

    [Table("UnitCommodityQuota", Schema = "inventory")]
    public partial class UnitCommodityQuota : DomainBaseEntity, IAggregateRoot, IAuditable
    {

        public int CommodityId { get; set; } = default!;
        public int QuotaGroupsId { get; set; } = default!;
        public int CommodityQuota { get; set; } = default!;
        public int QuotaDays { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}
