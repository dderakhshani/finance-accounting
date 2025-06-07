using System;
using System.Collections.Generic;
using AutoMapper;

public class AccountHeadModel : IMapFrom<AccountHead>
{
    public int Id { get; set; }
    public int CompanyId { get; set; } = default!;
    public string LevelCode { get; set; } = default!;
    public int CodeLevel { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string FullCode { get; set; } = default!;
    public int CodeLength { get; set; }
    public bool LastLevel { get; set; } = default!;
    public int? ParentId { get; set; }
    public string ParentTitle { get; set; }
    public string ParentCode { get; set; }
    public string Title { get; set; } = default!;
    public int? BalanceId { get; set; }
    public int BalanceBaseId { get; set; } = default!;
    public string? BalanceName { get; set; }
    public int TransferId { get; set; } = default!;
    public string? TransferName { get; set; }
    public int? GroupId { get; set; }
    public int CurrencyBaseTypeId { get; set; } = default!;
    public bool CurrencyFlag { get; set; } = default!;
    public bool ExchengeFlag { get; set; } = default!;
    public bool TraceFlag { get; set; } = default!;
    public bool QuantityFlag { get; set; } = default!;
    public bool? IsActive { get; set; } = default!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }

    public ICollection<AccountHead> AccountHeads { get; set; }
    public ICollection<AccountHeadRelReferenceGroupModel>? AccountHeadRelReferenceGroups { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccountHead, AccountHeadModel>()
            .ForMember(src => src.AccountHeadRelReferenceGroups, opt => opt.MapFrom(dest => dest.AccountHeadRelReferenceGroups))
            .ForMember(src => src.FullCode, opt => opt.MapFrom(dest => dest.Code))
            .ForMember(src => src.ParentTitle, opt => opt.MapFrom(x => x.Parent.Title))
            .ForMember(src => src.ParentCode, opt => opt.MapFrom(x => x.Parent.Code));
    }
}