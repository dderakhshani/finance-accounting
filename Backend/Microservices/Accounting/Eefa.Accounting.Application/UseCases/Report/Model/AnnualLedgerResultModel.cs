using System.Collections.Generic;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class AnnualLedgerResultModel
    {
        public AnnualLedgerResultModel(double beforeDebit, double beforeCredit, double sumDebit, double sumCredit, List<AnnualLedgerData> datas)
        {
            BeforeDebit = beforeDebit;
            BeforeCredit = beforeCredit;
            SumDebit = sumDebit;
            SumCredit = sumCredit;
            Datas = datas;
        }
        public double BeforeDebit { get; set; }
        public double BeforeCredit { get; set; }
        public double SumDebit { get; set; }
        public double SumCredit { get; set; }
        public List<AnnualLedgerData> Datas { get; set; }
    }
    public class AnnualLedgerData
    {
        public int Id { get; set; }
        public int? Level2 { get; set; }
        public string Level2Code { get; set; }
        public string Level2Title { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
    }
}
