public partial class UserYear : AuditableEntity
    {
        public int UserId { get; set; } = default!;
        public int YearId { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual User User { get; set; } = default!;
        public virtual Year Year { get; set; } = default!;
    }

