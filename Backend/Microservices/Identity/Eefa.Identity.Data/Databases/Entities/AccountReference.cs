using System.Collections.Generic;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class AccountReference : BaseEntity
    {


        /// <summary>
        /// شناسه
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool? IsActive { get; set; } = default!;


        public virtual Person Person { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId1Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId2Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId3Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferences { get; set; } = default!;
    }
}
