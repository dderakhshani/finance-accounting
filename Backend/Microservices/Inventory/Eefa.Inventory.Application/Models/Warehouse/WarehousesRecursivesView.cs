using Eefa.Common;

namespace Eefa.Inventory.Application
{

    public  class WarehousesLastLevelViewModel : IMapFrom<Eefa.Inventory.Domain.WarehousesLastLevelView>
    {
        
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string LevelCode { get; set; }
        public int? CommodityCategoryId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int? AccountRererenceGroupId { get; set; } = default!;
        public int? AccountReferenceId { get; set; } = default!;
        public int? AccountHeadId { get; set; } = default!;
        public int? Sort { get; set; } = default!;
        

    }



}
