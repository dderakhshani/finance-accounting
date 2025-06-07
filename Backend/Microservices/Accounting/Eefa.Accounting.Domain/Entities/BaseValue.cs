using System.Collections.Generic;


public partial class BaseValue : AuditableEntity
{
    public int BaseValueTypeId { get; set; } = default!;
    public int? ParentId { get; set; }
    public string Code { get; set; } = default!;
    public string LevelCode { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string UniqueName { get; set; } = default!;
    public string Value { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public bool IsReadOnly { get; set; } = default!;

    public virtual BaseValueType BaseValueType { get; set; } = default!;
    public virtual User CreatedBy { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual ICollection<Attachment> Attachments { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
    public virtual ICollection<PersonAddress> PersonAddresses { get; set; } = default!;
    public virtual ICollection<PersonFingerprint> PersonFingerprints { get; set; } = default!;
    public virtual ICollection<Person> PersonGenderBases { get; set; } = default!;
    public virtual ICollection<Person> PersonGovernmentalBases { get; set; } = default!;
    public virtual ICollection<Person> PersonLegalBases { get; set; } = default!;
    public virtual ICollection<User> Users { get; set; } = default!;
    public virtual ICollection<Customer> Customers { get; set; } = default!;
}

