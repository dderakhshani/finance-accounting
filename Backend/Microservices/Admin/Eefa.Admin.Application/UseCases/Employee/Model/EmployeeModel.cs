using System;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.Employee.Model
{
    public class EmployeeModel : IMapFrom<Data.Databases.Entities.Employee>
    {
        public int Id { get; set; }

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// کد موقعیت واحد
        /// </summary>
        public int? UnitPositionId { get; set; } = default!;

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public string EmployeeCode { get; set; } = default!;

        /// <summary>
        /// تاریخ استخدام
        /// </summary>
        public DateTime? EmploymentDate { get; set; } = default!;

        /// <summary>
        /// شناور
        /// </summary>
        public bool Floating { get; set; } = default!;

        /// <summary>
        /// تاریخ ترک کار
        /// </summary>
        public DateTime? LeaveDate { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalNumber { get; set; }
        public int? BranchId { get; set; }

        public int? UnitId { get; set; }
        public string? AccountReferenceCode { get; set; }
        public string LegalTitle { get; set; }
        public string GovernmentalTitle { get; set; }
        public string FullName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.Employee, EmployeeModel>().IgnoreAllNonExisting()
                .ForMember(src => src.NationalNumber, opt => opt.MapFrom(dest => dest.Person.NationalNumber))
                .ForMember(src => src.FirstName, opt => opt.MapFrom(dest => dest.Person.FirstName))
                .ForMember(src => src.LastName, opt => opt.MapFrom(dest => dest.Person.LastName))
                .ForMember(src => src.FullName, opt => opt.MapFrom(t => t.Person.FirstName + " " + t.Person.LastName))
                .ForMember(src => src.UnitId, opt => opt.MapFrom(dest => dest.UnitPosition.UnitId))
                .ForMember(src => src.BranchId, opt => opt.MapFrom(dest => dest.UnitPosition.Unit.BranchId))
                .ForMember(src => src.AccountReferenceCode, opt => opt.MapFrom(dest => dest.Person.AccountReference.Code))
                .ForMember(src => src.LegalTitle, opt => opt.MapFrom(dest => dest.Person.LegalBase.Title))
                .ForMember(src => src.GovernmentalTitle, opt => opt.MapFrom(dest => dest.Person.GovernmentalBase.Title))
                ;
        }
    }
}
