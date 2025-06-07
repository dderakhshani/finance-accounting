using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class DeleteBaseValueCommand : IRequest<ServiceResult<int>>
{
    public int Id { get; set; }
}

public class DeleteBaseValueCommandHandler : IRequestHandler<DeleteBaseValueCommand, ServiceResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteBaseValueCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<int>> Handle(DeleteBaseValueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.BaseValues.GetByIdAsync(request.Id);

        _unitOfWork.BaseValues.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(entity.Id);
    }
}