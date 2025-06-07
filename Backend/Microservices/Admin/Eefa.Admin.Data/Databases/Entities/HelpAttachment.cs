using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class HelpAttachment : BaseEntity
    {
        public int HelpDataId { get; set; } = default!;

        /// <summary>
        /// کد پیوست
        /// </summary>
        public int AttachmentId { get; set; } = default!;


        public virtual Attachment Attachment { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual HelpData HelpData { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
