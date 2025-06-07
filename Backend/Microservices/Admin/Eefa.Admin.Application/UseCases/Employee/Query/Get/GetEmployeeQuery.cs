using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Employee.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Employee.Query.Get
{
    public class GetEmployeeQuery : IRequest<ServiceResult>, IQuery
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }
    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetEmployeeQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                return ServiceResult.Success(await _repository
                    .Find<Data.Databases.Entities.Employee>(c
                        => c.ObjectId(request.Id))
                    .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken));
            }
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Employee>(c
                    => c.ConditionExpression(x => x.PersonId == request.PersonId || x.Id == request.EmployeeId))
                .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken));
        }
    }
}
