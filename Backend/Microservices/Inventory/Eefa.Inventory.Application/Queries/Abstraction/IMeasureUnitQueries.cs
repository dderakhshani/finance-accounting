using System.Collections.Generic;
using Eefa.Common.CommandQuery;

namespace Eefa.Inventory.Application
{
    public interface IMeasureUnitQueries : IQuery
    {
        List<MeasureUnitConversionModel> GetMeasureConversionsUnit(int? MeasureId);
        List<CommodityMeasureUnitModel> GetCommodityMeasuresUnit(int? commodityId);
    }
}
