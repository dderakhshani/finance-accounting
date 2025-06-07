namespace Eefa.Identity.Services.User.Models
{
    public class UserPermissionModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string LevelCode { get; set; }
        public string UniqueName { get; set; }
        public string Title { get; set; }
    }
}