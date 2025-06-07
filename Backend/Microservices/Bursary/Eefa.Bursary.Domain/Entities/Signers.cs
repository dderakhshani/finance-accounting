using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1605;&#1590;&#1575; &#1705;&#1606;&#1606;&#1583;&#1711;&#1575;&#1606;
    /// </summary>
    public partial class Signers : BaseEntity
    {

        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//عنوان امضاء کننده 
        /// </summary>
        public string SignerDescription { get; set; } = default!;

        /// <summary>
//چندمین امضاء کننده
        /// </summary>
        public int SignerOrderIndex { get; set; } = default!;

        /// <summary>
//فعال
        /// </summary>
        public bool? IsActive { get; set; } = default!;

        /// <summary>
//تاریخ فعال شدن
        /// </summary>
        public DateTime ActiveDate { get; set; } = default!;

        /// <summary>
//تاریخ غیر فعال شدن
        /// </summary>
        public DateTime? ExpireDate { get; set; }

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
    }
}
