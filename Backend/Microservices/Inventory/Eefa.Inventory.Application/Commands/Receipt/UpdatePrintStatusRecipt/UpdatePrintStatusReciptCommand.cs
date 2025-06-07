using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using MediatR;
using Eefa.Inventory.Domain;

namespace Eefa.Inventory.Application
{
    public class UpdatePrintStatusReceiptCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
    }

    public class UpdatePrintStatusReceiptCommandHandler : IRequestHandler<UpdatePrintStatusReceiptCommand, ServiceResult>
    {
        private readonly IRepository<Receipt> _receiptRepository;
        
        private readonly IMapper _mapper;

        public UpdatePrintStatusReceiptCommandHandler(
            IRepository<Receipt> receiptRepository,
            
            IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
           
        }

        public async Task<ServiceResult> Handle(UpdatePrintStatusReceiptCommand request, CancellationToken cancellationToken)
        {
            Receipt receipt = await _receiptRepository.Find(request.Id);
            
            receipt.DocumentStateBaseId = request.StatusId;

            var entity = _receiptRepository.Update(receipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
              
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
        
    }
}
