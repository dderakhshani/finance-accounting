using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1578;&#1575;&#1740;&#1740;&#1583; &#1705;&#1606;&#1606;&#1583;&#1711;&#1575;&#1606; &#1583;&#1585;&#1582;&#1608;&#1575;&#1587;&#1578; &#1605;&#1575;&#1604;&#1740;
    /// </summary>
    public partial class FinancialRequestVerifiers : BaseEntity
    {
         
        public int FinancialRequestId { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DateTime { get; set; } = default!;

 
        public int VerifiersStatus { get; set; } = default!;
         

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
         

        public virtual Users CreatedBy { get; set; } = default!;
        public virtual FinancialRequest FinancialRequest { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
