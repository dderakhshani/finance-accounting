using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class GetAllAutoVoucherFormulaQuery : Specification<AutoVoucherFormula>, IRequest<ServiceResult<PaginatedList<AutoVoucherFormulaModel>>>
{
}

public class GetAllAutoVoucherFormulaQueryHandler : IRequestHandler<GetAllAutoVoucherFormulaQuery, ServiceResult<PaginatedList<AutoVoucherFormulaModel>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllAutoVoucherFormulaQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<PaginatedList<AutoVoucherFormulaModel>>> Handle(GetAllAutoVoucherFormulaQuery request, CancellationToken cancellationToken)
    {
        return ServiceResult.Success(await _unitOfWork.AutoVoucherFormulas
                            .GetPaginatedProjectedListAsync<AutoVoucherFormulaModel>(request));
    }
}