using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    [HasUniqueIndex]
    public partial class BaseValue : HierarchicalBaseEntity
    {

        public int BaseValueTypeId { get; set; } = default!;


        /// <summary>
        /// شناسه
        /// </summary>
        [UniqueIndex]
        public string Code { get; set; } = default!;


        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// نام یکتا
        /// </summary>
        [UniqueIndex]
        public string UniqueName { get; set; } = default!;

        /// <summary>
        /// مقدار
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// ترتیب آرتیکل سند حسابداری
        /// </summary>
        public int OrderIndex { get; set; } = default!;

        /// <summary>
        /// آیا فقط قابل خواندن است؟
        /// </summary>
        public bool IsReadOnly { get; set; } = default!;


        public virtual BaseValueType BaseValueType { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual ICollection<Attachment> Attachments { get; set; } = default!;
        public virtual ICollection<CommodityCategoryProperty> CommodityCategoryProperties { get; set; } = default!;
        public virtual ICollection<Commodity> CommodityMeasures { get; set; } = default!;
        public virtual ICollection<Commodity> CommodityPricingTypeNavigations { get; set; } = default!;
        public virtual ICollection<CommodityProperty> CommodityProperties { get; set; } = default!;
        public virtual ICollection<Commodity> CommodityTypes { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeadFormTypes { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeadInvoiceTypes { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeadPaymentTypes { get; set; } = default!;
        public virtual ICollection<DocumentHead> DocumentHeadTypes { get; set; } = default!;
        public virtual ICollection<Language> DefaultCurrencyLanguages { get; set; } = default!;
        public virtual ICollection<Language> DirectionLanguages { get; set; } = default!;
        public virtual ICollection<Bank> Banks { get; set; } = default!;
        public virtual ICollection<PersonBankAccount> PersonBankAccounts { get; set; } = default!;
        public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
        public virtual ICollection<PersonPhone> PersonPhones { get; set; } = default!;
        public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
        public virtual ICollection<Person> PersonGenderBases { get; set; } = default!;
        public virtual ICollection<Person> PersonGovernmentalBases { get; set; } = default!;
        public virtual ICollection<Person> PersonLegalBases { get; set; } = default!;
        public virtual ICollection<User> Users { get; set; } = default!;

        public virtual ICollection<Customer> Customers { get; set; } = default!;

    }
}
