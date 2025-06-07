using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

#nullable disable

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "DataBaseMetadata", Schema = "common")]

    public partial class DataBaseMetadata : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>();
        }
    }
}