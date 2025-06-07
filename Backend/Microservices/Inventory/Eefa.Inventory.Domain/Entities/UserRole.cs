using Eefa.Common.Domain;
using System;

namespace Eefa.Inventory.Domain
{

    public partial class UserRole : DomainBaseEntity
    {
        public int RoleId { get; set; } = default!;
        public int UserId { get; set; } = default!;
        public bool AllowedStatus { get; set; } = default!;       
    }
}