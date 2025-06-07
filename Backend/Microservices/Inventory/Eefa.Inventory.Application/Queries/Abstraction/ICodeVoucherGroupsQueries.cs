using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;

namespace Eefa.Inventory.Application
{
    public interface ICodeVoucherGroupsQueries : IQuery
    {
        Task<PagedList<ReceiptALLStatusModel>> GetReceiptALLVoucherGroup(string Code);
        Task<PagedList<ReceiptALLStatusModel>> GetALL();



    }
}
