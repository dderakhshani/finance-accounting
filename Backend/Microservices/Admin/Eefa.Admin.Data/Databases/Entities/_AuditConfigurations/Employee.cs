using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "Employees", Schema = "admin")]

    public partial class Employee : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.Employee, UnitPosition>(x => x.UnitPositionId, x => x.Unit.Title + " " + x.Position.Title, x => x.Id),
                new AuditMapRule<Entities.Employee, Person>(x => x.PersonId, x => x.FirstName + " " + x.LastName, x => x.Id),
            };
        }
    }
}