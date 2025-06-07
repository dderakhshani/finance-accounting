using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class BusinessRules : BaseEntity
    {
        public BusinessRules()
        {
            BusinessRuleConditions = new HashSet<BusinessRuleConditions>();
            Transitions = new HashSet<Transitions>();
        }

         
        public string RuleName { get; set; } = default!;

        /// <summary>
//1=And 2=OR
        /// </summary>
        public int CombinationType { get; set; } = default!;
        public bool Invert { get; set; } = default!;

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
         

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        public virtual ICollection<BusinessRuleConditions> BusinessRuleConditions { get; set; } = default!;
        public virtual ICollection<Transitions> Transitions { get; set; } = default!;
    }
}
