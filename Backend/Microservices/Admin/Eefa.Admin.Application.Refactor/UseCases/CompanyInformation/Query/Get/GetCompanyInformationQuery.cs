using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetCompanyInformationQuery : IRequest<ServiceResult<CompanyInformationModel>>
{
    public int Id { get; set; }
}

public class GetCompanyInformationQueryHandler : IRequestHandler<GetCompanyInformationQuery, ServiceResult<CompanyInformationModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetCompanyInformationQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CompanyInformationModel>> Handle(GetCompanyInformationQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CompanyInformations
                            .GetProjectedByIdAsync<CompanyInformationModel>(request.Id));
    }
}