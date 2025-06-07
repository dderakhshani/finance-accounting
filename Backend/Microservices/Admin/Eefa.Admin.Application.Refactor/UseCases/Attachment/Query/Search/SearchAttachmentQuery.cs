using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class SearchAttachmentQuery : Specification<Attachment>, IRequest<ServiceResult<PaginatedList<AttachmentModel>>>
{
}

public class SearchAttachmentQueryHandler : IRequestHandler<SearchAttachmentQuery, ServiceResult<PaginatedList<AttachmentModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public SearchAttachmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AttachmentModel>>> Handle(SearchAttachmentQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Attachments
            .GetPaginatedProjectedListAsync<AttachmentModel>(request));
    }
}