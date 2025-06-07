using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Application.Models
{
    public class QueryModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? YearId { get; set; }
        public int? DebitAccountReferenceId { get; set; }
        public int? CreditAccountReferenceId { get; set; }
        public List<int> FinancialRequestIds { get; set; }



    }
}
