using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

public class UserModel : IMapFrom<User>
{
    public int Id { get; set; }
    public int PersonId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public bool IsBlocked { get; set; } = default!;
    public int? BlockedReasonBaseId { get; set; }
    public string? BlockedReason { get; set; }
    public string? OneTimePassword { get; set; }
    public DateTime PasswordExpiryDate { get; set; }
    public int FailedCount { get; set; } = default!;
    public IList<int>? RolesIdList { get; set; }
    public string? UnitPositionTitle { get; set; }
    public DateTime? LastOnlineTime { get; set; }
    public string? RoleTitle { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalNumber { get; set; }
    public string FullName { get; set; }
    public ICollection<int> UserYears { get; set; }
    public ICollection<int> UserCompanies { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserModel>()
            .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName))
            .ForMember(x => x.BlockedReason, opt => opt.MapFrom(x => x.BlockedReasonBase.Title))
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.Person.FirstName))
            .ForMember(x => x.NationalNumber, opt => opt.MapFrom(x => x.Person.NationalNumber))
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.Person.LastName))
            .ForMember(x => x.RoleTitle, opt => opt.MapFrom(x => x.UserRoleUsers.FirstOrDefault().Role.Title))
            .ForMember(x => x.RolesIdList, opt => opt.MapFrom(x => x.UserRoleUsers.Select(x => x.RoleId)))
            .ForMember(x => x.UnitPositionTitle, opt => opt.MapFrom(x => x.Person.Employee.UnitPosition.Unit.Title + "-" + x.Person.Employee.UnitPosition.Position.Title))
            .ForMember(x => x.UserYears, opt => opt.MapFrom(x => x.UserYearUsers.Select(t => t.YearId)))
            .ForMember(x => x.UserCompanies, opt => opt.MapFrom(x => x.UserYearUsers.Select(t => t.Year.CompanyId)));
    }
}