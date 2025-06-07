using System.Collections.Generic;
using Library.Common;

namespace Eefa.Accounting.Data.Entities
{
    public partial class CommodityCategory : BaseEntity
    {
        public int? ParentId { get; set; }
        public string LevelCode { get; set; } = default!;
        public string Code { get; set; }
        public int CodingMode { get; set; }
        public string? Title { get; set; }
        public int MeasureId { get; set; }
        public int OrderIndex { get; set; } = default!;
        public bool IsReadOnly { get; set; } = default!;
        public bool? RequireParentProduct { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual CommodityCategory? Parent { get; set; } = default!;
        public virtual ICollection<CommodityCategory> InverseParent { get; set; } = default!;
    }
}
