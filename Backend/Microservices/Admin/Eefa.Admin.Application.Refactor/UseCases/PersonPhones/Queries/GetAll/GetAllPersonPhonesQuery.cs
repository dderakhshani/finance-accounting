using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllPersonPhonesQuery : Specification<PersonPhone>, IRequest<ServiceResult<PaginatedList<PersonPhoneModel>>>
{
}

public class GetAllPersonPhonesQueryHandler : IRequestHandler<GetAllPersonPhonesQuery, ServiceResult<PaginatedList<PersonPhoneModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonPhonesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResult<PaginatedList<PersonPhoneModel>>> Handle(GetAllPersonPhonesQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonPhones
            .GetPaginatedProjectedListAsync<PersonPhoneModel>(request));
    }
}