using Eefa.Bursary.Domain.OutboxEntities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Interfaces
{
    public interface IOutboxRepository
    {
        void Insert(OutboxMessages message);
        Task SaveAsync(CancellationToken cancellationToken);
        Task<List<OutboxMessages>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken);
        Task<List<OutboxMessages>> GetProcessedMessagesByAccountingAsync(CancellationToken cancellationToken);
        Task MarkAsProcessedAsync(int messageId,int voucherHeadId, CancellationToken cancellationToken);
        Task MarkAsProcessedByBursaryAsync(int messageId, CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        void CommitTransaction(IDbContextTransaction transaction);
        void RollbackTransaction(IDbContextTransaction transaction);

    }
}
