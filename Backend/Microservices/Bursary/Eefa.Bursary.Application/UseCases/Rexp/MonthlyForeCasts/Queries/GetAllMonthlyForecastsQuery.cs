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

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Queries
{
    public class GetAllMonthlyForecastsQuery : Pagination, IRequest<ServiceResult<PagedList<MonthlyForecast_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllMonthlyForecastsQueryHandler : IRequestHandler<GetAllMonthlyForecastsQuery, ServiceResult<PagedList<MonthlyForecast_View>>>
    {
        private readonly IRepository<MonthlyForecast_View> _repository;

        public GetAllMonthlyForecastsQueryHandler(IRepository<MonthlyForecast_View> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<MonthlyForecast_View>>> Handle(GetAllMonthlyForecastsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<MonthlyForecast_View>>.Success(new PagedList<MonthlyForecast_View>()
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
