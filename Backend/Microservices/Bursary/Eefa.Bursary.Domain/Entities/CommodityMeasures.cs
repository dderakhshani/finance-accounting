using Eefa.Common.Data;

namespace Eefa.Bursary.Domain.Entities
{
    public partial class CommodityMeasures : BaseEntity
    {
         
        public int CommodityId { get; set; } = default!;
        public int MeasureId { get; set; } = default!;
        public int OrderIndex { get; set; } = default!;
         
         
         
         
         
         

        public virtual Commodities Commodity { get; set; } = default!;
        public virtual Users CreatedBy { get; set; } = default!;
        public virtual MeasureUnits Measure { get; set; } = default!;
    }
}
