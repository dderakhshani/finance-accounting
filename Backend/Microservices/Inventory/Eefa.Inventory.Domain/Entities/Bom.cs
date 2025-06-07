using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// فرمول های ساخت
    /// </summary>
    public partial class Bom: DomainBaseEntity
    {
        public int? RootId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
    /// <description>
            /// کد سطح
    ///</description>
    
        public string LevelCode { get; set; } = default!;
    /// <description>
            /// کد گروه کالا
    ///</description>
    
        public int CommodityCategoryId { get; set; } = default!;
    
   
    
   
    }
}
