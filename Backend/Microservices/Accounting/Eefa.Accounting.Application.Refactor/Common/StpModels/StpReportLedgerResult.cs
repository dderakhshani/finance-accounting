using System;

public class StpReportLedgerResult
{
    public long Id { get; set; }
    public DateTime? VoucherDate { get; set; }
    public int? VoucherNo { get; set; }
    public string? AccountHeadCode { get; set; }
    public double? Debit { get; set; }
    public double? Credit { get; set; }
    public double? RemainDebit { get; set; }
    public double? RemainCredit { get; set; }
    public string? Title { get; set; }
    public int? ReferenceId_1 { get; set; }
    public string? ReferenceCode_1 { get; set; }
    public string? ReferenceName_1 { get; set; }
    public string? VoucherRowDescription { get; set; }
    public string? PersianVoucherDate { get; set; }
    public int? DocumentId { get; set; }
    public string? DocumentNo { get; set; }
    public int? TraceNumber { get; set; }
    public string? InvoiceNo { get; set; }
    public double? CurrencyFee { get; set; }
    public double? CurrencyAmount { get; set; }
    public int? CurrencyTypeBaseId { get; set; }
}