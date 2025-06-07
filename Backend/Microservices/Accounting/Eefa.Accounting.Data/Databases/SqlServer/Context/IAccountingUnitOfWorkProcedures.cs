using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Data.Databases.Sp;

namespace Eefa.Accounting.Data.Databases.SqlServer.Context
{
    public interface IAccountingUnitOfWorkProcedures
    {
        Task<List<StpReportBalance6Result>> StpReportBalance6Async<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default
        );

  
        Task<List<StpReportLedgerResult>> StpReportLedgerAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default
        );

        Task StpUpdateTotalDebitCreditOnVoucherDetail(
           CancellationToken cancellationToken = default
       );

        Task<List<StpReportReference2AccountHeadResult>> StpReportReference2AccountHeadAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default
        );
        
        Task<List<StpAccountReferenceBookResult>> StpAccountReferenceBook<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default
        );
    }
}