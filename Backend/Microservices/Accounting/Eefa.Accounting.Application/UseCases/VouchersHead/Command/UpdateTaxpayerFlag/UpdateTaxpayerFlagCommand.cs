using Library.Common;
using Library.Interfaces;
using Library.Models;
using Library.Utility;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Accounting.Application.UseCases.VouchersHead.Command.Combine
{
    public class UpdateTaxpayerFlagCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public List<TaxpayerFlagUpdateModel> TaxpayerFlags { get; set; }
    }

    public class TaxpayerFlagUpdateModel
    {
        public int VoucherDetailId { get; set; }
        public bool Status { get; set; }
    }

    public class UpdateTaxpayerFlagCommandHandler : IRequestHandler<UpdateTaxpayerFlagCommand, ServiceResult>
    {
        private readonly ICurrentUserAccessor currentUser;

        public UpdateTaxpayerFlagCommandHandler(IUnitOfWork accountingUnitOfWork, ICurrentUserAccessor currentUser)
        {
            _accountingUnitOfWork = accountingUnitOfWork;
            this.currentUser = currentUser;
        }

        public IUnitOfWork _accountingUnitOfWork { get; }

        public async Task<ServiceResult> Handle(UpdateTaxpayerFlagCommand request, CancellationToken cancellationToken)
        {
            var voucherDetailsToUpdate = await _accountingUnitOfWork.Set<Data.Entities.VouchersDetail>().Where(x => request.TaxpayerFlags.Select(y=> y.VoucherDetailId).Contains(x.Id)).ToListAsync();

            foreach (var voucherDetail in voucherDetailsToUpdate)
            {
                voucherDetail.TaxpayerFlag = request.TaxpayerFlags.FirstOrDefault(x => x.VoucherDetailId == voucherDetail.Id).Status;
            }

            await _accountingUnitOfWork.SaveAsync(cancellationToken);

            return ServiceResult.Success();
        }
    }
}
