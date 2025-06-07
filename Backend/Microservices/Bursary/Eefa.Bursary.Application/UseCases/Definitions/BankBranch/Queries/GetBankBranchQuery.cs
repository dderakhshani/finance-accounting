using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Models;
using Eefa.Bursary.Infrastructure.Interfaces;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.UseCases.Definitions.BankBranch.Queries
{
    public class GetBankBranchQuery : IRequest<ServiceResult<BankBranchResponseModel>>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBankBranchQueryHandler : IRequestHandler<GetBankBranchQuery, ServiceResult<BankBranchResponseModel>>
    {
        private readonly IBursaryUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetBankBranchQueryHandler(IBursaryUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ServiceResult<BankBranchResponseModel>> Handle(GetBankBranchQuery request, CancellationToken cancellationToken)
        {
            var r = await _uow.BankBranches_View
                 .ProjectTo<BankBranchResponseModel>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(w => w.Id == request.Id);
            return ServiceResult<BankBranchResponseModel>.Success(r);
        }
    }


}
