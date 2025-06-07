using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1588;&#1607;&#1585; &#1608; &#1575;&#1587;&#1578;&#1575;&#1606;
    /// </summary>
    public partial class CountryDivisions : BaseEntity
    {
        public CountryDivisions()
        {
            BankBranches = new HashSet<BankBranches>();
            PersonAddresses = new HashSet<PersonAddress>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
// کد استان
        /// </summary>
        public string? Ostan { get; set; }

        /// <summary>
//نام استان
        /// </summary>
        public string? OstanTitle { get; set; }

        /// <summary>
//کد شهرستان
        /// </summary>
        public string? Shahrestan { get; set; }

        /// <summary>
//نام شهرستان
        /// </summary>
        public string? ShahrestanTitle { get; set; }

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
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
        public virtual ICollection<BankBranches> BankBranches { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
    }
}
