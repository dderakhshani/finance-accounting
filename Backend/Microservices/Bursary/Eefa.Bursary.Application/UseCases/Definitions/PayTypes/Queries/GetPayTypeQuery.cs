using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Models;
using Eefa.Bursary.Domain.Entities.Payables;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.PayTypes.Queries
{
    public class GetPayTypeQuery : IRequest<ServiceResult<Payables_PayTypesResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPayTypeQueryHandler : IRequestHandler<GetPayTypeQuery, ServiceResult<Payables_PayTypesResponseModel>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetPayTypeQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<Payables_PayTypesResponseModel>> Handle(GetPayTypeQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Payables_PayTypes_View
                 .ProjectTo<Payables_PayTypesResponseModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<Payables_PayTypesResponseModel>.Success(r);
        }
    }

}
