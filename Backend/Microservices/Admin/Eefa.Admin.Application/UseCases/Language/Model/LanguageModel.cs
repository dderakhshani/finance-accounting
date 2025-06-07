using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Language.Model
{
    public class LanguageModel : IMapFrom<Data.Databases.Entities.Language>
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نماد
        /// </summary>
        public string Culture { get; set; } = default!;

        /// <summary>
        /// کد سئو
        /// </summary>
        public string? SeoCode { get; set; }

        /// <summary>
        /// نماد پرچم کشور
        /// </summary>
        public string? FlagImageUrl { get; set; }

        /// <summary>
        /// راست چین
        /// </summary>
        public int DirectionBaseId { get; set; } = default!;
        public string DirectionBaseTitle { get; set; } = default!;

        /// <summary>
        /// واحد پول پیش فرض
        /// </summary>
        public int DefaultCurrencyBaseId { get; set; } = default!;
        public string DefaultCurrencyBaseTitle { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Language, LanguageModel>()
                .ForMember(x=>x.DirectionBaseTitle,opt=>opt.MapFrom(x=>x.DirectionBase.Title))
                .ForMember(x=>x.DefaultCurrencyBaseTitle,opt=>opt.MapFrom(x=>x.DefaultCurrencyBase.Title))
                ;
        }
    }

}
