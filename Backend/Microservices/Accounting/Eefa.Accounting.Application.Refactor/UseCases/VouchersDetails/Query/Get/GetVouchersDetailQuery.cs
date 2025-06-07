using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetVouchersDetailQuery : IRequest<ServiceResult<VouchersDetailModel>>
{
    public int Id { get; set; }
}

public class GetVouchersDetailQueryHandler : IRequestHandler<GetVouchersDetailQuery, ServiceResult<VouchersDetailModel>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetVouchersDetailQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<VouchersDetailModel>> Handle(GetVouchersDetailQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.VouchersDetails
                            .GetProjectedByIdAsync<VouchersDetailModel>(request.Id));
    }
}