using Eefa.Accounting.Domain.Entities;
using System;
using System.Collections.Generic;

public partial class User : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public bool IsBlocked { get; set; } = default!;
    public int? BlockedReasonBaseId { get; set; }
    public string? OneTimePassword { get; set; }
    public string Password { get; set; } = default!;
    public int FailedCount { get; set; } = default!;
    public DateTime? LastOnlineTime { get; set; }

    public virtual BaseValue? BlockedReasonBase { get; set; } = default!;
    public virtual Person Person { get; set; } = default!;


    public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCodeCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCodeModifiedBies { get; set; } = default!;


    public virtual ICollection<AccountHead> AccountHeadCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountHead> AccountHeadModifiedBies { get; set; } = default!;
    public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<AccountReference> AccountReferenceCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountReference> AccountReferenceModifiedBies { get; set; } = default!;
    public virtual ICollection<AccountReferencesGroup> AccountReferencesGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountReferencesGroup> AccountReferencesGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<Attachment> AttachmentCreatedBies { get; set; } = default!;
    public virtual ICollection<Attachment> AttachmentModifiedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulaCreatedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulaModifiedBies { get; set; } = default!;
    public virtual ICollection<BaseValue> BaseValueCreatedBies { get; set; } = default!;
    public virtual ICollection<BaseValue> BaseValueModifiedBies { get; set; } = default!;
    public virtual ICollection<BaseValueType> BaseValueTypeCreatedBies { get; set; } = default!;
    public virtual ICollection<BaseValueType> BaseValueTypeModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptionCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptionModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypeCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypeModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<CompanyInformation> CompanyInformationCreatedBies { get; set; } = default!;
    public virtual ICollection<CompanyInformation> CompanyInformationModifiedBies { get; set; } = default!;
    public virtual ICollection<CountryDivision> CountryDivisionCreatedBies { get; set; } = default!;
    public virtual ICollection<CountryDivision> CountryDivisionModifiedBies { get; set; } = default!;
    public virtual ICollection<Permission> PermissionCreatedBies { get; set; } = default!;
    public virtual ICollection<Permission> PermissionModifiedBies { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddressCreatedBies { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddressModifiedBies { get; set; } = default!;
    public virtual ICollection<Person> PersonCreatedBies { get; set; } = default!;
    public virtual ICollection<PersonFingerprint> PersonFingerprintCreatedBies { get; set; } = default!;
    public virtual ICollection<PersonFingerprint> PersonFingerprintModifiedBies { get; set; } = default!;
    public virtual ICollection<Person> PersonModifiedBies { get; set; } = default!;
    public virtual ICollection<Position> PositionCreatedBies { get; set; } = default!;
    public virtual ICollection<Position> PositionModifiedBies { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionCreatedBies { get; set; } = default!;
    public virtual ICollection<RequiredPermission> RequiredPermissionModifiedBies { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissionCreatedBies { get; set; } = default!;
    public virtual ICollection<RolePermission> RolePermissionModifiedBies { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoleCreatedBies { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoleModifiedBies { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoleUsers { get; set; } = default!;
    public virtual ICollection<UserSetting> UserSettingCreatedBies { get; set; } = default!;
    public virtual ICollection<UserSetting> UserSettingModifiedBies { get; set; } = default!;
    public virtual ICollection<UserSetting> UserSettingUsers { get; set; } = default!;
    public virtual ICollection<UserYear> UserYearCreatedBies { get; set; } = default!;
    public virtual ICollection<UserYear> UserYearModifiedBies { get; set; } = default!;
    public virtual ICollection<UserYear> UserYearUsers { get; set; } = default!;
    public virtual ICollection<VoucherAttachment> VoucherAttachmentCreatedBies { get; set; } = default!;
    public virtual ICollection<VoucherAttachment> VoucherAttachmentModifiedBies { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetailCreatedBies { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetailModifiedBies { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeadCreatedBies { get; set; } = default!;
    public virtual ICollection<VouchersHead> VouchersHeadModifiedBies { get; set; } = default!;
    public virtual ICollection<Year> YearCreatedBies { get; set; } = default!;
    public virtual ICollection<Year> YearModifiedBies { get; set; } = default!;
    public virtual ICollection<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
}

