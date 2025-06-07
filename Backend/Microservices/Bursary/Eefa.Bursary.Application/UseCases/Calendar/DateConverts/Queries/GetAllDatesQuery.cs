using Eefa.Bursary.Domain.Entities.Calendar;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Calendar.DateConverts.Queries
{
    public class GetAllDatesQuery : Pagination, IRequest<ServiceResult<PagedList<DateConversion>>>
    {
        public List<QueryCondition> Conditions { get; set; }
    }
    public class GetAllDatesQueryHandler : IRequestHandler<GetAllDatesQuery, ServiceResult<PagedList<DateConversion>>>
    {
        private readonly IRepository<DateConversion> _repository;

        public GetAllDatesQueryHandler(IRepository<DateConversion> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<DateConversion>>> Handle(GetAllDatesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<DateConversion>>.Success(new PagedList<DateConversion>()
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
