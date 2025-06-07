using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Application.UseCases.AccountReferencesGroup.Query.Get
{
    public class GetAccountReferencesGroupQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAccountReferencesGroupQueryHandler : IRequestHandler<GetAccountReferencesGroupQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAccountReferencesGroupQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAccountReferencesGroupQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Eefa.Accounting.Data.Entities.AccountReferencesGroup>(c
            => c.ObjectId(request.Id))
            .ProjectTo<AccountReferencesGroupModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
