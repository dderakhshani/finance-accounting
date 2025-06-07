using System;
using AutoMapper;

public class VouchersDetailModel : IMapFrom<VouchersDetail>
{
    public int Id { get; set; }
    public int VoucherId { get; set; } = default!;
    public DateTime? VoucherDate { get; set; }
    public int AccountHeadId { get; set; } = default!;
    public string AccountHeadCode { get; set; }
    public string ReferenceCode1 { get; set; }
    public string ReferenceTitle1 { get; set; }
    public string ReferenceTitle2 { get; set; }
    public string ReferenceTitle3 { get; set; }
    public string ReferenceCode2 { get; set; }
    public string ReferenceCode3 { get; set; }
    public string VoucherRowDescription { get; set; } = default!;
    public double Debit { get; set; } = default!;
    public double Credit { get; set; } = default!;
    public int? RowIndex { get; set; }
    public int? DocumentId { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public double? Weight { get; set; }
    public int? ReferenceId1 { get; set; }
    public int? ReferenceId2 { get; set; }
    public int? ReferenceId3 { get; set; }
    public int? Level1 { get; set; }
    public string Level1Name { get; set; }
    public int? Level2 { get; set; }
    public int? Level3 { get; set; }
    public byte? DebitCreditStatus { get; set; }
    public double? Remain { get; set; }
    public int? CurrencyTypeBaseId { get; set; }
    public double? CurrencyFee { get; set; }
    public double? CurrencyAmount { get; set; }
    public int? TraceNumber { get; set; }
    public int? AccountReferencesGroupId { get; set; } = default!;
    public bool CurrencyFlag { get; set; } = default!;
    public bool ExchengeFlag { get; set; } = default!;
    public bool TraceFlag { get; set; } = default!;
    public bool QuantityFlag { get; set; } = default!;
    public int VoucherNo { get; set; }
    public int VoucherDailyId { get; set; }
    public double? Quantity { get; set; }
    public string? InvoiceNo { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<VouchersDetail, VouchersDetailModel>()
            .ForMember(src => src.ReferenceCode1, opt => opt.MapFrom(dest => dest.ReferenceId1Navigation.Code))
            .ForMember(src => src.ReferenceCode2, opt => opt.MapFrom(dest => dest.ReferenceId2Navigation.Code))
            .ForMember(src => src.ReferenceCode3, opt => opt.MapFrom(dest => dest.ReferenceId3Navigation.Code))
            .ForMember(src => src.ReferenceTitle1, opt => opt.MapFrom(dest => dest.ReferenceId1Navigation.Title))
            .ForMember(src => src.ReferenceTitle2, opt => opt.MapFrom(dest => dest.ReferenceId2Navigation.Title))
            .ForMember(src => src.ReferenceTitle3, opt => opt.MapFrom(dest => dest.ReferenceId3Navigation.Title))
            .ForMember(src => src.AccountHeadCode, opt => opt.MapFrom(dest => dest.AccountHead.Code))
            .ForMember(src => src.QuantityFlag, opt => opt.MapFrom(dest => dest.AccountHead.QuantityFlag))
            .ForMember(src => src.TraceFlag, opt => opt.MapFrom(dest => dest.AccountHead.TraceFlag))
            .ForMember(src => src.ExchengeFlag, opt => opt.MapFrom(dest => dest.AccountHead.ExchengeFlag))
            .ForMember(src => src.CurrencyFlag, opt => opt.MapFrom(dest => dest.AccountHead.CurrencyFlag))
            .ForMember(src => src.VoucherNo, opt => opt.MapFrom(dest => dest.Voucher.VoucherNo))
            .ForMember(src => src.VoucherDailyId, opt => opt.MapFrom(dest => dest.Voucher.VoucherDailyId));
    }
}