using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Library.Mappings;
using System;

namespace Eefa.Accounting.Application.UseCases.Report.Model
{
    public class LedgerReportModel : IMapFrom<Data.Entities.VouchersDetail>
    {
        public int Id { get; set; }
        public DateTime? VoucherDate { get; set; }
        public int? VoucherNo { get; set; }
        public int VoucherId { get; set; }
        public string AccountHeadCode { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public double? RemainDebit { get; set; }
        public double? RemainCredit { get; set; }
        public string Title { get; set; }
        public int? ReferenceId_1 { get; set; }
        public string? ReferenceCode_1 { get; set; }
        public string? ReferenceName_1 { get; set; }
        public string VoucherRowDescription { get; set; }
        public string? AccountReferenceGroupCode { get; set; }
        public string? AccountReferenceGroupTitle { get; set; }
        public int? DocumentId { get; set; }
        public int? TraceNumber { get; set; }
        public double? CurrencyFee { get; set; }
        public double? CurrencyCredit { get; set; }
        public double? CurrencyDebit { get; set; }
        public double? CurrencyRemain { get; set; }
        public double? CurrencyAmount { get; set; }
        public double? CustomerCurrencyDebit { get; set; }
        public double? CustomerCurrencyCredit { get; set; }
        public double? CustomerCurrencyAmount { get; set; }
        public string CurrencyTypeBaseTitle { get; set; }
        public double Remaining { get; set; }
        public string? DocumentNo { get; set; }
        public int? RowIndex { get; set; }
        public bool? TaxpayerFlag { get; set; }       
        public string AccountHeadLevel2Code { get; set; }
        public string AccountHeadLevel2Title { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Data.Entities.VouchersDetail, LedgerReportModel>()
                .ForMember(x => x.VoucherNo, opt => opt.MapFrom(x => x.Voucher.VoucherNo))
                .ForMember(x => x.DocumentNo, opt => opt.MapFrom(x => x.DocumentNo))
                .ForMember(x => x.VoucherDate, opt => opt.MapFrom(x => x.Voucher.VoucherDate))
                .ForMember(x => x.AccountHeadCode, opt => opt.MapFrom(x => x.AccountHead.Code))
                .ForMember(x => x.Title, opt => opt.MapFrom(x => x.AccountHead.Title))
                .ForMember(x => x.ReferenceId_1, opt => opt.MapFrom(x => x.ReferenceId1))
                .ForMember(x => x.ReferenceCode_1, opt => opt.MapFrom(x => x.ReferenceId1Navigation.Code))
                .ForMember(x => x.ReferenceName_1, opt => opt.MapFrom(x => x.ReferenceId1Navigation.Title))
                .ForMember(x => x.AccountReferenceGroupCode, opt => opt.MapFrom(x => x.AccountReferencesGroup.Code))
                .ForMember(x => x.AccountReferenceGroupTitle, opt => opt.MapFrom(x => x.AccountReferencesGroup.Title))
                .ForMember(x => x.CurrencyTypeBaseTitle, opt => opt.MapFrom(x => x.CurrencyBaseTypeBaseValue.Title))
                .ForMember(x => x.CurrencyCredit, opt => opt.MapFrom(x => x.Credit > 0 ? (x.CurrencyAmount ?? 0) : 0))
                .ForMember(x => x.CurrencyDebit, opt => opt.MapFrom(x => x.Debit > 0 ? (x.CurrencyAmount ?? 0) : 0))
                .ForMember(x => x.CustomerCurrencyCredit, opt => opt.MapFrom(x => x.Credit > 0 ? (x.CustomerCurrencyAmount ?? 0) : 0))
                .ForMember(x => x.CustomerCurrencyDebit, opt => opt.MapFrom(x => x.Debit > 0 ? (x.CustomerCurrencyAmount ?? 0) : 0))
                .ForMember(x => x.AccountHeadLevel2Code, opt => opt.MapFrom(x => x.AccountHead.Parent.Code))
                .ForMember(x => x.AccountHeadLevel2Title, opt => opt.MapFrom(x => x.AccountHead.Parent.Title));
        }
    }
}
