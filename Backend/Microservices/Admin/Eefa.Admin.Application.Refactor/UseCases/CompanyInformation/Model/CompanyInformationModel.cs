using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class CompanyInformationModel : IMapFrom<CompanyInformation>
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public DateTime? ExpireDate { get; set; }
    public int MaxNumOfUsers { get; set; } = default!;
    public string? Logo { get; set; }
    public ICollection<int> YearsId { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CompanyInformation, CompanyInformationModel>()
            .ForMember(x => x.YearsId, opt => opt.MapFrom(x => x.Years.Select(x => x.Id)))
            ;
    }
}