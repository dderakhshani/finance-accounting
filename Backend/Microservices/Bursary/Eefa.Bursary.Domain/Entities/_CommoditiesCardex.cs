using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class _CommoditiesCardex : BaseEntity
    {
         
        public int Commodityld { get; set; } = default!;
        public int WarehouseLayoutId { get; set; } = default!;
        public double Quantity { get; set; } = default!;
 
        public int Mode { get; set; } = default!;
        public int? DocumentItemId { get; set; }
        public int? Barcode { get; set; }
         
         
         
         
         
         

        public virtual Commodities CommodityldNavigation { get; set; } = default!;
    }
}
