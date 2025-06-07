using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "UnitPositions", Schema = "admin")]

    public partial class UnitPosition : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.UnitPosition, Unit>(x => x.UnitId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.UnitPosition, Position>(x => x.PositionId, x => x.Title, x => x.Id),
            };
        }
    }
}