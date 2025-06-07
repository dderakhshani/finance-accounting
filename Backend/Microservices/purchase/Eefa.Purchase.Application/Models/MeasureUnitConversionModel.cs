using Eefa.Common;
using Eefa.Purchase.Domain.Entities;

namespace Eefa.Purchase.Application.Models
{
    public record CommodityMeasureUnitModel : IMapFrom<MeasureUnit>
    {
        public int Id { get; set; }
        public string Title { get; set; }
       

    }
    public record MeasureUnitConversionModel : CommodityMeasureUnitModel
    {
      
        public int MeasureUnitConversionId { get; set; }

    }
}
