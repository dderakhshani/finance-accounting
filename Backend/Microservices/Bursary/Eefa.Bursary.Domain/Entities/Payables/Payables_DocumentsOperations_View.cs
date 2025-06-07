using Eefa.Common.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_DocumentsOperations_View", Schema = "bursary")]
    public class Payables_DocumentsOperations_View : BaseEntity
    {
        public int DocumentId { get; set; }
        public DateTime OperationDate { get; set; }
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        public int YearId { get; set; }
        public int? VoucherId { get; set; }
        public string Descp { get; set; }

    }
}
