using Eefa.Accounting.Domain.Entities;
using System.Collections.Generic;

public partial class Role : AuditableEntity
{
    public int? ParentId { get; set; }
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string? Description { get; set; }


    public virtual Role? Parent { get; set; } = default!;
    public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
    public virtual ICollection<AccountHead> AccountHeads { get; set; } = default!;
    public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCode { get; set; } = default!;
    public virtual ICollection<AccountReference> AccountReferences { get; set; } = default!;
    public virtual ICollection<AccountReferencesGroup> AccountReferencesGroups { get; set; } = default!;
    public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
    public virtual ICollection<Attachment> Attachments { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
    public virtual ICollection<BaseValueType> BaseValueTypes { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
    public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroups { get; set; } = default!;
    public virtual ICollection<CountryDivision> CountryDivisions { get; set; } = default!;
    public virtual ICollection<Role> InverseParent { get; set; } = default!;
    public virtual ICollection<Permission> Permissions { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
    public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
    public virtual ICollection<Person> Persons { get; set; } = default!;
    public virtual ICollection<Position> Positions { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissions { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissionOwnerRoles { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissionRoles { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoleOwnerRoles { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoleRoles { get; set; } = default!;
    public virtual ICollection<UserSetting> UserSettings { get; set; } = default!;
    public virtual ICollection<UserYear> UserYears { get; set; } = default!;
    public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
    public virtual ICollection<Year> Years { get; set; } = default!;
}

