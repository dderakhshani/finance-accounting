using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CountryDivisionsMain : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد استان
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
//کد بخش
        /// </summary>
        public string? Bakhsh { get; set; }

        /// <summary>
//نام بخش
        /// </summary>
        public string? BakhshTitle { get; set; }
        public string? ShrDeh { get; set; }
        public string? ShrDehTitle { get; set; }
        public string? Blkabd { get; set; }
        public string? CodeRec { get; set; }
        public string? Name { get; set; }
        public string? Diag { get; set; }

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
         
    }
}
