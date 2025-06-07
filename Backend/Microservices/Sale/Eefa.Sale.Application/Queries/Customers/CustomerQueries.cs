using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using System.Threading.Tasks;
using Eefa.Sale.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Queries.Customers
{
    public class CustomerQueries : ICustomerQueries
    {
        private readonly ISalesUnitOfWork _context;
        private readonly IMapper _mapper;

        public CustomerQueries(
            ISalesUnitOfWork context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }




        public async Task<ServiceResult<PagedList<CustomerModel>>> GetAll(PaginatedQueryModel query)
        {
            var data = _context.Customers
                .Include(x => x.Person)
                .ThenInclude(x => x.AccountReference)
                .Include(x => x.CustomerTypeBase)
                .Include(x => x.CurrentAgent)
                .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty)
               ;

            return ServiceResult<PagedList<CustomerModel>>.Success(
                new PagedList<CustomerModel>()
                {
                    Data = await data.Paginate(query.Paginator()).ToListAsync(),

                    TotalCount = query.PageIndex <= 1 ? await data.CountAsync() : 0
                }
            );

        }


        public async Task<ServiceResult<CustomerModel>> GetById(int id)
        {
            var data = await _context.Customers
                .Where(x => x.Id == id)
                .Include(x => x.Person)
                .ThenInclude(x => x.AccountReference)
                .Include(x => x.CustomerTypeBase)
                .Include(x => x.CurrentAgent)
                .ProjectTo<CustomerModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return ServiceResult<CustomerModel>.Success(data);

        }
    }
}
