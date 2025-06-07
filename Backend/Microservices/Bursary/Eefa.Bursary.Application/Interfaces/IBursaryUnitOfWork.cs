using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Domain.Entities.Calendar;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Domain.Entities.EditRequest;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Infrastructure.Interfaces
{
    public interface IBursaryUnitOfWork:IUnitOfWork
    {
        public DbSet<AccountHead> AccountHead { get; set; }  
        public DbSet<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroup { get; set; } 
        public DbSet<AccountReferences> AccountReferences { get; set; }  
        public DbSet<AccountReferencesGroups> AccountReferencesGroups { get; set; }  
        public DbSet<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; }  
        public DbSet<Activities> Activities { get; set; }  
        public DbSet<Attachment> Attachment { get; set; }  
        public DbSet<AutoVoucherFormula> AutoVoucherFormula { get; set; }  
        public DbSet<AutoVoucherIncompleteVouchers> AutoVoucherIncompleteVouchers { get; set; }  
        public DbSet<AutoVoucherLog> AutoVoucherLog { get; set; }  
        public DbSet<AutoVoucherRowsLink> AutoVoucherRowsLink { get; set; }  
        public DbSet<BankAccountCardex> BankAccountCardex { get; set; }  
        public DbSet<BankAccounts> BankAccounts { get; set; }  
        public DbSet<BankBranches> BankBranches { get; set; }  
        public DbSet<Banks> Banks { get; set; }  
        public DbSet<BaseValueTypes> BaseValueTypes { get; set; }  
        public DbSet<BaseValues> BaseValues { get; set; }  
        public DbSet<BomItems> BomItems { get; set; }  
        public DbSet<BomValueHeaders> BomValueHeaders { get; set; }  
        public DbSet<BomValues> BomValues { get; set; }  
        public DbSet<Boms> Boms { get; set; }  
        public DbSet<Branches> Branches { get; set; }  
        public DbSet<BusinessRuleConditions> BusinessRuleConditions { get; set; }  
        public DbSet<BusinessRules> BusinessRules { get; set; }  
        public DbSet<CategoryPropertyMappings> CategoryPropertyMappings { get; set; }  
        public DbSet<ChequeSheets> ChequeSheets { get; set; }  
        public DbSet<CodeAccountHeadGroup> CodeAccountHeadGroup { get; set; }  
        public DbSet<CodeAutoVoucherViews> CodeAutoVoucherViews { get; set; }  
        public DbSet<CodeRowDescription> CodeRowDescription { get; set; }  
        public DbSet<CodeVoucherExtendType> CodeVoucherExtendType { get; set; }  
        public DbSet<CodeVoucherGroups> CodeVoucherGroups { get; set; }  
        public DbSet<CodingTemplateProperties> CodingTemplateProperties { get; set; }  
        public DbSet<CodingTemplates> CodingTemplates { get; set; }  
        public DbSet<Commodities> Commodities { get; set; }  
        public DbSet<CommodityCategories> CommodityCategories { get; set; }  
        public DbSet<CommodityCategoryMeasures> CommodityCategoryMeasures { get; set; }  
        public DbSet<CommodityCategoryProperties> CommodityCategoryProperties { get; set; }  
        public DbSet<CommodityCategoryPropertyItems> CommodityCategoryPropertyItems { get; set; }  
        public DbSet<CommodityMeasures> CommodityMeasures { get; set; }  
        public DbSet<CommodityPropertyValues> CommodityPropertyValues { get; set; }  
        public DbSet<CompanyInformations> CompanyInformations { get; set; }  
        public DbSet<CorrectionRequests> CorrectionRequests { get; set; }  
        public DbSet<CountryDivisions> CountryDivisions { get; set; }  
        public DbSet<CountryDivisionsMain> CountryDivisionsMain { get; set; }  
        public DbSet<DataBaseMetadata> DataBaseMetadata { get; set; }  
        public DbSet<DataDictionary> DataDictionary { get; set; }  
        public DbSet<DataDictionaryold> DataDictionaryold { get; set; }  
        public DbSet<DocumentAttachments> DocumentAttachments { get; set; }  
        public DbSet<DocumentHeadExtend> DocumentHeadExtend { get; set; }  
        public DbSet<DocumentHeadPurchaseOrders> DocumentHeadPurchaseOrders { get; set; }  
        public DbSet<DocumentHeads> DocumentHeads { get; set; }  
        public DbSet<DocumentItems> DocumentItems { get; set; }  
        public DbSet<DocumentItemsBom> DocumentItemsBom { get; set; }  
        public DbSet<DocumentItemsMeasurValues> DocumentItemsMeasurValues { get; set; }  
        public DbSet<DocumentNumberingFormats> DocumentNumberingFormats { get; set; }  
        public DbSet<DocumentPayments> DocumentPayments { get; set; }  
        public DbSet<Documents> Documents { get; set; }  
        public DbSet<Employees> Employees { get; set; }  
        public DbSet<Events> Events { get; set; }  
        public DbSet<FinancialRequestAttachments> FinancialRequestAttachments { get; set; }  
        public DbSet<FinancialRequestDetails> FinancialRequestDetails { get; set; }  
        public DbSet<FinancialRequestDocuments> FinancialRequestDocuments { get; set; }  
        public DbSet<FinancialRequestPartial> FinancialRequestPartial { get; set; }  
        public DbSet<FinancialRequestVerifiers> FinancialRequestVerifiers { get; set; }  
        public DbSet<FinancialRequest> FinancialRequests { get; set; }  
        public DbSet<FreightPayList> FreightPayList { get; set; }  
        public DbSet<FreightPays> FreightPays { get; set; }  
        public DbSet<HelpAttachment> HelpAttachment { get; set; }  
        public DbSet<HelpData> HelpData { get; set; }  
        public DbSet<Holidays> Holidays { get; set; }  
        public DbSet<Lanes> Lanes { get; set; }  
        public DbSet<Languages> Languages { get; set; }  
        public DbSet<MeasureUnitConversions> MeasureUnitConversions { get; set; }  
        public DbSet<MeasureUnits> MeasureUnits { get; set; }  
        public DbSet<MenuItems> MenuItems { get; set; }  
        public DbSet<MoadianInvoiceDetails> MoadianInvoiceDetails { get; set; }  
        public DbSet<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; }  
        public DbSet<OtherFrieght> OtherFrieght { get; set; }  
        public DbSet<PayCheque> PayCheque { get; set; }  
        public DbSet<PerformerCondition> PerformerCondition { get; set; }  
        public DbSet<Performers> Performers { get; set; }  
        public DbSet<Permissions> Permissions { get; set; }  
        public DbSet<PersonAddress> PersonAddress { get; set; }  
        public DbSet<PersonBankAccounts> PersonBankAccounts { get; set; }  
        public DbSet<PersonBankAcounts> PersonBankAcounts { get; set; }  
        public DbSet<PersonFingerprint> PersonFingerprint { get; set; }  
        public DbSet<PersonPhones> PersonPhones { get; set; }  
        public DbSet<PersonRelAttachments> PersonRelAttachments { get; set; }  
        public DbSet<Persons> Persons { get; set; }  
        public DbSet<Pools> Pools { get; set; }  
        public DbSet<Positions> Positions { get; set; }  
        public DbSet<Processes> Processes { get; set; }  
        public DbSet<Requests> Requests { get; set; }  
        public DbSet<RequiredPermission> RequiredPermission { get; set; }  
        public DbSet<RolePermissions> RolePermissions { get; set; }  
        public DbSet<Roles> Roles { get; set; }  
        public DbSet<RunningWorkflow> RunningWorkflow { get; set; }  
        public DbSet<RuntimeActivities> RuntimeActivities { get; set; }  
        public DbSet<RuntimeProcess> RuntimeProcess { get; set; }  
        public DbSet<RuntimeWorkflow> RuntimeWorkflow { get; set; }  
        public DbSet<RuntimeWorkflowParameter> RuntimeWorkflowParameter { get; set; }  
        public DbSet<ServiceActivities> ServiceActivities { get; set; }  
        public DbSet<Shapes> Shapes { get; set; }  
        public DbSet<ShiftInfo> ShiftInfo { get; set; }  
        public DbSet<Signers> Signers { get; set; }  
        public DbSet<TaskComments> TaskComments { get; set; }  
        public DbSet<TaskWorkHistory> TaskWorkHistory { get; set; }  
        public DbSet<Tasks> Tasks { get; set; }  
        public DbSet<TransitionShapes> TransitionShapes { get; set; }  
        public DbSet<Transitions> Transitions { get; set; }  
        public DbSet<UIFormControls> UIFormControls { get; set; }  
        public DbSet<UIFormVariables> UIFormVariables { get; set; }  
        public DbSet<UIForms> UIForms { get; set; }  
        public DbSet<UIGrid> UIGrid { get; set; }  
        public DbSet<UIGridColumns> UIGridColumns { get; set; }  
        public DbSet<UnitPositions> UnitPositions { get; set; }  
        public DbSet<Units> Units { get; set; }  
        public DbSet<UserCompany> UserCompany { get; set; }  
        public DbSet<UserRoles> UserRoles { get; set; }  
        public DbSet<UserSetting> UserSetting { get; set; }  
        public DbSet<UserYear> UserYear { get; set; }  
        public DbSet<Users> Users { get; set; }  
        public DbSet<ValidationMessages> ValidationMessages { get; set; }  
        public DbSet<VerificationCodes> VerificationCodes { get; set; }  
        public DbSet<VoucherAttachments> VoucherAttachments { get; set; }  
        public DbSet<VoucherDetailAttachments> VoucherDetailAttachments { get; set; }  
        public DbSet<VouchersDetail> VouchersDetail { get; set; }  
        public DbSet<VouchersHead> VouchersHead { get; set; }  
        public DbSet<Workflows> Workflows { get; set; }  
        public DbSet<Years> Years { get; set; }
        public DbSet<FinancialRequestLogs> FinancialRequestLogs { get; set; }

        public DbSet<_CommoditiesCardex> _CommoditiesCardex { get; set; }  
        public DbSet<_FileAccessRole> _FileAccessRole { get; set; }  
        public DbSet<_FileAccessRoleUsers> _FileAccessRoleUsers { get; set; }

        public DbSet<Payables_ChequeBooks> Payables_ChequeBooks { get; set; }
        public DbSet<Payables_ChequeBooksSheets> Payables_ChequeBooksSheets { get; set; }
        public DbSet<Payables_Documents> Payables_Documents { get; set; }
        public DbSet<Payables_DocumentsAccounts> Payables_DocumentsAccounts { get; set; }
        public DbSet<Payables_DocumentsOperations> Payables_DocumentsOperations { get; set; }
        public DbSet<Payables_ChecqueBooks_View> Payables_CheckBooks_View { get; set; }
        public DbSet<Payables_DocumentsPayOrders> Payables_DocumentsPayOrders { get; set; }
        public DbSet<Payables_Documents_View> Payables_Documents_View { get; set; }
        public DbSet<Payables_DocumentsAccounts_View> Payables_DocumentsAccounts_View { get; set; }
        public DbSet<Payables_DocumentsOperations_View> Payables_DocumentsOperations_View { get; set; }
        public DbSet<Payables_DocumentOperations_Chain> Payables_DocumentOperations_Chain { get; set; }
        public DbSet<Banks_View> Banks_View { get; set; }
        public DbSet<BankAccountReferences_View> BankAccountReferences_View { get; set; }
        public DbSet<BankAccounts_View> BankAccounts_View { get; set; }
        public DbSet<Payables_ChecqueBooks_View> Payables_ChecqueBooks_View { get; set; }
        public DbSet<Payables_ChequeBooksSheets_View> Payables_ChequeBooksSheets_View { get; set; }
        public DbSet<Payables_PayOrders_View> Payables_PayOrders_View { get; set; }
        public DbSet<Payables_PayRequests_View> Payables_PayRequests_View { get; set; }
        public DbSet<Payables_PayTypes_View> Payables_PayTypes_View { get; set; }
        public DbSet<Payables_Subjects_View> Payables_Subjects_View { get; set; }
        public DbSet<Payables_DocumentsPayOrders_View> Payables_DocumentsPayOrders_View { get; set; }
        public DbSet<BankBranches_View> BankBranches_View { get; set; }
        public DbSet<CountryDivisions_View> CountryDivisions_View { get; set; }
        public DbSet<BankAccountTypes_View> BankAccountTypes_View { get; set; }
        public DbSet<Payables_DocumentOperations_Chain_View> Payables_DocumentOperations_Chain_View { get; set; }
        public DbSet<Payables_ChequeOperations_View> Payables_ChequeOperations_View { get; set; }

        public DbSet<ResourceExpense_View> ResourceExpense_View { get; set; }
        public DbSet<MonthlyForecast> MonthlyForecast { get; set; }
        public DbSet<DailyForecast> DailyForecast { get; set; }
        public DbSet<MonthlyForecast_View> MonthlyForecast_View { get; set; }

        public DbSet<BankTypes_View> BankTypes_View { get; set; }
        
        public   DbSet<BankTransactionHead> BankTransactionHead { get; set; }
        public   DbSet<BankTransactions> BankTransaction { get; set; }

        public DbSet<CurrencyTypes_View> CurrencyTypes_View { get; set; }
        public DbSet<ChequeTypes_View> ChequeTypes_View { get; set; }

        public DbSet<DateConversion> DateConversion { get; set; }
    }
}
