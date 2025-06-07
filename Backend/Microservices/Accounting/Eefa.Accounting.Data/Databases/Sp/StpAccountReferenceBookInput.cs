using System;

namespace Eefa.Accounting.Data.Databases.Sp
{
    public class StpAccountReferenceBookInput
    {
        public int ReportType { get; set; }
        public int Level { get; set; }
        public int CompanyId { get; set; }
        public string? YearIds { get; set; }
        public int? VoucherStateId { get; set; }
        public int? CodeVoucherGroupId { get; set; }
        public int? TransferId { get; set; }
        public string? AccountHeadIds { get; set; }
        public string? ReferenceGroupIds { get; set; }
        public string? ReferenceIds { get; set; }
        public int? ReferenceNo { get; set; }
        public int? VoucherNoFrom { get; set; }
        public int? VoucherNoTo { get; set; }
        public DateTime? VoucherDateFrom { get; set; }
        public DateTime? VoucherDateTo { get; set; }
        public long? DebitFrom { get; set; }
        public long? DebitTo { get; set; }
        public long? CreditFrom { get; set; }
        public long? CreditTo { get; set; }
        public int? DocumentIdFrom { get; set; }
        public int? DocumentIdTo { get; set; }
        public string? VoucherDescription { get; set; }
        public string? VoucherRowDescription { get; set; }
        public bool Remain { get; set; }
    }
}