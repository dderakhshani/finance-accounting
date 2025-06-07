using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateAutoVoucherFormulaCommand : IRequest<ServiceResult<AutoVoucherFormulaModel>>, IMapFrom<AutoVoucherFormula>
{
    public int Id { get; set; }
    public int VoucherTypeId { get; set; } = default!;
    public int SourceVoucherTypeId { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public byte DebitCreditStatus { get; set; } = default!;
    public int AccountHeadId { get; set; } = default!;
    public string? RowDescription { get; set; }
    public string? Formula { get; set; }
    public string? Conditions { get; set; }
    public string? GroupBy { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateAutoVoucherFormulaCommand, AutoVoucherFormula>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateAutoVoucherFormulaCommandHandler : IRequestHandler<UpdateAutoVoucherFormulaCommand, ServiceResult<AutoVoucherFormulaModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateAutoVoucherFormulaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AutoVoucherFormulaModel>> Handle(UpdateAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
    {
        AutoVoucherFormula entity = await _unitOfWork.AutoVoucherFormulas.GetByIdAsync(request.Id);

        _mapper.Map(entity, request);

        _unitOfWork.AutoVoucherFormulas.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AutoVoucherFormulaModel>(entity));
    }
}