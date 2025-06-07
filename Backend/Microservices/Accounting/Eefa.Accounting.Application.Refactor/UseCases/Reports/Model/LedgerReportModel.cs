using AutoMapper;
using System;

public class LedgerReportModel : IMapFrom<VouchersDetail>
{
    public int Id { get; set; }
    public DateTime? VoucherDate { get; set; }
    public int? VoucherNo { get; set; }
    public int VoucherId { get; set; }
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
    public int? DocumentId { get; set; }
    public int? TraceNumber { get; set; }
    public double? CurrencyFee { get; set; }
    public double? CurrencyAmount { get; set; }
    public string? CurrencyTypeBaseTitle { get; set; }
    public double Remaining { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VouchersDetail, LedgerReportModel>()
            .ForMember(x => x.VoucherNo, opt => opt.MapFrom(x => x.Voucher.VoucherNo))
            .ForMember(x => x.VoucherDate, opt => opt.MapFrom(x => x.Voucher.VoucherDate))
            .ForMember(x => x.AccountHeadCode, opt => opt.MapFrom(x => x.AccountHead.Code))
            .ForMember(x => x.Title, opt => opt.MapFrom(x => x.AccountHead.Title))
            .ForMember(x => x.ReferenceId_1, opt => opt.MapFrom(x => x.ReferenceId1))
            .ForMember(x => x.ReferenceCode_1, opt => opt.MapFrom(x => x.ReferenceId1Navigation.Code))
            .ForMember(x => x.ReferenceName_1, opt => opt.MapFrom(x => x.ReferenceId1Navigation.Title))
            .ForMember(x => x.CurrencyTypeBaseTitle, opt => opt.MapFrom(x => x.CurrencyBaseTypeBaseValue.Title));
    }
}