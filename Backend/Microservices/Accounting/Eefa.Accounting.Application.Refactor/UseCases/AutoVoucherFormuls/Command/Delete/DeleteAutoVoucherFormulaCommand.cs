using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class DeleteAutoVoucherFormulaCommand : IRequest<ServiceResult<AutoVoucherFormulaModel>>
{
    public int Id { get; set; }
}

public class DeleteAutoVoucherFormulaCommandHandler : IRequestHandler<DeleteAutoVoucherFormulaCommand, ServiceResult<AutoVoucherFormulaModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteAutoVoucherFormulaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AutoVoucherFormulaModel>> Handle(DeleteAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
    {
        AutoVoucherFormula entity = await _unitOfWork.AutoVoucherFormulas.GetByIdAsync(request.Id);

        _unitOfWork.AutoVoucherFormulas.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AutoVoucherFormulaModel>(entity));
    }
}