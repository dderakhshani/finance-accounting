#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.User.Model
{
    public class UserModel : IMapFrom<Data.Databases.Entities.User>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// آیا قفل شده است؟
        /// </summary>
        public bool IsBlocked { get; set; } = default!;

        /// <summary>
        /// علت قفل شدن
        /// </summary>
        public int? BlockedReasonBaseId { get; set; }
        public string? BlockedReason { get; set; }

        /// <summary>
        /// رمز موقت
        /// </summary>
        public string? OneTimePassword { get; set; }

        public DateTime PasswordExpiryDate { get; set; }

        /// <summary>
        /// دفعات ورود ناموفق
        /// </summary>
        public int FailedCount { get; set; } = default!;

        public IList<int>? RolesIdList { get; set; } 
        /// <summary>
        /// آخرین زمان آنلاین بودن
        /// </summary>
        
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
            profile.CreateMap<Data.Databases.Entities.User, UserModel>()
                .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName))
                .ForMember(x=>x.BlockedReason,opt=>opt.MapFrom(x=>x.BlockedReasonBase.Title))
                .ForMember(x=>x.FirstName,opt=>opt.MapFrom(x=>x.Person.FirstName))
                .ForMember(x=>x.NationalNumber,opt=>opt.MapFrom(x=>x.Person.NationalNumber))
                .ForMember(x=>x.LastName,opt=>opt.MapFrom(x=>x.Person.LastName))
                .ForMember(x=>x.RoleTitle,opt=>opt.MapFrom(x=>x.UserRoleUsers.FirstOrDefault().Role.Title))
                .ForMember(x=>x.RolesIdList,opt=>opt.MapFrom(x=>x.UserRoleUsers.Select(x=>x.RoleId)))
                .ForMember(x=>x.UnitPositionTitle,opt=>opt.MapFrom(x=>x.Person.Employee.UnitPosition.Unit.Title + "-" + x.Person.Employee.UnitPosition.Position.Title))
                .ForMember(x=>x.UserYears,opt=>opt.MapFrom(x=>x.UserYearUsers.Select(t=>t.YearId)))
                .ForMember(x=>x.UserCompanies,opt=>opt.MapFrom(x=>x.UserYearUsers.Select(t=>t.Year.CompanyId)))
                ;
        }
    }

}
