using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Bursary.Application.Models;

namespace Eefa.Bursary.Application.Queries.FinancialRequest
{
    public interface IFinancialRequestQueries : IQuery
    {
        Task<FinancialRequestModel> GetById(int id);
        Task<FinancialRequestModel> GetLastReceiptInfo();


        Task<PagedList<FinancialRequestModel>> GetAll(PaginatedQueryModel query);
        Task<PagedList<FinancialRequestDetailModel>> GetDetailsByFinancialRequestId(int financialRequestId);
        Task<PagedList<FinancialAttachmentModel>> GetAttachmentsByFinancialRequestId(int financialRequestId);
        Task<List<int>> SetDocumentsForBursaryPaymentArticles(SendDocument model);
        Task<int> GetReqeustCountByStatus(int status);


    }
}
