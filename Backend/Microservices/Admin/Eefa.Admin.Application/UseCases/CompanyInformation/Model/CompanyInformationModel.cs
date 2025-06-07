using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Library.Mappings;

namespace Eefa.Admin.Application.CommandQueries.CompanyInformation.Model
{
    public class CompanyInformationModel : IMapFrom<Data.Databases.Entities.CompanyInformation>
    {
        public int Id { get; set; }
        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// تاریخ انقضاء
        /// </summary>
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// حداکثر تعداد کابران
        /// </summary>
        public int MaxNumOfUsers { get; set; } = default!;

        /// <summary>
        /// لوگو
        /// </summary>
        public string? Logo { get; set; }
        public ICollection<int> YearsId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Databases.Entities.CompanyInformation, CompanyInformationModel>()
                .ForMember(x=>x.YearsId,opt=>opt.MapFrom(x=>x.Years.Select(x=>x.Id)))
                ;
        }
    }

}
