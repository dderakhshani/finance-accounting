using System.Collections.Generic;
using AutoMapper;
using Eefa.Inventory.Domain.Common;

namespace Eefa.Inventory.Application
{

    public record WarehouseLayoutGetIdModel : WarehouseLayoutModel
    {

        public virtual ICollection<WarehouseLayoutPropertyModel> WarehouseLayoutProperties { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutCategoryModel> WarehouseLayoutCategories { get; set; } = default!;
        public virtual ICollection<WarehouseLayoutQuantityModel> WarehouseLayoutQuantities { get; set; } = default!;
        public List<WarhousteCategoryItemsModel> Items { get; set; }
        public override void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.WarehouseLayout, WarehouseLayoutGetIdModel>()
           .ForMember(x => x.EntryModeTitle, opt => opt.MapFrom(t => (WarehouseEntryMode)(int)(t.EntryMode)))
           .ForMember(a => a.WarehouseLayoutProperties, x => x.MapFrom(a => a.WarehouseLayoutProperties))
           .ForMember(a => a.WarehouseLayoutCategories, x => x.MapFrom(a => a.WarehouseLayoutCategories))
           ;


        }

    }

    
    
    

}
