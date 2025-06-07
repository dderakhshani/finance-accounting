using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllPersonAddressQuery : Specification<PersonAddress>, IRequest<ServiceResult<PaginatedList<PersonAddressModel>>>
{
    public int PersonId { get; set; }
}

public class GetAllPersonAddressQueryHandler : IRequestHandler<GetAllPersonAddressQuery, ServiceResult<PaginatedList<PersonAddressModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllPersonAddressQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<PersonAddressModel>>> Handle(GetAllPersonAddressQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.PersonsAddress
                            .GetPaginatedProjectedListAsync<PersonAddressModel>(request));
    }
}