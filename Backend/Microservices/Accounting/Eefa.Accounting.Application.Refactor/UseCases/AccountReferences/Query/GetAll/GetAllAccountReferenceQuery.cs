using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllAccountReferenceQuery : Specification<AccountReference>, IRequest<ServiceResult<PaginatedList<AccountReferenceModel>>>
{
}

public class GetAllAccountReferenceQueryHandler : IRequestHandler<GetAllAccountReferenceQuery, ServiceResult<PaginatedList<AccountReferenceModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllAccountReferenceQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AccountReferenceModel>>> Handle(GetAllAccountReferenceQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AccountReferences
                                    .GetPaginatedProjectedListAsync<AccountReferenceModel>(request));
    }
}