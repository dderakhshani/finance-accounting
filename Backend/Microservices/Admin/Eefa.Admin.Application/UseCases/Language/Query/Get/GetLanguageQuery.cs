using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Language.Model;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;


namespace Eefa.Admin.Application.CommandQueries.Language.Query.Get
{
    public class GetLanguageQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetLanguageQueryHandler : IRequestHandler<GetLanguageQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetLanguageQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetLanguageQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Language>(c
            => c.ObjectId(request.Id))
            .ProjectTo<LanguageModel>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
