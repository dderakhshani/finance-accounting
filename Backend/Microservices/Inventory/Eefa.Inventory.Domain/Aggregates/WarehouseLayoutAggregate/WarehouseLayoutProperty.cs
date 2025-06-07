using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    public partial class WarehouseLayoutProperty : DomainBaseEntity
    {

        public int WarehouseLayoutId { get; set; } = default!;
        /// <description>
        /// کد ویژگی گروه
        ///</description>

        public int CategoryPropertyId { get; set; } = default!;
        /// <description>
        /// کد آیتم ویژگی گروه
        ///</description>

        public int? CategoryPropertyItemId { get; set; }
        /// <description>
        /// مقدار
        ///</description>

        public string Value { get; set; }

        public int WarehouseLayoutCategoryId { get; set; }

       
        

        public virtual CommodityCategoryProperty CategoryProperty { get; set; } = default!;
        public virtual CommodityCategoryPropertyItem CategoryPropertyItem { get; set; } = default!;
        public virtual WarehouseLayoutCategories WarehouseLayoutCategory { get; set; } = default!;
        public virtual WarehouseLayout WarehouseLayout { get; set; } = default!;
    }


}
