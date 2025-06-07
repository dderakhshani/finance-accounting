using System;
using AutoMapper;

public class CodeVoucherGroupModel : IMapFrom<CodeVoucherGroup>
{
    public int Id { get; set; }
    public string Code { get; set; }
    public int CompanyId { get; set; }
    public string Title { get; set; }
    public string UniqueName { get; set; }
    public DateTime? LastEditableDate { get; set; }
    public bool IsAuto { get; set; }
    public bool IsEditable { get; set; }
    public bool? IsActive { get; set; }
    public bool AutoVoucherEnterGroup { get; set; }
    public string BlankDateFormula { get; set; }
    public int? ViewId { get; set; }
    public int? ExtendTypeId { get; set; }

    public string? ExtendTypeName { get; set; }
    public string? TableName { get; set; }
    public string? SchemaName { get; set; }
    public int? MenuId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CodeVoucherGroup, CodeVoucherGroupModel>();
    }
}