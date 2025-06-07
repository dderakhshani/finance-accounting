using System;
using System.Collections.Generic;
using Eefa.Accounting.Data.Events.Abstraction;
using Eefa.Accounting.Data.Logs;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{

    public partial class User : BaseEntity
    {
       


        /// <summary>
        /// کد
        /// </summary>
         

        /// <summary>
        /// کد پرسنلی
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// نام کاربری
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        /// آیا قفل شده است؟
        /// </summary>
        public bool IsBlocked { get; set; } = default!;

        /// <summary>
        /// علت قفل شدن
        /// </summary>
        public int? BlockedReasonBaseId { get; set; }

        /// <summary>
        /// رمز یکبار مصرف
        /// </summary>
        public string? OneTimePassword { get; set; }

        /// <summary>
        /// رمز
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// دفعات ورود ناموفق
        /// </summary>
        public int FailedCount { get; set; } = default!;

        /// <summary>
        /// آخرین زمان آنلاین بودن
        /// </summary>
        public DateTime? LastOnlineTime { get; set; }

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
        

        public virtual BaseValue? BlockedReasonBase { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCodeCreatedBies { get; set; } = default!;
        public virtual ICollection<AccountHeadCloseCode> AccountHeadCloseCodeModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentPayment> DocumentPaymentCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentPayment> DocumentPaymentModifiedBies { get; set; } = default!;
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

        public virtual ICollection<ApplicationEvent> ApplicationEventCreatedBies { get; set; } = default!;
        public virtual ICollection<ApplicationRequestLog> ApplicationRequestLogCreatedBies { get; set; } = default!;

        public virtual ICollection<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
    }
}
