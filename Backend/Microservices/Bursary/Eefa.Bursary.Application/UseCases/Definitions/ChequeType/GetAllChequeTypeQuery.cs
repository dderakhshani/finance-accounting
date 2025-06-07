using AutoMapper;
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

namespace Eefa.Bursary.Application.UseCases.Definitions.ChequeType
{
    public class GetAllChequeTypeQuery : Pagination, IRequest<ServiceResult<PagedList<ChequeTypes_View>>>, IQuery
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllChequeTypeQueryHandler : IRequestHandler<GetAllChequeTypeQuery, ServiceResult<PagedList<ChequeTypes_View>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ChequeTypes_View> _repository;

        public GetAllChequeTypeQueryHandler(IMapper mapper, IRepository<ChequeTypes_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<ChequeTypes_View>>> Handle(GetAllChequeTypeQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult<PagedList<ChequeTypes_View>>.Success(new PagedList<ChequeTypes_View>()
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
