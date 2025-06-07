using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetAllCodeVoucherExtendTypeQuery : Specification<CodeVoucherExtendType>, IRequest<ServiceResult<PaginatedList<CodeVoucherExtendTypeModel>>>
{
}

public class GetAllCodeVoucherExtendTypeQueryHandler : IRequestHandler<GetAllCodeVoucherExtendTypeQuery, ServiceResult<PaginatedList<CodeVoucherExtendTypeModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllCodeVoucherExtendTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<CodeVoucherExtendTypeModel>>> Handle(GetAllCodeVoucherExtendTypeQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeVoucherExtendTypes
                            .GetPaginatedProjectedListAsync<CodeVoucherExtendTypeModel>(request));
    }
}