using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "UserRoles", Schema = "admin")]

    public partial class UserRole : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.UserRole, User>(x => x.UserId, x => x.Person.FirstName + " " + x.Person.LastName, x => x.Id),
                new AuditMapRule<Entities.UserRole, Role>(x => x.RoleId, x => x.Title, x => x.Id),
            };
        }
    }
}