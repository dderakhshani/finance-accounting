using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Employee.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Employee.Query.GetByPersonId
{
    public class GetByPersonIdQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetByPersonIdQueryHandler : IRequestHandler<GetByPersonIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetByPersonIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetByPersonIdQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Employee>(c
                    => c.ConditionExpression(x=>x.PersonId == request.Id))
                .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }

      
    }
}