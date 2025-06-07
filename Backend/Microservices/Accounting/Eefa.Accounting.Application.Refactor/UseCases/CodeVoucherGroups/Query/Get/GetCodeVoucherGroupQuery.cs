using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetCodeVoucherGroupQuery : IRequest<ServiceResult<CodeVoucherGroupModel>>
{
    public int Id { get; set; }
}

public class GetCodeVoucherGroupQueryHandler : IRequestHandler<GetCodeVoucherGroupQuery, ServiceResult<CodeVoucherGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCodeVoucherGroupQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherGroupModel>> Handle(GetCodeVoucherGroupQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeVoucherGroups
                            .GetProjectedByIdAsync<CodeVoucherGroupModel>(request.Id));
    }
}