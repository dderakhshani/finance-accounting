using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllAttachmentQuery : Specification<Attachment>, IRequest<ServiceResult<PaginatedList<AttachmentModel>>>
{
}

public class GetAllAttachmentQueryHandler : IRequestHandler<GetAllAttachmentQuery, ServiceResult<PaginatedList<AttachmentModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllAttachmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AttachmentModel>>> Handle(GetAllAttachmentQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Attachments
                .GetPaginatedProjectedListAsync<AttachmentModel>(request));
    }
}