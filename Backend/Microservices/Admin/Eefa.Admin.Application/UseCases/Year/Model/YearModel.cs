using System;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Year.Model
{
    public class YearModel : IMapFrom<Data.Databases.Entities.Year>, IMapFrom<Data.Databases.Entities.UserYear>
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int YearName { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public bool? IsEditable { get; set; }
        public bool IsCalculable { get; set; }
        public DateTime? LastEditableDate { get; set; }
        public bool CreateWithOutStartVoucher { get; set; } = default!;
        public bool IsCurrentYear { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Year, YearModel>()
                
                ;

            profile.CreateMap<Data.Databases.Entities.UserYear, YearModel>()
                .ForMember(src => src.YearName, opt => opt.MapFrom(dest => dest.Year.YearName))
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.Year.Id));

        }
    }

}
