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
    public class TicketDetailRepository : BaseRepository<TicketDetail, int>, ITicketDetailRepository
    {
        private readonly EefaTicketingContext _context;
        public TicketDetailRepository(EefaTicketingContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<GetTicketDetailListDto>> GetList(int ticketId, CancellationToken cancelationtoken)
        {

            var query = (from d in _context.TicketDetails
                         join t in _context.Tickets on d.TicketId equals t.Id
                         from h in _context.DetailHistories.Where(a => a.TicketDetailId == d.Id).DefaultIfEmpty()
                         select new { d, t, h }).GroupBy(a => new
                         {
                             a.d.Id,
                             a.d.TicketId,
                             a.d.AttachmentIds,
                             a.d.CreatedAt,
                             DetailCreatorUserId = a.d.CreatedById,
                             a.d.RoleId,
                             a.d.Description,
                             a.d.ReadDate,
                             a.d.ReaderUserId,
                             a.t.Title,
                             TicketCreatorUserId = a.t.CreatedById,
                             a.d.OwnerRoleId,
                             a.t.Status
                         }).Select(a => new GetTicketDetailListDto()
                         {
                             TicketId = a.Key.TicketId,
                             Id = a.Key.Id,
                             AttachmentIds = a.Key.AttachmentIds,
                             CreatDate = a.Key.CreatedAt,
                             DetailCreatorUserId = a.Key.DetailCreatorUserId,
                             RoleId = a.Key.RoleId,
                             Description = a.Key.Description,
                             ReadDate = a.Key.ReadDate,
                             ReaderUserId = a.Key.ReaderUserId,
                             TicketTitle = a.Key.Title,
                             TicketCreatorUserId = a.Key.TicketCreatorUserId,
                             DetailCreatorUserRoleId = a.Key.OwnerRoleId,
                             TicketStatus = a.Key.Status,
                             HistoryCount = a.Count(f => f.h.Id != null)
                         }).Where(a => a.TicketId == ticketId).OrderByDescending(a => a.Id).AsQueryable();

            return await query.ToListAsync();
        }

        public async Task<List<TicketDetail>> GetUnreadListAsync(int ticketId, CancellationToken cancelationtoken)
        {
            return await _context.TicketDetails.Where(a => a.TicketId == ticketId && a.ReadDate == null).ToListAsync();
        }
        public async Task<GetCreatorUserAndRoleIdDto> GetCreatorUserAndRoleId(int id, CancellationToken cancelationtoken)
        {
            var ticket = await _context.TicketDetails.Where(a => a.Id == id).Select(a => new GetCreatorUserAndRoleIdDto()
            {
                CreatorUserId = a.CreatedById,
                RoleId =
                a.RoleId
            }).FirstOrDefaultAsync(cancelationtoken);
            return ticket;
        }
    }
}
