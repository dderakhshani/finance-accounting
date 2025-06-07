using System;
using System.ComponentModel.DataAnnotations;

public abstract class AuditableEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedById { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
    public int OwnerRoleId { get; set; }
    public int CreatedById { get; set; }
}

