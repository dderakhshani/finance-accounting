using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Commodity.Data.Entities
{
    public partial class BaseValueType : BaseEntity
    {

        /// <summary>
        /// کد والد
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
        /// نام اختصاصی
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

        public virtual BaseValueType? Parent { get; set; } = default!;
        public virtual ICollection<BaseValueType> InverseParent { get; set; } = default!;
    }
}
