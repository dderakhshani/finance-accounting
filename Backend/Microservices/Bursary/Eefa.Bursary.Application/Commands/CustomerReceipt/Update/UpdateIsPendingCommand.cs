using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Update
{
    //TODO: when RabbitMQ starts to work this command must replace with that

    /*
      before start to insert AccountingDocument :

      FIRST : all bursary documents must be in IsPending mode (THIS COMMAND COVER THIS FIRST STEP)
      SECOND : all Accounting Documents insert 
      THIRD : all bursary documents with IsPending mode must be done (UpdateVoucherHeadIdCommand)

      we use of this way to track all documents if any breaks happened */

    public class UpdateIsPendingCommand : Common.CommandQuery.CommandBase,
      IRequest<ServiceResult>,
      IMapFrom<UpdateIsPendingCommand>, ICommand
    {
        public List<int> ReceiveIds { get; set; }
  
        public class UpdateIsPendingCommandHandler : IRequestHandler<UpdateIsPendingCommand,
            ServiceResult>
        {
            private readonly IMapper _autoMaper;
            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> _financialRepository;

 
            public UpdateIsPendingCommandHandler(IMapper autoMaper,
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository)
            {
                _autoMaper = autoMaper;
                _financialRepository = financialRepository;
            }

            public async Task<ServiceResult> Handle(
                UpdateIsPendingCommand request, CancellationToken cancellationToken)
            {

                var financialList =await (from f in _financialRepository.GetAll()
                        where request.ReceiveIds.Contains(f.Id)
                        select f
                    ).ToListAsync(cancellationToken);
                financialList.ForEach(x=>x.IsPending = true);

                await _financialRepository.SaveChangesAsync(cancellationToken);
                return ServiceResult.Success();

            }

 
 

        }
    }

}
