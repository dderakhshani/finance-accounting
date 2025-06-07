using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Models
{
    public class BankReceiptsModel
    {
        public int DocumentNo { get; set; }
        public int CodeVoucherGroupId { get; set; } = 2259;
        public DateTime DocumentDate { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; } = 1;
        public int YearId { get; set; } = 5;
        public int FinancialStatusBaseId { get; set; }
        public int PaymentStatus { get; set; }
        public bool IsEmergent { get; set; } = false;
        public bool IsAccumulativePayment { get; set; } = false;
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentTypeBaseId { get; set; } = 28509;
        public List<ReceiptModel> FinancialRequestDetails { get; set; }
    }
}
