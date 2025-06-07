using AutoMapper;
using System;

public class CorrectionRequestModel : IMapFrom<CorrectionRequest>
{
    public int Id { get; set; } = default!;
    public string Status { get; set; } = default!;
    public int CodeVoucherGroupId { get; set; } = default!;
    public string CodeVoucherGroupTitle { get; set; } = default!;
    public int? AccessPermissionId { get; set; } = default!;
    public int? DocumentId { get; set; }
    public string OldData { get; set; } = default!;
    public int VerifierUserId { get; set; } = default!;
    public string? PayLoad { get; set; }
    public string ApiUrl { get; set; } = default!;
    public string? ViewUrl { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<CorrectionRequest, CorrectionRequestModel>()
            .ForMember(x => x.CodeVoucherGroupTitle, opt => opt.MapFrom(x => x.CodeVoucherGroup.Title));
    }
}