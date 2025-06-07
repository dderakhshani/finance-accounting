using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Domain.Aggregates.ChequeAggregate;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Queries.Cheque
{
    public class ChequeQueries : IChequeQueries
    {
        private readonly IChequeRepository _chequeRepository;
        private readonly IMapper _mapper;

        public ChequeQueries(IChequeRepository chequeRepository, IMapper mapper)
        {
            _chequeRepository = chequeRepository;
            _mapper = mapper;
        }


        public async Task<PagedList<ChequeModel>> GetAll(PaginatedQueryModel query)
        {
                       var cheques = _chequeRepository.GetAll().ProjectTo<ChequeModel>(_mapper.ConfigurationProvider)
              .FilterQuery(query.Conditions)
              .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<ChequeModel>()
            {
                Data = await cheques.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await cheques
                        .CountAsync()
                    : 0
            };
        }

        public async Task<ChequeModel> GetById(int id)
        {
            var cheque = await _chequeRepository.Find(id);
            return _mapper.Map<ChequeModel>(cheque);
        }
    }
}
