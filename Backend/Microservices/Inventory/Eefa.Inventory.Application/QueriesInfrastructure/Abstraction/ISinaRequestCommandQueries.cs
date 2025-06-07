using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public interface ISinaRequestCommandQueries : IQuery
    {

        Task<ICollection<SinaProduct>> GetProductByMaxSinaIdList();
        Task<ICollection<SinaProduct>> GetAllProductNeedUpdate();
        Task<ReceiptQueryModel> GetInputProductToWarehouse(string Date);
        Task<ReceiptQueryModel> GetProductLeaveWarehouse(string Date);
        Task<ICollection<FilesByPaymentNumber>> GetFilesByPaymentNumber(string financialOperationNumber);
        Task InsertProduct(SinaProduct model);

        Task UpdateProductProperty(string Date);


    }
}

