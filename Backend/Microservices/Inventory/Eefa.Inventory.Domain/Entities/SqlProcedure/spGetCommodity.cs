using System;

namespace Eefa.Inventory.Domain
{

    public partial class spGetCommodity
    {
        public int Id { get; set; }
        public Nullable<int> CommodityCategoryId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public Nullable<int> MeasureId { get; set; }
        public string CategoryLevelCode { get; set; }
        public string CategoryTitle { get; set; }
        public string MeasureTitle { get; set; }
        public string SearchTerm { get; set; }
        public Nullable<bool> IsConsumable { get; set; }
        public Nullable<bool> IsHaveWast { get; set; }
        public Nullable<bool> IsAsset { get; set; }

    }

}
