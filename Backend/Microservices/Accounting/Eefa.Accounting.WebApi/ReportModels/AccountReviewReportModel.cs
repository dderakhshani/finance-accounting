using System;
using System.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Eefa.Accounting.WebApi.ReportModels
{
    public class AccountReviewReportModel
    {
        public AccountReviewReportModel(DateTime fromDate, DateTime toDate, string level, string voucherNoFrom, string voucherNoTo, Image logoAddress, string filter)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Level = level;
            VoucherNoFrom = voucherNoFrom;
            VoucherNoTo = voucherNoTo;
            LogoAddress = logoAddress;
            Filters = filter;

        }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Level { get; set; }
        public string VoucherNoFrom { get; set; }
        public string VoucherNoTo { get; set; }
        public Image LogoAddress { get; set; }
        public string Filters { get; set; }

    }
}
