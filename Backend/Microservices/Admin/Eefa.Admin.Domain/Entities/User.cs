using System;
using System.Collections.Generic;

[HasUniqueIndex]
public partial class User : AuditableEntity
{
    public int PersonId { get; set; } = default!;
    [UniqueIndex]
    public string Username { get; set; } = default!;
    public bool IsBlocked { get; set; } = default!;
    public int? BlockedReasonBaseId { get; set; }
    public string? OneTimePassword { get; set; }
    public string Password { get; set; } = default!;
    public int FailedCount { get; set; } = default!;
    public DateTime? LastOnlineTime { get; set; }
    public DateTime PasswordExpiryDate { get; set; }


    public virtual ICollection<UserCompany> UserCompanyCreatedBies { get; set; } = default!;
    public virtual ICollection<UserCompany> UserCompanyModifiedBies { get; set; } = default!;
    public virtual ICollection<UserCompany> UserCompanyUsers { get; set; } = default!;
    public virtual BaseValue? BlockedReasonBase { get; set; } = default!;
    public virtual Person Person { get; set; } = default!;
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
    public virtual ICollection<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVoucherCreatedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVoucherModifiedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherLog> AutoVoucherLogCreatedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherLog> AutoVoucherLogModifiedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinkCreatedBies { get; set; } = default!;
    public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinkModifiedBies { get; set; } = default!;
    public virtual ICollection<BaseValue> BaseValueCreatedBies { get; set; } = default!;
    public virtual ICollection<BaseValue> BaseValueModifiedBies { get; set; } = default!;
    public virtual ICollection<BaseValueType> BaseValueTypeCreatedBies { get; set; } = default!;
    public virtual ICollection<BaseValueType> BaseValueTypeModifiedBies { get; set; } = default!;
    public virtual ICollection<Branch> BranchCreatedBies { get; set; } = default!;
    public virtual ICollection<Branch> BranchModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeAutoVoucherView> CodeAutoVoucherViewCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeAutoVoucherView> CodeAutoVoucherViewModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptionCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeRowDescription> CodeRowDescriptionModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypeCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypeModifiedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroupCreatedBies { get; set; } = default!;
    public virtual ICollection<CodeVoucherGroup> CodeVoucherGroupModifiedBies { get; set; } = default!;
    public virtual ICollection<CommodityCategory> CommodityCategoryCreatedBies { get; set; } = default!;
    public virtual ICollection<CommodityCategory> CommodityCategoryModifiedBies { get; set; } = default!;
    public virtual ICollection<CommodityCategoryProperty> CommodityCategoryPropertyCreatedBies { get; set; } = default!;
    public virtual ICollection<CommodityCategoryProperty> CommodityCategoryPropertyModifiedBies { get; set; } = default!;
    public virtual ICollection<Commodity> CommodityCreatedBies { get; set; } = default!;
    public virtual ICollection<Commodity> CommodityModifiedBies { get; set; } = default!;
    public virtual ICollection<CommodityProperty> CommodityPropertyCreatedBies { get; set; } = default!;
    public virtual ICollection<CommodityProperty> CommodityPropertyModifiedBies { get; set; } = default!;
    public virtual ICollection<CompanyInformation> CompanyInformationCreatedBies { get; set; } = default!;
    public virtual ICollection<CompanyInformation> CompanyInformationModifiedBies { get; set; } = default!;
    public virtual ICollection<CountryDivision> CountryDivisionCreatedBies { get; set; } = default!;
    public virtual ICollection<CountryDivision> CountryDivisionModifiedBies { get; set; } = default!;
    public virtual ICollection<DocumentItem> DocumentItemCreatedBies { get; set; } = default!;
    public virtual ICollection<DocumentItem> DocumentItemModifiedBies { get; set; } = default!;
    public virtual ICollection<DocumentHead> DocumentHeadCreatedBies { get; set; } = default!;
    public virtual ICollection<DocumentHead> DocumentHeadModifiedBies { get; set; } = default!;
    public virtual ICollection<Employee> EmployeeCreatedBies { get; set; } = default!;
    public virtual ICollection<Employee> EmployeeModifiedBies { get; set; } = default!;
    public virtual ICollection<HelpAttachment> HelpAttachmentCreatedBies { get; set; } = default!;
    public virtual ICollection<HelpAttachment> HelpAttachmentModifiedBies { get; set; } = default!;
    public virtual ICollection<HelpData> HelpDataCreatedBies { get; set; } = default!;
    public virtual ICollection<HelpData> HelpDataModifiedBies { get; set; } = default!;
    public virtual ICollection<Holiday> HolidayCreatedBies { get; set; } = default!;
    public virtual ICollection<Holiday> HolidayModifiedBies { get; set; } = default!;
    public virtual ICollection<Language> LanguageCreatedBies { get; set; } = default!;
    public virtual ICollection<Language> LanguageModifiedBies { get; set; } = default!;
    public virtual ICollection<MenuItem> MenuItemCreatedBies { get; set; } = default!;
    public virtual ICollection<MenuItem> MenuItemModifiedBies { get; set; } = default!;
    public virtual ICollection<Permission> PermissionCreatedBies { get; set; } = default!;
    public virtual ICollection<Permission> PermissionModifiedBies { get; set; } = default!;
    public virtual ICollection<Bank> BankCreatedBies { get; set; } = default!;
    public virtual ICollection<Bank> BankModifiedBies { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddressCreatedBies { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddressModifiedBies { get; set; } = default!;

    public virtual ICollection<PersonBankAccount> PersonBankAccountsCreatedBies { get; set; } = default!;
    public virtual ICollection<PersonPhone> PersonPhonesCreatedBies { get; set; } = default!;
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
    public virtual ICollection<ShiftInfo> ShiftInfoCreatedBies { get; set; } = default!;
    public virtual ICollection<ShiftInfo> ShiftInfoModifiedBies { get; set; } = default!;
    public virtual ICollection<Signer> SignerCreatedBies { get; set; } = default!;
    public virtual ICollection<Signer> SignerModifiedBies { get; set; } = default!;
    public virtual ICollection<Unit> UnitCreatedBies { get; set; } = default!;
    public virtual ICollection<Unit> UnitModifiedBies { get; set; } = default!;
    public virtual ICollection<UnitPosition> UnitPositionCreatedBies { get; set; } = default!;
    public virtual ICollection<UnitPosition> UnitPositionModifiedBies { get; set; } = default!;
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
    public virtual ICollection<Help> HelpCreatedBies { get; set; } = default!;
    public virtual ICollection<Help> HelpModifiedBies { get; set; } = default!;
    public List<UserPermission> UserPermissionCreatedBies { get; set; }
    public List<UserPermission> UserPermissionModifiedBies { get; set; }
    public List<UserPermission> UserPermissions { get; set; }
}