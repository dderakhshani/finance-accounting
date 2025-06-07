using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries
{
    public class GetBankAccountTypeQuery : Pagination, IRequest<ServiceResult<PagedList<BankAccountTypeResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }

    }

    public class GetBankAccountTypeQueryHandler : IRequestHandler<GetBankAccountTypeQuery, ServiceResult<PagedList<BankAccountTypeResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BankAccountTypes_View> _repository;

        public GetBankAccountTypeQueryHandler(IMapper mapper, IRepository<BankAccountTypes_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankAccountTypeResponseModel>>> Handle(GetBankAccountTypeQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankAccountTypeResponseModel>>.Success(new PagedList<BankAccountTypeResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankAccountTypeResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }

}
