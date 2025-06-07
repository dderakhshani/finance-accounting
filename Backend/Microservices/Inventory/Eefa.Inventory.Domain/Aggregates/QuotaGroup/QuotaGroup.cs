using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    [Table("QuotaGroups", Schema = "inventory")]
    public partial class QuotaGroup : DomainBaseEntity, IAggregateRoot, IAuditable
    {
        
        public int ErpQuotaGroupId { get; set; }
        public string QuotaGroupName { get; set; }
        public Nullable<int> UnitId { get; set; }
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }

    }
   

}
