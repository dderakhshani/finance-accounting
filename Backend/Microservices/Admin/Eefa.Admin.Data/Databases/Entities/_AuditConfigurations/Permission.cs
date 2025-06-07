using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "Permissions", Schema = "admin")]
    public partial class Permission : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}
