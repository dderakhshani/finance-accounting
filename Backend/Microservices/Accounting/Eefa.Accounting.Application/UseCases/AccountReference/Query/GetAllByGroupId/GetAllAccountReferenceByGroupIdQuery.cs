using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.AccountReference.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReference.Query.GetAllByGroupId
{
    public class GetAllAccountReferenceByGroupIdQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public int GroupId { get; set; }
        public List<Condition> Conditions { get; set; }

        public GetAllAccountReferenceByGroupIdQuery()
        {

        }
    }

    public class GetAllAccountReferenceByGroupIdQueryHandler : IRequestHandler<GetAllAccountReferenceByGroupIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAccountReferenceByGroupIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllAccountReferenceByGroupIdQuery request, CancellationToken cancellationToken)
        {
            var entitis = (from r in _repository
                        .GetAll<Data.Entities.AccountReference>(x =>
                            x.ConditionExpression(t => t.IsActive))
                    join rg in _repository.GetQuery<Data.Entities.AccountReferencesRelReferencesGroup>()
                        on r.Id equals rg.ReferenceId
                    where rg.ReferenceGroupId == request.GroupId
                    select r
                ).ProjectTo<AccountReferenceModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
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
