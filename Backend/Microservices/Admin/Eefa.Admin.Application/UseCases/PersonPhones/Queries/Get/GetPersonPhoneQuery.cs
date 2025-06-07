using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonPhones.Models;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonPhones.Queries.Get
{
    public class GetPersonPhoneQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPersonPhoneQueryHandler : IRequestHandler<GetPersonPhoneQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetPersonPhoneQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(GetPersonPhoneQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.PersonPhone>(c
                    => c.ObjectId(request.Id))
                .ProjectTo<PersonPhoneModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
