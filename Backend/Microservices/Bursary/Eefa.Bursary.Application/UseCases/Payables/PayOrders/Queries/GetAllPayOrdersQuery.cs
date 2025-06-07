using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Application.UseCases.Payables.PayOrders.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.PayOrders.Queries
{
    public class GetAllPayOrdersQuery : Pagination, IRequest<ServiceResult<PagedList<PayOrderResponseModel>>>
    {
        public List<QueryCondition> Conditions { get; set; }
    }

    public class GetAllPayOrdersQueryHandler : IRequestHandler<GetAllPayOrdersQuery, ServiceResult<PagedList<PayOrderResponseModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Payables_PayOrders_View> _repository;

        public GetAllPayOrdersQueryHandler(IMapper mapper, IRepository<Payables_PayOrders_View> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult<PagedList<PayOrderResponseModel>>> Handle(GetAllPayOrdersQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll()
                .ProjectTo<PayOrderResponseModel>(_mapper.ConfigurationProvider)
                .FilterQuery(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);
            return ServiceResult<PagedList<PayOrderResponseModel>>.Success(new PagedList<PayOrderResponseModel>()
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
