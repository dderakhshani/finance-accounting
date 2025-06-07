public interface IHierarchicalAuditableEntity
{
    public string LevelCode { get; set; }
    public int? ParentId { get; set; }
}

public class HierarchicalAuditableEntity : AuditableEntity, IHierarchicalAuditableEntity
{
    [UniqueIndex]
    public string LevelCode { get; set; }
    public int? ParentId { get; set; }
}