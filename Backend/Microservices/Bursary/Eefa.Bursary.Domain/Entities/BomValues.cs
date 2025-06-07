using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1602;&#1575;&#1583;&#1740;&#1585; &#1601;&#1585;&#1605;&#1608;&#1604; &#1587;&#1575;&#1582;&#1578;
    /// </summary>
    public partial class BomValues : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد سند فرمول ساخت
        /// </summary>
        public int BomValueHeaderId { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;

        /// <summary>
//کد کالای مصرفی
        /// </summary>
        public int UsedCommodityId { get; set; } = default!;

        /// <summary>
//مقدار
        /// </summary>
        public double Value { get; set; } = default!;

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
         

        public virtual BomValueHeaders BomValueHeader { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual Commodities UsedCommodity { get; set; } = default!;

    }
}
