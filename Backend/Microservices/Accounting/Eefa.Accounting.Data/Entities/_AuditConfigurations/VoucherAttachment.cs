using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

// ReSharper disable CheckNamespace

namespace Eefa.Accounting.Data.Entities
{
    [Table(name: "VoucherAttachments", Schema = "accounting")]

    public partial class VoucherAttachment : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}