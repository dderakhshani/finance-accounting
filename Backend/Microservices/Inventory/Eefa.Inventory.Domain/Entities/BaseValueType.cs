using System.Collections.Generic;
using Eefa.Common.Data;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// نوع اطلاعات پایه 
    /// </summary>
    public partial class BaseValueType: BaseEntity
    {
    
        public int? ParentId { get; set; }
    /// <description>
            /// کد سطح
    ///</description>
    
        public string LevelCode { get; set; } = default!;
    /// <description>
            /// عنوان
    ///</description>
    
        public string Title { get; set; } = default!;
    /// <description>
            /// نام اختصاصی
    ///</description>
    
        public string UniqueName { get; set; } = default!;
    /// <description>
            /// نام گروه
    ///</description>
    
        public string? GroupName { get; set; }
    /// <description>
            /// آیا فقط قابل خواندن است؟
    ///</description>
    
        public bool IsReadOnly { get; set; } = default!;
    /// <description>
            /// زیر سیستم
    ///</description>
    
        public string? SubSystem { get; set; }
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual BaseValueType? Parent { get; set; }
                public virtual ICollection<BaseValueType>
    InverseParent { get; set; } = default!;
    }
}
