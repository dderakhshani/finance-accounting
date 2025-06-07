using Library.Exceptions;
using System.Collections.Generic;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class ProfitAndLossReportResult
    {
        public ProfitAndLossReportResult(string code, string name, double price)
        {
            Code = code;
            Name = name;
            Price = price;

        }
        public ProfitAndLossReportResult()
        {

        }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
