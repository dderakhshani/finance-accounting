using System;

namespace Eefa.Accounting.Data.Databases.Sp
{
    public class StpAccountReferenceBookResult
    {
		public int Id { get; set; }
		public int YearId { get; set; }
		public string? AccountHeadCode { get; set; }
		public string? AccountHeadTitle { get; set; }
		public string? ReferenceCode { get; set; }
		public string? ReferenceTitle { get; set; }
		public int? VoucherNo { get; set; }
		public int? DocumentId { get; set; }
		public DateTime VoucherDate { get; set; }
		public string? VoucherDescription { get; set; }
		public string? VoucherStateName { get; set; }
		public string? VoucherGroupTitle { get; set; }
		public string? PersianVoucherDate { get; set; }
		public string? VoucherRowDescription { get; set; }
		public long? Debit { get; set; }
		public long? Credit { get; set; }
		public int? RowIndex { get; set; }
    }
}
