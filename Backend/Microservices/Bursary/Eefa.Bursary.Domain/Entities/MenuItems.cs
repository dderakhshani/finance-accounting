using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1605;&#1606;&#1608;&#1607;&#1575; 
    /// </summary>
    public partial class MenuItems : BaseEntity
    {

        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد دسترسی
        /// </summary>
        public int? PermissionId { get; set; }

        /// <summary>
//عنوان منو
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//لینک تصویر
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
//لینک فرم
        /// </summary>
        public string? FormUrl { get; set; }
        public string? HelpUrl { get; set; }

        /// <summary>
//عنوان صفحه
        /// </summary>
        public string? PageCaption { get; set; }

        /// <summary>
//فعال 
        /// </summary>
        public bool IsActive { get; set; } = default!;
        public int? CodeVoucherGroupId { get; set; }
        public int OrderIndex { get; set; } = default!;

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
        public virtual Permissions Permission { get; set; } = default!;
    }
}
