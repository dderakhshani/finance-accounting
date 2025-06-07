using Eefa.Ticketing.Infrastructure.Repositories.BaseInfo;
using Eefa.Ticketing.Infrastructure.Repositories.Tickets;
using System;
using System.Threading.Tasks;

namespace Eefa.Ticketing.Infrastructure.Patterns
{
    public interface IUnitOfWork : IDisposable
    {
        public TicketRepository TicketRepository { get; }
        public TicketDetailRepository TicketDetailRepository { get; }
        public PrivetMessageRepository PrivetMessageRepository { get; }
        public DetailHistoryRepository DetailHistoryRepository { get; }
        public BaseInfoRepository BaseInfoRepository { get; }
        Task SaveChangesAsync();
    }
}
