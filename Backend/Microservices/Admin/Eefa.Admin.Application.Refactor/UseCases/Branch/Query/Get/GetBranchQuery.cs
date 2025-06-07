using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetBranchQuery : IRequest<ServiceResult<BranchModel>>
{
    public int Id { get; set; }
}

public class GetBranchQueryHandler : IRequestHandler<GetBranchQuery, ServiceResult<BranchModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetBranchQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BranchModel>> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.Branchs
                            .GetProjectedByIdAsync<BranchModel>(request.Id));
    }
}