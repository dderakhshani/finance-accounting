using Eefa.Bursary.Domain.Entities;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Eefa.Bursary.Application.Commands.CustomerReceipt.Update
{
   public class UpdateDescriptionsCommand : Common.CommandQuery.CommandBase, IRequest<ServiceResult>, ICommand
    {
        public class UpdateDescriptionsCommandHandler : IRequestHandler<UpdateDescriptionsCommand, ServiceResult>
        {


            private readonly IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest>
                _financialRepository;

            private readonly IRepository<FinancialRequestDetail> _financialDetailRepository;

            public UpdateDescriptionsCommandHandler(
                IRepository<Domain.Aggregates.FinancialRequestAggregate.FinancialRequest> financialRepository,
                IRepository<Year> yearRepository, IRepository<FinancialRequestDetail> financialDetailRepository)
            {
                _financialRepository = financialRepository;
                _financialDetailRepository = financialDetailRepository;
            }

            public async Task<ServiceResult> Handle(UpdateDescriptionsCommand request, CancellationToken cancellationToken)
            {

                var requests = await (from fr in _financialRepository.GetAll()
                                      join fd in _financialDetailRepository.GetAll() on fr.Id equals fd.FinancialRequestId
                                      where fr.Id < 20623
                                      select fr).ToListAsync();

                foreach (var item in requests)
                {
                    string tempDes = item.Description;
                    item.Description = "بابت دریافت شماره ردیف " + item.DocumentNo + " و شماره پیگیری  " + tempDes;
                }

                await _financialRepository.SaveChangesAsync();


                return ServiceResult.Success();

            }
        }
    }
}
