using Eefa.Common.Data;
using Eefa.Common.Data.Query;

using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.AccountHead
{
    public class AccountHeadQueries : IAccountHeadQueries
    {


        private readonly IRepository<Domain.Entities.AccountHead> _accountHeadRepository; 
        private readonly IMapper _mapper;

        public AccountHeadQueries(  IMapper mapper, IRepository<Domain.Entities.AccountHead> accountHeadRepository)
        {
            _mapper = mapper;
            _accountHeadRepository = accountHeadRepository;
        }



        public async Task<PagedList<AccountHeadViewModel>> GetAll(PaginatedQueryModel query)
        {

            var accountHeads = _accountHeadRepository.GetAll()
                .ProjectTo<AccountHeadViewModel>(_mapper.ConfigurationProvider)
                .FilterQuery(query.Conditions)
                .OrderByMultipleColumns(query.OrderByProperty);
 
             
                 var result = new PagedList<AccountHeadViewModel>()
                {
                    Data = await accountHeads.Paginate(query.Paginator()).ToListAsync(),
                    TotalCount = query.PageIndex <= 1 ? await accountHeads.CountAsync() : 0
                };

            return result;

        }
    }
}
