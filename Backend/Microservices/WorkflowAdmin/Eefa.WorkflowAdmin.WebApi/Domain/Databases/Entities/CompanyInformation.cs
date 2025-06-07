using System;
using System.Collections.Generic;
using Library.Common;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    public partial class CompanyInformation : BaseEntity
    {
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضاء
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// حداکثر تعداد کابران
        /// </summary>
        public int MaxNumOfUsers { get; set; } = default!;

        /// <summary>
        /// لوگو
        /// </summary>
        public string? Logo { get; set; }


        public ICollection<UserCompany> UserCompanies { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroup> AccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
        public virtual ICollection<CodeVoucherGroup> CodeVoucherGroups { get; set; } = default!;
        public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
        public virtual ICollection<Year> Years { get; set; } = default!;
    }
}
