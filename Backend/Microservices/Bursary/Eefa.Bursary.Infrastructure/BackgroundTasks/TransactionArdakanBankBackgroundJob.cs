using Eefa.Bursary.Application.Interfaces;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eefa.Bursary.Infrastructure.BackgroundTasks
{
    [DisallowConcurrentExecution]
    public class TransactionArdakanBankBackgroundJob : IJob
    {

        private readonly ITransactionBankServices _syncService;

        public TransactionArdakanBankBackgroundJob(ITransactionBankServices syncService)
        {
            _syncService = syncService;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                 await _syncService.SyncArdakanBankTransactions();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Job: {ex.Message}");
            }
        }
    }
}
