using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{

    public class UnitsModel : IMapFrom<Units>
    {
        public int? Id { get; set; } = default!;
        public int? ParentId { get; set; } = default!;
        public string LevelCode { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int? BranchId { get; set; } = default!;

    }

}

