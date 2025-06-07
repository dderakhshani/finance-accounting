using AutoMapper;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Bom
{

    public record BomItemModel : IMapFrom<Eefa.Commodity.Data.Entities.BomItem>
    {
        public int Id { get; set; }
        public int BomId { get; set; } = default!;
        public int? SubCategoryId { get; set; } = default!;
        public int CommodityId { get; set; } = default!;
        public string CommodityCode { get; set; } = default!;
        public string CommodityTitle { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.BomItem, BomItemModel>().ForMember(o => o.CommodityTitle, opt => opt.MapFrom(src => src.Commodity.Title))
                                                                                   .ForMember(o => o.CommodityCode, opt => opt.MapFrom(src => src.Commodity.Code))
                                                                                   .IgnoreAllNonExisting();
        }
    }

}
