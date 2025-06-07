using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    public partial class Payables_DocumentsOperations : BaseEntity
    {
        public int DocumentId { get; set; }
        public DateTime OperationDate { get; set; }
        public int OperationId { get; set; }
        public int YearId { get; set; }
        public string Descp { get; set; }

        public virtual Payables_Documents Document { get; set; }
        public virtual BaseValues Operation { get; set; }

    }
}