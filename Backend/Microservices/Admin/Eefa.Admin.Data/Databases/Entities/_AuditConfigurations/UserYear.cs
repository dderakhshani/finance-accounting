using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "UserYear", Schema = "common")]

    public partial class UserYear : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.UserYear, Year>(x => x.YearId, x => x.YearName.ToString(), x => x.Id),
                new AuditMapRule<Entities.UserYear, CompanyInformation>(x => x.YearId, x => x.Title, x => x.Id),
            };
        }
    }
}