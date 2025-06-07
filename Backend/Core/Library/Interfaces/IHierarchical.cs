using Library.Attributes;
using Library.Common;

namespace Library.Interfaces
{
    public interface IHierarchicalBaseEntity
    {
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
    }

    public class HierarchicalBaseEntity : BaseEntity,IHierarchicalBaseEntity
    {
        [UniqueIndex]
        public string LevelCode { get; set; }
        public int? ParentId { get; set; }
    }
}