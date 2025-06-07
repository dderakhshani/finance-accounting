using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.AccountReference
{
    public record AccountReferenceModel : IMapFrom<Domain.Entities.AccountReferences>
    {

        /// <summary>
        /// شناسه
        /// </summary>
        public int Id { get; set; } = default!;

        /// <summary>
        /// کد
        /// </summary>
        public string Code { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// فعال
        /// </summary>
        public bool IsActive { get; set; } = default!;

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
        public int OwnerRoleId { get; set; } = default!;

        /// <summary>
        /// ایجاد کننده
        /// </summary>
        public int CreatedById { get; set; } = default!;

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
        public DateTime CreatedAt { get; set; } = default!;

        /// <summary>
        /// اصلاح کننده
        /// </summary>
        public int? ModifiedById { get; set; }

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
        public DateTime ModifiedAt { get; set; } = default!;

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        public bool IsDeleted { get; set; } = default!;

        public List<int> AccountReferencesGroupsIdList { get; set; }
                         
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.AccountReferences, AccountReferenceModel>()

                .ForMember(item => item.AccountReferencesGroupsIdList, 
                    opt => opt.MapFrom(item => item.AccountReferencesRelReferencesGroups.Select(x => x.ReferenceGroupId)));

        }



    }
}
