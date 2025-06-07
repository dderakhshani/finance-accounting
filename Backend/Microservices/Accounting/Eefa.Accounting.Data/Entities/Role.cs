using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class Role : BaseEntity
    {
     
        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// نقش صاحب سند
        /// </summary>
         

        /// <summary>
        /// ایجاد کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
        /// اصلاح کننده
        /// </summary>
         

        /// <summary>
        /// تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
        /// آیا حذف شده است؟
        /// </summary>
        

        public virtual Role? Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCode { get; set; } = default!;
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
        public virtual ICollection<DocumentPayment> DocumentPayments { get; set; } = default!;
    }
}
