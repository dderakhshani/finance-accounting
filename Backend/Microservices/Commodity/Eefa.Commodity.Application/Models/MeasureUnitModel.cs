using AutoMapper;
using Eefa.Commodity.Data.Entities;
using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Commodity.Application.Queries.Measure
{
    public record MeasureUnitModel : IMapFrom<Eefa.Commodity.Data.Entities.MeasureUnit>
    {

        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string? UniqueName { get; set; }

        public List<MeasureUnit>? Children { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Eefa.Commodity.Data.Entities.MeasureUnit, MeasureUnitModel>();
        }
    }


}
