using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Library.Attributes;
using Library.Common;

namespace FileTransfer.WebApi.Persistance.Entities
{
    [Table(name: "Users", Schema = "admin")]

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


     
        public virtual BaseValue? BlockedReasonBase { get; set; } = default!;
      
        public virtual ICollection<Attachment> AttachmentCreatedBies { get; set; } = default!;
        public virtual ICollection<Attachment> AttachmentModifiedBies { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValueCreatedBies { get; set; } = default!;
        public virtual ICollection<BaseValue> BaseValueModifiedBies { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypeCreatedBies { get; set; } = default!;
        public virtual ICollection<BaseValueType> BaseValueTypeModifiedBies { get; set; } = default!;
        public virtual ICollection<Language> LanguageCreatedBies { get; set; } = default!;
        public virtual ICollection<Language> LanguageModifiedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleCreatedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleModifiedBies { get; set; } = default!;
        public virtual ICollection<UserRole> UserRoleUsers { get; set; } = default!;
    }
}
