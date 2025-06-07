using System.Collections.Generic;
using Library.Attributes;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    [HasUniqueIndex]
    public partial class AccountReference : BaseEntity
    {


        /// <summary>
        /// شناسه
        /// </summary>
       [UniqueIndex]
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        public string? Description { get; set; } = default!;

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;

        public string? DepositId { get; set; } = default!;

        public virtual Person Person { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId1Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId2Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId3Navigation { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeads { get; set; } = default!;
        public virtual ICollection<MoadianInvoiceHeader> MoadianInvoices { get; set; } = default!;


    }
}
