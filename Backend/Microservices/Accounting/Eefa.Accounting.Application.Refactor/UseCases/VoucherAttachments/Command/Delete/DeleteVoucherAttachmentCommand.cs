using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;

public class DeleteVoucherAttachmentCommand : IRequest<ServiceResult<VoucherAttachmentModel>>
{
    public int Id { get; set; }
}

public class DeleteVoucherAttachmentCommandHandler : IRequestHandler<DeleteVoucherAttachmentCommand, ServiceResult<VoucherAttachmentModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteVoucherAttachmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VoucherAttachmentModel>> Handle(DeleteVoucherAttachmentCommand request, CancellationToken cancellationToken)
    {
        VoucherAttachment entity = await _unitOfWork.VoucherAttachments.GetByIdAsync(request.Id);

        _unitOfWork.VoucherAttachments.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VoucherAttachmentModel>(entity));
    }
}