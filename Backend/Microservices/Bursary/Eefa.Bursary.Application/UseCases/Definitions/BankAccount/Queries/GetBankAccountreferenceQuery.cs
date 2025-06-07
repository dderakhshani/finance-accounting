using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.Bank.Models;
using Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankAccount.Queries
{
    public class GetBankAccountreferenceQuery : IRequest<ServiceResult<BankAccountReferenceResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBankAccountQueryandler : IRequestHandler<GetBankAccountreferenceQuery, ServiceResult<BankAccountReferenceResponseModel>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetBankAccountQueryandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<BankAccountReferenceResponseModel>> Handle(GetBankAccountreferenceQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.BankAccountReferences_View
                 .ProjectTo<BankAccountReferenceResponseModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<BankAccountReferenceResponseModel>.Success(r);
        }
    }


}
