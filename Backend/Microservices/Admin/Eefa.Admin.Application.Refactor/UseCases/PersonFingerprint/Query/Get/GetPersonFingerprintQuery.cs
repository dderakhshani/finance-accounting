using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetPersonFingerprintQuery : IRequest<ServiceResult<PersonFingerprintModel>>
{
    public int Id { get; set; }
}

public class GetPersonFingerprintQueryHandler : IRequestHandler<GetPersonFingerprintQuery, ServiceResult<PersonFingerprintModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetPersonFingerprintQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PersonFingerprintModel>> Handle(GetPersonFingerprintQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonFingerprints
                            .GetProjectedByIdAsync<PersonFingerprintModel>(request.Id));
    }
}