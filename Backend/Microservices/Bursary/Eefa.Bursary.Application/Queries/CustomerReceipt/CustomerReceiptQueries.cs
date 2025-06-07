using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.CustomerReceipt
{
    public  class CustomerReceiptQueries : ICustomerReceiptQueries
  {
      private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
      private readonly IRepository<FinancialRequestDetails> _financialDetailRepository;
      private readonly IMapper _mapper;


        public CustomerReceiptQueries(IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository, IRepository<FinancialRequestDetails> financialDetailRepository, IMapper mapper)
        {
            _financialRepository = financialRepository;
            _financialDetailRepository = financialDetailRepository;
            _mapper = mapper;
        }

        public async Task<PagedList<CustomerReceiptViewModel>> GetAll(PaginatedQueryModel paginatedQuery)
        {
            var customerReceiveDocs = _financialRepository.GetAll()
                .Include(x => x.FinancialRequestDetails)
                .Include(x => x.FinancialRequestAttachments)
              .ProjectTo<CustomerReceiptViewModel>(_mapper.ConfigurationProvider);


          var result = new PagedList<CustomerReceiptViewModel>()
          {
              Data = await customerReceiveDocs.ToListAsync(),
              TotalCount = paginatedQuery.PageIndex <= 1
                  ? await customerReceiveDocs
                      .CountAsync()
                  : 0
          };

          return result;

        }


        public  PagedList<CustomerReceiptViewModel>  GetUploadedReceipts(string urlAddress)
        {
            return new PagedList<CustomerReceiptViewModel>();
        }
    }
}
