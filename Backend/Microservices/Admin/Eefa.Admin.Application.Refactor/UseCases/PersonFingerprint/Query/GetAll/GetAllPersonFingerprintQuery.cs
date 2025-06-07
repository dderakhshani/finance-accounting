using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllPersonFingerprintQuery : Specification<PersonFingerprint>, IRequest<ServiceResult<PaginatedList<PersonFingerprintModel>>>
{
}

public class GetAllPersonFingerprintQueryHandler : IRequestHandler<GetAllPersonFingerprintQuery, ServiceResult<PaginatedList<PersonFingerprintModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonFingerprintQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<PersonFingerprintModel>>> Handle(GetAllPersonFingerprintQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonFingerprints
            .GetPaginatedProjectedListAsync<PersonFingerprintModel>(request));
    }
}