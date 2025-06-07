using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllVoucherAttachmentByVoucherHeadIdQuery : Specification<VoucherAttachment>, IRequest<ServiceResult<PaginatedList<VoucherAttachmentModel>>>
{
    public int VoucherHeadId { get; set; }
}

public class GetAllVoucherAttachmentQueryHandler : IRequestHandler<GetAllVoucherAttachmentByVoucherHeadIdQuery, ServiceResult<PaginatedList<VoucherAttachmentModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllVoucherAttachmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<VoucherAttachmentModel>>> Handle(GetAllVoucherAttachmentByVoucherHeadIdQuery request, CancellationToken cancellationToken)
    {
        request.ApplicationConditions.Add(x => x.VoucherHeadId == request.VoucherHeadId);
        return ServiceResult.Success(await _unitOfWork.VoucherAttachments
               .GetPaginatedProjectedListAsync<VoucherAttachmentModel>(request));
    }
}