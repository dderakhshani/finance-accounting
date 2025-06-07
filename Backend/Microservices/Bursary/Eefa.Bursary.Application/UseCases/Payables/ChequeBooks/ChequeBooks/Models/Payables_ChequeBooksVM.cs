using Eefa.Bursary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Models
{
    public class Payables_ChequeBooksVM
    {
        public int BankAccountId { get; set; }
        public string? BankAccountTitle { get; set; }
        public DateTime GetDate { get; set; }
        public string Serial { get; set; }
        public int SheetsCount { get; set; }
        public long StartNumber { get; set; }
        public List<Payables_ChequeBooksSheets> Payables_ChequeBooksSheets { get; set; }
    }
}
