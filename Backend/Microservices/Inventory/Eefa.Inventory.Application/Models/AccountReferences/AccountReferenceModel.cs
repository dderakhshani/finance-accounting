using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class AccountReferenceModel : IMapFrom<AccountReference>
    {
        
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public string Title { get; set; } = default!;
        public int AccountReferenceGroupId { get; set; }

    }
}
