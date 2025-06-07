using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    [Table("AssetAttachments", Schema = "inventory")]
    public partial class AssetAttachments : DomainBaseEntity, IAuditable
    {
        public int AssetId { get; set; } = default!;
        public int AttachmentId { get; set; } = default!;
        public int PersonsDebitedCommoditiesId { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
   

}
