using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
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
    public class GetAllBankAccountReferencesQuery : Pagination, IRequest<ServiceResult<PagedList<BankAccountReferenceResponseModel>>>,IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllBankAccountReferencesQueryHandler : IRequestHandler<GetAllBankAccountReferencesQuery, ServiceResult<PagedList<BankAccountReferenceResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BankAccountReferences_View> _repository;

        public GetAllBankAccountReferencesQueryHandler(IMapper mapper, IRepository<BankAccountReferences_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankAccountReferenceResponseModel>>> Handle(GetAllBankAccountReferencesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankAccountReferenceResponseModel>>.Success(new PagedList<BankAccountReferenceResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankAccountReferenceResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }


}
