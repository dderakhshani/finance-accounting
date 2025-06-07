using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CodeAutoVoucherViews : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد شرکت
        /// </summary>
        public int CompanyId { get; set; } = default!;

        /// <summary>
//مشاهده نام
        /// </summary>
        public string ViewName { get; set; } = default!;

        /// <summary>
//مشاهده عنوان
        /// </summary>
        public string ViewCaption { get; set; } = default!;

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
         

        public virtual CompanyInformations Company { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual Users ModifiedBy { get; set; } = default!;
        public virtual Roles OwnerRole { get; set; } = default!;
    }
}
