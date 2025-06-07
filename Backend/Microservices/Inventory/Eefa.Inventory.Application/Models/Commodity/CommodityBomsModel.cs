using System;

namespace Eefa.Inventory.Application.Models
{
    /// <summary>
    /// فرمول ساخت
    /// </summary>
    public class CommodityBomsModel
    {
        public int Id { get; set; } = default!;
        public int BomsId { get; set; } = default!;
        public int BomsHeaderId { get; set; } = default!;
        public int BomWarehouseId { get; set; } = default!;
        public int? RootId { get; set; } = default!;
        public string LevelCode { get; set; } = default!;
        public int? CommodityCategoryId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool? IsActive { get; set; } = default!;
        public int? CommodityId { get; set; } = default!;
        public DateTime BomDate { get; set; } = default!;
    }
}
