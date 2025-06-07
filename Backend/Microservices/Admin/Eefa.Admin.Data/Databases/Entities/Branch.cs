using System.Collections.Generic;
using Library.Attributes;
using Library.Interfaces;

namespace Eefa.Admin.Data.Databases.Entities
{

    [HasUniqueIndex]
    public partial class Branch : HierarchicalBaseEntity
    {
        /// <summary>
        /// کد
        /// </summary>

        public double? Lat { get; set; }
        public double? Lng { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [UniqueIndex]
        public string Title { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Branch? Parent { get; set; } = default!;
        public virtual ICollection<Branch> InverseParent { get; set; } = default!;
        public virtual ICollection<Unit> Units { get; set; } = default!;
    }
}
