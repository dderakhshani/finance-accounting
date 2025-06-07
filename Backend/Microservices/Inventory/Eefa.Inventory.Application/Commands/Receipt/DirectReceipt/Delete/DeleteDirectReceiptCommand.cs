using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class DeleteDirectReceiptCommand : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteDirectReceiptCommandHandler : IRequestHandler<DeleteDirectReceiptCommand, ServiceResult>
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IMapper _mapper;

        public DeleteDirectReceiptCommandHandler(IReceiptRepository receiptRepository, IMapper mapper)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
        }

        public async Task<ServiceResult> Handle(DeleteDirectReceiptCommand request, CancellationToken cancellationToken)
        {
            var entity = await _receiptRepository.Find(request.Id);

            var deletedEntity = _receiptRepository.Delete(entity);
            if (await _receiptRepository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Success();
            }
            return ServiceResult.Failed();
        }
    }
}
