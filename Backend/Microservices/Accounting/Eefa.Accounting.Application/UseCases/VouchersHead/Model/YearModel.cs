using System;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Model
{
    public class YearModel : IMapFrom<Data.Entities.Year>, IMapFrom<Data.Entities.UserYear>
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.Year, YearModel>()

                ;

            profile.CreateMap<Data.Entities.UserYear, YearModel>()
                .ForMember(src => src.YearName, opt => opt.MapFrom(dest => dest.Year.YearName))
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.Year.Id));

        }
    }

}
