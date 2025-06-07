using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.Definitions.Queries
{
    public class GetAllResourcesExpensesQuery : Pagination, IRequest<ServiceResult<PagedList<ResourceExpense_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllResourcesExpensesQueryHandler : IRequestHandler<GetAllResourcesExpensesQuery, ServiceResult<PagedList<ResourceExpense_View>>>
    {
        private readonly IRepository<ResourceExpense_View> _repository;

        public GetAllResourcesExpensesQueryHandler(IRepository<ResourceExpense_View> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<ResourceExpense_View>>> Handle(GetAllResourcesExpensesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<ResourceExpense_View>>.Success(new PagedList<ResourceExpense_View>()
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
