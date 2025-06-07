using System.Collections.Generic;
using System.Linq;
using Eefa.Inventory.Domain;
namespace Eefa.Inventory.Application
{
    public class MeasureUnitQueries : IMeasureUnitQueries
    {
        private readonly IInvertoryUnitOfWork _contex;
       public MeasureUnitQueries(IInvertoryUnitOfWork contex)
        {
            _contex = contex;
        }
        public List<MeasureUnitConversionModel> GetMeasureConversionsUnit(int? MeasureId)
        {
            return (from muc in _contex.MeasureUnitConversions.Where(c => c.SourceMeasureUnitId == MeasureId)
                    join smu in _contex.MeasureUnits on muc.DestinationMeasureUnitId equals smu.Id
                    select new MeasureUnitConversionModel
                    {
                        Title = smu.Title,
                        Id = smu.Id,
                        MeasureUnitConversionId = muc.Id
                    }).ToList();
        }
        public List<CommodityMeasureUnitModel> GetCommodityMeasuresUnit(int? commodityId)
        {
            return (from com in _contex.CommodityMeasures.Where(c => c.CommodityId == commodityId)
                    join smu in _contex.MeasureUnits on com.MeasureId equals smu.Id

                    select new CommodityMeasureUnitModel
                    {
                        Title = smu.Title,
                        Id = smu.Id,

                    }).ToList();
        }
    }
}
