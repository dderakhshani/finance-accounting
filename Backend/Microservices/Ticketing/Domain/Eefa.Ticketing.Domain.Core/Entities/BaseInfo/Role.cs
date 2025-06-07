using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.BaseInfo
{
    public class Role : BaseEntity
    {
        public string Title { get; set; }

        [UniqueIndex]
        public string UniqueName { get; set; }

        public string Description { get; set; }

        [UniqueIndex]
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
        public virtual Role Parent { get; set; }
        public virtual ICollection<Role> InverseParent { get; set; }
        public virtual ICollection<UserRole> UserRoleRoles { get; set; }
        public virtual ICollection<UserRole> UserRoleOwnerRoles { get; set; } 
    }
}
