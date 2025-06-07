using System;

namespace Library.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public string OwnerRoleTitle{ get; set; }
        public int CreatorUserId { get; set; }

    }
}