using System.Collections.Generic;
using Library.Common;

namespace Eefa.Admin.Data.Databases.Entities
{
    public partial class UnitPosition : BaseEntity
    {

        public int PositionId { get; set; } = default!;

        /// <summary>
        /// کد واحد
        /// </summary>
        public int UnitId { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Position Position { get; set; } = default!;
        public virtual Unit Unit { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
    }
}
