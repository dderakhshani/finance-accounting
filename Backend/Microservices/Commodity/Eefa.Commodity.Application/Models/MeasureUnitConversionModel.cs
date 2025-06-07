using AutoMapper;
using Eefa.Commodity.Data.Entities;
using Eefa.Common;

namespace Eefa.Commodity.Application.Queries.Measure
{

    public record MeasureUnitConversionModel:IMapFrom<MeasureUnitConversion>
    {
        public int Id { get; set; }
        /// <summary>
        /// واحد اندازه گیری اولیه
        /// </summary>
        public int SourceMeasureUnitId { get; set; } = default!;

        public string SourceMeasureTitle { get; set; } = default!;

        /// <summary>
        /// واحد اندازه گیری ثانویه
        /// </summary>
        public int DestinationMeasureUnitId { get; set; } = default!;
        public string DestinationMeasureTitle { get; set; } = default!;

        /// <summary>
        /// ضریب تبدیل
        /// </summary>
        public double? Ratio { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MeasureUnitConversion, MeasureUnitConversionModel>().IgnoreAllNonExisting()
                .ForMember(x=>x.SourceMeasureTitle,opt=>opt.MapFrom(src => src.SourceMeasureUnit.Title))
                .ForMember(x => x.DestinationMeasureTitle, opt => opt.MapFrom(src => src.DestinationMeasureUnit.Title))
               ;
        }

    }


}
