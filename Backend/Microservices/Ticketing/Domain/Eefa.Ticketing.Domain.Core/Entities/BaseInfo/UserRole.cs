namespace Eefa.Ticketing.Domain.Core.Entities.BaseInfo
{
    public class UserRole : BaseEntity
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public bool AllowedStatus { get; set; }
        public User CreatedBy { get; set; }
        public User ModifiedBy { get; set; }
        public Role OwnerRole { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
