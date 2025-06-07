using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Sale.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Sale.Application.Queries.SaleAgents
{
    public class SalesAgentQueries : ISalesAgentQueries
    {
        private readonly ISalesUnitOfWork _context;
        private readonly IMapper _mapper;

        public SalesAgentQueries(
            ISalesUnitOfWork context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }




        public async Task<ServiceResult<PagedList<SalesAgentModel>>> GetAll(PaginatedQueryModel query)
        {
            var data = _context.SalesAgents
                    .ProjectTo<SalesAgentModel>(_mapper.ConfigurationProvider)
                    .FilterQuery(query.Conditions)
                    .OrderByMultipleColumns(query.OrderByProperty)
                ;

            return ServiceResult<PagedList<SalesAgentModel>>.Success(
                new PagedList<SalesAgentModel>()
                {
                    Data = await data.Paginate(query.Paginator()).ToListAsync(),

                    TotalCount = query.PageIndex <= 1 ? await _context.Persons.CountAsync() : 0
                }
            );

        }


        public async Task<ServiceResult<SalesAgentModel>> GetById(int id)
        {
            var data = await _context.SalesAgents
                .Where(x => x.Id == id)
                .ProjectTo<SalesAgentModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return ServiceResult<SalesAgentModel>.Success(data);

        }
    }
}
