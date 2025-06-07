using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;

namespace Eefa.Commodity.Application.Queries.Measure
{
    public interface IMeasureQueries: IQuery
    {
        Task<MeasureUnitModel> GetMeasureUnitById(int id);
        Task<PagedList<MeasureUnitModel>> GetMeasures(PaginatedQueryModel query);
        Task<MeasureUnitConversionModel> GetConversionById(int id);
        Task<PagedList<MeasureUnitConversionModel>> GetConversionsByMeasureUnitId(int measureUnitId, PaginatedQueryModel query);
    }
}