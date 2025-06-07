using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.PersonCustomer.Models;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.PersonCustomer.Queries.Get
{
    public class GetPersonCustomerQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPersonCustomerQueryHandler : IRequestHandler<GetPersonCustomerQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetPersonCustomerQueryHandler(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(GetPersonCustomerQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Customer>(c
                    => c.ObjectId(request.Id))
                .ProjectTo<PersonCustomerModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
