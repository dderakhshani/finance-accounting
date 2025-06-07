using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1591;&#1604;&#1575;&#1593;&#1575;&#1578; &#1588;&#1585;&#1705;&#1578;
    /// </summary>
    public partial class CompanyInformations : BaseEntity
    {
        public CompanyInformations()
        {
            AccountReferencesGroups = new HashSet<AccountReferencesGroups>();
            CodeAccountHeadGroups = new HashSet<CodeAccountHeadGroup>();
            CodeAutoVoucherViews = new HashSet<CodeAutoVoucherViews>();
            CodeRowDescriptions = new HashSet<CodeRowDescription>();
            CodeVoucherGroups = new HashSet<CodeVoucherGroups>();
            UserCompanies = new HashSet<UserCompany>();
            VouchersHeads = new HashSet<VouchersHead>();
            Years = new HashSet<Years>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//تاریخ انقضاء
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
//حداکثر تعداد کابران
        /// </summary>
        public int MaxNumOfUsers { get; set; } = default!;

        /// <summary>
//لوگو
        /// </summary>
        public string? Logo { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual ICollection<AccountReferencesGroups> AccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherViews> CodeAutoVoucherViews { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
        public virtual ICollection<CodeVoucherGroups> CodeVoucherGroups { get; set; } = default!;
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = default!;
        public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
        public virtual ICollection<Years> Years { get; set; } = default!;
    }
}
