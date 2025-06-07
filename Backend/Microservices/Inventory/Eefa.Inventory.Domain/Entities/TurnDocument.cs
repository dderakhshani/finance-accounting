using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// نوبت دهی
    /// </summary>
    public partial class TurnDocument: DomainBaseEntity
    {
        public int TurnId { get; set; } = default!;
    /// <description>
            /// کد سرفصل سند
    ///</description>
    
        public int DocumentHeadId { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual Turn Turn { get; set; } = default!;
    }
}
