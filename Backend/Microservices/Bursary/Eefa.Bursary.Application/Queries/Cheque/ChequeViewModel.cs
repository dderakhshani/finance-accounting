using System;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common;


namespace Eefa.Bursary.Application.Queries.Cheque
{
    public record ChequeModel : IMapFrom<PayCheque>
    {
        public int Id { get; set; }
        public string Sheba { get; private set; } = default!;
        public int BankBranchId { get; private set; } = default!;
        public string AccountNumber { get; private set; } = default!;
        public int SheetsCount { get; private set; } = default!;
        public string? ChequeNumberIdentification { get; private set; }
        public int? OwnerEmployeeId { get; private set; }
        public DateTime? SetOwnerTime { get; private set; }
        public bool? IsFinished { get; private set; }
        public int? BankAccountId { get; private set; }

     
    }



}
