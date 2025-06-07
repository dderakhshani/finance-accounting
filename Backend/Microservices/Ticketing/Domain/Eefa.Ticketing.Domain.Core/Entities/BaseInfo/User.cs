using Eefa.Common;
using System;
using System.Collections.Generic;

namespace Eefa.Ticketing.Domain.Core.Entities.BaseInfo
{
    public class User : BaseEntity
    {
        public int PersonId { get; set; } 
        [UniqueIndex]
        public string Username { get; set; } 
        public bool IsBlocked { get; set; } 
        public int? BlockedReasonBaseId { get; set; }
        public string OneTimePassword { get; set; }
        public string Password { get; set; } 
        public int FailedCount { get; set; }
        public DateTime? LastOnlineTime { get; set; }
        public DateTime PasswordExpiryDate { get; set; }
        public virtual ICollection<UserRole> UserRoleUsers { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<UserRole> UserRoleCreatedBies { get; set; }
        public virtual ICollection<UserRole> UserRoleModifiedBies { get; set; } 
    }
}
