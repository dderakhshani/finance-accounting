using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class DeleteBranchCommand : IRequest<ServiceResult<int>>
{
    public int Id { get; set; }
}

public class DeleteBranchCommandHandler : IRequestHandler<DeleteBranchCommand, ServiceResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    public DeleteBranchCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<int>> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        Branch entity = await _unitOfWork.Branchs.GetByIdAsync(request.Id);

        _unitOfWork.Branchs.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(entity.Id);
    }
}