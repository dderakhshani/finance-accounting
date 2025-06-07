using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "UserCompany", Schema = "common")]

    public partial class UserCompany : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.UserCompany, User>(x => x.UserId, x => x.Person.FirstName + " " + x.Person.LastName, x => x.Id),
            };
        }
    }
}