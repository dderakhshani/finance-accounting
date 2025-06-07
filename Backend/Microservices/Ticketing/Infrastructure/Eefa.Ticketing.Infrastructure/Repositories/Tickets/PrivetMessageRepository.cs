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
    public class PrivetMessageRepository : BaseRepository<PrivetMessage, int>, IPrivetMessageRepository
    {
        private readonly EefaTicketingContext _context;
        public PrivetMessageRepository(EefaTicketingContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<GetPrivetMessageListDto>> GetPrivetMessageList(int ticketDitailId, CancellationToken cancellationToken)
        {
            return await _context.PrivetMessages.Where(a => a.TicketDetailId == ticketDitailId).Select(a => new GetPrivetMessageListDto()
            {
                TicketDetailId = a.TicketDetailId,
                CreatDate = a.CreatedAt,
                CreatorUserId = a.CreatedById,
                Id = a.Id,
                Message = a.Message,
            }).ToListAsync();
        }

        public async Task<GetCreatorUserAndRoleIdDto> GetCreatorUserAndRoleId(int ticketDitailId, CancellationToken cancelationtoken)
        {
            var ticket = await _context.PrivetMessages.Where(a => a.TicketDetailId == ticketDitailId).Select(a => new GetCreatorUserAndRoleIdDto()
            {
                CreatorUserId = a.CreatedById
            }).FirstOrDefaultAsync(cancelationtoken);
            return ticket;
        }
    }
}
