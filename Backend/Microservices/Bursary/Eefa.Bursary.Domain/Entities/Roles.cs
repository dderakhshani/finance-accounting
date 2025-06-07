using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1606;&#1602;&#1588; &#1607;&#1575;
    /// </summary>
    public partial class Roles : BaseEntity
    {
        public Roles()
        {
            AccountHeadRelReferenceGroups = new HashSet<AccountHeadRelReferenceGroup>();
            AccountHeads = new HashSet<AccountHead>();
            AccountReferences = new HashSet<AccountReferences>();
            AccountReferencesGroups = new HashSet<AccountReferencesGroups>();
            AccountReferencesRelReferencesGroups = new HashSet<AccountReferencesRelReferencesGroups>();
            AutoVoucherFormulas = new HashSet<AutoVoucherFormula>();
            AutoVoucherIncompleteVouchers = new HashSet<AutoVoucherIncompleteVouchers>();
            AutoVoucherLogs = new HashSet<AutoVoucherLog>();
            AutoVoucherRowsLinks = new HashSet<AutoVoucherRowsLink>();
            BankAccountCardexes = new HashSet<BankAccountCardex>();
            BankAccounts = new HashSet<BankAccounts>();
            BankBranches = new HashSet<BankBranches>();
            Banks = new HashSet<Banks>();
            BaseValueTypes = new HashSet<BaseValueTypes>();
            BomItems = new HashSet<BomItems>();
            BomValueHeaders = new HashSet<BomValueHeaders>();
            BomValues = new HashSet<BomValues>();
            Branches = new HashSet<Branches>();
            CategoryPropertyMappings = new HashSet<CategoryPropertyMappings>();
            ChequeSheets = new HashSet<ChequeSheets>();
            CodeAccountHeadGroups = new HashSet<CodeAccountHeadGroup>();
            CodeAutoVoucherViews = new HashSet<CodeAutoVoucherViews>();
            CodeRowDescriptions = new HashSet<CodeRowDescription>();
            CodeVoucherExtendTypes = new HashSet<CodeVoucherExtendType>();
            CodeVoucherGroups = new HashSet<CodeVoucherGroups>();
            Commodities = new HashSet<Commodities>();
            CommodityCategories = new HashSet<CommodityCategories>();
            CommodityCategoryProperties = new HashSet<CommodityCategoryProperties>();
            CommodityCategoryPropertyItems = new HashSet<CommodityCategoryPropertyItems>();
            CommodityPropertyValues = new HashSet<CommodityPropertyValues>();
            CountryDivisions = new HashSet<CountryDivisions>();
            DocumentAttachments = new HashSet<DocumentAttachments>();
            DocumentHeadExtends = new HashSet<DocumentHeadExtend>();
            DocumentHeads = new HashSet<DocumentHeads>();
            DocumentItems = new HashSet<DocumentItems>();
            DocumentItemsBoms = new HashSet<DocumentItemsBom>();
            DocumentNumberingFormats = new HashSet<DocumentNumberingFormats>();
            DocumentPayments = new HashSet<DocumentPayments>();
            Employees = new HashSet<Employees>();
            FinancialRequestAttachments = new HashSet<FinancialRequestAttachments>();
            FinancialRequestDetails = new HashSet<FinancialRequestDetails>();
            FinancialRequestDocuments = new HashSet<FinancialRequestDocuments>();
            FinancialRequestPartials = new HashSet<FinancialRequestPartial>();
            FinancialRequestVerifiers = new HashSet<FinancialRequestVerifiers>();
            FinancialRequests = new HashSet<FinancialRequest>();
            HelpAttachments = new HashSet<HelpAttachment>();
            HelpDatas = new HashSet<HelpData>();
            Holidays = new HashSet<Holidays>();
            InverseParent = new HashSet<Roles>();
            MeasureUnitConversions = new HashSet<MeasureUnitConversions>();
            MeasureUnits = new HashSet<MeasureUnits>();
            MenuItems = new HashSet<MenuItems>();
            PayCheques = new HashSet<PayCheque>();
            Permissions = new HashSet<Permissions>();
            PersonAddresses = new HashSet<PersonAddress>();
            PersonBankAccounts = new HashSet<PersonBankAccounts>();
            PersonBankAcounts = new HashSet<PersonBankAcounts>();
            PersonPhones = new HashSet<PersonPhones>();
            PersonRelAttachments = new HashSet<PersonRelAttachments>();
            Positions = new HashSet<Positions>();
            RequiredPermissions = new HashSet<RequiredPermission>();
            RolePermissionsOwnerRoles = new HashSet<RolePermissions>();
            RolePermissionsRoles = new HashSet<RolePermissions>();
            ShiftInfoes = new HashSet<ShiftInfo>();
            Signers = new HashSet<Signers>();
            UnitPositions = new HashSet<UnitPositions>();
            Units = new HashSet<Units>();
            UserCompanies = new HashSet<UserCompany>();
            UserRolesOwnerRoles = new HashSet<UserRoles>();
            UserRolesRoles = new HashSet<UserRoles>();
            UserSettings = new HashSet<UserSetting>();
            UserYears = new HashSet<UserYear>();
            ValidationMessages = new HashSet<ValidationMessages>();
            VoucherAttachments = new HashSet<VoucherAttachments>();
            VoucherDetailAttachments = new HashSet<VoucherDetailAttachments>();
            VouchersDetails = new HashSet<VouchersDetail>();
            Years = new HashSet<Years>();
        }


        /// <summary>
//کد
        /// </summary>
         

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد سطح
        /// </summary>
        public string LevelCode { get; set; } = default!;

        /// <summary>
//عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
//نام اختصاصی
        /// </summary>
        public string UniqueName { get; set; } = default!;

        /// <summary>
//توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
//نقش صاحب سند
        /// </summary>
         

        /// <summary>
//ایجاد کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان ایجاد
        /// </summary>
         

        /// <summary>
//اصلاح کننده
        /// </summary>
         

        /// <summary>
//تاریخ و زمان اصلاح
        /// </summary>
         

        /// <summary>
//آیا حذف شده است؟
        /// </summary>
         

        public virtual Roles Parent { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroups { get; set; } = default!;
        public virtual ICollection<AccountHead> AccountHeads { get; set; } = default!;
        public virtual ICollection<AccountReferences> AccountReferences { get; set; } = default!;
        public virtual ICollection<AccountReferencesGroups> AccountReferencesGroups { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroups { get; set; } = default!;
        public virtual ICollection<AutoVoucherFormula> AutoVoucherFormulas { get; set; } = default!;
        public virtual ICollection<AutoVoucherIncompleteVouchers> AutoVoucherIncompleteVouchers { get; set; } = default!;
        public virtual ICollection<AutoVoucherLog> AutoVoucherLogs { get; set; } = default!;
        public virtual ICollection<AutoVoucherRowsLink> AutoVoucherRowsLinks { get; set; } = default!;
        public virtual ICollection<BankAccountCardex> BankAccountCardexes { get; set; } = default!;
        public virtual ICollection<BankAccounts> BankAccounts { get; set; } = default!;
        public virtual ICollection<BankBranches> BankBranches { get; set; } = default!;
        public virtual ICollection<Banks> Banks { get; set; } = default!;
        public virtual ICollection<BaseValueTypes> BaseValueTypes { get; set; } = default!;
        public virtual ICollection<BomItems> BomItems { get; set; } = default!;
        public virtual ICollection<BomValueHeaders> BomValueHeaders { get; set; } = default!;
        public virtual ICollection<BomValues> BomValues { get; set; } = default!;
        public virtual ICollection<Branches> Branches { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMappings> CategoryPropertyMappings { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheets { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroups { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherViews> CodeAutoVoucherViews { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptions { get; set; } = default!;
        public virtual ICollection<CodeVoucherExtendType> CodeVoucherExtendTypes { get; set; } = default!;
        public virtual ICollection<CodeVoucherGroups> CodeVoucherGroups { get; set; } = default!;
        public virtual ICollection<Commodities> Commodities { get; set; } = default!;
        public virtual ICollection<CommodityCategories> CommodityCategories { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItems> CommodityCategoryPropertyItems { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValues { get; set; } = default!;
        public virtual ICollection<CountryDivisions> CountryDivisions { get; set; } = default!;
        public virtual ICollection<DocumentAttachments> DocumentAttachments { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtends { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeads { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItems { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBoms { get; set; } = default!;
        public virtual ICollection<DocumentNumberingFormats> DocumentNumberingFormats { get; set; } = default!;
        public virtual ICollection<DocumentPayments> DocumentPayments { get; set; } = default!;
        public virtual ICollection<Employees> Employees { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachments { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetails { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocuments { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartials { get; set; } = default!;
        public virtual ICollection<FinancialRequestVerifiers> FinancialRequestVerifiers { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequests { get; set; } = default!;
        public virtual ICollection<HelpAttachment> HelpAttachments { get; set; } = default!;
        public virtual ICollection<HelpData> HelpDatas { get; set; } = default!;
        public virtual ICollection<Holidays> Holidays { get; set; } = default!;
        public virtual ICollection<Roles> InverseParent { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversions> MeasureUnitConversions { get; set; } = default!;
        public virtual ICollection<MeasureUnits> MeasureUnits { get; set; } = default!;
        public virtual ICollection<MenuItems> MenuItems { get; set; } = default!;
        public virtual ICollection<PayCheque> PayCheques { get; set; } = default!;
        public virtual ICollection<Permissions> Permissions { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonBankAccounts> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcounts { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<PersonRelAttachments> PersonRelAttachments { get; set; } = default!;
        public virtual ICollection<Positions> Positions { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissions { get; set; } = default!;
        public virtual ICollection<RolePermissions> RolePermissionsOwnerRoles { get; set; } = default!;
        public virtual ICollection<RolePermissions> RolePermissionsRoles { get; set; } = default!;
        public virtual ICollection<ShiftInfo> ShiftInfoes { get; set; } = default!;
        public virtual ICollection<Signers> Signers { get; set; } = default!;
        public virtual ICollection<UnitPositions> UnitPositions { get; set; } = default!;
        public virtual ICollection<Units> Units { get; set; } = default!;
        public virtual ICollection<UserCompany> UserCompanies { get; set; } = default!;
        public virtual ICollection<UserRoles> UserRolesOwnerRoles { get; set; } = default!;
        public virtual ICollection<UserRoles> UserRolesRoles { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettings { get; set; } = default!;
        public virtual ICollection<UserYear> UserYears { get; set; } = default!;
        public virtual ICollection<ValidationMessages> ValidationMessages { get; set; } = default!;
        public virtual ICollection<VoucherAttachments> VoucherAttachments { get; set; } = default!;
        public virtual ICollection<VoucherDetailAttachments> VoucherDetailAttachments { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual ICollection<Years> Years { get; set; } = default!;
        public virtual ICollection<BankTransactions> BankTransactionsOwnerRoles { get; set; } = default!;
        public virtual ICollection<BankTransactionHead> BankTransactionHeadOwnerRoles { get; set; } = default!;

    }
}
