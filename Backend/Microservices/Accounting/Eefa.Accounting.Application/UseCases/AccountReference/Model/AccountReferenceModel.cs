using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Model
{
    public class AccountReferenceModel : IMapFrom<Data.Entities.AccountReference>
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


        public int? PersonalGroupId { get; set; }
        public List<int>? PersonalGroupAccountHeadIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string EmployeeCode { get; set; }
        public string? DepositId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.AccountReference, AccountReferenceModel>()
                .ForMember(src => src.EmployeeCode, opt => opt.MapFrom(dest => dest.Person.Employee.EmployeeCode))
                .ForMember(src => src.Status, opt => opt.MapFrom(dest => dest.IsActive ? "فعال" : "غیر فعال"))
                .ForMember(src => src.FirstName, opt => opt.MapFrom(dest => dest.Person.FirstName))
                .ForMember(src => src.FullName, opt => opt.MapFrom(dest => dest.Person.FirstName + " " + dest.Person.LastName))
                .ForMember(src => src.LastName, opt => opt.MapFrom(dest => dest.Person.LastName))
                .ForMember(src => src.PersonPhotoUrl, opt => opt.MapFrom(dest => dest.Person.PhotoURL ?? null))
                .ForMember(src => src.PersonId, opt => opt.MapFrom(dest => dest.Person.Id))
                .ForMember(src => src.DepositId, opt => opt.MapFrom(dest => dest.DepositId))
                .ForMember(src => src.NationalNumber, opt => opt.MapFrom(dest => dest.Person.NationalNumber))
                .ForMember(src => src.PersonalGroupId, opt => opt.MapFrom(dest => dest.AccountReferencesRelReferencesGroups.First(x => x.ReferenceGroup.IsVisible == false).ReferenceGroupId))
                .ForMember(src => src.PersonalGroupAccountHeadIds, opt => opt.MapFrom(dest => dest.AccountReferencesRelReferencesGroups.First(x => x.ReferenceGroup.IsVisible == false).ReferenceGroup.AccountHeadRelReferenceGroups.Select(x => x.AccountHeadId).ToList()))
                .ForMember(src => src.AccountReferencesGroupsIdList, opt => opt.MapFrom(dest => dest.AccountReferencesRelReferencesGroups.Select(t => t.ReferenceGroupId)));
        }
    }

  
}
