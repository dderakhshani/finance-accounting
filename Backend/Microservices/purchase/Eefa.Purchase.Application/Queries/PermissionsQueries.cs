using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Eefa.Common;
using Eefa.Common.Data;
using Eefa.Common.Data.Query;
using Eefa.Purchase.Application.Models;
using Eefa.Purchase.Application.Queries.Abstraction;
using Eefa.Purchase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Purchase.Application.Queries
{
    public class PermissionsQueries : IPermissionsQueries
    {

        private readonly IRepository<Permissions> _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public PermissionsQueries(
            IRepository<Permissions> context,
            ICurrentUserAccessor currentUserAccessor,
            IMapper mapper)
        {
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _context = context;
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

       
    }
    }
    
    

