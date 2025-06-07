using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public class WarhousteCategoryItemsModel
    {
        public int? CommodityCategoryId { get; set; } = default!;
        public int WarehouseLayoutId { get; set; } = default!;
        public int? CategoryPropertyId { get; set; } = default!;
        public int? CategoryPropertyItemId { get; set; } = default!;
        public string ValueItem { get; set; } = default!;
        public int? WarehouseLayoutCategoriesId { get; set; } = default!;
        public int? WarehouseLayoutPropertiesId { get; set; } = default!;
        
        public List<WarhousteCategoryItemsModel> Items { get; set; }
    }
}
