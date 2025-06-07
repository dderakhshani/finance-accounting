using System;

namespace Eefa.Bursary.Application.Models
{
    public class VoucherDetailModel
    {
        public int Id { get; set; }
        public int VoucherId { get; set; } = default!;
        public int AccountHeadId { get; set; } = default!;
        public string VoucherRowDescription { get; set; } = default!;
        public decimal Debit { get; set; } = default!;
        public decimal Credit { get; set; } = default!;
        public int? RowIndex { get; set; } = default!;
        public int? DocumentId { get; set; } = default!;

        public int? ReferenceId1 { get; set; } = default!;
        public string ReferenceCode_1 { get; set; } = default!;
        public string ReferenceTitle_1 { get; set; } = default!;
        public string AccountHeadTitle { get; set; } = default!;
        public string AccountHeadCode { get; set; } = default!;
        public int YearId { get; set; } = default!;
        public DateTime VoucherDate { get; set; } = default!;
        public int VoucherStateId { get; set; } = default!;
        public int CodeVoucherGroupId { get; set; } = default!;
        public int VoucherNo { get; set; } = default!;
        public string VoucherDescription { get; set; } = default!;
        public string VoucherStateName { get; set; } = default!;

        public string Level1Code { get; set; } = default!;
        public string Level1Title { get; set; } = default!;
        public string Level2Code { get; set; } = default!;
        public string Level2Title { get; set; } = default!;
        public string Level3Code { get; set; } = default!;
        public string Level3Title { get; set; } = default!;
        public int? Level1 { get; set; } = default!;
        public int? Level2 { get; set; } = default!;
        public int? Level3 { get; set; } = default!;
        public int? ReferenceId2 { get; set; } = default!;
        public int? ReferenceId3 { get; set; } = default!;

        public int VoucherRowId { get; set; } = default!;
        public int? DebitCreditStatus { get; set; } = default!;
        public int CompanyId { get; set; } = default!;
        public int YearName { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public int? Diagnosis { get; set; } = default!;
        public string PersianDate { get; set; } = default!;
        public string CodeVoucherGroupTitle { get; set; } = default!;
        public int? ReferenceGroupId { get; set; } = default!;
        public int? TraceNumber { get; set; } = default!;
        public int? AccountReferencesGroupId { get; set; } = default!;

        public string PersianVoucherDate { get; set; } = default!;
        public string DocumentNo { get; set; } = default!;
        public int? CurrencyTypeBaseId { get; set; } = default!;
        public decimal CurrencyFee { get; set; } = default!;
        public decimal CurrencyAmount { get; set; } = default!;
        public string InvoiceNo { get; set; } = default!;
        public int ChequeSheetId { get; set; } = default!;



    }
}
