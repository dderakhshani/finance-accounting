using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class TadbirReportModel
    {
        public int VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string VoucherDescription { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountRefrenceCode { get; set; }
        public string CostCenterCode { get; set; } = "1";
        public string ProjectCode { get; set; } = "1";
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string VoucherDetailDescription { get; set; }
    }
}
