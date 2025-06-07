using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAccountReferenceQuery : IRequest<ServiceResult<AccountReferenceModel>>
{
    public int Id { get; set; }
}

public class GetAccountReferenceQueryHandler : IRequestHandler<GetAccountReferenceQuery, ServiceResult<AccountReferenceModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountReferenceQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<AccountReferenceModel>> Handle(GetAccountReferenceQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AccountReferences
                                    .GetProjectedByIdAsync<AccountReferenceModel>(request.Id));
    }
}