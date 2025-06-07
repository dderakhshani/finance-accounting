using System;
using Eefa.Common.Domain;

namespace Eefa.Inventory.Domain
{
    public partial class RolePermission : DomainBaseEntity
    {
        public int RoleId { get; set; } = default!;
        public int PermissionId { get; set; } = default!;
    }
}
