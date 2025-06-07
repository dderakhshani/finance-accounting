using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;

namespace Eefa.Bursary.Application.Queries.CustomerReceipt
{
    public interface ICustomerReceiptQueries: IQuery
    {
       Task<PagedList<CustomerReceiptViewModel>> GetAll(PaginatedQueryModel paginatedQuery);
      PagedList<CustomerReceiptViewModel>  GetUploadedReceipts(string urlAddress);
 
   }
}
