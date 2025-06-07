using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{
    public partial class UserSetting : BaseEntity
    {
        public int UserId { get; set; } = default!;

        /// <summary>
        /// کلمه کلیدی
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public string? Value { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
