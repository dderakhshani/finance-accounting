using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "Years", Schema = "common")]

    public partial class Year : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.Year, CompanyInformation>(x => x.CompanyId, x => x.Title, x => x.Id),
            };
        }
    }
}
