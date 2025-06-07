using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.Data;
using Eefa.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Eefa.Bursary.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Bursary.Domain.Entities.Calendar;
using Eefa.Bursary.Domain.Entities.EditRequest;

namespace Eefa.Bursary.Infrastructure
{
    public partial class BursaryContext : AuditableDbContext, IBursaryUnitOfWork
    {

        public virtual DbSet<AccountHead> AccountHead { get; set; }
        public virtual DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroup { get; set; }
        public virtual DbSet<AccountReferences> AccountReferences { get; set; }
        public virtual DbSet<AccountReferencesGroups> AccountReferencesGroups { get; set; }
        public virtual DbSet<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; }
        public virtual DbSet<Activities> Activities { get; set; }
        public virtual DbSet<Attachment> Attachment { get; set; }
        public virtual DbSet<AutoVoucherFormula> AutoVoucherFormula { get; set; }
        public virtual DbSet<AutoVoucherIncompleteVouchers> AutoVoucherIncompleteVouchers { get; set; }
        public virtual DbSet<AutoVoucherLog> AutoVoucherLog { get; set; }
        public virtual DbSet<AutoVoucherRowsLink> AutoVoucherRowsLink { get; set; }
        public virtual DbSet<BankAccountCardex> BankAccountCardex { get; set; }
        public virtual DbSet<BankAccounts> BankAccounts { get; set; }
        public virtual DbSet<BankBranches> BankBranches { get; set; }
        public virtual DbSet<Banks> Banks { get; set; }
        public virtual DbSet<BaseValueTypes> BaseValueTypes { get; set; }
        public virtual DbSet<BaseValues> BaseValues { get; set; }
        public virtual DbSet<BomItems> BomItems { get; set; }
        public virtual DbSet<BomValueHeaders> BomValueHeaders { get; set; }
        public virtual DbSet<BomValues> BomValues { get; set; }
        public virtual DbSet<Boms> Boms { get; set; }
        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<BusinessRuleConditions> BusinessRuleConditions { get; set; }
        public virtual DbSet<BusinessRules> BusinessRules { get; set; }
        public virtual DbSet<CategoryPropertyMappings> CategoryPropertyMappings { get; set; }
        public virtual DbSet<ChequeSheets> ChequeSheets { get; set; }
        public virtual DbSet<CodeAccountHeadGroup> CodeAccountHeadGroup { get; set; }
        public virtual DbSet<CodeAutoVoucherViews> CodeAutoVoucherViews { get; set; }
        public virtual DbSet<CodeRowDescription> CodeRowDescription { get; set; }
        public virtual DbSet<CodeVoucherExtendType> CodeVoucherExtendType { get; set; }
        public virtual DbSet<CodeVoucherGroups> CodeVoucherGroups { get; set; }
        public virtual DbSet<CodingTemplateProperties> CodingTemplateProperties { get; set; }
        public virtual DbSet<CodingTemplates> CodingTemplates { get; set; }
        public virtual DbSet<Commodities> Commodities { get; set; }
        public virtual DbSet<CommodityCategories> CommodityCategories { get; set; }
        public virtual DbSet<CommodityCategoryMeasures> CommodityCategoryMeasures { get; set; }
        public virtual DbSet<CommodityCategoryProperties> CommodityCategoryProperties { get; set; }
        public virtual DbSet<CommodityCategoryPropertyItems> CommodityCategoryPropertyItems { get; set; }
        public virtual DbSet<CommodityMeasures> CommodityMeasures { get; set; }
        public virtual DbSet<CommodityPropertyValues> CommodityPropertyValues { get; set; }
        public virtual DbSet<CompanyInformations> CompanyInformations { get; set; }
        public virtual DbSet<CorrectionRequests> CorrectionRequests { get; set; }
        public virtual DbSet<CountryDivisions> CountryDivisions { get; set; }
        public virtual DbSet<CountryDivisionsMain> CountryDivisionsMain { get; set; }
        public virtual DbSet<DataBaseMetadata> DataBaseMetadata { get; set; }
        public virtual DbSet<DataDictionary> DataDictionary { get; set; }
        public virtual DbSet<DataDictionaryold> DataDictionaryold { get; set; }
        public virtual DbSet<DocumentAttachments> DocumentAttachments { get; set; }
        public virtual DbSet<DocumentHeadExtend> DocumentHeadExtend { get; set; }
        public virtual DbSet<DocumentHeadPurchaseOrders> DocumentHeadPurchaseOrders { get; set; }
        public virtual DbSet<DocumentHeads> DocumentHeads { get; set; }
        public virtual DbSet<DocumentItems> DocumentItems { get; set; }
        public virtual DbSet<DocumentItemsBom> DocumentItemsBom { get; set; }
        public virtual DbSet<DocumentItemsMeasurValues> DocumentItemsMeasurValues { get; set; }
        public virtual DbSet<DocumentNumberingFormats> DocumentNumberingFormats { get; set; }
        public virtual DbSet<DocumentPayments> DocumentPayments { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<FinancialRequestAttachments> FinancialRequestAttachments { get; set; }
        public virtual DbSet<FinancialRequestDetails> FinancialRequestDetails { get; set; }
        public virtual DbSet<FinancialRequestDocuments> FinancialRequestDocuments { get; set; }
        public virtual DbSet<FinancialRequestPartial> FinancialRequestPartial { get; set; }
        public virtual DbSet<FinancialRequestVerifiers> FinancialRequestVerifiers { get; set; }
        public virtual DbSet<FinancialRequest> FinancialRequests { get; set; }
        public virtual DbSet<FreightPayList> FreightPayList { get; set; }
        public virtual DbSet<FreightPays> FreightPays { get; set; }
        public virtual DbSet<HelpAttachment> HelpAttachment { get; set; }
        public virtual DbSet<HelpData> HelpData { get; set; }
        public virtual DbSet<Holidays> Holidays { get; set; }
        public virtual DbSet<Lanes> Lanes { get; set; }
        public virtual DbSet<Languages> Languages { get; set; }
        public virtual DbSet<MeasureUnitConversions> MeasureUnitConversions { get; set; }
        public virtual DbSet<MeasureUnits> MeasureUnits { get; set; }
        public virtual DbSet<MenuItems> MenuItems { get; set; }
        public virtual DbSet<MoadianInvoiceDetails> MoadianInvoiceDetails { get; set; }
        public virtual DbSet<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; }
        public virtual DbSet<OtherFrieght> OtherFrieght { get; set; }
        public virtual DbSet<PayCheque> PayCheque { get; set; }
        public virtual DbSet<PerformerCondition> PerformerCondition { get; set; }
        public virtual DbSet<Performers> Performers { get; set; }
        public virtual DbSet<Permissions> Permissions { get; set; }
        public virtual DbSet<PersonAddress> PersonAddress { get; set; }
        public virtual DbSet<PersonBankAccounts> PersonBankAccounts { get; set; }
        public virtual DbSet<PersonBankAcounts> PersonBankAcounts { get; set; }
        public virtual DbSet<PersonFingerprint> PersonFingerprint { get; set; }
        public virtual DbSet<PersonPhones> PersonPhones { get; set; }
        public virtual DbSet<PersonRelAttachments> PersonRelAttachments { get; set; }
        public virtual DbSet<Persons> Persons { get; set; }
        public virtual DbSet<Pools> Pools { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
        public virtual DbSet<Requests> Requests { get; set; }
        public virtual DbSet<RequiredPermission> RequiredPermission { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<RunningWorkflow> RunningWorkflow { get; set; }
        public virtual DbSet<RuntimeActivities> RuntimeActivities { get; set; }
        public virtual DbSet<RuntimeProcess> RuntimeProcess { get; set; }
        public virtual DbSet<RuntimeWorkflow> RuntimeWorkflow { get; set; }
        public virtual DbSet<RuntimeWorkflowParameter> RuntimeWorkflowParameter { get; set; }
        public virtual DbSet<ServiceActivities> ServiceActivities { get; set; }
        public virtual DbSet<Shapes> Shapes { get; set; }
        public virtual DbSet<ShiftInfo> ShiftInfo { get; set; }
        public virtual DbSet<Signers> Signers { get; set; }
        public virtual DbSet<TaskComments> TaskComments { get; set; }
        public virtual DbSet<TaskWorkHistory> TaskWorkHistory { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<TransitionShapes> TransitionShapes { get; set; }
        public virtual DbSet<Transitions> Transitions { get; set; }
        public virtual DbSet<UIFormControls> UIFormControls { get; set; }
        public virtual DbSet<UIFormVariables> UIFormVariables { get; set; }
        public virtual DbSet<UIForms> UIForms { get; set; }
        public virtual DbSet<UIGrid> UIGrid { get; set; }
        public virtual DbSet<UIGridColumns> UIGridColumns { get; set; }
        public virtual DbSet<UnitPositions> UnitPositions { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<UserCompany> UserCompany { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }
        public virtual DbSet<UserSetting> UserSetting { get; set; }
        public virtual DbSet<UserYear> UserYear { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<ValidationMessages> ValidationMessages { get; set; }
        public virtual DbSet<VerificationCodes> VerificationCodes { get; set; }
        public virtual DbSet<VoucherAttachments> VoucherAttachments { get; set; }
        public virtual DbSet<VoucherDetailAttachments> VoucherDetailAttachments { get; set; }
        public virtual DbSet<VouchersDetail> VouchersDetail { get; set; }
        public virtual DbSet<VouchersHead> VouchersHead { get; set; }
        public virtual DbSet<Workflows> Workflows { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<FinancialRequestLogs> FinancialRequestLogs { get; set; }

        public virtual DbSet<_CommoditiesCardex> _CommoditiesCardex { get; set; }
        public virtual DbSet<_FileAccessRole> _FileAccessRole { get; set; }
        public virtual DbSet<_FileAccessRoleUsers> _FileAccessRoleUsers { get; set; }
        public virtual DbSet<MapDanaAndTadbir> MapDanaAndTadbir { get; set; }
        public virtual DbSet<Payables_ChequeBooks> Payables_ChequeBooks { get; set; }
        public virtual DbSet<Payables_ChequeBooksSheets> Payables_ChequeBooksSheets { get; set; }
        public virtual DbSet<Payables_Documents> Payables_Documents { get; set; }
        public virtual DbSet<Payables_DocumentsAccounts> Payables_DocumentsAccounts { get; set; }
        public virtual DbSet<Payables_DocumentsOperations> Payables_DocumentsOperations { get; set; }
        public virtual DbSet<Payables_ChecqueBooks_View> Payables_CheckBooks_View { get; set; }
        public virtual DbSet<Payables_DocumentsPayOrders> Payables_DocumentsPayOrders { get; set; }
        public virtual DbSet<Payables_Documents_View> Payables_Documents_View { get; set; }
        public virtual DbSet<Payables_DocumentsAccounts_View> Payables_DocumentsAccounts_View { get; set; }
        public virtual DbSet<Payables_DocumentsOperations_View> Payables_DocumentsOperations_View { get; set; }
        public virtual DbSet<Payables_DocumentOperations_Chain> Payables_DocumentOperations_Chain { get; set; }
        public virtual DbSet<Banks_View> Banks_View { get; set; }
        public virtual DbSet<Payables_ChecqueBooks_View> Payables_ChecqueBooks_View { get; set; }
        public virtual DbSet<BankAccountReferences_View> BankAccountReferences_View { get; set; }
        public virtual DbSet<Payables_ChequeBooksSheets_View> Payables_ChequeBooksSheets_View { get; set; }
        public virtual DbSet<Payables_PayOrders_View> Payables_PayOrders_View { get; set; }
        public virtual DbSet<Payables_PayRequests_View> Payables_PayRequests_View { get; set; }
        public virtual DbSet<Payables_PayTypes_View> Payables_PayTypes_View { get; set; }
        public virtual DbSet<Payables_Subjects_View> Payables_Subjects_View { get; set; }
        public virtual DbSet<Payables_DocumentsPayOrders_View> Payables_DocumentsPayOrders_View { get; set; }
        public virtual DbSet<Payables_DocumentOperations_Chain_View> Payables_DocumentOperations_Chain_View { get; set; }
        public virtual DbSet<Payables_ChequeOperations_View> Payables_ChequeOperations_View { get; set; }

        public virtual DbSet<BankTypes_View> BankTypes_View { get; set; }
        public virtual DbSet<BankAccounts_View> BankAccounts_View { get; set; }
        public virtual DbSet<BankBranches_View> BankBranches_View { get; set; }
        public virtual DbSet<CountryDivisions_View> CountryDivisions_View { get; set; }
        public virtual DbSet<BankAccountTypes_View> BankAccountTypes_View { get; set; }
        public virtual DbSet<BankTransactionHead> BankTransactionHead { get; set; }
        public virtual DbSet<BankTransactions> BankTransaction { get; set; }
        public virtual DbSet<CurrencyTypes_View> CurrencyTypes_View { get; set; }
        public virtual DbSet<ChequeTypes_View> ChequeTypes_View { get; set; }

        public virtual DbSet<ResourceExpense_View> ResourceExpense_View { get; set; }
        public virtual DbSet<MonthlyForecast> MonthlyForecast { get; set; }
        public virtual DbSet<DailyForecast> DailyForecast { get; set; }
        public virtual DbSet<MonthlyForecast_View> MonthlyForecast_View { get; set; }
        public virtual DbSet<DateConversion> DateConversion { get; set; }

        public BursaryContext(DbContextOptions<BursaryContext> options, ICurrentUserAccessor currentUserAccessor, IConfiguration configuration)
             : base(options, currentUserAccessor, configuration)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var userYearId = base._currentUserAccessor.GetYearId();

            if (userYearId == 3)
            {
                optionsBuilder.UseSqlServer(base._configuration.GetConnectionString("DanaString"));
            }
            else if (userYearId > 3)
            {
                optionsBuilder.UseSqlServer(base._configuration.GetConnectionString("DefaultString"));
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                //Automatically remove logical deleted record by Adding Default Query
                if (typeof(IBaseEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

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
