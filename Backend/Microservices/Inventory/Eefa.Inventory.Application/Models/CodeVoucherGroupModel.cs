using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class ReceiptALLStatusModel : IMapFrom<CodeVoucherGroup>
    {
        public int Id { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; }
        public int? ViewId { get; set; } = default!;
        public int? DefultDebitAccountHeadId { get; set; } = default!;
        public int? DefultCreditAccountHeadId { get; set; } = default!;


    }
}
