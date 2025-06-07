using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1591;&#1585;&#1601; &#1581;&#1587;&#1575;&#1576;&#1607;&#1575; 
    /// </summary>
    public partial class AccountReferences : BaseEntity
    {
        public AccountReferences()
        {
            AccountReferencesRelReferencesGroups = new HashSet<AccountReferencesRelReferencesGroups>();
            ChequeSheetsAccountReferences = new HashSet<ChequeSheets>();
            ChequeSheetsOwnerChequeReferences = new HashSet<ChequeSheets>();
            ChequeSheetsReceiveChequeReferences = new HashSet<ChequeSheets>();
            DocumentHeadExtendCorroborantReferences = new HashSet<DocumentHeadExtend>();
            DocumentHeadExtendFollowUpReferences = new HashSet<DocumentHeadExtend>();
            DocumentHeadExtendRequesterReferences = new HashSet<DocumentHeadExtend>();
            DocumentHeadsCreditAccountReferences = new HashSet<DocumentHeads>();
            DocumentHeadsDebitAccountReferences = new HashSet<DocumentHeads>();
            FinancialRequestDetailsCreditAccountReferences = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsDebitAccountReferences = new HashSet<FinancialRequestDetails>();
            FinancialRequestPartialCreditReferences = new HashSet<FinancialRequestPartial>();
            FinancialRequestPartialDebitReferences = new HashSet<FinancialRequestPartial>();
            MoadianInvoiceHeaders = new HashSet<MoadianInvoiceHeaders>();
            Persons = new HashSet<Persons>();
            VouchersDetailReferenceId1Navigation = new HashSet<VouchersDetail>();
            VouchersDetailReferenceId2Navigation = new HashSet<VouchersDetail>();
            VouchersDetailReferenceId3Navigation = new HashSet<VouchersDetail>();
            Payables_DocumentsAccounts = new HashSet<Payables_DocumentsAccounts>();
        }


        /// <summary>
        //شناسه
        /// </summary>


        /// <summary>
        //کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//فعال
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        public string? DepositId { get; set; }

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


        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsAccountReferences { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsOwnerChequeReferences { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsReceiveChequeReferences { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtendCorroborantReferences { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtendFollowUpReferences { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtendRequesterReferences { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsCreditAccountReferences { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsDebitAccountReferences { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsCreditAccountReferences { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsIssueReferenceBank { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsDebitAccountReferences { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialCreditReferences { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialDebitReferences { get; set; } = default!;
        public virtual ICollection<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; } = default!;
        public virtual ICollection<Persons> Persons { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId1Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId2Navigation { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetailReferenceId3Navigation { get; set; } = default!;
        public virtual ICollection<Payables_DocumentsAccounts> Payables_DocumentsAccounts { get; set; }

    }
}
