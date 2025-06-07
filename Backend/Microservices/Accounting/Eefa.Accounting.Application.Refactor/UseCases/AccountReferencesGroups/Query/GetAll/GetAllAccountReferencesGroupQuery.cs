using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllAccountReferencesGroupQuery : Specification<AccountReferencesGroup>, IRequest<ServiceResult<PaginatedList<AccountReferencesGroupModel>>>
{
}

public class GetAllAccountReferencesGroupQueryHandler : IRequestHandler<GetAllAccountReferencesGroupQuery, ServiceResult<PaginatedList<AccountReferencesGroupModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllAccountReferencesGroupQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AccountReferencesGroupModel>>> Handle(GetAllAccountReferencesGroupQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AccountReferencesGroups
                            .GetPaginatedProjectedListAsync<AccountReferencesGroupModel>(request));
    }
}