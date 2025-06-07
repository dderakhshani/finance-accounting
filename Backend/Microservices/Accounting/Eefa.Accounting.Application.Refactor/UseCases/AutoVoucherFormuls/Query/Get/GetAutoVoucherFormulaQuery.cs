using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

public class GetAutoVoucherFormulaQuery : IRequest<ServiceResult<AutoVoucherFormulaModel>>
{
    public int Id { get; set; }
}

public class GetAutoVoucherFormulaQueryHandler : IRequestHandler<GetAutoVoucherFormulaQuery, ServiceResult<AutoVoucherFormulaModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetAutoVoucherFormulaQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AutoVoucherFormulaModel>> Handle(GetAutoVoucherFormulaQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AutoVoucherFormulas
            .GetProjectedByIdAsync<AutoVoucherFormulaModel>(request.Id));
    }
}