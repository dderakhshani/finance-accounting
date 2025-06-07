using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;

namespace Eefa.Inventory.Application
{
    public interface IAssetsQueries : IQuery
    {
        Task<PagedList<AssetsModel>> GetAll(string FromDate,string ToDate, PaginatedQueryModel query);
        Task<AssetsModel> GetByDocumentId(int DocumentHeadId, int CommodityId);
        Task<AssetsModel> GetById(int id);
        Task<string> GetLastNumber(int AssetGroupId);
        Task<int[]> GetAssetAttachmentsIdByPersonsDebitedCommoditiesId(int AssetId, int PersonsDebitedCommoditiesId);
        Task<AssetsSerialModel> GetDuplicateAssets(AssetsSerialModel[] AssetsSerialModels);
    }
}
