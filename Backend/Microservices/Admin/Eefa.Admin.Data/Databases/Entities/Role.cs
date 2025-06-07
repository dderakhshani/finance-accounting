using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.Admin.Data.Databases.Entities
{
    [HasUniqueIndex]
    public partial class Role : HierarchicalBaseEntity
    {
        //public int? ParentId { get; set; }

        ///// <summary>
        ///// کد سطح
        ///// </summary>
        //public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        [UniqueIndex]
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }


        public virtual Role? Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = default!;

        public virtual ICollection<AccountHead> AccountHeads { get; set; } = default!;
        public virtual ICollection<AccountReference> AccountReferences { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroup> AccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVouchers { get; set; } = default!;
        public virtual ICollection<AutoVoucherLog> AutoVoucherLogs { get; set; } = default!;
        public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypes { get; set; } = default!;
        public virtual ICollection<Branch> Branches { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
        public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; } = default!;
        public virtual ICollection<CodeVoucherGroup> CodeVoucherGroups { get; set; } = default!;
        public virtual ICollection<Commodity> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategory> CommodityCategories { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
        public virtual ICollection<CountryDivision> CountryDivisions { get; set; } = default!;
        public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeads { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
        public virtual ICollection<HelpData> HelpDatas { get; set; } = default!;
        public virtual ICollection<Holiday> Holidays { get; set; } = default!;
        public virtual ICollection<Role> InverseParent { get; set; } = default!;
        public virtual ICollection<Language> Languages { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = default!;
        public virtual ICollection<Permission> Permissions { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<Bank> Banks { get; set; } = default!;
        public virtual ICollection<PersonBankAccount> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonPhone> PersonPhones { get; set; } = default!;
        public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
        public virtual ICollection<Person> Persons { get; set; } = default!;
        public virtual ICollection<Position> Positions { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissions { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionOwnerRoles { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissionRoles { get; set; } = default!;
        public virtual ICollection<ShiftInfo> ShiftInfoes { get; set; } = default!;
        public virtual ICollection<Signer> Signers { get; set; } = default!;
        public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
        public virtual ICollection<Unit> Units { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleOwnerRoles { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleRoles { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettings { get; set; } = default!;
        public virtual ICollection<UserYear> UserYears { get; set; } = default!;
        public virtual ICollection<VoucherAttachment> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual ICollection<VouchersHead> VouchersHeads { get; set; } = default!;
        public virtual ICollection<Year> Years { get; set; } = default!;
        public List<UserPermission> UserPermissions { get; set; }
    }
}
