using Eefa.Common.Data;
using System;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class OtherFrieght : BaseEntity
    {
     
        public string AccountNumber { get; set; } = default!;
        public string? Description { get; set; }
        public long Amount { get; set; } = default!;
        public string? PersonName { get; set; }
        public string? ShebaNumber { get; set; }
        public string? PlateNumber { get; set; }
        public int UserId { get; set; } = default!;
         
        public DateTime? DateTime { get; set; }
        public bool? IsNewOtherFreight { get; set; }

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
         
    }
}
