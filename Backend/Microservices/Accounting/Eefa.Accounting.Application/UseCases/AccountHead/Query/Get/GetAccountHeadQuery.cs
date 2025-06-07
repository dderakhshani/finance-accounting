using AutoMapper;
using Eefa.Accounting.Application.UseCases.AccountHead.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Accounting.Application.UseCases.AccountHead.Query.Get
{
    public class GetAccountHeadQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetAccountHeadQueryHandler : IRequestHandler<GetAccountHeadQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public GetAccountHeadQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAccountHeadQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Entities.AccountHead>(c
            => c.ObjectId(request.Id)).Include(x=>x.Parent)
            .ProjectTo<AccountHeadModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
