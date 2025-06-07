using System.Collections.Generic;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class BaseValueType : BaseEntity
    {

        /// <summary>
        /// کد
        /// </summary>
         
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// نام گروه
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

        /// <summary>
        /// زیر سیستم
        /// </summary>
        public string? SubSystem { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValueType? Parent { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValues { get; set; } = default!;
        public virtual ICollection<BaseValueType> InverseParent { get; set; } = default!;
    }
}
