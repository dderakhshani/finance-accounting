using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

// ReSharper disable All

namespace Eefa.Admin.Data.Databases.Entities
{

    [Table(name: "BaseValueTypes", Schema = "admin")]
    public partial class BaseValueType : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.BaseValueType, BaseValueType>(x => x.ParentId, x => x.Title, x => x.Id),
            };
        }
    }
}