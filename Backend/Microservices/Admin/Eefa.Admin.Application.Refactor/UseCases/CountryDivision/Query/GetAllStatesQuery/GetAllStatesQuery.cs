using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllStatesQuery : Specification<CountryDivision>, IRequest<ServiceResult<PaginatedList<CountryDivisionModel>>>
{
}

public class GetAllStatesQueryHandler : IRequestHandler<GetAllStatesQuery, ServiceResult<PaginatedList<CountryDivisionModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllStatesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CountryDivisionModel>>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CountryDivisions
            .GetPaginatedProjectedListAsync<CountryDivisionModel>(request));

        //return ServiceResult.Success(await _repository
        //.GetAll<CountryDivision>(c =>
        //    c.Paginate(new Pagination()
        //     {
        //         PageIndex = request.PageIndex,
        //         PageSize = request.PageSize,


        //     })).Select(x => new CountryDivisionModel(){Ostan = x.Ostan, OstanTitle = x.OstanTitle }).Distinct()
        ////.ProjectTo<CountryDivisionModel>(_mapper.ConfigurationProvider)
        //.ToListAsync(cancellationToken));
    }
}