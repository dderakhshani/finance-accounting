using Eefa.Bursary.Domain.Entities.Rexp;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Rexp.MonthlyForeCasts.Queries
{
    public class GetMonthlyForecatQuery : IRequest<ServiceResult<MonthlyForecast_View>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetMonthlyForecatQueryHandler : IRequestHandler<GetMonthlyForecatQuery, ServiceResult<MonthlyForecast_View>>
    {
        private readonly IBursaryUnitOfWork _uow;
        public GetMonthlyForecatQueryHandler(IBursaryUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<ServiceResult<MonthlyForecast_View>> Handle(GetMonthlyForecatQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.MonthlyForecast_View
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<MonthlyForecast_View>.Success(r);

        }
    }

}
