using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1578;&#1594;&#1740;&#1740;&#1585;&#1575;&#1578; &#1587;&#1606;&#1583;&#1607;&#1575;&#1740; &#1605;&#1705;&#1575;&#1606;&#1740;&#1586;&#1607; 
    /// </summary>
    public partial class AutoVoucherLog : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//تاریخ فعالیت
        /// </summary>
        public DateTime? ActionDate { get; set; }

        /// <summary>
//کد نوع سند
        /// </summary>
        public int? VoucherTypeId { get; set; }

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
//توضیحات سطر
        /// </summary>
        public string? RowDescription { get; set; }

        /// <summary>
//کد نهایی
        /// </summary>
        public byte? ResultId { get; set; }

        /// <summary>
//نام نهایی
        /// </summary>
        public string? ResultName { get; set; }

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
         

        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual CodeVoucherGroups VoucherType { get; set; } = default!;
    }
}
