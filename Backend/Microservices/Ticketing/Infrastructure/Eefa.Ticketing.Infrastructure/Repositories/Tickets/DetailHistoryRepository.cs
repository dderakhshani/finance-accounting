using Eefa.Ticketing.Domain.Core.Dtos.Tickets;
using Eefa.Ticketing.Domain.Core.Entities.Tickets;
using Eefa.Ticketing.Domain.Core.Interfaces.Tickets;
using Eefa.Ticketing.Infrastructure.Database.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Eefa.Ticketing.Infrastructure.Repositories.Tickets
{
    public class DetailHistoryRepository : BaseRepository<DetailHistory, int>, IDetailHistoryRepository
    {
        private readonly EefaTicketingContext _context;
        public DetailHistoryRepository(EefaTicketingContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public List<GetTicketDetailHistoryListDto> GetHistoriesIncludeMessage(int ticketDetailId, CancellationToken cancellationToken)
        {
            return _context.DetailHistories.Where(a => a.TicketDetailId == ticketDetailId).Join(_context.PrivetMessages, d => d.Id, p => p.DetailHistoryId, (d, p) => new { d, p })
                  .Select(a => new GetTicketDetailHistoryListDto()
                  {
                      CreatDate = a.d.CreatedAt,
                      HistoryId = a.d.Id,
                      PrimaryRoleId = a.d.PrimaryRoleId,
                      SecondaryRoleId = a.d.SecondaryRoleId,
                      TicketDetailId = a.d.TicketDetailId,
                      Message = a.p.Message,
                  }).ToList();
        }
    }
}
