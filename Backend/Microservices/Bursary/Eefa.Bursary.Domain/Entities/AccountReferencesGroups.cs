using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1711;&#1585;&#1608;&#1607; &#1591;&#1585;&#1601; &#1581;&#1587;&#1575;&#1576;&#1607;&#1575;
    /// </summary>
    public partial class AccountReferencesGroups : BaseEntity
    {
        public AccountReferencesGroups()
        {
            AccountHeadRelReferenceGroups = new HashSet<AccountHeadRelReferenceGroup>();
            AccountReferencesRelReferencesGroups = new HashSet<AccountReferencesRelReferencesGroups>();
            ChequeSheetsOwnerChequeReferenceGroups = new HashSet<ChequeSheets>();
            ChequeSheetsReceiveChequeReferenceGroups = new HashSet<ChequeSheets>();
            DocumentHeadsCreditAccountReferenceGroups = new HashSet<DocumentHeads>();
            DocumentHeadsDebitAccountReferenceGroups = new HashSet<DocumentHeads>();
            FinancialRequestDetailsCreditAccountReferenceGroups = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsDebitAccountReferenceGroups = new HashSet<FinancialRequestDetails>();
            FinancialRequestPartialCreditAccountReferencesGroups = new HashSet<FinancialRequestPartial>();
            FinancialRequestPartialDebitAccountReferencesGroups = new HashSet<FinancialRequestPartial>();
            InverseParent = new HashSet<AccountReferencesGroups>();
            VouchersDetails = new HashSet<VouchersDetail>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }
        public string Code { get; set; } = default!;

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//آیا قابل ویرایش است؟
        /// </summary>
        public bool? IsEditable { get; set; } = default!;

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
         

        public virtual CompanyInformations Company { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual AccountReferencesGroups Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsOwnerChequeReferenceGroups { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsReceiveChequeReferenceGroups { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsCreditAccountReferenceGroups { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsDebitAccountReferenceGroups { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsCreditAccountReferenceGroups { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsDebitAccountReferenceGroups { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialCreditAccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialDebitAccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroups> InverseParent { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    }
}
