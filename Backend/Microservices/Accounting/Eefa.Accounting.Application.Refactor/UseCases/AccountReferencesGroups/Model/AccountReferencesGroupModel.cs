using AutoMapper;
using System;

public class AccountReferencesGroupModel : IMapFrom<AccountReferencesGroup>
{
    public int Id { get; set; }
    public int CompanyId { get; set; } = default!;
    public int? ParentId { get; set; }

    [UniqueIndex]
    public string LevelCode { get; set; }

    [UniqueIndex]
    public string Title { get; set; } = default!;
    public bool? IsEditable { get; set; } = default!;
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<AccountReferencesGroup, AccountReferencesGroupModel>().IgnoreAllNonExisting();
    }
}