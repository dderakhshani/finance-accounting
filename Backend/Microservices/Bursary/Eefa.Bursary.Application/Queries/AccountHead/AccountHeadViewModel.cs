using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.AccountHead
{
    public  class AccountHeadViewModel : IMapFrom<Domain.Entities.AccountHead>
    {
        public int Id { get; set; }

        public string Code { get; set; } = default!;
 
        public string Title { get; set; } = default!;

        public string? Description { get; set; }

        public string CodeTitle { get; set; }

        public List<int> AccountReferenceGroupsIds { get; set; }
                         

        public void Mapping(Profile profile)
        {
            profile.CreateMap< Domain.Entities.AccountHead, AccountHeadViewModel>()

                .ForMember(item => item.AccountReferenceGroupsIds, opt => opt.MapFrom(item => item.AccountHeadRelReferenceGroups.Select(x => x.ReferenceGroupId)));

        }

    }
}
