using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common.Data;
using Eefa.Common;
using Eefa.Common.Data.Query;
using MediatR;
using Eefa.Common.CommandQuery;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Eefa.Bursary.Application.UseCases.Payables.Documents.Queries
{
    public class GetDocumentAccountQuery : Pagination, IRequest<ServiceResult<PagedList<Payables_DocumentsAccounts_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetDocumentAccountQueryHandler : IRequestHandler<GetDocumentAccountQuery, ServiceResult<PagedList<Payables_DocumentsAccounts_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_DocumentsAccounts_View> _repository;

        public GetDocumentAccountQueryHandler(IMapper mapper, IRepository<Payables_DocumentsAccounts_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<Payables_DocumentsAccounts_View>>> Handle(GetDocumentAccountQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
               .GetAll()
               .FilterQuery(request.Conditions)
               .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<Payables_DocumentsAccounts_View>>.Success(new PagedList<Payables_DocumentsAccounts_View>()
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
