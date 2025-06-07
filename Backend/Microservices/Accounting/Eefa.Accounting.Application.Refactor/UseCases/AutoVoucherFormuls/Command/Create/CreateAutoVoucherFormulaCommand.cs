using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateAutoVoucherFormulaCommand : IRequest<ServiceResult<AutoVoucherFormulaModel>>, IMapFrom<CreateAutoVoucherFormulaCommand>
{
    public int VoucherTypeId { get; set; } = default!;
    public int SourceVoucherTypeId { get; set; } = default!;
    public int OrderIndex { get; set; } = default!;
    public byte DebitCreditStatus { get; set; } = default!;
    public int AccountHeadId { get; set; } = default!;
    public string? RowDescription { get; set; }
    //public FormulasModel.FormulaCondition FormulaConditions { get; set; }
    public string? GroupBy { get; set; }
    //public ICollection<FormulasModel.Formula> Formulas { get; set; }

    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateAutoVoucherFormulaCommand, AutoVoucherFormula>()
            //.ForMember(x => x.Formula, opt => opt.MapFrom(x => JsonConvert.SerializeObject(x.Formulas)))
            //.ForMember(x => x.Conditions, opt => opt.MapFrom(x => JsonConvert.SerializeObject(x.FormulaConditions)))
            .IgnoreAllNonExisting();
    }
}

public class CreateAutoVoucherFormulaCommandHandler : IRequestHandler<CreateAutoVoucherFormulaCommand, ServiceResult<AutoVoucherFormulaModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAutoVoucherFormulaCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork= unitOfWork;
    }

    public async Task<ServiceResult<AutoVoucherFormulaModel>> Handle(CreateAutoVoucherFormulaCommand request, CancellationToken cancellationToken)
    {
        AutoVoucherFormula entity = _mapper.Map<AutoVoucherFormula>(request);

        _unitOfWork.AutoVoucherFormulas.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<AutoVoucherFormulaModel>(entity));
    }
}