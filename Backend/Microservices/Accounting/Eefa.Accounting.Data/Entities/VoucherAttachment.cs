using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class VoucherAttachment : BaseEntity
    {
        public int VoucherHeadId { get; set; } = default!;

        /// <summary>
        /// کد پیوست
        /// </summary>
        public int AttachmentId { get; set; } = default!;


        public virtual Attachment Attachment { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual VouchersHead VoucherHead { get; set; } = default!;
    }
}
