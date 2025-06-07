using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Accounts.Models;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.CommandQueries.Accounts.Queries.Get
{


    public class GetAccountQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }

    }

    public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAccountQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
               .Find<Data.Databases.Entities.AccountReference>(c
           => c.ObjectId(request.Id))
           .ProjectTo<AccountModel>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
