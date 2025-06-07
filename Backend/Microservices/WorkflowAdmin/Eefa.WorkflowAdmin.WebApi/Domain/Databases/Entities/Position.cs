using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.WorkflowAdmin.WebApi.Domain.Databases.Entities
{
    [HasUniqueIndex]
    public partial class Position : HierarchicalBaseEntity
    {

        /// <summary>
        /// عنوان
        /// </summary>
       [UniqueIndex]
        public string Title { get; set; } = default!;

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
    }
}
