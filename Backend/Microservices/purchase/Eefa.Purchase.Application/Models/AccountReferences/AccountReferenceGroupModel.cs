using Eefa.Common;
using Eefa.Purchase.Domain.Entities.SqlView;

namespace Eefa.Purchase.Application.Models.AccountReferences
{
    public class AccountReferenceViewModel : IMapFrom<AccountReferenceView>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public string Title { get; set; } = default!;
        public string AccountReferencesGroupsCode { get; set; } = default!;
        public int AccountReferenceGroupId { get; set; } = default!;
        public string AccountReferenceGoupCode { get; set; } = default!;

    }
}
