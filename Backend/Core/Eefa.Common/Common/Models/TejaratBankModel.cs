using Eefa.Common.Common.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Common
{
    public partial class Welcome
    {
        [JsonProperty("soapenv:Envelope")]
        public SoapenvEnvelope SoapenvEnvelope { get; set; }
    }

    public partial class SoapenvEnvelope
    {
        [JsonProperty("@xmlns:soapenv")]
        public Uri XmlnsSoapenv { get; set; }

        [JsonProperty("@xmlns:soapenc")]
        public Uri XmlnsSoapenc { get; set; }

        [JsonProperty("@xmlns:xsd")]
        public Uri XmlnsXsd { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public Uri XmlnsXsi { get; set; }

        [JsonProperty("soapenv:Header")]
        public object SoapenvHeader { get; set; }

        [JsonProperty("soapenv:Body")]
        public SoapenvBody SoapenvBody { get; set; }
    }

    public partial class SoapenvBody
    {
        [JsonProperty("AccountHistoryResponse")]
        public AccountHistoryResponse AccountHistoryResponse { get; set; }
    }

    public partial class AccountHistoryResponse
    {
        [JsonProperty("@xmlns")]
        public Uri Xmlns { get; set; }

        [JsonProperty("transactionCount")]
        public long? TransactionCount { get; set; }

        [JsonProperty("statementBalance")]
        public string StatementBalance { get; set; }

        [JsonProperty("accountHistoryItems")]
        public AccountHistoryItems AccountHistoryItems { get; set; }
    }

    public partial class AccountHistoryItems
    {
        [JsonProperty("accountHistoryItem")]
        [JsonConverter(typeof(SingleOrArrayConverter<AccountHistoryItem>))]
        public List<AccountHistoryItem> AccountHistoryItem { get; set; }
    }

    public partial class AccountHistoryItem
    {

        [Display(Name = "عملیات")]
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        [Display(Name = "کدعملیات")]
        [JsonProperty("operationCode")]
        public string OperationCode { get; set; }


        [JsonProperty("origKey")]
        public string OrigKey { get; set; }

        [Display(Name = "شماره عملیات")]
        [JsonProperty("transactionUniqueId")]
        public string TransactionUniqueId { get; set; }

        [JsonProperty("transactionAmount")]
        public long? TransactionAmount { get; set; }

        [Display(Name = "بدهکار")]
        [JsonProperty("transactionAmountDebit")]
        public long? TransactionAmountDebit { get; set; }

        [Display(Name = "بستانکار")]
        [JsonProperty("transactionAmountCredit")]
        public long? TransactionAmountCredit { get; set; }

        [JsonProperty("transactionDate")]
        public long? TransactionDate { get; set; }

        [JsonProperty("transactionTime")]
        public string TransactionTime { get; set; }

        [JsonProperty("effectiveDate")]
        public long? EffectiveDate { get; set; }


        [Display(Name = "رهگیری")]
        [JsonProperty("docNumber")]
        public string DocNumber { get; set; }

        [Display(Name = "توضیحات")]
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("transactionRow")]
        public string TransactionRow { get; set; }

        [Display(Name = "مانده")]
        [JsonProperty("balance")]
        public string balance { get; set; }

        [Display(Name = "شناسه پرداخت")]
        [JsonProperty("payId1")]
        public string PayId1 { get; set; }

        [JsonProperty("payId2")]
        public string PayId2 { get; set; }

        [JsonProperty("codeDigit")]
        public string CodeDigit { get; set; }

        [Display(Name = "کدشعبه")]
        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }

        [Display(Name = "بانک")]
        [JsonProperty("BankName")]
        public string BankName { get; set; }

        [JsonProperty("persianDateAndTime")]
        [Display(Name = "تاریخ")]
        public string persianDateAndTime { get; set; }
        [JsonProperty("dateTime")]
        [Display(Name = "تاریخ میلادی")]
        public DateTime dateTime { get; set; }

        public string transactionTypeTitle { get; set; }

        [JsonProperty("accountReferenceTitle")]
        [Display(Name = "اسم")]
        public string? accountReferenceTitle { get; set; }
        public int? voucherHeadId { get; set; }
        public int? voucherNo { get; set; }
        public int? bursaryNo { get; set; }



        public void init()
        {
            // ترکیب زمان و تاریخ شمسی برای ساخت تاریخ میلادی
            persianDateAndTime = TransactionTime.Insert(2, ":").Insert(5, ":") + " " +
                                 TransactionDate.ToString().Insert(4, "/").Insert(7, "/");

            // تبدیل تاریخ شمسی و زمان به تاریخ میلادی
            var persianCalendar = new PersianCalendar();
            var year = int.Parse(TransactionDate.ToString().Substring(0, 4));
            var month = int.Parse(TransactionDate.ToString().Substring(4, 2));
            var day = int.Parse(TransactionDate.ToString().Substring(6, 2));

            var hour = int.Parse(TransactionTime.Substring(0, 2));
            var minute = int.Parse(TransactionTime.Substring(2, 2));
            var second = int.Parse(TransactionTime.Substring(4, 2));

            // ساخت تاریخ میلادی با استفاده از PersianCalendar
            dateTime = persianCalendar.ToDateTime(year, month, day, hour, minute, second, 0);

            // فرمت‌بندی موجودی
            var ff = long.Parse(balance);
            balance = ff.ToString("N0");

            transactionTypeTitle = TransactionType == "C" ? "واریز" : "برداشت";


        }

    }


    public partial class ResponseTejaratModel
    {
        public string Balance { get; set; }
        public long? TransactionCount { get; set; }
        public List<AccountHistoryItem> DetailItems { get; set; }
        public string filePath { get; set; }

    }

}
