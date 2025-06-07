using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Models
{
    public class AccountBalanceModel
    {

        public string TransactionType { get; set; }

 
        public string OperationCode { get; set; }
 
        public string OrigKey { get; set; }

 
        public string TransactionUniqueId { get; set; }

 
        public long? TransactionAmount { get; set; }

 
        public decimal TransactionAmountDebit { get; set; }

 
        public decimal TransactionAmountCredit { get; set; }

        public long? TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public long? EffectiveDate { get; set; }



        public string DocNumber { get; set; }


        public string Description { get; set; }

        public string TransactionRow { get; set; }


        public string balance { get; set; }


        public string PayId1 { get; set; }

 
        public string PayId2 { get; set; }

    
        public string CodeDigit { get; set; }

  
        public string BranchCode { get; set; }

  
        public string BankName { get; set; }


        public string persianDateAndTime { get; set; }

        public DateTime dateTime { get; set; }

        public string transactionTypeTitle { get; set; }

 
        public string? accountReferenceTitle { get; set; }
        public int? voucherHeadId { get; set; }
        public int? voucherNo { get; set; }
        public int? bursaryNo { get; set; }
        public int? financialRequestId { get; set; }
     



        public void init()
        {
            persianDateAndTime = TransactionTime.Insert(2, ":").Insert(5, ":") + " " +
                                 TransactionDate.ToString().Insert(4, "/").Insert(7, "/");

            var persianCalendar = new PersianCalendar();
            var year = int.Parse(TransactionDate.ToString().Substring(0, 4));
            var month = int.Parse(TransactionDate.ToString().Substring(4, 2));
            var day = int.Parse(TransactionDate.ToString().Substring(6, 2));

            var hour = int.Parse(TransactionTime.Substring(0, 2));
            var minute = int.Parse(TransactionTime.Substring(2, 2));
            var second = int.Parse(TransactionTime.Substring(4, 2));

            // ساخت تاریخ میلادی با استفاده از PersianCalendar
            dateTime = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);


            balance = balance.Replace(",", "");
            var ff = long.Parse(balance);
            balance = ff.ToString("N0");

            transactionTypeTitle = TransactionType == "C" ? "واریز" : "برداشت";


        }




    }
}
