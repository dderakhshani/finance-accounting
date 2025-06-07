using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.UseCases.MenuItem.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Admin.Application.UseCases.MenuItem.Query.GetAllRoleByMenuItemPermissionId
{
    public class GetAllRoleByMenuItemPermissionIdQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public List<Condition> Conditions { get; set; }
        public int? MenuItemPermissionId { get; set; }
    }
    public class GetAllRoleByMenuItemPermissionIdQueryHandler : IRequestHandler<GetAllRoleByMenuItemPermissionIdQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public GetAllRoleByMenuItemPermissionIdQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<ServiceResult> Handle(GetAllRoleByMenuItemPermissionIdQuery request, CancellationToken cancellationToken)
        {
            var entities = _repository.GetAll<Data.Databases.Entities.Role>(x =>
                           x.ConditionExpression(y => y.RolePermissionRoles
                                   .Any(x => x.PermissionId == request.MenuItemPermissionId)))
                           .ProjectTo<MenuItemRoleModel>(_mapper.ConfigurationProvider)
                           .WhereQueryMaker(request.Conditions)
                           .OrderByMultipleColumns(request.OrderByProperty);

            return ServiceResult.Success(new PagedList()
            {
                Data = await entities
                .Paginate(request.Paginator())
                .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                ? await entities
                    .CountAsync(cancellationToken)
                : 0
            });
        }
    }
}
