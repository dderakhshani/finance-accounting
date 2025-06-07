using System;
using AutoMapper;

public class YearModel : IMapFrom<Year>, IMapFrom<UserYear>
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
        profile.CreateMap<Year, YearModel>();

        profile.CreateMap<UserYear, YearModel>()
            .ForMember(src => src.YearName, opt => opt.MapFrom(dest => dest.Year.YearName))
            .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.Year.Id));
    }
}