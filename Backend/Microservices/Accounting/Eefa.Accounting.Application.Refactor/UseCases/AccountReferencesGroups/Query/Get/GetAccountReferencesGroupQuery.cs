using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAccountReferencesGroupQuery : IRequest<ServiceResult<AccountReferencesGroupModel>>
{
    public int Id { get; set; }
}

public class GetAccountReferencesGroupQueryHandler : IRequestHandler<GetAccountReferencesGroupQuery, ServiceResult<AccountReferencesGroupModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAccountReferencesGroupQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AccountReferencesGroupModel>> Handle(GetAccountReferencesGroupQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AccountReferencesGroups
                            .GetProjectedByIdAsync<AccountReferencesGroupModel>(request));
    }
}