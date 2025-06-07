using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities;
using Library.Common;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
    public partial class WorkflowAdminUnitOfWork : AuditableDbContext, IWorkflowAdminUnitOfWork
    {
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }
        public async Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class
        {
            return await this.SqlQueryAsync<TResult>(query, parameters, cancellationToken);
        }

        public DbContext DbContex()
        {
            return this;
        }

        private WorkflowAdminUnitOfWork _appDbContext;

        public WorkflowAdminUnitOfWork(DbContextOptions<WorkflowAdminUnitOfWork> options)
            : base(options)
        {
            _appDbContext = this;

            this.ChangeTracker.LazyLoadingEnabled = false;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=192.168.14.13\\sql2019;Initial Catalog=EefaTest;User Id=ssrs;Password=123456;TrustServerCertificate=True").LogTo(x => Debug.WriteLine(x));
          //  optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        public WorkflowAdminUnitOfWork()
        {

        }


        public WorkflowAdminUnitOfWork Mock()
        {
            var dbContextOptions = new DbContextOptionsBuilder<WorkflowAdminUnitOfWork>()
                .UseInMemoryDatabase("MockDB")
                .Options;
            _appDbContext = new WorkflowAdminUnitOfWork(dbContextOptions);
            return _appDbContext;
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }
            //--------------------------Convert Date To UTC-----------------------------
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
            //------------------------------------------------------------------------
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CorrectionRequest> CorrectionRequests { get; set; }

        public DbSet<AccountHead> AccountHeads { get; set; }
        public DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        public DbSet<AccountReference> AccountReferences { get; set; }
        public DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        public DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesAgent> SalesPersons { get; set; }
        public DbSet<AutoVoucherFormula> AutoVoucherFormulas { get; set; }
        public DbSet<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVouchers { get; set; }
        public DbSet<AutoVoucherLog> AutoVoucherLogs { get; set; }
        public DbSet<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; }
        public DbSet<BaseValue> BaseValues { get; set; }
        public DbSet<BaseValueType> BaseValueTypes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; }
        public DbSet<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; }
        public DbSet<CodeRowDescription> CodeRowDescriptions { get; set; }
        public DbSet<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; }
        public DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<CommodityCategory> CommodityCategories { get; set; }
        public DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }
        public DbSet<CommodityProperty> CommodityProperties { get; set; }
        public DbSet<CompanyInformation> CompanyInformations { get; set; }
        public DbSet<CountryDivision> CountryDivisions { get; set; }
        public DbSet<DataBaseMetadata> DataBaseMetadatas { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<DocumentHead> DocumentHeads { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<HelpAttachment> HelpAttachments { get; set; }
        public DbSet<HelpData> HelpDatas { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonAddress> PersonAddresses { get; set; }
        public DbSet<Bank> Banks { get; set; } = default!;
        public DbSet<PersonBankAccount> PersonBankAccounts { get; set; } = default!;
        public DbSet<PersonPhone> PersonPhones { get; set; } = default!;
        public DbSet<PersonFingerprint> PersonFingerprints { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<RequiredPermission> RequiredPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<ShiftInfo> ShiftInfoes { get; set; }
        public DbSet<Signer> Signers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitPosition> UnitPositions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<UserYear> UserYears { get; set; }
        public DbSet<ValidationMessage> ValidationMessages { get; set; }
        public DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        public DbSet<VouchersDetail> VouchersDetails { get; set; }
        public DbSet<VouchersHead> VouchersHeads { get; set; }
        public DbSet<Year> Years { get; set; }

    }

    public static class EFFilterExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder });
        }

        static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EFFilterExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(t => t.IsGenericMethod);

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder)
            where TEntity : class, IBaseEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(x => !x.IsDeleted);
        }
    }

    public class Cloner : ICloneable
    {
        public ChangeTracker ChangeTracker { get; set; }

        public Cloner(ChangeTracker changeTracker)
        {
            ChangeTracker = changeTracker;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}