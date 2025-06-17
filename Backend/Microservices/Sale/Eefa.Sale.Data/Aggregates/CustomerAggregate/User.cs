using Eefa.Common;
using Eefa.Common.Data;
using System;
using System.Collections.Generic;


namespace Eefa.Sale.Domain.Aggregates.CustomerAggregate
{
    [HasUniqueIndex]
    public partial class User : BaseEntity
    {
        public int PersonId { get; set; } = default!;

        /// <summary>
        /// نام کاربری
        /// </summary>
        [UniqueIndex]
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

        public virtual Person Person { get; set; } = default!;

        public virtual ICollection<Customer> CustomersCreatedBies { get; set; } = default!;

    }
}
