using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Models.Receipt;

namespace Eefa.Purchase.Application.Queries.Abstraction
{
    public interface IInvoiceQueries : IQuery
    {
        Task<InvoiceQueryModel> GetById(int id);
        
        Task<PagedList<InvoiceQueryModel>> GetAll(int codeVoucherGroupId, bool? IsImportPurchase, DateTime? FromDate, DateTime? ToDate, PaginatedQueryModel query);
        Task<PagedList<InvoiceQueryModel>> GetInvoiceActiveRequestNo(int RequestNo, int ReferenceId, string FromDate, string ToDate);
        Task<PagedList<InvoiceQueryModel>> GetInvoiceActiveCommodityId(int CommodityId, int ReferenceId, string FromDate, string ToDate);

        Task<PagedList<InvoiceALLStatusModel>> GetInvoiceALLVoucherGroup();
        Task<InvoiceQueryModel> GetByListId(List<int> ListId);




    }
}

