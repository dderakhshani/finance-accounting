using AutoMapper;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Queries
{
    public class GetPaySubjectsQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_Subjects_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetPaySubjectsQueryHandler : IRequestHandler<GetPaySubjectsQuery, ServiceResult<PagedList<Payables_Subjects_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_Subjects_View> _repository;

        public GetPaySubjectsQueryHandler(IMapper mapper, IRepository<Payables_Subjects_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_Subjects_View>>> Handle(GetPaySubjectsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
               .GetAll()
               .FilterQuery(request.Conditions)
               .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_Subjects_View>>.Success(new PagedList<Payables_Subjects_View>()
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
