using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1608;&#1593; &#1575;&#1591;&#1604;&#1575;&#1593;&#1575;&#1578; &#1662;&#1575;&#1740;&#1607; 
    /// </summary>
    public partial class BaseValueTypes : BaseEntity
    {
        public BaseValueTypes()
        {
            InverseParent = new HashSet<BaseValueTypes>();
        }


        /// <summary>
//شناسه
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//نام گروه
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
//آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        /// <summary>
//زیر سیستم
        /// </summary>
        public string? SubSystem { get; set; }

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
        public virtual BaseValueTypes Parent { get; set; } = default!;
        public virtual ICollection<BaseValueTypes> InverseParent { get; set; } = default!;
    }
}
