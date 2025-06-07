using System;
using System.Collections.Generic;

public partial class VerificationCode : AuditableEntity
{

    public string Code { get; set; } = default!;
    public DateTime ExpiryDate { get; set; } = default!;
    public string Description { get; set; } = default!;
    public bool IsUsed { get; set; } = default!;


    public virtual ICollection<MoadianInvoiceHeader> MoadianInvoiceHeaders { get; set; } = default!;
}

