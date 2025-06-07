using AutoMapper;
using Eefa.Bursary.Domain.Entities.Definitions;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.CurrencyType.Queries
{
    public class GetCurrencyTypeQuerry : IRequest<ServiceResult<CurrencyTypes_View>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCurrencyTypeQuerryHandler : IRequestHandler<GetCurrencyTypeQuerry, ServiceResult<CurrencyTypes_View>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCurrencyTypeQuerryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<CurrencyTypes_View>> Handle(GetCurrencyTypeQuerry request, CancellationToken cancellationToken)
        {
            var r = await _uow.CurrencyTypes_View
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<CurrencyTypes_View>.Success(r);
        }
    }

}
