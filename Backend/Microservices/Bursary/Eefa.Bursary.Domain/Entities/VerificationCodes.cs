using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class VerificationCodes : BaseEntity
    {
        public VerificationCodes()
        {
            MoadianInvoiceHeaders = new HashSet<MoadianInvoiceHeaders>();
        }

         
        public string Code { get; set; } = default!;
        public DateTime ExpiryDate { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsUsed { get; set; } = default!;
         

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
         

        public virtual ICollection<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; } = default!;
    }
}
