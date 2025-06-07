using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.PayRequests.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.PayRequests.Queries
{
    public class GetAllPayRequestsQuery : Pagination, IRequest<ServiceResult<PagedList<PayRequestResponseModel>>>
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllPayRequestsQueryHandler : IRequestHandler<GetAllPayRequestsQuery, ServiceResult<PagedList<PayRequestResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_PayRequests_View> _repository;

        public GetAllPayRequestsQueryHandler(IMapper mapper, IRepository<Payables_PayRequests_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<PayRequestResponseModel>>> Handle(GetAllPayRequestsQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .ProjectTo<PayRequestResponseModel>(_mapper.ConfigurationProvider)
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);
            return ServiceResult<PagedList<PayRequestResponseModel>>.Success(new PagedList<PayRequestResponseModel>()
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
