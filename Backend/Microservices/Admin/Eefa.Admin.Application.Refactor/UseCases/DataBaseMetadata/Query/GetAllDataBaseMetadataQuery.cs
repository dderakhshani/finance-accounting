using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllDataBaseMetadataQuery : Specification<DataBaseMetadata>, IRequest<ServiceResult<PaginatedList<DataBaseMetadataModel>>>
{
}

public class GetAllDataBaseMetadataQueryHandler : IRequestHandler<GetAllDataBaseMetadataQuery, ServiceResult<PaginatedList<DataBaseMetadataModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllDataBaseMetadataQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<DataBaseMetadataModel>>> Handle(GetAllDataBaseMetadataQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.DataBaseMetadatas
                            .GetPaginatedProjectedListAsync<DataBaseMetadataModel>(request));
    }
}