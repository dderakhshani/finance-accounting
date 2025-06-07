using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllVouchersDetailQuery : Specification<VouchersDetail>, IRequest<ServiceResult<PaginatedList<VouchersDetailModel>>>
{
}

public class GetAllVouchersDetailQueryHandler : IRequestHandler<GetAllVouchersDetailQuery, ServiceResult<PaginatedList<VouchersDetailModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAllVouchersDetailQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<VouchersDetailModel>>> Handle(GetAllVouchersDetailQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.VouchersDetails
                            .GetPaginatedProjectedListAsync<VouchersDetailModel>(request));
    }
}