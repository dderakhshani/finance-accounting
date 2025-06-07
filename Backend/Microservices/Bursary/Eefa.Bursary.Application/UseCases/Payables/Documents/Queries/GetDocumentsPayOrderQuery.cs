using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using Eefa.Common;
using MediatR;
using Eefa.Common.Data.Query;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Queries
{
    public class GetDocumentsPayOrderQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_DocumentsPayOrders_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetDocumentsPayOrderQueryHandler : IRequestHandler<GetDocumentsPayOrderQuery, ServiceResult<PagedList<Payables_DocumentsPayOrders_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_DocumentsPayOrders_View> _repository;

        public GetDocumentsPayOrderQueryHandler(IMapper mapper, IRepository<Payables_DocumentsPayOrders_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_DocumentsPayOrders_View>>> Handle(GetDocumentsPayOrderQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
               .GetAll()
               .FilterQuery(request.Conditions)
               .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_DocumentsPayOrders_View>>.Success(new PagedList<Payables_DocumentsPayOrders_View>()
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
