using AutoMapper;

public class AccountHeadRelReferenceGroupModel : IMapFrom<AccountHeadRelReferenceGroup>
{
    public int Id { get; set; }
    public int AccountHeadId { get; set; } = default!;
    public int ReferenceGroupId { get; set; } = default!;
    public int ReferenceNo { get; set; } = default!;
    public bool IsDebit { get; set; } = default!;
    public bool IsCredit { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccountHeadRelReferenceGroup, AccountHeadRelReferenceGroupModel>().IgnoreAllNonExisting();
    }
}