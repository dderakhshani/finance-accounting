using System.Collections.Generic;
using Library.Common;

namespace Eefa.Identity.Data.Databases.Entities
{

    public partial class Branch : BaseEntity
    {

        public string LevelCode { get; set; } = default!;

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// کد والد
        /// </summary>
        public int? ParentId { get; set; }

        public double? Lat { get; set; }
        public double? Lng { get; set; }


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Branch? Parent { get; set; } = default!;
        public virtual ICollection<Branch> InverseParent { get; set; } = default!;
        public virtual ICollection<Unit> Units { get; set; } = default!;
    }
}
