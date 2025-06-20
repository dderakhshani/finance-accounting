﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Common;
using Library.Models;

namespace Eefa.Admin.Data.Databases.Entities
{
    [Table(name: "RolePermissions", Schema = "admin")]

    public partial class RolePermission : IAuditable
    {
        public List<AuditMapRule> Audit()
        {
            return new List<AuditMapRule>()
            {
                new AuditMapRule<Entities.RolePermission, Permission>(x => x.PermissionId, x => x.Title, x => x.Id),
                new AuditMapRule<Entities.RolePermission, Role>(x => x.RoleId, x => x.Title, x => x.Id),
            };
        }
    }
}