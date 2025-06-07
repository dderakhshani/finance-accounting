using System;
using Eefa.Common.Domain;

namespace Eefa.Purchase.Domain.Aggregates.InvoiceAggregate
{

    public partial class Document : DomainBaseEntity
    {
        public  int  DocumentId  { get; set; } = default!;
        public int  DocumentNo  { get; set; } = default!;
        public DateTime  DocumentDate  { get; set; } = default!;
        public int  ReferenceId  { get; set; } = default!;
        public int	DocumentTypeBaseId  { get; set; } = default!;

    }
}
