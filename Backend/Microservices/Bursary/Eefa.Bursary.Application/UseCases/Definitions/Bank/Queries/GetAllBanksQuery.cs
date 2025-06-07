using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries
{
    public class GetAllBanksQuery : Pagination, IRequest<ServiceResult<PagedList<BankResponseModel>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllBanksQueryHandler : IRequestHandler<GetAllBanksQuery, ServiceResult<PagedList<BankResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Banks_View> _repository;

        public GetAllBanksQueryHandler(IMapper mapper, IRepository<Banks_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<BankResponseModel>>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<BankResponseModel>>.Success(new PagedList<BankResponseModel>()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ProjectTo<BankResponseModel>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
        }
    }


}
