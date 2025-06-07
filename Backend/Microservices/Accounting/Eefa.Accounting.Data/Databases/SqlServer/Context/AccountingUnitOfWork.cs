using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using Eefa.Accounting.Data.Entities;
using Library.Common;
using Library.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Eefa.Accounting.Data.Views;
using Library.Interfaces;
using Microsoft.Extensions.Configuration;
using Eefa.Accounting.Data.Events.Abstraction;
using Eefa.Accounting.Data.Logs;
using Eefa.Accounting.Data.Databases.Sp;

namespace Eefa.Accounting.Data.Databases.SqlServer.Context
{

    public partial class AccountingUnitOfWork : AuditableDbContext, IAccountingUnitOfWork
    {
        public virtual DbSet<ApplicationRequestLog> ApplicationRequestLogs { get; set; }
        public virtual DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public virtual DbSet<AccountHead> AccountHeads { get; set; }
        public virtual DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        public virtual DbSet<AccountReference> AccountReferences { get; set; }
        public virtual DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        public virtual DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AutoVoucherFormula> AutoVoucherFormulas { get; set; }
        public virtual DbSet<AutoVoucherIncompleteVoucher> AutoVoucherIncompleteVouchers { get; set; }
        public virtual DbSet<AutoVoucherLog> AutoVoucherLogs { get; set; }
        public virtual DbSet<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; }
        public virtual DbSet<BaseValue> BaseValues { get; set; }
        public virtual DbSet<BaseValueType> BaseValueTypes { get; set; }
        public virtual DbSet<DocumentPayment> DocumentPayments { get; set; } = default!;

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; }
        public virtual DbSet<CodeAutoVoucherView> CodeAutoVoucherViews { get; set; }
        public virtual DbSet<CodeRowDescription> CodeRowDescriptions { get; set; }
        public virtual DbSet<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; }
        public virtual DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<CommodityCategory> CommodityCategories { get; set; }
        public virtual DbSet<CommodityCategoryProperty> CommodityCategoryProperties { get; set; }
        public virtual DbSet<CommodityProperty> CommodityProperties { get; set; }
        public virtual DbSet<CompanyInformation> CompanyInformations { get; set; }
        public virtual DbSet<CountryDivision> CountryDivisions { get; set; }
        public virtual DbSet<DocumentItem> DocumentItems { get; set; }
        public virtual DbSet<DocumentHead> DocumentHeads { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<HelpAttachment> HelpAttachments { get; set; }
        public virtual DbSet<HelpData> HelpDatas { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonAddress> PersonAddresses { get; set; }
        public virtual DbSet<PersonFingerprint> PersonFingerprints { get; set; }
        public virtual DbSet<PersonPhone> PersonPhones { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<RequiredPermission> RequiredPermissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<ShiftInfo> ShiftInfoes { get; set; }
        public virtual DbSet<Signer> Signers { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<UnitPosition> UnitPositions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<UserYear> UserYears { get; set; }
        public virtual DbSet<ValidationMessage> ValidationMessages { get; set; }
        public virtual DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        public virtual DbSet<VouchersDetail> VouchersDetails { get; set; }
        public virtual DbSet<VouchersHead> VouchersHeads { get; set; }
        public virtual DbSet<Year> Years { get; set; }
        public virtual DbSet<Customer> Customers { get; set; } = default!;
        public virtual DbSet<MoadianInvoiceDetail> MoadianInvoiceDetails { get; set; } = default!;
        public virtual DbSet<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; } = default!;

        public DbSet<ViewVoucherDetail> ViewVoucherDetails { get; set; }

        public DbSet<MapDanaAndTadbir> MapDanaAndTadbir { get; set; }
        public DbSet<MapDanaToTadbir> MapDanaToTadbir { get; set; }

        public DbSet<CorrectionRequest> CorrectionRequests {  get; set; }

        public DbSet<StpReportBalance6Result> StpReportBalance6Result { get; set; }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }

        public DbContext DbContext()
        {
            return _appDbContext;
        }



        private readonly AccountingUnitOfWork _appDbContext;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IConfiguration _configuration;

        public AccountingUnitOfWork(DbContextOptions<AccountingUnitOfWork> options, ICurrentUserAccessor currentUserAccessor, IConfiguration configuration)
            : base(options)
        {
            _appDbContext = this;
            _currentUserAccessor = currentUserAccessor;
            _configuration = configuration;
        }
        public async Task<List<TResult>> ExecuteSqlQueryAsync<TResult>(string query, object[] parameters, CancellationToken cancellationToken) where TResult : class
        {
            return await this.SqlQueryAsync<TResult>(query, parameters, cancellationToken);
        }

        public async Task ExecuteSqlQueryAsync(string query, object[] parameters, CancellationToken cancellationToken)
        {
            await this.SqlQueryAsync(query, parameters, cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Remove For Chocolate Factory Project

            var userYearId = _currentUserAccessor.GetYearId();

            if (userYearId == 3)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DanaString"));
            }
            else
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultString"));
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ;

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


            modelBuilder.Entity<ViewVoucherDetail>().ToView("ViewVoucherDetail", schema: "accounting").HasNoKey();
            //------------------------------------------------------------------------
            base.OnModelCreating(modelBuilder);
        }
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
}