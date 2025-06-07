using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.Domain;
using Eefa.Inventory.Domain;

namespace Eefa.Invertory.Infrastructure.Services
{
    public interface ISinaService : IService
    {
        Task<ICollection<SinaProduct>> GetProductList(int LastId, string url);
        Task<ICollection<SinaProducingInputProduct>> GetInputProductToWarehouse(string Date, int firingType, string url);
        Task<ICollection<SinaProducingProduct>> GetOutProductToWarehouse(string Date, string url);

        Task<ICollection<FilesByPaymentNumber>> GetFilesByPaymentNumber(string url, string financialOperationNumber);
    }
    
}