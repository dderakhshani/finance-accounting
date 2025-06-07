using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class TadbirReportResultModel
    {
        public string TadbirExcelResult { get; set; }
        public int ErrorCount { get; set; }
    }
}
