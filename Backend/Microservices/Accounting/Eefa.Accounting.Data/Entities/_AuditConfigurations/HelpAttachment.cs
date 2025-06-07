using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Accounting.Data.Entities
{
    [Table(name: "HelpAttachment", Schema = "admin")]

    public partial class HelpAttachment : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}