using Eefa.Common.Domain;
using System.Collections.Generic;

namespace Eefa.Inventory.Domain
{

    public partial class Role : DomainBaseEntity
    {
        public int? ParentId { get; set; }
        public string LevelCode { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; } = default!;
        public string? Description { get; set; }        

    }
}
