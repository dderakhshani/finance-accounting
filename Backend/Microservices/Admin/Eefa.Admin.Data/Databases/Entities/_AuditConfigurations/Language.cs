using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "Languages", Schema = "admin")]
    public partial class Language : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.Language, BaseValue>(x => x.DefaultCurrencyBaseId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.Language, BaseValue>(x => x.DirectionBaseId, x => x.Title, x => x.Id),
            };
        }
    }
}
