using System;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class UpdateVouchersDetailCommand : IRequest<ServiceResult<VouchersDetailModel>>, IMapFrom<VouchersDetail>
{
    public int Id { get; set; }
    public int VoucherId { get; set; } = default!;
    public DateTime VoucherDate { get; set; }
    public int AccountHeadId { get; set; } = default!;
    public string VoucherRowDescription { get; set; } = default!;
    public double Debit { get; set; } = default!;
    public double Credit { get; set; } = default!;
    public int RowIndex { get; set; }
    public int? DocumentId { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public double? ReferenceQty { get; set; }
    public int? AccountReferencesGroupId { get; set; } = default!;
    public int? ReferenceId1 { get; set; }
    public int? ReferenceId2 { get; set; }
    public int? ReferenceId3 { get; set; }
    public int? Level1 { get; set; }
    public int? Level2 { get; set; }
    public int? Level3 { get; set; }
    public int? CurrencyTypeBaseId { get; set; }
    public double? CurrencyFee { get; set; }
    public double? CurrencyAmount { get; set; }
    public int TraceNumber { get; set; }
    public double? Quantity { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateVouchersDetailCommand, VouchersDetail>()
            .IgnoreAllNonExisting();
    }
}

public class UpdateVouchersDetailCommandHandler : IRequestHandler<UpdateVouchersDetailCommand, ServiceResult<VouchersDetailModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVouchersDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VouchersDetailModel>> Handle(UpdateVouchersDetailCommand request, CancellationToken cancellationToken)
    {
        VouchersDetail entity = await _unitOfWork.VouchersDetails.GetByIdAsync(request.Id);

        _mapper.Map(entity, request);

        _unitOfWork.VouchersDetails.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VouchersDetailModel>(entity));
    }
}