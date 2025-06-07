using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eefa.Common.Data
{
    [Table("Audit", Schema = "dbo")]
    public class Audit:BaseEntity
    {
        public int UserId { get; set; }
        public string? SubSystem { get; set; }
        public int MenuId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public Guid TransactionId { get; set; }
        public DateTime DateTime { get; set; }
        public string ChangesValues { get; set; }
        public int PrimaryId { get; set; }
    }
}