using System;
using System.Collections.Generic;
using AutoMapper;

public class VouchersHeadWithDetailModel : IMapFrom<VouchersHead>
{
    public int Id { get; set; }
    public int CompanyId { get; set; } = default!;
    public int VoucherDailyId { get; set; } = default!;
    public int YearId { get; set; } = default!;
    public int VoucherNo { get; set; } = default!;
    public DateTime VoucherDate { get; set; } = default!;
    public string VoucherDescription { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public int VoucherStateId { get; set; } = default!;
    public string? VoucherStateName { get; set; }
    public int? AutoVoucherEnterGroup { get; set; }
    public double? TotalDebit { get; set; }
    public double? TotalCredit { get; set; }
    public double? Difference { get; set; }
    public string Creator { get; set; }
    public string Modifier { get; set; }
    public string CodeVoucherGroupTitle { get; set; }
    public ICollection<VouchersDetailModel> VouchersDetails { get; set; }
    public ICollection<VoucherAttachmentModel> VoucherAttachmentsList { get; set; }
    
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<VouchersHead, VouchersHeadWithDetailModel>()
       .ForMember(src => src.Creator, opt => opt.MapFrom(dest => dest.CreatedBy.Person.FirstName + " " + dest.CreatedBy.Person.LastName))
            .ForMember(src => src.Modifier, opt => opt.MapFrom(dest => dest.ModifiedBy.Person.FatherName + " " + dest.ModifiedBy.Person.LastName))
            .ForMember(x => x.CodeVoucherGroupTitle, opt => opt.MapFrom(x => x.CodeVoucherGroup.Title))
            .ForMember(src => src.VoucherAttachmentsList, opt => opt.MapFrom(dest => dest.VoucherAttachments))
            .ForMember(x => x.VouchersDetails, opt => opt.MapFrom(x => x.VouchersDetails));
    }
}