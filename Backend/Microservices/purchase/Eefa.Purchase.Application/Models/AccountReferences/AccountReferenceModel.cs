using Eefa.Common;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Application.Models.AccountReferences
{
    public class AccountReferenceModel : IMapFrom<AccountReference>
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public string Title { get; set; } = default!;
        public int AccountReferenceGroupId { get; set; }
    }
}
