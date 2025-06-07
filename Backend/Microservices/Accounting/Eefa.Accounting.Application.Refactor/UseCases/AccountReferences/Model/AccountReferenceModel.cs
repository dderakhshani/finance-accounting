using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class AccountReferenceModel : IMapFrom<AccountReference>
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public bool? IsActive { get; set; } = default!;
    public string Status { get; set; } = default!;

    public string Code { get; set; }
    public string? Description { get; set; }

    public ICollection<int> AccountReferencesGroupsIdList { get; set; }
    public string NationalNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? PersonId { get; set; }
    public string? PersonPhotoUrl { get; set; }
    public string FullName { get; set; }


    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccountReference, AccountReferenceModel>()
            .ForMember(src => src.Status, opt => opt.MapFrom(dest => dest.IsActive ? "فعال" : "غیر فعال"))
            .ForMember(src => src.FirstName, opt => opt.MapFrom(dest => dest.Person.FirstName))
            .ForMember(src => src.FullName, opt => opt.MapFrom(dest => dest.Person.FirstName + " " + dest.Person.LastName))
            .ForMember(src => src.LastName, opt => opt.MapFrom(dest => dest.Person.LastName))
            .ForMember(src => src.PersonPhotoUrl, opt => opt.MapFrom(dest => dest.Person.PhotoURL ?? null))
            .ForMember(src => src.PersonId, opt => opt.MapFrom(dest => dest.Person.Id))
            .ForMember(src => src.NationalNumber, opt => opt.MapFrom(dest => dest.Person.NationalNumber))
            .ForMember(src => src.AccountReferencesGroupsIdList, opt => opt.MapFrom(dest => dest.AccountReferencesRelReferencesGroups.Select(t => t.ReferenceGroupId)));
    }
}