using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Library.Interfaces;
using Library.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonBankAccounts.Queries.Get
{
    public class GetPersonBankAccountQuery: IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPersonBankAccountQueryHandler : IRequestHandler<GetPersonBankAccountQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetPersonBankAccountQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(GetPersonBankAccountQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.PersonBankAccount>(c
                    => c.ObjectId(request.Id))
                .ProjectTo<PersonBankAccountModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
