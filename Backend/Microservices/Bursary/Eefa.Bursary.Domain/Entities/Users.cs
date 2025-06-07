using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities
{
    /// <summary>
   // &#1705;&#1575;&#1585;&#1576;&#1585;&#1575;&#1606;  
    /// </summary>
    public partial class Users : BaseEntity
    {
        public Users()
        {
            AccountHeadCreatedBies = new HashSet<AccountHead>();
            AccountHeadModifiedBies = new HashSet<AccountHead>();
            AccountHeadRelReferenceGroupCreatedBies = new HashSet<AccountHeadRelReferenceGroup>();
            AccountHeadRelReferenceGroupModifiedBies = new HashSet<AccountHeadRelReferenceGroup>();
            AccountReferencesCreatedBies = new HashSet<AccountReferences>();
            AccountReferencesModifiedBies = new HashSet<AccountReferences>();
            AccountReferencesRelReferencesGroupsCreatedBies = new HashSet<AccountReferencesRelReferencesGroups>();
            AccountReferencesRelReferencesGroupsModifiedBies = new HashSet<AccountReferencesRelReferencesGroups>();
            BankAccountCardexCreatedBies = new HashSet<BankAccountCardex>();
            BankAccountCardexModifiedBies = new HashSet<BankAccountCardex>();
            BankAccountsCreatedBies = new HashSet<BankAccounts>();
            BankAccountsModifiedBies = new HashSet<BankAccounts>();
            BankBranchesCreatedBies = new HashSet<BankBranches>();
            BankBranchesModifiedBies = new HashSet<BankBranches>();
            BanksCreatedBies = new HashSet<Banks>();
            BanksModifiedBies = new HashSet<Banks>();
            BaseValueTypesCreatedBies = new HashSet<BaseValueTypes>();
            BaseValueTypesModifiedBies = new HashSet<BaseValueTypes>();
            BomItemsCreatedBies = new HashSet<BomItems>();
            BomItemsModifiedBies = new HashSet<BomItems>();
            BomValueHeadersCreatedBies = new HashSet<BomValueHeaders>();
            BomValueHeadersModifiedBies = new HashSet<BomValueHeaders>();
            BomValuesCreatedBies = new HashSet<BomValues>();
            BomValuesModifiedBies = new HashSet<BomValues>();
            CategoryPropertyMappingsCreatedBies = new HashSet<CategoryPropertyMappings>();
            CategoryPropertyMappingsModifiedBies = new HashSet<CategoryPropertyMappings>();
            ChequeSheetsCreatedBies = new HashSet<ChequeSheets>();
            ChequeSheetsModifiedBies = new HashSet<ChequeSheets>();
            CodeAccountHeadGroupCreatedBies = new HashSet<CodeAccountHeadGroup>();
            CodeAccountHeadGroupModifiedBies = new HashSet<CodeAccountHeadGroup>();
            CodeAutoVoucherViewsCreatedBies = new HashSet<CodeAutoVoucherViews>();
            CodeAutoVoucherViewsModifiedBies = new HashSet<CodeAutoVoucherViews>();
            CodeRowDescriptionCreatedBies = new HashSet<CodeRowDescription>();
            CodeRowDescriptionModifiedBies = new HashSet<CodeRowDescription>();
            CommoditiesCreatedBies = new HashSet<Commodities>();
            CommoditiesModifiedBies = new HashSet<Commodities>();
            CommodityCategoriesCreatedBies = new HashSet<CommodityCategories>();
            CommodityCategoriesModifiedBies = new HashSet<CommodityCategories>();
            CommodityCategoryMeasures = new HashSet<CommodityCategoryMeasures>();
            CommodityCategoryPropertiesCreatedBies = new HashSet<CommodityCategoryProperties>();
            CommodityCategoryPropertiesModifiedBies = new HashSet<CommodityCategoryProperties>();
            CommodityCategoryPropertyItemsCreatedBies = new HashSet<CommodityCategoryPropertyItems>();
            CommodityCategoryPropertyItemsModifiedBies = new HashSet<CommodityCategoryPropertyItems>();
            CommodityMeasures = new HashSet<CommodityMeasures>();
            CommodityPropertyValuesCreatedBies = new HashSet<CommodityPropertyValues>();
            CommodityPropertyValuesModifiedBies = new HashSet<CommodityPropertyValues>();
            CountryDivisionsCreatedBies = new HashSet<CountryDivisions>();
            CountryDivisionsModifiedBies = new HashSet<CountryDivisions>();
            DocumentAttachments = new HashSet<DocumentAttachments>();
            DocumentHeadExtendCreatedBies = new HashSet<DocumentHeadExtend>();
            DocumentHeadExtendModifiedBies = new HashSet<DocumentHeadExtend>();
            DocumentHeadsCreatedBies = new HashSet<DocumentHeads>();
            DocumentHeadsModifiedBies = new HashSet<DocumentHeads>();
            DocumentItemsBomCreatedBies = new HashSet<DocumentItemsBom>();
            DocumentItemsBomModifiedBies = new HashSet<DocumentItemsBom>();
            DocumentItemsCreatedBies = new HashSet<DocumentItems>();
            DocumentItemsModifiedBies = new HashSet<DocumentItems>();
            DocumentNumberingFormatsCreatedBies = new HashSet<DocumentNumberingFormats>();
            DocumentNumberingFormatsModifiedBies = new HashSet<DocumentNumberingFormats>();
            DocumentPaymentsCreatedBies = new HashSet<DocumentPayments>();
            DocumentPaymentsModifiedBies = new HashSet<DocumentPayments>();
            FinancialRequestAttachmentsCreatedBies = new HashSet<FinancialRequestAttachments>();
            FinancialRequestAttachmentsModifiedBies = new HashSet<FinancialRequestAttachments>();
            FinancialRequestDetailsCreatedBies = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsModifiedBies = new HashSet<FinancialRequestDetails>();
            FinancialRequestDocumentsCreatedBies = new HashSet<FinancialRequestDocuments>();
            FinancialRequestDocumentsModifiedBies = new HashSet<FinancialRequestDocuments>();
            FinancialRequestPartialCreatedBies = new HashSet<FinancialRequestPartial>();
            FinancialRequestPartialModifiedBies = new HashSet<FinancialRequestPartial>();
            FinancialRequestVerifiersCreatedBies = new HashSet<FinancialRequestVerifiers>();
            FinancialRequestVerifiersModifiedBies = new HashSet<FinancialRequestVerifiers>();
            FinancialRequestsCreatedBies = new HashSet<FinancialRequest>();
            FinancialRequestsModifiedBies = new HashSet<FinancialRequest>();
            FreightPayLists = new HashSet<FreightPayList>();
            HolidaysCreatedBies = new HashSet<Holidays>();
            HolidaysModifiedBies = new HashSet<Holidays>();
            MeasureUnitConversionsCreatedBies = new HashSet<MeasureUnitConversions>();
            MeasureUnitConversionsModifiedBies = new HashSet<MeasureUnitConversions>();
            MeasureUnitsCreatedBies = new HashSet<MeasureUnits>();
            MeasureUnitsModifiedBies = new HashSet<MeasureUnits>();
            MoadianInvoiceHeaders = new HashSet<MoadianInvoiceHeaders>();
            PayChequeCreatedBies = new HashSet<PayCheque>();
            PayChequeModifiedBies = new HashSet<PayCheque>();
            PermissionsCreatedBies = new HashSet<Permissions>();
            PermissionsModifiedBies = new HashSet<Permissions>();
            PersonAddresses = new HashSet<PersonAddress>();
            PersonBankAccounts = new HashSet<PersonBankAccounts>();
            PersonBankAcountsCreatedBies = new HashSet<PersonBankAcounts>();
            PersonBankAcountsModifiedBies = new HashSet<PersonBankAcounts>();
            PersonPhones = new HashSet<PersonPhones>();
            PersonRelAttachmentsCreatedBies = new HashSet<PersonRelAttachments>();
            PersonRelAttachmentsModifiedBies = new HashSet<PersonRelAttachments>();
            PositionsCreatedBies = new HashSet<Positions>();
            PositionsModifiedBies = new HashSet<Positions>();
            RequiredPermissionCreatedBies = new HashSet<RequiredPermission>();
            RequiredPermissionModifiedBies = new HashSet<RequiredPermission>();
            ShiftInfoCreatedBies = new HashSet<ShiftInfo>();
            ShiftInfoModifiedBies = new HashSet<ShiftInfo>();
            UnitPositionsCreatedBies = new HashSet<UnitPositions>();
            UnitPositionsModifiedBies = new HashSet<UnitPositions>();
            UnitsCreatedBies = new HashSet<Units>();
            UnitsModifiedBies = new HashSet<Units>();
            UserCompanyCreatedBies = new HashSet<UserCompany>();
            UserCompanyModifiedBies = new HashSet<UserCompany>();
            UserCompanyUsers = new HashSet<UserCompany>();
            UserRolesCreatedBies = new HashSet<UserRoles>();
            UserRolesUsers = new HashSet<UserRoles>();
            UserSettingCreatedBies = new HashSet<UserSetting>();
            UserSettingModifiedBies = new HashSet<UserSetting>();
            UserSettingUsers = new HashSet<UserSetting>();
            YearsCreatedBies = new HashSet<Years>();
            YearsModifiedBies = new HashSet<Years>();
            _FileAccessRoleUsers = new HashSet<_FileAccessRoleUsers>();
        }


        /// <summary>
        //شناسه
        /// </summary>


        /// <summary>
        //کد شخص
        /// </summary>
        public int PersonId { get; set; } = default!;

        /// <summary>
//نام کاربری
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
//آیا قفل شده است؟
        /// </summary>
        public bool IsBlocked { get; set; } = default!;

        /// <summary>
//علت قفل شدن
        /// </summary>
        public int? BlockedReasonBaseId { get; set; }

        /// <summary>
//رمز یکبار مصرف
        /// </summary>
        public string? OneTimePassword { get; set; }

        /// <summary>
//رمز
        /// </summary>
        public string Password { get; set; } = default!;
        public DateTime PasswordExpiryDate { get; set; } = default!;

        /// <summary>
//دفعات ورود ناموفق
        /// </summary>
        public int FailedCount { get; set; } = default!;

        /// <summary>
//آخرین زمان آنلاین بودن
        /// </summary>
        public DateTime? LastOnlineTime { get; set; }

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


        public virtual Persons Person { get; set; } = default!;
        public virtual ICollection<AccountHead> AccountHeadCreatedBies { get; set; } = default!;
        public virtual ICollection<AccountHead> AccountHeadModifiedBies { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroupCreatedBies { get; set; } = default!;
        public virtual ICollection<AccountHeadRelReferenceGroup> AccountHeadRelReferenceGroupModifiedBies { get; set; } = default!;
        public virtual ICollection<AccountReferences> AccountReferencesCreatedBies { get; set; } = default!;
        public virtual ICollection<AccountReferences> AccountReferencesModifiedBies { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroupsCreatedBies { get; set; } = default!;
        public virtual ICollection<AccountReferencesRelReferencesGroups> AccountReferencesRelReferencesGroupsModifiedBies { get; set; } = default!;
        public virtual ICollection<BankAccountCardex> BankAccountCardexCreatedBies { get; set; } = default!;
        public virtual ICollection<BankAccountCardex> BankAccountCardexModifiedBies { get; set; } = default!;
        public virtual ICollection<BankAccounts> BankAccountsCreatedBies { get; set; } = default!;
        public virtual ICollection<BankAccounts> BankAccountsModifiedBies { get; set; } = default!;
        public virtual ICollection<BankBranches> BankBranchesCreatedBies { get; set; } = default!;
        public virtual ICollection<BankBranches> BankBranchesModifiedBies { get; set; } = default!;
        public virtual ICollection<Banks> BanksCreatedBies { get; set; } = default!;
        public virtual ICollection<Banks> BanksModifiedBies { get; set; } = default!;
        public virtual ICollection<BaseValueTypes> BaseValueTypesCreatedBies { get; set; } = default!;
        public virtual ICollection<BaseValueTypes> BaseValueTypesModifiedBies { get; set; } = default!;
        public virtual ICollection<BomItems> BomItemsCreatedBies { get; set; } = default!;
        public virtual ICollection<BomItems> BomItemsModifiedBies { get; set; } = default!;
        public virtual ICollection<BomValueHeaders> BomValueHeadersCreatedBies { get; set; } = default!;
        public virtual ICollection<BomValueHeaders> BomValueHeadersModifiedBies { get; set; } = default!;
        public virtual ICollection<BomValues> BomValuesCreatedBies { get; set; } = default!;
        public virtual ICollection<BomValues> BomValuesModifiedBies { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMappings> CategoryPropertyMappingsCreatedBies { get; set; } = default!;
        public virtual ICollection<CategoryPropertyMappings> CategoryPropertyMappingsModifiedBies { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsCreatedBies { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheetsModifiedBies { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroupCreatedBies { get; set; } = default!;
        public virtual ICollection<CodeAccountHeadGroup> CodeAccountHeadGroupModifiedBies { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherViews> CodeAutoVoucherViewsCreatedBies { get; set; } = default!;
        public virtual ICollection<CodeAutoVoucherViews> CodeAutoVoucherViewsModifiedBies { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptionCreatedBies { get; set; } = default!;
        public virtual ICollection<CodeRowDescription> CodeRowDescriptionModifiedBies { get; set; } = default!;
        public virtual ICollection<Commodities> CommoditiesCreatedBies { get; set; } = default!;
        public virtual ICollection<Commodities> CommoditiesModifiedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategories> CommodityCategoriesCreatedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategories> CommodityCategoriesModifiedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategoryMeasures> CommodityCategoryMeasures { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryPropertiesCreatedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryPropertiesModifiedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItems> CommodityCategoryPropertyItemsCreatedBies { get; set; } = default!;
        public virtual ICollection<CommodityCategoryPropertyItems> CommodityCategoryPropertyItemsModifiedBies { get; set; } = default!;
        public virtual ICollection<CommodityMeasures> CommodityMeasures { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValuesCreatedBies { get; set; } = default!;
        public virtual ICollection<CommodityPropertyValues> CommodityPropertyValuesModifiedBies { get; set; } = default!;
        public virtual ICollection<CountryDivisions> CountryDivisionsCreatedBies { get; set; } = default!;
        public virtual ICollection<CountryDivisions> CountryDivisionsModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentAttachments> DocumentAttachments { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtendCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentHeadExtend> DocumentHeadExtendModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentHeads> DocumentHeadsModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBomCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBomModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItemsCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentItems> DocumentItemsModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentNumberingFormats> DocumentNumberingFormatsCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentNumberingFormats> DocumentNumberingFormatsModifiedBies { get; set; } = default!;
        public virtual ICollection<DocumentPayments> DocumentPaymentsCreatedBies { get; set; } = default!;
        public virtual ICollection<DocumentPayments> DocumentPaymentsModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachmentsCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestAttachments> FinancialRequestAttachmentsModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocumentsCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocumentsModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestPartial> FinancialRequestPartialModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestVerifiers> FinancialRequestVerifiersCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequestVerifiers> FinancialRequestVerifiersModifiedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequestsCreatedBies { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequestsModifiedBies { get; set; } = default!;
        public virtual ICollection<FreightPayList> FreightPayLists { get; set; } = default!;
        public virtual ICollection<Holidays> HolidaysCreatedBies { get; set; } = default!;
        public virtual ICollection<Holidays> HolidaysModifiedBies { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversions> MeasureUnitConversionsCreatedBies { get; set; } = default!;
        public virtual ICollection<MeasureUnitConversions> MeasureUnitConversionsModifiedBies { get; set; } = default!;
        public virtual ICollection<MeasureUnits> MeasureUnitsCreatedBies { get; set; } = default!;
        public virtual ICollection<MeasureUnits> MeasureUnitsModifiedBies { get; set; } = default!;
        public virtual ICollection<MoadianInvoiceHeaders> MoadianInvoiceHeaders { get; set; } = default!;
        public virtual ICollection<PayCheque> PayChequeCreatedBies { get; set; } = default!;
        public virtual ICollection<PayCheque> PayChequeModifiedBies { get; set; } = default!;
        public virtual ICollection<Permissions> PermissionsCreatedBies { get; set; } = default!;
        public virtual ICollection<Permissions> PermissionsModifiedBies { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonBankAccounts> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcountsCreatedBies { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcountsModifiedBies { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<PersonRelAttachments> PersonRelAttachmentsCreatedBies { get; set; } = default!;
        public virtual ICollection<PersonRelAttachments> PersonRelAttachmentsModifiedBies { get; set; } = default!;
        public virtual ICollection<Positions> PositionsCreatedBies { get; set; } = default!;
        public virtual ICollection<Positions> PositionsModifiedBies { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionCreatedBies { get; set; } = default!;
        public virtual ICollection<RequiredPermission> RequiredPermissionModifiedBies { get; set; } = default!;
        public virtual ICollection<ShiftInfo> ShiftInfoCreatedBies { get; set; } = default!;
        public virtual ICollection<ShiftInfo> ShiftInfoModifiedBies { get; set; } = default!;
        public virtual ICollection<UnitPositions> UnitPositionsCreatedBies { get; set; } = default!;
        public virtual ICollection<UnitPositions> UnitPositionsModifiedBies { get; set; } = default!;
        public virtual ICollection<Units> UnitsCreatedBies { get; set; } = default!;
        public virtual ICollection<Units> UnitsModifiedBies { get; set; } = default!;

        public virtual ICollection<BankTransactionHead> BankTransactionHeadsCreatedBies { get; set; } = default!;
        public virtual ICollection<BankTransactionHead> BankTransactionHeadsModifiedBies { get; set; } = default!;

        public virtual ICollection<BankTransactions> BankTransactionsCreatedBies { get; set; } = default!;
        public virtual ICollection<BankTransactions> BankTransactionsModifiedBies { get; set; } = default!;

        public virtual ICollection<UserCompany> UserCompanyCreatedBies { get; set; } = default!;
        public virtual ICollection<UserCompany> UserCompanyModifiedBies { get; set; } = default!;
        public virtual ICollection<UserCompany> UserCompanyUsers { get; set; } = default!;
        public virtual ICollection<UserRoles> UserRolesCreatedBies { get; set; } = default!;
        public virtual ICollection<UserRoles> UserRolesUsers { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingCreatedBies { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingModifiedBies { get; set; } = default!;
        public virtual ICollection<UserSetting> UserSettingUsers { get; set; } = default!;
        public virtual ICollection<Years> YearsCreatedBies { get; set; } = default!;
        public virtual ICollection<Years> YearsModifiedBies { get; set; } = default!;
        public virtual ICollection<_FileAccessRoleUsers> _FileAccessRoleUsers { get; set; } = default!;
        public virtual ICollection<FinancialRequestLogs> FinancialLogsCreatedBies { get; set; } = default!;

    }
}
