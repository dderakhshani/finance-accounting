using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.Domain;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Services.Arani
{
    public interface IAraniService : IService
    {
        Task<ICollection<AraniPurchaseRequestModel>> GetRequestById(int requestId,string url);
        Task<AraniRequestCommodityWarehouseModel> GetRequestCommodityWarehouse(string requestId, string url);
        Task<AraniReturnCommodityWarehouseModel> GetReturnCommodityWarehouse(int requestId, string url);



    }
    
}