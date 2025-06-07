using System.Collections.Generic;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class AccountReferencesGroup : BaseEntity
    {
     

        /// <summary>
        /// کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

       

        public virtual CompanyInformation Company { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual AccountReferencesGroup? Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroup> InverseParent { get; set; } = default!;
    }
}
