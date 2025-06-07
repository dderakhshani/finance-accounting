using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.Role.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.Role.Query.GetAll
{
    public class GetAllRoleQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions{ get; set; }

    }

    public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoleQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var entitis = _repository
                .GetAll<Data.Databases.Entities.Role>()
                .ProjectTo<RoleModel>(_mapper.ConfigurationProvider)
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
