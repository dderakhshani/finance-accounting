using Eefa.Common;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public class CommodityCategoryModel : IMapFrom<Domain.CommodityCategory>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        public string Title { get; set; }
        public string Code { get; set; }
        public string LevelCode { get; set; } = default!;
        public List<CommodityCategoryModel> Children { get; set; } = default!;
    }
}
