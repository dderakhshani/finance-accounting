using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1587;&#1606;&#1575;&#1583; &#1662;&#1585;&#1583;&#1575;&#1582;&#1578;&#1740;
    /// </summary>
    public partial class DocumentPayments : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سرفصل سند
        /// </summary>
        public int DocumentHeadId { get; set; } = default!;

        /// <summary>
//تاریخ سررسید
        /// </summary>
        public DateTime? PaymentDate { get; set; }

        /// <summary>
//تاریخ پرداخت
        /// </summary>
        public DateTime? PaiedDate { get; set; }

        /// <summary>
//پرداخت شده است
        /// </summary>
        public string? IsPaied { get; set; }

        /// <summary>
//نرخ شناور
        /// </summary>
        public long? LiquidationPrice { get; set; }

        /// <summary>
//موازنه
        /// </summary>
        public long? Balance { get; set; }

        /// <summary>
//شماره بارگیری
        /// </summary>
        public int? LadingNo { get; set; }

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
        public virtual DocumentHeads DocumentHead { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
