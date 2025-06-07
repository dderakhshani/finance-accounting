using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.Accounting.Data.Entities
{

    public partial class AccountReferencesGroup : HierarchicalBaseEntity
    {

        //public int? PermissionId { get; set; }
        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        [UniqueIndex]
        public string Code { get; set; }


        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;
        public bool? IsVisible { get; set; } = default!;


        public string TadbirCode { get; set; }

        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AccountReferencesGroup? Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroup> InverseParent { get; set; } = default!;
        public virtual ICollection<Customer> Customers { get; set; } = default!;

        //public virtual ICollection<DocumentHead> DocumentHeads { get; set; }
    }
}
