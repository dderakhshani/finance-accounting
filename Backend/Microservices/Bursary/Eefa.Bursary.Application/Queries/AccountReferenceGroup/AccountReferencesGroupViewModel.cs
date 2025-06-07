using System.Collections.Generic;
using AutoMapper;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;

namespace Eefa.Bursary.Application.Queries.AccountReferenceGroup
{
    public class AccountReferencesGroupViewModel:IMapFrom<AccountReferencesGroups>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
         
        public string LevelCode { get; set; } = default!;
         
        public string Title { get; set; } = default!;

        public int AccountHeadId { get; set; }

          public List<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountReferencesGroups, AccountReferencesGroupViewModel>();
        }

    }
}
