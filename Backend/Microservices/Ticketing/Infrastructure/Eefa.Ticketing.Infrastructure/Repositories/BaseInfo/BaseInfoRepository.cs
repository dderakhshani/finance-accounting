using Eefa.Ticketing.Domain.Core.Dtos.BaseInfo;
using Eefa.Ticketing.Domain.Core.Entities.BaseInfo;
using Eefa.Ticketing.Domain.Core.Interfaces.BaseInfo;
using Eefa.Ticketing.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Infrastructure.Repositories.BaseInfo
{
    public class BaseInfoRepository : BaseRepository<Role, int>, IBaseInfoRepository
    {
        private readonly EefaTicketingContext _context;
        public BaseInfoRepository(EefaTicketingContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _context.Roles.ToListAsync();
        }
        public async Task<List<GetUserDataDto>> GetUsersByRoleIdAsync(int RoleId)
        {
            var users = await (
                    from u in _context.Users
                    join ur in _context.UserRoles on u.Id equals ur.UserId
                    join p in _context.Persons on u.PersonId equals p.Id
                    where ur.RoleId == RoleId
                    select new GetUserDataDto() { Id = u.Id, PersonId = u.PersonId, Username = u.Username, FirstName = p.FirstName, LastName = p.LastName, FullName = p.FirstName + " " + p.LastName }
                    ).ToListAsync();
            return users;
        }
        public async Task<Role> GetRoleById(int Id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == Id);
        }
        public async Task<List<GetUsersByIdsObjResultDto>> GetUsersByIds(List<int> UserIds)
        {

            var result = await (
                  from u in _context.Users
                  join ur in _context.UserRoles on u.Id equals ur.UserId
                  join r in _context.Roles on ur.RoleId equals r.Id
                  join p in _context.Persons on u.PersonId equals p.Id
                  where UserIds.Contains(u.Id)
                  select new GetUsersByIdsObjResultDto()
                  {
                      Fullname = p.FirstName + " " + p.LastName,
                      UserId = u.Id,
                      RoleId = r.Id,
                      RoleTitle = r.Title
                  }).ToListAsync();
            return result;
        }
        public async Task<List<int>> GetTreeRole(int RoleId)
        {
            var levelcode = await _context.Roles.Where(a => a.Id == RoleId)
               .Select(a => a.LevelCode).FirstOrDefaultAsync();
            var all = await _context.Roles.Where(a => a.LevelCode.StartsWith(levelcode))
                .Select(a =>
                    a.Id
                ).ToListAsync();
            return all;
        }
    }
}
