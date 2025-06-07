using System;
using System.Linq;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

public class CreateVouchersDetailCommand : /*ICloneable,*/ IRequest<ServiceResult<VouchersDetailModel>>
    , IMapFrom<CreateVouchersDetailCommand>
{
    [SwaggerExclude]
    public VouchersHead EntityEntryVouchersHead { get; set; }
    public DateTime? VoucherDate { get; set; }
    public int? VoucherId { get; set; }
    public int AccountHeadId { get; set; } = default!;
    public int? AccountReferencesGroupId { get; set; } = default!;
    public string VoucherRowDescription { get; set; } = default!;
    public double Debit { get; set; } = default!;
    public double Credit { get; set; } = default!;
    public int? RowIndex { get; set; }
    public int? DocumentId { get; set; }
    public DateTime? ReferenceDate { get; set; }
    public double? ReferenceQty { get; set; }
    public int? ReferenceId1 { get; set; }
    public int? ReferenceId2 { get; set; }
    public int? ReferenceId3 { get; set; }
    public int? Level1 { get; set; }
    public int? Level2 { get; set; }
    public int? Level3 { get; set; }
    //public int? CurrencyBaseTypeId { get; set; }
    //public int? CurrencyValue { get; set; }
    //public int? ExchengeValue { get; set; }
    public int? CurrencyTypeBaseId { get; set; }
    public double? CurrencyFee { get; set; }
    public double? CurrencyAmount { get; set; }
    public int? TraceNumber { get; set; }
    public double? Quantity { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateVouchersDetailCommand, VouchersDetail>()
            .IgnoreAllNonExisting();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class CreateVouchersDetailCommandHandler : IRequestHandler<CreateVouchersDetailCommand, ServiceResult<VouchersDetailModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateVouchersDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResult<VouchersDetailModel>> Handle(CreateVouchersDetailCommand request, CancellationToken cancellationToken)
    {
        if (request.ReferenceId1 is not (null or 0) ||
            request.ReferenceId2 is not (null or 0) ||
            request.ReferenceId3 is not (null or 0))
        {
            var referenceId = 0;
            if (request.ReferenceId1 is not (null or 0))
            {
                referenceId = request.ReferenceId1 ?? 0;
            }
            if (request.ReferenceId2 is not (null or 0))
            {
                referenceId = request.ReferenceId2 ?? 0;
            }
            if (request.ReferenceId3 is not (null or 0))
            {
                referenceId = request.ReferenceId3 ?? 0;
            }

            var accountReferenceGroups = await _unitOfWork
                .AccountReferencesRelReferencesGroups.GetProjectedListAsync(x => x.ReferenceId == referenceId, y => y.ReferenceGroupId);

            AccountHeadRelReferenceGroup accountHeadRelReferenceGroup = await _unitOfWork
                .AccountHeadRelReferenceGroups.GetAsync(x => x.AccountHeadId == request.AccountHeadId &&
                accountReferenceGroups.Contains(x.ReferenceGroupId));

            if (accountHeadRelReferenceGroup.ReferenceNo == 0)
            {
                throw new Exception("");
            }
            else
            {
                switch (accountHeadRelReferenceGroup.ReferenceNo)
                {
                    case 1:
                        if (request.ReferenceId1 is (null or 0))
                        {
                            throw new Exception("ReferenceId1");
                        }

                        break;
                    case 2:
                        if (request.ReferenceId2 is (null or 0) &&
                            (request.ReferenceId1 is (null or 0)))
                        {
                            throw new Exception("ReferenceId2");
                        }

                        break;
                    case 3:
                        if (request.ReferenceId3 is (null or 0) &&
                            (request.ReferenceId1 is (null or 0) &&
                             (request.ReferenceId2 is (null or 0))))
                        {
                            throw new Exception("ReferenceId3");
                        }

                        break;
                }
            }
        }

        var details = _mapper.Map<VouchersDetail>(request);
        if (request.VoucherId != 0 && request.EntityEntryVouchersHead != null)
        {
            details.Voucher = request.EntityEntryVouchersHead;
        }

        _unitOfWork.VouchersDetails.Add(details);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ServiceResult.Success(_mapper.Map<VouchersDetailModel>(details));
    }
}