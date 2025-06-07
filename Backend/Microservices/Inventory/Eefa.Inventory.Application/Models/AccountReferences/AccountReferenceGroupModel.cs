using AutoMapper;
using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class AccountReferenceViewModel : IMapFrom<AccountReferenceView>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccountHead, AccountReferenceViewModel>();
            profile.CreateMap<AccountReferenceView, AccountReferenceViewModel>();
            profile.CreateMap<AccountReferenceEmployeeView, AccountReferenceViewModel>();

        }
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public string Title { get; set; } = default!;
        public int AccountReferenceGroupId { get; set; } = default!;
        public string AccountReferenceGoupCode { get; set; } = default!;

        public bool? isActive { get; set; } = default!;

        public bool? LastLevel { get; set; } = default!;
        public string SearchTerm { get; set; } = default!;
        public string EmployeeCode { get; set; }

    }
}
