using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Eefa.Common;
using Eefa.Common.CommandQuery;
using Eefa.Common.Data;
using Eefa.Inventory.Domain;
using MediatR;

namespace Eefa.Inventory.Application
{
    public class UpdateDirectReceiptCommand : CommandBase, IRequest<ServiceResult<ReceiptModel>>, IMapFrom<Domain.Receipt>, ICommand
    {
        public int Id { get; set; }
        public int CodeVoucherGroupId { get; set; } = default!;


        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateDirectReceiptCommand, Domain.Receipt>()
                .IgnoreAllNonExisting();
        }

    }


    public class UpdateDirectReceiptCommandHandler : IRequestHandler<UpdateDirectReceiptCommand, ServiceResult<ReceiptModel>>
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<DocumentItem> _DocumentItem;
        

        public UpdateDirectReceiptCommandHandler(IReceiptRepository receiptRepository, IMapper mapper, IRepository<DocumentItem> DocumentItem, IRepository<MeasureUnitConversion> measureUnitConversion)
        {
            _mapper = mapper;
            _receiptRepository = receiptRepository;
            _DocumentItem = DocumentItem;
           
        }

        public async Task<ServiceResult<ReceiptModel>> Handle(UpdateDirectReceiptCommand request, CancellationToken cancellationToken)
        {
            Domain.Receipt receipt;

            receipt = _mapper.Map<Domain.Receipt>(request);
          
            _receiptRepository.Update(receipt);

            if (await _receiptRepository.SaveChangesAsync() > 0)
            {
                var model = _mapper.Map<ReceiptModel>(receipt);
                return ServiceResult<ReceiptModel>.Success(model);
            }
            else
            {
                return ServiceResult<ReceiptModel>.Failed();
            }


        }
    }
}
