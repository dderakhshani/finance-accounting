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
    public class UpdateIsPlacementCompleteCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
        public bool IsPlacementComplete { get; set; }
    }

    public class UpdateIsPlacementCompleteCommandHandler : IRequestHandler<UpdateIsPlacementCompleteCommand, ServiceResult>
    {
        private readonly IRepository<Receipt> _receiptRepository;
        
        private readonly IMapper _mapper;

        public UpdateIsPlacementCompleteCommandHandler(
            IRepository<Receipt> receiptRepository,
            
            IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
           
        }

        public async Task<ServiceResult> Handle(UpdateIsPlacementCompleteCommand request, CancellationToken cancellationToken)
        {
            Receipt receipt = await _receiptRepository.Find(request.Id);
            
            receipt.IsPlacementComplete = request.IsPlacementComplete;

            var entity = _receiptRepository.Update(receipt);
            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
              
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
        
    }
}
