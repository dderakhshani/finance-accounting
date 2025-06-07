using Eefa.Bursary.Domain.Entities;
using Eefa.Common.Data;
using Eefa.Common;
using MediatR;
using Eefa.Common.Data.Query;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Queries
{
    public class GetAllDocumentsQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_Documents>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }

    }

    public class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, ServiceResult<PagedList<Payables_Documents>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_Documents> _repository;

        public GetAllDocumentsQueryHandler(IMapper mapper, IRepository<Payables_Documents> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_Documents>>> Handle(GetAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .Include(w => w.Payables_DocumentsAccounts)
                .Include(w => w.Payables_DocumentsOperations)
                .Include(w => w.Payables_DocumentsPayOrders)
                .Include(w => w.ChequeBookSheet)

                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_Documents>>.Success(new PagedList<Payables_Documents>()
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
