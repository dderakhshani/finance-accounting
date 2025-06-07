using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

// ReSharper disable once CheckNamespace
namespace Eefa.Accounting.Data.Entities
{
    [Table(name: "BaseValues", Schema = "admin")]
    public partial class BaseValue : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.BaseValue, BaseValueType>(x => x.BaseValueTypeId, x => x.Title, x => x.Id),
            };
        }
    }
}