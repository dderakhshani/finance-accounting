using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Accounting.Data.Databases.Sp;
using Library.Interfaces;
using Library.Utility;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Accounting.Data.Databases.SqlServer.Context
{
    public partial class AccountingUnitOfWorkProcedures : IAccountingUnitOfWorkProcedures
    {
        private readonly AccountingUnitOfWork _accountingUnitOfWork;
        private readonly DanaAccountingUnitOfWork _danaAccountingUnitOfWork;
        private readonly ICurrentUserAccessor _currentUser;
        // Remove For Chocolate Factory Project

        public AccountingUnitOfWorkProcedures(AccountingUnitOfWork context, DanaAccountingUnitOfWork danaAccountingUnitOfWork, ICurrentUserAccessor currentUser)
        {
            _accountingUnitOfWork = context;
            _danaAccountingUnitOfWork = danaAccountingUnitOfWork;
            _currentUser = currentUser;
        }


        public async Task<List<StpReportBalance6Result>> StpReportBalance6Async<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var parameters = entity.EntityToSqlParameters().Select(x =>
            {
                x.ParameterName = "@" + x.ParameterName;
                return x;
            }).ToArray();
            
            if (_currentUser.GetYearId() == 3)
            {
                return  _danaAccountingUnitOfWork.StpReportBalance6Result.FromSqlRaw($"EXEC [accounting].[Stp_Acc_ReportBalance6]" + string.Join(", ", parameters.Select(x => x.ParameterName).ToArray()), parameters)
                    .AsQueryable()
                    .AsEnumerable()
                    .Where(x => x.Credit > 0 || x.Debit > 0 || x.CreditBeforeDate > 0 || x.DebitBeforeDate > 0)
                    .ToList();
            }
            else
            {
                return _accountingUnitOfWork.StpReportBalance6Result.FromSqlRaw($"EXEC [accounting].[Stp_Acc_ReportBalance6] " + string.Join(", ", parameters.Select(x => x.ParameterName).ToArray()), parameters)
                    .AsQueryable()
                    .AsEnumerable()
                    .Where(x => x.Credit > 0 || x.Debit > 0 || x.CreditBeforeDate > 0 || x.DebitBeforeDate > 0)
                    .ToList();

            }
        }

        public async Task<List<StpReportLedgerResult>> StpReportLedgerAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var parameters = entity.EntityToSqlParameters();
            if (_currentUser.GetYearId() == 3)
            {
                return await _danaAccountingUnitOfWork.ExecuteSqlQueryAsync<StpReportLedgerResult>($"EXEC [accounting].[Stp_Acc_ReportLedger] {QueryUtility.SqlParametersToQuey(parameters)}",
                     parameters,
                     cancellationToken);
            }
            else
            {
                return await _accountingUnitOfWork.ExecuteSqlQueryAsync<StpReportLedgerResult>($"EXEC [accounting].[Stp_Acc_ReportLedger] {QueryUtility.SqlParametersToQuey(parameters)}",
                        parameters,
                        cancellationToken);
            }
        }

        public async Task<List<StpReportReference2AccountHeadResult>> StpReportReference2AccountHeadAsync<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var parameters = entity.EntityToSqlParameters();

            if (_currentUser.GetYearId() == 3)
            {
                return await _danaAccountingUnitOfWork.ExecuteSqlQueryAsync<StpReportReference2AccountHeadResult>($"EXEC [accounting].[Stp_Acc_ReportReference2AccountHead] {QueryUtility.SqlParametersToQuey(parameters)}",
                 parameters,
                 cancellationToken);
            }
            else
            {
                return await _accountingUnitOfWork.ExecuteSqlQueryAsync<StpReportReference2AccountHeadResult>($"EXEC [accounting].[Stp_Acc_ReportReference2AccountHead] {QueryUtility.SqlParametersToQuey(parameters)}",
                  parameters,
                  cancellationToken);
            }
        }

        public async Task<List<StpAccountReferenceBookResult>> StpAccountReferenceBook<TEntity>(TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var parameters = entity.EntityToSqlParameters();

            if (_currentUser.GetYearId() == 3)
            {
                return await _danaAccountingUnitOfWork.ExecuteSqlQueryAsync<StpAccountReferenceBookResult>($"EXEC [accounting].[Stp_Acc_AccountReferenceBook] {QueryUtility.SqlParametersToQuey(parameters)}",
             parameters,
             cancellationToken);
            }
            else
            {
                return await _accountingUnitOfWork.ExecuteSqlQueryAsync<StpAccountReferenceBookResult>($"EXEC [accounting].[Stp_Acc_AccountReferenceBook] {QueryUtility.SqlParametersToQuey(parameters)}",
                parameters,
                cancellationToken);
            }
        }

        public async Task StpUpdateTotalDebitCreditOnVoucherDetail(CancellationToken cancellationToken = default)
        {

            if (_currentUser.GetYearId() == 3)
            {
                await _danaAccountingUnitOfWork.ExecuteSqlQueryAsync($"EXEC [accounting].[StpUpdateTotalDebitCreditOnVoucherDetail]", null, cancellationToken);
            }
            else
            {
                await _accountingUnitOfWork.ExecuteSqlQueryAsync($"EXEC [accounting].[StpUpdateTotalDebitCreditOnVoucherDetail]", null, cancellationToken);
            }
        }
    }
}