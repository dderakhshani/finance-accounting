using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Inventory.Domain;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Inventory.Application
{
    public class PermissionsQueries : IPermissionsQueries
    {

        private readonly IRepository<Permissions> _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;
        private readonly IInvertoryUnitOfWork _invertoryUnitOfWork;
        public PermissionsQueries(
            IRepository<Permissions> context,
            ICurrentUserAccessor currentUserAccessor,
            IMapper mapper,
            IInvertoryUnitOfWork invertoryUnitOfWork)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _context = context;
            _invertoryUnitOfWork = invertoryUnitOfWork;
        }

        public async Task<PermissionsModel> GetById(int id)
        {
            var entity = await _context.Find(id);
            return _mapper.Map<PermissionsModel>(entity);
        }

        public async Task<PagedList<PermissionsModel>> GetAll(PaginatedQueryModel query)
        {
            var roleId = _currentUserAccessor.GetRoleId();
            var entitis = _context.GetAll()//c => c.ConditionExpression(x => x.OwnerRoleId == roleId)
                           .ProjectTo<PermissionsModel>(_mapper.ConfigurationProvider)
                           .FilterQuery(query.Conditions)
                           .OrderByMultipleColumns(query.OrderByProperty);
            return new PagedList<PermissionsModel>()
            {

                Data = await entitis.Paginate(query.Paginator()).ToListAsync(),
                TotalCount = query.PageIndex <= 1
                    ? await entitis
                        .CountAsync()
                    : 0
            };
        }

        public IQueryable<User> GetAllUserByPermision(int permisionId)
        {
            var users = (from rp in _invertoryUnitOfWork.Set<RolePermission>()
                         join ru in _invertoryUnitOfWork.Set<UserRole>() on rp.RoleId equals ru.RoleId
                         join u in _invertoryUnitOfWork.Set<User>() on ru.UserId equals u.Id
                         where rp.PermissionId == permisionId
                         select u);
            return users;

        }
    }
    }
    
    

