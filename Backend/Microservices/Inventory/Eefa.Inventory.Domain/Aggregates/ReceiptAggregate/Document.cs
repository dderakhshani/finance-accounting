using System;
using Eefa.Common.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using Eefa.Common.Data;
using System.Collections.Generic;

namespace Eefa.Inventory.Domain
{
    [Table("Documents", Schema = "common")]
    public partial class Document : DomainBaseEntity
    {
        public int DocumentId { get; set; } = default!;
        public int DocumentNo { get; set; } = default!;
        public DateTime DocumentDate { get; set; } = default!;
        public int ReferenceId { get; set; } = default!;
        public int DocumentTypeBaseId { get; set; } = default!;
        public string FinancialOperationNumber { get; set; } = default!;
        public string DocumentDescription { get; set; } = default!;
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }

    }
}
