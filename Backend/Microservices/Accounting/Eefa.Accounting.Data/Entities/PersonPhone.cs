using Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Accounting.Data.Entities
{
    public partial class PersonPhone : BaseEntity
    {

        public int PersonId { get; set; } = default!;
        public int PhoneTypeBaseId { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string? Description { get; set; }
        public bool IsDefault { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual BaseValue PhoneTypeBase { get; set; } = default!;
    }
}
