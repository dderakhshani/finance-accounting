using System;
using System.Collections.Generic;

public partial class DocumentHead : AuditableEntity
{
    public int FormTypeId { get; set; } = default!;
    public int CompanyId { get; set; } = default!;
    public int StoreId { get; set; } = default!;
    public int? ParentId { get; set; }
    public int? ReferenceId { get; set; }
    public int? FormNo { get; set; }
    public DateTime FormDate { get; set; } = default!;
    public string? FormDescription { get; set; }
    public byte FormStateId { get; set; } = default!;
    public bool IsManual { get; set; } = default!;
    public int? VocherId { get; set; }
    public int InvoiceTypeId { get; set; } = default!;
    public int TypeId { get; set; } = default!;
    public string? PayDescription { get; set; }
    public long TotalItemPrice { get; set; } = default!;
    public long TotalTax { get; set; } = default!;
    public long TotalDiscount { get; set; } = default!;
    public long? TotalVat { get; set; }
    public long? PriceMinusDiscount { get; set; }
    public long? PriceMinusDiscountPlusTax { get; set; }
    public int PaymentTypeId { get; set; } = default!;
    public DateTime? PaymentDate { get; set; }
    public DateTime? PaiedDate { get; set; }
    public int IsPaied { get; set; } = default!;
    public long? LiquidationPrice { get; set; }
    public long? Balance { get; set; }
    public int? LadingNo { get; set; }


    public virtual User CreatedBy { get; set; } = default!;
    public virtual BaseValue FormType { get; set; } = default!;
    public virtual BaseValue InvoiceType { get; set; } = default!;
    public virtual User? ModifiedBy { get; set; } = default!;
    public virtual Role OwnerRole { get; set; } = default!;
    public virtual BaseValue PaymentType { get; set; } = default!;
    public virtual BaseValue Type { get; set; } = default!;
    public virtual ICollection<DocumentItem> DocumentItems { get; set; } = default!;
    public virtual ICollection<VouchersDetail> VouchersDetails { get; set; } = default!;
}