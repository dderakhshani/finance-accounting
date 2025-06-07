using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.Bank.Queries
{
    public class GetBankQuery : IRequest<ServiceResult<BankResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBankQueryHandler : IRequestHandler<GetBankQuery, ServiceResult<BankResponseModel>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetBankQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<BankResponseModel>> Handle(GetBankQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.Banks_View
                 .ProjectTo<BankResponseModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<BankResponseModel>.Success(r);
        }
    }

}
