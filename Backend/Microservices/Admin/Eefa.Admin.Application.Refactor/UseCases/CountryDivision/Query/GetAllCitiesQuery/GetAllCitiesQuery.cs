using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllCitiesQuery : Specification<CountryDivision>, IRequest<ServiceResult<PaginatedList<CountryDivisionModel>>>
{
    public string StateCode { get; set; }
}

public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, ServiceResult<PaginatedList<CountryDivisionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllCitiesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CountryDivisionModel>>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        if (request.StateCode == default)
        {
            return ServiceResult.Success(await _unitOfWork.CountryDivisions
                                .GetPaginatedProjectedListAsync<CountryDivisionModel>(request));
        }
        else
        {
            request.ApplicationConditions.Add(x => x.Ostan.Equals(request.StateCode));

            return ServiceResult.Success(await _unitOfWork.CountryDivisions
                                .GetPaginatedProjectedListAsync<CountryDivisionModel>(request));
        }
    }
}