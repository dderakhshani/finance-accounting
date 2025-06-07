using Library.Common;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.Combine
{
    public class CombineVoucherHeadsCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int MainVoucherId { get; set; }
        public int MainVoucherNo { get; set; }
        public List<int> VoucherHeadIdsToCombine { get; set; }
    }


    public class CombineVoucherHeadsCommandHandler : IRequestHandler<CombineVoucherHeadsCommand, ServiceResult>
    {
        private readonly ICurrentUserAccessor currentUser;

        public CombineVoucherHeadsCommandHandler(IUnitOfWork accountingUnitOfWork, ICurrentUserAccessor currentUser)
        {
            _accountingUnitOfWork = accountingUnitOfWork;
            this.currentUser = currentUser;
        }

        public IUnitOfWork _accountingUnitOfWork { get; }

        public async Task<ServiceResult> Handle(CombineVoucherHeadsCommand request, CancellationToken cancellationToken)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter
            {
                ParameterName = "UserId",
                Value = currentUser.GetId()
            });
            parameters.Add(new SqlParameter
            {
                ParameterName = "NewVoucherId",
                Value = request.MainVoucherId
            });
            parameters.Add(new SqlParameter
            {
                ParameterName = "NewVoucherNo",
                Value = request.MainVoucherNo
            });
     
            parameters.Add(new SqlParameter
            {
                ParameterName = "OldVouchersId",
                Value = String.Join(",",request.VoucherHeadIdsToCombine)
            });


            var response = await _accountingUnitOfWork.ExecuteSqlQueryAsync<object>($"EXEC [accounting].[Stp_MergeVouchersId]  {QueryUtility.SqlParametersToQuey(parameters)}",
                    parameters.ToArray(),
                    cancellationToken);

            return ServiceResult.Success();
        }
    }
}
