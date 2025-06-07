using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "AutoVoucherRowsLink", Schema = "accounting")]

    public partial class AutoVoucherRowsLink : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}