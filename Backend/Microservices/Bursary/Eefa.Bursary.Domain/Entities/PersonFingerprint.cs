using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1575;&#1579;&#1585; &#1575;&#1606;&#1711;&#1588;&#1578;
    /// </summary>
    public partial class PersonFingerprint : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//شماره انگشت
        /// </summary>
        public int FingerBaseId { get; set; } = default!;

        /// <summary>
//الگوی اثر انگشت
        /// </summary>
        public string FingerPrintTemplate { get; set; } = default!;

        /// <summary>
//عکس اثر انگشت
        /// </summary>
        public string? FingerPrintPhotoURL { get; set; }

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
         

        public virtual BaseValues FingerBase { get; set; } = default!;
    }
}
