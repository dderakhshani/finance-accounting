using Eefa.Bursary.Application.Interfaces;
using Eefa.Bursary.Domain.OutboxEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly OutboxDbContext _context;

        public OutboxRepository(OutboxDbContext context)
        {
            _context = context;
        }

        public void Insert(OutboxMessages message)
        {
          _context.Set<OutboxMessages>().Add(message);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<OutboxMessages>> GetUnprocessedMessagesAsync(CancellationToken cancellationToken)
        {
            return
                await _context.Set<OutboxMessages>()
                .Where(m => !m.IsProcessed && m.VoucherHeadId == null)
                .ToListAsync(cancellationToken);
        }


        public async Task<List<OutboxMessages>> GetProcessedMessagesByAccountingAsync(CancellationToken cancellationToken)
        {
            return
                await _context.Set<OutboxMessages>()
                .Where(m => m.IsProcessed && m.VoucherHeadId != null && m.IsBursaryUpdated == false)
                .ToListAsync(cancellationToken);
        }




        public async Task MarkAsProcessedAsync(int messageId, int voucherHeadId, CancellationToken cancellationToken)
        {
            var message = await _context.Set<OutboxMessages>().FindAsync(new object[] { messageId }, cancellationToken);
            if (message != null)
            {
                message.IsProcessed = true;
                message.ProcessedAt = DateTime.UtcNow;
                message.VoucherHeadId = voucherHeadId;  
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        public async Task MarkAsProcessedByBursaryAsync(int messageId, CancellationToken cancellationToken)
        {
            var message = await _context.Set<OutboxMessages>().FindAsync(new object[] { messageId }, cancellationToken);
            if (message != null)
            {
                message.IsBursaryUpdated = true;
                
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

         
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(IDbContextTransaction transaction)
        {
            transaction.Rollback();
        }



    }
}
