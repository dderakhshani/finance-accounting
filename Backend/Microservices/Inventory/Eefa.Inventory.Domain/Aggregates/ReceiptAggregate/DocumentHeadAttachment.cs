
namespace Eefa.Inventory.Domain
{
    using Eefa.Common.Domain;

    public partial class DocumentHeadAttachment : DomainBaseEntity, IAggregateRoot
    {
        public int DocumentHeadId { get; set; }
        public int AttachmentId { get; set; }
        
    }
}
