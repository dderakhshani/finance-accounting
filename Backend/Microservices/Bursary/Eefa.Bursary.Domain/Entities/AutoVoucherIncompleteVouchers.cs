using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1587;&#1606;&#1583;&#1607;&#1575;&#1740; &#1605;&#1705;&#1575;&#1606;&#1740;&#1586;&#1607; &#1705;&#1575;&#1605;&#1604; &#1606;&#1588;&#1583;&#1607; 
    /// </summary>
    public partial class AutoVoucherIncompleteVouchers : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد نوع سند
        /// </summary>
        public int? VoucherTypeId { get; set; }

        /// <summary>
//تاریخ سند
        /// </summary>
        public DateTime? VoucherDate { get; set; }

        /// <summary>
//کد سطر
        /// </summary>
        public int? RowId { get; set; }

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

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
         

        public virtual CodeVoucherGroups IdNavigation { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
