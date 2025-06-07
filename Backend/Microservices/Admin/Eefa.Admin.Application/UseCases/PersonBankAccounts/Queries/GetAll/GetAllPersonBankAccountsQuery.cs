using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Utility;

namespace Eefa.Admin.Application.CommandQueries.PersonAddress.Query.GetAll
{
    public class GetAllPersonBankAccountsQuery : Pagination,IRequest<ServiceResult>, IQuery
    {
        public List<Condition> Conditions { get; set; }
    }

    public class GetAllPersonBankAccountsQueryHandler : IRequestHandler<GetAllPersonBankAccountsQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPersonBankAccountsQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ServiceResult> Handle(GetAllPersonBankAccountsQuery request , CancellationToken cancellationToken)
        {
            var entities = _repository
                .GetAll<Data.Databases.Entities.PersonBankAccount>()
                .ProjectTo<PersonBankAccountModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }
}
