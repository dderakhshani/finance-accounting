using AutoMapper;
using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Queries.Bom
{
    public record BomModel : IMapFrom<Eefa.Commodity.Data.Entities.Bom>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد والد
        /// </summary>
        public int? RootId { get; set; }
        public string? RootTitle { get; set; }
        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;
        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// کد گروه کالا
        /// </summary>
        public int CommodityCategoryId { get; set; } = default!;
        public string CommodityCategoryTitle { get; set; }

        public bool IsActive { get; set; }
        public List<BomItemModel> Items { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.Bom, BomModel>().IgnoreAllNonExisting();
            profile.CreateMap<Eefa.Commodity.Data.Entities.BomsView, BomModel>().IgnoreAllNonExisting();
        }
    }

}
