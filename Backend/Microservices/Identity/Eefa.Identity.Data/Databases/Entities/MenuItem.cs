using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class MenuItem : BaseEntity
    {
        public int? ParentId { get; set; }

        /// <summary>
        /// کد دسترسی
        /// </summary>
        public int? PermissionId { get; set; }

        /// <summary>
        /// عنوان منو
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// لینک تصویر
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// لینک فرم
        /// </summary>
        public string? FormUrl { get; set; }
        public string? HelpUrl { get; set; }

        /// <summary>
        /// عنوان صفحه
        /// </summary>
        public string? PageCaption { get; set; }

        /// <summary>
        /// فعال است؟
        /// </summary>
        public bool IsActive { get; set; } = default!;
        public int OrderIndex { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Permission? Permission { get; set; } = default!;
    }
}
