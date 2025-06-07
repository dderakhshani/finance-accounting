using System;
using System.Collections.Generic;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Query.Get
{
    public class ResultModel
    {
        public ResultModel(List<string> debitorders, List<VocherResult> debitdata, List<string> creditorders, List<VocherResult> creditdata, double debit, double credit)
        {
            DebitDatas = debitdata;
            DebitOrders = debitorders;
            CreditDatas = creditdata;
            CreditOrders = creditorders;
            SumDebit = debit;
            SumCredit = credit;
        }
        public List<string> DebitOrders { get; set; }
        public List<VocherResult> DebitDatas { get; set; }
        public List<string> CreditOrders { get; set; }
        public List<VocherResult> CreditDatas { get; set; }
        public double SumDebit { get; set; }
        public double SumCredit { get; set; }
    }
    public class VocherResult
    {
        public int? RowIndex { get; set; }
        public int VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public int VoucherDailyId { get; set; }
        public string CodeVocherGroupName { get; set; }
        public string VoucherDescription { get; set; }
        public string Level2AccountHead { get; set; }
        public string AccountHeadCode { get; set; }
        public string AccountReferenceCode { get; set; }
        public string VoucherRowDescription { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Description { get; set; }
    }

}
