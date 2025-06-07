using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBooks.Models;
using Eefa.Common.Data;
using Eefa.Common;
using MediatR;
using Eefa.Common.Data.Query;
using Eefa.Common.CommandQuery;
using Eefa.Bursary.Domain.Entities.Payables;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.UseCases.Payables.ChequeOperations.Queries
{
    public class GetAllOperationsListQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_ChequeOperations_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllOperationsListQueryHandler : IRequestHandler<GetAllOperationsListQuery, ServiceResult<PagedList<Payables_ChequeOperations_View>>>
    {
        private readonly IRepository<Payables_ChequeOperations_View> _repository;

        public GetAllOperationsListQueryHandler(IRepository<Payables_ChequeOperations_View> repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_ChequeOperations_View>>> Handle(GetAllOperationsListQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_ChequeOperations_View>>.Success(new PagedList<Payables_ChequeOperations_View>()
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
