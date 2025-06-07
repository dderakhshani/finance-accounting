namespace Eefa.Identity.Services.User.Models
{
    public class UserMenuModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string FormUrl { get; set; }
        public int? PermissionId { get; set; }
        public string ImageUrl { get; set; }
    }
}