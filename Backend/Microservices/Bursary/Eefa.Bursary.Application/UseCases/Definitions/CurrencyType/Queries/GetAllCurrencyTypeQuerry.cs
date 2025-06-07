using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.CurrencyType.Queries
{
    public class GetAllCurrencyTypeQuerry : Pagination, IRequest<ServiceResult<PagedList<CurrencyTypes_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllCurrencyTypeQuerryHandler : IRequestHandler<GetAllCurrencyTypeQuerry, ServiceResult<PagedList<CurrencyTypes_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<CurrencyTypes_View> _repository;

        public GetAllCurrencyTypeQuerryHandler(IMapper mapper, IRepository<CurrencyTypes_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<CurrencyTypes_View>>> Handle(GetAllCurrencyTypeQuerry request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<CurrencyTypes_View>>.Success(new PagedList<CurrencyTypes_View>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }




}
