using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1608;&#1575;&#1583; &#1601;&#1585;&#1605;&#1608;&#1604; &#1587;&#1575;&#1582;&#1578;
    /// </summary>
    public partial class BomValueHeaders : BaseEntity
    {
        public BomValueHeaders()
        {
            BomValues = new HashSet<BomValues>();
            DocumentItemsBoms = new HashSet<DocumentItemsBom>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد فرمول ساخت
        /// </summary>
        public int BomId { get; set; } = default!;
        public string? Name { get; set; }

        /// <summary>
//کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;

        /// <summary>
//تاریخ فرمول ساخت
        /// </summary>
        public DateTime BomDate { get; set; } = default!;

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
         

        public virtual Boms Bom { get; set; } = default!;
        public virtual Commodities Commodity { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<BomValues> BomValues { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBoms { get; set; } = default!;
    }
}
