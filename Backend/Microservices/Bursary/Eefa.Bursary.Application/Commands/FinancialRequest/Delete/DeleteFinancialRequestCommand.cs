using AutoMapper;
using Eefa.Bursary.Domain.Aggregates.FinancialRequestAggregate;
using Eefa.Common.CommandQuery;
 
 
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Eefa.Common;

namespace Eefa.Bursary.Application.Commands.FinancialRequest.Delete
{
   public class DeleteFinancialRequestCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }


    public class DeleteFinancialRequestCommandHandler : IRequestHandler<DeleteFinancialRequestCommand, ServiceResult>
    {

        private readonly IFinancialRequestRepository _financialRequestRepository;
        private readonly IMapper _mapper;

        public DeleteFinancialRequestCommandHandler(IFinancialRequestRepository financialRequestRepository, IMapper mapper)
        {
            _financialRequestRepository = financialRequestRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResult> Handle(DeleteFinancialRequestCommand request, CancellationToken cancellationToken)
        {
            var cheque = await _financialRequestRepository.Find(request.Id);

            var deletedEntity = _financialRequestRepository.Delete(cheque);
            if (await _financialRequestRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();

        }
    }

}
