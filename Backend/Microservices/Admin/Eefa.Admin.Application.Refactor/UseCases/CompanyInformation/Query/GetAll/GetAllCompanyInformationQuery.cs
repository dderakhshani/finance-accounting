using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllowedUserCompanyByUserIdQuery : Specification<CompanyInformation>, IRequest<ServiceResult<PaginatedList<CompanyInformationModel>>>
{
}

public class GetAllCompanyInformationQueryHandler : IRequestHandler<GetAllowedUserCompanyByUserIdQuery, ServiceResult<PaginatedList<CompanyInformationModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllCompanyInformationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CompanyInformationModel>>> Handle(GetAllowedUserCompanyByUserIdQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CompanyInformations
                            .GetPaginatedProjectedListAsync<CompanyInformationModel>(request));
    }
}