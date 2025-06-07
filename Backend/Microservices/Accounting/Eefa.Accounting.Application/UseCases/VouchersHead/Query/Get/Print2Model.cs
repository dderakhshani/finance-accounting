using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.Get
{
    public class Print2Model
    {

        public Print2Model(List<VocherResult> data, double debit, double credit, List<string> orders)
        {
            Datas = data;
            SumDebit = debit;
            SumCredit = credit;
            Orders = orders;
        }
        public List<string> Orders { get; set; }
        public List<VocherResult> Datas { get; set; }
        public double SumDebit { get; set; }
        public double SumCredit { get; set; }
    }

}
