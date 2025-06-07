using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1585;&#1578;&#1576;&#1575;&#1591; &#1570;&#1585;&#1578;&#1740;&#1705;&#1604; &#1587;&#1606;&#1583;&#1607;&#1575;&#1740; &#1605;&#1705;&#1575;&#1606;&#1740;&#1586;&#1607; 
    /// </summary>
    public partial class AutoVoucherRowsLink : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         
        public int? VoucherId { get; set; }
        public int? VoucherTypeId { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int? RowId { get; set; }

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
