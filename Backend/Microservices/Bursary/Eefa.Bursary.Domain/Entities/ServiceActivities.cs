using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class ServiceActivities : BaseEntity
    {
         
        public string Url { get; set; } = default!;
        public string Parameters { get; set; } = default!;

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
         

        public virtual Activities IdNavigation { get; set; } = default!;
    }
}
