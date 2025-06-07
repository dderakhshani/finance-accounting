using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Definitions
{
    /// <summary>
   // &#1575;&#1591;&#1604;&#1575;&#1593;&#1575;&#1578; &#1662;&#1575;&#1740;&#1607; 
    /// </summary>
    public partial class BaseValues : BaseEntity
    {
        public BaseValues()
        {
            Attachments = new HashSet<Attachment>();
            BankAccounts = new HashSet<BankAccounts>();
            Banks = new HashSet<Banks>();
            ChequeSheets = new HashSet<ChequeSheets>();
            CommodityCategoryProperties = new HashSet<CommodityCategoryProperties>();
            DocumentItemsBoms = new HashSet<DocumentItemsBom>();
            DocumentNumberingFormats = new HashSet<DocumentNumberingFormats>();
            FinancialRequestDetailsCurrencyTypeBases = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsDocumentTypeBases = new HashSet<FinancialRequestDetails>();
            FinancialRequestDetailsFinancialReferenceTypeBases = new HashSet<FinancialRequestDetails>();
            FinancialRequestDocuments = new HashSet<FinancialRequestDocuments>();
            FinancialRequestsFinancialStatusBases = new HashSet<FinancialRequest>();
            FinancialRequestsPaymentTypeBases = new HashSet<FinancialRequest>();
            FreightPaysSources = new HashSet<FreightPays>();
            FreightPaysVehicleTypes = new HashSet<FreightPays>();
            LanguagesDefaultCurrencyBases = new HashSet<Languages>();
            LanguagesDirectionBases = new HashSet<Languages>();
            PayCheques = new HashSet<PayCheque>();
            PersonAddresses = new HashSet<PersonAddress>();
            PersonBankAccounts = new HashSet<PersonBankAccounts>();
            PersonBankAcounts = new HashSet<PersonBankAcounts>();
            PersonFingerprints = new HashSet<PersonFingerprint>();
            PersonPhones = new HashSet<PersonPhones>();
            PersonsGenderBases = new HashSet<Persons>();
            PersonsGovernmentalBases = new HashSet<Persons>();
            PersonsLegalBases = new HashSet<Persons>();
            VouchersDetails = new HashSet<VouchersDetail>();
            Payables_DocumentsCurrencyTypeBase = new HashSet<Payables_Documents>();
            Payables_DocumentsOperations = new HashSet<Payables_DocumentsOperations>();
            Payables_DocumentsPayType = new HashSet<Payables_Documents>();
            Payables_DocumentsSubject = new HashSet<Payables_Documents>();

        }


        /// <summary>
        //شناسه
        /// </summary>


        /// <summary>
        //کد نوع مقدار
        /// </summary>
        public int BaseValueTypeId { get; set; } = default!;

        /// <summary>
//کد والد
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
//کد
        /// </summary>
        public string Code { get; set; } = default!;

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
//مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
//ترتیب نمایش 
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
//آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;

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
         

        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<BankAccounts> BankAccounts { get; set; } = default!;
        public virtual ICollection<Banks> Banks { get; set; } = default!;
        public virtual ICollection<ChequeSheets> ChequeSheets { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperties> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<DocumentItemsBom> DocumentItemsBoms { get; set; } = default!;
        public virtual ICollection<DocumentNumberingFormats> DocumentNumberingFormats { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsCurrencyTypeBases { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsDocumentTypeBases { get; set; } = default!;
        public virtual ICollection<FinancialRequestDetails> FinancialRequestDetailsFinancialReferenceTypeBases { get; set; } = default!;
        public virtual ICollection<FinancialRequestDocuments> FinancialRequestDocuments { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequestsFinancialStatusBases { get; set; } = default!;
        public virtual ICollection<FinancialRequest> FinancialRequestsPaymentTypeBases { get; set; } = default!;
        public virtual ICollection<FreightPays> FreightPaysSources { get; set; } = default!;
        public virtual ICollection<FreightPays> FreightPaysVehicleTypes { get; set; } = default!;
        public virtual ICollection<Languages> LanguagesDefaultCurrencyBases { get; set; } = default!;
        public virtual ICollection<Languages> LanguagesDirectionBases { get; set; } = default!;
        public virtual ICollection<PayCheque> PayCheques { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonBankAccounts> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonBankAcounts> PersonBankAcounts { get; set; } = default!;
        public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
        public virtual ICollection<PersonPhones> PersonPhones { get; set; } = default!;
        public virtual ICollection<Persons> PersonsGenderBases { get; set; } = default!;
        public virtual ICollection<Persons> PersonsGovernmentalBases { get; set; } = default!;
        public virtual ICollection<Persons> PersonsLegalBases { get; set; } = default!;
        public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
        public virtual ICollection<Payables_Documents> Payables_DocumentsCurrencyTypeBase { get; set; }
        public virtual ICollection<Payables_DocumentsOperations> Payables_DocumentsOperations { get; set; }
        public virtual ICollection<Payables_Documents> Payables_DocumentsPayType { get; set; }
        public virtual ICollection<Payables_Documents> Payables_DocumentsSubject { get; set; }

    }
}
