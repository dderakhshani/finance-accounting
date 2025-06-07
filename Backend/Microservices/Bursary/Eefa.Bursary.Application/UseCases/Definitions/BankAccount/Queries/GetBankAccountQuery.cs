using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries
{
    public class GetBankAccountQuery : IRequest<ServiceResult<BankAccountsResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBankAccountQueryHandler : IRequestHandler<GetBankAccountQuery, ServiceResult<BankAccountsResponseModel>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetBankAccountQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<BankAccountsResponseModel>> Handle(GetBankAccountQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.BankAccounts_View
                 .ProjectTo<BankAccountsResponseModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<BankAccountsResponseModel>.Success(r);
        }
    }


}
