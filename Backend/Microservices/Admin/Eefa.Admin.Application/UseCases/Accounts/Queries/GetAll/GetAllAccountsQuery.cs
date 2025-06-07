using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Accounts.Models;
using Eefa.Admin.Data.Databases.Entities;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Queries.GetAll
{


    public class GetAllAccountsQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }

        public GetAllAccountsQuery()
        {

        }
    }

    public class GetAllAccountsQueryHandler : IRequestHandler<GetAllAccountsQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllAccountsQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<AccountReference>(x =>
                    x.ConditionExpression(x => x.IsActive))
                .ProjectTo<AccountMinifiedModel>(_mapper.ConfigurationProvider)
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
