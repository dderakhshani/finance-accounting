using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Admin.Application.CommandQueries.User.Model;
using Eefa.Persistence.Data.SqlServer.QueryProvider;
using Library.Interfaces;
using Library.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Admin.Application.CommandQueries.User.Query.GetAllByRoleId
{
    public class GetAllUserByRoleQuery : Pagination, IRequest<ServiceResult>, ISearchableRequest, IQuery
    {
        public int RoleId { get; set; }
    }

    public class GetAllUserByRoleQueryHandler : IRequestHandler<GetAllUserByRoleQuery, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUserByRoleQueryHandler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetAllUserByRoleQuery request, CancellationToken cancellationToken)
        {
            var temp = (
                    from u in _repository.GetQuery<Data.Databases.Entities.User>()
                    join ur in _repository.GetQuery<Data.Databases.Entities.UserRole>()
                        on u.Id equals ur.UserId
                        join r in _repository.GetQuery<Data.Databases.Entities.Role>()
                            on ur.RoleId equals r.Id
                            where r.Id == request.RoleId
                    select u
                    );


            var entitis = temp
                .ProjectTo<UserModel>(_mapper.ConfigurationProvider)
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
