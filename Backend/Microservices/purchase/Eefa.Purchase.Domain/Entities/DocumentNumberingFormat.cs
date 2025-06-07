using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Entities
{
    /// <summary>
    /// شماره گذاری اسناد
    /// </summary>
    public partial class DocumentNumberingFormat: DomainBaseEntity
    {
    
        public int DocumentTypeBaseId { get; set; } = default!;
    /// <description>
            /// عنوان
    ///</description>
    
        public string Title { get; set; } = default!;
    /// <description>
            /// فرمت کد دهی
    ///</description>
    
        public string CodingTemplate { get; set; } = default!;
    /// <description>
            /// فعال
    ///</description>
    
        public bool IsActive { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual BaseValue DocumentTypeBase { get; set; } = default!;
    }
}
