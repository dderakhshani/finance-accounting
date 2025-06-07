using Eefa.Admin.Data.Databases.Entities;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Data.Databases.SqlServer.Context
{
    public interface IAdminUnitOfWork : IUnitOfWork
    {
        DbSet<AccountHead> AccountHeads { get; set; }
        DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        DbSet<AccountReference> AccountReferences { get; set; }
        DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<SalesAgents> SalesPersons { get; set; }
        DbSet<AutoVoucherFormula> AutoVoucherFormulas { get; set; }
        DbSet<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVouchers { get; set; }
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


        DbSet<DataBaseMetadata> DataBaseMetadatas { get; set; }
        DbSet<DocumentItem> DocumentItems { get; set; }
        DbSet<DocumentHead> DocumentHeads { get; set; }
        DbSet<Employee> Employees { get; set; }
        DbSet<HelpAttachment> HelpAttachments { get; set; }
        DbSet<HelpData> HelpDatas { get; set; }
        DbSet<Holiday> Holidays { get; set; }
        DbSet<Language> Languages { get; set; }
        DbSet<MenuItem> MenuItems { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<Entities.Person> Persons { get; set; }
        DbSet<PersonAddress> PersonAddresses { get; set; }
        DbSet<Bank> Banks { get; set; }
        DbSet<PersonBankAccount> PersonBankAccounts { get; set; }
        DbSet<PersonPhone> PersonPhones { get; set; }
        DbSet<PersonFingerprint> PersonFingerprints { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<RequiredPermission> RequiredPermissions { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }
        DbSet<ShiftInfo> ShiftInfoes { get; set; }
        DbSet<Signer> Signers { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<UnitPosition> UnitPositions { get; set; }
        DbSet<Entities.User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserSetting> UserSettings { get; set; }
        DbSet<UserYear> UserYears { get; set; }
        DbSet<ValidationMessage> ValidationMessages { get; set; }
        DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        DbSet<VouchersDetail> VouchersDetails { get; set; }
        DbSet<VouchersHead> VouchersHeads { get; set; }
        DbSet<Year> Years { get; set; }
        DbSet<CorrectionRequest> CorrectionRequests { get; set; }
        public AdminUnitOfWork Mock();

    }


}