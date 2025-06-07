using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using AccountHead = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AccountHead;
using AccountHeadRelReferenceGroup = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AccountHeadRelReferenceGroup;
using AccountReference = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AccountReference;
using AccountReferencesGroup = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AccountReferencesGroup;
using AccountReferencesRelReferencesGroup = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AccountReferencesRelReferencesGroup;
using Attachment = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Attachment;
using AutoVoucherFormula = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AutoVoucherFormula;
using AutoVoucherIncompleteVoucher = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AutoVoucherIncompleteVoucher;
using AutoVoucherLog = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AutoVoucherLog;
using AutoVoucherRowsLink = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.AutoVoucherRowsLink;
using Branch = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Branch;
using CodeAccountHeadGroup = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CodeAccountHeadGroup;
using CodeAutoVoucherView = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CodeAutoVoucherView;
using CodeRowDescription = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CodeRowDescription;
using CodeVoucherExtendType = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CodeVoucherExtendType;
using CodeVoucherGroup = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CodeVoucherGroup;
using Commodity = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Commodity;
using CommodityCategory = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CommodityCategory;
using CommodityCategoryProperty = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CommodityCategoryProperty;
using CommodityProperty = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CommodityProperty;
using CompanyInformation = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CompanyInformation;
using CountryDivision = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.CountryDivision;
using DataBaseMetadata = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.DataBaseMetadata;
using DocumentHead = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.DocumentHead;
using DocumentItem = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.DocumentItem;
using Employee = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Employee;
using HelpAttachment = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.HelpAttachment;
using HelpData = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.HelpData;
using Holiday = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Holiday;
using Language = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Language;
using MenuItem = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.MenuItem;
using Permission = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Permission;
using Person = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Person;
using PersonAddress = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.PersonAddress;
using PersonFingerprint = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.PersonFingerprint;
using Position = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Position;
using RequiredPermission = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.RequiredPermission;
using Role = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Role;
using RolePermission = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.RolePermission;
using ShiftInfo = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.ShiftInfo;
using Signer = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Signer;
using Unit = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Unit;
using UnitPosition = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.UnitPosition;
using User = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.User;
using UserRole = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.UserRole;
using UserSetting = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.UserSetting;
using UserYear = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.UserYear;
using VoucherAttachment = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.VoucherAttachment;
using VouchersDetail = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.VouchersDetail;
using VouchersHead = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.VouchersHead;
using Year = Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities.Year;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.SqlServer.Context
{
    public interface IWorkflowAdminUnitOfWork : IUnitOfWork
    {
        DbSet<CorrectionRequest> CorrectionRequests { get; set; }
        DbSet<AccountHead> AccountHeads { get; set; }
        DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        DbSet<AccountReference> AccountReferences { get; set; }
        DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        DbSet<Attachment> Attachments { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<SalesAgent> SalesPersons { get; set; }
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
        DbSet<Person> Persons { get; set; }
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
        DbSet<User> Users { get; set; }
        DbSet<UserRole> UserRoles { get; set; }
        DbSet<UserSetting> UserSettings { get; set; }
        DbSet<UserYear> UserYears { get; set; }
        DbSet<ValidationMessage> ValidationMessages { get; set; }
        DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        DbSet<VouchersDetail> VouchersDetails { get; set; }
        DbSet<VouchersHead> VouchersHeads { get; set; }
        DbSet<Year> Years { get; set; }
        public WorkflowAdminUnitOfWork Mock();

    }


}