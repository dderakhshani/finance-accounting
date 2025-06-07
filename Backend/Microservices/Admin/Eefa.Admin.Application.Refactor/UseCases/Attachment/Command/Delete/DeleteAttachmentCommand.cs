using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class DeleteAttachmentCommand : IRequest<ServiceResult<int>>
{
    public int Id { get; set; }
}

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ServiceResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAttachmentCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<int>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.Attachments.GetByIdAsync(request.Id);

        _unitOfWork.Attachments.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
     
        return ServiceResult.Success(entity.Id);
    }
}