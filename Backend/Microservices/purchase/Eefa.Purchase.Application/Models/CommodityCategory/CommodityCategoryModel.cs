using Eefa.Common;

namespace Eefa.Purchase.Application.Models.CommodityCategory
{
    public class CommodityCategoryModel : IMapFrom<Domain.Entities.CommodityCategory>
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        /// <description>
        /// کد سطح
        public string Title { get; set; }
        public string LevelCode { get; set; } = default!;
    }
}
