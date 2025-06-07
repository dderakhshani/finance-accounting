using AutoMapper;
using Eefa.Common;
using System;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Queries.Bom
{
    public record BomValueHeaderModel : IMapFrom<Eefa.Commodity.Data.Entities.BomValueHeader>
    {
        public int Id { get; set; }
        /// <summary>
        /// کد فرمول ساخت
        /// </summary>
        public int BomId { get; set; } = default!;
        public string BomTitle { get; set; }
        /// <summary>
        /// کد کالا
        /// </summary>
        public int CommodityId { get; set; } = default!;
        public string CommodityTitle { get; set; }

        public string CommodityCode { get; set; }
       
        public string Title { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// تاریخ فرمول ساخت
        /// </summary>
        public DateTime BomDate { get; set; } = default!;

        public List<BomValueModel> Values { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.BomValueHeader, BomValueHeaderModel>().ForMember(o => o.Title, opt => opt.MapFrom(src => src.Commodity.Title + "🔅" + src.Commodity.Code + "🔅" + src.Bom.Title + "🔅" + src.Name));

        }
    }

}
