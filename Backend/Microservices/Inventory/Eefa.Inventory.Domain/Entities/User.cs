using Eefa.Common.Domain;
using System;

namespace Eefa.Inventory.Domain { 
    
    public partial class User : DomainBaseEntity
    
    {
        public int PersonId { get; set; } = default!;

       
        public string Username { get; set; } = default!;

        /// <summary>
        /// آیا قفل شده است؟
        /// </summary>
        public bool IsBlocked { get; set; } = default!;

        /// <summary>
        /// علت قفل شدن
        /// </summary>
        public int? BlockedReasonBaseId { get; set; }

        /// <summary>
        /// رمز یکبار مصرف
        /// </summary>
        public string? OneTimePassword { get; set; }

        /// <summary>
        /// رمز
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// دفعات ورود ناموفق
        /// </summary>
        public int FailedCount { get; set; } = default!;

        /// <summary>
        /// آخرین زمان آنلاین بودن
        /// </summary>
        public DateTime? LastOnlineTime { get; set; }
        public DateTime PasswordExpiryDate { get; set; }


        
    }
}
