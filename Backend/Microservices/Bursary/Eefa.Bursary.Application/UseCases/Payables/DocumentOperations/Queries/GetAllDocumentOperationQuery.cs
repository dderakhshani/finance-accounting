using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using Eefa.Common;
using MediatR;
using Eefa.Common.Data.Query;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;
using Eefa.Bursary.Application.UseCases.Payables.Documents.Queries;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.UseCases.Payables.DocumentOperations.Queries
{
    public class GetAllDocumentOperationQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_DocumentsOperations_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllDocumentOperationQueryHandler : IRequestHandler<GetAllDocumentOperationQuery, ServiceResult<PagedList<Payables_DocumentsOperations_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_DocumentsOperations_View> _repository;

        public GetAllDocumentOperationQueryHandler(IMapper mapper, IRepository<Payables_DocumentsOperations_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_DocumentsOperations_View>>> Handle(GetAllDocumentOperationQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_DocumentsOperations_View>>.Success(new PagedList<Payables_DocumentsOperations_View>()
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
