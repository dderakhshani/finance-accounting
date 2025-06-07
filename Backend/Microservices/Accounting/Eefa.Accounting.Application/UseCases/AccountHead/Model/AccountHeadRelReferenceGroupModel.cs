using AutoMapper;
using Library.Mappings;

namespace Eefa.Accounting.Application.UseCases.AccountHeadRelReferenceGroup.Model
{
    public class AccountHeadRelReferenceGroupModel : IMapFrom<Data.Entities.AccountHeadRelReferenceGroup>
    {
        public int Id { get; set; }
        public int AccountHeadId { get; set; } = default!;
        public int ReferenceGroupId { get; set; } = default!;
        public int ReferenceNo { get; set; } = default!;
        public bool IsDebit { get; set; } = default!;
        public bool IsCredit { get; set; } = default!;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.AccountHeadRelReferenceGroup, AccountHeadRelReferenceGroupModel>().IgnoreAllNonExisting();
        }
    }

}
