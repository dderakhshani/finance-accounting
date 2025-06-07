using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAttachmentQuery : IRequest<ServiceResult<AttachmentModel>>
{
    public int Id { get; set; }
}

public class GetAttachmentQueryHandler : IRequestHandler<GetAttachmentQuery, ServiceResult<AttachmentModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAttachmentQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<AttachmentModel>> Handle(GetAttachmentQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Attachments
                        .GetProjectedByIdAsync<AttachmentModel>(request.Id));
    }
}