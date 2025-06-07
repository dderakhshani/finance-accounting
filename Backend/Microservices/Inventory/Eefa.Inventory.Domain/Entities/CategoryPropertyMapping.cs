using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    /// <summary>
    /// نگاشت بین گروه ها و مشخصات کالا
    /// </summary>
    public partial class CategoryPropertyMapping: DomainBaseEntity
    {
        public int CommodityCategoryPropertyItems1 { get; set; } = default!;
    /// <description>
            /// آیتم ویژگی گروه محصول2
    ///</description>
    
        public int CommodityCategoryPropertyItems2 { get; set; } = default!;
    /// <description>
            /// نقش صاحب سند
    ///</description>
    
    public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems1Navigation { get; set; } = default!;
    public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems2Navigation { get; set; } = default!;
    }
}
