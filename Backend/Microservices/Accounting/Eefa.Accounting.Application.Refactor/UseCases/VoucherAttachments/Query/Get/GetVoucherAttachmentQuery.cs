using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetVoucherAttachmentQuery : IRequest<ServiceResult<VoucherAttachmentModel>>
{
    public int Id { get; set; }
}

public class GetVoucherAttachmentQueryHandler : IRequestHandler<GetVoucherAttachmentQuery, ServiceResult<VoucherAttachmentModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetVoucherAttachmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<VoucherAttachmentModel>> Handle(GetVoucherAttachmentQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.VoucherAttachments
                            .GetProjectedByIdAsync<VoucherAttachmentModel>(request.Id));
    }
}