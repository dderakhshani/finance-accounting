using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CodeVoucherExtendType : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

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
         

        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
