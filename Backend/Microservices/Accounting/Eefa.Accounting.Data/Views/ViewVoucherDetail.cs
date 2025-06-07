using System;

namespace Eefa.Accounting.Data.Views
{
    public class ViewVoucherDetail
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public int AccountHeadId { get; set; }
        public string VoucherRowDescription { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public int? RowIndex { get; set; }
        public int? DocumentId { get; set; }
        public int? ReferenceId1 { get; set; }
        public string ReferenceCode_1 { get; set; }
        public string ReferenceTitle_1 { get; set; }
        public string AccountHeadTitle { get; set; }
        public string AccountHeadCode { get; set; }
        public int YearId { get; set; }
        public DateTime VoucherDate { get; set; }
        public int VoucherStateId { get; set; }
        public int CodeVoucherGroupId { get; set; }
        public int VoucherNo { get; set; }
        public string VoucherDescription { get; set; }
        public string VoucherStateName { get; set; }
        public string Level1Code { get; set; }
        public string Level1Title { get; set; }
        public string Level2Code { get; set; }
        public string Level2Title { get; set; }
        public string Level3Code { get; set; }
        public string Level3Title { get; set; }
        public int? Level1 { get; set; }
        public int? Level2 { get; set; }
        public int? Level3 { get; set; }
        public int? ReferenceId2 { get; set; }
        public int? ReferenceId3 { get; set; }
        public int VoucherRowId { get; set; }
        public byte? DebitCreditStatus { get; set; }
        public int CompanyId { get; set; }
        public int YearName { get; set; }
        public string CompanyName { get; set; }
        public int Diagnosis { get; set; }
        public string PersianDate { get; set; }
        public string CodeVoucherGroupTitle { get; set; }
        public int? ReferenceGroupId { get; set; }
        public int? TraceNumber { get; set; }
        public int? AccountReferencesGroupId { get; set; }
        public string PersianVoucherDate { get; set; }
        public string DocumentNo { get; set; }
        public int? CurrencyTypeBaseId { get; set; }
        public double? CurrencyFee { get; set; }
        public double? CurrencyAmount { get; set; }
        public string InvoiceNo { get; set; }
        public int? ChequeSheetId { get; set; }
        public double? DebitCurrencyAmount { get; set; }
        public double? CreditCurrencyAmount { get; set; }
        public int? PermissionId { get; set; }
        public DateTime? ReferenceDate { get; set; }
    }
}
