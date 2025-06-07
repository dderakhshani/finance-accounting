using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Application.UseCases.Payables.PayOrders.Models;
using Eefa.Bursary.Domain.Entities;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.PayOrders.Queries
{
    public class GetPayOrderQuery : IRequest<ServiceResult<PayOrderResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPayOrderQueryHandler : IRequestHandler<GetPayOrderQuery, ServiceResult<PayOrderResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetPayOrderQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<PayOrderResponseModel>> Handle(GetPayOrderQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_PayOrders_View.ProjectTo<PayOrderResponseModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<PayOrderResponseModel>.Success(r);
        }
    }

}
