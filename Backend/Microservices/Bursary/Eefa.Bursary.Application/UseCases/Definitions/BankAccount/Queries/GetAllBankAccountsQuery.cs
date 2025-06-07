using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models;
using Eefa.Common.Data;
using Eefa.Common;
using MediatR;
using Eefa.Common.Data.Query;
using Eefa.Common.CommandQuery;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries
{
    public class GetAllBankAccountsQuery : Pagination, IRequest<ServiceResult<PagedList<BankAccountsResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllBankAccountsQueryHandler : IRequestHandler<GetAllBankAccountsQuery, ServiceResult<PagedList<BankAccountsResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BankAccounts_View> _repository;

        public GetAllBankAccountsQueryHandler(IMapper mapper, IRepository<BankAccounts_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankAccountsResponseModel>>> Handle(GetAllBankAccountsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankAccountsResponseModel>>.Success(new PagedList<BankAccountsResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankAccountsResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }

}
