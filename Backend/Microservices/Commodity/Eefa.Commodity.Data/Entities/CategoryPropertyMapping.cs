using Eefa.Common.Data;

namespace Eefa.Commodity.Data.Entities
{
    public partial class CategoryPropertyMapping : BaseEntity
    {

        /// <summary>
        /// آیتم ویژگی گروه محصول1
        /// </summary>
        public int CommodityCategoryPropertyItems1 { get; set; } = default!;

        /// <summary>
        /// آیتم ویژگی گروه محصول2
        /// </summary>
        public int CommodityCategoryPropertyItems2 { get; set; } = default!;

        public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems1Navigation { get; set; } = default!;
        public virtual CommodityCategoryPropertyItem CommodityCategoryPropertyItems2Navigation { get; set; } = default!;
    }
}
