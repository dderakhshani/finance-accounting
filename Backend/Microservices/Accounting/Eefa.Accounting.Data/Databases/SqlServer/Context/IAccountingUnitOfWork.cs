using Eefa.Accounting.Data.Databases.Sp;
using Eefa.Accounting.Data.Entities;
using Eefa.Accounting.Data.Events.Abstraction;
using Eefa.Accounting.Data.Logs;
using Eefa.Accounting.Data.Views;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Data.Databases.SqlServer.Context
{
    public interface IAccountingUnitOfWork : IUnitOfWork
    {
        DbSet<ApplicationRequestLog> ApplicationRequestLogs { get; set; }
        DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        DbSet<AccountHead> AccountHeads { get; set; }
        DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        DbSet<AccountReference> AccountReferences { get; set; }
        DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<AutoVoucherFormula> AutoVoucherFormulas { get; set; }
        DbSet<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVouchers { get; set; }
        DbSet<DocumentPayment> DocumentPayments { get; set; }

        DbSet<AutoVoucherLog> AutoVoucherLogs { get; set; }
        DbSet<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; }
        DbSet<BaseValue> BaseValues { get; set; }
        DbSet<BaseValueType> BaseValueTypes { get; set; }
        DbSet<Branch> Branches { get; set; }
        DbSet<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; }
        DbSet<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; }
        DbSet<CodeRowDescription> CodeRowDescriptions { get; set; }
        DbSet<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; }
        DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        DbSet<Commodity> Commodities { get; set; }
        DbSet<CommodityCategory> CommodityCategories { get; set; }
        DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }
        DbSet<CommodityProperty> CommodityProperties { get; set; }
        DbSet<CompanyInformation> CompanyInformations { get; set; }
        DbSet<CountryDivision> CountryDivisions { get; set; }


        DbSet<DocumentItem> DocumentItems { get; set; }
        DbSet<DocumentHead> DocumentHeads { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<HelpAttachment> HelpAttachments { get; set; }
        DbSet<HelpData> HelpDatas { get; set; }
        DbSet<Holiday> Holidays { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<MenuItem> MenuItems { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Person> Persons { get; set; }
        DbSet<PersonAddress> PersonAddresses { get; set; }
        DbSet<PersonFingerprint> PersonFingerprints { get; set; }
        DbSet<PersonPhone> PersonPhones { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<RequiredPermission> RequiredPermissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<ShiftInfo> ShiftInfoes { get; set; }
        DbSet<Signer> Signers { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<UnitPosition> UnitPositions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserSetting> UserSettings { get; set; }
        DbSet<UserYear> UserYears { get; set; }
        DbSet<ValidationMessage> ValidationMessages { get; set; }
        DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        DbSet<VouchersDetail> VouchersDetails { get; set; }
        DbSet<VouchersHead> VouchersHeads { get; set; }
        DbSet<Year> Years { get; set; }
        DbSet<ViewVoucherDetail> ViewVoucherDetails { get; set; }
        DbSet<MapDanaAndTadbir> MapDanaAndTadbir { get; set; }
        DbSet<MapDanaToTadbir> MapDanaToTadbir { get; set; }
        DbSet<CorrectionRequest> CorrectionRequests { get; set; }

        DbSet<StpReportBalance6Result> StpReportBalance6Result { get; set; }

    }
}