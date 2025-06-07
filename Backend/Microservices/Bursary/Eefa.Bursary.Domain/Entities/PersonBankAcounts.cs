using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1581;&#1587;&#1575;&#1576; &#1607;&#1575;&#1740; &#1576;&#1575;&#1606;&#1705;&#1740; &#1588;&#1582;&#1589;
    /// </summary>
    public partial class PersonBankAcounts : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//کد بانک
        /// </summary>
        public int BankId { get; set; } = default!;

        /// <summary>
//کد شعبه
        /// </summary>
        public int? BankBranchId { get; set; }

        /// <summary>
//نوع حساب
        /// </summary>
        public int TypeBaseId { get; set; } = default!;

        /// <summary>
//شماره حساب
        /// </summary>
        public string? AccountNumber { get; set; }

        /// <summary>
//شماره کارت
        /// </summary>
        public string? CardNumber { get; set; }

        /// <summary>
//شماره شبا
        /// </summary>
        public string? IbanNumber { get; set; }

        /// <summary>
//نام کامل شخص صاحب حساب
        /// </summary>
        public string AccountOwnerFullName { get; set; } = default!;

        /// <summary>
//نوع ارتباط با شخص صاحب حساب 
        /// </summary>
        public int AccountOwnerRelativeTypeBaseId { get; set; } = default!;

        /// <summary>
//فعال
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
//پیش فرض
        /// </summary>
        public bool IsDefault { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Descriptions { get; set; }

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
         

        public virtual BaseValues AccountOwnerRelativeTypeBase { get; set; } = default!;
        public virtual Banks Bank { get; set; } = default!;
        public virtual BankBranches BankBranch { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
