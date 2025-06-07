using Library.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "Archive", Schema = "admin")]
    public class Archive : BaseEntity
    {
        public int BaseValueTypeId { get; set; } = default!;
        public int TypeBaseId { get; set; } = default!;
        public string FileNumber { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string KeyWords { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValueType BaseValueType { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
        public virtual List<ArchiveAttachments> ArchiveAttachments { get; set; }
    }
}
