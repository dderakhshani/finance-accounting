using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Payables.ChequeBooks.ChequeBookSheets.Models;
using Eefa.Bursary.Application.UseCases.Payables.PayRequests.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Payables.PayRequests.Queries
{
    public class GetPayRequestQuery : IRequest<ServiceResult<PayRequestResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPayRequestQueryHandler : IRequestHandler<GetPayRequestQuery, ServiceResult<PayRequestResponseModel>>
    {
        private readonly IMapper _mapper;
        private readonly IBursaryUnitOfWork _uow;

        public GetPayRequestQueryHandler(IMapper mapper, IBursaryUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<ServiceResult<PayRequestResponseModel>> Handle(GetPayRequestQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_PayRequests_View
                .ProjectTo<PayRequestResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<PayRequestResponseModel>.Success(r);
        }
    }

}
