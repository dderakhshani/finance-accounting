using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class WarehouseLayoutCategoryModel : IMapFrom<WarehouseLayoutCategories>
    {
        public int? Id { get; set; }
        public int? WarehouseLayoutId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public bool IsDeleted { get; set; }
        public virtual CommodityCategoryModel CommodityCategory { get; set; } = default!;
        public virtual WarehouseLayoutModel WarehouseLayout { get; set; } = default!;
        public void Mapping(Profile profile)
        {

            profile.CreateMap<WarehouseLayoutCategories, WarehouseLayoutCategoryModel>()
                    .ForMember(x => x.CategoryTitle, opt => opt.MapFrom(t => t.Category.Title));
                    

        }
    }
}
