using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1601;&#1585;&#1605;&#1608;&#1604; &#1587;&#1606;&#1583; &#1575;&#1578;&#1608; &#1605;&#1575;&#1578;&#1740;&#1705;
    /// </summary>
    public partial class AutoVoucherFormula : BaseEntity
    {
         

        /// <summary>
//کد نوع سند
        /// </summary>
        public int VoucherTypeId { get; set; } = default!;

        /// <summary>
//ترتیب آرتیکل سند 
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
//کد نوع سند
        /// </summary>
        public int SourceVoucherTypeId { get; set; } = default!;

        /// <summary>
//وضعیت مانده حساب debit 1 credit 2
        /// </summary>
        public byte DebitCreditStatus { get; set; } = default!;

        /// <summary>
//سرفصل حساب
        /// </summary>
        public int AccountHeadId { get; set; } = default!;

        /// <summary>
//توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        /// <summary>
//ستونهای مقصد
        /// </summary>
        public string? Formula { get; set; }

        /// <summary>
//شرط
        /// </summary>
        public string? Conditions { get; set; }

        /// <summary>
//گروهبندی
        /// </summary>
        public string? GroupBy { get; set; }

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
         

        public virtual AccountHead AccountHead { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroups VoucherType { get; set; } = default!;
    }
}
