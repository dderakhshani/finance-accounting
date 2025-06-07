using System.Collections.Generic;

[HasUniqueIndex]
public partial class BaseValue : HierarchicalAuditableEntity
{
    public int BaseValueTypeId { get; set; } = default!;
    [UniqueIndex]
    public string Code { get; set; } = default!;
    public string Title { get; set; } = default!;
    [UniqueIndex]
    public string UniqueName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
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