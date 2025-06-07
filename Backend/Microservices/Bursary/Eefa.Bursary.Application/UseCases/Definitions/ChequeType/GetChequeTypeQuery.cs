using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.ChequeType
{
    public class GetChequeTypeQuery : IRequest<ServiceResult<ChequeTypes_View>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetChequeTypeQueryHandler : IRequestHandler<GetChequeTypeQuery, ServiceResult<ChequeTypes_View>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetChequeTypeQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ChequeTypes_View>> Handle(GetChequeTypeQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.ChequeTypes_View
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<ChequeTypes_View>.Success(r);
        }
    }

}