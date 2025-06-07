using Library.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "ArchiveAttachments", Schema = "admin")]
    public class ArchiveAttachments : BaseEntity
    {
        public int ArchiveId { get; set; }
        public int AttachmentId { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Archive Archive { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
