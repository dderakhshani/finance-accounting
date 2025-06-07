using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class CodeVoucherExtendType : BaseEntity
    {
        public string Title { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
