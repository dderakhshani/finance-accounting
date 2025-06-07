using Eefa.Common;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
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
