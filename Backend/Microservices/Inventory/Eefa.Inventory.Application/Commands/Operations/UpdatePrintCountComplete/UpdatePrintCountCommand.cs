using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Eefa.Inventory.Domain;
using System;

namespace Eefa.Inventory.Application
{
    public class UpdatePrintCountCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        
    }

    public class UpdatePrintCountCommandHandler : IRequestHandler<UpdatePrintCountCommand, ServiceResult>
    {
        private readonly IRepository<Receipt> _receiptRepository;
        
        private readonly IMapper _mapper;

        public UpdatePrintCountCommandHandler(
            IRepository<Receipt> receiptRepository,
            
            IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
           
        }

        public async Task<ServiceResult> Handle(UpdatePrintCountCommand request, CancellationToken cancellationToken)
        {
            Receipt receipt = await _receiptRepository.Find(request.Id);

            receipt.PrintNumber = (Convert.ToInt32(receipt.PrintNumber) + 1).ToString();

            var entity = _receiptRepository.Update(receipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
              
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
        
    }
}
