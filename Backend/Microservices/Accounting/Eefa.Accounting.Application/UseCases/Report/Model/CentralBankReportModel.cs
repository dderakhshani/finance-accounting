using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class CentralBankReportModel
    {
        public string? Code { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public double? RemainDebit { get; set; }
        public double? RemainCredit { get; set; }

        public int PersonType { get; set; }
        public string Name { get; set; }
        public string NationalNumber { get; set; }
        public string EconomicCode { get; set; }
        public string Address  { get; set; }
        public string Phone { get; set; }
    }
}
