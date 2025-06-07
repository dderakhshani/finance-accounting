using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Application.Models.CommodityCategory;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    public class WarehouseLayoutPropertyModel : IMapFrom<WarehouseLayoutProperty>
    {
        public int Id { get; set; }
        public int WarehouseLayoutId { get; set; } = default!;
        /// <description>
        /// کد ویژگی گروه
        ///</description>

        public int CategoryPropertyId { get; set; } = default!;
        /// <summary>
        /// عنوان
        /// </summary>
        public string CategoryPropertyTitle { get; set; } = default!;

        /// <description>
        /// کد آیتم ویژگی گروه
        ///</description>

        public int? CategoryPropertyItemId { get; set; } = default!;
        /// <summary>
        /// عنوان
        /// </summary>
        public string CategoryPropertyItemTitle { get; set; } = default!;
        
         public int? WarehouseLayoutCategoryId { get; set; }

        public string Value { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CommodityCategoryPropertyModel CategoryProperty { get; set; } = default!;
        public virtual CommodityCategoryPropertyItemModel CategoryPropertyItem { get; set; } = default!;
        public virtual WarehouseLayoutModel WarehouseLayout { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<WarehouseLayoutProperty, WarehouseLayoutPropertyModel>()
            .ForMember(x => x.CategoryPropertyTitle, opt => opt.MapFrom(t => t.CategoryProperty.Title))
            .ForMember(x => x.CategoryPropertyItemTitle, opt => opt.MapFrom(t => t.CategoryPropertyItem.Title)) ;
           
        }
    }

   
 

}
