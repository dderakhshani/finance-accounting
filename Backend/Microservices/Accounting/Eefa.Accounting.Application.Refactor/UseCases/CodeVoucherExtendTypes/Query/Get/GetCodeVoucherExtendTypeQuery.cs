using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class GetCodeVoucherExtendTypeQuery : IRequest<ServiceResult<CodeVoucherExtendTypeModel>>
{
    public int Id { get; set; }
}

public class GetCodeVoucherExtendTypeQueryHandler : IRequestHandler<GetCodeVoucherExtendTypeQuery, ServiceResult<CodeVoucherExtendTypeModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetCodeVoucherExtendTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<CodeVoucherExtendTypeModel>> Handle(GetCodeVoucherExtendTypeQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.CodeVoucherExtendTypes
                            .GetProjectedByIdAsync<CodeVoucherExtendTypeModel>(request));
    }
}