using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class AccountReferencesRelReferencesGroup : BaseEntity
    {
        /// <summary>
        /// کد طرف حساب
        /// </summary>
        public int ReferenceId { get; set; } = default!;

        /// <summary>
        /// کد گروه طرف حساب
        /// </summary>
        public int ReferenceGroupId { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AccountReference Reference { get; set; } = default!;
        public virtual AccountReferencesGroup ReferenceGroup { get; set; } = default!;
    }
}
