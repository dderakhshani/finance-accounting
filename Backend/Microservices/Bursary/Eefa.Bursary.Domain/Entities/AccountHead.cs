using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1705;&#1583;&#1740;&#1606;&#1705; &#1587;&#1585;&#1601;&#1589;&#1604; &#1581;&#1587;&#1575;&#1576; &#1607;&#1575;
    /// </summary>
    public partial class AccountHead : BaseEntity
    {
        public AccountHead()
        {
            AccountHeadRelReferenceGroups = new HashSet<AccountHeadRelReferenceGroup>();
            AutoVoucherFormulas = new HashSet<AutoVoucherFormula>();
            DocumentHeadsCreditAccountHeads = new HashSet<DocumentHeads>();
            DocumentHeadsDebitAccountHeads = new HashSet<DocumentHeads>();
            FinancialRequestDetailsCreditAccountHeads = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsDebitAccountHeads = new HashSet<FinancialRequestDetails>();
            FinancialRequestPartialCreditAccountHeads = new HashSet<FinancialRequestPartial>();
            FinancialRequestPartialDebitAccountHeads = new HashSet<FinancialRequestPartial>();
            InverseParent = new HashSet<AccountHead>();
            VouchersDetails = new HashSet<VouchersDetail>();
            Payables_DocumentsAccounts = new HashSet<Payables_DocumentsAccounts>();
        }


        /// <summary>
        //شناسه
        /// </summary>

        public int? PermissionId { get; set; } = default!;

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//کد سطح سیستمی
        /// </summary>
        public int CodeLevel { get; set; } = default!;

        /// <summary>
//کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
//طول کد
        /// </summary>
        public int? CodeLength { get; set; }
        public bool? LastLevel { get; set; }

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;
        public string? Description { get; set; }

        /// <summary>
//نوع موازنه
        /// </summary>
        public int? BalanceId { get; set; }

        /// <summary>
//کنترل ماهیت حساب 
        /// </summary>
        public int BalanceBaseId { get; set; } = default!;

        /// <summary>
//ماهیت حساب
        /// </summary>
        public string? BalanceName { get; set; }

        /// <summary>
//وضعیت سند
        /// </summary>
        public int TransferId { get; set; } = default!;

        /// <summary>
//شرح وضعیت سند
        /// </summary>
        public string? TransferName { get; set; }

        /// <summary>
//کد گروه
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
//نوع ارز 
        /// </summary>
        public int CurrencyBaseTypeId { get; set; } = default!;

        /// <summary>
//ویژگی ارزی دارد
        /// </summary>
        public bool CurrencyFlag { get; set; } = default!;

        /// <summary>
//تسعیر پذیر است
        /// </summary>
        public bool ExchengeFlag { get; set; } = default!;

        /// <summary>
//ویژگی پیگیری دارد 
        /// </summary>
        public bool TraceFlag { get; set; } = default!;

        /// <summary>
//ویژگی تعداد دارد
        /// </summary>
        public bool QuantityFlag { get; set; } = default!;

        /// <summary>
//فعال
        /// </summary>
        public bool? IsActive { get; set; } = default!;

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
        public virtual AccountHead Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsCreditAccountHeads { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsDebitAccountHeads { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsCreditAccountHeads { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsDebitAccountHeads { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialCreditAccountHeads { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialDebitAccountHeads { get; set; } = default!;
        public virtual ICollection<AccountHead> InverseParent { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual ICollection<Payables_DocumentsAccounts> Payables_DocumentsAccounts { get; set; }

    }
}
