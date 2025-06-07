using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    [HasUniqueIndex]
    public partial class BaseValueType : HierarchicalBaseEntity
    {

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        [UniqueIndex]
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

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValueType? Parent { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValues { get; set; } = default!;
        public virtual ICollection<BaseValueType> InverseParent { get; set; } = default!;
    }
}
