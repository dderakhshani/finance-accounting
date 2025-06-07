using System;

public class StpReportReference2AccountHeadResult
{
    public int Id { get; set; }
    public int YearId { get; set; }
    public string AccountHeadCode { get; set; }
    public string AccountHeadTitle { get; set; }
    public string ReferenceCode { get; set; }
    public string ReferenceTitle { get; set; }
    public string VoucherNo { get; set; }
    public DateTime VoucherDate { get; set; }
    public string VoucherDescription { get; set; }
    public string VoucherStateName { get; set; }
    public string VoucherGroupTitle { get; set; }
    public string PersianVoucherDate { get; set; }
    public string VoucherRowDecription { get; set; }
    public int RowIndex { get; set; }
    public long? Credit { get; set; }
    public long? Debit { get; set; }
}