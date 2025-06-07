using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.Bank
{
    public class BankQueries : IBankQueries
    {
        private readonly IRepository<Banks> _backRepository;
        private readonly IMapper _mapper;

        public BankQueries( IMapper mapper, IRepository<Banks> backRepository)
        {
            _mapper = mapper;
            _backRepository = backRepository;
        }


        public async Task<PagedList<BankModel>> GetAll(PaginatedQueryModel query)
        {
               var banks = _backRepository.GetAll().ProjectTo<BankModel>(_mapper.ConfigurationProvider)
              .FilterQuery(query.Conditions)
              .OrderByMultipleColumns(query.OrderByProperty);
            var result =  new PagedList<BankModel>()
            {
                Data = await banks.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await banks.CountAsync()
                    : 0
            };
            return result;
        }

 
    }
}
