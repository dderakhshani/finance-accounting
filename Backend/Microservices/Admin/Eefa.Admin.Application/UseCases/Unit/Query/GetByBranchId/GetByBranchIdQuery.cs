using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Unit.Model;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Unit.Query.GetByBranchId
{
    public class GetByBranchIdQuery : IRequest<ServiceResult>, IQuery
    {
        public int BranchId { get; set; }
    }

    public class GetByBranchIdQueryHandler : IRequestHandler<GetByBranchIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetByBranchIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetByBranchIdQuery request, CancellationToken cancellationToken)
        {
            return ServiceResult.Success(await _repository
                .Find<Data.Databases.Entities.Unit>(c
                    => c.ConditionExpression(x=>x.BranchId == request.BranchId)).Include(x => x.UnitPositions)
                .ProjectTo<UnitModel>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken));
        }
    }
}