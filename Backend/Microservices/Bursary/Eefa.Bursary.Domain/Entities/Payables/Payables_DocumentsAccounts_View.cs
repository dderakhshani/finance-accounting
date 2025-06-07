using Eefa.Common.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Bursary.Domain.Entities.Payables
{
    [Table("Payables_DocumentsAccounts_View", Schema = "bursary")]
    public class Payables_DocumentsAccounts_View : BaseEntity
    {
        public int DocumentId { get; set; }
        public int ReferenceId { get; set; }
        public string? ReferenceCode { get; set; }
        public string? ReferenceName { get; set; }
        public int? ReferenceGroupId { get; set; }
        public string? ReferenceGroupName { get; set; }
        public int? RexpId { get; set; }
        public string? RexpName { get; set; }
        public int? AccountHeadId { get; set; }
        public string? AccountHeadCode { get; set; }
        public string? AccountHeadName { get; set; }
        public string Descp { get; set; }
        public long Amount { get; set; }
    }
}
