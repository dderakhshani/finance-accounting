using Eefa.Common.Data.Query;
using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Interfaces.Tickets;
using Eefa.Ticketing.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Infrastructure.Repositories.Tickets
{
    public class TicketRepository : BaseRepository<Ticket, int>, ITicketRepository
    {
        private readonly EefaTicketingContext _context;

        public TicketRepository(EefaTicketingContext context) : base(context)
        {
            _context = context;
        }
        public async Task<int> GetRoleId(int ticketId)
        {
            return await _context.Tickets.Where(a => a.Id == ticketId).Select(a => a.RoleId).FirstOrDefaultAsync();
        }
        public async Task<List<GetTicketListDto>> GetList(GetTicketListInputDto input, List<int> roleIds, CancellationToken cancellationToken)
        {
            var query = (from t in _context.Tickets
                         join d in _context.TicketDetails on t.Id equals d.TicketId
                         join u in _context.Users on t.CreatedById equals u.Id
                         join p in _context.Persons on u.PersonId equals p.Id
                         from h in _context.DetailHistories.Where(a => a.TicketDetailId == d.Id).DefaultIfEmpty()
                         select new { t, d, h,p }).Where(a =>
           (a.t.ReceiverUserId == input.UserId) ||
           (a.t.ReceiverUserId == null && roleIds.Contains(a.t.RoleId)) ||
           (a.t.CreatedById == input.UserId) ||
           roleIds.Contains(a.d.RoleId) ||
           roleIds.Contains(a.h.PrimaryRoleId))
                .Select(a => new GetTicketListDto()
                {
                    Id = a.t.Id,
                    CreateDate = a.t.CreatedAt,
                    CreatorUserId = a.t.CreatedById,
                    RoleId = a.t.RoleId,
                    Priority = (int)a.t.Priority,
                    ReceiverUserId = a.t.ReceiverUserId,
                    Status = (int)a.t.Status,
                    Title = a.t.Title,
                    UpdateDate = a.t.ModifiedAt,
                    RoleTitle = a.t.RoleTitle,
                    CreateUserTitle=a.p.LastName
                }).Distinct().FilterQuery(input.Conditions).OrderByMultipleColumns(input.OrderByProperty).Paginate(input.Paginator());



            return await query.ToListAsync(cancellationToken);
        }
    }
}
