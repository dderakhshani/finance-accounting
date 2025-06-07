using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Bursary.Application.Models.Enums;
using Eefa.Bursary.Infrastructure;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Update
{

    //TODO: when RabbitMQ starts to work this command must replace with that

    /*
      before start to insert AccountingDocument :
      FIRST : all bursary documents must be in IsPending mode (UpdateIsPendingCommand)
      
      SECOND : all Accounting Documents insert 

      THIRD : all bursary documents with IsPending mode must be done and AccountHeadId in FinancialRequest Must update (THIS COMMAND COVER THIS FIRST STEP)

      we use of this way to track all documents if any breaks happened */

    public class UpdateVoucherHeadIdCommand : Common.CommandQuery.CommandBase,
        IRequest<ServiceResult>,
        IMapFrom<UpdateVoucherHeadIdCommand>, ICommand
    {
        public List<int> ReceiveIds { get; set; }
        public int VoucherHeadId { get; set; }
        public int? TransactionId { get; set; }

        public class UpdateVoucherHeadIdCommandHandler : IRequestHandler<UpdateVoucherHeadIdCommand,
            ServiceResult>
        {
            private readonly IMapper _autoMaper;
            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;
           // private readonly IApplicationLogs _applicationLogs;
            private readonly ILogger<UpdateVoucherHeadIdCommand> _logger;

            public UpdateVoucherHeadIdCommandHandler(IMapper autoMaper,
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
                ILogger<UpdateVoucherHeadIdCommand> logger)
            {
                _autoMaper = autoMaper;
                _financialRepository = financialRepository;
                _logger = logger;
            }

            public async Task<ServiceResult> Handle(
                UpdateVoucherHeadIdCommand request, CancellationToken cancellationToken)
            {

                //  await _applicationLogs.CommitLog(request);
                using var transaction = await _financialRepository.BeginTransactionAsync();
                try
                {
                    var financialList = await (from f in _financialRepository.GetAll()
                        where request.ReceiveIds.Contains(f.Id)
                        select f
                    ).ToListAsync(cancellationToken);

                foreach (var item in financialList)
                {
                    item.VoucherHeadId = request.VoucherHeadId;
                    item.AutomateState = (short?)AutomateEnum.SaveByUser;
                    item.IsPending = false;
                        if (request.TransactionId != null && request.TransactionId > 0)
                        {
                            item.TransactionId = request.TransactionId;
                            item.AutomateState = (short?)AutomateEnum.SaveBySystem;
                        }
                }

                    await _financialRepository.SaveAsync(cancellationToken);
                     _financialRepository.CommitTransaction(transaction);

                    return ServiceResult.Success();
                }
                catch(Exception ex)
                {
                        _financialRepository.RollbackTransaction(transaction);
                    _logger.LogError(ex, "*****", DateTime.Now);

                    return ServiceResult.Failed();

                }


            }

      
        }
    }



  
}
