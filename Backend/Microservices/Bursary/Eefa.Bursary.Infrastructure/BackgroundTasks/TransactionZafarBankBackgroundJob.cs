using Eefa.Bursary.Application.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{

    [DisallowConcurrentExecution]
    public class TransactionZafarBankBackgroundJob : IJob
    {

        private readonly ITransactionBankServices _syncService;

        public TransactionZafarBankBackgroundJob(ITransactionBankServices syncService)
        {
            _syncService = syncService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _syncService.SyncZafarBankTransactions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Job: {ex.Message}");
            }
        }
    }
}
