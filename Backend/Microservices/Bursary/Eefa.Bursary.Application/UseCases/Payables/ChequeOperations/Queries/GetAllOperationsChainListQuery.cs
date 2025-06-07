using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeOperations.Queries
{
    public class GetAllOperationsChainListQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_DocumentOperations_Chain_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllOperationsChainListQueryhandler : IRequestHandler<GetAllOperationsChainListQuery, ServiceResult<PagedList<Payables_DocumentOperations_Chain_View>>>
    {
        private readonly IRepository<Payables_DocumentOperations_Chain_View> _repository;
        public GetAllOperationsChainListQueryhandler(IRepository<Payables_DocumentOperations_Chain_View> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_DocumentOperations_Chain_View>>> Handle(GetAllOperationsChainListQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_DocumentOperations_Chain_View>>.Success(new PagedList<Payables_DocumentOperations_Chain_View>()
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
