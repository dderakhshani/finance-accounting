using System;

namespace Eefa.Accounting.Data.Databases.Sp
{
    public class StpReportLedgerInput : IReportSpBase
    {
        public int ReportType { get; set; }
        public int Level { get; set; }
        public int CompanyId { get; set; }
        public string? YearIds { get; set; }
        public int? VoucherStateId { get; set; }
        public string? CodeVoucherGroupIds { get; set; }
        public int? TransferId { get; set; }
        public string? AccountHeadIds { get; set; }
        public string? ReferenceGroupIds { get; set; }
        public string? ReferenceIds { get; set; }
        public int? ReferenceNo { get; set; }
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public double? DebitFrom { get; set; }
        public double? DebitTo { get; set; }
        public double? CreditFrom { get; set; }
        public double? CreditTo { get; set; }
        public int? DocumentIdFrom { get; set; }
        public int? DocumentIdTo { get; set; }
        public string? VoucherDescription { get; set; }
        public string? VoucherRowDescription { get; set; }
        public bool Remain { get; set; }
        public string? CompanyName { get; set; }
        public string? UserName { get; set; }
        public string? PersianDateFrom { get; set; }
        public string? PersianDateTo { get; set; }
        public string? YearName { get; set; }
        public string? ReportTitle { get; set; }
        public string? ReportTime { get; set; }
    }


}