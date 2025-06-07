using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries
{
    public class GetBankTypesQuery : Pagination, IRequest<ServiceResult<PagedList<BankTypeResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetBankTypesQueryHandler : IRequestHandler<GetBankTypesQuery, ServiceResult<PagedList<BankTypeResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<BankTypes_View> _repository;

        public GetBankTypesQueryHandler(IMapper mapper, IRepository<BankTypes_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankTypeResponseModel>>> Handle(GetBankTypesQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankTypeResponseModel>>.Success(new PagedList<BankTypeResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankTypeResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }
}

