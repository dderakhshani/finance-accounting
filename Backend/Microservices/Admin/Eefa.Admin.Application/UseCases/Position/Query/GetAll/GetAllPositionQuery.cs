using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Position.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Position.Query.GetAll
{
    public class GetAllPositionQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions{ get; set; }

    }

    public class GetAllPositionQueryHandler : IRequestHandler<GetAllPositionQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPositionQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllPositionQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Databases.Entities.Position>()
                .ProjectTo<PositionModel>(_mapper.ConfigurationProvider)
                .WhereQueryMaker(request.Conditions)
                .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
            {
                Data = await entitis
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entitis
                        .CountAsync(cancellationToken)
                    : 0
            });
         
        }
    }
}
