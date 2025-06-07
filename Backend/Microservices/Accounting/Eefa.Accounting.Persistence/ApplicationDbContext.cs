using Eefa.Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;


public partial class ApplicationDbContext : DbContext
    {
        public virtual DbSet<AccountHead> AccountHeads { get; set; }
        public virtual DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; }
        public virtual DbSet<AccountReference> AccountReferences { get; set; }
        public virtual DbSet<AccountReferencesGroup> AccountReferencesGroups { get; set; }
        public virtual DbSet<AccountReferencesRelReferencesGroup> AccountReferencesRelReferencesGroups { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<AutoVoucherFormula> AutoVoucherFormulas { get; set; }
        public virtual DbSet<BaseValue> BaseValues { get; set; }
        public virtual DbSet<BaseValueType> BaseValueTypes { get; set; }
        public virtual DbSet<CodeRowDescription> CodeRowDescriptions { get; set; }
        public virtual DbSet<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; }
        public virtual DbSet<CodeVoucherGroup> CodeVoucherGroups { get; set; }
        public virtual DbSet<CompanyInformation> CompanyInformations { get; set; }
        public virtual DbSet<CountryDivision> CountryDivisions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonAddress> PersonAddresses { get; set; }
        public virtual DbSet<PersonFingerprint> PersonFingerprints { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<RequiredPermission> RequiredPermissions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<UserYear> UserYears { get; set; }
        public virtual DbSet<VoucherAttachment> VoucherAttachments { get; set; }
        public virtual DbSet<VouchersDetail> VouchersDetails { get; set; }
        public virtual DbSet<VouchersHead> VouchersHeads { get; set; }
        public virtual DbSet<Year> Years { get; set; }
        public virtual DbSet<Customer> Customers { get; set; } = default!;
        public virtual DbSet<MoadianInvoiceDetail> MoadianInvoiceDetails { get; set; } = default!;
        public virtual DbSet<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
        public virtual DbSet<VerificationCode> VerificationCodes { get; set; } = default!;
        public virtual DbSet<AccountHeadCloseCode> AccountHeadCloseCodes { get; set; } = default!;


        public ApplicationDbContext() : base() {}
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

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


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }

