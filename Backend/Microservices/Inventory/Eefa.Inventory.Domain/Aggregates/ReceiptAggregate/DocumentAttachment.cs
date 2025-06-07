
namespace Eefa.Inventory.Domain
{
    using Eefa.Common.Domain;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Eefa.Common.Data;
    [Table("DocumentAttachments", Schema = "common")]
    public partial class DocumentAttachment : DomainBaseEntity, IAggregateRoot
    {
        public int DocumentId { get; set; }
        public int AttachmentId { get; set; }
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}
