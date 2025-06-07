using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetBaseValueTypeQuery : IRequest<ServiceResult<BaseValueTypeModel>>
{
    public int Id { get; set; }
}

public class GetBaseValueTypeQueryHandler : IRequestHandler<GetBaseValueTypeQuery, ServiceResult<BaseValueTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetBaseValueTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<BaseValueTypeModel>> Handle(GetBaseValueTypeQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.BaseValueTyps
                                     .GetProjectedByIdAsync<BaseValueTypeModel>(request.Id));
    }
}