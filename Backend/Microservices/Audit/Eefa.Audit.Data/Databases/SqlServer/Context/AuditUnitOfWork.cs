using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Audit.Data.Databases.Entities;
using Library.Common;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Eefa.Audit.Data.Databases.SqlServer.Context
{
    public partial class AuditUnitOfWork : DbContext, IAuditUnitOfWork
    {
        public new IModel Model()
        {
            return base.Model;
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {

            return base.Set<TEntity>();

        }

        public EntityState State<TEntity>() where TEntity : class, IBaseEntity
        {
            return _appDbContext.Entry(typeof(TEntity)).State;
        }

        public EntityEntry<TEntity> GetEntry<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            throw new NotImplementedException();
        }

        public DbContext DbContext()
        {
            return _appDbContext;
        }

        public Task<List<TEntity>> ExecuteSqlQueryAsync<TEntity>(string query, object[] parameters, CancellationToken cancellationToken) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                //var logs = await _appDbContext.DbContex().BeforeSaveChanges(SubSystem.Admin, 0);
                //foreach (var auditEntry in logs)
                //{
                //    AuditLogs.Add(auditEntry.ToAudit());
                //}

                var res = await _appDbContext.SaveChangesAsync(cancellationToken);

                _appDbContext.ChangeTracker.Clear();

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        public async Task<int> SaveAsync(int formId, CancellationToken cancellationToken)
        {
            try
            {
                //var logs = await _appDbContext.DbContex().BeforeSaveChanges(SubSystem.Admin, formId);
                //foreach (var auditEntry in logs)
                //{
                //    AuditLogs.Add(auditEntry.ToAudit());
                //}

                var res = await _appDbContext.SaveChangesAsync(cancellationToken);

                _appDbContext.ChangeTracker.Clear();

                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        private AuditUnitOfWork _appDbContext;

        public AuditUnitOfWork(DbContextOptions<AuditUnitOfWork> options)
            : base(options)
        {
            _appDbContext = this;
            //this.ChangeTracker.LazyLoadingEnabled = false;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(message => Debug.WriteLine(message));
        }

        public AuditUnitOfWork()
        {

        }
        public DbSet<Library.Models.Audit> AuditLogs { get; set; }

        public DbSet<AccountHead> AccountHeads { get; set; }
        public DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        public DbSet<AccountReference> AccountReferences { get; set; }
        public DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        public DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
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
        
        public AuditUnitOfWork Mock()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AuditUnitOfWork>()
                .UseInMemoryDatabase("MockDB")
                .Options;
            _appDbContext = new AuditUnitOfWork(dbContextOptions);
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